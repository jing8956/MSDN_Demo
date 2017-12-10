Imports System.Net.Http
Imports System.Threading

Class MainWindow
    Private _pendingWork As Task = Nothing
    Private _group As Char = Chr(Asc("A"c) - 1)

    Private Async Sub StartButton_Click(sender As Object, e As RoutedEventArgs) Handles StartButton.Click
        _group = Chr(Asc(_group) + 1)
        ResultsTextBox.Text += String.Format(vbCrLf & vbCrLf & "#开始第{0}组.", _group)

        Try
            Dim finishedGroup As Char = Await AccessTheWebAsync(_group)
            ResultsTextBox.Text += String.Format(vbCrLf & vbCrLf & "#第{0}组完成" & vbCrLf, finishedGroup)
        Catch
            ResultsTextBox.Text += vbCrLf & "下载失败。"
        End Try
    End Sub
    Private Async Function AccessTHeWebAsync(grp As Char) As Task(Of Char)
        Dim client As New HttpClient(),
            urlList As List(Of String) = SetUpURLList(),
            getContentTasks As Task(Of Byte())() = urlList.Select(Function(url) client.GetByteArrayAsync(url)).ToArray

        _pendingWork = FinishOneGroupAsync(urlList, getContentTasks, grp)

        ResultsTextBox.Text = String.Format(vbCrLf & "")
    End Function

    Private Function FinishOneGroupAsync(urlList As List(Of String), getContentTasks() As Task(Of Byte()), grp As Char) As Task
        Throw New NotImplementedException()
    End Function

    Private Function SetUpURLList() As List(Of String)
        Throw New NotImplementedException()
    End Function
End Class
