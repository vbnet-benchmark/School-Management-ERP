Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls

Partial Class Staff_scoresheet
    Inherits System.Web.UI.Page

    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder
    Dim par As New parentcheck
    Private Sub Show_Alert(type As Boolean, msg As String)
        alertPLC = Me.Master.FindControl("plcAlert")
        If type = True Then
            alert.Text = alertmsg.success_message(msg)
        Else
            alert.Text = alertmsg.error_message(msg)
        End If
        alertPLC.Controls.Add(alert)
    End Sub
    

    Private Sub Bind_Data()
       
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Session("Selectterm") = Session("sessionid")
            If cboClass.Text = "Select Class" Then
                Show_Alert(False, "Please select a subject to continue")
               
        Exit Sub
            End If
        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Class WHERE Class = ?", con)
        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cboClass.Text))
        Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader()
        reader2.Read()
        Session("thisclass") = reader2.Item(0)
        reader2.Close()

        Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Subjects WHERE Subject = ?", con)
        cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", cboSubject.Text))
        Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader()
        reader3.Read()
        Session("thissubid") = reader3.Item(0)
        reader3.Close()
        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cano from class Where id = ?", con)
        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("thisclass")))
        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
        reader20.Read()
        Dim caquery As Integer = 2
        If reader20(0) = 3 Then
            caquery = 3
        ElseIf reader20(0) = 4 Then
            caquery = 4
        End If
        reader20.Close()

        If caquery = 2 Then
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT StudentsProfile.admno as 'Reg No', studentsprofile.surname as 'Name', subjectreg.ca1 as '1st CA', subjectreg.ca2 as '2nd CA', SubjectReg.examination as Exams, subjectreg.total as 'Total', subjectreg.grade as 'Grade', subjectreg.remarks as 'Remarks' FROM StudentsProfile INNER JOIN SubjectReg ON StudentsProfile.admno = SubjectReg.Student WHERE SubjectReg.SubjectsOfferred = ? And SubjectReg.Session = ? And SubjectReg.Class = ? ORDER BY StudentsProfile.surname", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.SubjectsOfferred", Session("thissubid")))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("Selectterm")))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("thisclass")))
            Dim ds As New DataTable
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1.SelectCommand = cmd
            adapter1.Fill(ds)
            GridView1.DataSource = ds
            GridView1.Columns(4).Visible = False
            GridView1.Columns(5).Visible = False
            GridView1.DataBind()

        ElseIf caquery = 3 Then
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT StudentsProfile.admno as 'Reg No', studentsprofile.surname as 'Name', subjectreg.ca1 as '1st CA', subjectreg.ca2 as '2nd CA', subjectreg.ca3 as '3rd CA', SubjectReg.examination as Exams, subjectreg.total as 'Total', subjectreg.grade as 'Grade', subjectreg.remarks as 'Remarks' FROM StudentsProfile INNER JOIN SubjectReg ON StudentsProfile.admno = SubjectReg.Student WHERE SubjectReg.SubjectsOfferred = ? And SubjectReg.Session = ? And SubjectReg.Class = ? ORDER BY StudentsProfile.surname", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.SubjectsOfferred", Session("thissubid")))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("Selectterm")))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("thisclass")))
            Dim ds As New DataTable
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1.SelectCommand = cmd
            adapter1.Fill(ds)
            GridView1.DataSource = ds
            GridView1.Columns(5).Visible = False
            GridView1.DataBind()

        ElseIf caquery = 4 Then
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT StudentsProfile.admno as 'Reg No', studentsprofile.surname as 'Name', subjectreg.ca1 as '1st CA', subjectreg.ca2 as '2nd CA', subjectreg.ca3 as '3rd CA', subjectreg.project as '4th CA', SubjectReg.examination as Exams, subjectreg.total as 'Total', subjectreg.grade as 'Grade', subjectreg.remarks as 'Remarks' FROM StudentsProfile INNER JOIN SubjectReg ON StudentsProfile.admno = SubjectReg.Student WHERE SubjectReg.SubjectsOfferred = ? And SubjectReg.Session = ? And SubjectReg.Class = ? ORDER BY StudentsProfile.surname", con)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.SubjectsOfferred", Session("thissubid")))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("Selectterm")))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("thisclass")))
            Dim ds As New DataTable
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1.SelectCommand = cmd
            adapter1.Fill(ds)
            GridView1.DataSource = ds
            GridView1.DataBind()
        End If
        reader20.Close()
        Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjectreg.* from SubjectReg WHERE SubjectReg.SubjectsOfferred = ? And SubjectReg.Session = ? And SubjectReg.Class = ?", con)
        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.SubjectsOfferred", Session("thissubid")))
        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Session", Session("Selectterm")))
        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Class", Session("thisclass")))

        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
        Dim no As Integer
        Do While reader.Read()
            lblSA.Text = reader.Item("avg").ToString
            lblLowest.Text = reader.Item("lowest").ToString
            lbhighest.Text = reader.Item("highest").ToString
            no = no + 1
        Loop

        reader.Close()
        lblNo.Text = no

        con.close()end using
        panel1.Visible = True
        GridView1.DataBind()

    End Sub


    Public Function Get_subordinates(ByVal dept As String) As ArrayList
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept from depts where superior = '" & dept & "'", con)
            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim subo As New ArrayList
            Do While student1.Read
                subo.Add(student1.Item(0))
            Loop
            student1.Close()
            Return subo
            con.Close()
        End Using
    End Function
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try


            If IsPostBack Then
            Else
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
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
                        DropDownList1.Items.Clear()
                        For Each sitem As String In deptstaff
                            Dim classes11 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname from classsubjects inner join staffprofile on classsubjects.teacher = staffprofile.staffid where classsubjects.teacher = '" & sitem & "'  and staffprofile.activated = '" & 1 & "'", con)
                            Dim schclass11 As MySql.Data.MySqlClient.MySqlDataReader = classes11.ExecuteReader

                            If schclass11.Read Then
                                DropDownList1.Items.Add(schclass11.Item(0).ToString)
                            End If
                            schclass11.Close()
                        Next
                    Next
                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.Class, subjects.Subject FROM classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher INNER JOIN class ON classsubjects.class = class.ID INNER JOIN subjects ON classsubjects.subject = subjects.ID Where staffprofile.surname = '" & DropDownList1.Text & "' Order By subjects.subject ", con)
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                    cboClass.Items.Clear()
                    cboSubject.Items.Clear()
                    cboSubject.Items.Add("Select Subject")
                    Dim subject As New ArrayList
                    Do While reader2.Read
                        If Not subject.Contains(reader2.Item(1).ToString) Then
                            subject.Add(reader2.Item(1).ToString)
                            cboSubject.Items.Add(reader2.Item(1).ToString)
                        End If
                    Loop
                    reader2.Close()

                    GridView1.DataSource = ""
                    panel1.Visible = False
                    GridView1.DataBind()
                    con.Close()                End Using
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub cboSubject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubject.SelectedIndexChanged
        Try
            If cboSubject.Text = "Select Subject" Then
                Show_Alert(False, "Please select a subject to continue")
                cboClass.Items.Clear()
                Exit Sub
            End If

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Subjects WHERE Subject = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", cboSubject.Text))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader()
                reader3.Read()
                Dim subID As Integer = reader3.Item(0)
                reader3.Close()

                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.Class, subjects.Subject FROM classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher INNER JOIN class ON classsubjects.class = class.ID INNER JOIN subjects ON classsubjects.subject = subjects.ID Where staffprofile.surname = '" & DropDownList1.Text & "' And classsubjects.subject = '" & subID & "' ", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                cboClass.Items.Clear()
                cboClass.Items.Add("Select Class")
                Do While reader2.Read
                    cboClass.Items.Add(reader2.Item(0).ToString)
                Loop
                reader2.Close()
                con.close()            End Using
            GridView1.DataSource = ""
            panel1.Visible = False
            GridView1.DataBind()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub cboSubject_TextChanged(sender As Object, e As EventArgs) Handles cboSubject.TextChanged

    End Sub




    Protected Sub cboClass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboClass.SelectedIndexChanged
        Try
            Bind_Data()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.Class, subjects.Subject FROM classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher INNER JOIN class ON classsubjects.class = class.ID INNER JOIN subjects ON classsubjects.subject = subjects.ID Where staffprofile.surname = '" & DropDownList1.Text & "' Order By subjects.subject ", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                cboClass.Items.Clear()
                cboSubject.Items.Clear()
                cboSubject.Items.Add("Select Subject")
                Dim subject As New ArrayList
                Do While reader2.Read
                    If Not subject.Contains(reader2.Item(1).ToString) Then
                        subject.Add(reader2.Item(1).ToString)
                        cboSubject.Items.Add(reader2.Item(1).ToString)
                    End If
                Loop
                reader2.Close()
              
            GridView1.DataSource = ""
            panel1.Visible = False
            GridView1.DataBind()
            con.close()end using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
