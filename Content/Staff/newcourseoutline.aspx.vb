Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_addteacher
    Inherits System.Web.UI.Page
    Dim subselect As Integer
    Dim subremove As String
    Dim classremove As String
    Dim staffID As String

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


    Sub all_topics()
        Dim db As New DB_Interface
        Dim a As ArrayList = db.Select_1D("select id from class where type = 'K.G 1 Special'")
        For Each cla In a
            Dim s As ArrayList = db.Select_1D("select id from courseoutline where class = '" & cla & "' and session = '" & Session("sessionid") & "'")
            For Each sn In s
                Dim j As ArrayList = db.Select_1D("select * from courseoutline where id = '" & sn & "'")
                db.Non_Query("insert into kcourseoutline (subject, class, session, week, topic) values ('" & j(0) & "', '" & j(2) & "', '" & j(1) & "', '" & j(3) & "', '" & j(4) & "'")
                Dim id As Integer = db.Select_single("select id from kcourseoutline where topic = '" & j(4).ToString.Replace("'", "") & "' and session = '" & Session("sessionid") & "' and subject = '" & j(0) & "' and class = '" & j(2) & "'")
                Dim students As ArrayList = db.Select_1D("select sudent from studentsummary where class = '" * cla & "' and session = '" & Session("sessionid") & "'")
                For Each st In students
                    db.Non_Query("insert into kscoresheet (session, class, subject, topic, student) values ('" & Session("sessionid") & "', '" & cla & "', '" & j(0) & "', '" & id & "', '" & st & "'")
                Next
            Next
        Next
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim exists As Boolean
                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select id, type from class where class = '" & cboClass.Text & "'", con)
                Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                readref2.Read()
                Dim clas As Integer = readref2.Item(0)
                Dim type As String = readref2(1).ToString
                readref2.Close()
                Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Select id from subjects where subject = '" & cboSubject.Text & "'", con)
                Dim readref3 As MySql.Data.MySqlClient.MySqlDataReader = ref3.ExecuteReader
                readref3.Read()
                Dim subject As Integer = readref3.Item(0)
                readref3.Close()
                If type <> "K.G 1 Special" Then
                    Dim cmdCheck200 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where session.id = '" & Session("sessionid") & "' and class.class = '" & cboClass.Text & "' and subjects.subject = '" & cboSubject.Text & "' and week = '" & cboWeek.Text & "'", con)
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck200.ExecuteReader()
                    If reader20.Read() Then
                        exists = True
                    End If
                    reader20.Close()
                    If exists = True Then
                        Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update courseoutline Set topic = '" & txtTopic.Text & "', content = '" & txtContent.Text & "' where subject = '" & subject & "' and class = '" & clas & "' and session = '" & Session("sessionid") & "' and week = '" & cboWeek.Text & "'", con)
                        cmdCheck3.ExecuteNonQuery()
                    Else
                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into courseoutline (session, class, subject, topic, content, week) Values (?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", Session("sessionid")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", clas))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", subject))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", txtTopic.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("bu", txtContent.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("b", cboWeek.Text))
                        cmdCheck2.ExecuteNonQuery()
                    End If
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "' and courseoutline.session = '" & Session("NSessionId") & "' order by courseoutline.week asc", con)
                    Dim ds As New DataTable
                    ds.Columns.Add("Week")
                    ds.Columns.Add("Topic")
                    ds.Columns.Add("Content")

                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Do While reader.Read
                        ds.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2))
                    Loop
                    reader.Close()
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                Else
                    Dim cmdCheck200 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM kcourseoutline inner join subjects on subjects.id = kcourseoutline.subject inner join session on session.id = kcourseoutline.session inner join class on class.id = kcourseoutline.class where session.id = '" & Session("sessionid") & "' and class.class = '" & cboClass.Text & "' and subjects.subject = '" & cboSubject.Text & "' and week = '" & cboWeek.Text & "'", con)
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck200.ExecuteReader()
                    If reader20.Read() Then
                        exists = True
                    End If
                    reader20.Close()
                    If exists = True Then
                        Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update kcourseoutline Set topic = '" & txtTopic.Text & "', content = '" & txtContent.Text & "' where subject = '" & subject & "' and class = '" & clas & "' and session = '" & Session("sessionid") & "' and week = '" & cboWeek.Text & "'", con)
                        cmdCheck3.ExecuteNonQuery()
                    Else
                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into kcourseoutline (session, class, subject, topic, content, week) Values (?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", Session("sessionid")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", clas))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", subject))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", txtTopic.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("bu", txtContent.Text))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("b", cboWeek.Text))
                        cmdCheck2.ExecuteNonQuery()
                        Dim refs As New MySql.Data.MySqlClient.MySqlCommand("Select studentsummary.student from studentsummary inner join class on class.id = studentsummary.class where class.class = '" & cboClass2.Text & "'", con)
                        Dim readrefs As MySql.Data.MySqlClient.MySqlDataReader = refs.ExecuteReader
                        Dim students As New ArrayList
                        Do While readrefs.Read
                            students.Add(readrefs(0).ToString)
                        Loop
                        readrefs.Close()
                        Dim cmdLoa As New MySql.Data.MySqlClient.MySqlCommand("SELECT kcourseoutline.id from kcourseoutline inner join subjects on subjects.id = kcourseoutline.subject inner join session on session.id = kcourseoutline.session inner join class on class.id = kcourseoutline.class where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "' and kcourseoutline.session = '" & Session("NSessionId") & "' and kcourseoutline.week = '" & cboWeek.Text & "'", con)
                        Dim sss As MySql.Data.MySqlClient.MySqlDataReader = cmdLoa.ExecuteReader
                        sss.Read()
                        Dim courseid As Integer = sss(0)
                        sss.Close()

                        For Each student As String In students
                            Dim cmdceck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into kscoresheet (session, class, subject, topic, student) Values (?,?,?,?,?)", con)
                            cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", Session("sessionid")))
                            cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", clas))
                            cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", subject))
                            cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", courseid))
                            cmdceck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("bu", student))
                            cmdceck2.ExecuteNonQuery()
                        Next


                    End If
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from kcourseoutline inner join subjects on subjects.id = kcourseoutline.subject inner join session on session.id = kcourseoutline.session inner join class on class.id = kcourseoutline.class where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "' and kcourseoutline.session = '" & Session("NSessionId") & "' order by kcourseoutline.week asc", con)
                    Dim ds As New DataTable
                    ds.Columns.Add("Week")
                    ds.Columns.Add("Topic")
                    ds.Columns.Add("Content")

                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Do While reader.Read
                        ds.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2))
                    Loop
                    reader.Close()
                    GridView1.DataSource = ds
                    GridView1.DataBind()


                End If
                Show_Alert(True, "Course Outline Updated.")
                panel1.Visible = False
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Subject(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If Not IsPostBack Then
                    Session("NSessionId") = Session("SessionId")

                    If Session("classub") <> Nothing Then
                        Dim refs As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject from classsubjects inner join subjects on classsubjects.subject = subjects.ID where classsubjects.teacher = '" & Session("StaffId") & "'", con)
                        Dim readrefs As MySql.Data.MySqlClient.MySqlDataReader = refs.ExecuteReader
                        cboSubject2.Items.Clear()
                        Dim subj As New ArrayList
                        cboSubject2.Items.Add("SELECT")
                        Do While readrefs.Read
                            If Not subj.Contains(readrefs(0)) Then
                                cboSubject2.Items.Add(readrefs.Item(0))
                                subj.Add(readrefs(0))
                            End If
                        Loop
                        readrefs.Close()
                        Dim cl As String
                        Dim cmdCheck200 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, class.class FROM classsubjects inner join subjects on subjects.id = classsubjects.subject inner join class on class.id = classsubjects.class where classsubjects.id = '" & Session("classub") & "'", con)
                        Dim reader200 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck200.ExecuteReader()
                        If reader200.Read() Then
                            cboSubject2.Text = reader200.Item(0)
                            cl = reader200.Item(1).ToString
                        End If
                        reader200.Close()


                        Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id where subjects.subject = '" & cboSubject2.Text & "'", con)
                        Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                        cboClass2.Items.Clear()
                        Do While student0.Read
                            cboClass2.Items.Add(student0.Item(0))
                        Loop
                        student0.Close()
                        cboClass2.Text = cl
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "' and courseoutline.session = '" & Session("NSessionId") & "' order by courseoutline.week asc", con)
                        Dim ds As New DataTable
                        ds.Columns.Add("Week")
                        ds.Columns.Add("Topic")
                        ds.Columns.Add("Content")

                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Do While reader.Read
                            ds.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2))
                        Loop
                        reader.Close()
                        GridView1.DataSource = ds
                        GridView1.DataBind()

                    Else
                        Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject from classsubjects inner join subjects on classsubjects.subject = subjects.ID where classsubjects.teacher = '" & Session("StaffId") & "'", con)
                        Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                        cboSubject2.Items.Clear()
                        Dim subj As New ArrayList
                        cboSubject2.Items.Add("SELECT")
                        Do While readref.Read
                            If Not subj.Contains(readref(0)) Then
                                cboSubject2.Items.Add(readref.Item(0))
                                subj.Add(readref(0))
                            End If
                        Loop
                        readref.Close()
                    End If





                    cboWeek.Items.Clear()
                    cboWeek.Items.Add("SELECT")
                    For i As Integer = 1 To 14

                        cboWeek.Items.Add(i)
                    Next
                    If Request.QueryString.ToString <> Nothing Then

                    Else
                        Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject from classsubjects inner join subjects on classsubjects.subject = subjects.ID where classsubjects.teacher = '" & Session("StaffId") & "'", con)
                        Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                        cboSubject.Items.Clear()
                        cboSubject.Items.Add("SELECT")
                        Do While readref.Read
                            cboSubject.Items.Add(readref.Item(0))
                        Loop
                        readref.Close()
                        Dim cl As String
                        Dim cmdCheck200 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, class.class FROM classsubjects inner join subjects on subjects.id = classsubjects.subject inner join class on class.id = classsubjects.class where classsubjects.id = '" & Session("classub") & "'", con)
                        Dim reader200 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck200.ExecuteReader()
                        If reader200.Read() Then
                            cboSubject.Text = reader200.Item(0)
                            cl = reader200.Item(1).ToString
                        End If
                        reader200.Close()


                        Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id where subjects.subject = '" & cboSubject.Text & "'", con)
                        Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                        cboClass.Items.Clear()
                        Do While student0.Read
                            cboClass.Items.Add(student0.Item(0))
                        Loop
                        student0.Close()
                        cboClass.Text = cl
                    End If
                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub cboSubject2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubject2.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id where subjects.subject = '" & cboSubject2.Text & "' and classsubjects.teacher = '" & Session("staffid") & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                cboClass2.Items.Clear()
                cboClass2.Items.Add("SELECT")
                Do While student0.Read
                    cboClass2.Items.Add(student0.Item(0))
                Loop
                student0.Close()
                GridView1.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub GridView1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView1.RowDeleting

        Try
            Dim row As GridViewRow = GridView1.Rows(e.RowIndex)
            Dim week As String = row.Cells(0).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim id As String
                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select id, type from class where class = '" & cboClass2.Text & "'", con)
                Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                readref2.Read()
                Dim clas As Integer = readref2.Item(0)
                Dim type As String = readref2(1).ToString
                readref2.Close()
                If type <> "K.G 1 Special" Then
                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT courseoutline.id FROM courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where courseoutline.week = '" & week & "' and subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "'", con)
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck20.ExecuteReader()
                    If reader20.Read() Then
                        id = reader20(0).ToString
                    End If
                    reader20.Close()
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("delete from courseoutline where id = '" & id & "'", con)
                    cmdLoad0.ExecuteNonQuery()
                Else
                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT kcourseoutline.id FROM kcourseoutline inner join subjects on subjects.id = kcourseoutline.subject inner join session on session.id = kcourseoutline.session inner join class on class.id = kcourseoutline.class where kcourseoutline.week = '" & week & "' and subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "'", con)
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck20.ExecuteReader()
                    If reader20.Read() Then
                        id = reader20(0).ToString
                    End If
                    reader20.Close()
                    Dim cmdceck2 As New MySql.Data.MySqlClient.MySqlCommand("delete from kscoresheet where topic = '" & id & "' and session = '" & Session("sessionid") & "'", con)
                    cmdceck2.ExecuteNonQuery()
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("delete from kcourseoutline where id = '" & id & "' and session = '" & Session("sessionid") & "'", con)
                    cmdLoad0.ExecuteNonQuery()

                End If

                If type <> "K.G 1 Special" Then
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "' and courseoutline.session = '" & Session("NSessionId") & "' order by courseoutline.week asc", con)
                    Dim ds As New DataTable
                    ds.Columns.Add("Week")
                    ds.Columns.Add("Topic")
                    ds.Columns.Add("Content")

                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Do While reader.Read
                        ds.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2))
                    Loop
                    reader.Close()
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                Else
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from kcourseoutline inner join subjects on subjects.id = kcourseoutline.subject inner join session on session.id = kcourseoutline.session inner join class on class.id = kcourseoutline.class where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "' and kcourseoutline.session = '" & Session("NSessionId") & "' order by kcourseoutline.week asc", con)
                    Dim ds As New DataTable
                    ds.Columns.Add("Week")
                    ds.Columns.Add("Topic")
                    ds.Columns.Add("Content")

                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Do While reader.Read
                        ds.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2))
                    Loop
                    reader.Close()
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                End If

                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub cboClass2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboClass2.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select id, type from class where class = '" & cboClass2.Text & "'", con)
                Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                readref2.Read()
                Dim clas As Integer = readref2.Item(0)
                Dim type As String = readref2(1).ToString
                readref2.Close()
                If type <> "K.G 1 Special" Then
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "' and courseoutline.session = '" & Session("NSessionId") & "' order by courseoutline.week asc", con)
                    Dim ds As New DataTable
                    ds.Columns.Add("Week")
                    ds.Columns.Add("Topic")
                    ds.Columns.Add("Content")

                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Do While reader.Read
                        ds.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2))
                    Loop
                    reader.Close()
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                Else
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from kcourseoutline inner join subjects on subjects.id = kcourseoutline.subject inner join session on session.id = kcourseoutline.session inner join class on class.id = kcourseoutline.class where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "' and kcourseoutline.session = '" & Session("NSessionId") & "' order by kcourseoutline.week asc", con)
                    Dim ds As New DataTable
                    ds.Columns.Add("Week")
                    ds.Columns.Add("Topic")
                    ds.Columns.Add("Content")

                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Do While reader.Read
                        ds.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2))
                    Loop
                    reader.Close()
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                End If

                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select classsubjects.id from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                If student0.Read() Then Session("classub") = student0.Item(0)
                student0.Close()
                con.close()            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub cboSubject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubject.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id where subjects.subject = '" & cboSubject.Text & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                cboClass.Items.Clear()
                Do While student0.Read
                    cboClass.Items.Add(student0.Item(0))
                Loop
                student0.Close()
                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select id, type from class where class = '" & cboClass2.Text & "'", con)
                Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                readref2.Read()
                Dim clas As Integer = readref2.Item(0)
                Dim type As String = readref2(1).ToString
                readref2.Close()
                If type <> "K.G 1 Special" Then
                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where session.id = '" & Session("Sessionid") & "' and class.class = '" & cboClass.Text & "' and subjects.subject = '" & cboSubject.Text & "' and courseoutline.week = '" & cboWeek.Text & "'", con)
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck20.ExecuteReader()
                    If reader20.Read() Then
                        txtTopic.Text = reader20.Item("topic")
                        txtContent.Text = reader20.Item("content")
                    Else
                        txtTopic.Text = ""
                        txtContent.Text = ""
                    End If
                    reader20.Close()
                Else
                    Dim cmdcheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from kcourseoutline inner join subjects on subjects.id = kcourseoutline.subject inner join session on session.id = kcourseoutline.session inner join class on class.id = kcourseoutline.class where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "' and kcourseoutline.session = '" & Session("NSessionId") & "' order by kcourseoutline.week asc", con)
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck20.ExecuteReader()
                    If reader20.Read() Then
                        txtTopic.Text = reader20.Item("topic")
                        txtContent.Text = reader20.Item("content")
                    Else
                        txtTopic.Text = ""
                        txtContent.Text = ""
                    End If
                    reader20.Close()
                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub cboWeek_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboWeek.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select id, type from class where class = '" & cboClass2.Text & "'", con)
                Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                readref2.Read()
                Dim clas As Integer = readref2.Item(0)
                Dim type As String = readref2(1).ToString
                readref2.Close()
                If type <> "K.G 1 Special" Then
                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where session.id = '" & Session("Sessionid") & "' and class.class = '" & cboClass.Text & "' and subjects.subject = '" & cboSubject.Text & "' and courseoutline.week = '" & cboWeek.Text & "'", con)
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck20.ExecuteReader()
                    If reader20.Read() Then
                        txtTopic.Text = reader20.Item("topic")
                        txtContent.Text = reader20.Item("content")
                    Else
                        txtTopic.Text = ""
                        txtContent.Text = ""
                    End If
                    reader20.Close()
                Else
                    Dim cmdcheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from kcourseoutline inner join subjects on subjects.id = kcourseoutline.subject inner join session on session.id = kcourseoutline.session inner join class on class.id = kcourseoutline.class where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "' and kcourseoutline.session = '" & Session("NSessionId") & "'and kcourseoutline.week = '" & cboWeek.Text & "'", con)
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck20.ExecuteReader()
                    If reader20.Read() Then
                        txtTopic.Text = reader20.Item("topic")
                        txtContent.Text = reader20.Item("content")
                    Else
                        txtTopic.Text = ""
                        txtContent.Text = ""
                    End If
                    reader20.Close()
                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles GridView1.SelectedIndexChanging
        Try

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim week As Integer = GridView1.Rows(e.NewSelectedIndex).Cells(0).Text
                cboWeek.Items.Clear()
                cboWeek.Items.Add("SELECT")
                For i As Integer = 1 To 14

                    cboWeek.Items.Add(i)
                Next
                Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject from classsubjects inner join subjects on classsubjects.subject = subjects.ID where classsubjects.teacher = '" & Session("StaffId") & "'", con)
                Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                cboSubject.Items.Clear()
                cboSubject.Items.Add("SELECT")
                Do While readref.Read
                    cboSubject.Items.Add(readref.Item(0))
                Loop
                readref.Close()
                Dim cl As String
                Dim cmdCheck200 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, class.class FROM classsubjects inner join subjects on subjects.id = classsubjects.subject inner join class on class.id = classsubjects.class where classsubjects.id = '" & Session("classub") & "'", con)
                Dim reader200 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck200.ExecuteReader()
                If reader200.Read() Then
                    cboSubject.Text = reader200.Item(0)
                    cl = reader200.Item(1)
                End If
                reader200.Close()
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id where subjects.subject = '" & cboSubject.Text & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                cboClass.Items.Clear()
                Do While student0.Read
                    cboClass.Items.Add(student0.Item(0))
                Loop
                student0.Close()
                cboClass.Text = cl

                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select id, type from class where class = '" & cboClass2.Text & "'", con)
                Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                readref2.Read()
                Dim clas As Integer = readref2.Item(0)
                Dim type As String = readref2(1).ToString
                readref2.Close()
                If type <> "K.G 1 Special" Then

                    Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT courseoutline.* FROM courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where courseoutline.week = '" & week & "' and subjects.subject = '" & cboSubject.Text & "' and class.class = '" & cboClass.Text & "' and courseoutline.session = '" & Session("sessionid") & "'", con)
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck20.ExecuteReader()
                    If reader20.Read() Then
                        cboWeek.Text = week
                        txtTopic.Text = reader20.Item("topic")
                        txtContent.Text = reader20.Item("content")
                    End If
                    reader20.Close()
                Else
                    Dim cmdcheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from kcourseoutline inner join subjects on subjects.id = kcourseoutline.subject inner join session on session.id = kcourseoutline.session inner join class on class.id = kcourseoutline.class where subjects.subject = '" & cboSubject2.Text & "' and class.class = '" & cboClass2.Text & "' and kcourseoutline.session = '" & Session("NSessionId") & "' order by kcourseoutline.week asc", con)
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck20.ExecuteReader()
                    If reader20.Read() Then
                        cboWeek.Text = week
                        txtTopic.Text = reader20.Item("topic")
                        txtContent.Text = reader20.Item("content")
                    End If
                    reader20.Close()
                End If
                panel1.Visible = True
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkNew_Click(sender As Object, e As EventArgs) Handles lnkNew.Click
        If cboSubject2.Text = "SELECT" Or cboClass2.Text = "SELECT" Then
            Show_Alert(False, "Select a subject and class")
            Exit Sub
        End If
        panel1.Visible = True
        cboSubject.Items.Clear()
        cboClass.Items.Clear()
        cboSubject.Items.Add(cboSubject2.Text)
        cboClass.Items.Add(cboClass2.Text)
        cboWeek.Items.Clear()
        cboWeek.Items.Add("SELECT")
        For i As Integer = 1 To 14

            cboWeek.Items.Add(i)
        Next
        txtTopic.Text = ""
        txtContent.Text = ""
    End Sub
End Class
