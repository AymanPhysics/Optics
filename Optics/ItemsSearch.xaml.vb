Imports System
Imports System.Data
Imports System.Threading
Imports System.Windows.Threading

Public Class ItemsSearch
    Dim bm As New BasicMethods
    Public Id As Integer = 0
    Public Ids As List(Of Integer) = New List(Of Integer)
    Public StoreId As Integer
    Public GroupId As Integer
    Public TypeId As Integer
    Public GroupName As String
    Public TypeName As String

    Dim t As New DispatcherTimer With {.IsEnabled = False, .Interval = New TimeSpan(0, 0, 0, 0, 100)}
    Private Sub Window_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadResource()
        Dim bf As New BasicForm

        Dim dt As DataTable = bm.ExecuteAdapter("GetLenseBal", {"StoreId", "GroupId", "TypeId"}, {StoreId, GroupId, TypeId})
        Dim x As Integer = 0, y As Integer = 0
        For i As Integer = 0 To dt.Rows.Count - 1
            If i > 0 Then
                If dt.Rows(i)("SPH").ToString <> dt.Rows(i - 1)("SPH").ToString Then
                    y += 1
                    x = 0
                End If
            End If
            Dim x2 As New Button
            x2.Style = Application.Current.FindResource("GlossyCloseButton")
            x2.FontSize -= 1
            x2.Width = 90
            x2.Height = 50
            'x2.Margin = New Thickness(10 + x * 100, 10 + y * 60, 0, 0)

            x2.Margin = New Thickness(-400 * dt.Rows(i)("MinSPH") + +dt.Rows(i)("SPH") * 400, -300 * dt.Rows(i)("MinCLY") + dt.Rows(i)("CLY") * 300, 0, 0)

            x2.HorizontalAlignment = HorizontalAlignment.Left
            x2.VerticalAlignment = VerticalAlignment.Top
            x2.HorizontalContentAlignment = HorizontalAlignment.Center
            x2.VerticalContentAlignment = HorizontalAlignment.Center
            x2.Cursor = Input.Cursors.Pen
            x2.Name = "Item_" & dt.Rows(i)("Id").ToString
            x2.Tag = dt.Rows(i)("Id").ToString
            x2.Content = dt.Rows(i)("Name").ToString & vbCrLf & dt.Rows(i)("Qty").ToString
            x2.ToolTip = x2.Content
            x2.Background = bf.btnNew.Background 'System.Windows.Media.Brushes.Blue
            x2.Foreground = System.Windows.Media.Brushes.Black
            If dt.Rows(i)("Qty") < 0 Then
                x2.Background = System.Windows.Media.Brushes.Blue
            ElseIf dt.Rows(i)("Qty") = 0 Then
                x2.Background = System.Windows.Media.Brushes.Red
            ElseIf dt.Rows(i)("Qty") = 1 Then
                x2.Background = System.Windows.Media.Brushes.Yellow
            ElseIf dt.Rows(i)("Qty") = 2 Then
                x2.Background = System.Windows.Media.Brushes.GreenYellow
            Else
                x2.Background = System.Windows.Media.Brushes.Orange
            End If
            x2.FontSize = 14
            WR.Children.Add(x2)
            AddHandler x2.Click, AddressOf btnItemClick
            x += 1

        Next
    End Sub

    Private Sub btnItemClick(sender As Object, e As RoutedEventArgs)
        'Id = sender.Tag
        'Close()
        Ids.Add(sender.Tag)
    End Sub

    Private Sub btnNo_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles btnNo.Click
        Close()
    End Sub

    Public Sub Tick()
        Dim f As Window = Parent
        Opacity -= 0.1
        If Math.Round(Opacity, 2) = 0.1 Then
            Close()
        End If
    End Sub

    Private Sub LoadResource()
    End Sub

    Private Sub btnSave_Click(sender As Object, e As RoutedEventArgs) Handles btnSave.Click
        Dim rpt As New ReportViewer
        rpt.Rpt = "LenseBal.rpt"
        rpt.paraname = New String() {"@StoreId", "@GroupId", "GroupName", "@TypeId", "TypeName", "Header"}
        rpt.paravalue = New String() {StoreId, GroupId, GroupName, TypeId, TypeName, "أرصدة العدسات"}
        rpt.Show()
    End Sub

    Private Sub btnClose_Click(sender As Object, e As RoutedEventArgs) Handles btnClose.Click
        Close()
    End Sub
End Class
