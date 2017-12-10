Class GameWindow
    Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Dim _l As New List(Of Char)
        Dim r As New Random

        For i = 0 To 7
            Dim c = Chr(r.Next(32, 127))
            For j = 0 To 1
                _l.Add(c)
            Next
        Next

        For i = 0 To 3
            For j = 0 To 3
                Dim b As New Button With {.TabIndex = i * 4 + j}
                AddHandler b.Click, AddressOf Button_Click
                Grid.SetRow(b, i)
                Grid.SetColumn(b, j)
                g.Children.Add(b)

                Dim n = r.Next(0, _l.Count)
                l.Add(_l(n))
                _l.RemoveAt(n)
            Next
        Next
    End Sub
    Private f As Button
    Private l As New List(Of String)
    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        If f Is Nothing Then
            f = sender
            f.Content = l(f.TabIndex)
            f.IsEnabled = False
        Else
            Dim b As Button = sender
            Dim c As String = CStr(l(b.TabIndex))
            If c = l(f.TabIndex) Then
                b.Content = c
                b.IsEnabled = False
            Else
                f.Content = ""
                f.IsEnabled = True
            End If
            f = Nothing
        End If
    End Sub
End Class
