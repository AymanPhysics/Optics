Imports System.Data
Imports System.IO

Public Class RPT36
    Dim bm As New BasicMethods
    Public Flag As Integer = 1
    Dim dt As New DataTable

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click
        If Flag = 3 OrElse Flag = 4 Then
            If Val(StoreId.Text) = 0 Then
                bm.ShowMSG("برجاء تحديد المخزن")
                StoreId.Focus()
                Return
            End If
        End If

        Dim rpt As New ReportViewer
        rpt.paraname = New String() {"@EntryTypeId", "@StoreId", "@FromDate", "@ToDate", "@FromInvoiceNo", "@ToInvoiceNo", "Header", "@UnDeliveredOnly"}
        rpt.paravalue = New String() {Val(EntryTypeId.SelectedValue), Val(StoreId.Text), FromDate.SelectedDate, ToDate.SelectedDate, Val(FromInvoice.Text), Val(ToInvoice.Text), CType(Parent, Page).Title, IIf(UnDeliveredOnly.IsChecked, 1, 0)}

        Select Case Flag
            Case 1
                rpt.Rpt = "EntryMainAll.rpt"
                If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.Optics Then
                    rpt.Rpt = "EntryAll.rpt"
                End If
            Case 2
                rpt.Rpt = "DeliveryCustomers3.rpt"
            Case 3
                SaveSalesToFile()
                Return
            Case 4
                GetSalesFromFile()
                Return
        End Select
        rpt.Show()
    End Sub

    Sub SaveSalesToFile()
        Try
            bm.ExecuteNonQuery("sp_configure 'show advanced options', '1' RECONFIGURE")
            bm.ExecuteNonQuery("sp_configure 'xp_cmdshell', '1' RECONFIGURE")

            Dim dialog As New System.Windows.Forms.FolderBrowserDialog()
            Dim Result As Forms.DialogResult = dialog.ShowDialog()
            If Result = Forms.DialogResult.OK Then
                Dim MyPath As String = dialog.SelectedPath & IIf(dialog.SelectedPath.EndsWith("\"), "", "\") & "Sales" & bm.MyGetDateTime.Replace(":", "").Replace(" ", "_") & ".xml"

                bm.ExecuteScalar("EXEC xp_cmdshell 'bcp ""EXEC " & con.Database & ".dbo.GenerateSalesXML 13," & Val(StoreId.Text) & "," & Val(FromInvoice.Text) & "," & Val(ToInvoice.Text) & """ queryout " & MyPath & " -w -T'")
                Process.Start("explorer.exe", "/select, " + MyPath)
            End If
        Catch ex As Exception
        End Try
    End Sub
    Sub GetSalesFromFile()
        Dim OFD As New System.Windows.Forms.OpenFileDialog
        OFD.Filter = "Sales (*.xml)|*.xml|All files (*.*)|*.*"
        If OFD.ShowDialog = Windows.Forms.DialogResult.OK Then
            Dim st As New StreamReader(OFD.FileName, True)
            Dim XML As String = st.ReadToEnd
            XML = XML.Replace(vbCrLf, "")
            st.Close()
            If bm.ExecuteNonQuery("GetSalesFromXML", {"Flag", "StoreId", "XML"}, {13, Val(StoreId.Text), XML}) Then
                bm.ShowMSG("تم استيراد المبيعات من ملف")
            End If
        End If
        Return
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        bm.Addcontrol_MouseDoubleClick({StoreId})

        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()

        If Flag = 1 Then
            lblStoreId.Visibility = Windows.Visibility.Hidden
            StoreId.Visibility = Windows.Visibility.Hidden
            StoreName.Visibility = Windows.Visibility.Hidden

            UnDeliveredOnly.Visibility = Windows.Visibility.Hidden
        ElseIf Flag = 3 Then

            lblMain.Visibility = Windows.Visibility.Hidden
            EntryTypeId.Visibility = Windows.Visibility.Hidden
            lblFromDate.Visibility = Windows.Visibility.Hidden
            FromDate.Visibility = Windows.Visibility.Hidden
            lblToDate.Visibility = Windows.Visibility.Hidden
            ToDate.Visibility = Windows.Visibility.Hidden
            UnDeliveredOnly.Visibility = Windows.Visibility.Hidden
        ElseIf Flag = 4 Then

            lblMain.Visibility = Windows.Visibility.Hidden
            EntryTypeId.Visibility = Windows.Visibility.Hidden
            lblFromDate.Visibility = Windows.Visibility.Hidden
            FromDate.Visibility = Windows.Visibility.Hidden
            lblToDate.Visibility = Windows.Visibility.Hidden
            ToDate.Visibility = Windows.Visibility.Hidden
            UnDeliveredOnly.Visibility = Windows.Visibility.Hidden

            lblFromId.Visibility = Visibility.Hidden
            FromInvoice.Visibility = Visibility.Hidden
            lblToId.Visibility = Visibility.Hidden
            ToInvoice.Visibility = Visibility.Hidden
        Else
            lblMain.Visibility = Windows.Visibility.Hidden
            EntryTypeId.Visibility = Windows.Visibility.Hidden
        End If

        bm.FillCombo("GetEmpEntryTypes @Flag=" & 4 & ",@EmpId=" & Md.UserName & "", EntryTypeId)



        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, 1, 1, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        If Md.RptFromToday Then FromDate.SelectedDate = ToDate.SelectedDate

    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")
        'lblFromId.SetResourceReference(ContentProperty, "From Id")
        'lblToId.SetResourceReference(ContentProperty, "To Id")
        lblFromDate.SetResourceReference(ContentProperty, "From Date")
        lblToDate.SetResourceReference(ContentProperty, "To Date")
        'lblLinkFile.SetResourceReference(ContentProperty, "LinkFile")
        'lblSubAccNo.SetResourceReference(ContentProperty, "Sub AccNo")
        'lblBank.SetResourceReference(ContentProperty, "Bank") 

    End Sub

    Private Sub StoreId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles StoreId.KeyUp
        If bm.ShowHelp("Stores", StoreId, StoreName, e, "select cast(Id as varchar(100)) Id,Name from Fn_EmpStores(" & Md.UserName & ")") Then
            StoreId_LostFocus(StoreId, Nothing)
        End If
    End Sub

    Private Sub StoreId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles StoreId.LostFocus
        bm.LostFocus(StoreId, StoreName, "select Name from Fn_EmpStores(" & Md.UserName & ") where Id=" & StoreId.Text.Trim())
    End Sub


End Class