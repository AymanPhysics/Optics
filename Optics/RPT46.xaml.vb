Imports System.Data

Public Class RPT46

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    Public Flag As Integer = 0
    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Val(GroupId.Text) = 0 OrElse Val(TypeId.Text) = 0 OrElse FromCLY.Text.Trim = "" OrElse ToCLY.Text.Trim = "" OrElse FromSPH.Text.Trim = "" OrElse ToSPH.Text.Trim = "" Then

            bm.ShowMSG("برجاء تحديد كافة البيانات")
            Return
        End If
        bm.ShowMSG(bm.ExecuteScalar("GenerateItems", {"GroupId", "TypeId", "SalesPrice", "FromCLY", "ToCLY", "FromSPH", "ToSPH"}, {Val(GroupId.Text), Val(TypeId.Text), Val(SalesPrice.Text), Val(FromCLY.Text), Val(ToCLY.Text), Val(FromSPH.Text), Val(ToSPH.Text)}))
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({GroupId, TypeId})
    End Sub

    Dim lop As Boolean = False



    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")

    End Sub


    Private Sub GroupId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles GroupId.LostFocus
        bm.LostFocus(GroupId, GroupName, "select Name from Groups where Id=" & GroupId.Text.Trim())
        TypeId_LostFocus(Nothing, Nothing)
    End Sub

    Private Sub TypeId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles TypeId.LostFocus
        bm.LostFocus(TypeId, TypeName, "select Name from Types where GroupId=" & GroupId.Text.Trim & " and Id=" & TypeId.Text.Trim())
    End Sub

    Private Sub GroupId_KeyDown(ByVal sender As System.Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles GroupId.KeyUp
        If bm.ShowHelp("Groups", GroupId, GroupName, e, "select cast(Id as varchar(100)) Id,Name from Groups") Then
            GroupId_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub TypeId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles TypeId.KeyUp
        bm.ShowHelp("Types", TypeId, TypeName, e, "select cast(Id as varchar(100)) Id,Name from Types where GroupId=" & GroupId.Text.Trim)
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles GroupId.KeyDown, TypeId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SalesPrice.KeyDown, FromCLY.KeyDown, ToCLY.KeyDown, FromSPH.KeyDown, ToSPH.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub
End Class