Imports System.Net.Sockets
Imports System.Threading
Imports System.Text

Public Class Form1

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub TextBox1_GotFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.GotFocus
        If TextBox1.Text = "IP or Hostname" Then
            TextBox1.Text = ""
        End If
    End Sub

    Private Sub TextBox1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles TextBox1.LostFocus
        If TextBox1.Text = "" Then
            TextBox1.Text = "IP or Hostname"
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        If TextBox1.Text = "" Or TextBox1.Text = "IP or Hostname" Then
            MsgBox("Please enter a valid IP or Hostname running APCUPSD")
        Else
            'Fetch Status Info
            For Each line As String In GetStatus()
                RichTextBox1.AppendText(line)
            Next
            'Fetch Event Info
            For Each line As String In GetEvents()
                RichTextBox2.AppendText(line)
            Next
        End If
    End Sub

    Private Shared ReadOnly GetStatusMessage As Byte() = New Byte() {&H0, &H6, &H73, &H74, &H61, &H74, &H75, &H73}
    Private Shared ReadOnly GetEventsMessage As Byte() = New Byte() {&H0, &H6, &H65, &H76, &H65, &H6E, &H74, &H73}
    Private Shared ReadOnly EndBytes As Byte() = New Byte() {&H0, &H0}

    Private Function ExecuteClientAction(ByVal sendMessage As Byte()) As String()
        Dim ret = New List(Of String)()
        Try
            Using client As TcpClient = New TcpClient()
                client.Connect(TextBox1.Text, 3551)

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
            MsgBox("Failed to reach APCUPSD server")
        End Try

        Return ret.ToArray()
    End Function

    Public Function GetStatus()
        Dim arr = Me.ExecuteClientAction(GetStatusMessage)
        Return arr
    End Function

    Public Function GetEvents()
        Dim arr = Me.ExecuteClientAction(GetEventsMessage)
        Return arr
    End Function

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        RichTextBox1.Clear()
        RichTextBox2.Clear()
    End Sub
End Class
