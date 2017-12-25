Imports System.Timers

Class GameWindow
    Sub New()

        ' 此调用是设计器所必需的。
        InitializeComponent()

        ' 在 InitializeComponent() 调用之后添加任何初始化。
        Dim _l As New List(Of Char)
        Dim r As New Random

        For i = 0 To 7
            Dim c As Char,
                b As Boolean
            Do
                c = Chr(r.Next(32, 127))
                b = _l.IndexOf(c) = -1
            Loop Until b
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

        AddHandler t.Elapsed, AddressOf t_Elapsed
    End Sub

    Private Sub t_Elapsed(sender As Object, e As ElapsedEventArgs)
        t.Enabled = False
        fsHide()
    End Sub
    Private Sub fsHide()
        f.Content = s.Content = ""
        f = Nothing
        s = Nothing
    End Sub

    Private f, s As Button
    Private l As New List(Of String)
    Private t As New Timers.Timer() With {.Interval = 3000}

    Private Sub Button_Click(sender As Object, e As RoutedEventArgs)
        If t.Enabled Then
            t.Stop()
            fsHide()
        End If
        If f Is Nothing Then
            f = sender
            f.Content = l(f.TabIndex)
            f.IsEnabled = False
        Else
            Dim b As Button = sender
            Dim s As String = CStr(l(b.TabIndex))
            If s = l(f.TabIndex) Then
                b.Content = s
                b.IsEnabled = False
                f = Nothing
            Else
                Me.s = b
                t.Start()
            End If
        End If
    End Sub
End Class
