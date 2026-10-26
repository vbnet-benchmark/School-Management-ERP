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

    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim par As New parentcheck
    Dim alertPLC As New PlaceHolder
    Private Sub Show_Alert(type As Boolean, msg As String)
        alertPLC = Me.Master.FindControl("plcAlert")
        If type = True Then
            alert.Text = alertmsg.success_message(msg)
        Else
            alert.Text = alertmsg.error_message(msg)
        End If
        alertPLC.Controls.Add(alert)
    End Sub
    Sub dob_purge()

        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad03 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from studentsprofile", con)
            Dim student03 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad03.ExecuteReader
            Dim students As New ArrayList
            Dim dob As New ArrayList
            Do While student03.Read
                students.Add(student03(0))
                dob.Add(student03("dateofbirth"))
            Loop
            student03.Close()
            Dim count As Integer
            For Each student As String In students
                Dim a As Array = Split(dob(count), " ")
                Dim realdob2 As Array = Split(a(0), "-")
                Dim realdob As Array = Split(a(0), "/")
                Dim day As String
                Dim month As String
                Dim year As String
                If realdob2.Length <> 1 Then
                    day = realdob2(2)
                    month = realdob2(1)
                    year = realdob2(0)
                Else
                    day = realdob(0)
                    month = realdob(1)
                    year = realdob(2)
                End If
                Dim finaldob As String = IIf(day.Count <> 2, "0" & day, day) & "/" & IIf(month.Count <> 2, "0" & month, month) & "/" & year
                Dim cmdLoad0w3 As New MySql.Data.MySqlClient.MySqlCommand("update studentsprofile set dateofbirth = '" & finaldob & "' where admno = '" & student & "'", con)
                cmdLoad0w3.ExecuteNonQuery()
                count = count + 1
            Next
            con.close()
        End Using
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
         If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

        If IsPostBack Then
        Else
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdLoad03 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from hostel Order by Id", con)
                    Dim student03 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad03.ExecuteReader
                    cboHostel.Items.Clear()
                    cboHostel.Items.Add("Select Hostel")
                    Do While student03.Read
                        cboHostel.Items.Add(student03.Item(0).ToString)
                    Loop
                    student03.Close()

                    Dim cmdLoad01 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees Order by Id", con)
                    Dim student01 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad01.ExecuteReader
                    cboTransport.Items.Clear()
                    cboTransport.Items.Add("Select Route")
                    Do While student01.Read
                        cboTransport.Items.Add(student01.Item(0).ToString)
                    Loop
                    student01.Close()

                    Dim cmd40 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from options", con)
                    Dim reader40 As MySql.Data.MySqlClient.MySqlDataReader = cmd40.ExecuteReader
                    reader40.Read()
                    Dim board As Boolean = reader40.Item(0)
                    Dim trans As Boolean = reader40.Item(1)
                    reader40.Close()
                    If board = True Then
                        CheckBox1.Visible = True
                        cboHostel.Visible = True
                    End If
                    If trans = True Then
                        CheckBox2.Visible = True
                        cboTransport.Visible = True
                    End If
                    panel3.Visible = False

                    Dim ds As New DataTable
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    If DropDownList1.Text = "Active" Then
                        Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile where activated = '" & 1 & "'", con)
                        Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                        Do While student0.Read
                            ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                        Loop
                        student0.Close()
                    ElseIf DropDownList1.Text = "Inactive" Then
                        Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile where activated = '" & 0 & "'", con)
                        Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                        Do While student0.Read
                            ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                        Loop
                        student0.Close()
                    Else
                        Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile", con)
                        Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                        Do While student0.Read
                            ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                        Loop
                        student0.Close()
                    End If
                    gridview1.DataSource = ds
                    gridview1.DataBind()
                    con.close()                End Using
                If Session("studentadd") <> Nothing Then
                    Student_Details()
                Else
                    panel3.Visible = False
                    gridview1.SelectedIndex = -1
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Student_Details()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, activated, hostelstay, hostel, admfees, transport from StudentsProfile where admno = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
            Dim pass As String
            student.Read()
            pass = student.Item("passport").ToString
            If student.Item("activated") = True Then
                chkActivated.Checked = True
                chkActivated.Text = "Activated"
            Else
                chkActivated.Checked = False
                chkActivated.Text = "Deactivated"
            End If
            If student.Item("hostelstay") = True Then
                CheckBox1.Checked = True
                cboHostel.Enabled = True
                If student.Item("hostel").ToString <> "" Then
                    cboHostel.Text = student.Item("hostel").ToString
                End If
            Else
                CheckBox1.Checked = False
                cboHostel.Enabled = False
            End If
            cboAdm.Text = student.Item("admfees").ToString
            If student.Item("transport").ToString <> "" Then
                cboTransport.Text = student.Item("transport").ToString
                CheckBox2.Checked = True
                cboTransport.Enabled = True
            End If
            student.Close()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.admno as 'Admission number', studentsprofile.surname as Name, studentsprofile.Sex, studentsprofile.dateOfBirth as Birthday, studentsummary.age as 'Age', studentsprofile.phone as 'Phone number' from StudentsProfile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? order by studentsummary.session desc", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
            Dim ds As New DataTable
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1.SelectCommand = cmdLoad1
            adapter1.Fill(ds)

            DetailsView1.DataSource = ds
            DetailsView1.DataBind()
            Dim cmdLoad01 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.parentname as 'Parents/Guardians', parentprofile.phone as 'Parents Phone number', parentprofile.address as Address from StudentsProfile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? order by studentsummary.session desc", con)
            cmdLoad01.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            cmdLoad01.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
            Dim ds2 As New DataTable
            Dim adapter2 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter2.SelectCommand = cmdLoad01
            adapter2.Fill(ds2)

            DetailsView2.DataSource = ds2
            DetailsView2.DataBind()
            Dim cmdLoad1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT  class.class as Class, staffprofile.surname as 'Class Teacher' from StudentsProfile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
            cmdLoad1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            cmdLoad1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
            Dim dsa As New DataTable
            Dim adapter1a As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1a.SelectCommand = cmdLoad1a
            adapter1a.Fill(dsa)

            DetailsView3.DataSource = dsa
            DetailsView3.DataBind()
            If Not pass = "" Then Image1.ImageUrl = pass
            pnlAll.Visible = False
            panel3.Visible = True
            con.Close()
        End Using

    End Sub




    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Session("edit") = "studentprofile"
        Response.Redirect("~/content/Admin/addstudent.aspx")
    End Sub

    Protected Sub LinkButton4_Click(sender As Object, e As EventArgs) Handles LinkButton4.Click
        Session("edit") = "passport"
        Response.Redirect("~/content/Admin/addstudent.aspx")
    End Sub



    Protected Sub chkActivated_CheckedChanged(sender As Object, e As EventArgs) Handles chkActivated.CheckedChanged
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim active As Boolean = -Val(chkActivated.Checked)
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update StudentsProfile Set activated = ? Where admno = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("active", active))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", Session("studentadd")))
                cmdCheck3.ExecuteNonQuery()
                If active = False Then
                    Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student, age FROM StudentSummary WHERE student = ? And Session = ?", con)
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                    If reader2.Read() Then
                        Dim cla As Integer = reader2.Item(1).ToString
                        Dim age As String = reader2.Item(3).ToString
                        reader2.Close()
                       
                        Dim cmdSelect2a As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM StudentSummary WHERE student = ? And Session = ?", con)
                        cmdSelect2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdSelect2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdSelect2a.ExecuteNonQuery()

                        Dim cmdSelect2ax As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM kscoresheet WHERE student = ? And Session = ?", con)
                        cmdSelect2ax.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdSelect2ax.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdSelect2ax.ExecuteNonQuery()

                        Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("Delete from SubjectReg WHERE student = ? And Session = ?", con)
                        cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmd.ExecuteNonQuery()
                        Dim cmdSelect2s As New MySql.Data.MySqlClient.MySqlCommand("SELECT age FROM StudentSummary WHERE class = ?", con)
                        cmdSelect2s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", cla))
                        Dim reader2s As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2s.ExecuteReader
                        Dim average As Integer
                        Dim total As Integer
                        Dim count As Integer
                        Do While reader2s.Read()
                            total = total + reader2s.Item("age")
                            count = count + 1
                        Loop
                        If Not count = 0 Then
                            average = total \ count
                        End If
                        reader2s.Close()

                        Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update StudentSummary Set Aveage = ?, ClassNo = ? where class = ?", con)
                        cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", average))
                        cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", count))
                        cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", cla))
                        cmdInsert2.ExecuteNonQuery()

                        Dim cmdSelect2as As New MySql.Data.MySqlClient.MySqlCommand("SELECT smsno FROM Session WHERE id = ?", con)
                        cmdSelect2as.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionId")))
                        Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2as.ExecuteReader
                        reader2a.Read()
                        Dim smsno As Integer = reader2a(0)
                        reader2a.Close()
                        Dim cmdInsert2a As New MySql.Data.MySqlClient.MySqlCommand("Update session set smsno = ? where id = ?", con)
                        cmdInsert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("aveage", smsno - 35))
                        cmdInsert2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("No", Session("Sessionid")))
                        cmdInsert2a.ExecuteNonQuery()
                        Dim cmdckg As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set hostelstay = ? Where admno = ?", con)
                        cmdckg.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", 0))
                        cmdckg.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                        cmdckg.ExecuteNonQuery()

                        Dim cmdCheck3d As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set hostel = ?, transport = '" & "" & "' Where admno = ?", con)
                        cmdCheck3d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", ""))
                        cmdCheck3d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                        cmdCheck3d.ExecuteNonQuery()
                        Dim cmdSelect2q1 As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM discount WHERE student = ?", con)
                        cmdSelect2q1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdSelect2q1.ExecuteNonQuery()

                        Dim cmdSelect2sa As New MySql.Data.MySqlClient.MySqlCommand("SELECT amount, paid from feeschedule where student = '" & Session("studentadd") & "' and session = '" & Session("sessionid") & "'", con)
                        Dim reader2sa As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2sa.ExecuteReader
                        Dim paid As Integer
                        Dim amount As Integer
                        Dim owed As Integer
                        Do While reader2sa.Read
                            amount = amount + reader2sa.Item(0)
                            paid = paid + reader2sa.Item(1)
                        Loop
                        owed = amount - paid
                        reader2sa.Close()
                        Dim cmdSelect2q As New MySql.Data.MySqlClient.MySqlCommand("Delete FROM feeschedule WHERE student = ? And Session = ?", con)
                        cmdSelect2q.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdSelect2q.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdSelect2q.ExecuteNonQuery()
                        If owed <> 0 Then
                            Dim test2 As Boolean
                            Dim f As New Random
                            Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                            Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader

                            Dim refs2 As New ArrayList
                            Do While readref2.Read
                                refs2.Add(readref2.Item(0))
                            Loop
                            Dim d As Integer
                            Do Until test2 = True
                                d = f.Next(100000, 999999)
                                If refs2.Contains(d) Then
                                    test2 = False
                                Else
                                    test2 = True
                                End If
                            Loop
                            readref2.Close()

                            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(owed, , , TriState.True)))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Student withdrawal"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                            cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                            cmdCheck2.ExecuteNonQuery()

                            Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Expense"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(owed, , , TriState.True)))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BAD DEBTS"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Student withdrawal"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                            cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                            cmdCheck4.ExecuteNonQuery()
                        End If
                        logify.log(Session("staffid"), par.getstuname(Session("studentadd")) & " was deactivated")
                        Session("studentadd") = Nothing
                        panel3.Visible = False
                        Dim ds As New DataTable
                        ds.Columns.Add("passport")
                        ds.Columns.Add("staffname")
                        If DropDownList1.Text = "Active" Then
                            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile where activated = '" & 1 & "'", con)
                            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                            Do While student0.Read
                                ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                            Loop
                            student0.Close()
                        ElseIf DropDownList1.Text = "Inactive" Then
                            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile where activated = '" & 0 & "'", con)
                            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                            Do While student0.Read
                                ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                            Loop
                            student0.Close()
                        Else
                            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile", con)
                            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                            Do While student0.Read
                                ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                            Loop
                            student0.Close()
                        End If
                        gridview1.DataSource = ds
                        gridview1.DataBind()


                       

                    End If
            pnlAll.Visible = True
                Else
            logify.log(Session("staffid"), par.getstuname(Session("studentadd")) & " was activated")
            Exit Sub
                End If
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkmsg_Click(sender As Object, e As EventArgs) Handles lnkmsg.Click
        Session("receivert") = "Student"
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = ?", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                student.Read()
                Session("receiver") = student.Item(0).ToString
                student.Close()
                con.close()            End Using
            Response.Redirect("~/content/admin/personnewmg.aspx")
            Session("sendermsg") = Request.RawUrl
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub gridview1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gridview1.PageIndexChanging
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
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
        Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
        Session("studentadd") = RTrim(x(0))
        pnlAll.Visible = False
        gridview1.SelectedIndex = -1
        Student_Details()
    End Sub

    Protected Sub lnkPwd_Click(sender As Object, e As EventArgs) Handles lnkPwd.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update StudentsProfile Set password = ? where admno = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("password", "password"))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("Studentadd")))
                cmdCheck3.ExecuteNonQuery()
                Show_Alert(True, "Password reset successfully")

                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim s As String = "%" & txtSearch.Text & "%"
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile where surname like '" & s & "' or admno = '" & txtSearch.Text & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub cboHostel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboHostel.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If Not cboHostel.Text = "Select Hostel" Then
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & Session("studentadd") & "' or admno = '" & txtSearch.Text & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    student0.Read()
                    Dim stu As String = student0(0).ToString
                    student0.Close()
                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set hostel = ? Where admno = ?", con)
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", cboHostel.Text))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                    cmdCheck3.ExecuteNonQuery()
                    logify.log(Session("staffid"), stu & " was made a boarder in " & cboHostel.Text)
                    logify.Notifications("You are now a boarder in " & cboHostel.Text, Session("studentadd"), Session("staffid"), "Admin", "~/content/student/studentprofile.aspx", "")
                    If par.getparent(Session("studentadd")) <> "" Then logify.Notifications(stu & " is now a boarder in " & cboHostel.Text, par.getparent(Session("studentadd")), Session("staffid"), "Admin", "~/content/parent/studentprofile.aspx?" & Session("studentadd"), "")

                Else
                    Show_Alert(False, "Select a hostel")
                End If
                con.close()            End Using
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub cboAdm_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAdm.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set admfees = ? Where admno = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", cboAdm.Text))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                cmdCheck3.ExecuteNonQuery()

                Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Session, Class, student FROM StudentSummary WHERE student = ? And Session = ?", con)
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                reader2.Read()
                Dim cla As Integer = reader2.Item(1).ToString
                reader2.Close()
                Dim cmdLoad3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from onetimefees", con)
                Dim student3 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad3.ExecuteReader
                Dim admfee As New ArrayList
                Dim admamount As New ArrayList
                Dim admin As New ArrayList
                Dim admi As Integer
                Do While student3.Read

                    admfee.Add(student3.Item(1))
                    admamount.Add(student3.Item(2))
                    admin.Add(student3.Item("amount") * (student3.Item("min") / 100))
                Loop
                student3.Close()
                Dim cmdLoad3z As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classonefees where class = '" & cla & "'", con)
                Dim student3z As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad3z.ExecuteReader
                Do While student3z.Read
                    admfee.Add(student3z.Item(1))
                    admamount.Add(student3z.Item(2))
                    admin.Add(student3z.Item("amount") * (student3z.Item("min") / 100))
                Loop
                student3z.Close()
                If cboAdm.Text = "Not Paid" Then
                    For Each item As String In admfee
                        Dim test22 As Boolean
                        Dim f2 As New Random
                        Dim ref22 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                        Dim readref22 As MySql.Data.MySqlClient.MySqlDataReader = ref22.ExecuteReader

                        Dim refs22 As New ArrayList
                        Do While readref22.Read
                            refs22.Add(readref22.Item(0))
                        Loop
                        Dim d2 As Integer
                        Do Until test22 = True
                            d2 = f2.Next(100000, 999999)
                            If refs22.Contains(d2) Then
                                test22 = False
                            Else
                                test22 = True
                            End If
                        Loop
                        readref22.Close()
                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, compulsory, min) Values (?,?,?,?,?,?,?)", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", admamount(admi)))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("compulsory", 1))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", admin(admi)))
                        cmdInsert22.ExecuteNonQuery()

                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(admamount(admi), , , TriState.True)))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", item & " DEBTS"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                        cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdCheck2.ExecuteNonQuery()

                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values (?,?,?,?,?,?,?,?)", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d2))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(admamount(admi), , , TriState.True)))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdCheck4.ExecuteNonQuery()
                        admi = admi + 1
                    Next


                Else
                    Dim db As New DB_Interface
                    For Each item As String In admfee
                        Dim paid As Integer = db.Select_single("select paid from feeschedule where student = '" & Session("studentadd") & "' and fee = '" & item & "' and session = '" & Session("sessionid") & "'")
                        Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Delete from feeschedule where session = ? and fee = ? and student = ?", con)
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", item))
                        cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        cmdInsert22.ExecuteNonQuery()
                        Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ?", con)
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", item & "%"))
                        cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                        Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader

                        Dim refe As Integer
                        If readref220.Read() Then refe = readref220.Item(0).ToString
                        readref220.Close()
                        Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                        cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                        cmdCheck04.ExecuteNonQuery()

                        Dim test2 As Boolean
                        Dim f As New Random
                        Dim refs2 As ArrayList = db.Select_1D("Select ref from transactions")
                        Dim d As Integer
                        Do Until test2 = True
                            d = f.Next(100000, 999999)
                            If refs2.Contains(d) Then
                                test2 = False
                            Else
                                test2 = True
                            End If
                        Loop
                        If paid > 0 Then
                            db.Non_Query("Insert Into transactions (ref, type, cr, account, details, date, trtype, student) Values ('" & d & "','Liability','" & FormatNumber(paid, , , TriState.True) & "', 'ADVANCE FEE PAYMENT', 'Payment Advance' , '" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss") & "','Credit', '" & Session("studentadd") & "')")                            db.Non_Query("Insert Into transactions (ref, type, dr, account, details, date, trtype, student) Values ('" & d & "','Income','" & FormatNumber(paid, , , TriState.True) & "', '" & item & " PAID" & "', 'Temporary refresh' , '" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss") & "','Debit', '" & Session("studentadd") & "')")                        End If
                    Next
                End If

                con.close()            End Using
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Sub readd_traits()

        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdxx As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from StudentSummary WHERE Session = '" & Session("sessionid") & "' and student = '" & Session("studentadd") & "'", con)
            Dim student03x As MySql.Data.MySqlClient.MySqlDataReader = cmdxx.ExecuteReader
            Dim cla As Integer
            If student03x.Read() Then
                cla = student03x(0)
            End If
            student03x.Close()

            Dim cmdLoad03 As New MySql.Data.MySqlClient.MySqlCommand("SELECT traits.id from traits inner join class on class.traitgroup = traits.traitgroup where traits.used = '" & 1 & "' and class.id = '" & cla & "'", con)
            Dim student03 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad03.ExecuteReader
            Dim traits As New ArrayList
            Do While student03.Read()
                traits.Add(student03(0).ToString)
            Loop
            student03.Close()
            Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("delete from termtraits where student = '" & Session("studentadd") & "' and session = '" & Session("sessionid") & "'", con)
            cmdInsert22.ExecuteNonQuery()
            For Each trait As String In traits
                Dim cmdInsert22a As New MySql.Data.MySqlClient.MySqlCommand("insert into termtraits (session, student, trait) values ('" & Session("sessionid") & "', '" & Session("studentadd") & "', '" & trait & "')", con)
                cmdInsert22a.ExecuteNonQuery()
            Next

            con.close()        End Using

    End Sub

    Protected Sub CheckBox1_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox1.CheckedChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim board As Boolean = CheckBox1.Checked
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set hostelstay = ? Where admno = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", -Val(board)))
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                cmdCheck3.ExecuteNonQuery()

                Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsummary.Session, studentsummary.Class, studentsummary.student, studentsprofile.surname FROM StudentSummary inner join studentsprofile on studentsprofile.admno = studentsummary.student WHERE studentsummary.student = ? And studentsummary.Session = ?", con)
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                reader2.Read()
                Dim cla As Integer = reader2.Item(1).ToString
                Dim stu As String = reader2(3).ToString
                reader2.Close()
                If board = True Then
                    cboHostel.Enabled = True
                    Dim test24 As Boolean
                    Dim f4 As New Random
                    Dim ref24 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                    Dim readref24 As MySql.Data.MySqlClient.MySqlDataReader = ref24.ExecuteReader

                    Dim refs24 As New ArrayList
                    Do While readref24.Read
                        refs24.Add(readref24.Item(0))
                    Loop
                    Dim d4 As Integer
                    Do Until test24 = True
                        d4 = f4.Next(100000, 999999)
                        If refs24.Contains(d4) Then
                            test24 = False
                        Else
                            test24 = True
                        End If
                    Loop
                    readref24.Close()
                    Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from boarding", con)
                    Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                    student24.Read()

                    Dim sesamount As String = student24.Item(0)
                    Dim sesmin As Integer = (student24.Item("cost") * (student24.Item("min") / 100))
                    student24.Close()
                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "BOARDING"))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin))
                    cmdInsert22.ExecuteNonQuery()

                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "BOARDING DEBTS"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                    cmdCheck2.ExecuteNonQuery()
                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , TriState.True)))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                    cmdCheck4.ExecuteNonQuery()

                Else
                    cboHostel.Enabled = False
                    Dim cmdCheck32 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set hostel = ? Where admno = ?", con)
                    cmdCheck32.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", ""))
                    cmdCheck32.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                    cmdCheck32.ExecuteNonQuery()
                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Delete from feeschedule where session = ? and fee = ? and student = ?", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "BOARDING"))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdInsert22.ExecuteNonQuery()
                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", "BOARDING" & "%"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                    Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader
                    readref220.Read()
                    Dim refe As Integer = readref220.Item(0).ToString
                    readref220.Close()
                    Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                    cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                    cmdCheck04.ExecuteNonQuery()
                    logify.log(Session("staffid"), stu & " was removed as a boarder in " & cboHostel.Text)
                    logify.Notifications("You have been removed from being a boarder in " & cboHostel.Text, Session("studentadd"), Session("staffid"), "Admin", "~/content/student/studentprofile.aspx", "")
                    If par.getparent(Session("studentadd")) <> "" Then logify.Notifications(stu & " has been removed from being a boarder in " & cboHostel.Text, par.getparent(Session("studentadd")), Session("staffid"), "Admin", "~/content/parent/studentprofile.aspx?" & Session("studentadd"), "")
                End If
                con.close()            End Using
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub CheckBox2_CheckedChanged(sender As Object, e As EventArgs) Handles CheckBox2.CheckedChanged
        Try
            Dim trans As Boolean = CheckBox2.Checked
            If trans = True Then
                cboTransport.Enabled = True
                cboTransport.Text = "Select Route"
            Else

                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    cboTransport.Enabled = False
                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set transport = ? Where admno = ?", con)
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", ""))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                    cmdCheck3.ExecuteNonQuery()

                    Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsummary.Session, studentsummary.Class, studentsummary.student, studentsprofile.surname FROM StudentSummary inner join studentsprofile on studentsprofile.admno = studentsummary.student WHERE studentsummary.student = ? And studentsummary.Session = ?", con)
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                    reader2.Read()
                    Dim cla As Integer = reader2.Item(1).ToString
                    Dim stu As String = reader2(3).ToString
                    reader2.Close()


                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("Delete from feeschedule where session = ? and fee = ? and student = ?", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "TRANSPORT"))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdInsert22.ExecuteNonQuery()
                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", "TRANSPORT" & "%"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                    Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck4.ExecuteReader
                    readref220.Read()
                    Dim refe As Integer = readref220.Item(0).ToString
                    readref220.Close()
                    Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                    cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                    cmdCheck04.ExecuteNonQuery()
                    logify.log(Session("staffid"), stu & " was removed from transport")
                    logify.Notifications("You have been removed from transport", Session("studentadd"), Session("staffid"), "Admin", "~/content/student/studentprofile.aspx", "")
                    If par.getparent(Session("studentadd")) <> "" Then logify.Notifications(stu & " has been removed from transport", par.getparent(Session("studentadd")), Session("staffid"), "Admin", "~/content/parent/studentprofile.aspx?" & Session("studentadd"), "")
                    con.Close()
                End Using
            End If
          
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub cboTransport_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboTransport.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()




                If Not cboTransport.Text = "Select Route" Then
                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update studentsprofile Set transport = ? Where admno = ?", con)
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", cboTransport.Text))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", Session("studentadd")))
                    cmdCheck3.ExecuteNonQuery()

                    Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsummary.Session, studentsummary.Class, studentsummary.student, studentsprofile.surname FROM StudentSummary inner join studentsprofile on studentsprofile.admno = studentsummary.student WHERE studentsummary.student = ? And studentsummary.Session = ?", con)
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
                    reader2.Read()
                    Dim cla As Integer = reader2.Item(1).ToString
                    Dim stu As String = reader2(3).ToString
                    reader2.Close()


                    Dim cmdInsert222 As New MySql.Data.MySqlClient.MySqlCommand("Delete from feeschedule where session = ? and fee = ? and student = ?", con)
                    cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "TRANSPORT"))
                    cmdInsert222.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdInsert222.ExecuteNonQuery()
                    Dim cmdCheck44 As New MySql.Data.MySqlClient.MySqlCommand("select ref from transactions where account like ? and student = ? and session = ?", con)
                    cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", "TRANSPORT" & "%"))
                    cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdCheck44.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                    Dim readref220 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck44.ExecuteReader
                    Dim refe As Integer
                    If readref220.Read() Then
                        refe = readref220.Item(0).ToString
                    End If
                    readref220.Close()
                    Dim cmdCheck04 As New MySql.Data.MySqlClient.MySqlCommand("delete from transactions where ref = ?", con)
                    cmdCheck04.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("refa", refe))
                    cmdCheck04.ExecuteNonQuery()

                    Dim test24 As Boolean
                    Dim f4 As New Random
                    Dim ref24 As New MySql.Data.MySqlClient.MySqlCommand("Select ref from transactions", con)
                    Dim readref24 As MySql.Data.MySqlClient.MySqlDataReader = ref24.ExecuteReader

                    Dim refs24 As New ArrayList
                    Do While readref24.Read
                        refs24.Add(readref24.Item(0))
                    Loop
                    Dim d4 As Integer
                    Do Until test24 = True
                        d4 = f4.Next(100000, 999999)
                        If refs24.Contains(d4) Then
                            test24 = False
                        Else
                            test24 = True
                        End If
                    Loop
                    readref24.Close()
                    Dim cmdLoad24 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees where route = ?", con)
                    cmdLoad24.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", cboTransport.Text))
                    Dim student24 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad24.ExecuteReader
                    student24.Read()

                    Dim sesamount As String = student24.Item(1)
                    Dim sesmin As Integer = (student24.Item("amount") * (student24.Item("min") / 100))
                    student24.Close()
                    Dim cmdInsert22 As New MySql.Data.MySqlClient.MySqlCommand("INSERT INTO feeschedule (Session, fee, amount, student, class, min) Values (?,?,?,?,?,?)", con)
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "TRANSPORT"))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amt", sesamount))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                    cmdInsert22.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("min", sesmin))
                    cmdInsert22.ExecuteNonQuery()

                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, cr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Income"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cr", FormatNumber(sesamount, , , TriState.True)))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "TRANSPORT DEBTS"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees generated"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "credit"))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))
                    cmdCheck2.ExecuteNonQuery()
                    Dim cmdCheck4 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into transactions (ref, type, dr, account, details, date, trtype, student, session) Values (?,?,?,?,?,?,?,?,?)", con)
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", d4))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Receivable"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dr", FormatNumber(sesamount, , , TriState.True)))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("account", "DEBTORS"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("details", "Fees debts"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("trtype", "Debit"))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("studentadd")))
                    cmdCheck4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("sessionid")))

                    cmdCheck4.ExecuteNonQuery()
                    logify.log(Session("staffid"), stu & " was put on transport. route = " & cboTransport.Text)
                    logify.Notifications("You have been put from transport. Route = " & cboTransport.Text, Session("studentadd"), Session("staffid"), "Admin", "~/content/student/studentprofile.aspx", "")
                    If par.getparent(Session("studentadd")) <> "" Then logify.Notifications(stu & " has put from transport. Route = " & cboTransport.Text, par.getparent(Session("studentadd")), Session("staffid"), "Admin", "~/content/parent/studentprofile.aspx?" & Session("studentadd"), "")
                Else
                    Show_Alert(False, "Select a route")
                End If
                con.close()            End Using
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub LinkButton5_Click(sender As Object, e As EventArgs) Handles LinkButton5.Click
        Session("edit") = Nothing
        Session("studentadd") = Nothing
        Response.Redirect("~/content/Admin/addstudent.aspx")
    End Sub


    Protected Sub lnkClass_Click(sender As Object, e As EventArgs) Handles lnkClass.Click
        Response.Redirect("~/content/Admin/newclass.aspx")
    End Sub



    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                If DropDownList1.Text = "Active" Then
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile where activated = '" & 1 & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    Do While student0.Read
                        ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                    Loop
                    student0.Close()
                ElseIf DropDownList1.Text = "Inactive" Then
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile where activated = '" & 0 & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    Do While student0.Read
                        ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                    Loop
                    student0.Close()
                Else
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    Do While student0.Read
                        ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                    Loop
                    student0.Close()
                End If
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.close()            End Using
            Session("studentadd") = Nothing
            panel3.Visible = False
            gridview1.SelectedIndex = -1
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkParent_Click(sender As Object, e As EventArgs) Handles lnkParent.Click
        Session("edit") = "parent"
        Response.Redirect("~/content/Admin/addstudent.aspx")
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
    End Sub

    Protected Sub btnNext_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.close()            End Using
            gridview1.DataBind()
            Session("studentadd") = Nothing
            panel3.Visible = False
            gridview1.SelectedIndex = -1
           
            If gridview1.PageIndex = gridview1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If gridview1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If


            If gridview1.PageIndex + 1 <= gridview1.PageCount Then
                gridview1.PageIndex = gridview1.PageIndex + 1
                gridview1.DataBind()
                If gridview1.PageIndex = gridview1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If gridview1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, admno, surname from studentsprofile", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.close()            End Using
            gridview1.DataBind()
            Session("studentadd") = Nothing
            panel3.Visible = False
            gridview1.SelectedIndex = -1
            
            If gridview1.PageIndex = gridview1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If gridview1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If

           
            If gridview1.PageIndex - 1 >= 0 Then
                gridview1.PageIndex = gridview1.PageIndex - 1
                gridview1.DataBind()
                If gridview1.PageIndex = gridview1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If gridview1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub BtnTraits_Click(sender As Object, e As EventArgs) Handles BtnTraits.Click
        Try

            readd_traits()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
