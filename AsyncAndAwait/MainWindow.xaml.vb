Imports System.Net.Http
Imports System.Threading

Class MainWindow

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

    Private Property Group As Integer = 0
    Private Property PendingWork As New Dictionary(Of Integer, List(Of Task(Of Integer)))
    Private Property CtsList As New Dictionary(Of Integer, List(Of CancellationTokenSource))

    Private Shared _maxPendingWork As Byte = 2
    Private Shared Property MaxPendingWork As Byte
        Get
            Return _maxPendingWork
        End Get
        Set(value As Byte)
            _maxPendingWork = value
        End Set
    End Property

    Private Async Sub StarButton_Click(sender As Object, e As RoutedEventArgs) Handles StarButton.Click

        Group += 1

        Dim urlList As List(Of String) = SetUpURLList(),
            baseitem As New TreeViewItem() With {
            .Header = $"任务{Group}",
            .Background = Brushes.Red}
        AddHandler baseitem.Selected, Sub(sender2 As Object, e2 As RoutedEventArgs)

                                          Dim baseitem2 = TryCast(sender2, TreeViewItem)
                                          If baseitem2 IsNot Nothing Then
                                              For Each i In baseitem2.Items
                                                  Dim item = TryCast(i, TreeViewItem)
                                                  If item IsNot Nothing Then
                                                      TryCast(item.Tag, CancellationTokenSource)?.Cancel()
                                                  End If
                                              Next
                                              baseitem2.Header = DirectCast(baseitem2.Header, String) & "已取消"
                                          End If

                                          e2.Handled = True

                                      End Sub

        CtsList.Add(Group, New List(Of CancellationTokenSource)())
        For Each url In urlList
            Dim item As New TreeViewItem() With {
                .Header = url,
                .Background = Brushes.Red},
                cts = New CancellationTokenSource()
            CtsList(Group).Add(cts)
            item.Tag = cts
            AddHandler item.Selected, Sub(sender2 As Object, e2 As RoutedEventArgs)

                                          Dim item2 = TryCast(sender2, TreeViewItem)
                                          If item2 IsNot Nothing Then
                                              TryCast(item2.Tag, CancellationTokenSource)?.Cancel()
                                          End If

                                          e2.Handled = True

                                      End Sub
            baseitem.Items.Add(item)
        Next

        baseitem.IsExpanded = True
        tv.Items.Add(baseitem)

        If PendingWork.Count = MaxPendingWork Then Await Task.WhenAny(PendingWork)

        Dim tlist As New List(Of Task(Of Integer))()
        PendingWork.Add(Group, tlist)
        Using client As New HttpClient()

            For i = 0 To urlList.Count - 1
                tlist.Add((Async Function(c As HttpClient, u As String, t As CancellationToken) As Task(Of Integer)

                               Dim response = Await c.GetAsync(u, t),
                               urlContents As Byte() = Await response.Content.ReadAsByteArrayAsync()
                               Return urlContents.Length

                           End Function).Invoke(client, urlList(i), CtsList(Group)(i).Token))
            Next

            For Each i In baseitem.Items
                Dim item = TryCast(i, TreeViewItem)
                If item IsNot Nothing Then
                    item.Background = Brushes.Green
                End If
            Next
            baseitem.Background = Brushes.Green

            Dim l As Integer = 0,
                newurlList As List(Of String) = SetUpURLList()
            Do While tlist.Count > 0
                Dim t As Task(Of Integer) = Await Task.WhenAny(tlist),
                    index As Integer = tlist.IndexOf(t),
                    item As TreeViewItem = DirectCast(baseitem.Items(urlList.IndexOf(newurlList(index))), TreeViewItem)

                Try
                    item.Header = DirectCast(item.Header, String) & $"  长度:{Await t}"
                Catch ex As OperationCanceledException
                    item.Header = DirectCast(item.Header, String) & "已取消"
                    item.Background = Brushes.Gray
                Catch ex As Exception
                    item.Header = DirectCast(item.Header, String) & ex.Message
                    item.Background = Brushes.Yellow
                    baseitem.Background = Brushes.Yellow
                Finally
                    tlist.Remove(t)
                    newurlList.RemoveAt(index)
                End Try

            Loop
            baseitem.Header = DirectCast(baseitem.Header, String) & $"  长度:{l}"

        End Using


    End Sub

End Class
