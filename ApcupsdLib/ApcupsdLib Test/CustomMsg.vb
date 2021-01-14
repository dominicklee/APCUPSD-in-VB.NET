Public Class CustomMsg

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim result As String = Form1.client.processCustomMessage(RichTextBox1.Text)
        RichTextBox2.Text = result
    End Sub
End Class