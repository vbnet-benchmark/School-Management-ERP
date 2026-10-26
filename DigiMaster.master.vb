
Partial Class Content_DigiMaster
    Inherits System.Web.UI.MasterPage
    Dim terms As New Literal



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim path As String = "https://" & Request.Url.Authority & "/"
            If Session("usertype") = "student" Then
                Dim stumenuitems As New Literal
                Dim stumobmenuitems As New Literal
                Dim studentmenu As String = ""
                studentmenu = studentmenu + "<li class='nav-item'><a href='" + path + "Content/Student/studentprofile.aspx" + "' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>PROFILE</span> </a></li>"
                studentmenu = studentmenu + "<li class='nav-item'><a href='" + path + "Content/Student/assignments.aspx" + "' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>ASSIGNMENTS</span> </a></li>"
                studentmenu = studentmenu + "<li class='nav-item'><a href='" + path + "Content/Student/classdetails.aspx" + "' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>CLASS DETAILS</span> </a></li>"
                studentmenu = studentmenu + "<li class='nav-item'><a href='" + path + "Content/Student/myscores.aspx" + "' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>MY SCORES</span> </a></li>"
                studentmenu = studentmenu + "<li class='nav-item'><a href='" + path + "Content/Student/studentprofile.aspx" + "' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>E-LEARNING</span> </a></li>"
                stumenuitems.Text = studentmenu
                Dim stumobmenu As String = "<li><a href='" + path + "Content/Student/studentprofile.aspx" + "'>PROFILE</a></li>" & _
                                               "<li><a href='" + path + "Content/Student/assignments.aspx" + "'>ASSIGNMENTS</a></li>" & _
                                               "<li><a href='" + path + "Content/Student/classdetails.aspx" + "'>CLASS DETAILS</a></li>" & _
                                               "<li><a href='" + path + "Content/Student/studentprofile.aspx" + "'>MY SCORES</a></li>" & _
                                               "<li><a href='" + path + "Content/Student/studentprofile.aspx" + "'>E-LEARNING</a></li>"


                PlaceHolder2.Controls.Add(stumenuitems)
                stumobmenuitems.Text = stumobmenu
                plcMob.Controls.Add(stumobmenuitems)
            ElseIf Session("usertype") = "staff" Then
                Dim staffmenu As String
                Dim stamenuitems As New Literal
                Dim stamobmenuitems As New Literal
                staffmenu = "<li class='nav-item'><a href='#' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>STAFF</span><span class='indicator-right-menu mini-dn'><i class='fa indicator-mn fa-angle-left'></i></span></a>" & _
                                "<div role='menu' class='dropdown-menu left-menu-dropdown animated flipInX'>" & _
                                    "<a href='" + path + "Content/Staff/profile.aspx" + "' class='dropdown-item'>Profile</a>" & _
                                    "<a href='" + path + "Content/Staff/messages.aspx" + "' class='dropdown-item'>View Mail</a></div></li>"
                Dim staffmob As String
                staffmob = "<li><a data-toggle='collapse' data-target='#demo' href='#'>STAFF <span class='admin-project-icon adminpro-icon adminpro-down-arrow'></span></a>" & _
                                                "<ul id='demo' class='collapse dropdown-header-top'>" & _
                                                    "<li><a href='" + path + "Content/Staff/profile.aspx" + "'>Profile</a></li>" & _
                                                    "<li><a href='" + path + "Content/Staff/messages.aspx" + "'>Messaging</a></li></ul></li>"
                stamenuitems.Text = staffmenu
                PlaceHolder2.Controls.Add(stamenuitems)
                stamobmenuitems.Text = staffmob
                plcMob.Controls.Add(stamobmenuitems)
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("select * from admin where username='" & Session("staffid") & "'", con)
                Dim dr As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
                If dr.Read = True Then
                    Dim admmenuitems As New Literal
                    Dim admmobmenuitems As New Literal
                    Dim adminmenu As String = "<li class='nav-item'><a href='#' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>ADMIN</span><span class='indicator-right-menu mini-dn'><i class='fa indicator-mn fa-angle-left'></i></span></a>" & _
                                "<div role='menu' class='dropdown-menu left-menu-dropdown animated flipInX' style='padding:4px;'>" & _
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
                                    "<a href='" + path + "Content/admin/traits.aspx" + "' class='dropdown-item'>Manage Traits</a></div></li>"

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
                                                        "<li><a href='" + path + "Content/admin/traits.aspx" + "'>Manage Traits</a></li>" & _
                                                        "<li><a href='" + path + "Content/admin/messages.aspx" + "'>Messaging</a></li></ul></li>"

                    admmenuitems.Text = adminmenu
                    PlaceHolder2.Controls.Add(admmenuitems)
                    admmobmenuitems.Text = adminmob
                    plcMob.Controls.Add(admmobmenuitems)
                End If
                dr.Close()
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("select * from acclogin where username='" & Session("staffid") & "'", con)
                Dim dr2 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                If dr2.Read = True Then
                    Dim accmenuitems As New Literal
                    Dim accmobmenuitems As New Literal
                    Dim accmenu As String = "<li class='nav-item'><a href='#' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>ACCOUNTS</span><span class='indicator-right-menu mini-dn'><i class='fa indicator-mn fa-angle-left'></i></span></a>" & _
                                "<div role='menu' class='dropdown-menu left-menu-dropdown animated flipInX'>" & _
                                    "<a href='" + path + "Content/account/messages.aspx" + "' class='dropdown-item'>Messaging</a>" & _
                                    "<a href='" + path + "Content/account/accsettings.aspx" + "' class='dropdown-item'>Account Settings</a>" & _
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
                                                        "<li><a href='" + path + "Content/staff/parentprofiles.aspx" + "'>Parents Profile</a></li>" & _
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
                    Dim stmenuitems As New Literal
                    Dim stmobmenuitems As New Literal
                    Dim stmenu As String = "<li class='nav-item'><a href='#' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>SUBJECT TEACHER</span><span class='indicator-right-menu mini-dn'><i class='fa indicator-mn fa-angle-left'></i></span></a>" & _
                                            "<div role='menu' class='dropdown-menu left-menu-dropdown animated flipInX'>" & _
                                                "<a href='" + path + "Content/staff/courseoverview.aspx" + "' class='dropdown-item'>Course Overview</a>" & _
                                                "<a href='" + path + "Content/staff/courseoutline.aspx" + "' class='dropdown-item'>Course Outlines</a>" & _
                                                 "<a href='" + path + "Content/staff/timetable.aspx" + "' class='dropdown-item'>Time Table</a>" & _
                                                  "<a href='" + path + "Content/staff/myplans.aspx" + "' class='dropdown-item'>Lesson Plan</a>" & _
                                                   "<a href='" + path + "Content/staff/assignments.aspx" + "' class='dropdown-item'>Students Assignments</a>" & _
                                                    "<a href='" + path + "Content/staff/scoresheet.aspx" + "' class='dropdown-item'>Students Scores</a>" & _
                                                   "<a href='" + path + "Content/staff/elearning.aspx" + "' class='dropdown-item'>E-Learning</a></div></li>"
                    Dim stmob As String
                    stmob = "<li><a data-toggle='collapse' data-target='#demo' href='#'>SUBJECT TEACHER<span class='admin-project-icon adminpro-icon adminpro-down-arrow'></span></a>" & _
                                                    "<ul id='demo' class='collapse dropdown-header-top'>" & _
                                                        "<li><a href='" + path + "Content/staff/courseoverview.aspx" + "'>Course Overview</a></li>" & _
                                                        "<li><a href='" + path + "Content/staff/courseoutline.aspx" + "'>Course Outline</a></li>" & _
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
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept, headtitle from depts where head = '" & Session("staffid") & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Dim deptshead As New ArrayList
                Do While student0.Read
                    deptshead.Add(student0.Item(0))
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
                    Dim f As Boolean = False
                    Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                    Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                    Do While schclass.Read
                        f = True
                    Loop
                    If f = True Then
                        classgroups.Add(item)
                    End If
                    schclass.Close()
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
                If teachingstaff.Count <> 0 Then
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
                If classgroups.Count <> 0 Then
                    Dim shmenuitems As New Literal
                    Dim shmobmenuitems As New Literal
                    Dim shmenu As String = "<li class='nav-item'><a href='#' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>SCHOOL HEAD</span><span class='indicator-right-menu mini-dn'><i class='fa indicator-mn fa-angle-left'></i></span></a>" & _
                            "<div role='menu' class='dropdown-menu left-menu-dropdown animated flipInX'>" & _
                                "<a href='" + path + "Content/staff/checkattendance.aspx" + "' class='dropdown-item'>Check Attendance</a>" & _
                                "<a href='" + path + "Content/staff/sclassdetails.aspx" + "' class='dropdown-item'>Class Details</a>" & _
                                 "<a href='" + path + "Content/staff/ssentmsgs.aspx" + "' class='dropdown-item'>Check Messages</a>/li></ul></li>"

                    Dim shmob As String
                    shmob = "<li><a data-toggle='collapse' data-target='#demo' href='#'>SCHOOL HEAD<span class='admin-project-icon adminpro-icon adminpro-down-arrow'></span></a>" & _
                                    "<ul id='demo' class='collapse dropdown-header-top'>" & _
                                        "<li><a href='" + path + "Content/staff/checkattendance.aspx" + "'>Check Attendance</a></li>" & _
                                        "<li><a href='" + path + "Content/staff/sclassdetails.aspx" + "'>Class Details</a></li>" & _
                                        "<li><a href='" + path + "Content/staff/ssentmsgs.aspx" + "'>Check Messages</a></li></ul></li>"
                    shmobmenuitems.Text = shmob
                    plcMob.Controls.Add(shmobmenuitems)

                    shmenuitems.Text = shmenu
                    PlaceHolder2.Controls.Add(shmobmenuitems)
                End If
            ElseIf Session("usertype") = "parent" Then
                Dim pmenuitems As New Literal
                Dim pmobmenuitems As New Literal
                Dim parentmenu As String = ""
                parentmenu = parentmenu + "<li class='nav-item'><a href='" + path + "Content/parent/parentprofile.aspx" + "' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>PROFILE</span> </a></li>"
                parentmenu = parentmenu + "<li class='nav-item'><a href='" + path + "Content/parent/messages.aspx" + "' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>MESSAGES</span> </a></li>"
                parentmenu = parentmenu + "<li class='nav-item'><a href='" + path + "Content/parent/studentprofile.aspx" + "' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>CHILDREN/WARDS</span> </a></li>"
                parentmenu = parentmenu + "<li class='nav-item'><a href='" + path + "Content/parent/feeboard.aspx" + "' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>FEES</span> </a></li>"
                parentmenu = parentmenu + "<li class='nav-item'><a href='" + path + "Content/parent/classdetails.aspx" + "' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>CLASSES/COURSES</span> </a></li>"
                parentmenu = parentmenu + "<li class='nav-item'><a href='" + path + "Content/parent/assignments.aspx" + "' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>ASSIGNMENTS</span> </a></li>"
                parentmenu = parentmenu + "<li class='nav-item'><a href='" + path + "Content/parent/childscores.aspx" + "' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>SCORES</span> </a></li>"

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

            ElseIf Session("usertype") = "super admin" Then
                Dim sadmmenuitems As New Literal
                Dim sadmmobmenuitems As New Literal
                Dim sadminmenu As String = "<li class='nav-item'><a href='#' data-toggle='dropdown' role='button' aria-expanded='false' class='nav-link dropdown-toggle'><span class='mini-dn'>ADMIN</span><span class='indicator-right-menu mini-dn'><i class='fa indicator-mn fa-angle-left'></i></span></a>" & _
                            "<div role='menu' class='dropdown-menu left-menu-dropdown animated flipInX' id='exmenu' style='padding:4px;'>" & _
                                "<a href='" + path + "Content/admin/adminoptions.aspx" + "' class='dropdown-item'>Portal Settings</a>" & _
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
                                "<a href='" + path + "Content/admin/traits.aspx" + "' class='dropdown-item'>Manage Traits</a></div></li>"
                sadmmenuitems.Text = sadminmenu
                PlaceHolder2.Controls.Add(sadmmenuitems)

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
                                                    "<li><a href='" + path + "Content/admin/traits.aspx" + "'>Manage Traits</a></li>" & _
                                                    "<li><a href='" + path + "Content/admin/messages.aspx" + "'>Messaging</a></li></ul></li>"


                sadmmobmenuitems.Text = adminmob
                plcMob.Controls.Add(sadmmobmenuitems)
            End If

            imgProf.Src = path + "img/message/beststudent.jpg"
            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session", con)
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
            Dim termtext As String = ""
            Dim url As String = Request.RawUrl
            Do While reader2.Read()
                If reader2(0).ToString = Session("sessionid") Then
                    lblSession.Text = reader2(1) & " - " + reader2(2)
                End If
                termtext = termtext & "<li><a href='" + url + "?term=" + reader2(0).ToString + "'><span class='adminpro-icon adminpro-money author-log-ic'></span>" + reader2(1) & " - " + reader2(2) + "</a></li>"
            Loop
            terms.Text = termtext
            PlaceHolder1.Controls.Add(terms)
            Dim notno As Integer = 1
            Dim notifications As New Literal
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
            Dim msgno As Integer = 1
            Dim messages As New Literal
            If msgno > 1 Then
                lblMsg.Text = msgno & " unread messages"
                messages.Text = "<span class='badge' style='background-color:red;'>" & msgno & "</span>"
            ElseIf msgno = 1 Then
                lblMsg.Text = msgno & " unread message"
                messages.Text = "<span class='badge' style='background-color:red;'>" & msgno & "</span>"
            Else
                lblMsg.Text = "no unread message"
            End If
            PlaceHolder4.Controls.Add(messages)

            Dim msgdetail As New Literal
            Dim notdetail As New Literal
            Dim eachmsg As String = "<li><a href='#'><div class='message-img'>" & _
                                    "<img src='" & path + "img/message/beststudent.jpg" & "' alt=''></div><div class='message-content'>" & _
                                    "<span class='message-date'>16 Sept</span><h2>Admin</h2>" & _
                                    "<p>This is an unread message from admin.</p></div></a></li>"
            msgdetail.Text = eachmsg
            notdetail.Text = eachmsg
            plcmsg.Controls.Add(msgdetail)
            plcNot.Controls.Add(notdetail)
            If Request.QueryString("term") <> Nothing Then
                Session("Sessionid") = Request.QueryString("term")
                Response.Redirect(Request.Path)
            End If
            con.close()
        End Using

    End Sub
    Public Function Get_subordinates(ByVal dept As String) As ArrayList
        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept from depts where superior = '" & dept & "'", con)
        Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
        Dim subo As New ArrayList
        Do While student1.Read
            subo.Add(student1.Item(0))
        Loop
        student1.Close()
        Return subo
    End Function
    Protected Sub lnkLogOut_Click(sender As Object, e As EventArgs) Handles lnkLogOut.Click
        Session.Abandon()
        Response.Redirect("~/default.aspx")
    End Sub
End Class

