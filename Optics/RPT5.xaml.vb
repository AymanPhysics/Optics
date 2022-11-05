Imports System.Data

Public Class RPT5
    Dim bm As New BasicMethods
    Public Flag As Integer = 0
    Public All As Boolean = False
    Public Summarized As Boolean = False

    Public Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles Button2.Click

        If EmpId.SelectedValue Is Nothing Then EmpId.SelectedIndex = 0
        If CsId.SelectedValue Is Nothing Then CsId.SelectedIndex = 0
        If VisitingType.SelectedValue Is Nothing Then VisitingType.SelectedIndex = 0
        If UserId.SelectedValue Is Nothing Then UserId.SelectedIndex = 0
        If SerialType.SelectedValue Is Nothing Then SerialType.SelectedIndex = 0
        If ServiceGroupId.SelectedValue Is Nothing Then ServiceGroupId.SelectedIndex = 0
        If ServiceTypeId.SelectedValue Is Nothing Then ServiceTypeId.SelectedIndex = 0

        Dim rpt As New ReportViewer
        If EmpId.SelectedValue Is Nothing Then EmpId.SelectedIndex = 0
        If UserId.SelectedValue Is Nothing Then EmpId.SelectedIndex = 0
        rpt.paraname = New String() {"@EmpId", "@CaseId", "@ServiceGroupId", "@ServiceTypeId", "@CsId", "@VisitingType", "@FromDate", "@ToDate", "@FromId", "@ToId", "Header", "@UserId", "@FromSerialId", "@ToSerialId", "@Canceled", "@SerialType", "@All", "@IsReservations", "@IsServices", "SerialTypeName", "@Returned", "@CompanyId", "@DepartmentId", "@All", "@FromHH", "@FromMM", "@ToHH", "@ToMM", "@ShowZeroValues"}
        rpt.paravalue = New String() {Val(EmpId.SelectedValue), Val(CaseId.Text), ServiceGroupId.SelectedValue.ToString(), ServiceTypeId.SelectedValue.ToString(), CsId.SelectedValue.ToString(), Val(VisitingType.SelectedValue.ToString), FromDate.SelectedDate, ToDate.SelectedDate, Val(FromInvoice.Text), Val(ToInvoice.Text), CType(Parent, Page).Title, Val(UserId.SelectedValue), Val(FromSerialId.Text), Val(ToSerialId.Text), Canceled.SelectedIndex.ToString, SerialType.SelectedValue.ToString, 0, IIf(CheckReservations.IsChecked, 1, 0), IIf(CheckServices.IsChecked, 1, 0), SerialType.Text, Returned.SelectedIndex.ToString, Val(CompanyId.Text), Val(DepartmentId.Text), IIf(All, 1, 0), FromHH.SelectedValue, FromMM.SelectedValue, ToHH.SelectedValue, ToMM.SelectedValue, IIf(Md.MyProjectType = ProjectType.X2, 1, 1)}

        Select Case Flag
            Case 1
                rpt.Rpt = "Invoices.rpt"
                If Md.MyProjectType = ProjectType.X8 Then
                    If Not Md.Manager AndAlso EmpId.SelectedIndex = 0 AndAlso Val(DepartmentId.Text) = 0 AndAlso Val(CompanyId.Text) = 0 AndAlso Val(CaseId.Text) = 0 Then
                        bm.ShowMSG("برجاء تحديد أحد الخيارات")
                        Return
                    End If
                    rpt.Rpt = "InvoicesZohor.rpt"
                End If
            Case 2
                rpt.Rpt = "InvoicesPatient.rpt"
            Case 3
                rpt.Rpt = "InvoicesLab.rpt"
            Case 4
                rpt.Rpt = "Invoices2.rpt"
                If Md.MyProjectType = ProjectType.X8 Then
                    rpt.Rpt = "Invoices22.rpt"
                End If
            Case 5
                rpt.Rpt = "InvoicesCompany.rpt"
                If All Then rpt.Rpt = "InvoicesCompany2.rpt"
                If Summarized Then rpt.Rpt = "InvoicesCompany3.rpt"
            Case 6
                rpt.Rpt = "InvoicesExternalDoctors.rpt"
        End Select
        rpt.Show()
    End Sub

    Private Sub UserControl_Loaded(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles MyBase.Loaded
        If bm.TestIsLoaded(Me, True) Then Return
        LoadResource()
        bm.Addcontrol_MouseDoubleClick({CaseId, CompanyId, DepartmentId})
        bm.FillCombo("SerialTypes", SerialType, "")
        bm.FillCombo("HoursAll", FromHH, "", , True, True)
        bm.FillCombo("HoursAll", ToHH, "", , True, True)
        bm.FillCombo("Minutes", FromMM, "", , True, True)
        bm.FillCombo("Minutes", ToMM, "", , True, True)

        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where SystemUser='1' and Stopped='0' union select 0 Id,'-' Name order by Name", UserId)
        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where Doctor='1' and Stopped='0' union select 0 Id,'-' Name order by Name", EmpId)

        bm.FillCombo("select Id," & Resources.Item("CboName") & " Name from Employees where Nurse='1' and Stopped='0' union select 0 Id,'-' Name order by Name", CsId)

        CheckReservations.IsChecked = True
        CheckServices.IsChecked = True

        If Md.MyProjectType = ProjectType.X9 Then
            CheckServices.IsChecked = False
            CheckServices.Visibility = Windows.Visibility.Hidden
        End If

        'If EmpId.Visibility = Windows.Visibility.Visible Then EmpId.SelectedValue = Md.UserName
        Canceled.SelectedIndex = 2
        Returned.SelectedIndex = 2
        Dim MyNow As DateTime = bm.MyGetDate()
        FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)
        ToDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, MyNow.Day, 0, 0, 0)

        If Flag = 4 AndAlso Md.MyProjectType = ProjectType.X8 Then
            FromDate.SelectedDate = New DateTime(MyNow.Year, MyNow.Month, 1, 0, 0, 0)
        End If

        If Flag = 3 OrElse Flag = 5 Then
            EmpId.SelectedValue = 0
            UserId.SelectedValue = 0
            If Flag = 3 Then
                CheckReservations.IsChecked = False
                ServiceGroupId.SelectedValue = bm.ExecuteScalar("select top 1 LabServiceGroupId from Statics")
            End If
            
            lblDoctor.Visibility = Windows.Visibility.Hidden
            EmpId.Visibility = Windows.Visibility.Hidden
            lblPatient.Visibility = Windows.Visibility.Hidden
            CaseId.Visibility = Windows.Visibility.Hidden
            CaseName.Visibility = Windows.Visibility.Hidden

            If Flag <> 5 Then
                lblCompanyId.Visibility = Windows.Visibility.Hidden
                CompanyId.Visibility = Windows.Visibility.Hidden
                CompanyName.Visibility = Windows.Visibility.Hidden
            End If
            
            lblDepartmentId.Visibility = Windows.Visibility.Hidden
            DepartmentId.Visibility = Windows.Visibility.Hidden
            DepartmentName.Visibility = Windows.Visibility.Hidden
            lblServiceGroup.Visibility = Windows.Visibility.Hidden
            ServiceGroupId.Visibility = Windows.Visibility.Hidden
            lblServiceType.Visibility = Windows.Visibility.Hidden
            ServiceTypeId.Visibility = Windows.Visibility.Hidden
            lblCS.Visibility = Windows.Visibility.Hidden
            CsId.Visibility = Windows.Visibility.Hidden
            lblVisitingType.Visibility = Windows.Visibility.Hidden
            VisitingType.Visibility = Windows.Visibility.Hidden
            lblFromResId.Visibility = Windows.Visibility.Hidden
            FromInvoice.Visibility = Windows.Visibility.Hidden
            lblToResId.Visibility = Windows.Visibility.Hidden
            ToInvoice.Visibility = Windows.Visibility.Hidden
            lblUsername.Visibility = Windows.Visibility.Hidden
            UserId.Visibility = Windows.Visibility.Hidden
            lblFromSerialId.Visibility = Windows.Visibility.Hidden
            FromSerialId.Visibility = Windows.Visibility.Hidden
            lblToSerialId.Visibility = Windows.Visibility.Hidden
            ToSerialId.Visibility = Windows.Visibility.Hidden
            lblSerialType.Visibility = Windows.Visibility.Hidden
            SerialType.Visibility = Windows.Visibility.Hidden
            CheckReservations.Visibility = Windows.Visibility.Hidden
            CheckServices.Visibility = Windows.Visibility.Hidden
            lblSerialType.Visibility = Windows.Visibility.Hidden
            SerialType.Visibility = Windows.Visibility.Hidden
        End If
    End Sub

    Private Sub SerialType_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles SerialType.SelectionChanged
        Try
            bm.FillCombo("VisitingTypes", VisitingType, "where (SerialId=" & SerialType.SelectedValue.ToString & " or " & SerialType.SelectedValue.ToString & "=0)")
        Catch ex As Exception
        End Try
        Try
            bm.FillCombo("ServiceGroups", ServiceGroupId, "where (SerialId=" & SerialType.SelectedValue.ToString & " or " & SerialType.SelectedValue.ToString & "=0)")
        Catch ex As Exception
        End Try
    End Sub

    Private Sub ServiceGroupId_SelectionChanged(sender As Object, e As SelectionChangedEventArgs) Handles ServiceGroupId.SelectionChanged
        Try
            bm.FillCombo("ServiceTypes", ServiceTypeId, " Where ServiceGroupId=" & Val(ServiceGroupId.SelectedValue.ToString))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub CaseId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CaseId.KeyUp
        If bm.ShowHelpCases(CaseId, CaseName, e) Then
            CaseID_LostFocus(sender, Nothing)
        End If
    End Sub

    Private Sub CaseID_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CaseId.LostFocus
        bm.LostFocus(CaseId, CaseName, "select EnName Name from Cases where Id=" & CaseId.Text.Trim())
    End Sub

    Private Sub LoadResource()
        Button2.SetResourceReference(ContentProperty, "View Report")

        CheckReservations.SetResourceReference(ContentProperty, "Reservations")
        CheckServices.SetResourceReference(ContentProperty, "Services")

        lblVisitingType.SetResourceReference(ContentProperty, "VisitingType")
        lblServiceGroup.SetResourceReference(ContentProperty, "ServiceGroupId")
        lblServiceType.SetResourceReference(ContentProperty, "ServiceTypeId")
        lblFromDate.SetResourceReference(ContentProperty, "From Date")
        lblToDate.SetResourceReference(ContentProperty, "To Date")
        lblDoctor.SetResourceReference(ContentProperty, "Doctor")
        lblCS.SetResourceReference(ContentProperty, "C. S.")
        lblFromResId.SetResourceReference(ContentProperty, "From Res. Id")
        lblToResId.SetResourceReference(ContentProperty, "To Res. Id")
        lblPatient.SetResourceReference(ContentProperty, "Patient")
        lblUsername.SetResourceReference(ContentProperty, "Username")
        lblFromSerialId.SetResourceReference(ContentProperty, "From Serial Id")
        lblToSerialId.SetResourceReference(ContentProperty, "To Serial Id")
        lblCanceled.SetResourceReference(ContentProperty, "Canceled")
        lblReturned.SetResourceReference(ContentProperty, "Returned")
        lblSerialType.SetResourceReference(ContentProperty, "Serial Type")
        lblCompanyId.SetResourceReference(ContentProperty, "Company")

        bm.ResetComboboxContent(Canceled)
        bm.ResetComboboxContent(Returned)
    End Sub

    Private Sub CheckReservations_Checked(sender As Object, e As RoutedEventArgs) Handles CheckReservations.Checked
        lblVisitingType.Visibility = Windows.Visibility.Visible
        VisitingType.Visibility = Windows.Visibility.Visible
    End Sub
    Private Sub CheckReservations_UnChecked(sender As Object, e As RoutedEventArgs) Handles CheckReservations.Unchecked
        lblVisitingType.Visibility = Windows.Visibility.Hidden
        VisitingType.Visibility = Windows.Visibility.Hidden
    End Sub

    Private Sub CheckServices_Checked(sender As Object, e As RoutedEventArgs) Handles CheckServices.Checked
        lblServiceGroup.Visibility = Windows.Visibility.Visible
        ServiceGroupId.Visibility = Windows.Visibility.Visible
        lblServiceType.Visibility = Windows.Visibility.Visible
        ServiceTypeId.Visibility = Windows.Visibility.Visible
        lblCS.Visibility = Windows.Visibility.Visible
        CsId.Visibility = Windows.Visibility.Visible
    End Sub

    Private Sub CheckServices_UnChecked(sender As Object, e As RoutedEventArgs) Handles CheckServices.Unchecked
        lblServiceGroup.Visibility = Windows.Visibility.Hidden
        ServiceGroupId.Visibility = Windows.Visibility.Hidden
        lblServiceType.Visibility = Windows.Visibility.Hidden
        ServiceTypeId.Visibility = Windows.Visibility.Hidden
        lblCS.Visibility = Windows.Visibility.Hidden
        CsId.Visibility = Windows.Visibility.Hidden
    End Sub

    Private Sub CompanyId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles CompanyId.KeyUp
        bm.ShowHelp("Companies", CompanyId, CompanyName, e, "select cast(Id as varchar(100)) Id,Name from Companies")
    End Sub

    Private Sub CompanyId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles CompanyId.LostFocus
        bm.LostFocus(CompanyId, CompanyName, "select Name from Companies where Id=" & CompanyId.Text.Trim())
    End Sub

    Private Sub DepartmentId_LostFocus(ByVal sender As System.Object, ByVal e As System.Windows.RoutedEventArgs) Handles DepartmentId.LostFocus
        bm.LostFocus(DepartmentId, DepartmentName, "select Name from Departments where Id=" & DepartmentId.Text.Trim())
    End Sub

    Private Sub DepartmentId_KeyDown(ByVal sender As Object, ByVal e As System.Windows.Input.KeyEventArgs) Handles DepartmentId.KeyUp
        bm.ShowHelp("Departments", DepartmentId, DepartmentName, e, "select cast(Id as varchar(100)) Id,Name from Departments", "Departments")
    End Sub

End Class