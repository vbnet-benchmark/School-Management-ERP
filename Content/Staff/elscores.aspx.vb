Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_studentprofile
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList
    Dim loadedsubs As Boolean = False
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






    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
       
        If check.Check_Class(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try
            If IsPostBack Then
                Dim ct As Integer = 1
                If Session("grades") <> Nothing Then
                    Dim thisarray As New ArrayList
                    thisarray = CType(Session("subs"), ArrayList)

                    For s = 1 To Session("grades") - 1

                        Options_Recreate(s, thisarray(s - 1))

                    Next
                End If
            Else
                Session("grades") = Nothing
                If Session("term") = "1st term" Then
                    cboTerm.Items.Add("1st Term")
                ElseIf Session("term") = "2nd term" Then
                    cboTerm.Items.Add("1st Term")
                    cboTerm.Items.Add("2nd Term")
                    cboTerm.Text = "2nd Term"
                Else
                    cboTerm.Items.Add("1st Term")
                    cboTerm.Items.Add("2nd Term")
                    cboTerm.Items.Add("3rd Term")
                    cboTerm.Text = "3rd Term"
                End If
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from classteacher inner join class on class.id = classteacher.class where classteacher .teacher = ?", con)
                    cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("staffid")))
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    Do While student.Read
                        DropDownList1.Items.Add(student.Item(0).ToString)
                    Loop
                    panel3.Visible = False
                    student.Close()
                    Dim ds As New DataTable
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname, class.type from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.id WHERE class.class = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                    Do While student0.Read

                        ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                    Loop
                    student0.Close()
                    gridview1.DataSource = ds

                    gridview1.DataBind()


                    Dim ds2 As New DataTable
                    ds2.Columns.Add("aspect")
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic from eltopics inner join class on class.id = eltopics.class where class.class = '" & DropDownList1.Text & "'", con)
                    Dim studentd As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

                    Do While studentd.Read()
                        ds2.Rows.Add(studentd(0))
                    Loop
                    student.Close()


                    con.Close()                End Using
                If Session("studentadd") <> Nothing Then
                    Student_Details()
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub

    Protected Sub Student_Details()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from StudentsProfile where admno = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
            Dim pass As String
            student.Read()
            pass = student.Item("passport").ToString
            student.Close()
            If pass <> "" Then Image1.ImageUrl = pass

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic from eltopics inner join class on class.id = eltopics.class where  class.class = '" & DropDownList1.Text & "'", con)
            Dim students As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            cboAspect.Items.Clear()
            cboAspect.Items.Add("Select Aspect")
            Do While students.Read()
                cboAspect.Items.Add(students(0))
            Loop
            cboAspect.Visible = True
            students.Close()

            pnlAll.Visible = False
            panel3.Visible = True
            con.Close()
        End Using
    End Sub




    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Class WHERE Class = ?", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", DropDownList1.Text))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader()
                reader2.Read()
                Dim cla As Integer = reader2.Item(0)
                reader2.Close()
                Dim getlabel As Boolean = True
                Dim count As Integer = 1
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT elsubtopics.id from elsubtopics inner join eltopics on elsubtopics.topic = eltopics.id where eltopics.topic = '" & cboAspect.Text & "' and eltopics.class = '" & cla & "' order by elsubtopics.id", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                Dim ids As New ArrayList
                Do While student0.Read()
                    ids.Add(student0(0))
                Loop
                student0.Close()
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Sessioncreate where sessionname = '" & Session("SessionName") & "' Order by ID Desc", con)
                Dim reader2b As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                reader2b.Read()
                Dim tissession = reader2b(0)

                reader2b.Close()
                For s = 1 To Session("grades") - 1
                    Dim o As RadioButtonList = plcC.FindControl("option" & count)
                    Dim atleast As Boolean = False
                    For Each item As ListItem In o.Items
                        If item.Selected = True Then atleast = True
                    Next
                    If atleast = True Then
                        Dim cmd1v As New MySql.Data.MySqlClient.MySqlCommand("update elscores set " & IIf(cboTerm.Text = "1st Term", IIf(o.SelectedItem.Text = "Beginning", " grade = 1", IIf(o.SelectedItem.Text = "Developing", " grade = 2", IIf(o.SelectedItem.Text = "Proficient", " grade = 3", " grade = 'N/A'"))), IIf(cboTerm.Text = "2nd Term", IIf(o.SelectedItem.Text = "Beginning", " grade2 = 1", IIf(o.SelectedItem.Text = "Developing", " grade2 = 2", IIf(o.SelectedItem.Text = "Proficient", " grade2 = 3", " grade2 = 'N/A'"))), IIf(o.SelectedItem.Text = "Beginning", " grade3 = 1", IIf(o.SelectedItem.Text = "Developing", " grade3 = 2", IIf(o.SelectedItem.Text = "Proficient", " grade3 = 3", " grade3 = 'N/A'"))))) & " WHERE session = '" & tissession & "' and student = '" & Session("studentadd") & "' and subtopic = '" & ids(s - 1) & "'", con)
                        cmd1v.ExecuteNonQuery()
                    End If
                    count = count + 1
                Next
                Dim cmd1vs As New MySql.Data.MySqlClient.MySqlCommand("update elscores set " & IIf(cboTerm.Text = "1st Term", "recommendation = '" & txtRecommend.Text & "', comments = '" & txtComments.Text, IIf(cboTerm.Text = "2nd Term", "recommendation2 = '" & txtRecommend.Text & "', comments2 = '" & txtComments.Text, "recommendation3 = '" & txtRecommend.Text & "', comments3 = '" & txtComments.Text)) & "' WHERE session = '" & tissession & "' and student = '" & Session("studentadd") & "'", con)
                cmd1vs.ExecuteNonQuery()

                logify.log(Session("staffid"), "The progress report of " & par.getstuname(Session("studentadd")) & " was updated")
                Show_Alert(True, "Progress Updated")
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub

    Sub Options_Recreate(count As Integer, txt As String)
        Try
            Dim l As New Label
            l.ID = "labels" & count & count
            l.Text = txt
            Dim sx As New LiteralControl
            sx.Text = "<div class='row'  id='cf" & count & "' style='padding-left:8.33%;'><div class = 'col-lg-1'></div><br/><div class='col-lg-11'><label id='" & count & "er" & "' style='text-align:left; font-size:16px;' class='login2'>"
            Dim sxend As New LiteralControl
            sxend.Text = "</label></div></div>"

            Dim sxq As New LiteralControl
            sxq.Text = "<div class='row'  id='cf" & count & "' style='padding-left:8.33%;'><div class = 'col-lg-1'></div><br/><div class='col-lg-11'><label id='" & count & "er" & "' style='text-align:left; font-size:14px;' class='login2'>"
            Dim sxqend As New LiteralControl
            sxqend.Text = "</label></div></div>"

            Dim options As New RadioButtonList
            options.ID = "option" & count

            options.Items.Add("Beginning")
            options.Items.Add("Developing")
            options.Items.Add("Proficient")


            plcC.Controls.Add(sx)
            plcC.Controls.Add(l)
            plcC.Controls.Add(sxend)

            plcC.Controls.Add(sxq)
            plcC.Controls.Add(options)
            plcC.Controls.Add(sxqend)


        Catch ex As Exception
            Show_Alert(False, logify.error_log("Option", Replace(ex.ToString, "'", "")))

        End Try

    End Sub


    Sub Options_create(count As Integer, grade As Integer, txt As String)
        Try
            Dim l As New Label
            l.ID = "labels" & count & count
            l.Text = count & ". " & txt
            Dim sx As New LiteralControl
            sx.Text = "<div class='row' id='cf" & count & "' style='padding-left:8.33%;'><div class = 'col-lg-1'></div><br/><div class='col-lg-11'><label id='" & count & "er" & "' style='text-align:left; font-size:16px;' class='login2'>"
            Dim sxend As New LiteralControl
            sxend.Text = "</label></div></div>"


            Dim sxq As New LiteralControl
            sxq.Text = "<div class='row'  id='cf" & count & "' style='padding-left:8.33%;'><div class = 'col-lg-1'></div><br/><div class='col-lg-11'><label id='" & count & "er" & "' style='text-align:left; font-size:14px;' class='login2'>"
            Dim sxqend As New LiteralControl
            sxqend.Text = "</label></div></div>"

            Dim options As New RadioButtonList
            options.ID = "option" & count
            options.Items.Add("Beginning")
            options.Items.Add("Developing")
            options.Items.Add("Proficient")


            If grade = 1 Then
                options.SelectedIndex = 0
            ElseIf grade = 2 Then
                options.SelectedIndex = 1
            ElseIf grade = 3 Then
                options.SelectedIndex = 2
            End If
            plcC.Controls.Add(sx)
            plcC.Controls.Add(l)
            plcC.Controls.Add(sxend)

            plcC.Controls.Add(sxq)
            plcC.Controls.Add(options)
            plcC.Controls.Add(sxqend)

        Catch ex As Exception
            Show_Alert(False, logify.error_log("Option", Replace(ex.ToString, "'", "")))

        End Try

    End Sub


    Protected Sub gridview1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gridview1.PageIndexChanging
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join (class inner join classteacher on classteacher.class = class.id) on studentsummary.class = class.id WHERE class.class = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.close()            End Using
            gridview1.PageIndex = e.NewPageIndex
            gridview1.DataBind()
            Session("studentadd") = Nothing
            panel3.Visible = False
            gridview1.SelectedIndex = -1
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub




    Protected Sub gridview1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gridview1.SelectedIndexChanged

    End Sub

    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview1.SelectedIndexChanging
        Try


            Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
            Session("studentadd") = RTrim(x(0))
           

        
            Student_Details()
            Dim cts(plcC.Controls.Count) As Control            Dim j As Integer = 0            For Each s In plcC.Controls
                cts(j) = s
                j = j + 1
            Next            For Each k In cts
                plcC.Controls.Remove(k)
            Next            Session("grades") = Nothing            txtComments.Text = ""
        gridview1.SelectedIndex = -1
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub



    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged

        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.id WHERE class.class = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()


                con.close()            End Using
            Session("studentadd") = Nothing
            gridview1.SelectedIndex = -1
            panel3.Visible = False
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub









    Protected Sub cboAspect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAspect.SelectedIndexChanged
        Try            Dim cts(plcC.Controls.Count) As Control            Dim j As Integer = 0            For Each s In plcC.Controls
                cts(j) = s
                j = j + 1
            Next            For Each k In cts
                plcC.Controls.Remove(k)
            Next            Session("grades") = Nothing            txtComments.Text = ""            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Sessioncreate where sessionname = '" & Session("SessionName") & "' Order by ID Desc", con)
                Dim reader2b As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                reader2b.Read()
                Dim tissession = reader2b(0)
                reader2b.Close()

                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT elsubtopics.subtopic, " & IIf(cboTerm.Text = "1st Term", "elscores.grade", IIf(cboTerm.Text = "2nd Term", "elscores.grade2", "elscores.grade3")) & " from elscores inner join elsubtopics on elscores.subtopic = elsubtopics.id inner join eltopics  on elscores.topic = eltopics.id where eltopics.topic = '" & cboAspect.Text & "' and elscores.student = '" & Session("studentadd") & "' and elscores.session = '" & tissession & "' order by elsubtopics.id", con)
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim count As Integer = 1
                Dim subs As New ArrayList
                Do While student.Read()
                    subs.Add(count & ". " & student(0))
                    Options_create(count, Val(student(1).ToString), student(0).ToString)
                    count = count + 1
                Loop
                student.Close()
                Session("grades") = count
                Session("subs") = subs

                Dim cmdLoad1s As New MySql.Data.MySqlClient.MySqlCommand("SELECT " & IIf(cboTerm.Text = "1st Term", "elscores.comments, elscores.recommendation", IIf(cboTerm.Text = "2nd Term", "elscores.comments2, elscores.recommendation2", "elscores.comments3, elscores.recommendation3")) & " from elscores where elscores.student = '" & Session("studentadd") & "' and elscores.session = '" & tissession & "'", con)
                Dim students As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1s.ExecuteReader

                Do While students.Read()
                    txtComments.Text = students(0).ToString
                    txtRecommend.Text = students(1).ToString
                Loop
                students.Close()

                pnlcomments.Visible = True
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub btnTopUpdate_Click(sender As Object, e As EventArgs) Handles btnTopUpdate.Click
        Try            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If txtAspect.Text = "" Then
                    Show_Alert(False, "Enter an aspect")
                    Exit Sub
                End If

                Dim cmdLoad1x As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = '" & DropDownList1.Text & "'", con)

                Dim studentx As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1x.ExecuteReader

                studentx.Read()
                Dim cla As Integer = studentx(0)
                studentx.Close()
                If Session("edittopic") <> Nothing Then


                    Dim cmdLoad1xs As New MySql.Data.MySqlClient.MySqlCommand("update eltopics set topic = '" & txtAspect.Text & "' where class = '" & cla & "' and topic = '" & Session("edittopic") & "'", con)
                    cmdLoad1xs.ExecuteNonQuery()
                    Session("edittopic") = Nothing
                Else

                    Dim cmdLoad1xs As New MySql.Data.MySqlClient.MySqlCommand("Insert into eltopics (topic, class) values ('" & txtAspect.Text & "', '" & cla & "')", con)
                    cmdLoad1xs.ExecuteNonQuery()

                End If
                Dim ds2 As New DataTable
                ds2.Columns.Add("aspect")

                Dim cmdLoad1f As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic from eltopics inner join class on class.id = eltopics.class where class.class = '" & DropDownList1.Text & "'", con)
                Dim studentf As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1f.ExecuteReader

                Do While studentf.Read()
                    ds2.Rows.Add(studentf(0))
                Loop
                studentf.Close()
                GridView2.DataSource = ds2
                GridView2.DataBind()

                Session("edittopic") = Nothing

                txtAspect.Text = ""
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnsubAspects_Click(sender As Object, e As EventArgs) Handles btnSubAspects.Click
        Try            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Session("editsubtopic") = Nothing
                Dim cmdLoad1z As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic from eltopics inner join class on eltopics.class = class.id where class.class = '" & DropDownList1.Text & "'", con)
                Dim studentz As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1z.ExecuteReader
                cboSubAspect.Items.Clear()
                cboSubAspect.Items.Add("Select Aspect")
                Do While studentz.Read()
                    cboSubAspect.Items.Add(studentz(0))
                Loop
                studentz.Close()

                panel5.Visible = True
                panel4.Visible = False
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub cbosubAspect_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubAspect.SelectedIndexChanged
        Try            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()

                Dim ds2 As New DataTable
                ds2.Columns.Add("aspect")
                ds2.Columns.Add("subaspect")
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic, elsubtopics.subtopic from elsubtopics inner join eltopics on eltopics.id = elsubtopics.topic inner join class on elsubtopics.class = class.id where class.class = '" & DropDownList1.Text & "' and eltopics.topic = '" & cboSubAspect.Text & "'", con)
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

                Do While student.Read()
                    ds2.Rows.Add(student(0), student(1))
                Loop
                student.Close()
                GridView3.DataSource = ""
                GridView3.DataSource = ds2
                GridView3.DataBind()

                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnsubsubAspect_Click(sender As Object, e As EventArgs) Handles btnSubSubAspect.Click
        Try            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If cboSubAspect.Text = "Select Aspect" Then
                    Show_Alert(False, "Please select a topic")
                    Exit Sub
                End If
                If txtSubSubAspect.Text = "" Then
                    Show_Alert(False, "Enter an aspect")
                    Exit Sub
                End If
                Dim cmdLoad1x As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = '" & DropDownList1.Text & "'", con)

                Dim studentx As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1x.ExecuteReader

                studentx.Read()
                Dim cla As Integer = studentx(0)
                studentx.Close()


                Dim cmdLoad1x2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from eltopics where topic = '" & cboSubAspect.Text & "' and class = '" & cla & "'", con)

                Dim studentx2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1x2.ExecuteReader

                studentx2.Read()
                Dim topicid As Integer = studentx2(0)
                studentx2.Close()

                If Session("editsubtopic") <> Nothing Then
                    Dim cmdLoad1xs As New MySql.Data.MySqlClient.MySqlCommand("update elsubtopics set subtopic = '" & txtSubSubAspect.Text & "' where topic = '" & topicid & "' and subtopic = '" & Session("editsubtopic") & "'", con)
                    cmdLoad1xs.ExecuteNonQuery()
                    Session("editsubtopic") = Nothing

                Else
                    Dim cmdLoad1xs As New MySql.Data.MySqlClient.MySqlCommand("Insert into elsubtopics (topic, class, subtopic) values ('" & topicid & "', '" & cla & "', '" & txtSubSubAspect.Text & "')", con)
                    cmdLoad1xs.ExecuteNonQuery()

                    Dim cmdLoad1xa As New MySql.Data.MySqlClient.MySqlCommand("SELECT student from studentsummary where class = '" & cla & "' and session = '" & Session("sessionid") & "'", con)

                    Dim studentxa As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1xa.ExecuteReader
                    Dim students As New ArrayList
                    Do While studentxa.Read()
                        students.Add(studentxa(0))
                    Loop

                    studentxa.Close()

                    Dim cmdLoad1x12 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from elsubtopics where subtopic = '" & txtSubSubAspect.Text & "' and class = '" & cla & "' and topic = '" & topicid & "'", con)

                    Dim studentx12 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1x12.ExecuteReader

                    studentx12.Read()
                    Dim subtopicid As Integer = studentx12(0)
                    studentx12.Close()

                    Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Sessioncreate where sessionname = '" & Session("SessionName") & "' Order by ID Desc", con)
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                    reader2.Read()
                    Dim tissession = reader2(0)
                    reader2.Close()

                    For Each s As String In students
                        Dim cmdLoadf As New MySql.Data.MySqlClient.MySqlCommand("Insert into elscores (topic, class, subtopic, session, student) values ('" & topicid & "', '" & cla & "', '" & subtopicid & "', '" & tissession & "', '" & s & "')", con)
                        cmdLoadf.ExecuteNonQuery()
                    Next
                End If
                Dim ds2 As New DataTable
                ds2.Columns.Add("aspect")
                ds2.Columns.Add("subaspect")

                Dim cmdLoad1g As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic, elsubtopics.subtopic from elsubtopics inner join eltopics on eltopics.id = elsubtopics.topic inner join class on elsubtopics.class = class.id where class.class = '" & DropDownList1.Text & "' and eltopics.topic = '" & cboSubAspect.Text & "'", con)
                Dim studentg As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1g.ExecuteReader

                Do While studentg.Read()
                    ds2.Rows.Add(studentg(0), studentg(1))
                Loop
                studentg.Close()
                GridView3.DataSource = ds2
                GridView3.DataBind()


                Session("editsubtopic") = Nothing


                con.Close()
                txtSubSubAspect.Text = ""
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Private Sub Fill_Students()
        Dim db As New DB_Interface
        Dim tissession As Integer = db.Select_single("select id from sessioncreate where sessionname = '" & Session("sessionname") & "'")
        Dim classes As ArrayList = db.Select_1D("select id from class where type = 'Early Years'")
        For Each cl In classes
            Dim students As ArrayList = db.Select_1D("select student from studentsummary where session = '" & Session("sessionid") & "' and class = '" & cl & "'")
            For Each st In students
                Dim a As Array = db.Select_Query("select id, topic from elsubtopics where class = '" & cl & "'")
                For j = 0 To a.GetLength(1) - 2
                    db.Non_Query("Insert into elscores (topic, class, subtopic, session, student) values ('" & a(1, j) & "', '" & cl & "', '" & a(0, j) & "', '" & tissession & "', '" & st & "')")
                Next
            Next
        Next
    End Sub

    Protected Sub GridView2_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView2.RowDeleting
        Try
            Dim row As GridViewRow = GridView2.Rows(e.RowIndex)
            Dim topic As String = row.Cells(0).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()


                Dim cmdLoad1x As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = '" & DropDownList1.Text & "'", con)

                Dim studentx As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1x.ExecuteReader

                studentx.Read()
                Dim cla As Integer = studentx(0)
                studentx.Close()
                Dim cmdLoad1x2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from eltopics where topic = '" & topic & "' and class = '" & cla & "'", con)

                Dim studentx2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1x2.ExecuteReader

                studentx2.Read()
                Dim topicid As Integer = studentx2(0)
                studentx2.Close()

                Dim cmdLoad1xs As New MySql.Data.MySqlClient.MySqlCommand("Delete from eltopics where id = '" & topicid & "'", con)
                cmdLoad1xs.ExecuteNonQuery()



                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Delete from elsubtopics where topic = '" & topicid & "'", con)
                ref2.ExecuteNonQuery()

                Dim ds2 As New DataTable
                ds2.Columns.Add("aspect")

                Dim cmdLoad1f As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic from eltopics inner join class on class.id = eltopics.class where class.class = '" & DropDownList1.Text & "'", con)
                Dim studentf As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1f.ExecuteReader

                Do While studentf.Read()
                    ds2.Rows.Add(studentf(0))
                Loop
                studentf.Close()
                GridView2.DataSource = ds2
                GridView2.DataBind()

                txtAspect.Text = ""
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView2_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles GridView2.SelectedIndexChanging
        Try
            Dim row As GridViewRow = GridView2.Rows(e.NewSelectedIndex)
            Dim topic As String = row.Cells(0).Text

            Session("edittopic") = topic
            txtAspect.Text = topic


        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView3_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView3.RowDeleting
        Try
            Dim row As GridViewRow = GridView3.Rows(e.RowIndex)
            Dim topic As String = row.Cells(0).Text
            Dim subtopic As String = row.Cells(1).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()


                Dim cmdLoad1x As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = '" & DropDownList1.Text & "'", con)

                Dim studentx As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1x.ExecuteReader

                studentx.Read()
                Dim cla As Integer = studentx(0)
                studentx.Close()

                Dim cmdLoad1x12 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from elsubtopics where subtopic = '" & subtopic & "' and class = '" & cla & "'", con)

                Dim studentx12 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1x12.ExecuteReader

                studentx12.Read()
                Dim subtopicid As Integer = studentx12(0)
                studentx12.Close()



                Dim cmdLoad1xs As New MySql.Data.MySqlClient.MySqlCommand("Delete from elsubtopics where id = '" & subtopicid & "'", con)
                cmdLoad1xs.ExecuteNonQuery()

                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Delete from elscores where subtopic = '" & subtopicid & "'", con)
                ref2.ExecuteNonQuery()
                Dim ds2 As New DataTable
                ds2.Columns.Add("aspect")
                ds2.Columns.Add("subaspect")
                Dim cmdLoad12 As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic, elsubtopics.subtopic from elsubtopics inner join eltopics on eltopics.id = elsubtopics.topic inner join class on elsubtopics.class = class.id where class.class = '" & DropDownList1.Text & "' and eltopics.topic = '" & cboSubAspect.Text & "'", con)
                Dim student2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad12.ExecuteReader

                Do While student2.Read()
                    ds2.Rows.Add(student2(0), student2(1))
                Loop
                student2.Close()

                GridView3.DataSource = ""
                GridView3.DataSource = ds2
                GridView3.DataBind()

                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView3_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles GridView3.SelectedIndexChanging
        Try
            Dim row As GridViewRow = GridView3.Rows(e.NewSelectedIndex)
            Dim subtopic As String = row.Cells(1).Text

            Session("editsubtopic") = subtopic
            txtsubsubAspect.Text = subtopic

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub lnkaspects_Click(sender As Object, e As EventArgs) Handles lnkAspects.Click
        Try

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim ds2 As New DataTable
                ds2.Columns.Add("aspect")

                Dim cmdLoad1f As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic from eltopics inner join class on class.id = eltopics.class where class.class = '" & DropDownList1.Text & "'", con)
                Dim studentf As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1f.ExecuteReader

                Do While studentf.Read()
                    ds2.Rows.Add(studentf(0))
                Loop
                studentf.Close()
                GridView2.DataSource = ds2
                GridView2.DataBind()

                panel4.Visible = True
                pnlstu.Visible = False
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub bnStushow_Click(sender As Object, e As EventArgs) Handles bnStushow.Click
        panel4.Visible = False
        pnlstu.Visible = True
        plcC.Controls.Clear()
        Dim s As New Literal
        s.Text = "<br/>"
        plcC.Controls.Add(s)
    End Sub

    Protected Sub btnShowaspects_Click(sender As Object, e As EventArgs) Handles btnShowAspects.Click
        panel5.Visible = False
        panel4.Visible = True

    End Sub



    Protected Sub cboTerm_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTerm.SelectedIndexChanged
        If cboAspect.Text <> "Select Aspect" Then
            Try                Dim cts(plcC.Controls.Count) As Control                Dim j As Integer = 0                For Each s In plcC.Controls
                    cts(j) = s
                    j = j + 1
                Next                For Each k In cts
                    plcC.Controls.Remove(k)
                Next                Session("grades") = Nothing                txtComments.Text = ""                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Sessioncreate where sessionname = '" & Session("SessionName") & "' Order by ID Desc", con)
                    Dim reader2b As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                    reader2b.Read()
                    Dim tissession = reader2b(0)
                    reader2b.Close()

                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT elsubtopics.subtopic, " & IIf(cboTerm.Text = "1st Term", "elscores.grade", IIf(cboTerm.Text = "2nd Term", "elscores.grade2", "elscores.grade3")) & " from elscores inner join elsubtopics on elscores.subtopic = elsubtopics.id inner join eltopics  on elscores.topic = eltopics.id where eltopics.topic = '" & cboAspect.Text & "' and elscores.student = '" & Session("studentadd") & "' and elscores.session = '" & tissession & "' order by elsubtopics.id", con)
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Dim count As Integer = 1
                    Dim subs As New ArrayList
                    Do While student.Read()
                        subs.Add(count & ". " & student(0))
                        Options_create(count, Val(student(1).ToString), student(0).ToString)
                        count = count + 1
                    Loop
                    student.Close()
                    Session("grades") = count
                    Session("subs") = subs

                    Dim cmdLoad1s As New MySql.Data.MySqlClient.MySqlCommand("SELECT " & IIf(cboTerm.Text = "1st Term", "elscores.comments, elscores.recommendation", IIf(cboTerm.Text = "2nd Term", "elscores.comments2, elscores.recommendation2", "elscores.comments3, elscores.recommendation3")) & " from elscores where elscores.student = '" & Session("studentadd") & "' and elscores.session = '" & tissession & "'", con)
                    Dim students As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1s.ExecuteReader

                    Do While students.Read()
                        txtComments.Text = students(0).ToString
                        txtRecommend.Text = students(1).ToString
                    Loop
                    students.Close()

                    pnlcomments.Visible = True
                    con.Close()
                End Using
            Catch ex As Exception
                Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
            End Try
        End If
    End Sub

    Protected Sub btnReport_Click(sender As Object, e As EventArgs) Handles btnReport.Click
        Try            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim type As String
                Dim cmd21 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.type, class.id from studentsummary inner join class on class.id = studentsummary.class where studentsummary.student = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
                cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("Studentadd")))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd21.ExecuteReader()
                reader2.Read()
                type = reader2(0)
                Session("rsClass") = reader2(1)
                reader2.Close()
                con.Close()
            End Using

            Session("rsSession") = Session("sessionid")
            Session("studentid") = Session("studentadd")
            Response.Redirect("~/content/Student/result.aspx")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
        gridview1.SelectedIndex = -1
    End Sub
End Class
