Imports System.Data

Public Class PrintParcodeGrid

    Dim bm As New BasicMethods
    Dim dt As New DataTable
    WithEvents G As New MyGrid

    Dim dv As New DataView
    Dim HelpDt As New DataTable
    Public Flag As Integer = 1
    Public FirstColumn As String = "الكـــــود", SecondColumn As String = "الاســــــــــــم", ThirdColumn As String = "السعــــر", Statement As String = ""
    Dim Gp As String = "المجموعات", Tp As String = "الأنواع", It As String = "الأصناف"

    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        G.EndEdit()

        Dim rpt As New ReportViewer

        If Flag = 1 Then
            If Md.MyProjectType = ProjectType.Optics Then
                If bm.ShowDeleteMSG("هل تريد طباعة المقاس الكبير؟") Then
                    rpt.Rpt = "PrintBarcodeS2.rpt"
                Else
                    rpt.Rpt = "PrintBarcodeS1.rpt"
                End If
            Else
                rpt.Rpt = "PrintBarcode.rpt"
            End If
            rpt.paraname = New String() {"@InvoiceNo", "@ItemId", "@ColorId", "@SizeId", "@Count", "Header"}
        End If


        For i As Integer = 0 To G.Rows.Count - 1
            Try
                If Val(G.Rows(i).Cells(GC.Id).Value) = 0 Then Continue For



                If Flag = 1 Then

                    rpt.paravalue = New String() {Val(InvoiceNo.Text), Val(G.Rows(i).Cells(GC.Id).Value), Val(G.Rows(i).Cells(GC.Color).Value), Val(G.Rows(i).Cells(GC.Size).Value), 1, CType(Parent, Page).Title}
                    rpt.Print(".", Md.BarcodePrinter, Val(G.Rows(i).Cells(GC.Qty).Value), False)
                ElseIf Flag = 2 Then
                    bm.ExecuteScalar("Update Items set IsStopped=1 where Id=" & Val(G.Rows(i).Cells(GC.Id).Value))
                ElseIf Flag = 3 Then
                    bm.ExecuteScalar("Update Items set Commission='" & Val(G.Rows(i).Cells(GC.Commission).Value) & "' where Id=" & Val(G.Rows(i).Cells(GC.Id).Value))
                End If
            Catch ex As Exception
            End Try
        Next
        G.Rows.Clear()

        If Flag = 2 Then
            LoadAllItems()
        End If
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me) Then Return
        LoadWFH()
        LoadResource()

        If Not Md.MyProjectType = ProjectType.X14 Then
            lblUnitCount.Visibility = Visibility.Hidden
            InvoiceNo.Visibility = Visibility.Hidden
            btnItemsSearch.Visibility = Visibility.Hidden
            btnBalSearch.Visibility = Visibility.Hidden
        End If


        LoadAllItems()
        If Flag = 2 Then
            Button2.Content = "إيقاف الأصناف"
        ElseIf Flag = 3 Then
            Button2.Content = "تحديث الأصناف"
        End If
    End Sub

    Sub LoadAllItems()
        Try
            HelpDt = bm.ExecuteAdapter("Select cast(Id as nvarchar(100))Id,Name,SalesPrice Price From Items  where IsStopped=0 ")
            HelpDt.TableName = "tbl"
            HelpDt.Columns(0).ColumnName = FirstColumn
            HelpDt.Columns(1).ColumnName = SecondColumn
            HelpDt.Columns(2).ColumnName = ThirdColumn

            dv.Table = HelpDt
            HelpGD.ItemsSource = dv
            HelpGD.Columns(0).Width = 75
            HelpGD.Columns(1).Width = 220
            HelpGD.Columns(2).Width = 75

            If (Md.Receptionist AndAlso Md.MyProjectType = ProjectType.X14) OrElse (Not Md.Manager AndAlso Md.Nurse AndAlso Md.MyProjectType = ProjectType.X8) Then
                HelpGD.Columns(2).Visibility = Windows.Visibility.Hidden
            End If

            HelpGD.SelectedIndex = 0
        Catch
        End Try

    End Sub


    Dim lop As Boolean = False



    Private Sub LoadResource()


    End Sub


    Structure GC
        Shared Barcode As String = "Barcode"
        Shared Id As String = "Id"
        Shared Name As String = "Name"
        Shared Color As String = "Color"
        Shared Size As String = "Size"
        Shared Qty As String = "Qty"
        Shared Commission As String = "Commission"
    End Structure


    Private Sub LoadWFH()
        WFH.Child = G

        G.Columns.Clear()
        G.ForeColor = System.Drawing.Color.DarkBlue

        G.Columns.Add(GC.Barcode, "الباركود")
        G.Columns.Add(GC.Id, "كود الصنف")
        G.Columns.Add(GC.Name, "اسم الصنف")

        Dim GCColor As New Forms.DataGridViewComboBoxColumn
        GCColor.HeaderText = "اللون"
        GCColor.Name = GC.Color
        bm.FillCombo("select 0 Id,'' Name", GCColor)
        G.Columns.Add(GCColor)

        Dim GCSize As New Forms.DataGridViewComboBoxColumn
        GCSize.HeaderText = "المقاس"
        GCSize.Name = GC.Size
        bm.FillCombo("select 0 Id,'' Name", GCSize)
        G.Columns.Add(GCSize)
        G.Columns.Add(GC.Qty, "العدد")
        G.Columns.Add(GC.Commission, "العمولة")

        G.Columns(GC.Barcode).FillWeight = 150
        G.Columns(GC.Id).FillWeight = 110
        G.Columns(GC.Name).FillWeight = 300
        G.Columns(GC.Color).FillWeight = 100
        G.Columns(GC.Size).FillWeight = 100
        G.Columns(GC.Qty).FillWeight = 100
        
        G.Columns(GC.Name).ReadOnly = True
        G.BarcodeIndex = G.Columns(GC.Barcode).Index

        If Not Md.MyProjectType = ProjectType.X14 Then
            G.Columns(GC.Color).Visible = False
            G.Columns(GC.Size).Visible = False
        End If

        If Flag <> 1 Then
            G.Columns(GC.Qty).Visible = False
        End If

        If Flag <> 3 Then
            G.Columns(GC.Commission).Visible = False
        End If

        AddHandler G.CellEndEdit, AddressOf GridCalcRow
        AddHandler G.KeyDown, AddressOf GridKeyDown
    End Sub

    Private Sub GridCalcRow(ByVal sender As Object, ByVal e As Forms.DataGridViewCellEventArgs)
        If G.Columns(G.CurrentCell.ColumnIndex).Name = GC.Barcode AndAlso Not G.Rows(G.CurrentCell.RowIndex).Cells(GC.Barcode).Value Is Nothing Then
            If Not G.Rows(G.CurrentCell.RowIndex).Cells(GC.Barcode).Value = Nothing Then
                G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id).Value = bm.ExecuteScalar("select Id from Items where IsStopped=0 and Barcode='" & Val(G.Rows(G.CurrentCell.RowIndex).Cells(GC.Barcode).Value.ToString) & "'")
                AddItem(G.Rows(G.CurrentCell.RowIndex).Cells(GC.Id).Value, G.CurrentCell.RowIndex, 0)
                Exit Sub
            End If
        ElseIf G.Columns(e.ColumnIndex).Name = GC.Id Then
            AddItem(G.Rows(e.RowIndex).Cells(GC.Id).Value, e.RowIndex, 0)
        End If
        G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
        CalcRow(e.RowIndex)
    End Sub



    Private Sub GridKeyDown(ByVal sender As Object, ByVal e As Forms.KeyEventArgs)
        e.Handled = True
        Try
            If G.CurrentCell.RowIndex = G.Rows.Count - 1 Then
                Dim c = G.CurrentCell.RowIndex
                G.Rows.Add()
                G.CurrentCell = G.Rows(c).Cells(G.CurrentCell.ColumnIndex)
            End If

            If G.CurrentCell.ColumnIndex = G.Columns(GC.Id).Index OrElse G.CurrentCell.ColumnIndex = G.Columns(GC.Name).Index Then
                If bm.ShowHelpGrid("Items", G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, "select cast(Id as varchar(100)) Id,Name from Items where IsStopped=0") Then
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
                    If G.Rows(G.CurrentCell.RowIndex).Cells(GC.Color).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Color)
                    ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                    End If
                End If

                If bm.ShowHelpGridItemBal(G.CurrentRow.Cells(GC.Id), G.CurrentRow.Cells(GC.Name), e, "GetItemCurrentBal " & Val(G.CurrentRow.Cells(GC.Id).Value)) Then
                    GridCalcRow(sender, New Forms.DataGridViewCellEventArgs(G.Columns(GC.Id).Index, G.CurrentCell.RowIndex))
                    If G.Rows(G.CurrentCell.RowIndex).Cells(GC.Color).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Color)
                    ElseIf G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty).Visible Then
                        G.CurrentCell = G.Rows(G.CurrentCell.RowIndex).Cells(GC.Qty)
                    End If

                End If
            End If
        Catch ex As Exception
        End Try
    End Sub

    Public Sub AddItem(ByVal Id As String, Optional ByVal i As Integer = -1, Optional ByVal Add As Decimal = 1)
        Try
            Dim Exists As Boolean = False
            Dim Move As Boolean = False
            If i = -1 Then Move = True

            G.AutoSizeColumnsMode = Forms.DataGridViewAutoSizeColumnsMode.Fill
            If i = -1 Then
                i = G.Rows.Add()
Br:
            End If

            Dim dt As DataTable = bm.ExecuteAdapter("Select * From Items  where IsStopped=0 and Id='" & Id & "'")
            Dim dr() As DataRow = dt.Select("Id='" & Id & "'")
            If dr.Length = 0 Then
                If Not G.Rows(i).Cells(GC.Id).Value Is Nothing Or G.Rows(i).Cells(GC.Id).Value <> "" Then bm.ShowMSG("هذا الصنف غير موجود")
                ClearRow(i)
                Return
            End If
            G.Rows(i).Cells(GC.Id).Value = dr(0)(GC.Id)
            G.Rows(i).Cells(GC.Name).Value = dr(0)(GC.Name)
            G.Rows(i).Cells(GC.Barcode).Value = dr(0)(GC.Barcode)
            G.Rows(i).Cells(GC.Commission).Value = dr(0)(GC.Commission)

            bm.FillCombo("select 0 Id,'-' Name union select Id,Name from ColorsDetails where ColorId=(select It.ColorId from Items It where It.Id='" & Id & "'" & ") order by Id", G.Rows(i).Cells(GC.Color))

            bm.FillCombo("select 0 Id,'-' Name union select Id,Name from SizesDetails where SizeId=(select It.SizeId from Items It where It.Id='" & Id & "'" & ") order by Id", G.Rows(i).Cells(GC.Size))

            If G.Rows(i).Cells(GC.Color).Value Is Nothing Then G.Rows(i).Cells(GC.Color).Value = 0
            If G.Rows(i).Cells(GC.Size).Value Is Nothing Then G.Rows(i).Cells(GC.Size).Value = 0
            If Val(G.Rows(i).Cells(GC.Qty).Value) = 0 Then Add = 1
            G.Rows(i).Cells(GC.Qty).Value = Add + Val(G.Rows(i).Cells(GC.Qty).Value)

            CalcRow(i)
            If Move Then
                G.Focus()
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                If Flag = 1 Then
                    G.CurrentCell = G.Rows(i).Cells(GC.Qty)
                Else
                    G.CurrentCell = G.Rows(i).Cells(GC.Name)
                End If
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
            If Exists Then
                G.Rows(i).Selected = True
                G.FirstDisplayedScrollingRowIndex = i
                G.CurrentCell = G.Rows(i).Cells(GC.Name)
                If Flag = 1 Then G.CurrentCell = G.Rows(i).Cells(GC.Qty)
                G.EditMode = Forms.DataGridViewEditMode.EditOnEnter
                G.BeginEdit(True)
            End If
        Catch ex As Exception
            If i <> -1 Then
                ClearRow(i)
            End If
        End Try
    End Sub

    Sub CalcRow(ByVal i As Integer)
        Try
            If G.Rows(i).Cells(GC.Id).Value Is Nothing OrElse G.Rows(i).Cells(GC.Id).Value.ToString = "" Then
                ClearRow(i)
                Return
            End If
            G.Rows(i).Cells(GC.Qty).Value = Val(G.Rows(i).Cells(GC.Qty).Value)
        Catch ex As Exception
            ClearRow(i)
        End Try
    End Sub

    Sub ClearRow(ByVal i As Integer)
        G.Rows(i).Cells(GC.Id).Value = Nothing
        G.Rows(i).Cells(GC.Name).Value = Nothing
        G.Rows(i).Cells(GC.Color).Value = Nothing
        G.Rows(i).Cells(GC.Size).Value = Nothing
        G.Rows(i).Cells(GC.Qty).Value = Nothing
        G.Rows(i).Cells(GC.Commission).Value = Nothing
    End Sub


    Private Sub btnItemsSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnItemsSearch.Click
        Try
            If G.CurrentRow Is Nothing Then G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(GC.Id)
            G.Focus()
            G.CurrentCell = G.Rows(G.CurrentRow.Index).Cells(GC.Id)
            GridKeyDown(G, New System.Windows.Forms.KeyEventArgs(Forms.Keys.F1))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub btnBalSearch_Click(sender As Object, e As RoutedEventArgs) Handles btnBalSearch.Click
        Try
            If G.CurrentRow Is Nothing Then G.CurrentCell = G.Rows(G.Rows.Count - 1).Cells(GC.Id)
            G.Focus()
            G.CurrentCell = G.Rows(G.CurrentRow.Index).Cells(GC.Id)
            GridKeyDown(G, New System.Windows.Forms.KeyEventArgs(Forms.Keys.F12))
        Catch ex As Exception
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
            dv.RowFilter = " [" & FirstColumn & "] like '" & txtID.Text.Trim & "%' and [" & SecondColumn & "] like '%" & txtName.Text & "%' and [" & ThirdColumn & "] >=" & IIf(txtPrice.Text.Trim = "", 0, txtPrice.Text) & ""
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



End Class