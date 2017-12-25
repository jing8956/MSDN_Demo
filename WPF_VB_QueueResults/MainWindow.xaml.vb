Imports System.Net.Http
Imports System.Threading

Class MainWindow
    Private _pendingWork As Task = Nothing
    Private _group As Char = Chr(Asc("A"c) - 1)

    Private Async Sub StartButton_Click(sender As Object, e As RoutedEventArgs) Handles StartButton.Click
        _group = Chr(Asc(_group) + 1)
        ResultsTextBox.Text += $"{vbCrLf}{vbCrLf}#开始第{_group}组."

        Try
            Dim finishedGroup As Char = Await AccessTheWebAsync(_group)
            ResultsTextBox.Text += $"{vbCrLf}{vbCrLf}#第{finishedGroup}组完成{vbCrLf}"
        Catch ex As Exception
            ResultsTextBox.Text += $"{vbCrLf}下载失败。{ex.Message}"
        End Try
    End Sub

    Private Async Function AccessTHeWebAsync(grp As Char) As Task(Of Char)
        Dim client As New HttpClient(),
            urlList As List(Of String) = SetUpURLList(),
            getContentTasks As Task(Of Byte())() = urlList.Select(Function(url) client.GetByteArrayAsync(url)).ToArray

        _pendingWork = FinishOneGroupAsync(urlList, getContentTasks, grp)

        ResultsTextBox.Text += $"{vbCrLf}#任务{grp}正在等待。"

        Await _pendingWork

        Return grp
    End Function

    Private Async Function FinishOneGroupAsync(urls As List(Of String), contentTasks() As Task(Of Byte()), grp As Char) As Task
        If _pendingWork IsNot Nothing Then Await _pendingWork

        Dim total = 0
        Dim contentTasksList = contentTasks.ToList()

        For i = 0 To contentTasks.Length - 1
            Dim t = Await Task.WhenAny(contentTasksList),
                index = contentTasksList.IndexOf(t)
            contentTasksList.Remove(t)
            Dim content = Await t
            DisplayResults(urls(index), content, i, grp)
            urls.RemoveAt(index)
            total += content.Length
        Next

        ResultsTextBox.Text += $"{vbCrLf}{vbCrLf}#总长度为{total}。"
    End Function

    Private Sub DisplayResults(url As String, content() As Byte, pos As Integer, grp As Char)
        Dim displayURL = url.Replace("http://", "")
        ResultsTextBox.Text += String.Format(vbCrLf & "{0}-{1}. {2,-58} {3,8}", grp, pos + 1, displayURL, content.Length)
    End Sub

    Private Function SetUpURLList() As List(Of String)
        Return New List(Of String)({
            "http://msdn.microsoft.com/en-us/library/hh191443.aspx",
            "http://msdn.microsoft.com/en-us/library/aa578028.aspx",
            "http://msdn.microsoft.com/en-us/library/jj155761.aspx",
            "http://msdn.microsoft.com/en-us/library/hh290140.aspx",
            "http://msdn.microsoft.com/en-us/library/hh524395.aspx",
            "http://msdn.microsoft.com/en-us/library/ms404677.aspx",
            "http://msdn.microsoft.com",
            "http://msdn.microsoft.com/en-us/library/ff730837.aspx"})
    End Function
End Class
