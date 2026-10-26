
Partial Class Content_DigiMaster
    Inherits System.Web.UI.MasterPage
    Dim terms As New Literal
    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder


    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim scriptman As New ScriptManager
        If Session("upload") = True Then

            scriptman = ScriptManager1
            scriptman.EnablePartialRendering = False
            Timer1.Enabled = False

        Else
            scriptman.EnablePartialRendering = True
            Timer1.Enabled = True

        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
       
        If Session("usertype") = Nothing Then Response.Redirect("~/default.aspx")
        Try

          
            If Not IsPostBack Then
                get_Updates()
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim path As String = "https://" & Request.Url.Authority & "/"

                If Session("usertype") = "STUDENT" Then
                    Dim studentarray As New ArrayList
                    studentarray.Add("student")
                    Session("roles") = studentarray
                    Dim stumenuitems As New Literal
                    Dim stumobmenuitems As New Literal
                    Dim studentmenu As String = ""
                    studentmenu = studentmenu + "<li class='nav-item'><a href='" + path + "Content/Student/studentprofile.aspx'>PROFILE</span> </a></li>"
                    studentmenu = studentmenu + "<li class='nav-item'><a href='" + path + "Content/Student/assignments.aspx'>ASSIGNMENTS</span> </a></li>"
                    studentmenu = studentmenu + "<li class='nav-item'><a href='" + path + "Content/Student/classdetails.aspx'>CLASS DETAILS</span> </a></li>"
                    studentmenu = studentmenu + "<li class='nav-item'><a href='" + path + "Content/Student/myscores.aspx'>MY SCORES</span> </a></li>"
                    studentmenu = studentmenu + "<li class='nav-item'><a href='" + path + "Content/Student/elearning.aspx'>E-LEARNING</span> </a></li>"
                    studentmenu = studentmenu + "<li class='nav-item'><a href='" + path + "Content/Student/library.aspx'>E-LIBRARY</span> </a></li>"
                    stumenuitems.Text = studentmenu
                    Dim stumobmenu As String = "<li><a href='" + path + "Content/Student/studentprofile.aspx" + "'>PROFILE</a></li>" & _
                                                   "<li><a href='" + path + "Content/Student/assignments.aspx" + "'>ASSIGNMENTS</a></li>" & _
                                                   "<li><a href='" + path + "Content/Student/classdetails.aspx" + "'>CLASS DETAILS</a></li>" & _
                                                   "<li><a href='" + path + "Content/Student/studentprofile.aspx" + "'>MY SCORES</a></li>" & _
                                                   "<li><a href='" + path + "Content/Student/elearning.aspx" + "'>E-LEARNING</a></li>" & _
                                                      "<li><a href='" + path + "Content/Student/library.aspx" + "'>E-LIBRARY</a></li>"


                    PlaceHolder2.Controls.Add(stumenuitems)
                    stumobmenuitems.Text = stumobmenu
                    plcMob.Controls.Add(stumobmenuitems)
                ElseIf Session("usertype") = "STAFF" Then
                    Dim staffarray As New ArrayList
                    staffarray.Add("staff")
                    Dim staffmenu As String
                    Dim stamenuitems As New Literal
                    Dim stamobmenuitems As New Literal
                    staffmenu = "<li class='nav-item'><a href='#' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>STAFF</span><span class='indicator-right-menu mini-dn'><i class='fa indicator-mn fa-angle-left'></i></span></a>" & _
                                      "<div role='menu' class='dropdown-menu left-menu-dropdown animated flipInX'>" & _
                                          "<a href='" + path + "Content/Staff/staffprofile.aspx" + "' class='dropdown-item'>Profile</a>" & _
                                          "<a href='" + path + "Content/Staff/library.aspx" + "' class='dropdown-item'>E-Library</a>" & _
                                          "<a href='" + path + "Content/Staff/messages.aspx" + "' class='dropdown-item'>View Mail</a></div></li>"
                    Dim staffmob As String
                    staffmob = "<li><a data-toggle='collapse' data-target='#demo' href='#'>STAFF <span class='admin-project-icon adminpro-icon adminpro-down-arrow'></span></a>" & _
                                                    "<ul id='demo' class='collapse dropdown-header-top'>" & _
                                                        "<li><a href='" + path + "Content/Staff/profile.aspx" + "'>Profile</a></li>" & _
                                                         "<li><a href='" + path + "Content/Staff/library.aspx" + "'>E-Library</a></li>" & _
                                                        "<li><a href='" + path + "Content/Staff/messages.aspx" + "'>Messaging</a></li></ul></li>"
                    stamenuitems.Text = staffmenu
                    PlaceHolder2.Controls.Add(stamenuitems)
                    stamobmenuitems.Text = staffmob
                    plcMob.Controls.Add(stamobmenuitems)
                    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("select * from admin where username='" & Session("staffid") & "'", con)
                    Dim dr As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
                    If dr.Read = True Then
                        staffarray.Add("admin")
                        Dim admmenuitems As New Literal
                        Dim admmobmenuitems As New Literal
                        Dim adminmenu As String = "<li class='nav-item'><a href='#' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>ADMIN</span><span class='indicator-right-menu mini-dn'><i class='fa indicator-mn fa-angle-left'></i></span></a>" & _
                                    "<div role='menu' class='dropdown-menu left-menu-dropdown animated flipInX' style='top:-250px;padding:4px;'>" & _
                                        "<a href='" + path + "Content/admin/messages.aspx" + "' class='dropdown-item'>Messaging</a>" & _
                                        "<a href='" + path + "Content/admin/newsession.aspx" + "' class='dropdown-item'>Manage Sessions</a>" & _
                                         "<a href='" + path + "Content/admin/newterm.aspx" + "' class='dropdown-item'>Manage Terms</a>" & _
                                          "<a href='" + path + "Content/admin/addsubject.aspx" + "' class='dropdown-item'>Manage Subjects</a>" & _
                                           "<a href='" + path + "Content/admin/departments.aspx" + "' class='dropdown-item'>Manage Departments</a>" & _
                                            "<a href='" + path + "Content/admin/manageclass.aspx" + "' class='dropdown-item'>Manage Classes</a>" & _
                                             "<a href='" + path + "Content/admin/staffprofile.aspx" + "' class='dropdown-item'>Manage Staff</a>" & _
                                              "<a href='" + path + "Content/admin/transport.aspx" + "' class='dropdown-item'>Manage Transport</a>" & _
                                               "<a href='" + path + "Content/admin/boarding.aspx" + "' class='dropdown-item'>Manage Hostels</a>" & _
                                                "<a href='" + path + "Content/admin/managegrades.aspx" + "' class='dropdown-item'>Manage Grades</a>" & _
                                                 "<a href='" + path + "Content/admin/studentprofile.aspx" + "' class='dropdown-item'>Manage Students</a>" & _
                                                  "<a href='" + path + "Content/admin/parentprofile.aspx" + "' class='dropdown-item'>Manage Parents</a>" & _
                                                   "<a href='" + path + "Content/admin/publishca.aspx" + "' class='dropdown-item'>Publish Assessments</a>" & _
                                                    "<a href='" + path + "Content/admin/managetimetable.aspx" + "' class='dropdown-item'>Manage Time Tables</a>" & _
                                                    "<a href='" + path + "Content/admin/library.aspx" + "' class='dropdown-item'>Manage Library</a>" & _
                                                    "<a href='" + path + "Content/admin/subjectnest.aspx" + "' class='dropdown-item'>Subject Nests</a>" & _
                                                     "<a href='" + path + "Content/admin/stock.aspx" + "' class='dropdown-item'>Manage Stock</a>" & _
                                        "<a href='" + path + "Content/admin/managetraits.aspx" + "' class='dropdown-item'>Manage Traits</a></div></li>"

                        Dim adminmob As String
                        adminmob = "<li><a data-toggle='collapse' data-target='#demo' href='#'>ADMIN<span class='admin-project-icon adminpro-icon adminpro-down-arrow'></span></a>" & _
                                                        "<ul id='demo' class='collapse dropdown-header-top'>" & _
                                                            "<li><a href='" + path + "Content/admin/messages.aspx" + "'>Messaging</a></li>" & _
                                                            "<li><a href='" + path + "Content/Admin/newsession.aspx" + "'>Manage Sessions</a></li>" & _
                                                            "<li><a href='" + path + "Content/admin/newterm.aspx" + "'>Manage Terms</a></li>" & _
                                                            "<li><a href='" + path + "Content/admin/addsubject.aspx" + "'>Manage Subjects</a></li>" & _
                                                            "<li><a href='" + path + "Content/admin/departments.aspx" + "'>Manage Departments</a></li>" & _
                                                            "<li><a href='" + path + "Content/admin/manageclass.aspx" + "'>Manage Classes</a></li>" & _
                                                            "<li><a href='" + path + "Content/admin/staffprofile.aspx" + "'>Manage Staff</a></li>" & _
                                                            "<li><a href='" + path + "Content/admin/transport.aspx" + "'>Manage Transport</a></li>" & _
                                                            "<li><a href='" + path + "Content/admin/managegrades.aspx" + "'>Manage Grades</a></li>" & _
                                                            "<li><a href='" + path + "Content/admin/studentprofile.aspx" + "'>Manage Students</a></li>" & _
                                                            "<li><a href='" + path + "Content/admin/parentprofile.aspx" + "'>Manage Parents</a></li>" & _
                                                            "<li><a href='" + path + "Content/admin/publishca.aspx" + "'>Publish Assessments</a></li>" & _
                                                            "<li><a href='" + path + "Content/admin/managetimetable.aspx" + "'>Manage Time Tables</a></li>" & _
                                                            "<li><a href='" + path + "Content/admin/managetraits.aspx" + "'>Manage Traits</a></li>" & _
                                                            "<li><a href='" + path + "Content/admin/subjectnest.aspx" + "'>Subject Nest</a></li>" & _
                                                            "<li><a href='" + path + "Content/admin/library.aspx" + "'>Manage Library/a></li></ul></li>"


                        admmenuitems.Text = adminmenu
                        PlaceHolder2.Controls.Add(admmenuitems)
                        admmobmenuitems.Text = adminmob
                        plcMob.Controls.Add(admmobmenuitems)
                    End If
                    dr.Close()
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("select * from acclogin where username='" & Session("staffid") & "'", con)
                    Dim dr2 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                    If dr2.Read = True Then
                        staffarray.Add("account")
                        Dim accmenuitems As New Literal
                        Dim accmobmenuitems As New Literal
                        Dim accmenu As String = "<li class='nav-item'><a href='#' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>ACCOUNTS</span><span class='indicator-right-menu mini-dn'><i class='fa indicator-mn fa-angle-left'></i></span></a>" & _
                               "<div role='menu' class='dropdown-menu left-menu-dropdown animated flipInX'>" & _
                                   "<a href='" + path + "Content/account/messages.aspx" + "' class='dropdown-item'>Messaging</a>" & _
                                   "<a href='" + path + "Content/account/accsettings.aspx" + "' class='dropdown-item'>Account Settings</a>" & _
                                   "<a href='" + path + "Content/account/staffprofile.aspx" + "' class='dropdown-item'>Manage Staff</a>" & _
                                   "<a href='" + path + "Content/account/parentprofile.aspx" + "' class='dropdown-item'>Manage Parents</a>" & _
                                    "<a href='" + path + "Content/account/mngfees.aspx" + "' class='dropdown-item'>Manage Fees</a>" & _
                                     "<a href='" + path + "Content/account/feeboard.aspx" + "' class='dropdown-item'>Student Fees</a>" & _
                                      "<a href='" + path + "Content/account/discount.aspx" + "' class='dropdown-item'>Manage Discounts</a>" & _
                                       "<a href='" + path + "Content/account/transactions.aspx" + "' class='dropdown-item'>Manage Transactions</a>" & _
                                        "<a href='" + path + "Content/account/salary.aspx" + "' class='dropdown-item'>Mange Salary</a></div></li>"
                        Dim accmob As String
                        accmob = "<li><a data-toggle='collapse' data-target='#demo' href='#'>ACCOUNTS<span class='admin-project-icon adminpro-icon adminpro-down-arrow'></span></a>" & _
                                                        "<ul id='demo' class='collapse dropdown-header-top'>" & _
                                                            "<li><a href='" + path + "Content/account/messages.aspx" + "'>Messaging</a></li>" & _
                                                            "<li><a href='" + path + "Content/account/accsettings.aspx" + "'>Account Settings</a></li>" & _
                                                            "<li><a href='" + path + "Content/account/mngfees.aspx" + "'>Manage Fees</a></li>" & _
                                                            "<li><a href='" + path + "Content/account/staffprofile.aspx" + "'>Manage Staff</a></li>" & _
                                                            "<li><a href='" + path + "Content/account/parentprofile.aspx" + "'>Manage Parents</a></li>" & _
                                                            "<li><a href='" + path + "Content/account/feeboard.aspx" + "'>Student Fees</a></li>" & _
                                                            "<li><a href='" + path + "Content/account/discount.aspx" + "'>Manage Discounts</a></li>" & _
                                                            "<li><a href='" + path + "Content/account/transactions.aspx" + "'>Manage Transactions</a></li>" & _
                                                            "<li><a href='" + path + "Content/account/salary.aspx" + "'>Manage Salary</a></li></ul></li>"
                        accmenuitems.Text = accmenu
                        PlaceHolder2.Controls.Add(accmenuitems)
                        accmobmenuitems.Text = accmob
                        plcMob.Controls.Add(accmobmenuitems)
                    End If
                    dr2.Close()
                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("select * from classteacher where teacher='" & Session("staffid") & "'", con)
                    Dim dr3 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader
                    If dr3.Read = True Then
                        staffarray.Add("classteacher")
                        Dim ctmenuitems As New Literal
                        Dim ctmobmenuitems As New Literal
                        Dim ctmenu As String = "<li class='nav-item'><a href='#' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>CLASS TEACHER</span><span class='indicator-right-menu mini-dn'><i class='fa indicator-mn fa-angle-left'></i></span></a>" & _
                                  "<div role='menu' class='dropdown-menu left-menu-dropdown animated flipInX'>" & _
                                      "<a href='" + path + "Content/staff/classdetails.aspx" + "' class='dropdown-item'>Manage Class</a>" & _
                                      "<a href='" + path + "Content/staff/studentprofile.aspx" + "' class='dropdown-item'>Students Profile</a>" & _
                                       "<a href='" + path + "Content/staff/parentprofile.aspx" + "' class='dropdown-item'>Parents Profile</a>" & _
                                        "<a href='" + path + "Content/staff/feeboard.aspx" + "' class='dropdown-item'>Student Fees</a>" & _
                                         "<a href='" + path + "Content/staff/studentsubjects.aspx" + "' class='dropdown-item'>Students Subjects</a>" & _
                                          "<a href='" + path + "Content/staff/attendance.aspx" + "' class='dropdown-item'>Students Attendance</a>" & _
                                         "<a href='" + path + "Content/staff/childscores.aspx" + "' class='dropdown-item'>Students Scores</a>" & _
                                        "<a href='" + path + "Content/staff/studentbehaviour.aspx" + "' class='dropdown-item'>Students Behaviour</a></div></li>"
                        Dim ctmob As String
                        ctmob = "<li><a data-toggle='collapse' data-target='#demo' href='#'>CLASS TEACHER<span class='admin-project-icon adminpro-icon adminpro-down-arrow'></span></a>" & _
                                                        "<ul id='demo' class='collapse dropdown-header-top'>" & _
                                                            "<li><a href='" + path + "Content/staff/classdetails.aspx" + "'>Manage Class</a></li>" & _
                                                            "<li><a href='" + path + "Content/staff/studentprofile.aspx" + "'>Students Profile</a></li>" & _
                                                            "<li><a href='" + path + "Content/staff/parentprofile.aspx" + "'>Parents Profile</a></li>" & _
                                                            "<li><a href='" + path + "Content/staff/feeboard.aspx" + "'>Student Fees</a></li>" & _
                                                            "<li><a href='" + path + "Content/staff/studentsubjects.aspx" + "'>Students Subjects</a></li>" & _
                                                            "<li><a href='" + path + "Content/staff/attendance.aspx" + "'>Students Attendance</a></li>" & _
                                                            "<li><a href='" + path + "Content/staff/childscores.aspx" + "'>Students Scores</a></li>" & _
                                                            "<li><a href='" + path + "Content/staff/studentbehaviour.aspx" + "'>Students Behaviour</a></li></ul></li>"

                        ctmenuitems.Text = ctmenu
                        PlaceHolder2.Controls.Add(ctmenuitems)
                        ctmobmenuitems.Text = ctmob
                        plcMob.Controls.Add(ctmobmenuitems)
                    End If
                    dr3.Close()
                    Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("select * from classsubjects where teacher='" & Session("staffid") & "'", con)
                    Dim dr4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader
                    If dr4.Read = True Then
                        staffarray.Add("subjectteacher")
                        Dim stmenuitems As New Literal
                        Dim stmobmenuitems As New Literal
                        Dim stmenu As String = "<li class='nav-item'><a href='#' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>SUBJECT TEACHER</span><span class='indicator-right-menu mini-dn'><i class='fa indicator-mn fa-angle-left'></i></span></a>" & _
                                                "<div role='menu' class='dropdown-menu left-menu-dropdown animated flipInX'>" & _
                                                    "<a href='" + path + "Content/staff/courseoverview.aspx" + "' class='dropdown-item'>Course Overview</a>" & _
                                                    "<a href='" + path + "Content/staff/newcourseoutline.aspx" + "' class='dropdown-item'>Course Outlines</a>" & _
                                                     "<a href='" + path + "Content/staff/timetable.aspx" + "' class='dropdown-item'>Time Table</a>" & _
                                                      "<a href='" + path + "Content/staff/myplans.aspx" + "' class='dropdown-item'>Lesson Plan</a>" & _
                                                       "<a href='" + path + "Content/staff/assignments.aspx" + "' class='dropdown-item'>Students Assignments</a>" & _
                                                        "<a href='" + path + "Content/staff/scoresheet.aspx" + "' class='dropdown-item'>Students Scores</a>" & _
                                                       "<a href='" + path + "Content/staff/elearning.aspx" + "' class='dropdown-item'>E-Learning</a></div></li>"
                        Dim stmob As String
                        stmob = "<li><a data-toggle='collapse' data-target='#demo' href='#'>SUBJECT TEACHER<span class='admin-project-icon adminpro-icon adminpro-down-arrow'></span></a>" & _
                                                        "<ul id='demo' class='collapse dropdown-header-top'>" & _
                                                            "<li><a href='" + path + "Content/staff/courseoverview.aspx" + "'>Course Overview</a></li>" & _
                                                            "<li><a href='" + path + "Content/staff/newcourseoutline.aspx" + "'>Course Outline</a></li>" & _
                                                            "<li><a href='" + path + "Content/staff/timetable.aspx" + "'>Time Table</a></li>" & _
                                                            "<li><a href='" + path + "Content/staff/myplans.aspx" + "'>Lesson Plan</a></li>" & _
                                                            "<li><a href='" + path + "Content/staff/assignments.aspx" + "'>Students Assignments</a></li>" & _
                                                            "<li><a href='" + path + "Content/staff/scoresheet.aspx" + "'>Students Scores</a></li>" & _
                                                            "<li><a href='" + path + "Content/staff/elearning.aspx" + "'>E-Learning</a></li></ul></li>"

                        stmenuitems.Text = stmenu
                        PlaceHolder2.Controls.Add(stmenuitems)
                        stmobmenuitems.Text = stmob
                        plcMob.Controls.Add(stmobmenuitems)
                    End If
                    dr4.Close()
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept, headtitle, superior from depts where head = '" & Session("staffid") & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    Dim super As Boolean = False
                    Dim deptshead As New ArrayList
                    Do While student0.Read
                        deptshead.Add(student0.Item(0))
                        If student0(2) = "None" Then super = True
                    Loop
                    student0.Close()
                    Dim classcontroled As New ArrayList
                    Dim secsub As New ArrayList
                    Dim mysub As New ArrayList
                    For Each item As String In deptshead
                        classcontroled.Add(item)
                        mysub = Get_subordinates(item)

                        For Each subitem As String In mysub
                            classcontroled.Add(subitem)
                            secsub.Add(subitem)
                        Next
                    Next
                    Dim thirdsub As New ArrayList
                    For Each item As String In secsub

                        mysub = Get_subordinates(item)
                        For Each subitem As String In mysub
                            classcontroled.Add(subitem)
                            thirdsub.Add(subitem)
                        Next
                    Next
                    Dim fourthsub As New ArrayList
                    For Each item As String In thirdsub

                        mysub = Get_subordinates(item)
                        For Each subitem As String In mysub
                            classcontroled.Add(subitem)
                            fourthsub.Add(subitem)
                        Next
                    Next
                    Dim fifthsub As New ArrayList
                    For Each item As String In fourthsub

                        mysub = Get_subordinates(item)
                        For Each subitem As String In mysub
                            classcontroled.Add(subitem)
                        Next
                    Next
                    Dim classgroups As New ArrayList
                    For Each item As String In deptshead
                        Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                        Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                        Dim d As Boolean = False
                        Do While schclass.Read
                            d = True
                        Loop
                        If d = True Then
                            classgroups.Add(item)
                        End If
                        schclass.Close()
                    Next
                    Dim teachingstaff As New ArrayList
                    Dim deptstaff As New ArrayList
                    For Each item As String In classcontroled
                        Dim classes1 As New MySql.Data.MySqlClient.MySqlCommand("Select staff from staffdept inner join depts on staffdept.dept = depts.id where depts.dept = '" & item & "'", con)
                        Dim schclass1 As MySql.Data.MySqlClient.MySqlDataReader = classes1.ExecuteReader
                        Do While schclass1.Read
                            deptstaff.Add(schclass1.Item(0))
                        Loop
                        schclass1.Close()
                        For Each sitem As String In deptstaff
                            Dim classes11 As New MySql.Data.MySqlClient.MySqlCommand("Select teacher from classsubjects where teacher = '" & sitem & "'", con)
                            Dim schclass11 As MySql.Data.MySqlClient.MySqlDataReader = classes11.ExecuteReader
                            If schclass11.Read Then
                                teachingstaff.Add(sitem)
                            End If
                            schclass11.Close()
                        Next
                    Next
                    For Each dept As String In deptshead
                        classcontroled.Remove(dept)
                    Next
                    Dim subclassgroups As New ArrayList
                    For Each item As String In classcontroled
                        Dim f As Boolean = False
                        Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                        Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                        Do While schclass.Read
                            f = True
                        Loop
                        If f = True Then
                            subclassgroups.Add(item)
                        End If
                        schclass.Close()
                    Next
                    If teachingstaff.Count <> 0 Then
                        staffarray.Add("depthead")
                        Dim dhmenuitems As New Literal
                        Dim dhmobmenuitems As New Literal
                        Dim dhmenu As String = "<li class='nav-item'><a href='#' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>DEPARTMENT HEAD</span><span class='indicator-right-menu mini-dn'><i class='fa indicator-mn fa-angle-left'></i></span></a>" & _
                                "<div role='menu' class='dropdown-menu left-menu-dropdown animated flipInX'>" & _
                                    "<a href='" + path + "Content/staff/submittedplans.aspx" + "' class='dropdown-item'>Check Lesson Plans</a>" & _
                                    "<a href='" + path + "Content/staff/dcourseoverview.aspx" + "' class='dropdown-item'>Check Course Overview</a>" & _
                                     "<a href='" + path + "Content/staff/dcourseoutline.aspx" + "' class='dropdown-item'>Check Course Outlines</a>" & _
                                      "<a href='" + path + "Content/staff/dtimetable.aspx" + "' class='dropdown-item'>Check Time Tables</a>" & _
                                       "<a href='" + path + "Content/staff/dassignments.aspx" + "' class='dropdown-item'>Check Assignments</a>" & _
                                        "<a href='" + path + "Content/staff/dscoresheet.aspx" + "' class='dropdown-item'>Check Scores</a></div></li>"
                        Dim dhmob As String
                        dhmob = "<li><a data-toggle='collapse' data-target='#demo' href='#'>DEPARTMENT HEAD<span class='admin-project-icon adminpro-icon adminpro-down-arrow'></span></a>" & _
                                        "<ul id='demo' class='collapse dropdown-header-top'>" & _
                                            "<li><a href='" + path + "Content/staff/submittedplans.aspx" + "'>Check Lesson Plans</a></li>" & _
                                            "<li><a href='" + path + "Content/staff/dcourseoverview.aspx" + "'>Check Course Overview</a></li>" & _
                                            "<li><a href='" + path + "Content/staff/dcourseoutline.aspx" + "'>Check Course Outlines</a></li>" & _
                                            "<li><a href='" + path + "Content/staff/dtimetable.aspx" + "'>Check Time Tables</a></li>" & _
                                            "<li><a href='" + path + "Content/staff/dassignments.aspx" + "'>Check Assignments</a></li>" & _
                                            "<li><a href='" + path + "Content/staff/dscoresheet.aspx" + "'>Check Scores</a></li></ul></li>"


                        dhmobmenuitems.Text = dhmob
                        plcMob.Controls.Add(dhmobmenuitems)

                        dhmenuitems.Text = dhmenu
                        PlaceHolder2.Controls.Add(dhmenuitems)

                    End If
                    If classgroups.Count <> 0 Or subclassgroups.Count <> 0 Then
                        staffarray.Add("schoolhead")
                        Dim shmenuitems As New Literal
                        Dim shmobmenuitems As New Literal
                        Dim shmenu As String = "<li class='nav-item'><a href='#' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>SCHOOL HEAD</span><span class='indicator-right-menu mini-dn'><i class='fa indicator-mn fa-angle-left'></i></span></a>" & _
                                "<div role='menu' class='dropdown-menu left-menu-dropdown animated flipInX'>" & _
                                    "<a href='" + path + "Content/staff/checkattendance.aspx" + "' class='dropdown-item'>Check Attendance</a>" & _
                                    "<a href='" + path + "Content/staff/sclassdetails.aspx" + "' class='dropdown-item'>Class Details</a>" & _
                                    "<a href='" + path + "Content/staff/sstudentprofile.aspx" + "' class='dropdown-item'>Students Profile</a>" & _
                                      "<a href='" + path + "Content/staff/sparentprofile.aspx" + "' class='dropdown-item'>Parents Profile</a>" & _
                                    "<a href='" + path + "Content/staff/schildscores.aspx" + "' class='dropdown-item'>Students Scores</a>" & _
                                     "<a href='" + path + "Content/staff/ssentmsgs.aspx" + "' class='dropdown-item'>Check Messages</a></div></li>"

                        Dim shmob As String
                        shmob = "<li><a data-toggle='collapse' data-target='#demo' href='#'>SCHOOL HEAD<span class='admin-project-icon adminpro-icon adminpro-down-arrow'></span></a>" & _
                                        "<ul id='demo' class='collapse dropdown-header-top'>" & _
                                            "<li><a href='" + path + "Content/staff/checkattendance.aspx" + "'>Check Attendance</a></li>" & _
                                            "<li><a href='" + path + "Content/staff/sclassdetails.aspx" + "'>Class Details</a></li>" & _
                                            "<li><a href='" + path + "Content/staff/sstudentprofile.aspx" + "'>Students Profile</a></li>" & _
                                             "<li><a href='" + path + "Content/staff/sparentprofile.aspx" + "'>Parents Profile</a></li>" & _
                                             "<li><a href='" + path + "Content/staff/schildscores.aspx" + "'>Students Scores</a></li>" & _
                                            "<li><a href='" + path + "Content/staff/ssentmsgs.aspx" + "'>Check Messages</a></li></ul></li>"
                        shmobmenuitems.Text = shmob
                        plcMob.Controls.Add(shmobmenuitems)

                        shmenuitems.Text = shmenu
                        PlaceHolder2.Controls.Add(shmenuitems)
                    End If
                    If super = True Then
                        staffarray.Add("prop")
                        Dim phmenuitems As New Literal
                        Dim phmobmenuitems As New Literal
                        Dim phmenu As String = "<li class='nav-item'><a href='#' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>OVERALL HEAD</span><span class='indicator-right-menu mini-dn'><i class='fa indicator-mn fa-angle-left'></i></span></a>" & _
                                "<div role='menu' class='dropdown-menu left-menu-dropdown animated flipInX'>" & _
                                    "<a href='" + path + "Content/staff/ssentmsgs.aspx" + "' class='dropdown-item'>Subodinates Messages</a>" & _
                                    "<a href='" + path + "Content/account/projections.aspx" + "' class='dropdown-item'>Financial Projections</a>" & _
                                    "<a href='" + path + "Content/staff/sstudentprofile.aspx" + "' class='dropdown-item'>Students Profile</a>" & _
                                      "<a href='" + path + "Content/staff/sparentprofile.aspx" + "' class='dropdown-item'>Parents Profile</a>" & _
                                       "<a href='" + path + "Content/staff/schildscores.aspx" + "' class='dropdown-item'>Students Scores</a>" & _
                                    "<a href='" + path + "Content/account/transactions.aspx" + "' class='dropdown-item'>Transactions</a>"

                        Dim phmob As String
                        phmob = "<li><a data-toggle='collapse' data-target='#demo' href='#'>OVERALL HEAD<span class='admin-project-icon adminpro-icon adminpro-down-arrow'></span></a>" & _
                                        "<ul id='demo' class='collapse dropdown-header-top'>" & _
                                            "<li><a href='" + path + "Content/staff/ssentmsgs.aspx" + "'>Subodinates Messages</a></li>" & _
                                            "<li><a href='" + path + "Content/account/projections.aspx" + "'>Financial Projections</a></li>" & _
                                            "<li><a href='" + path + "Content/staff/sstudentprofile.aspx" + "'>Students Profile</a></li>" & _
                                             "<li><a href='" + path + "Content/staff/sparentprofile.aspx" + "'>Parents Profile</a></li>" & _
                                              "<li><a href='" + path + "Content/staff/schildscores.aspx" + "'>Students Scores</a></li>" & _
                                             "<li><a href='" + path + "Content/account/transactions.aspx" + "'>Transactions</a></li>"
                        phmobmenuitems.Text = phmob
                        plcMob.Controls.Add(phmobmenuitems)
                        phmenuitems.Text = phmenu
                        PlaceHolder2.Controls.Add(phmenuitems)
                    End If
                    Dim cmd2sd As New MySql.Data.MySqlClient.MySqlCommand("select * from lib where username='" & Session("staffid") & "'", con)
                    Dim dr2sd As MySql.Data.MySqlClient.MySqlDataReader = cmd2sd.ExecuteReader
                    If dr2sd.Read = True Then
                        staffarray.Add("lib")
                        Dim laccmenuitems As New Literal
                        Dim laccmobmenuitems As New Literal
                        Dim laccmenu As String = "<li><a href='" + path + "Content/admin/library.aspx'>MANAGE LIBRARY</a></li>"

                        Dim laccmob As String
                        laccmob = "<li><a href='" + path + "Content/admin/library.aspx" + "'>MANAGE LIBRARY</a></li>"
                        laccmenuitems.Text = laccmenu
                        PlaceHolder2.Controls.Add(laccmenuitems)
                        laccmobmenuitems.Text = laccmob
                        plcMob.Controls.Add(laccmobmenuitems)
                    End If
                    dr2sd.Close()
                    Session("roles") = staffarray

                ElseIf Session("usertype") = "PARENT" Then
                    Dim parentarray As New ArrayList
                    parentarray.Add("parent")
                    Session("roles") = parentarray
                    Dim pmenuitems As New Literal
                    Dim pmobmenuitems As New Literal
                    Dim parentmenu As String = ""
                    parentmenu = parentmenu + "<li><a href='" + path + "Content/parent/parentprofile.aspx'>PROFILE</a></li>"
                    parentmenu = parentmenu + "<li ><a href='" + path + "Content/parent/messages.aspx'>MESSAGES</span> </a></li>"
                    parentmenu = parentmenu + "<li ><a href='" + path + "Content/parent/studentprofile.aspx'>CHILDREN/WARDS</span> </a></li>"
                    parentmenu = parentmenu + "<li ><a href='" + path + "Content/parent/feeboard.aspx'>FEES</span> </a></li>"
                    parentmenu = parentmenu + "<li ><a href='" + path + "Content/parent/classdetails.aspx'>CLASSES/COURSES</span> </a></li>"
                    parentmenu = parentmenu + "<li ><a href='" + path + "Content/parent/assignments.aspx'>ASSIGNMENTS</span> </a></li>"
                    parentmenu = parentmenu + "<li ><a href='" + path + "Content/parent/childscores.aspx'>SCORES</span> </a></li>"

                    Dim pmob As String
                    pmob = "<li><a href='" + path + "Content/parent/parentprofile.aspx" + "'>PROFILE</a></li>" & _
                                                     "<li><a href='" + path + "Content/parent/messages.aspx" + "'>MESSAGING</a></li>" & _
                                                     "<li><a href='" + path + "Content/parent/studentprofile.aspx" + "'>CHILDREN/WARDS</a></li>" & _
                                                     "<li><a href='" + path + "Content/parent/feeboard.aspx" + "'>FEES</a></li>" & _
                                                     "<li><a href='" + path + "Content/parent/classdetails.aspx" + "'>CLASS DETAILS</a></li>" & _
                                                      "<li><a href='" + path + "Content/parent/assignments.aspx" + "'>ASSIGNMENTS</a></li>" & _
                                                   "<li><a href='" + path + "Content/parent/childscores.aspx" + "'>SCORES</a></li>"

                    pmobmenuitems.Text = pmob
                    plcMob.Controls.Add(pmobmenuitems)



                    pmenuitems.Text = parentmenu
                    PlaceHolder2.Controls.Add(pmenuitems)

                ElseIf Session("usertype") = "SUPER ADMIN" Then
                    Dim sadminarray As New ArrayList
                    sadminarray.Add("admin")

                    Session("roles") = sadminarray

                End If
                Dim passpath As String = "https://" & Request.Url.Authority
                If Session("passport") = Nothing Then
                    imgProf.Src = path + "img/noimage.jpg"

                Else
                    imgProf.Src = passpath + Replace(Session("passport"), "~", "")
                End If
                lblUsername.Text = Session("UserName")
                lblUserType.Text = Session("usertype")

                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT smsname from options", con)

                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                If reader3.Read() Then lblSchool.Text = reader3(0).ToString
                reader3.Close()
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session order by id desc", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                Dim termtext As String = ""
                Dim url As String = ""
                If Session("usertype") = "STAFF" Then
                    url = path + "content/dashboards/staffdashboard.aspx"
                ElseIf Session("usertype") = "STUDENT" Then
                    url = path + "content/dashboards/studentdashboard.aspx"
                ElseIf Session("usertype") = "PARENT" Then
                    url = path + "content/dashboards/parentdashboard.aspx"
                End If
                Do While reader2.Read()
                    If reader2(0).ToString = Session("sessionid") Then
                        lblSession.Text = reader2(1) & " - " + reader2(2)
                    End If
                    termtext = termtext & "<li><a href='" + url + "?term=" + reader2(0).ToString + "'><span class='adminpro-icon adminpro-money author-log-ic'></span>" + reader2(1) & " - " + reader2(2) + "</a></li>"
                Loop
                terms.Text = termtext
                PlaceHolder1.Controls.Add(terms)
                con.close()
            End Using


            If Request.QueryString("term") <> Nothing Then
                Session("Sessionid") = Request.QueryString("term")
                Session("studentadd") = Nothing
                Session("staffadd") = Nothing
                Session("studentadd") = Nothing
                Response.Redirect(Request.Path)


            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub

    Private Sub Show_Alert(type As Boolean, msg As String)
        alertPLC = plcAlert
        If type = True Then
            alert.Text = alertmsg.success_message(msg)
        Else
            alert.Text = alertmsg.error_message(msg)
        End If
        alertPLC.Controls.Add(alert)
    End Sub

    Public Function Get_subordinates(ByVal dept As String) As ArrayList
        Dim subo As New ArrayList
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept from depts where superior = '" & dept & "'", con)
            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Do While student1.Read
                subo.Add(student1.Item(0))
            Loop
            student1.Close()
            con.Close()
        End Using
        Return subo
    End Function
    Protected Sub lnkLogOut_Click(sender As Object, e As EventArgs) Handles lnkLogOut.Click
        Session.Abandon()
        Response.Redirect("~/default.aspx")
    End Sub

    Private Sub get_Updates()

        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim notmsg As String = ""
            Dim user As String = ""
            Dim ty As String = "Message"
            Select Case Session("usertype")
                Case "STAFF"
                    user = Session("staffid")
                Case "STUDENT"
                    user = Session("studentid")
                    ty = "assignment"
                Case "PARENT"
                    user = Session("parentid")
            End Select

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from notifications where recipient = '" & user & "' order by time desc", con)
            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim notno As Integer = 0
            Dim origin As New ArrayList
            Dim types As New ArrayList
            Dim path As String = "https://" & Request.Url.Authority
            Do While student1.Read
                origin.Add(student1("origin").ToString)
                types.Add(student1("type").ToString)
            Loop
            student1.Close()
            Dim passportsnot As New ArrayList
            Dim passports As New ArrayList
            Dim passportsmsg As New ArrayList
            Dim b As Integer = 0
            For Each item As String In origin
                If types(b) = ty Then
                    Dim cmdLoada As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from studentsprofile where admno = '" & item & "'", con)
                    Dim studenta As MySql.Data.MySqlClient.MySqlDataReader = cmdLoada.ExecuteReader
                    If studenta.Read() Then passportsmsg.Add(studenta.Item(0).ToString)
                    studenta.Close()
                    Dim cmdLoad11a As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from staffprofile where staffid = '" & item & "'", con)
                    Dim student1ca As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11a.ExecuteReader
                    If student1ca.Read() Then passportsmsg.Add(student1ca.Item(0).ToString)
                    student1ca.Close()
                    Dim cmdLoad11xa As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from parentprofile where parentid = '" & item & "'", con)
                    Dim student1xa As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11xa.ExecuteReader
                    If student1xa.Read() Then passportsmsg.Add(student1xa.Item(0).ToString)
                    student1xa.Close()

                Else
                    Dim cmdLoada As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from studentsprofile where admno = '" & item & "'", con)
                    Dim studenta As MySql.Data.MySqlClient.MySqlDataReader = cmdLoada.ExecuteReader
                    If studenta.Read() Then passportsnot.Add(studenta.Item(0).ToString)
                    studenta.Close()
                    Dim cmdLoad11a As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from staffprofile where staffid = '" & item & "'", con)
                    Dim student1ca As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11a.ExecuteReader
                    If student1ca.Read() Then passportsnot.Add(student1ca.Item(0).ToString)
                    student1ca.Close()
                    Dim cmdLoad11xa As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from parentprofile where parentid = '" & item & "'", con)
                    Dim student1xa As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11xa.ExecuteReader
                    If student1xa.Read() Then passportsnot.Add(student1xa.Item(0).ToString)
                    student1xa.Close()
                End If
                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from studentsprofile where admno = '" & item & "'", con)
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                If student.Read() Then passports.Add(student.Item(0).ToString)
                student.Close()
                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from staffprofile where staffid = '" & item & "'", con)
                Dim student1c As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                If student1c.Read() Then passports.Add(student1c.Item(0).ToString)
                student1c.Close()
                Dim cmdLoad11x As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from parentprofile where parentid = '" & item & "'", con)
                Dim student1x As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11x.ExecuteReader
                If student1x.Read() Then passports.Add(student1x.Item(0).ToString)
                student1x.Close()
                b = b + 1
            Next
            Dim student1s As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim notno2 As Integer = 0
            Do While student1s.Read
                If student1s("type").ToString <> ty Then
                    Dim dob As Date = student1s("time")
                    Dim sage As TimeSpan = Now.Subtract(dob)
                    Dim timelag As String = ""
                    If sage.Days < 1 Then
                        If sage.Hours < 1 Then
                            If sage.Minutes < 1 Then
                                timelag = "Just now"
                            ElseIf sage.Minutes = 1 Then
                                timelag = 1 & " Min ago"
                            Else
                                timelag = sage.Minutes & " Mins ago"
                            End If
                        ElseIf sage.Hours = 1 Then
                            timelag = "1 hour ago"
                        Else
                            timelag = sage.Hours & " Hrs ago"
                        End If
                    ElseIf sage.Days = 1 Then
                        timelag = "1 Day ago"
                    Else
                        timelag = sage.Days & " Days ago"
                    End If
                    Dim pass As String = path + "/img/noimage.jpg"
                    If passports(notno2) <> "" Then
                        pass = path + Replace(passports(notno2), "~", "")
                    End If
                    notmsg = notmsg & "<li " & IIf(student1s("status") = "Unread", "style='background-color:#ebebeb;'", "") & "><a href='" & path + Replace(student1s("url"), "~", "") & "'><div class='message-img' >" & _
                                           "<img  src='" & pass + "' alt=''></div><div class='message-content' id='" & notno & "msg" & "' style='width:280px;" & IIf(student1s("status") = "Unread", "color:#1f2c79;", "") & "'>" & _
                                           "<span class='message-date'>" & timelag & "</span><h2 style='width:70%;'>" & student1s("relationship") & "</h2>" & _
                                           "<p style='font-size:small;'>" & student1s("message") & "</p></div></a></li>"
                    If student1s("status") = "Unread" Then
                        notno = notno + 1
                    End If
                End If
                notno2 = notno2 + 1
            Loop
            student1s.Close()

            Dim updated As Boolean = False

            If Session("notified") = 0 Then
                Session("notified") = notno2

                popup.Visible = False
            ElseIf Session("notified") < notno2 Then
                Dim cmdLoad1s As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from notifications where notifications.status = '" & "Unread" & "' and notifications.recipient = '" & user & "' order by time desc", con)
                Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1s.ExecuteReader
                Dim thiscountnot As Integer = 1

                Do While student10.Read
                    If thiscountnot = notno2 - Val(Session("notified")) Then
                        Dim pass As String = path + "/img/noimage.jpg"
                        If passports(thiscountnot - 1) <> "" Then
                            pass = path + Replace(passports(thiscountnot - 1), "~", "")

                        End If
                        notimg.Src = pass
                        lblNOTTitle.Text = student10("relationship").ToString
                        lblNotBody.Text = student10("message").ToString
                        updated = True
                        Session("notified") = Val(Session("notified")) + 1
                        Dim sound As New Literal
                        sound.Text = "<div visible = 'false'><audio autoplay = 'true'> <source src='" & path & "/Sounds/sound1.ogg" & "' type 'audio/ogg' ></audio></div>"
                        plcNot.Controls.Add(sound)
                        Exit Do
                    End If
                    thiscountnot = thiscountnot + 1
                Loop
                student10.Close()
            Else
            End If
            If updated = True Then
                popup.Visible = True

            End If

            notifications.Text = ""
            If notno > 1 Then
                lblNot.Text = notno & " unread notifications"
                notifications.Text = "<span class='badge' style='background-color:red;'>" & notno & "</span>"
            ElseIf notno = 1 Then
                lblNot.Text = notno & " unread notification"
                notifications.Text = "<span class='badge' style='background-color:red;'>" & notno & "</span>"
            Else
                lblNot.Text = "no unread notofication"
            End If
            PlaceHolder3.Controls.Add(notifications)
            Dim messages As New Literal
            Dim msmsg As String = ""
            Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from notifications where notifications.recipient = '" & user & "' and type = '" & ty & "' order by time desc", con)
            Dim student2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad2.ExecuteReader
            Dim msgno As Integer = 0
            Dim msgno2 As Integer = 0
            Do While student2.Read
                Dim dob As Date = student2("time")
                Dim sage As TimeSpan = Now.Subtract(dob)
                Dim timelag As String = ""
                If sage.Days < 1 Then
                    If sage.Hours < 1 Then
                        If sage.Minutes < 1 Then
                            timelag = "Just now"
                        ElseIf sage.Minutes = 1 Then
                            timelag = 1 & " Min ago"
                        Else
                            timelag = sage.Minutes & " Mins ago"
                        End If
                    ElseIf sage.Hours = 1 Then
                        timelag = "1 hour ago"
                    Else
                        timelag = sage.Hours & " Hrs ago"
                    End If
                ElseIf sage.Days = 1 Then
                    timelag = "1 Day ago"
                Else
                    timelag = sage.Days & " Days ago"
                End If
                Dim pass As String = path + "/img/noimage.jpg"
                If passportsmsg(msgno2) <> "" Then pass = path + Replace(passportsmsg(msgno2), "~", "")
                msmsg = msmsg & "<li " & IIf(student2("status") = "Unread", "style='background-color:#ebebeb;'", "") & "><a href='" & path + Replace(student2("url"), "~", "") & "'><div class='message-img'>" & _
                                        "<img src='" & pass + "' alt=''></div><div class='message-content' id='" & notno & "msg" & "' style='width:280px;" & IIf(student2("status") = "Unread", "color:#1f2c79;", "") & "'>" & _
                                        "<span class='message-date'>" & timelag & "</span><h2 style='width:70%;'>" & student2("relationship") & "</h2>" & _
                                        "<p style='font-size:small;'>" & student2("message") & "</p></div></a></li>"
                If student2("status") = "Unread" Then
                    msgno = msgno + 1
                End If
                msgno2 = msgno2 + 1
            Loop
            student2.Close()
            Dim distinction As String = "message"
            Dim distinctions As String = "messages"
            If Session("usertype") = "STUDENT" Then
                distinction = "assignment"
                distinctions = "assignments"
            End If
            If msgno > 1 Then
                lblMsg.Text = msgno & " unread " & distinctions
                messages.Text = "<span class='badge' style='background-color:red;'>" & msgno & "</span>"
            ElseIf msgno = 1 Then
                lblMsg.Text = msgno & " unread " & distinction
                messages.Text = "<span class='badge' style='background-color:red;'>" & msgno & "</span>"
            Else
                lblMsg.Text = "no unread " & distinction
            End If
            PlaceHolder4.Controls.Add(messages)

            Dim msgdetail As New Literal

            msgdetail.Text = msmsg
            notdetail.Text = ""
            notdetail.Text = notmsg
            plcmsg.Controls.Add(msgdetail)
            con.close()
        End Using

    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs)
        Try



            If popup.Visible = True Then
                popup.Visible = False
            End If
            get_Updates()



        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub

    Protected Sub lnkCloseNot_Click(sender As Object, e As EventArgs) Handles lnkCloseNot.Click
        popup.Visible = False
    End Sub


    Protected Sub lnkDashBoard_Click(sender As Object, e As EventArgs) Handles lnkDashBoard.Click
        If Session("usertype") = "STAFF" Then
            Response.Redirect("~/content/dashboards/staffdashboard.aspx")
        ElseIf Session("usertype") = "STUDENT" Then
            Response.Redirect("~/content/dashboards/studentdashboard.aspx")
        ElseIf Session("usertype") = "PARENT" Then
            Response.Redirect("~/content/dashboards/parentdashboard.aspx")
        End If
    End Sub
End Class

