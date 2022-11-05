' Copyright © Microsoft Corporation.  All Rights Reserved.
' This code released under the terms of the 
' Microsoft Public License (MS-PL, http://opensource.org/licenses/ms-pl.html.)

Imports System.Text
Imports System.Windows.Media.Animation
Imports System.IO
Imports System.Windows.Threading
Imports System.Data
Imports System.Xml
Imports System.IO.Ports
Imports System.Threading

Partial Public Class MainPage
    Inherits Page
    Public NLevel As Boolean = False
    Dim m As MainWindow = Application.Current.MainWindow
    Dim bm As New BasicMethods
    WithEvents t As New DispatcherTimer With {.IsEnabled = True, .Interval = New TimeSpan(0, 0, 1)}

    Private sampleGridOpacityAnimation As DoubleAnimation
    Private sampleGridTranslateTransformAnimation As DoubleAnimation
    Private borderTranslateDoubleAnimation As DoubleAnimation

    Public Sub New()
        InitializeComponent()

        Dim widthBinding As New Binding("ActualWidth")
        widthBinding.Source = Me

        sampleGridOpacityAnimation = New DoubleAnimation()
        sampleGridOpacityAnimation.To = 0
        sampleGridOpacityAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.15))

        sampleGridTranslateTransformAnimation = New DoubleAnimation()
        sampleGridTranslateTransformAnimation.BeginTime = TimeSpan.FromSeconds(0.15)
        sampleGridTranslateTransformAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.15))

        borderTranslateDoubleAnimation = New DoubleAnimation()
        borderTranslateDoubleAnimation.Duration = New Duration(TimeSpan.FromSeconds(0.3))
        borderTranslateDoubleAnimation.BeginTime = TimeSpan.FromSeconds(0)

    End Sub
    Private Shared _packUri As New Uri("pack://application:,,,/")

    Private Sub btnBack_Click(sender As Object, e As RoutedEventArgs) Handles btnBack.Click
        borderTranslateDoubleAnimation.From = 0
        borderTranslateDoubleAnimation.To = -ActualWidth
        SampleDisplayBorderTranslateTransform.BeginAnimation(TranslateTransform.XProperty, borderTranslateDoubleAnimation)
        GridSampleViewer_Loaded(Nothing, Nothing)
        Md.Currentpage = ""
    End Sub

    Private Sub selectedSampleChanged(ByVal sender As Object, ByVal args As RoutedEventArgs)
        If TypeOf args.Source Is RadioButton Then
            Dim theButton As RadioButton = CType(args.Source, RadioButton)

            Dim theFrame
            If TypeOf theButton.Tag Is MyPage Then
                theFrame = CType(theButton.Tag, MyPage)
                If Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag) = "" Then
                    theFrame.Title = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Else
                    theFrame.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag)
                End If
                If Not Md.MyProjectType = ProjectType.PCs Then
                    CType(theButton.Tag, MyPage).MySecurityType.AllowEdit = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowEdit") = 1
                    CType(theButton.Tag, MyPage).MySecurityType.AllowDelete = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowDelete") = 1
                    CType(theButton.Tag, MyPage).MySecurityType.AllowNavigate = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowNavigate") = 1
                    CType(theButton.Tag, MyPage).MySecurityType.AllowPrint = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowPrint") = 1
                End If
            ElseIf TypeOf theButton.Tag Is Window Then
                theFrame = CType(theButton.Tag, MyWindow)
                If Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag) = "" Then
                    theFrame.Title = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Else
                    theFrame.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag)
                End If
                If Not Md.MyProjectType = ProjectType.PCs Then
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowEdit = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowEdit") = 1
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowDelete = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowDelete") = 1
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowNavigate = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowNavigate") = 1
                    CType(theButton.Tag, MyWindow).MySecurityType.AllowPrint = dtLevelsMenuitems.Select("Id=" & theButton.Name.Replace("menuitem", "")).ToList(0)("AllowPrint") = 1
                End If
            End If

            theButton.IsTabStop = False
            CType(args.Source, RadioButton).IsChecked = False

            If TypeOf theButton.Tag Is Window Then
                CType(theFrame, Window).Show()
                CType(theFrame, Window).WindowState = WindowState.Minimized
                CType(theFrame, Window).WindowState = WindowState.Maximized
            ElseIf m.layoutSwitcher.SelectedIndex = 1 Then
                Dim frm As New MyWindow
                If Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag) = "" Then
                    frm.Title = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Else
                    frm.Title = Resources.Item(CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag)
                End If
                frm.Content = theButton.Tag
                frm.WindowState = WindowState.Maximized
                frm.Show()
                frm.WindowState = WindowState.Minimized
                frm.WindowState = WindowState.Maximized
            Else
                SampleDisplayFrame.Content = theButton.Tag
                SampleDisplayBorder.Visibility = Visibility.Visible
                Try
                    theFrame.Tag = CType(CType(args.Source, RadioButton).Content, TranslateTextAnimationExample).RealText.Tag
                Catch ex As Exception
                End Try
                sampleDisplayFrameLoaded(theFrame, args)
            End If
        End If

    End Sub

    Private Sub sampleDisplayFrameLoaded(ByVal sender As Object, ByVal args As EventArgs)
        If TypeOf sender Is MyWindow Then
            Try
                If Resources.Item(CType(sender, MyWindow).Tag) = "" Then
                    CType(sender, MyWindow).Title = CType(sender, MyWindow).Tag
                Else
                    CType(sender, MyWindow).Title = Resources.Item(CType(sender, MyWindow).Tag)
                End If
                Md.Currentpage = CType(sender, MyWindow).Title
            Catch ex As Exception
            End Try
        ElseIf TypeOf sender Is Page Then
            Try
                If Resources.Item(CType(sender, Page).Tag) = "" Then
                    CType(sender, Page).Title = CType(sender, Page).Tag
                Else
                    CType(sender, Page).Title = Resources.Item(CType(sender, Page).Tag)
                End If
                Md.Currentpage = CType(sender, Page).Title
            Catch ex As Exception
            End Try
        ElseIf Not sender Is Nothing AndAlso TypeOf CType(sender, Frame).Content Is Page Then
            Try
                If Resources.Item(CType(CType(sender, Frame).Content, Page).Tag) = "" Then
                    CType(CType(sender, Frame).Content, Page).Title = CType(CType(sender, Frame).Content, Page).Tag
                Else
                    CType(CType(sender, Frame).Content, Page).Title = Resources.Item(CType(CType(sender, Frame).Content, Page).Tag)
                End If
                Md.Currentpage = CType(CType(sender, Frame).Content, Page).Title
            Catch ex As Exception
            End Try
            Try
                If Resources.Item(CType(sender, Page).Tag) = "" Then
                    CType(sender, Page).Title = CType(sender, Page).Tag
                Else
                    CType(sender, Page).Title = Resources.Item(CType(sender, Page).Tag)
                End If
                Md.Currentpage = CType(sender, Page).Title
            Catch ex As Exception
            End Try
        End If

        sampleGridTranslateTransformAnimation.To = -ActualWidth
        borderTranslateDoubleAnimation.From = -ActualWidth
        borderTranslateDoubleAnimation.To = 0

        SampleDisplayBorder.Visibility = Visibility.Visible
        SampleGrid.BeginAnimation(Grid.OpacityProperty, sampleGridOpacityAnimation)
        SampleGridTranslateTransform.BeginAnimation(TranslateTransform.XProperty, sampleGridTranslateTransformAnimation)
        SampleDisplayBorderTranslateTransform.BeginAnimation(TranslateTransform.XProperty, borderTranslateDoubleAnimation)
    End Sub

    Private Sub galleryLoaded(ByVal sender As Object, ByVal args As RoutedEventArgs)
        If bm.TestIsLoaded(Me, True) Then Return
        tab.Margin = New Thickness(0)
        tab.HorizontalAlignment = HorizontalAlignment.Stretch
        tab.VerticalAlignment = VerticalAlignment.Stretch
        'tab.Style = FindResource("TabControlLeftStyle")
        'tab.Style = FindResource("OutlookTabControlStyle")

        Load()

        SampleDisplayBorderTranslateTransform.X = -ActualWidth
        SampleDisplayBorder.Visibility = Visibility.Hidden
    End Sub

    Private Sub pageSizeChanged(ByVal sender As Object, ByVal args As SizeChangedEventArgs)
        SampleDisplayBorderTranslateTransform.X = Me.ActualWidth
    End Sub

    Dim DynamicMenuitem As Integer = 0
    Dim DtCurrentMenuitem As New DataTable With {.TableName = "T"}
    Sub TestCurrentMenuitem(CurrentMenuitem As Integer)
        If DtCurrentMenuitem.Columns.Count = 0 Then DtCurrentMenuitem.Columns.Add("C")
        If DtCurrentMenuitem.Select("C=" & CurrentMenuitem).Length > 0 AndAlso Not Md.MyProjectType = ProjectType.PCs Then MessageBox.Show(CurrentMenuitem)
        DtCurrentMenuitem.Rows.Add(CurrentMenuitem)
    End Sub
    Sub LoadLabel(CurrentMenuitem As Integer, ByVal G As WrapPanel, Ttl As String)
        TestCurrentMenuitem(CurrentMenuitem)

        For i As Integer = 0 To m.langSwitcher.Items.Count - 1
            Try
                If TryCast(TryCast(m.langSwitcher.Items(i), XmlElement).Attributes("Visibility"), XmlAttribute).Value = "2" Then Continue For
                Dim rd As ResourceDictionary = Md.MyDictionaries.Items(i)
                While rd.Item(Ttl).Length < 16
                    rd.Item(Ttl) = " " & rd.Item(Ttl) & " "
                End While
            Catch ex As Exception
            End Try
        Next

        Dim lbl0 As New Label With {.Height = ActualHeight, .Margin = New Windows.Thickness(24, 0, 0, 0)}
        G.Children.Add(lbl0)

        Dim lbl As New Label With {.Name = "menuitem" & CurrentMenuitem, .FontFamily = New System.Windows.Media.FontFamily("khalaad al-arabeh 2"), .FontSize = 30, .HorizontalContentAlignment = Windows.HorizontalAlignment.Center, .Foreground = New SolidColorBrush(Color.FromArgb(255, 9, 103, 168)), .FontWeight = FontWeight.FromOpenTypeWeight(1), .Height = 90}
        lbl.SetResourceReference(ContentProperty, Ttl)

        If Application.Current.MainWindow.Resources.Item(Ttl) = "" Then
            lbl.Content = Ttl
        End If

        G.Children.Add(lbl)
        'If Md.MyProjectType = ProjectType.NawarGroup OrElse Md.MyProjectType = ProjectType.Hamido Then
        lbl.Width = 240
        lbl.Height = 70
        lbl.FontSize = 24
        'End If

        If Ttl = "" Then lbl.Height = 0


        If Not Lvl Then
            Dim it As New MenuItem With {.Header = "-----------------", .Name = "NewMenuItemSub" & CurrentMenuitem}
            it.Visibility = Windows.Visibility.Collapsed
            CType(m.MyMenu.Items(m.MyMenu.Items.Count - 1), MenuItem).Items.Add(it)

            Dim it2 As New MenuItem With {.Header = "-----------------", .Name = "NewMenuItemSub" & CurrentMenuitem}
            it2.IsEnabled = False
            CType(m.MyMenu.Items(m.MyMenu.Items.Count - 1), MenuItem).Items.Add(it2)
        End If
    End Sub

    Function LoadRadio(CurrentMenuitem As Integer, ByVal G As WrapPanel, ByVal Ttl As String) As RadioButton
        TestCurrentMenuitem(CurrentMenuitem)

        For i As Integer = 0 To m.langSwitcher.Items.Count - 1
            Try
                If TryCast(TryCast(m.langSwitcher.Items(i), XmlElement).Attributes("Visibility"), XmlAttribute).Value = "2" Then Continue For
                Dim rd As ResourceDictionary = Md.MyDictionaries.Items(i)
                While rd.Item(Ttl).Length < 16
                    rd.Item(Ttl) = " " & rd.Item(Ttl) & " "
                End While
            Catch ex As Exception
            End Try
        Next

        Dim RName As String = "menuitem" & CurrentMenuitem
        Dim r As New RadioButton With {.Name = RName, .Style = Application.Current.FindResource("GlassRadioButtonStyle"), .Width = 180, .Height = 90}
        'r.Tag = New Page With {.Content = frm}
        'If Md.MyProjectType = ProjectType.NawarGroup OrElse Md.MyProjectType = ProjectType.Hamido Then
        r.Width = 140
        r.Height = 70
        'End If

        Dim t As New TranslateTextAnimationExample
        t.RealText.Tag = Ttl
        t.RealText.SetResourceReference(TextBlock.TextProperty, Ttl)

        If Application.Current.MainWindow.Resources.Item(Ttl) = "" Then
            t.RealText.Text = Ttl
        End If

        r.SetResourceReference(RadioButton.BackgroundProperty, "SC")
        t.SetResourceReference(RadioButton.BackgroundProperty, "SC")

        r.Content = t
        G.Children.Add(r)

        r.SetResourceReference(RadioButton.ToolTipProperty, Ttl)

        If Application.Current.MainWindow.Resources.Item(Ttl) = "" Then
            r.ToolTip = Ttl
        End If


        If Not Lvl Then
            Dim it As New MenuItem With {.Header = Ttl, .Name = "NewMenuItemSub" & CurrentMenuitem}
            it.Tag = r
            it.SetResourceReference(MenuItem.HeaderProperty, Ttl)
            CType(m.MyMenu.Items(m.MyMenu.Items.Count - 1), MenuItem).Items.Add(it)
            AddHandler it.Click, AddressOf it_Click
        End If
        Return r
    End Function

    Private Sub it_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim x As RadioButton = CType(sender.Tag, RadioButton)
            x.RaiseEvent(New RoutedEventArgs(RadioButton.CheckedEvent))
        Catch ex As Exception
        End Try
    End Sub

    Private Sub GridSampleViewer_Loaded(sender As Object, e As RoutedEventArgs) Handles Me.Loaded
        bm.TestIsLoaded(Me)
    End Sub

    Private Sub ResizeHeader(G As WrapPanel)
        If Lvl Then Return
        Dim Ttl As String = CType(CType(G.Parent, ScrollViewer).Parent, TabItem).Header
        Try
            While Md.DictionaryCurrent.Item(Ttl).Length < 16
                Md.DictionaryCurrent.Item(Ttl) = " " & Md.DictionaryCurrent.Item(Ttl) & " "
            End While
        Catch
        End Try
    End Sub


    Public Lvl As Boolean = False
    Public Sub Load()

        If MyProjectType = ProjectType.PCs Then
            LoadGPCs(0)
            Return
        End If

        LoadTabs()

        If Not Lvl Then
            dtLevelsMenuitems = bm.ExecuteAdapter("select * from LevelsMenuitems where LevelId=" & Md.LevelId)
            Dim dtLevelsTabs As DataTable = bm.ExecuteAdapter("select * from LevelsTabs where LevelId=" & Md.LevelId)
            If dtLevelsMenuitems.Rows.Count = 0 Then Application.Current.Shutdown()

            For i As Integer = 0 To tab.Items.Count - 1
                Dim item As TabItem = CType(tab.Items(i), TabItem)

                If dtLevelsTabs.Select("Id=" & tab.Items(i).Name.ToString.Replace("tab", "")).Length = 0 Then
                    item.Visibility = Windows.Visibility.Collapsed
                    CType(m.MyMenu.Items(i), MenuItem).Visibility = Windows.Visibility.Collapsed
                End If
                item.Content.Visibility = item.Visibility

                For x As Integer = 0 To CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children.Count - 1
                    If CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x).GetType = GetType(RadioButton) Then
                        Dim t As RadioButton = CType(CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x), RadioButton)
                        If dtLevelsMenuitems.Select("Id=" & t.Name.ToString.Replace("menuitem", "")).Length = 0 Then
                            t.Visibility = Windows.Visibility.Collapsed
                            CType(CType(m.MyMenu.Items(i), MenuItem).Items(x), MenuItem).Visibility = Windows.Visibility.Collapsed
                        End If
                    ElseIf CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x).GetType = GetType(Label) Then
                        Dim t As Label = CType(CType(CType(item.Content, ScrollViewer).Content, WrapPanel).Children(x), Label)
                        If t.Name = "" Then
                            t.Visibility = Windows.Visibility.Visible
                        ElseIf dtLevelsMenuitems.Select("Id=" & t.Name.ToString.Replace("menuitem", "")).Length = 0 Then
                            t.Visibility = Windows.Visibility.Collapsed
                            CType(CType(m.MyMenu.Items(i), MenuItem).Items(x), MenuItem).Visibility = Windows.Visibility.Collapsed
                        End If
                    End If
                Next
            Next

            For i As Integer = 0 To tab.Items.Count - 1
                If CType(tab.Items(i), TabItem).Visibility = Windows.Visibility.Visible Then
                    CType(tab.Items(i), TabItem).IsSelected = True
                    Exit For
                End If
            Next

        End If

    End Sub

    Function MakePanel(CurrentTab As Integer, MyHeader As String, ImagePath As String) As WrapPanel
        Dim SV As New MyScrollViewer
        bm.SetImage(SV.Img, ImagePath)
        Dim t As New TabItem With {.Content = SV, .Name = "tab" & CurrentTab, .Header = MyHeader, .Tag = MyHeader}

        'Template.ControlTemplate().Grid().Border().TextBlock()
        'FontFamily="khalaad al-arabeh 2" FontSize="12

        t.Style = FindResource("MyTabItem")
        't.Style = FindResource("OutlookTabItemStyle")
        't.Background = FindResource("OutlookButtonBackground")
        't.Foreground = FindResource("OutlookButtonForeground")

        tab.Items.Add(t)
        Dim G As WrapPanel = SV.MyWrapPanel
        G.Name = "MyWrapPanel" & CurrentTab
        G.AddHandler(System.Windows.Controls.Primitives.ToggleButton.CheckedEvent, New System.Windows.RoutedEventHandler(AddressOf Me.selectedSampleChanged))

        ResizeHeader(G)
        t.SetResourceReference(TabItem.HeaderProperty, t.Header)

        If Not Lvl Then
            Dim it As New MenuItem With {.Header = MyHeader, .MaxWidth = 150, .Name = "NewMenuItem" & CurrentTab}
            it.Tag = t
            it.SetResourceReference(MenuItem.HeaderProperty, MyHeader)
            m.MyMenu.Items.Add(it)
            AddHandler it.MouseEnter, AddressOf itm_Click
        End If

        Return G
    End Function

    Private Sub itm_Click(sender As Object, e As RoutedEventArgs)
        Try
            Dim x As TabItem = CType(sender.Tag, TabItem)
            x.Focus()
            x.IsSelected = True
            x.BringIntoView()
        Catch ex As Exception
        End Try
    End Sub

    Private Sub LoadGPCs(CurrentTab As Integer)
        Dim G As WrapPanel = MakePanel(CurrentTab, "File", "Omega.jpg")

        AddHandler LoadRadio(0, G, "PCs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       Dim frm As New BasicForm With {.TableName = "PCs"}
                                                       bm.SetImage(CType(frm, BasicForm).Img, "password.jpg")
                                                       frm.txtName.MaxLength = 1000
                                                       m.TabControl1.Items.Clear()
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub


        AddHandler LoadRadio(0, G, "فتح سنة مالية جديدة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       Dim frm As New CalcSalary With {.Flag = 11}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub



    End Sub

    Private Sub LoadGFile(CurrentTab As Integer)
        Dim s As String = "buttonscreen.jpg"
        Select Case Md.MyProjectType
            Case ProjectType.X8, ProjectType.X6, ProjectType.X9, ProjectType.X10, ProjectType.X11, ProjectType.X1, ProjectType.X2
                s = "buttonscreen.jpg"
            Case ProjectType.X16
                s = "buttonscreen3.jpg"
            Case ProjectType.X12
                s = "buttonscreen3.jpg"
            Case ProjectType.X13
                s = "Build2.jpg"
            Case Else
                s = "MainOMEGA.jpg"
                's = "Optics.jpeg"
        End Select

        Dim G As WrapPanel = MakePanel(CurrentTab, "File", s)
        Dim frm As UserControl

        AddHandler LoadRadio(101, G, "Employees").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               sender.Tag = New MyPage With {.Content = New Employees}
                                                           End Sub

        If Md.MyProjectType = ProjectType.X12 OrElse Md.MyProjectType = ProjectType.X13 Then
            AddHandler LoadRadio(102, G, "Drivers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New DriversSellers With {.TableName = "Drivers"}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
        End If

        If Not Md.MyProjectType = ProjectType.X9 AndAlso Not Md.MyProjectType = ProjectType.X10 AndAlso Not Md.MyProjectType = ProjectType.X7 Then
            AddHandler LoadRadio(103, G, "Countries").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New BasicForm With {.TableName = "Countries"}
                                                                   If Md.MyProjectType = ProjectType.X12 Then bm.SetImage(CType(frm, BasicForm).Img, "CustomerInvoicesItems.Jpg")
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

            AddHandler LoadRadio(104, G, "Cities").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm2 With {.MainTableName = "Countries", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "Country", .TableName = "Cities", .MainId = "CountryId", .SubId = "Id", .SubName = "Name"}

                                                                Select Case Md.MyProjectType
                                                                    Case ProjectType.X12
                                                                        bm.SetImage(CType(frm, BasicForm2).Img, "CustomerInvoicesItems.Jpg")
                                                                    Case Else
                                                                        bm.SetImage(CType(frm, BasicForm2).Img, "MainOMEGA.jpg")
                                                                End Select
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub


            AddHandler LoadRadio(105, G, "Areas").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New BasicForm3 With {.MainTableName = "Countries", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "Country", .Main2TableName = "Cities", .Main2MainId = "CountryId", .Main2SubId = "Id", .Main2SubName = "Name", .lblMain2_Content = "City", .TableName = "Areas", .MainId = "CountryId", .MainId2 = "CityId", .SubId = "Id", .SubName = "Name"}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        End If

        If Md.MyProjectType = ProjectType.X8 OrElse Md.MyProjectType = ProjectType.X6 OrElse Md.MyProjectType = ProjectType.X9 OrElse Md.MyProjectType = ProjectType.X11 OrElse Md.MyProjectType = ProjectType.X1 OrElse Md.MyProjectType = ProjectType.X2 OrElse Md.MyProjectType = ProjectType.X7 Then
            AddHandler LoadRadio(106, G, "Drugs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New BasicForm With {.TableName = "Drugs"}
                                                               bm.SetImage(CType(frm, BasicForm).Img, "drugs.jpg")
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

            AddHandler LoadRadio(107, G, "Doses").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New BasicForm With {.TableName = "Doses"}
                                                               bm.SetImage(CType(frm, BasicForm).Img, "doses.jpg")
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

            AddHandler LoadRadio(108, G, "Imaging").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BasicForm With {.TableName = "Rays"}
                                                                 bm.SetImage(CType(frm, BasicForm).Img, "ray.jpg")
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

            If Not Md.MyProjectType = ProjectType.X9 Then
                AddHandler LoadRadio(109, G, "Patient Jobs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New BasicForm With {.TableName = "Jobs"}
                                                                          bm.SetImage(CType(frm, BasicForm).Img, "jobs.jpg")
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub
            End If

        End If

        If Not Md.MyProjectType = ProjectType.X10 Then
            AddHandler LoadRadio(115, G, "Departments").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New BasicForm With {.TableName = "Departments"}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub
        End If

        AddHandler LoadRadio(116, G, "Attachment Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BasicForm With {.TableName = "AttachmentTypes"}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub



        If Md.ShowShifts Then
            AddHandler LoadRadio(128, G, "Shifts").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm With {.TableName = "Shifts"}
                                                                bm.SetImage(CType(frm, BasicForm).Img, "attchmenttype.jpg")
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub
        End If

        If Md.MyProjectType = ProjectType.X8 OrElse Md.MyProjectType = ProjectType.X2 OrElse Md.MyProjectType = ProjectType.X6 Then
            AddHandler LoadRadio(129, G, "Companies").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Companies
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        AddHandler LoadRadio(130, G, "Statics").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New Statics
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub


        If Md.MyProjectType = ProjectType.X8 OrElse Md.MyProjectType = ProjectType.X2 Then

            AddHandler LoadRadio(131, G, "ContactGroups").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New BasicForm With {.TableName = "ContactGroups"}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

            AddHandler LoadRadio(132, G, "ContactTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BasicForm2 With {.MainTableName = "ContactGroups", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "Group", .TableName = "ContactTypes", .MainId = "ContactGroupId", .SubId = "Id", .SubName = "Name"}
                                                                      bm.SetImage(CType(frm, BasicForm2).Img, "MainOMEGA.jpg")
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(133, G, "Contacts").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New Contacts
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub
        End If

        If Md.MyProjectType = ProjectType.X8 Then
            AddHandler LoadRadio(134, G, "CallCenterCategories").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New BasicForm With {.TableName = "CallCenterCategories"}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

            AddHandler LoadRadio(135, G, "CallCenter").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New CallCenter
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If


        If Md.MyProjectType = ProjectType.X9 Then

            LoadLabel(141, G, "Online")

            AddHandler LoadRadio(136, G, "Infertirity").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New BasicForm With {.TableName = "Infertirity", .IsMultiLine = True, .WithImage = True}
                                                                     bm.SetImage(CType(frm, BasicForm).Img, "attchmenttype.jpg")
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

            AddHandler LoadRadio(137, G, "Pregnancy").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New BasicForm With {.TableName = "Pregnancy", .IsMultiLine = True, .WithImage = True}
                                                                   bm.SetImage(CType(frm, BasicForm).Img, "attchmenttype.jpg")
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

            AddHandler LoadRadio(138, G, "Survey").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BasicForm With {.TableName = "Survey", .IsMultiLine = True}
                                                                bm.SetImage(CType(frm, BasicForm).Img, "attchmenttype.jpg")
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

            AddHandler LoadRadio(139, G, "Welcome Images").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BasicForm With {.TableName = "GalleryMain", .WithImage = True}
                                                                        bm.SetImage(CType(frm, BasicForm).Img, "attchmenttype.jpg")
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(140, G, "Gallery").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BasicForm With {.TableName = "Gallery", .WithImage = True}
                                                                 bm.SetImage(CType(frm, BasicForm).Img, "attchmenttype.jpg")
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
        End If




    End Sub


    Private Sub LoadGStores(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"
        's = "Optics.jpeg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Stores", s)
        Dim frm As UserControl

        AddHandler LoadRadio(501, G, "Groups").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New Groups
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub


        AddHandler LoadRadio(502, G, "Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           'frm = New BasicForm2 With {.MainTableName = "Groups", .MainSubId = "Id", .MainSubName = "Name", .lblMain_Content = "Group", .TableName = "Types", .MainId = "GroupId", .SubId = "Id", .SubName = "Name"}
                                                           frm = New Types
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        If Md.ShowPriceLists OrElse Md.MyProjectType = ProjectType.X17 Then
            AddHandler LoadRadio(503, G, "PriceLists").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New BasicForm With {.TableName = "PriceLists"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If

        If Md.MyProjectType = ProjectType.X17 Then
            AddHandler LoadRadio(577, G, "مندوبي التسليم").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BasicForm With {.TableName = "DeliveryPersons"}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        If Not Md.ShowQtySub Then
            If Not Md.MyProjectType = ProjectType.X4 AndAlso Not Md.MyProjectType = ProjectType.X5 Then
                AddHandler LoadRadio(563, G, "Itemunits").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New BasicForm With {.TableName = "Itemunits"}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
            End If
        End If

        If Md.MyProjectType = ProjectType.X25 Then
            AddHandler LoadRadio(563, G, "UnitsWeights").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New BasicForm1_2 With {.TableName = "UnitsWeights", .lblName2_text = "الوزن", .SubName2 = "Weights"}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub
        End If


        If Md.MyProjectType = ProjectType.X27 Then
            AddHandler LoadRadio(570, G, "ItemsUpdate").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New ItemsUpdate
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub
        End If

        If Md.MyProjectType = ProjectType.X14 OrElse Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 Then
            AddHandler LoadRadio(512, G, "Statics").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New Statics
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
        End If

        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X24 Then
            AddHandler LoadRadio(513, G, "ItemComponants").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New ItemComponants
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        AddHandler LoadRadio(510, G, "Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New Items
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        AddHandler LoadRadio(511, G, "Print Barcode").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPT18 With {.Flag = 1}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        AddHandler LoadRadio(508, G, "Print Barcode Grid").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New PrintParcodeGrid
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        AddHandler LoadRadio(583, G, "إيقاف أصناف").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New PrintParcodeGrid With {.Flag = 2}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

        AddHandler LoadRadio(584, G, "عمولات الأصناف").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New ItemCommissions
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(581, G, "ItemsPrices").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New ItemsPrices
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub


        If Md.MyProjectType = ProjectType.X8 Then
            AddHandler LoadRadio(803, G, "CostCenters").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New CostCenters
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub
        End If

        AddHandler LoadRadio(514, G, "Stores").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New Stores
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        AddHandler LoadRadio(582, G, "تكويد العدسات").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPT46
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        LoadLabel(515, G, "Stores Motion")

        AddHandler LoadRadio(516, G, "Starting balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New Sales With {.Flag = Sales.FlagState.أرصدة_افتتاحية}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        Dim str As String = "Adding"
        If Md.MyProjectType = ProjectType.X11 Then str = "Donations"
        If Md.MyProjectType = ProjectType.X17 Then str = "InventoryAdding"
        AddHandler LoadRadio(517, G, str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       frm = New Sales With {.Flag = Sales.FlagState.إضافة}
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub

        str = "Exchange"
        If Md.MyProjectType = ProjectType.X17 Then str = "InventoryExchange"
        AddHandler LoadRadio(518, G, str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       frm = New Sales With {.Flag = Sales.FlagState.صرف}
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub

        If Not Md.MyProjectType = ProjectType.X17 Then
            AddHandler LoadRadio(519, G, "Gifts").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Sales With {.Flag = Sales.FlagState.هدايا}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

            AddHandler LoadRadio(520, G, "Loses").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Sales With {.Flag = Sales.FlagState.هالك}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub
        End If

        AddHandler LoadRadio(521, G, "Transfer to a Store").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New Sales With {.Flag = Sales.FlagState.تحويل_إلى_مخزن}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

        If Md.MyProjectType = ProjectType.X17 Then
            AddHandler LoadRadio(522, G, "Manual Receive").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New Sales With {.Flag = Sales.FlagState.تحويل_إلى_مخزن, .Receive = True}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If


        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 Then
            AddHandler LoadRadio(523, G, "Separate and Collect Orders").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New ItemCollectionMotion
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub
        End If

        AddHandler LoadRadio(524, G, "Inventory").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Inventory
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.Optics Then
            LoadLabel(525, G, "Imports")

            AddHandler LoadRadio(526, G, "Shippers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New BasicForm With {.TableName = "Shippers"}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

            AddHandler LoadRadio(527, G, "ShippingLines").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New BasicForm With {.TableName = "ShippingLines"}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

            AddHandler LoadRadio(528, G, "ShippingCompanies").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New BasicForm With {.TableName = "ShippingCompanies"}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(529, G, "ContainerSizes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BasicForm With {.TableName = "ContainerSizes"}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(530, G, "PaymentMethods").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New BasicForm With {.TableName = "PaymentMethods"}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(531, G, "ShippingMethods").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New BasicForm With {.TableName = "ShippingMethods"}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(532, G, "Imports").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New Sales With {.Flag = Sales.FlagState.الاستيراد}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

            AddHandler LoadRadio(533, G, "Imports Returns").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New Sales With {.Flag = Sales.FlagState.مردودات_الاستيراد}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(534, G, "ImportMessages").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New ImportMessages
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        LoadLabel(535, G, "Purchases")

        AddHandler LoadRadio(536, G, "Purchases").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Sales With {.Flag = Sales.FlagState.مشتريات}
                                                               If Md.MyProjectType = ProjectType.X27 Then
                                                                   frm = New Sales2 With {.Flag = Sales.FlagState.مشتريات}
                                                               End If
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(537, G, "Purchase Returns").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New Sales With {.Flag = Sales.FlagState.مردودات_مشتريات}
                                                                      If Md.MyProjectType = ProjectType.X27 Then
                                                                          frm = New Sales2 With {.Flag = Sales.FlagState.مردودات_مشتريات}
                                                                      End If
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X26 Then
            Dim f As String = "Outer Purchases"
            Dim f2 As String = "Outer Purchase Returns"
            If Md.MyProjectType = ProjectType.X26 Then
                f = "Direct Purchases"
                f2 = "Direct Purchases Returns"
                LoadLabel(569, G, f)
            End If
            AddHandler LoadRadio(564, G, f).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                         frm = New Sales With {.Flag = Sales.FlagState.مشتريات_خارجية}
                                                         sender.Tag = New MyPage With {.Content = frm}
                                                     End Sub

            AddHandler LoadRadio(565, G, f2).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                          frm = New Sales With {.Flag = Sales.FlagState.مردودات_مشتريات_خارجية}
                                                          sender.Tag = New MyPage With {.Content = frm}
                                                      End Sub

        End If


        If Md.MyProjectType = ProjectType.X17 Then
            AddHandler LoadRadio(579, G, "إذن تسليم مورد").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New DeliveryOrder With {.Flag = 1}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        End If


        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.Optics Then
            AddHandler LoadRadio(538, G, "Purchase Order").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New Sales With {.Flag = Sales.FlagState.أمر_شراء}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        If Md.MyProjectType = ProjectType.X18 Then
            AddHandler LoadRadio(576, G, "Supplier Quotation").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New Sales With {.Flag = Sales.FlagState.عرض_أسعار_مورد}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub
        End If

        LoadLabel(539, G, "Sales")



        AddHandler LoadRadio(549, G, "Sales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Sales With {.Flag = Sales.FlagState.المبيعات}
                                                               If Md.MyProjectType = ProjectType.X27 Then
                                                                   frm = New Sales2 With {.Flag = Sales2.FlagState.المبيعات}
                                                               End If
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

            AddHandler LoadRadio(550, G, "Sales Returns").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New Sales With {.Flag = Sales.FlagState.مردودات_المبيعات}
                                                                       If Md.MyProjectType = ProjectType.X27 Then
                                                                           frm = New Sales2 With {.Flag = Sales2.FlagState.مردودات_المبيعات}
                                                                       End If
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

            If Md.MyProjectType = ProjectType.X25 OrElse Md.MyProjectType = ProjectType.X26 Then
            Dim f As String = "Exports"
            Dim f2 As String = "Exports Returns"
            If Md.MyProjectType = ProjectType.X26 Then
                f = "Direct Sales"
                f2 = "Direct Sales Returns"
            End If
            LoadLabel(551, G, f)

            AddHandler LoadRadio(552, G, f).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                         frm = New Sales With {.Flag = Sales.FlagState.التصدير}
                                                         sender.Tag = New MyPage With {.Content = frm}
                                                     End Sub

            AddHandler LoadRadio(553, G, f2).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                          frm = New Sales With {.Flag = Sales.FlagState.مردودات_التصدير}
                                                          sender.Tag = New MyPage With {.Content = frm}
                                                      End Sub

        End If

        If Md.MyProjectType = ProjectType.X28 Then
            LoadLabel(573, G, "مبيعات لمستثمر")

            AddHandler LoadRadio(574, G, "مبيعات لمستثمر").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New Sales With {.Flag = Sales.FlagState.مبيعات_لمستثمر}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(575, G, "مردودات مبيعات لمستثمر").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New Sales With {.Flag = Sales.FlagState.مردودات_مبيعات_لمستثمر}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

        End If

        If 1 = 2 Then
            AddHandler LoadRadio(554, G, "SalesDelivery").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New Sales With {.Flag = Sales.FlagState.مبيعات_التوصيل}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

            AddHandler LoadRadio(555, G, "SalesDeliveryReturns").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New Sales With {.Flag = Sales.FlagState.مردودات_مبيعات_التوصيل}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub
        End If

        If Md.MyProjectType = ProjectType.X29 Then
            AddHandler LoadRadio(580, G, "أمر توريد").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Sales With {.Flag = Sales.FlagState.أمر_توريد}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.X24 OrElse Md.MyProjectType = ProjectType.X18 OrElse Md.MyProjectType = ProjectType.Optics Then
            AddHandler LoadRadio(556, G, "Quotation").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Sales With {.Flag = Sales.FlagState.عرض_أسعار}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        If Md.MyProjectType = ProjectType.X24 Then
            AddHandler LoadRadio(557, G, "ItemsDelivery").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New ItemsDelivery
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
        End If

        If Md.MyProjectType = ProjectType.X17 Then
            AddHandler LoadRadio(558, G, "إذن تسليم عميل").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New DeliveryOrder
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub


            AddHandler LoadRadio(578, G, "ItemCollectionMaintenance").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                   frm = New ItemCollectionMaintenance
                                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                                               End Sub

        ElseIf Md.MyProjectType = ProjectType.Optics Then
            AddHandler LoadRadio(558, G, "إذن تسليم عميل").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New DeliveryOrder_O
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If




        If Md.MyProjectType = ProjectType.X16 Then
            AddHandler LoadRadio(559, G, "Wholesales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New Sales With {.Flag = Sales.FlagState.مبيعات_الجملة}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

            AddHandler LoadRadio(560, G, "WholesalesReturns").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New Sales With {.Flag = Sales.FlagState.مردودات_مبيعات_الجملة}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub
        End If

        If Md.MyProjectType = ProjectType.X24 Then
            AddHandler LoadRadio(566, G, "Cashier").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New Cashier
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
        End If

        If Md.MyProjectType = ProjectType.X18 Then
            AddHandler LoadRadio(567, G, "Tenders").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New Tenders
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
        End If

        '584
    End Sub

    Private Sub LoadGSalaries(CurrentTab As Integer)
        Dim s As String = ""
        Select Case Md.MyProjectType
            Case ProjectType.X11, ProjectType.X1, ProjectType.X2
                s = "reservation.jpg"
            Case Else
                s = "MainOMEGA.jpg"
        End Select

        Dim G As WrapPanel = MakePanel(CurrentTab, "Salaries", s)
        Dim frm As UserControl

        AddHandler LoadRadio(701, G, "OfficialHolidays").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New OfficialHolidays
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(702, G, "Import Attendance").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New CalcSalary With {.Flag = 4}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        AddHandler LoadRadio(703, G, "Edit Attendance").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New EditAttendance
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        AddHandler LoadRadio(704, G, "Loans").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New Loans
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        LoadLabel(705, G, "Employees Motion")

        AddHandler LoadRadio(706, G, "DirectBonus").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New DirectBonusCut With {.TableName = "DirectBonus"}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

        AddHandler LoadRadio(707, G, "DirectCut").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New DirectBonusCut With {.TableName = "DirectCut"}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(708, G, "LeaveRequests").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New LeaveRequests
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        AddHandler LoadRadio(709, G, "LeaveRequests2").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New LeaveRequests2 With {.TableName = "LeaveRequests2"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        LoadLabel(710, G, "Calculation")

        AddHandler LoadRadio(711, G, "Calc Salary").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New CalcSalary With {.Flag = 1}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub


    End Sub

    Private Sub LoadGAccountants(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Accounts", s)
        Dim frm As UserControl

        AddHandler LoadRadio(801, G, "Chart").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New Chart
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        If Md.ShowAnalysis Then
            AddHandler LoadRadio(802, G, "Analysis").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New BasicForm With {.TableName = "Analysis"}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub
        End If

        If Md.ShowCostCenter Then
            AddHandler LoadRadio(803, G, "CostCenters").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New CostCenters
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub
        End If

        If Md.ShowCostCenterSub Then
            AddHandler LoadRadio(804, G, "CostCenterSubs").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New CostCenters With {.TableName = "CostCenterSubs", .MyHeader = "عناصر التكلفة"}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If

        If Md.ShowCurrency Then
            AddHandler LoadRadio(805, G, "Currencies").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New BasicForm1_2 With {.TableName = "Currencies", .lblName2_text = "الرمز", .SubName2 = "Sign"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If

        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.X22 OrElse Md.MyProjectType = ProjectType.X25 OrElse Md.MyProjectType = ProjectType.X26 OrElse Md.MyProjectType = ProjectType.Optics Then
            AddHandler LoadRadio(806, G, "CheckBanks").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New BasicForm With {.TableName = "CheckBanks"}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

            AddHandler LoadRadio(807, G, "Income Daily Motion Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                   frm = New BankCash_G2Types With {.Flag = 1}
                                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                                               End Sub

            AddHandler LoadRadio(808, G, "Outcome Daily Motion Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                    frm = New BankCash_G2Types With {.Flag = 2}
                                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                                End Sub
        End If



        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 Then
            AddHandler LoadRadio(809, G, "Adjustments Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New BankCash_G2Types With {.Flag = 3}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        End If
        AddHandler LoadRadio(810, G, "Entry Types").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BankCash_G2Types With {.Flag = 4}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub


        If Md.MyProjectType = ProjectType.X25 Then
            AddHandler LoadRadio(811, G, "FinalReportsMain").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New BasicForm With {.TableName = "FinalReportsMain"}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

            AddHandler LoadRadio(812, G, "FinalReportsSub").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New BasicForm2 With {.MainTableName = "FinalReportsMain", .lblMain_Content = "FinalReportsMain", .TableName = "FinalReportsSub", .MainId = "FinalReportsMainId"}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(813, G, "FinalReportsSub2").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New FinalReportsSubDetails
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        End If


        If Md.ShowCurrency Then
            AddHandler LoadRadio(857, G, "CurrencyExchangeByDate").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New CurrencyExchangeByDate
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
        End If

        LoadLabel(814, G, "File")

        AddHandler LoadRadio(815, G, "Assets").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New Assets
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        If Md.MyProjectType <> ProjectType.X11 AndAlso Md.MyProjectType <> ProjectType.X1 AndAlso Md.MyProjectType <> ProjectType.X2 Then
            AddHandler LoadRadio(816, G, "Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Customers
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        AddHandler LoadRadio(817, G, "Suppliers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New Suppliers
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(818, G, "Debits").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New CreditsDebits With {.TableName = "Debits", .MyLinkFile = 3}
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        AddHandler LoadRadio(819, G, "Credits").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New CreditsDebits With {.TableName = "Credits", .MyLinkFile = 4}
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub

        AddHandler LoadRadio(820, G, "Saves").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New CreditsDebits With {.TableName = "Saves", .MyLinkFile = 5}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

        If Md.ShowBanks Then
            AddHandler LoadRadio(821, G, "Banks").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New CreditsDebits With {.TableName = "Banks", .MyLinkFile = 6}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub
        End If

        If Md.MyProjectType = ProjectType.X12 OrElse Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.X2 OrElse Md.MyProjectType = ProjectType.X25 OrElse Md.MyProjectType = ProjectType.X26 Then
            AddHandler LoadRadio(822, G, "Sellers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New CreditsDebits With {.TableName = "Sellers", .MyLinkFile = 7}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub
        End If

        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.X25 Then
            AddHandler LoadRadio(823, G, "MoneyChangers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New CreditsDebits With {.TableName = "MoneyChangers", .MyLinkFile = 8}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
        End If

        AddHandler LoadRadio(824, G, "OutComeTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New CreditsDebits With {.TableName = "OutComeTypes", .MyLinkFile = 9}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        AddHandler LoadRadio(825, G, "InComeTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New CreditsDebits With {.TableName = "InComeTypes", .MyLinkFile = 10}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.Optics Then
            AddHandler LoadRadio(826, G, "OrderTypes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New OrderTypes
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If

        If Md.MyProjectType = ProjectType.X24 Then
            AddHandler LoadRadio(827, G, "Teachers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New CreditsDebits With {.TableName = "Teachers", .MyLinkFile = 14}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub
        End If

        If Md.MyProjectType = ProjectType.X29 OrElse Md.MyProjectType = ProjectType.X28 OrElse Md.MyProjectType = ProjectType.Optics Then
            AddHandler LoadRadio(862, G, "Investors").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New CreditsDebits With {.TableName = "Investors", .MyLinkFile = 15}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        LoadLabel(828, G, "Daily Motion")



        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.Optics Then
            AddHandler LoadRadio(832, G, "Adjustments").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New Entry2
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        End If

        Dim str As String = "Entry"
        If Md.MyProjectType = ProjectType.X25 Then
            str = "التسويات"
        End If
        AddHandler LoadRadio(833, G, Str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                       frm = New Entry
                                                       sender.Tag = New MyPage With {.Content = frm}
                                                   End Sub

        LoadLabel(834, G, "Income and Outcome")

        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.X22 OrElse Md.MyProjectType = ProjectType.X25 OrElse Md.MyProjectType = ProjectType.X26 OrElse Md.MyProjectType = ProjectType.X18 OrElse Md.MyProjectType = ProjectType.X19 OrElse Md.MyProjectType = ProjectType.Optics Then
            AddHandler LoadRadio(835, G, "Income").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New BankCash_G2 With {.Flag = 1}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

            AddHandler LoadRadio(836, G, "Outcome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New BankCash_G2 With {.Flag = 2}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub



        End If



        'AddHandler LoadRadio(10111, G, "سداد الموردين").Checked, Sub(sender As Object, e As RoutedEventArgs)
        '                                                             frm = New BankCash0
        '                                                             sender.Tag = New MyPage With {.Content = frm}
        '                                                         End Sub


        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X18 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.X26 OrElse Md.MyProjectType = ProjectType.X19 OrElse Md.MyProjectType = ProjectType.Optics Then
            AddHandler LoadRadio(846, G, "Checks Tracing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New ChecksTracingNew
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub
        End If






        If Md.MyProjectType = ProjectType.X29 OrElse Md.MyProjectType = ProjectType.X28 OrElse Md.MyProjectType = ProjectType.Optics Then
            LoadLabel(10107, G, "Investors")

            AddHandler LoadRadio(10108, G, "InvestorsProfits").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New InvestorsProfits
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub


            AddHandler LoadRadio(10110, G, "IncomeFromInvestors").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New BankCash5 With {.Flag = 1, .MyLinkFile = 5}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            AddHandler LoadRadio(10111, G, "OutComeToInvestors").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New BankCash5 With {.Flag = 2, .MyLinkFile = 5}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        End If


        '10111
    End Sub

    Private Sub LoadGSecurity(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Options", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1101, G, "Change Password").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New ChangePassword
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(1102, G, "Levels").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New Levels
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub

        AddHandler LoadRadio(1103, G, "Attachement").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New Attachments
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        If Md.ShowShifts Then
            AddHandler LoadRadio(1104, G, "Close Shift").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New CalcSalary With {.Flag = 6}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub
        End If

        If Md.MyProjectType = ProjectType.X11 OrElse Md.MyProjectType = ProjectType.X1 OrElse Md.MyProjectType = ProjectType.X2 Then
            AddHandler LoadRadio(1105, G, "Schedule").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New Schedule
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        If Md.MyProjectType = ProjectType.X8 Then
            AddHandler LoadRadio(1109, G, "Tasks").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New Tasks
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub


            'AddHandler LoadRadio(1110, G, "TasksDetails").Checked, Sub(sender As Object, e As RoutedEventArgs)
            '                                                           frm = New TasksDetails
            '                                                           sender.Tag = New MyPage With {.Content = frm}
            '                                                       End Sub


            Dim w As New MyWindow With {.Content = New TasksDetails, .PreventClosing = True}

            w.Show()
        End If

        If Md.MyProjectType = ProjectType.X17 Then
            AddHandler LoadRadio(1106, G, "SHUTDOWN").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New CalcSalary With {.Flag = 10}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub
        End If

        If Md.MyProjectType = ProjectType.X27 Then


            AddHandler LoadRadio(1107, G, "فتح سنة مالية جديدة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New CalcSalary With {.Flag = 11}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub


            AddHandler LoadRadio(1108, G, "بدء سنة مالية جديدة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New CalcSalary With {.Flag = 12}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        End If

        AddHandler LoadRadio(1111, G, "Backup").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                             frm = New CalcSalary With {.Flag = 13}
                                                             sender.Tag = New MyPage With {.Content = frm}
                                                         End Sub

        AddHandler LoadRadio(1112, G, "Restore").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New CalcSalary With {.Flag = 14}
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub


        AddHandler LoadRadio(1113, G, "تصدير المبيعات لملف").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT36 With {.Flag = 3}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        AddHandler LoadRadio(1114, G, "استيراد المبيعات من ملف").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT36 With {.Flag = 4}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub


    End Sub

    Private Sub LoadGStoresReports(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Stores Reports", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1501, G, "Items Printing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT13 With {.Flag = 1}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        AddHandler LoadRadio(1502, G, "Items Printing With Images").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT13 With {.Flag = 2}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

        LoadLabel(1503, G, "Stores Motion")

        AddHandler LoadRadio(1504, G, "Stores Motions Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT6 With {.Flag = 1, .Detail = 1}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        AddHandler LoadRadio(1505, G, "Stores Motions Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT6 With {.Flag = 1, .Detail = 0}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        If Md.ShowStoresMotionsEditing Then
            AddHandler LoadRadio(1506, G, "Stores Motions Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT6 With {.Flag = 1, .Detail = 2}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
        End If

        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 Then
            AddHandler LoadRadio(1507, G, "Separate and Collect Orders").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT6 With {.Flag = 11, .Detail = 16}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            If Md.ShowStoresMotionsEditing Then
                AddHandler LoadRadio(1508, G, "Separate and Collect Orders Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                                  frm = New RPT6 With {.Flag = 11, .Detail = 17}
                                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                                              End Sub
            End If
        End If


        AddHandler LoadRadio(1509, G, "Purchase Invoices Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT6 With {.Flag = 2, .Detail = 1}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

        AddHandler LoadRadio(1510, G, "Purchase Invoices Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT6 With {.Flag = 2, .Detail = 0}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        If Md.ShowStoresMotionsEditing Then
            AddHandler LoadRadio(1511, G, "Purchase Invoices Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                    frm = New RPT6 With {.Flag = 2, .Detail = 2}
                                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                                End Sub
        End If


        Dim f1 As String = "Purchase Invoices Detailed"
        Dim f2 As String = "Purchase Invoices Total"
        Dim f3 As String = "Purchase Invoices Editing"
        If Md.MyProjectType = ProjectType.X26 Then
            f1 = "Direct Purchases Detailed"
            f2 = "Direct Purchases Total"
            f3 = "Direct Purchases Editing"
        End If


        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X26 Then
            AddHandler LoadRadio(1571, G, f1).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New RPT6 With {.Flag = 21, .Detail = 1}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

            AddHandler LoadRadio(1572, G, f2).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New RPT6 With {.Flag = 21, .Detail = 0}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

            If Md.ShowStoresMotionsEditing Then
                AddHandler LoadRadio(1573, G, f3).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT6 With {.Flag = 21, .Detail = 2}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub
            End If

        End If


        If Md.MyProjectType = ProjectType.X18 Then
            AddHandler LoadRadio(1585, G, "Supplier Quotation").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT6 With {.Flag = 28, .Detail = 1}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub
        End If




        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.Optics Then
            AddHandler LoadRadio(1512, G, "Imports Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT6 With {.Flag = 7, .Detail = 1}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(1513, G, "Imports Not In Messages").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT6 With {.Flag = 10, .Detail = 1}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub


            AddHandler LoadRadio(1514, G, "Imports Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT6 With {.Flag = 7, .Detail = 0}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            If Md.ShowStoresMotionsEditing Then
                AddHandler LoadRadio(1515, G, "Imports Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT6 With {.Flag = 7, .Detail = 2}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub
            End If

            AddHandler LoadRadio(1516, G, "ImportMessagePacking").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT32 With {.Flag = 3}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            AddHandler LoadRadio(1517, G, "ImportMessagePackingImages").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT32 With {.Flag = 4}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub

        End If

        If Md.MyProjectType = ProjectType.X8 OrElse Md.MyProjectType = ProjectType.X11 OrElse Md.MyProjectType = ProjectType.X1 OrElse Md.MyProjectType = ProjectType.X2 Then
            Dim str As String = "Consumables Detailed"
            If Md.MyProjectType = ProjectType.X8 Then
                str = "المبيعات نقدي تفصيلي"
            End If

            AddHandler LoadRadio(1518, G, str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New RPT6 With {.Flag = 4, .Detail = 1}
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

            str = "Consumables Total"
            If Md.MyProjectType = ProjectType.X8 Then
                str = "المبيعات نقدي إجمالي"
            End If
            AddHandler LoadRadio(1519, G, str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New RPT6 With {.Flag = 4, .Detail = 0}
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

            If Md.ShowStoresMotionsEditing Then
                str = "Consumables Editing"
                If Md.MyProjectType = ProjectType.X8 Then
                    str = "تعديلات المبيعات نقدي"
                End If
                AddHandler LoadRadio(1520, G, "Consumables Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT6 With {.Flag = 4, .Detail = 2}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub
            End If

            If Md.MyProjectType = ProjectType.X2 Then
                AddHandler LoadRadio(1584, G, "Consumables Profit").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT6 With {.Flag = 4, .Detail = 20}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

            End If
        Else
            AddHandler LoadRadio(1521, G, "Sales Invoices Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT6 With {.Flag = 3, .Detail = 1}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub

            AddHandler LoadRadio(1522, G, "Sales Invoices Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT6 With {.Flag = 3, .Detail = 0}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            If Md.ShowStoresMotionsEditing Then
                AddHandler LoadRadio(1524, G, "Sales Invoices Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT6 With {.Flag = 3, .Detail = 2}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub
            End If
        End If



        AddHandler LoadRadio(1597, G, "عمولات البائعين").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 3, .Detail = 22}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        f1 = "Exports Detailed"
        f2 = "Exports Total"
        f3 = "Exports Editing"
        If Md.MyProjectType = ProjectType.X26 Then
            f1 = "Direct Sales Detailed"
            f2 = "Direct Sales Total"
            f3 = "Direct Sales Editing"
        End If

        If Md.MyProjectType = ProjectType.X25 OrElse Md.MyProjectType = ProjectType.X26 Then
            AddHandler LoadRadio(1525, G, f1).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New RPT6 With {.Flag = 8, .Detail = 1}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

            AddHandler LoadRadio(1526, G, f2).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                           frm = New RPT6 With {.Flag = 8, .Detail = 0}
                                                           sender.Tag = New MyPage With {.Content = frm}
                                                       End Sub

            If Md.ShowStoresMotionsEditing Then
                AddHandler LoadRadio(1527, G, f3).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT6 With {.Flag = 8, .Detail = 2}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub
            End If

        End If

        If Md.MyProjectType = ProjectType.X28 Then
            AddHandler LoadRadio(1581, G, "Investors Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT6 With {.Flag = 11, .Detail = 1}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

            AddHandler LoadRadio(1582, G, "Investors Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT6 With {.Flag = 11, .Detail = 0}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

            If Md.ShowStoresMotionsEditing Then
                AddHandler LoadRadio(1583, G, "Investors Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT6 With {.Flag = 11, .Detail = 2}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
            End If

        End If

        If Md.MyProjectType = ProjectType.X8 OrElse Md.MyProjectType = ProjectType.X11 OrElse Md.MyProjectType = ProjectType.X1 OrElse Md.MyProjectType = ProjectType.X2 Then
        Else
            AddHandler LoadRadio(1523, G, "Stores Sales Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT6 With {.Flag = 3, .Detail = 9}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

        End If

        If Md.MyProjectType = ProjectType.X24 Then
            AddHandler LoadRadio(1528, G, "ItemsDelivery").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT36 With {.Flag = 2}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        End If


        If Md.MyProjectType = ProjectType.X17 Then

            AddHandler LoadRadio(1593, G, "إذن تسليم مورد").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPTFromDateToDate With {.Flag = 8}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(1591, G, "إذن تسليم عميل").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPTFromDateToDate With {.Flag = 8}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

        End If

        If Md.MyProjectType = ProjectType.X29 Then
            AddHandler LoadRadio(1595, G, "أمر توريد").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT6 With {.Flag = 31, .Detail = 21}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If


        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.X24 OrElse Md.MyProjectType = ProjectType.X18 Then
            AddHandler LoadRadio(1529, G, "Quotation").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT6 With {.Flag = 9, .Detail = 1}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub
        End If

        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.Optics Then
            AddHandler LoadRadio(1530, G, "ImportMessagesDetailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT32 With {.Flag = 1}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

            AddHandler LoadRadio(1531, G, "ImportMessagesTotal").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT32 With {.Flag = 2}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

            AddHandler LoadRadio(1596, G, "أصناف رسائل غير مستلمة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPTFromDateToDate With {.Flag = 9}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

            If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.Optics Then

                AddHandler LoadRadio(1532, G, "ImportMessageItemMotionCostPrice").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                               frm = New RPT32 With {.Flag = 5}
                                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                                           End Sub

                AddHandler LoadRadio(1533, G, "ImportMessageItemMotionSalesPrice").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                                frm = New RPT32 With {.Flag = 8}
                                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                                            End Sub

                AddHandler LoadRadio(1589, G, "ImportMessageItemMotionQty").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                         frm = New RPT32 With {.Flag = 10}
                                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                                     End Sub

                AddHandler LoadRadio(1534, G, "PrintSalesPone32_N").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT30 With {.Flag = 3}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

                AddHandler LoadRadio(1594, G, "أصناف موردين غير مستلمة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT32 With {.Flag = 9, .PRTFlag = 1}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub

                AddHandler LoadRadio(1535, G, "أصناف عملاء غير مستلمة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT32 With {.Flag = 9, .PRTFlag = 0}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub

            End If



            If Md.MyProjectType = ProjectType.X17 Then

                AddHandler LoadRadio(1592, G, "أصناف تحت الصيانة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT32 With {.Flag = 11}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

            End If


        End If

        If Md.MyProjectType = ProjectType.X26 Then
            AddHandler LoadRadio(1580, G, "SalesDeliveryDates").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT25 With {.Flag = 25}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

        End If



        LoadLabel(1536, G, "Items Motion")

        If Md.MyProjectType = ProjectType.X14 Then
            AddHandler LoadRadio(1537, G, "Print Pones").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 3, .Detail = 5}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub
        End If

        If Md.MyProjectType = ProjectType.X16 Then
            AddHandler LoadRadio(1538, G, "Sales Profit").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT6 With {.Flag = 3, .Detail = 4}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub
        Else
            If Md.MyProjectType = ProjectType.X24 Then
                AddHandler LoadRadio(1539, G, "Items Profit").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT13 With {.Flag = 4}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub
            End If

            AddHandler LoadRadio(1540, G, "Sales Profit").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT6 With {.Flag = 30, .Detail = 4}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub


            AddHandler LoadRadio(1541, G, "Items Sales Price Avg").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT6 With {.Flag = 3, .Detail = 10}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

            AddHandler LoadRadio(1542, G, "Item Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT6 With {.Flag = 3, .Detail = 11}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            AddHandler LoadRadio(1543, G, "Most Sales Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT6 With {.Flag = 3, .Detail = 12}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            AddHandler LoadRadio(1544, G, "Most Profit Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT6 With {.Flag = 3, .Detail = 13}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

            AddHandler LoadRadio(1545, G, "Items Sales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 3, .Detail = 3}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(1546, G, "Items Sales Summarized").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT6 With {.Flag = 3, .Detail = 15}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

            If Md.MyProjectType = ProjectType.X26 Then
                AddHandler LoadRadio(1545, G, "Direct Items Sales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT6 With {.Flag = 8, .Detail = 3}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

                AddHandler LoadRadio(1546, G, "Direct Items Sales Summarized").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                            frm = New RPT6 With {.Flag = 8, .Detail = 15}
                                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                                        End Sub

            End If
            AddHandler LoadRadio(1547, G, "Best-Selling Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT6 With {.Flag = 3, .Detail = 14}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

            'AddHandler LoadRadio(1547, G, "الأصناف الأسرع مبيعا").Checked, Sub(sender As Object, e As RoutedEventArgs)
            '                                                                   frm = New RPT6 With {.Flag = 3, .Detail = 23}
            '                                                                   sender.Tag = New MyPage With {.Content = frm}
            '                                                               End Sub



            AddHandler LoadRadio(1547, G, "الأصناف الأسرع مبيعا").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPTFromDateToDate With {.Flag = 11}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            AddHandler LoadRadio(1598, G, "عملاء أكثر تحصيلات").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPTFromDateToDate With {.Flag = 12}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub


            AddHandler LoadRadio(1548, G, "Items Sales All Stores").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT6 With {.Flag = 3, .Detail = 6}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub

            If Md.MyProjectType = ProjectType.X15 Then
                AddHandler LoadRadio(1549, G, "StagnantItems").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT25 With {.Flag = 17}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

                AddHandler LoadRadio(1550, G, "StagnantCustomers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT25 With {.Flag = 18}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
            End If

        End If

        If Md.MyProjectType = ProjectType.X27 Then
            AddHandler LoadRadio(1574, G, "Sales Mobile").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT6 With {.Flag = 3, .Detail = 9}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

            AddHandler LoadRadio(1575, G, "Sales Notes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 3, .Detail = 5}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(1576, G, "Car Withdrawals").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT6 With {.Flag = 4, .Detail = 3}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

        End If
        If Md.MyProjectType = ProjectType.X26 Then
            AddHandler LoadRadio(1573, G, "*ItemCard*").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT12 With {.Flag = 9}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

        End If

        AddHandler LoadRadio(1551, G, "ItemCard").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT12 With {.Flag = 1}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        AddHandler LoadRadio(1552, G, "Item Motion With Cost").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT6 With {.Flag = 5, .Detail = 7}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

        If Md.MyProjectType = ProjectType.X14 OrElse Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.X2 Then
            AddHandler LoadRadio(1553, G, "ItemsCards").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New ItemsCard
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

            AddHandler LoadRadio(1554, G, "Store Bal Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT6 With {.Flag = 6, .Detail = 8}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub
        End If

        If Md.MyProjectType = ProjectType.X17 Then
            AddHandler LoadRadio(1555, G, "ItemsPackages").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT13 With {.Flag = 3}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

            AddHandler LoadRadio(1556, G, "Item Profit").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 5, .Detail = 18}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(1557, G, "ItemsCardSalesPrice").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT6 With {.Flag = 5, .Detail = 19}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        End If

        If Md.MyProjectType = ProjectType.X27 Then
            AddHandler LoadRadio(1556, G, "Item Profit").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 5, .Detail = 18}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub
        End If

        LoadLabel(1558, G, "Items Balances")

        AddHandler LoadRadio(1559, G, "Items Balance In Store").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT12 With {.Flag = 2}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

        If Md.MyProjectType = ProjectType.X14 Then
            AddHandler LoadRadio(1560, G, "Items Balance In Store Sizes").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                       frm = New RPT12 With {.Flag = 8}
                                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                                   End Sub
        End If

        AddHandler LoadRadio(1561, G, "Store Balance with Purchase Price").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                        frm = New RPT12 With {.Flag = 5}
                                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                                    End Sub

        AddHandler LoadRadio(1562, G, "Store Balance with Cost").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT12 With {.Flag = 51, .ReportFlag = 1}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

        AddHandler LoadRadio(1563, G, "Store Balance with Sales Price").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT12 With {.Flag = 52, .ReportFlag = 2}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub

        AddHandler LoadRadio(1564, G, "Items Balance In All Stores").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT12 With {.Flag = 3}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub

        AddHandler LoadRadio(1565, G, "All Stores Balance with Purchase Price").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                             frm = New RPT12 With {.Flag = 6}
                                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                                         End Sub

        AddHandler LoadRadio(1566, G, "All Stores Balance with Cost").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                   frm = New RPT12 With {.Flag = 61, .ReportFlag = 1}
                                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                                               End Sub

        AddHandler LoadRadio(1567, G, "All Stores Balance with Sales Price").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                          frm = New RPT12 With {.Flag = 62, .ReportFlag = 2}
                                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                                      End Sub

        AddHandler LoadRadio(1568, G, "Items exceeded limit demand").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT12 With {.Flag = 4}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub

        If Md.MyProjectType = ProjectType.X8 Then
            AddHandler LoadRadio(1590, G, "Expired Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT12 With {.Flag = 10}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        End If


        If Md.MyProjectType = ProjectType.X14 Then
            AddHandler LoadRadio(1569, G, "Items exceeded limit demand Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                               frm = New RPT12 With {.Flag = 7}
                                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                                           End Sub
        End If

        If Md.MyProjectType = ProjectType.X14 Then
            AddHandler LoadRadio(1570, G, "Reservation Items").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT6 With {.Flag = 5, .Detail = 1}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub
        End If

        If Md.MyProjectType = ProjectType.X27 Then

            AddHandler LoadRadio(1577, G, "Target Comparison").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT3 With {.Flag = 1}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

            AddHandler LoadRadio(1578, G, "Daily Sales").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 4, .Detail = 4}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

            AddHandler LoadRadio(1579, G, "Sales Bonus").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT6 With {.Flag = 4, .Detail = 7}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        End If

        '1598
    End Sub

    Private Sub LoadGSalaryReports(CurrentTab As Integer)
        Dim s As String = ""
        Select Case Md.MyProjectType
            Case ProjectType.X11, ProjectType.X1, ProjectType.X2
                s = "reservation.jpg"
            Case Else
                s = "MainOMEGA.jpg"
        End Select


        Dim G As WrapPanel = MakePanel(CurrentTab, "Salary Reports", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1701, G, "Employees").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New CalcSalary With {.Flag = 8}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        AddHandler LoadRadio(1702, G, "Salary Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT9 With {.Flag = 1}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(1703, G, "Salary Total").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPT9 With {.Flag = 2}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        AddHandler LoadRadio(1704, G, "Attendance").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New RPT9 With {.Flag = 3}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

        AddHandler LoadRadio(1705, G, "Loans").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New RPT25 With {.Flag = 1}
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        AddHandler LoadRadio(1706, G, "Loans Status").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPT25 With {.Flag = 6}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub

        LoadLabel(1707, G, "Employees Motion")

        AddHandler LoadRadio(1708, G, "DirectBonus").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New RPT25 With {.Flag = 2}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub

        AddHandler LoadRadio(1709, G, "DirectCut").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New RPT25 With {.Flag = 3}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        AddHandler LoadRadio(1710, G, "LeaveRequests").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT25 With {.Flag = 4}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(1711, G, "LeaveRequests2").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPT25 With {.Flag = 5}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub

    End Sub

    Private Sub LoadGAccountsReports(CurrentTab As Integer)
        Dim s As String = "MainOMEGA.jpg"

        Dim G As WrapPanel = MakePanel(CurrentTab, "Accounts Reports", s)
        Dim frm As UserControl

        AddHandler LoadRadio(1801, G, "Chart").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New RPT26 With {.Flag = 1}
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        AddHandler LoadRadio(1802, G, "Customers").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New RPT38 With {.Flag = 1}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        Dim str As String = "Account Motion"
        If Md.MyProjectType = ProjectType.X25 Then
            str = "حساب الأستاذ"
        End If
        AddHandler LoadRadio(1803, G, str).Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                        frm = New RPT2 With {.Flag = 1, .MyLinkFile = -1}
                                                        sender.Tag = New MyPage With {.Content = frm}
                                                    End Sub

        If Not Md.MyProjectType = ProjectType.X16 Then
            AddHandler LoadRadio(1804, G, "EntryView").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT36 With {.Flag = 1}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

            If Md.ShowBankCash_G Then
                AddHandler LoadRadio(1805, G, "Income View").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT21 With {.Flag = 1}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

                If Md.ShowStoresMotionsEditing Then
                    AddHandler LoadRadio(1806, G, "Income Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT21 With {.Flag = 1, .Detailed = 2}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
                End If

                AddHandler LoadRadio(1807, G, "Outcome View").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT21 With {.Flag = 2}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

                If Md.ShowStoresMotionsEditing Then
                    AddHandler LoadRadio(1808, G, "Outcome Editing").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT21 With {.Flag = 2, .Detailed = 2}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub
                End If

            Else
                AddHandler LoadRadio(1809, G, "Safe Income View").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT4 With {.Flag = 1, .MyLinkFile = 5}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

                AddHandler LoadRadio(1810, G, "Safe Outcome View").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT4 With {.Flag = 2, .MyLinkFile = 5}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

                AddHandler LoadRadio(1811, G, "Bank Income View").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT4 With {.Flag = 3, .MyLinkFile = 6}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

                AddHandler LoadRadio(1812, G, "Bank Outcome View").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT4 With {.Flag = 4, .MyLinkFile = 6}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
            End If

            If Md.ShowCostCenter Then
                'AddHandler LoadRadio(1813,G, "CostCenterOutCome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                '                                                          frm = New RPT14 With {.Flag = 1}
                '                                                          sender.Tag = New MyPage With {.Content = frm}
                '                                                      End Sub

                'AddHandler LoadRadio(1814,G, "CostCenterOutComeToTal").Checked, Sub(sender As Object, e As RoutedEventArgs)
                '                                                               frm = New RPT14 With {.Flag = 2}
                '                                                               sender.Tag = New MyPage With {.Content = frm}
                '                                                           End Sub

                AddHandler LoadRadio(1815, G, "CostCenterAccountMotion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT14 With {.Flag = 3}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            End If
        End If

        AddHandler LoadRadio(1816, G, "Save Daily Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                        frm = New RPT2 With {.Flag = 2, .MyLinkFile = 5}
                                                                        sender.Tag = New MyPage With {.Content = frm}
                                                                    End Sub

        AddHandler LoadRadio(1892, G, "المطالبة").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                               frm = New RPT2 With {.Flag = 4, .MyLinkFile = 1}
                                                               sender.Tag = New MyPage With {.Content = frm}
                                                           End Sub

        If Md.ShowCurrency Then
            AddHandler LoadRadio(1817, G, "Currency Basket").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT28 With {.Flag = 1}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub
        End If


        AddHandler LoadRadio(1818, G, "AllEntries").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                 frm = New RPT25 With {.Flag = 22}
                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                             End Sub

        If Md.MyProjectType = ProjectType.X29 Then

            AddHandler LoadRadio(1891, G, "الأقساط").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                  frm = New RPT45 With {.Flag = 1}
                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                              End Sub
        End If

        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.X22 OrElse Md.MyProjectType = ProjectType.X3 OrElse Md.MyProjectType = ProjectType.X25 OrElse Md.MyProjectType = ProjectType.Optics Then

            If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.Optics Then
                AddHandler LoadRadio(1819, G, "Invoice Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT30 With {.Flag = 1}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

                AddHandler LoadRadio(1820, G, "Invoices Outcome").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT30 With {.Flag = 2}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

                AddHandler LoadRadio(1821, G, "Outcome Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT31 With {.Flag = 1}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            End If

            AddHandler LoadRadio(1822, G, "Statement of account").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT2 With {.Flag = 1}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            AddHandler LoadRadio(1823, G, "Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                   frm = New RPT11 With {.Flag = 1}
                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                               End Sub


            AddHandler LoadRadio(1824, G, "Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT2 With {.Flag = 3}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        Else

            LoadLabel(1825, G, "Account Motion")

            AddHandler LoadRadio(1826, G, "Asset Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT2 With {.Flag = 1, .MyLinkFile = 12}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            If Md.MyProjectType = ProjectType.X11 OrElse Md.MyProjectType = ProjectType.X1 OrElse Md.MyProjectType = ProjectType.X2 Then
                AddHandler LoadRadio(1827, G, "Customer Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT2 With {.Flag = 1, .MyLinkFile = 13}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            Else
                AddHandler LoadRadio(1828, G, "Customer Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT2 With {.Flag = 1, .MyLinkFile = 1}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            End If

            AddHandler LoadRadio(1829, G, "Supplier Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT2 With {.Flag = 1, .MyLinkFile = 2}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub

            AddHandler LoadRadio(1830, G, "Debit Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT2 With {.Flag = 1, .MyLinkFile = 3}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub

            AddHandler LoadRadio(1831, G, "Credit Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT2 With {.Flag = 1, .MyLinkFile = 4}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub

            AddHandler LoadRadio(1832, G, "Save Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT2 With {.Flag = 1, .MyLinkFile = 5}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

            If Md.ShowBanks Then
                AddHandler LoadRadio(1833, G, "Bank Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT2 With {.Flag = 1, .MyLinkFile = 6}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub
            End If

            If Md.MyProjectType = ProjectType.X12 OrElse Md.MyProjectType = ProjectType.X14 OrElse Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X13 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.X2 OrElse Md.MyProjectType = ProjectType.X22 OrElse Md.MyProjectType = ProjectType.X25 Then
                AddHandler LoadRadio(1834, G, "Seller Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                    frm = New RPT2 With {.Flag = 1, .MyLinkFile = 7}
                                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                                End Sub
            End If

            If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X25 Then
                AddHandler LoadRadio(1835, G, "MoneyChanger Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                          frm = New RPT2 With {.Flag = 1, .MyLinkFile = 8}
                                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                                      End Sub
            End If

            AddHandler LoadRadio(1836, G, "OutCome Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT2 With {.Flag = 1, .MyLinkFile = 9}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            If Md.MyProjectType <> ProjectType.X13 Then
                AddHandler LoadRadio(1837, G, "InCome Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT2 With {.Flag = 1, .MyLinkFile = 10}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub
            End If

            If Md.MyProjectType = ProjectType.X24 Then
                AddHandler LoadRadio(1838, G, "Teacher Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT2 With {.Flag = 1, .MyLinkFile = 14}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub
            End If

            If Md.MyProjectType = ProjectType.X28 OrElse Md.MyProjectType = ProjectType.Optics Then
                AddHandler LoadRadio(1884, G, "Investor Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT2 With {.Flag = 1, .MyLinkFile = 15}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            End If

            If Md.ShowSalaries Then
                AddHandler LoadRadio(1887, G, "Employee Account Motion").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT2 With {.Flag = 1, .MyLinkFile = 16}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            End If

            LoadLabel(1839, G, "Balances")

            AddHandler LoadRadio(1840, G, "Assets Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT11 With {.Flag = 1, .MyLinkFile = 12}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

            If Md.MyProjectType = ProjectType.X11 OrElse Md.MyProjectType = ProjectType.X1 OrElse Md.MyProjectType = ProjectType.X2 Then
                AddHandler LoadRadio(1841, G, "Customers Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT11 With {.Flag = 1, .MyLinkFile = 13}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
            Else
                AddHandler LoadRadio(1842, G, "Customers Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT11 With {.Flag = 1, .MyLinkFile = 1}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
            End If

            AddHandler LoadRadio(1843, G, "Suppliers Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT11 With {.Flag = 1, .MyLinkFile = 2}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

            AddHandler LoadRadio(1844, G, "Debits Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT11 With {.Flag = 1, .MyLinkFile = 3}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub

            AddHandler LoadRadio(1845, G, "Credits Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT11 With {.Flag = 1, .MyLinkFile = 4}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(1846, G, "Saves Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT11 With {.Flag = 1, .MyLinkFile = 5}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub

            If Md.ShowBanks Then
                AddHandler LoadRadio(1847, G, "Banks Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT11 With {.Flag = 1, .MyLinkFile = 6}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub
            End If

            If Md.MyProjectType = ProjectType.X12 Then
                AddHandler LoadRadio(1848, G, "Sellers Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                               frm = New RPT11 With {.Flag = 1, .MyLinkFile = 7}
                                                                               sender.Tag = New MyPage With {.Content = frm}
                                                                           End Sub
            End If

            If Md.MyProjectType = ProjectType.X17 Then
                AddHandler LoadRadio(1849, G, "MoneyChangers Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                     frm = New RPT11 With {.Flag = 1, .MyLinkFile = 8}
                                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                                 End Sub
            End If

            AddHandler LoadRadio(1850, G, "OutCome Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT11 With {.Flag = 1, .MyLinkFile = 9}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            If Md.MyProjectType <> ProjectType.X13 Then
                AddHandler LoadRadio(1851, G, "InCome Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT11 With {.Flag = 1, .MyLinkFile = 10}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub
            End If

            If Md.MyProjectType = ProjectType.X24 Then
                AddHandler LoadRadio(1852, G, "Teachers Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT11 With {.Flag = 1, .MyLinkFile = 14}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
            End If

            If Md.MyProjectType = ProjectType.X28 OrElse Md.MyProjectType = ProjectType.Optics Then
                AddHandler LoadRadio(1885, G, "Investors Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT11 With {.Flag = 1, .MyLinkFile = 15}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
            End If

            If Md.ShowSalaries Then
                AddHandler LoadRadio(1888, G, "Employees Balances").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                 frm = New RPT11 With {.Flag = 1, .MyLinkFile = 16}
                                                                                 sender.Tag = New MyPage With {.Content = frm}
                                                                             End Sub
            End If

            If Md.MyProjectType = ProjectType.X16 Then Return

            LoadLabel(1853, G, "Assistant")

            AddHandler LoadRadio(1854, G, "Assets Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT2 With {.Flag = 3, .MyLinkFile = 12}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            If Md.MyProjectType = ProjectType.X11 OrElse Md.MyProjectType = ProjectType.X1 OrElse Md.MyProjectType = ProjectType.X2 Then
                AddHandler LoadRadio(1855, G, "Customers Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT2 With {.Flag = 3, .MyLinkFile = 13}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub
            Else
                AddHandler LoadRadio(1856, G, "Customers Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                  frm = New RPT2 With {.Flag = 3, .MyLinkFile = 1}
                                                                                  sender.Tag = New MyPage With {.Content = frm}
                                                                              End Sub
            End If

            AddHandler LoadRadio(1857, G, "Suppliers Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT2 With {.Flag = 3, .MyLinkFile = 2}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub

            AddHandler LoadRadio(1858, G, "Debits Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT2 With {.Flag = 3, .MyLinkFile = 3}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

            AddHandler LoadRadio(1859, G, "Credits Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT2 With {.Flag = 3, .MyLinkFile = 4}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

            AddHandler LoadRadio(1860, G, "Saves Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT2 With {.Flag = 3, .MyLinkFile = 5}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub



            AddHandler LoadRadio(1861, G, "Banks Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                          frm = New RPT2 With {.Flag = 3, .MyLinkFile = 6}
                                                                          sender.Tag = New MyPage With {.Content = frm}
                                                                      End Sub



            If Md.MyProjectType = ProjectType.X12 Then
                AddHandler LoadRadio(1862, G, "Sellers Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT2 With {.Flag = 3, .MyLinkFile = 7}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
            End If

            If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X25 Then
                AddHandler LoadRadio(1863, G, "MoneyChangers Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                      frm = New RPT2 With {.Flag = 3, .MyLinkFile = 8}
                                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                                  End Sub
            End If

            AddHandler LoadRadio(1864, G, "OutCome Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                            frm = New RPT2 With {.Flag = 3, .MyLinkFile = 9}
                                                                            sender.Tag = New MyPage With {.Content = frm}
                                                                        End Sub

            AddHandler LoadRadio(1865, G, "InCome Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                           frm = New RPT2 With {.Flag = 3, .MyLinkFile = 10}
                                                                           sender.Tag = New MyPage With {.Content = frm}
                                                                       End Sub

        End If

        If Md.MyProjectType = ProjectType.X24 Then
            AddHandler LoadRadio(1866, G, "Teachers Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT2 With {.Flag = 3, .MyLinkFile = 14}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub
        End If

        If Md.MyProjectType = ProjectType.X28 OrElse Md.MyProjectType = ProjectType.Optics Then
            AddHandler LoadRadio(1886, G, "Investors Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT2 With {.Flag = 3, .MyLinkFile = 15}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub
        End If

        If Md.ShowSalaries Then
            AddHandler LoadRadio(1889, G, "Employees Assistant").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                              frm = New RPT2 With {.Flag = 3, .MyLinkFile = 16}
                                                                              sender.Tag = New MyPage With {.Content = frm}
                                                                          End Sub
        End If

        If Md.MyProjectType = ProjectType.X17 Then
            AddHandler LoadRadio(1867, G, "AdministrativeExpensesDistribution").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                             frm = New RPT32 With {.Flag = 6}
                                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                                         End Sub

        End If

        LoadLabel(1868, G, "Final Accounts")

        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.Optics Then
            AddHandler LoadRadio(1869, G, "CalcAllImportMessages").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT25 With {.Flag = 21}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
        End If

        If Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X15 Then
            AddHandler LoadRadio(1870, G, "Calc Openned Messages").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                frm = New RPT25 With {.Flag = 10}
                                                                                sender.Tag = New MyPage With {.Content = frm}
                                                                            End Sub
        End If

        AddHandler LoadRadio(1871, G, "CalcAssetsDepreciation").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New CalcSalary With {.Flag = 9}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

        AddHandler LoadRadio(1872, G, "Calc Store Cost").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT25 With {.Flag = 8}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        AddHandler LoadRadio(1873, G, "Account Balance").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                      frm = New RPT20 With {.Flag = 1}
                                                                      sender.Tag = New MyPage With {.Content = frm}
                                                                  End Sub

        If Md.MyProjectType = ProjectType.X2 Then
            AddHandler LoadRadio(1883, G, "Account Balance Detailed").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                   frm = New RPT20 With {.Flag = 2}
                                                                                   sender.Tag = New MyPage With {.Content = frm}
                                                                               End Sub

        End If

        AddHandler LoadRadio(1874, G, "Operating").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                frm = New RPT27 With {.Flag = 1, .MyEndType = 0}
                                                                sender.Tag = New MyPage With {.Content = frm}
                                                            End Sub

        AddHandler LoadRadio(1875, G, "Trading").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                              frm = New RPT27 With {.Flag = 1, .MyEndType = 1}
                                                              sender.Tag = New MyPage With {.Content = frm}
                                                          End Sub

        AddHandler LoadRadio(1876, G, "Gains and losses").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT27 With {.Flag = 1, .MyEndType = 2}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        AddHandler LoadRadio(1877, G, "Balance Sheet").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                    frm = New RPT27 With {.Flag = 1, .MyEndType = 3}
                                                                    sender.Tag = New MyPage With {.Content = frm}
                                                                End Sub

        AddHandler LoadRadio(1878, G, "Income Statement").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT27 With {.Flag = 2, .MyEndType = 2, .IsIncomeStatement = 1}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        AddHandler LoadRadio(1879, G, "Financial Position").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                         frm = New RPT27 With {.Flag = 3, .MyEndType = 3}
                                                                         sender.Tag = New MyPage With {.Content = frm}
                                                                     End Sub



        AddHandler LoadRadio(1893, G, "معدلات الدوران").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                     frm = New RPTFromDateToDate With {.Flag = 10}
                                                                     sender.Tag = New MyPage With {.Content = frm}
                                                                 End Sub


        If Md.MyProjectType = ProjectType.X25 Then
            AddHandler LoadRadio(1880, G, "FinalReports").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                       frm = New RPT40 With {.Flag = 1, .MainTableName = "FinalReportsMain", .lblMain_Content = "FinalReportsMain", .Main2TableName = "FinalReportsSub", .Main2MainId = "FinalReportsMainId", .lblMain2_Content = "FinalReportsSub"}
                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                   End Sub

        End If





        If Md.MyProjectType = ProjectType.X27 Then
            AddHandler LoadRadio(1881, G, "DeficitAndIncrease").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                             frm = New RPT7 With {.Flag = 2}
                                                                             sender.Tag = New MyPage With {.Content = frm}
                                                                         End Sub

            AddHandler LoadRadio(1882, G, "DeficitAndIncreaseComparison").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                                                       frm = New RPT7 With {.Flag = 3}
                                                                                       sender.Tag = New MyPage With {.Content = frm}
                                                                                   End Sub


        End If


        AddHandler LoadRadio(1890, G, "Zakat").Checked, Sub(sender As Object, e As RoutedEventArgs)
                                                            frm = New Zakat
                                                            sender.Tag = New MyPage With {.Content = frm}
                                                        End Sub

        '1893
    End Sub


    Private Sub LoadTabs()
        LoadGFile(1)

        If Md.MyProjectType = ProjectType.X11 OrElse Md.MyProjectType = ProjectType.X1 OrElse Md.MyProjectType = ProjectType.X2 OrElse Md.MyProjectType = ProjectType.X13 OrElse Md.MyProjectType = ProjectType.X14 OrElse Md.MyProjectType = ProjectType.X16 OrElse Md.MyProjectType = ProjectType.X4 OrElse Md.MyProjectType = ProjectType.X5 OrElse Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X18 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.X3 OrElse Md.MyProjectType = ProjectType.X23 OrElse Md.MyProjectType = ProjectType.X24 OrElse Md.MyProjectType = ProjectType.X25 OrElse Md.MyProjectType = ProjectType.X26 OrElse Md.MyProjectType = ProjectType.X27 OrElse Md.MyProjectType = ProjectType.X28 OrElse Md.MyProjectType = ProjectType.Optics OrElse Md.MyProjectType = ProjectType.X8 OrElse Md.MyProjectType = ProjectType.X29 Then
            LoadGStores(5)
        End If


        If Md.ShowSalaries Then
            LoadGSalaries(7)
        End If

        If Md.MyProjectType = ProjectType.X11 OrElse Md.MyProjectType = ProjectType.X2 OrElse Md.MyProjectType = ProjectType.X12 OrElse Md.MyProjectType = ProjectType.X13 OrElse Md.MyProjectType = ProjectType.X14 OrElse Md.MyProjectType = ProjectType.X16 OrElse Md.MyProjectType = ProjectType.X4 OrElse Md.MyProjectType = ProjectType.X5 OrElse Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X18 OrElse Md.MyProjectType = ProjectType.X19 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.X22 OrElse Md.MyProjectType = ProjectType.X3 OrElse Md.MyProjectType = ProjectType.X24 OrElse Md.MyProjectType = ProjectType.X25 OrElse Md.MyProjectType = ProjectType.X26 OrElse Md.MyProjectType = ProjectType.X27 OrElse Md.MyProjectType = ProjectType.X28 OrElse Md.MyProjectType = ProjectType.Optics OrElse Md.MyProjectType = ProjectType.X29 Then
            LoadGAccountants(8)
        End If

        LoadGSecurity(11)


        If Md.MyProjectType = ProjectType.X11 OrElse Md.MyProjectType = ProjectType.X1 OrElse Md.MyProjectType = ProjectType.X2 OrElse Md.MyProjectType = ProjectType.X13 OrElse Md.MyProjectType = ProjectType.X14 OrElse Md.MyProjectType = ProjectType.X16 OrElse Md.MyProjectType = ProjectType.X4 OrElse Md.MyProjectType = ProjectType.X5 OrElse Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X18 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.X3 OrElse Md.MyProjectType = ProjectType.X23 OrElse Md.MyProjectType = ProjectType.X24 OrElse Md.MyProjectType = ProjectType.X25 OrElse Md.MyProjectType = ProjectType.X26 OrElse Md.MyProjectType = ProjectType.X27 OrElse Md.MyProjectType = ProjectType.X28 OrElse Md.MyProjectType = ProjectType.Optics OrElse Md.MyProjectType = ProjectType.X8 OrElse Md.MyProjectType = ProjectType.X29 Then
            LoadGStoresReports(15)
        End If

        If Md.ShowSalaries Then
            LoadGSalaryReports(17)
        End If

        If Md.MyProjectType = ProjectType.X11 OrElse Md.MyProjectType = ProjectType.X2 OrElse Md.MyProjectType = ProjectType.X12 OrElse Md.MyProjectType = ProjectType.X13 OrElse Md.MyProjectType = ProjectType.X14 OrElse Md.MyProjectType = ProjectType.X16 OrElse Md.MyProjectType = ProjectType.X4 OrElse Md.MyProjectType = ProjectType.X5 OrElse Md.MyProjectType = ProjectType.X17 OrElse Md.MyProjectType = ProjectType.X18 OrElse Md.MyProjectType = ProjectType.X19 OrElse Md.MyProjectType = ProjectType.X15 OrElse Md.MyProjectType = ProjectType.X22 OrElse Md.MyProjectType = ProjectType.X3 OrElse Md.MyProjectType = ProjectType.X24 OrElse Md.MyProjectType = ProjectType.X25 OrElse Md.MyProjectType = ProjectType.X26 OrElse Md.MyProjectType = ProjectType.X27 OrElse Md.MyProjectType = ProjectType.X28 OrElse Md.MyProjectType = ProjectType.Optics OrElse Md.MyProjectType = ProjectType.X29 Then
            LoadGAccountsReports(18)
        End If



        'LoadAbout(21)

        'bm.SetModem()



    End Sub



End Class

