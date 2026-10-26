Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Partial Class MasterPage
    Inherits System.Web.UI.MasterPage

    Protected Sub Accordion1_ItemDataBound(sender As Object, _
                  e As AjaxControlToolkit.AccordionItemEventArgs)
        If e.ItemType = AjaxControlToolkit.AccordionItemType.Content Then
            Dim ds As New DataTable
            Dim x As New HyperLinkColumn
            x.Text = "productname"
            ds.Columns.Add(x.Text)
            Dim path As String = "http://" & Request.Url.Authority & "/Portal/"
            If DirectCast(e.AccordionItem.FindControl("txt_categoryID"), HiddenField).Value = "ad" Then
                ds.Rows.Add(String.Format("<a href=" + path + "admin/messages.aspx> MESSAGING</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/newsession.aspx> MANAGE SESSIONS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/newterm.aspx> MANAGE TERMS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/addsubject.aspx> MANAGE SUBJECTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/departments.aspx> MANAGE DEPARTMENTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/manageclass.aspx> MANAGE CLASSES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/staffprofile.aspx> MANAGE STAFF</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/transport.aspx> MANAGE TRANSPORT</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/boarding.aspx> MANAGE HOSTELS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/managegrades.aspx> MANAGE GRADES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/studentprofile.aspx> MANAGE STUDENTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/parentprofile.aspx> MANAGE PARENTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/publishca.aspx> PUBLISH ASSESSMENTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/managetimetable.aspx> MANAGE TIME TABLES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/traits.aspx> MANAGE TRAITS</a>"))

            ElseIf DirectCast(e.AccordionItem.FindControl("txt_categoryID"), HiddenField).Value = "ac" Then
                ds.Rows.Add(String.Format("<a href=" + path + "Account/accsettings.aspx> ACCOUNT SETTINGS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Account/mngfees.aspx> MANAGE FEES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Account/feeboard.aspx> STUDENT FEES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Account/discount.aspx> DISCOUNTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Account/transactions.aspx> MANAGE TRANSACTIONS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Account/salary.aspx> SALARY</a>"))

            ElseIf DirectCast(e.AccordionItem.FindControl("txt_categoryID"), HiddenField).Value = "sad" Then
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/adminoptions.aspx> PORTAL OPTIONS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "admin/messages.aspx> MESSAGING</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/newsession.aspx> MANAGE SESSIONS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/newterm.aspx> MANAGE TERMS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/addsubject.aspx> MANAGE SUBJECTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/departments.aspx> MANAGE DEPARTMENTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/manageclass.aspx> MANAGE CLASSES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/staffprofile.aspx> MANAGE STAFF</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/transport.aspx> MANAGE TRANSPORT</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/boarding.aspx> MANAGE HOSTELS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/managegrades.aspx> MANAGE GRADES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/publishca.aspx> PUBLISH ASSESSMENTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/managetimetable.aspx> MANAGE TIME TABLES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/traits.aspx> MANAGE TRAITS</a>"))

                ds.Rows.Add(String.Format("<a href=" + path + "Admin/studentprofile.aspx> MANAGE STUDENTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "Admin/parentprofile.aspx> MANAGE PARENTS</a>"))
            ElseIf DirectCast(e.AccordionItem.FindControl("txt_categoryID"), HiddenField).Value = "sta" Then
                ds.Rows.Add(String.Format("<a href=" + path + "staff/staffprofile.aspx> PROFILE</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/messages.aspx> MESSAGING</a>"))

            ElseIf DirectCast(e.AccordionItem.FindControl("txt_categoryID"), HiddenField).Value = "st" Then
                ds.Rows.Add(String.Format("<a href=" + path + "staff/courseoverview.aspx> COURSE OVERVIEW</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/courseoutline.aspx> COURSE OUTLINES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/timetable.aspx> TIME TABLE</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/myplans.aspx> LESSON PLAN</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/assignments.aspx> ASSIGNMENTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/scoresheet.aspx> SCORES</a>"))

            ElseIf DirectCast(e.AccordionItem.FindControl("txt_categoryID"), HiddenField).Value = "pa" Then
                ds.Rows.Add(String.Format("<a href=" + path + "parent/parentprofile.aspx> PROFILE</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "parent/messages.aspx> MESSAGING</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "parent/studentprofile.aspx> CHILDREN/WARDS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "parent/feeboard.aspx> FEES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "parent/classdetails.aspx> CLASSES/COURSES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "parent/assignments.aspx> ASSIGNMENTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "parent/childscores.aspx> SCORES</a>"))

            ElseIf DirectCast(e.AccordionItem.FindControl("txt_categoryID"), HiddenField).Value = "ct" Then
                ds.Rows.Add(String.Format("<a href=" + path + "staff/classdetails.aspx> CLASS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/studentprofile.aspx> STUDENTS PROFILE</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/parentprofile.aspx> PARENTS PROFILE</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/feeboard.aspx> STUDENT FEES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/studentsubjects.aspx> STUDENTS SUBJECTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/attendance.aspx> STUDENTS ATTENDANCE</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/childscores.aspx> STUDENTS SCORES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/studentbehaviour.aspx> STUDENTS BEHAVIOUR</a>"))

            ElseIf DirectCast(e.AccordionItem.FindControl("txt_categoryID"), HiddenField).Value = "sh" Then
                ds.Rows.Add(String.Format("<a href=" + path + "staff/checkattendance.aspx> ATENDANCE REGISTER</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/sclassdetails.aspx> CLASS/COURSES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/ssentmsgs.aspx> MESSAGES</a>"))

            ElseIf DirectCast(e.AccordionItem.FindControl("txt_categoryID"), HiddenField).Value = "dh" Then
                ds.Rows.Add(String.Format("<a href=" + path + "staff/submittedplans.aspx> CHECK LESSON PLANS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/dcourseoverview.aspx> CHECK COURSE OVERVIEW</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/dcourseoutline.aspx> CHECK COURSE OUTLINES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/dtimetable.aspx> CHECK TIME TABLES</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/dassignments.aspx> CHECK ASSIGNMENTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "staff/dscoresheet.aspx> CHECK SCORES</a>"))

            ElseIf DirectCast(e.AccordionItem.FindControl("txt_categoryID"), HiddenField).Value = "stu" Then
                ds.Rows.Add(String.Format("<a href=" + path + "student/studentprofile.aspx> PROFILE</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "student/assignments.aspx> ASSIGNMENTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "student/classdetails.aspx> CLASS/SUBJECTS</a>"))
                ds.Rows.Add(String.Format("<a href=" + path + "student/myscores.aspx> MY SCORES</a>"))
            End If



            Dim grd As New GridView()

            grd = DirectCast(e.AccordionItem.FindControl("GridView1"), GridView)
            grd.DataSource = ds
            grd.DataBind()
        End If
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("usertype") = Nothing Then
            Response.Redirect("~/default2.aspx")
        End If
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim ds As New DataTable
            ds.Columns.Add("categoryid")
            ds.Columns.Add("categoryName")
            If Session("usertype") = "student" Then
                ds.Rows.Add("stu", String.Format("</br> STUDENT"))
            ElseIf Session("usertype") = "staff" Then
                ds.Rows.Add("sta", String.Format("</br> STAFF"))
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("select * from admin where username='" & Session("staffid") & "'", con)
                Dim dr As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader
                If dr.Read = True Then
                    ds.Rows.Add("ad", String.Format("</br> ADMIN"))
                End If
                dr.Close()
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("select * from acclogin where username='" & Session("staffid") & "'", con)
                Dim dr2 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                If dr2.Read = True Then
                    ds.Rows.Add("ac", String.Format("</br> ACCOUNTANT"))
                End If
                dr2.Close()
                Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("select * from classteacher where teacher='" & Session("staffid") & "'", con)
                Dim dr3 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader
                If dr3.Read = True Then
                    ds.Rows.Add("ct", String.Format("</br> CLASS TEACHER"))
                End If
                dr3.Close()
                Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("select * from classsubjects where teacher='" & Session("staffid") & "'", con)
                Dim dr4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader
                If dr4.Read = True Then
                    ds.Rows.Add("st", String.Format("</br> SUBJECT TEACHER"))
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
                    ds.Rows.Add("dh", String.Format("</br> DEPARTMENT HEAD"))
                End If
                If classgroups.Count <> 0 Then
                    ds.Rows.Add("sh", String.Format("</br> SCHOOL HEAD"))
                End If
            ElseIf Session("usertype") = "parent" Then
                ds.Rows.Add("pa", String.Format("</br> PARENT"))

            ElseIf Session("usertype") = "super admin" Then
                ds.Rows.Add("sad", String.Format("</br>SUPER ADMIN"))
            End If






            Accordion1.DataSource = ds.DefaultView
            Accordion1.DataBind()
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
End Class

