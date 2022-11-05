Imports System.Data
Imports System.Windows
Imports System.Windows.Media
Imports System.Management

Public Class ItemsPrices

    Dim dv As New DataView
    Dim HelpDt As New DataTable
    Dim dt As New DataTable
    Dim bm As New BasicMethods


    WithEvents G As New MyGrid
    Public FirstColumn As String = "الكـــــود", SecondColumn As String = "الاســــــــــــم", ThirdColumn As String = "السعــــر", Statement As String = ""
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"


    Private Sub Sales_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return


        LoadResource()
        TabItem1.Height = 0
        LoadWFH()


        RdoGrouping_Checked(Nothing, Nothing)

        TabItem1.Header = "" ' TryCast(TryCast(Me.Parent, TabItem).Header, TabsHeader).MyTabHeader

        LoadGroups()
        LoadAllItems()


        btnNew_Click(Nothing, Nothing)
    End Sub


    Structure GC
        Shared Barcode As String = "Barcode"
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared AvgCost As String = "AvgCost"
        Shared SalesPrice As String = "SalesPrice"
        Shared ItemDiscountPerc As String = "ItemDiscountPerc"
        Shared ItemDiscountValue As String = "ItemDiscountValue"
        Shared Value As String = "Value"
        Shared Line As String = "Line"
    End Structure


    Private Sub LoadWFH()
        'WFH.Background = New SolidColorBrush(Colors.LightSalmon)
        'WFH.Foreground = New SolidColorBrush(Colors.Red)
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.Barcode, "الباركود")
        G.Columns.Add(GC.Id, "كود الصنف")
        G.Columns.Add(GC.Name, "اسم الصنف")

        G.Columns.Add(GC.AvgCost, "التكلفة")
        G.Columns.Add(GC.SalesPrice, "سعر البيع")
        G.Columns.Add(GC.ItemDiscountPerc, "خصم نسبة")
        G.Columns.Add(GC.ItemDiscountValue, "خصم قيمة")
        G.Columns.Add(GC.Value, "الصافي")
        G.Columns.Add(GC.Line, "Line")


        G.Columns(GC.Barcode).FillWeight = 150
        G.Columns(GC.Id).FillWeight = 110
        G.Columns(GC.Name).FillWeight = 280

        G.Columns(GC.Name).ReadOnly = True
        G.Columns(GC.Value).ReadOnly = True
        G.Columns(GC.AvgCost).ReadOnly = True


        G.BarcodeIndex = G.Columns(GC.Barcode).Index
        If Not Md.ShowBarcode Then
            G.Columns(GC.Barcode).Visible = False
        End If
        G.Columns(GC.Line).Visible = False


        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
    End Sub

    Sub LoadGroups()
        Try
            WGroups.Children.Clear()
            WTypes.Children.Clear()
            WItems.Children.Clear()
            TabGroups.Header = Gp
            TabTypes.Header = Tp
            TabItems.Header = It

            Dim dt As DataTable = bm.ExecuteAdapter("LoadGroups2", New String() {"Form"}, New String() {0})
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x)
                x.Name = "TabItem_" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WGroups.Children.Add(x)
                AddHandler x.Click, AddressOf LoadTypes
            Next
        Catch
        End Try
    End Sub

    Private Sub LoadTypes(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WTypes.Tag = xx.Tag
            WTypes.Children.Clear()
            WItems.Children.Clear()

            If All.IsChecked Then
                Dim str As String = "select Id from Items_View where GroupId=" & xx.Tag & " And IsStopped=0 "


                Dim dtdt As DataTable = bm.ExecuteAdapter(str)
                For i As Integer = 0 To dtdt.Rows.Count - 1
                    AddItem(dtdt.Rows(i)(0))
                Next
            End If


            TabTypes.Header = Tp & " - " & xx.Content.ToString
            TabItems.Header = It

            Dim dt As DataTable = bm.ExecuteAdapter("LoadTypes2", New String() {"GroupId", "Form"}, New String() {xx.Tag.ToString, 0})
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Name = "TabItem_" & xx.Tag.ToString & "_" & dt.Rows(i)("Id").ToString
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WTypes.Children.Add(x)
                AddHandler x.Click, AddressOf LoadItems
            Next
        Catch
        End Try
    End Sub


    Sub LoadAllItems()
        Try
            txtPrice.Visibility = Windows.Visibility.Hidden

            HelpDt = bm.ExecuteAdapter("Select cast(Id As nvarchar(100))Id,Name From Items_View  where IsStopped=0 " & ItemWhere())
            HelpDt.TableName = "tbl"
            HelpDt.Columns(0).ColumnName = FirstColumn
            HelpDt.Columns(1).ColumnName = SecondColumn

            dv.Table = HelpDt
            HelpGD.ItemsSource = dv
            HelpGD.Columns(0).Width = 75
            HelpGD.Columns(1).Width = 220

            HelpGD.SelectedIndex = 0
        Catch
        End Try

    End Sub

    Private Sub txtId_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.GotFocus
        Try
            dv.Sort = FirstColumn
        Catch
        End Try
    End Sub

    Private Sub txtName_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.GotFocus
        Try
            dv.Sort = SecondColumn
        Catch
        End Try
    End Sub

    Private Sub txtPrice_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtPrice.GotFocus
        Try
            dv.Sort = ThirdColumn
        Catch
        End Try
    End Sub

    Private Sub txtId_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.TextChanged, txtName.TextChanged, txtPrice.TextChanged
        Try
            dv.RowFilter = " [" & FirstColumn & "] Like '" & txtID.Text.Trim & "%' and [" & SecondColumn & "] like '%" & txtName.Text & "%'" '" and [" & ThirdColumn & "] >=" & IIf(txtPrice.Text.Trim = "", 0, txtPrice.Text) & ""
        Catch
        End Try
    End Sub


    Private Sub HelpGD_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.PreviewKeyDown, txtName.PreviewKeyDown, txtPrice.PreviewKeyDown
        Try
            If e.Key = Input.Key.Up Then
                HelpGD.SelectedIndex = HelpGD.SelectedIndex - 1
            ElseIf e.Key = Input.Key.Down Then
                HelpGD.SelectedIndex = HelpGD.SelectedIndex + 1
            End If
        Catch ex As Exception
        End Try
    End Sub


    Private Sub HelpGD_MouseDoubleClick(ByVal sender As Object, ByVal e As System.Windows.Input.MouseButtonEventArgs) Handles HelpGD.MouseDoubleClick
        Try
            AddItem(HelpGD.Items(HelpGD.SelectedIndex)(0))
        Catch ex As Exception
        End Try
    End Sub



    Function ItemWhere() As String
        Dim st As String = ""
        st = " and ItemType in(0,1,2,3) "

        Return st
    End Function
    Private Sub LoadItems(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Try
            Dim xx As Button = sender
            WItems.Tag = xx.Tag
            WItems.Children.Clear()


            If All.IsChecked Then
                Dim dtdt As DataTable = bm.ExecuteAdapter("select Id from Items_View where GroupId=" & WTypes.Tag.ToString & " and TypeId=" & xx.Tag & " and IsStopped=0 ")
                For i As Integer = 0 To dtdt.Rows.Count - 1
                    AddItem(dtdt.Rows(i)(0))
                Next
            End If


            TabItems.Header = It & " - " & xx.Content.ToString

            Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items_View  where IsStopped=0 " & ItemWhere() & " and GroupId=" & WTypes.Tag.ToString & " and TypeId=" & xx.Tag.ToString & " order by Name")
            For i As Integer = 0 To dt.Rows.Count - 1
                Dim x As New Button
                bm.SetStyle(x, 370)
                'bm.SetImage(x, CType(dt.Rows(i)("Image"), Byte()))
                x.Tag = dt.Rows(i)("Id").ToString
                x.Content = dt.Rows(i)("Name").ToString
                x.ToolTip = dt.Rows(i)("Name").ToString
                WItems.Children.Add(x)
                AddHandler x.Click, AddressOf TabItem
            Next
        Catch
        End Try
    End Sub

    Private Sub TabItem(ByVal sender As Object, ByVal e As RoutedEventArgs)
        Dim x As Button = sender
        AddItem(x.Tag)
    End Sub

    Function AddItem(ByVal Id As String, Optional ByVal i As Integer = -1, Optional ByVal Add As Decimal = 1) As Integer
        Try
            If Not TabControl1.SelectedIndex = 0 Then TabControl1.SelectedIndex = 0
            Dim Exists As Boolean = False
            Dim Move As Boolean = False
            If i = -1 Then Move = True

            G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            If i = -1 Then
                i = G.Rows.Add()
Br:
            End If

            Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items_View  where /*IsStopped=0 and*/ Id='" & Id & "' " & ItemWhere())
            Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
            If dr.Length = 0 Then
                If Not G.Rows(i).Cells(GC.Id).Value Is Nothing Or G.Rows(i).Cells(GC.Id).Value <> "" Then bm.ShowMSG("هذا الصنف غير موجود")
                ClearRow(i)
                Return i
            End If
            G.Rows(i).Cells(GC.Id).Value = dr(0)(GC.Id)
            G.Rows(i).Cells(GC.Name).Value = dr(0)(GC.Name)
            G.Rows(i).Cells(GC.Barcode).Value = dr(0)(GC.Barcode)
            G.Rows(i).Cells(GC.SalesPrice).Value = dr(0)(GC.SalesPrice)
            G.Rows(i).Cells(GC.ItemDiscountPerc).Value = dr(0)(GC.ItemDiscountPerc)
            G.Rows(i).Cells(GC.ItemDiscountValue).Value = dr(0)(GC.ItemDiscountValue)
            G.Rows(i).Cells(GC.AvgCost).Value = Math.Round(Val(bm.ExecuteScalar("select (case when isnull(sum(Qty),0)=0 then 0 else isnull(sum(AvgCost),0)/isnull(sum(Qty),0) end) AvgCost from dbo.Fn_AllItemMotion('" & Val(StoreId.Text) & "','" & Val(Id) & "',GETDATE())")), 2)


            GridCalcRow(Nothing, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ItemDiscountValue).Index, i))
            'CalcRow(i)
            If Move Then
                G.Focus()
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
            If Exists Then
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.CurrentCell = G.Rows(i).Cells(GC.ItemDiscountPerc)
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
        Catch
            If i <> -1 Then
                ClearRow(i)
            End If
        End Try
        Return i
    End Function

    Dim lop As Boolean = False
    Sub CalcRow(ByVal i As Integer)
        Try
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value.ToString = "" Then
                ClearRow(i)
                Return
            End If
            'G.Rows(i).Cells(GC.ItemDiscountValue).Value = bm.Rnd5LE(Val(G.Rows(i).Cells(GC.SalesPrice).Value) * Val(G.Rows(i).Cells(GC.ItemDiscountPerc).Value) / 100)
            G.Rows(i).Cells(GC.Value).Value = Val(G.Rows(i).Cells(GC.SalesPrice).Value) - Val(G.Rows(i).Cells(GC.ItemDiscountValue).Value)
        Catch ex As Exception
            ClearRow(i)
        End Try
    End Sub

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.Barcode).Value = Nothing
        G.Rows(i).Cells(GC.Id).Value = Nothing
        G.Rows(i).Cells(GC.Name).Value = Nothing
        G.Rows(i).Cells(GC.SalesPrice).Value = Nothing
        G.Rows(i).Cells(GC.ItemDiscountPerc).Value = Nothing
        G.Rows(i).Cells(GC.ItemDiscountValue).Value = Nothing
        G.Rows(i).Cells(GC.Value).Value = Nothing
        G.Rows(i).Cells(GC.Line).Value = Nothing
    End Sub



    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        Try
            If G.Columns(e.ColumnIndex).Name = GC.Barcode AndAlso Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value Is Nothing Then

                Dim BC As String = G.Rows(e.RowIndex).Cells(GC.Barcode).Value.ToString
                If (Md.MyProjectType = ProjectType.X14) AndAlso Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value = Nothing Then
                    If BC.Length > 12 AndAlso Val(BC.Substring(0, 1)) > 0 Then BC = BC.Substring(0, 12)
                    BC = BC.Substring(1)
                    G.Rows(e.RowIndex).Cells(GC.Id).Value = Val(Mid(BC, 1, BC.Length - 4))
                    AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
                    'LoadItemPrice(e.RowIndex)
                    Exit Sub
                ElseIf Not G.Rows(e.RowIndex).Cells(GC.Barcode).Value = Nothing Then
                    'G.Rows(e.RowIndex).Cells(GC.Id).Value = bm.ExecuteScalar("select Id from Items_View where IsStopped=0 and left(Barcode,12)='" & Val(BC) & "'")
                    G.Rows(e.RowIndex).Cells(GC.Id).Value = bm.ExecuteScalar("select Id from Items_View where IsStopped=0 and Barcode='" & BC & "'")
                    AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
                    Exit Sub
                End If

            ElseIf G.Columns(e.ColumnIndex).Name = GC.Id Then
                AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)

            ElseIf G.Columns(e.ColumnIndex).Name = GC.ItemDiscountPerc Then

                G.Rows(e.RowIndex).Cells(GC.ItemDiscountValue).Value = bm.Rnd5LE(Val(G.Rows(e.RowIndex).Cells(GC.SalesPrice).Value) * Val(G.Rows(e.RowIndex).Cells(GC.ItemDiscountPerc).Value) / 100)
            End If

            G.Rows(e.RowIndex).Cells(GC.Value).Value = Val(G.Rows(e.RowIndex).Cells(GC.SalesPrice).Value) - Val(G.Rows(e.RowIndex).Cells(GC.ItemDiscountValue).Value)

            G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
            CalcRow(e.RowIndex)
        Catch ex As Exception
        End Try
    End Sub



    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click


        G.EndEdit()
        Try
            CalcRow(G.CurrentRow.Index)
        Catch ex As Exception
        End Try

        Dim Str As String = ""
        For i As Integer = 0 To G.Rows.Count - 1
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value = 0 Then
                Continue For
            End If
            Str &= " Update Items set SalesPrice='" & Val(G.Rows(i).Cells(GC.SalesPrice).Value) & "',ItemDiscountPerc='" & Val(G.Rows(i).Cells(GC.ItemDiscountPerc).Value) & "',ItemDiscountValue='" & Val(G.Rows(i).Cells(GC.ItemDiscountValue).Value) & "' where Id='" & Val(G.Rows(i).Cells(GC.Id).Value) & "' "
        Next


        If Not bm.ExecuteNonQuery(Str) Then Return

        btnNew_Click(sender, e)

        DontClear = False
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        ClearControls()
    End Sub

    Dim SalesSerialNoCount As Integer = 0
    Sub ClearControls()
        Try

            G.Rows.Clear()
        Catch
        End Try
    End Sub

    Private Sub BtnSave_Copy_Click(sender As Object, e As RoutedEventArgs) Handles btnSave_Copy.Click
        For i As Integer = 0 To G.Rows.Count - 1
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value = 0 Then
                Continue For
            End If
            G.Rows(i).Cells(GC.ItemDiscountPerc).Value = Val(ItemDiscountPerc.Text)
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ItemDiscountPerc).Index, i))
        Next

    End Sub

    Private Sub BtnSave_Copy2_Click(sender As Object, e As RoutedEventArgs) Handles btnSave_Copy2.Click
        For i As Integer = 0 To G.Rows.Count - 1
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value = 0 Then
                Continue For
            End If
            G.Rows(i).Cells(GC.ItemDiscountValue).Value = Val(ItemDiscountValue.Text)
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ItemDiscountValue).Index, i))
        Next

    End Sub

    Private Sub btnSave_Copy1_Click(sender As Object, e As RoutedEventArgs) Handles btnSave_Copy1.Click
        For i As Integer = 0 To G.Rows.Count - 1
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value = 0 Then
                Continue For
            End If
            G.Rows(i).Cells(GC.SalesPrice).Value = bm.Rnd5LE(Val(G.Rows(i).Cells(GC.SalesPrice).Value) * (100 + Val(ItemNewPricePerc.Text)) / 100)
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ItemDiscountPerc).Index, i))
        Next
    End Sub

    Private Sub btnSave_Copy3_Click(sender As Object, e As RoutedEventArgs) Handles btnSave_Copy3.Click
        For i As Integer = 0 To G.Rows.Count - 1
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value = 0 Then
                Continue For
            End If
            G.Rows(i).Cells(GC.SalesPrice).Value = bm.Rnd5LE(Val(G.Rows(i).Cells(GC.SalesPrice).Value) * (100 - Val(ItemNewPricePerc2.Text)) / 100)
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.ItemDiscountPerc).Index, i))
        Next
    End Sub

    Private Sub RdoGrouping_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles RdoGrouping.Checked, RdoSearch.Checked
        Try
            If RdoGrouping.IsChecked Then
                txtID.Visibility = Visibility.Hidden
                txtName.Visibility = Visibility.Hidden
                txtPrice.Visibility = Visibility.Hidden
                HelpGD.Visibility = Visibility.Hidden
                PanelGroups.Visibility = Visibility.Visible
                PanelTypes.Visibility = Visibility.Visible
                PanelItems.Visibility = Visibility.Visible
            ElseIf RdoSearch.IsChecked Then
                txtID.Visibility = Visibility.Visible
                txtName.Visibility = Visibility.Visible
                txtPrice.Visibility = Visibility.Visible
                HelpGD.Visibility = Visibility.Visible
                PanelGroups.Visibility = Visibility.Hidden
                PanelTypes.Visibility = Visibility.Hidden
                PanelItems.Visibility = Visibility.Hidden
            End If
            If Md.Receptionist AndAlso Md.MyProjectType = ProjectType.X14 Then
                txtPrice.Visibility = Windows.Visibility.Hidden
            End If
        Catch
        End Try
    End Sub


    Dim DontClear As Boolean = False


    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        e.Handled = True
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If
            If G.CurrentCell.ColumnIndex = G.Columns(GC.Id).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.Name).Index Then
                Dim str As String = "select cast(Id as varchar(100)) Id,Name from Items_View where IsStopped=0 " & ItemWhere()
                If bm.ShowHelpGrid("Items", G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, str) Then
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
                    If G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Name)
                    End If

                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnSave_Copy4_Click(sender As Object, e As RoutedEventArgs) Handles btnSave_Copy4.Click
        For i As Integer = 0 To G.Rows.Count - 1
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value = 0 Then
                Continue For
            End If
            G.Rows(i).Cells(GC.SalesPrice).Value = bm.Rnd5LE(Val(G.Rows(i).Cells(GC.SalesPrice).Value) + Val(ItemNewPriceVal.Text))
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Value).Index, i))
        Next
    End Sub

    Private Sub btnSave_Copy5_Click(sender As Object, e As RoutedEventArgs) Handles btnSave_Copy5.Click
        For i As Integer = 0 To G.Rows.Count - 1
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value = 0 Then
                Continue For
            End If
            G.Rows(i).Cells(GC.SalesPrice).Value = bm.Rnd5LE(Val(G.Rows(i).Cells(GC.SalesPrice).Value) - Val(ItemNewPriceVal2.Text))
            GridCalcRow(G, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Value).Index, i))
        Next
    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnNew.SetResourceReference(ContentProperty, "New")
    End Sub

    Private Sub HideAcc()
        PanelItems.Margin = New Thickness(PanelItems.Margin.Left, PanelItems.Margin.Top, PanelItems.Margin.Right, 8)
        HelpGD.Margin = New Thickness(HelpGD.Margin.Left, HelpGD.Margin.Top, HelpGD.Margin.Right, 8)
    End Sub


    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Dim StoreUnitId As Integer = 0
    Public Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
    End Sub

End Class
