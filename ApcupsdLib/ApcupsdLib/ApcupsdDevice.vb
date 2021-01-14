Imports System.Net.Sockets
Imports System.Threading
Imports System.Text

Public Class ApcupsdDevice
    Private Shared _hostname As String
    Private Shared _apcStatus As Dictionary(Of String, String) = New Dictionary(Of String, String)

    Public Property hostname()
        Get
            Return _hostname
        End Get
        Set(ByVal value)
            _hostname = value
        End Set
    End Property

    Private Shared Function checkHostname(ByVal endpoint As String) As Boolean
        'Return false if hostname not valid
        Dim hostType As UriHostNameType = Uri.CheckHostName(endpoint)
        If hostType <> UriHostNameType.Unknown And endpoint <> "" Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Shared ReadOnly GetStatusMessage As Byte() = New Byte() {&H0, &H6, &H73, &H74, &H61, &H74, &H75, &H73}
    Private Shared ReadOnly GetEventsMessage As Byte() = New Byte() {&H0, &H6, &H65, &H76, &H65, &H6E, &H74, &H73}
    Private Shared ReadOnly EndBytes As Byte() = New Byte() {&H0, &H0}

    Private Function ExecuteClientAction(ByVal sendMessage As Byte()) As String()
        Dim ret = New List(Of String)()

        If checkHostname(_hostname) = False Then    'invalid hostname
            ret.Add("Invalid hostname" & vbNewLine)
            Return ret.ToArray()
        End If

        Try
            Using client As TcpClient = New TcpClient()
                client.Connect(_hostname, 3551)

                Using stream As NetworkStream = client.GetStream()
                    stream.Write(sendMessage, 0, sendMessage.Length)
                    Thread.Sleep(20)
                    Dim start = New Byte(1) {}
                    stream.Read(start, 0, 2)

                    While Not start.SequenceEqual(EndBytes)
                        Dim line = New Byte(start(1) - 1) {}
                        stream.Read(line, 0, start(1))
                        Dim lineStr = Encoding.ASCII.GetString(line)
                        ret.Add(lineStr)
                        stream.Read(start, 0, 2)
                    End While

                    While Not start.SequenceEqual(EndBytes)
                    End While
                End Using
            End Using
        Catch
            ret.Add("Failed to reach APCUPSD server" & vbNewLine)
            Return ret.ToArray()
        End Try

        Try
            LoadApcStatus(ret.ToArray())
        Catch ex As Exception
        End Try

        Return ret.ToArray()
    End Function

    Public Function GetApcStatus()
        Dim arr = Me.ExecuteClientAction(GetStatusMessage)
        Return arr
    End Function

    Public Function GetApcEvents()
        Dim arr = Me.ExecuteClientAction(GetEventsMessage)
        Return arr
    End Function

    Private Shared Sub LoadApcStatus(ByVal rawList As String())
        For Each line In rawList
            Dim keyEndInd As Integer = line.IndexOf(":")
            Dim propertyName As String = line.Substring(0, keyEndInd).Trim()
            Dim valueStartInd As Integer = line.IndexOf(":") + 1
            Dim valueName As String = line.Substring(valueStartInd).Trim()

            _apcStatus.Add(propertyName, valueName)
        Next
    End Sub

    Public Function GetApcValue(ByVal key As String) As String
        ' Get value with TryGetValue.
        Dim result As String = Nothing
        If _apcStatus.TryGetValue(key, result) Then
            Return result
        Else
            If _apcStatus.Count = 0 Then
                Return "Must run GetApcStatus to get info first"
            Else
                Return "N/A"
            End If
        End If
    End Function
End Class
