Imports ApcupsdLib

Public Class Form1
    Dim client As New ApcupsdDevice

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

    Private Sub TextBox1_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles TextBox1.KeyDown
        If e.KeyCode = Keys.Enter Then
            Button1.PerformClick()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        client.hostname = TextBox1.Text

        Dim stats As String() = client.GetApcStatus()
        Dim events As String() = client.GetApcEvents()

        For Each line In stats
            RichTextBox1.AppendText(line)
        Next

        For Each line In events
            RichTextBox2.AppendText(line)
        Next
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        RichTextBox1.Clear()
        RichTextBox2.Clear()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim myProperty As String = InputBox("Enter APC property you want to retrieve", "Enter Property Name", "BCHARGE")
        Dim val As String = client.GetApcValue(myProperty)
        MsgBox(val)
    End Sub
End Class
