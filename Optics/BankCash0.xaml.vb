Imports System.Data

Public Class BankCash0
    Public TableName As String = "BankCash0"
    Public SubId As String = "Flag"
    Public SubId2 As String = "InvoiceNo"


    Dim dt As New DataTable
    Dim bm As New BasicMethods

    Public Flag As Integer = 0
    Public MyLinkFile As Integer = 5
    Private Sub BasicForm_Loaded(ByVal sender As Object, ByVal e As System.Windows.RoutedEventArgs) Handles Me.Loaded
        If bm.TestIsLoaded(Me) Then Return
        bm.TestSecurity(Me, {btnSave}, {btnDelete}, {btnFirst, btnNext, btnPrevios, btnLast}, {})
        bm.FillCombo("select Id,Name from Currencies order by Id", CurrencyId)
        If Not Md.ShowCurrency Then CurrencyId.Visibility = Windows.Visibility.Hidden
        LoadResource()
        bm.Fields = New String() {"BankId", SubId, SubId2, "DayDate", "MainValue", "Value", "SupplierId", "Notes", "Canceled", "CurrencyId", "Exchange", "StoreId", "SalesInvoiceNo"}
        bm.control = New Control() {BankId, txtFlag, txtID, DayDate, MainValue, Value, SupplierId, Notes, Canceled, CurrencyId, Exchange, StoreId, SalesInvoiceNo}
        bm.KeyFields = New String() {"BankId", SubId, SubId2}
        bm.Table_Name = TableName
        btnNew_Click(sender, e)


        If MyLinkFile = 5 Then
            BankId.Text = Md.DefaultSave
        ElseIf MyLinkFile = 6 Then
            BankId.Text = Md.DefaultBank
        End If
        BankId_LostFocus(Nothing, Nothing)
        BankId.IsEnabled = Md.Manager
    End Sub

    Private Sub btnLast_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnLast.Click
        bm.FirstLast(New String() {"BankId", SubId, SubId2}, "Max", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Sub FillControls()
        bm.FillControls(Me)
        'BankId_LostFocus(Nothing, Nothing)
        SupplierId_LostFocus(Nothing, Nothing)
        StoreId_LostFocus(Nothing, Nothing)
        bm.FillControls(Me)
        DayDate.Focus()
    End Sub
    Private Sub btnNext_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNext.Click
        bm.NextPrevious(New String() {"BankId", SubId, SubId2}, New String() {BankId.Text, txtFlag.Text, txtID.Text}, "Next", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnSave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSave.Click
        If Val(BankId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد " & lblBank.Content)
            BankId.Focus()
            Return
        End If
        If Val(SupplierId.Text) = 0 Then
            bm.ShowMSG("برجاء تحديد نوع المستثمر")
            SupplierId.Focus()
            Return
        End If

        Value.Text = Val(Value.Text)
        bm.DefineValues()
        If Not bm.Save(New String() {"BankId", SubId, SubId2}, New String() {BankId.Text, txtFlag.Text.Trim, txtID.Text}) Then Return
        btnNew_Click(sender, e)
    End Sub

    Private Sub btnFirst_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnFirst.Click
        bm.FirstLast(New String() {"BankId", SubId, SubId2}, "Min", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub

    Private Sub btnNew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnNew.Click
        bm.ClearControls()
        ClearControls()
    End Sub

    Dim lop As Boolean = False
    Sub ClearControls()
        If lop OrElse lv Then Return
        lop = True

        bm.ClearControls()
        BankId_LostFocus(Nothing, Nothing)

        SupplierName.Clear()
        StoreId_LostFocus(Nothing, Nothing)

        Dim MyNow As DateTime = bm.MyGetDate()
        DayDate.SelectedDate = MyNow
        txtFlag.Text = Flag
        'BankName.Clear()
        txtID.Text = bm.ExecuteScalar("select max(" & SubId2 & ")+1 from " & TableName & " where " & SubId & "=" & txtFlag.Text & " and BankId=" & BankId.Text)
        If txtID.Text = "" Then txtID.Text = "1"
        'DayDate.Focus()
        txtID.Focus()
        txtID.SelectAll()
        lop = False
    End Sub

    Private Sub btnDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDelete.Click
        If bm.ShowDeleteMSG() Then
            bm.ExecuteNonQuery("delete from " & TableName & " where " & SubId & "='" & txtFlag.Text.Trim & "' and " & SubId2 & "=" & txtID.Text & " and BankId=" & BankId.Text)
            btnNew_Click(sender, e)
        End If
    End Sub

    Private Sub btnPrevios_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPrevios.Click
        bm.NextPrevious(New String() {"BankId", SubId, SubId2}, New String() {BankId.Text, txtFlag.Text, txtID.Text}, "Back", dt)
        If dt.Rows.Count = 0 Then Return
        FillControls()
    End Sub
    Dim lv As Boolean = False

    Private Sub txtID_LostFocus(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtID.LostFocus
        If lv Then
            Return
        End If
        lv = True

        bm.DefineValues()
        Dim dt As New DataTable
        bm.RetrieveAll(New String() {"BankId", SubId, SubId2}, New String() {BankId.Text, txtFlag.Text.Trim, txtID.Text}, dt)
        If dt.Rows.Count = 0 Then
            ClearControls()
            lv = False
            Return
        End If
        FillControls()
        lv = False
    End Sub

    Private Sub txtID_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles txtID.KeyDown, SupplierId.KeyDown, StoreId.KeyDown
        bm.MyKeyPress(sender, e)
    End Sub

    Private Sub txtID_KeyPress2(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles Value.KeyDown, MainValue.KeyDown, Exchange.KeyDown
        bm.MyKeyPress(sender, e, True)
    End Sub

    Private Sub BankId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles BankId.LostFocus

        If Val(BankId.Text.Trim) = 0 Then
            BankId.Clear()
            BankName.Clear()
            Return
        End If

        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
        bm.LostFocus(BankId, BankName, "select Name from " & dt.Rows(0)("TableName") & " where Id=" & BankId.Text.Trim())
        CurrencyId.SelectedValue = bm.ExecuteScalar("select CurrencyId from " & dt.Rows(0)("TableName") & " where Id=" & BankId.Text.Trim())

        Exchange.Text = bm.ExecuteScalar("select dbo.GetCurrencyExchange(" & Val(BankId.Text) & "," & MyLinkFile & "," & CurrencyId.SelectedValue.ToString & ",0,'" & bm.ToStrDate(DayDate.SelectedDate) & "')")

        If lop Then Return
        btnNew_Click(Nothing, Nothing)
    End Sub
    Private Sub BankId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles BankId.KeyUp
        dt = bm.ExecuteAdapter("select * from LinkFile where Id=" & MyLinkFile)
        If dt.Rows.Count > 0 AndAlso bm.ShowHelp(dt.Rows(0)("TableName"), BankId, BankName, e, "select cast(Id as varchar(100)) Id,Name from " & dt.Rows(0)("TableName")) Then
            BankId_LostFocus(Nothing, Nothing)
        End If
    End Sub



    Private Sub Canceled_Checked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Checked
        Value.Text = ""
        Value.IsEnabled = False
    End Sub

    Private Sub Canceled_Unchecked(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Canceled.Unchecked
        Value.IsEnabled = True
    End Sub

    Private Sub LoadResource()
        btnSave.SetResourceReference(ContentProperty, "Save")
        btnDelete.SetResourceReference(ContentProperty, "Delete")
        btnNew.SetResourceReference(ContentProperty, "New")

        btnFirst.SetResourceReference(ContentProperty, "First")
        btnNext.SetResourceReference(ContentProperty, "Next")
        btnPrevios.SetResourceReference(ContentProperty, "Previous")
        btnLast.SetResourceReference(ContentProperty, "Last")

        lblID.SetResourceReference(ContentProperty, "Id")

        lblBank.SetResourceReference(ContentProperty, "Bank")
        If MyLinkFile = 5 Then lblBank.SetResourceReference(ContentProperty, "Safe")

        lblDayDate.SetResourceReference(ContentProperty, "DayDate")
        lblValue.SetResourceReference(ContentProperty, "Value")
        lblNotes.SetResourceReference(ContentProperty, "Notes")

    End Sub


    Private Sub SupplierId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles SupplierId.KeyUp
        bm.ShowHelp("Suppliers", SupplierId, SupplierName, e, "select cast(Id as varchar(100)) Id,Name from Suppliers")
    End Sub

    Private Sub SupplierId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles SupplierId.LostFocus
        bm.LostFocus(SupplierId, SupplierName, "select Name from Suppliers where Id=" & SupplierId.Text.Trim())

        getSalesInvoiceNo()

    End Sub


    Public Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        Dim str As String = ""
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim() & str)

        getSalesInvoiceNo()

    End Sub

    Sub getSalesInvoiceNo()
        bm.FillCombo("select T.InvoiceNo Id,cast(T.InvoiceNo as nvarchar(100))+' - '+dbo.ToStrDate(T.DayDate)+' - ' +CAST((T.Total-ISNULL(sum(TT.MainValue),0)) as nvarchar(100))+' '+C.Sign Name from SalesMaster T left join BankCash0 TT on(T.StoreId=TT.StoreId and T.InvoiceNo=TT.SalesInvoiceNo) left join Currencies C on(C.Id=T.CurrencyId) where T.Flag=19 and T.StoreId=" & Val(StoreId.Text) & " and T.ToId=" & Val(SupplierId.Text) & " group by T.InvoiceNo,T.DayDate,T.Total,C.Sign union select 0,'-' order by T.InvoiceNo", SalesInvoiceNo)
    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        Dim str As String = " where 1=1 "
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")" & str) Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Private Sub MainValue_TextChanged(sender As Object, e As TextChangedEventArgs) Handles MainValue.TextChanged, Exchange.TextChanged
        Value.Text = bm.Rnd(Val(MainValue.Text) * Val(Exchange.Text))
    End Sub
End Class
