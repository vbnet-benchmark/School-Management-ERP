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
       
        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try
            If Not IsPostBack Then

                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class where type = 'Early Years'", con)
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    DropDownList2.Items.Clear()
                    DropDownList2.Items.Add("Select Class")
                    Do While student.Read
                        DropDownList2.Items.Add(student.Item(0).ToString)
                    Loop
                    student.Close()
                    Dim ds2 As New DataTable
                    ds2.Columns.Add("aspect")
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic from eltopics inner join class on class.id = eltopics.class where class.class = '" & DropDownList2.Text & "'", con)
                    Dim studentd As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

                    Do While studentd.Read()
                        ds2.Rows.Add(studentd(0))
                    Loop
                    studentd.Close()
                    GridView2.DataSource = ds2
                    GridView2.DataBind()

                    con.Close()                End Using

            End If
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

                Dim cmdLoad1x As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = '" & DropDownList2.Text & "'", con)

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

                Dim cmdLoad1f As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic from eltopics inner join class on class.id = eltopics.class where class.class = '" & DropdownList2.Text & "'", con)
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
                Dim cmdLoad1z As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic from eltopics inner join class on eltopics.class = class.id where class.class = '" & DropdownList2.Text & "'", con)
                Dim studentz As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1z.ExecuteReader
                cboSubAspect.Items.Clear()
                cboSubAspect.Items.Add("Select Aspect")
                Do While studentz.Read()
                    cboSubAspect.Items.Add(studentz(0))
                Loop
                studentz.Close()
                panel1.Visible = False
                panel5.Visible = True

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
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic, elsubtopics.subtopic from elsubtopics inner join eltopics on eltopics.id = elsubtopics.topic inner join class on elsubtopics.class = class.id where class.class = '" & DropdownList2.Text & "' and eltopics.topic = '" & cboSubAspect.Text & "'", con)
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
                Dim cmdLoad1x As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = '" & DropDownList2.Text & "'", con)

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

                Dim cmdLoad1g As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic, elsubtopics.subtopic from elsubtopics inner join eltopics on eltopics.id = elsubtopics.topic inner join class on elsubtopics.class = class.id where class.class = '" & DropdownList2.Text & "' and eltopics.topic = '" & cboSubAspect.Text & "'", con)
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



    Protected Sub GridView2_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView2.RowDeleting
        Try
            Dim row As GridViewRow = GridView2.Rows(e.RowIndex)
            Dim topic As String = row.Cells(0).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()


                Dim cmdLoad1x As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = '" & DropDownList2.Text & "'", con)

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

                Dim db As New DB_Interface
                Dim tissession As Integer = db.Select_single("select id from sessioncreate where sessionname = '" & Session("sessionname") & "'")

                Dim cmdLoad1xsd As New MySql.Data.MySqlClient.MySqlCommand("Delete from elscores where topic = '" & topicid & "' and session = '" & tissession & "'", con)
                cmdLoad1xsd.ExecuteNonQuery()


                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Delete from elsubtopics where topic = '" & topicid & "'", con)
                ref2.ExecuteNonQuery()

                Dim ds2 As New DataTable
                ds2.Columns.Add("aspect")

                Dim cmdLoad1f As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic from eltopics inner join class on class.id = eltopics.class where class.class = '" & DropdownList2.Text & "'", con)
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


                Dim cmdLoad1x As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = '" & DropDownList2.Text & "'", con)

                Dim studentx As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1x.ExecuteReader

                studentx.Read()
                Dim cla As Integer = studentx(0)
                studentx.Close()

                Dim cmdLoad1x12 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from elsubtopics where subtopic = '" & subtopic & "' and class = '" & cla & "'", con)

                Dim studentx12 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1x12.ExecuteReader

                studentx12.Read()
                Dim subtopicid As Integer = studentx12(0)
                studentx12.Close()
                Dim db As New DB_Interface
                Dim tissession As Integer = db.Select_single("select id from sessioncreate where sessionname = '" & Session("sessionname") & "'")



                Dim cmdLoad1xs As New MySql.Data.MySqlClient.MySqlCommand("Delete from elsubtopics where id = '" & subtopicid & "'", con)
                cmdLoad1xs.ExecuteNonQuery()

                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Delete from elscores where subtopic = '" & subtopicid & "' and session = '" & tissession & "'", con)
                ref2.ExecuteNonQuery()
                Dim ds2 As New DataTable
                ds2.Columns.Add("aspect")
                ds2.Columns.Add("subaspect")
                Dim cmdLoad12 As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic, elsubtopics.subtopic from elsubtopics inner join eltopics on eltopics.id = elsubtopics.topic inner join class on elsubtopics.class = class.id where class.class = '" & DropdownList2.Text & "' and eltopics.topic = '" & cboSubAspect.Text & "'", con)
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


    Protected Sub DropDownList2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList2.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()

                Dim ds2 As New DataTable
                ds2.Columns.Add("aspect")
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic from eltopics inner join class on class.id = eltopics.class where class.class = '" & DropDownList2.Text & "'", con)
                Dim studentd As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader

                Do While studentd.Read()
                    ds2.Rows.Add(studentd(0))
                Loop
                studentd.Close()
                GridView2.DataSource = ds2
                GridView2.DataBind()
                panel1.Visible = True
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub

    Protected Sub btnTopicLookup_Click(sender As Object, e As EventArgs) Handles btnTopicLookup.Click
        Try
            Dim strategy As TopicUsageLookupStrategy
            If ddlTopicLookupMode.SelectedValue = "legacy" Then
                strategy = New LegacyTopicNameLookup
            Else
                strategy = New NumericTopicIdLookup
            End If
            GridViewTopicUsage.DataSource = strategy.LookupUsage(txtTopicKey.Text)
            GridViewTopicUsage.DataBind()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

End Class
