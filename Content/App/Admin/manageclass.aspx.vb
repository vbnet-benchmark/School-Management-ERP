Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_newsession
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


    Protected Sub GridView1_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GridView1.RowCancelingEdit
        GridView1.EditIndex = -1
    End Sub

    Protected Sub GridView1_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GridView1.RowEditing
        Try
            Gridview1.EditIndex = e.NewEditIndex
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("Class")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, class from class order by ID", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview1.DataSource = a
                Gridview1.Columns(0).Visible = True
                Gridview1.DataBind()
                msg.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub GridView1_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridView1.RowUpdating
        Try
            Dim row As GridViewRow = Gridview1.Rows(e.RowIndex)
            Dim ID As String = row.Cells(0).Text
            Dim sessions As String = TryCast(row.Cells(1).Controls(0), TextBox).Text

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update class set class = '" & sessions & "' where ID = '" & ID & "'", con)
                enter.ExecuteNonQuery()
                Gridview1.EditIndex = -1
                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("Class")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, class from class order by ID", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview1.DataSource = a
                Gridview1.Columns(0).Visible = False

                Gridview1.DataBind()
                msg.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try
            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()

                    Dim cmd5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From depts", con)
                    Dim reader5 As MySql.Data.MySqlClient.MySqlDataReader = cmd5.ExecuteReader
                    cboDept.Items.Clear()
                    cboDept.Items.Add("Select Department")
                    Do While reader5.Read
                        cboDept.Items.Add(reader5.Item(1))
                    Loop
                    reader5.Close()

                    Dim a As New DataTable
                    a.Columns.Add("ID")
                    a.Columns.Add("Class")
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, class from class order by ID", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    Do While msg.Read()
                        a.Rows.Add(msg.Item(0), msg.Item(1))
                    Loop
                    Gridview1.DataSource = a

                    Gridview1.DataBind()
                    msg.Close()

                    If Session("tisclas") <> Nothing Then
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class as Class, studentsummary.classNo as 'No of Students', class.type as 'Type', depts.dept as 'Department' from class inner join depts on class.superior = depts.id left join studentsummary on class.id = studentsummary.class where class.class = '" & Session("tisclas") & "' and studentsummary.session = '" & Session("sessionid") & "' order by studentsummary.session desc", con)
                        Dim ds As New DataTable
                        Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                        adapter1.SelectCommand = cmdLoad1
                        adapter1.Fill(ds)
                        DetailsView1.DataSource = ds
                        DetailsView1.DataBind()
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from classteacher inner join class on class.id = classteacher.class inner join staffprofile on classteacher.teacher = staffprofile.staffid where class.class = '" & Session("tisclas") & "'", con)
                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Dim ds2 As New DataTable
                        ds2.Columns.Add("S/N")
                        ds2.Columns.Add("Name")
                        Dim i As Integer = 1
                        Do While reader.Read
                            ds2.Rows.Add(i.ToString & ".  ", reader.Item(0))
                            i = i + 1
                        Loop
                        GridView2.DataSource = ds2
                        GridView2.DataBind()
                        reader.Close()

                        Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, staffprofile.surname, classsubjects.periods from classsubjects inner join class on class.id = classsubjects.class left join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on subjects.id = classsubjects.subject where class.class = '" & Session("tisclas") & "'", con)
                        Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                        Dim ds3 As New DataTable
                        ds3.Columns.Add("S/N")
                        ds3.Columns.Add("Subject")
                        ds3.Columns.Add("name")
                        ds3.Columns.Add("periods")
                        ds3.Columns.Add("View")
                        Dim j As Integer = 1
                        Dim totalp As Integer
                        Do While reader0.Read
                            ds3.Rows.Add(j.ToString & ".    ", reader0.Item(0), reader0.Item(1), reader0.Item(2), "Edit")
                            j = j + 1
                            totalp = totalp + Val(reader0(2))
                        Loop

                        GridView3.DataSource = ds3
                        GridView3.DataBind()
                        Dim roww As GridViewRow = GridView3.FooterRow

                        roww.Cells(1).Text = "Total Periods"
                        roww.Cells(3).Text = totalp

                        reader0.Close()


                        panel1.Visible = True
                    End If
                    con.Close()                End Using
            End If

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub linkbterm_Click(sender As Object, e As EventArgs) Handles linkbterm.Click
        Response.Redirect("~/content/App/Admin/addclass.aspx")

    End Sub
    Private Function get_id() As Integer

        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            If con.State = ConnectionState.Closed Then
                con.Open()
            End If

            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID from class where class = '" & Session("tisclas") & "'", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            msg.Read()
            Dim clasid As Integer = msg(0).ToString
            msg.Close()
            Return clasid
            con.Close()
        End Using
    End Function
    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Response.Redirect("~/content/App/Admin/classallocate.aspx?" & get_id())

    End Sub

    Protected Sub GridView2_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView2.RowDeleting
        Try
            Dim row As GridViewRow = GridView2.Rows(e.RowIndex)
            Dim ID As String = row.Cells(0).Text
            Dim sessions As String = row.Cells(1).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim teacher As String
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from staffprofile where surname = '" & sessions & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                If student0.Read Then
                    teacher = student0.Item(0).ToString
                End If
                student0.Close()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("delete from classteacher where teacher = '" & teacher & "' and class = '" & get_id() & "'", con)
                enter.ExecuteNonQuery()
                logify.log(Session("staffid"), teacher & " was removed as a class teacher in " & Session("tisclas"))
                logify.Notifications("You are no more the class teacher of " & Session("tisclas"), teacher, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                Show_Alert(True, "Class teacher removed successfully")
                Gridview1.EditIndex = -1
                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid where classteacher.class = '" & get_id() & "'", con)
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                Dim ds2 As New DataTable
                ds2.Columns.Add("S/N")
                ds2.Columns.Add("Name")
                Dim i As Integer = 1
                Do While reader.Read
                    ds2.Rows.Add(i.ToString & ".  ", reader.Item(0))
                    i = i + 1
                Loop
                GridView2.DataSource = ds2
                GridView2.DataBind()
                reader.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView2.SelectedIndexChanged

    End Sub

    Protected Sub Gridview1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Gridview1.SelectedIndexChanged

    End Sub

    Protected Sub lnkSubject_Click(sender As Object, e As EventArgs) Handles lnkSubject.Click
        Response.Redirect("~/content/App/Admin/addclassubject.aspx")
    End Sub

    Protected Sub GridView3_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView3.RowDeleting
        Try
            Dim row As GridViewRow = GridView3.Rows(e.RowIndex)
            Dim ID As String = row.Cells(1).Text
            Dim subject As Integer
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad01 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from subjects where subject = '" & ID & "'", con)
                Dim student01 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad01.ExecuteReader
                If student01.Read Then
                    subject = student01.Item(0).ToString
                End If
                student01.Close()
                Dim teacher As String = ""
                Dim cmdLoad01s As New MySql.Data.MySqlClient.MySqlCommand("SELECT teacher from classsubjects where subject = '" & subject & "' and class = '" & get_id() & "'", con)
                Dim student01s As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad01s.ExecuteReader
                If student01s.Read Then
                    teacher = student01s.Item(0).ToString
                End If
                student01s.Close()

                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("delete from classsubjects where subject = '" & subject & "' and class = '" & get_id() & "'", con)
                enter.ExecuteNonQuery()
                Dim cmdceck2 As New MySql.Data.MySqlClient.MySqlCommand("delete from kscoresheet where subject = '" & subject & "' and session = '" & Session("sessionid") & "'", con)
                cmdceck2.ExecuteNonQuery()
                Dim cmdLoad0z As New MySql.Data.MySqlClient.MySqlCommand("delete from kcourseoutline where subject = '" & subject & "' and session = '" & Session("sessionid") & "'", con)
                cmdLoad0z.ExecuteNonQuery()
                logify.log(Session("staffid"), ID & " was removed as a subject offerred by " & Session("tisclas"))
                If teacher <> "" Then logify.Notifications(ID & " is no longer offerred by " & Session("tisclas"), teacher, Session("staffid"), "Admin", "~/content/staff/timetable.aspx", "")
                Dim classesa As New MySql.Data.MySqlClient.MySqlCommand("Select subjectreg.student from subjectreg inner join class on subjectreg.class = class.id inner join subjects on subjectreg.subjectsofferred =  subjects.id where subjects.subject = '" & ID & "' and subjectreg.session = '" & Session("sessionid") & "' and class.class = '" & Session("tisclas") & "'", con)
                Dim schclassa As MySql.Data.MySqlClient.MySqlDataReader = classesa.ExecuteReader
                Do While schclassa.Read
                    logify.Notifications("You no longer offer " & ID, schclassa(0), Session("staffid"), "Admin", "~/content/student/classdetails.aspx", "")
                    logify.Notifications(ID & " is no longer offerred by " & par.getstuname(schclassa(0)), par.getparent(schclassa(0)), Session("staffid"), "Admin", "~/content/parent/classdetails.aspx?" & schclassa(0), "")
                Loop
                schclassa.Close()
                Dim enters As New MySql.Data.MySqlClient.MySqlCommand("delete from subjectreg where subjectsofferred = '" & subject & "' and class = '" & get_id() & "' and session = '" & Session("sessionid") & "'", con)
                enters.ExecuteNonQuery()
                Dim cmdInsert137a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ttname.id, ttname.school, ttname.name from ttname inner join class on class.superior = ttname.school where class.class = '" & Session("tisclas") & "'", con)
                Dim student0d As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert137a.ExecuteReader
                If student0d.Read() Then
                    Dim ttid As String = student0d(0)
                    Dim superior As String = student0d(1)
                    Dim ttname As String = student0d(2)
                    student0d.Close()

                    Dim enters2 As New MySql.Data.MySqlClient.MySqlCommand("delete from timetable where tname = '" & ttid & "'", con)
                    enters2.ExecuteNonQuery()


                    Dim teachingstaff As New ArrayList
                    Dim classes112 As New MySql.Data.MySqlClient.MySqlCommand("Select classsubjects.teacher from classsubjects inner join class on class.id = classsubjects.class where class.superior = '" & superior & "'", con)
                    Dim schclass112 As MySql.Data.MySqlClient.MySqlDataReader = classes112.ExecuteReader
                    Do While schclass112.Read
                        If Not teachingstaff.Contains(schclass112(0)) Then
                            teachingstaff.Add(schclass112(0))
                        End If
                    Loop
                    schclass112.Close()
                    Dim students As New ArrayList
                    Dim classes112s As New MySql.Data.MySqlClient.MySqlCommand("Select studentsummary.student, studentsprofile.surname, class.class from studentsummary inner join studentsprofile on studentsprofile.admno = studentsummary.student inner join class on class.id = studentsummary.class where class.superior = '" & superior & "' and studentsummary.session = '" & Session("sessionid") & "'", con)
                    Dim schclass112s As MySql.Data.MySqlClient.MySqlDataReader = classes112s.ExecuteReader
                    Dim studentname As New ArrayList
                    Dim classes As New ArrayList

                    Do While schclass112s.Read
                        students.Add(schclass112s(0))
                        studentname.Add(schclass112s(1))
                        classes.Add(schclass112s(2))
                    Loop
                    schclass112s.Close()

                    Dim no As Integer = 0
                    For Each staff As String In teachingstaff
                        logify.Notifications(ttname & " is no more in use. A  new one will be published soon.", staff, Session("staffid"), "Admin", "~/content/staff/timetable.aspx", "")
                    Next
                    For Each student As String In students
                        logify.Notifications(ttname & " is no more in use. A  new one will be published soon.", student, Session("staffid"), "Admin", "~/content/student/classdetails.aspx", "")
                        logify.Notifications(ttname & " for " & studentname(no) & " is no more in use. A  new one will be published soon.", par.getparent(student), Session("staffid"), "Admin", "~/content/parent/classdetails.aspx?" & student, "")
                        no = no + 1
                    Next

                    logify.log(Session("staffid"), ttname & " was deleted.")
                    Show_Alert(True, "Subject Removed. " & ttname & " was deleted and needs to be regenerated")
                Else
                    Show_Alert(True, "Subject Removed.")
                End If
                student0d.Close()
                Gridview1.EditIndex = -1
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, staffprofile.surname, classsubjects.periods from classsubjects left join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on subjects.id = classsubjects.subject where classsubjects.class = '" & get_id() & "'", con)
                Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Dim ds3 As New DataTable
                ds3.Columns.Add("S/N")
                ds3.Columns.Add("Subject")
                ds3.Columns.Add("name")
                ds3.Columns.Add("periods")
                ds3.Columns.Add("View")
                Dim j As Integer = 1
                Do While reader0.Read
                    ds3.Rows.Add(j.ToString & ".    ", reader0.Item(0), reader0.Item(1), reader0.Item(2), "Edit")
                    j = j + 1
                Loop
                GridView3.DataSource = ds3
                GridView3.DataBind()
                reader0.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkTimeTable_Click(sender As Object, e As EventArgs) Handles lnkTimeTable.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT ttname.id, class.class from ttname inner join class on class.id = ttname.class where ttname.default = '" & 1 & "' and class.id = ?", con)
                cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", get_id()))
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                If student.Read() Then
                    Session("classselect") = student(1).ToString
                    Dim clid As String = student(0).ToString
                    student.Close()
                    Session("clid") = get_id()
                    Response.Redirect("~/content/App/Admin/classtimetable.aspx?timetable=" & clid)
                Else
                    student.Close()
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ttname.id, class.class from ttname inner join (depts inner join class on class.superior = depts.id) on ttname.school = depts.id where ttname.default = '" & 1 & "' and class.id = ?", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", get_id()))
                    Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    If student1.Read() Then
                        Session("classselect") = student1(1).ToString
                        Dim clid As String = student1(0).ToString

                        student1.Close()
                        Session("clid") = get_id()
                        Response.Redirect("~/content/App/Admin/classtimetable.aspx?timetable=" & clid)

                    Else
                        Show_Alert(False, "Time Table not active yet")
                    End If
                End If
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkStudents_Click(sender As Object, e As EventArgs) Handles lnkStudents.Click
        Try
            Dim school As String = ""
            Dim clas As String = ""
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class, superior from class where class = '" & Session("tisclas") & "'", con)
                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                student1.Read()
                Session("studentAdd") = Nothing
                school = student1(1)
                clas = student1(0)
                student1.Close()
                con.Close()
            End Using
            Response.Redirect("~/content/App/Admin/classprofile.aspx?school=" & school & "&currentclass=" & clas)
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles Gridview1.SelectedIndexChanging
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim clas As String = Gridview1.Rows(e.NewSelectedIndex).Cells(1).Text
                Session("tisclas") = clas
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class as Class, studentsummary.classNo as 'No of Students', class.type as 'Type', depts.dept as 'Department' from class inner join depts on class.superior = depts.id left join studentsummary on class.id = studentsummary.class where class.class = '" & clas & "' order by studentsummary.session desc", con)
                Dim ds As New DataTable
                Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                adapter1.SelectCommand = cmdLoad1
                adapter1.Fill(ds)
                DetailsView1.DataSource = ds
                DetailsView1.DataBind()
                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from classteacher inner join class on class.id = classteacher.class inner join staffprofile on classteacher.teacher = staffprofile.staffid where class.class = '" & clas & "'", con)
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                Dim ds2 As New DataTable
                ds2.Columns.Add("S/N")
                ds2.Columns.Add("Name")
                Dim i As Integer = 1
                Do While reader.Read
                    ds2.Rows.Add(i.ToString & ".  ", reader.Item(0))
                    i = i + 1
                Loop
                GridView2.DataSource = ds2
                GridView2.DataBind()
                reader.Close()

                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, staffprofile.surname, classsubjects.periods from classsubjects inner join class on class.id = classsubjects.class left join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on subjects.id = classsubjects.subject where class.class = '" & clas & "'", con)
                Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Dim ds3 As New DataTable
                ds3.Columns.Add("S/N")
                ds3.Columns.Add("Subject")
                ds3.Columns.Add("name")
                ds3.Columns.Add("periods")
                ds3.Columns.Add("View")
                Dim j As Integer = 1
                Dim totalp As Integer
                Do While reader0.Read
                    ds3.Rows.Add(j.ToString & ".    ", reader0.Item(0), reader0.Item(1), reader0.Item(2), "Edit")
                    j = j + 1
                    totalp = totalp + Val(reader0(2))
                Loop

                GridView3.DataSource = ds3
                GridView3.DataBind()
                Dim roww As GridViewRow = GridView3.FooterRow
                Try
                    roww.Cells(1).Text = "Total Periods"
                    roww.Cells(3).Text = totalp
                Catch
                End Try
                reader0.Close()

                con.Close()            End Using
            panel1.Visible = True
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class, class.type, depts.dept, class.cano from class inner join depts on class.superior = depts.id left join studentsummary on class.id = studentsummary.class where class.class = '" & Session("tisclas") & "'", con)
            Dim reads As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            reads.Read()
            txtId.Text = reads(0).ToString
            cbotype.Text = reads(1).ToString
            cboDept.Text = reads(2).ToString
            cboCaNo.Text = reads(3).ToString
            reads.Close()
            pnlEditClass.Visible = True
            con.close()        End Using
    End Sub

    Protected Sub btnClassCancel_Click(sender As Object, e As EventArgs) Handles btnClassCancel.Click
        pnlEditClass.Visible = False
    End Sub

    Protected Sub btnClassUpdate_Click(sender As Object, e As EventArgs) Handles btnClassUpdate.Click
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If txtId.Text = "" Then
                    Show_Alert(False, "Please enter a class name")
                    Exit Sub
                End If
                If cbotype.Text = "Select Type" Then
                    Show_Alert(False, "Please select a class type")
                    Exit Sub
                End If
                Dim cmd5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From depts where dept = '" & cboDept.Text & "'", con)
                Dim reader5 As MySql.Data.MySqlClient.MySqlDataReader = cmd5.ExecuteReader
                If Not reader5.Read() Then
                    reader5.Close()
                    Show_Alert(False, "Please Select A Valid Department or create one.")
                    Exit Sub
                Else
                    Dim dept As Integer = reader5.Item(0)
                    reader5.Close()
                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Class set class = ?, type = ?, superior = ?, cano = ?  where class = '" & Session("tisclas") & "'", con)
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", txtId.Text))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cbotype.Text))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dept", dept))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dt", cboCaNo.Text))
                    cmd3.ExecuteNonQuery()
                    logify.log(Session("staffid"), txtId.Text & " class was updated")
                    pnlEditClass.Visible = False
                    Session("tisclas") = txtId.Text
                End If
                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("Class")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, class from class order by ID", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview1.DataSource = a

                Gridview1.DataBind()
                msg.Close()

                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class as Class, studentsummary.classNo as 'No of Students', class.type as 'Type', depts.dept as 'Department' from class inner join depts on class.superior = depts.id left join studentsummary on class.id = studentsummary.class where class.class = '" & Session("tisclas") & "' and studentsummary.session = '" & Session("sessionid") & "' order by studentsummary.session desc", con)
                Dim ds As New DataTable
                Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                adapter1.SelectCommand = cmdLoad1
                adapter1.Fill(ds)
                DetailsView1.DataSource = ds
                DetailsView1.DataBind()
                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from classteacher inner join class on class.id = classteacher.class inner join staffprofile on classteacher.teacher = staffprofile.staffid where class.class = '" & Session("tisclas") & "'", con)
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                Dim ds2 As New DataTable
                ds2.Columns.Add("S/N")
                ds2.Columns.Add("Name")
                Dim i As Integer = 1
                Do While reader.Read
                    ds2.Rows.Add(i.ToString & ".  ", reader.Item(0))
                    i = i + 1
                Loop
                GridView2.DataSource = ds2
                GridView2.DataBind()
                reader.Close()

                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, staffprofile.surname, classsubjects.periods from classsubjects inner join class on class.id = classsubjects.class left join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on subjects.id = classsubjects.subject where class.class = '" & Session("tisclas") & "'", con)
                Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Dim ds3 As New DataTable
                ds3.Columns.Add("S/N")
                ds3.Columns.Add("Subject")
                ds3.Columns.Add("name")
                ds3.Columns.Add("periods")
                ds3.Columns.Add("View")
                Dim j As Integer = 1
                Dim totalp As Integer
                Do While reader0.Read
                    ds3.Rows.Add(j.ToString & ".    ", reader0.Item(0), reader0.Item(1), reader0.Item(2), "Edit")
                    j = j + 1
                    totalp = totalp + Val(reader0(2))
                Loop

                GridView3.DataSource = ds3
                GridView3.DataBind()
                Dim roww As GridViewRow = GridView3.FooterRow

                roww.Cells(1).Text = "Total Periods"
                roww.Cells(3).Text = totalp

                reader0.Close()

                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
 