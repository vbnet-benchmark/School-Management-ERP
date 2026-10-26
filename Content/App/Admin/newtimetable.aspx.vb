Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_addclass
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim sgroupsubjects As New ArrayList
    Dim nsgroupsubjects As New ArrayList
    Dim sgroupclasses As New ArrayList
    Dim nsgroupclasses As New ArrayList
    Dim cgroupsubjects As New ArrayList
    Dim ncgroupsubjects As New ArrayList
    Dim csgroupsubjects As New ArrayList
    Dim ncsgroupsubjects As New ArrayList
    Dim cgroupclasses As New ArrayList
    Dim ncgroupclasses As New ArrayList
    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
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
    Public con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            If txtCGrp.Text = "" Then
                Show_Alert(False, "Please enter a group name.")
                Exit Sub
            End If
            con.Open()
            For Each i As ListItem In chkCSgroup.Items

                If i.Selected = True Then
                    cgroupsubjects.Add(i.Text)
                Else
                    ncgroupsubjects.Add(i.Text)
                End If
            Next
            For Each i As ListItem In chkCSSgroup.Items

                If i.Selected = True Then
                    csgroupsubjects.Add(i.Text)
                Else
                    ncsgroupsubjects.Add(i.Text)
                End If
            Next
            For Each i As ListItem In chkCGroup.Items

                If i.Selected = True Then
                    cgroupclasses.Add(i.Text)
                Else
                    ncgroupclasses.Add(i.Text)
                End If
            Next

            If Session("updatecgroup") <> Nothing And Session("newcgroup") = Nothing Then
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Update tcgroups set name = ? Where name = ?", con)
                cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Su", txtCGrp.Text))
                cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.ject", Session("updatecgroup")))
                cmd2a.ExecuteNonQuery()
                logify.log(Session("staffid"), txtCGrp.Text & " class group was updated")
                Session("updatecgroup") = Nothing
                Dim sgroup As Integer
                Dim cmdf0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from tcgroups Where name = ?", con)
                cmdf0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", txtCGrp.Text))
                Dim readerf0 As MySql.Data.MySqlClient.MySqlDataReader = cmdf0.ExecuteReader
                readerf0.Read()
                sgroup = readerf0.Item(0)
                readerf0.Close()

                Dim isOfferred As Boolean = False
                Dim subId As Integer
                Dim cla As Integer
                For Each item As String In cgroupclasses
                    Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                    cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                    reader2a.Read()
                    cla = reader2a.Item(0)
                    reader2a.Close()
                    For Each subitem As String In cgroupsubjects
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        reader20.Read()
                        subId = reader20.Item(0)
                        reader20.Close()

                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set cgroup = ? where subject = ? and class = ?", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", sgroup))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                        cmd3.ExecuteNonQuery()
                    Next
                    For Each subitem As String In csgroupsubjects
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from tsgroups Where name = ?", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        reader20.Read()
                        subId = reader20.Item(0)
                        reader20.Close()

                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set cgroup = ? where sgroup = ? and class = ?", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", sgroup))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                        cmd3.ExecuteNonQuery()
                    Next

                Next


                For Each item As String In ncgroupclasses
                    Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                    cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                    reader2a.Read()
                    cla = reader2a.Item(0)
                    reader2a.Close()

                    For Each subitem As String In ncgroupsubjects
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        reader20.Read()
                        subId = reader20.Item(0)
                        reader20.Close()
                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set cgroup = ? where subject = ? and class = ?", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                        cmd3.ExecuteNonQuery()
                    Next
                    For Each subitem As String In ncsgroupsubjects
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from tsgroups Where name = ?", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        reader20.Read()
                        subId = reader20.Item(0)
                        reader20.Close()

                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set cgroup = ? where cgroup = ? and class = ?", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                        cmd3.ExecuteNonQuery()
                    Next

                Next
                For Each item As String In ncgroupsubjects
                    Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from subjects Where subject = ?", con)
                    cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                    reader2a.Read()
                    subId = reader2a.Item(0)
                    reader2a.Close()
                    For Each subitem As String In cgroupclasses
                        Dim cmd1as As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                        cmd1as.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                        Dim reader2as As MySql.Data.MySqlClient.MySqlDataReader = cmd1as.ExecuteReader
                        reader2as.Read()
                        cla = reader2as.Item(0)
                        reader2as.Close()
                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set cgroup = ? where class = ? and subject = ?", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("te", subId))
                        cmd3.ExecuteNonQuery()

                    Next
                    For Each subitem As String In ncsgroupsubjects
                        Dim cmd1ad As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from tsgroups Where name = ?", con)
                        cmd1ad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                        Dim reader2ad As MySql.Data.MySqlClient.MySqlDataReader = cmd1ad.ExecuteReader
                        reader2ad.Read()
                        subId = reader2ad.Item(0)
                        reader2ad.Close()
                        For Each ssubitem As String In cgroupclasses
                            Dim cmd1as As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                            cmd1as.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", ssubitem))
                            Dim reader2as As MySql.Data.MySqlClient.MySqlDataReader = cmd1as.ExecuteReader
                            reader2as.Read()
                            cla = reader2as.Item(0)
                            reader2as.Close()
                            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set cgroup = ? where class = ? and cgroup = ?", con)
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("te", subId))
                            cmd3.ExecuteNonQuery()
                        Next
                    Next

                Next
                Show_Alert(True, "Class Group updated successfully.")
            Else
                Dim cmdf4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from tcgroups Where name = ?", con)
                cmdf4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", txtCGrp.Text))
                Dim readerf4 As MySql.Data.MySqlClient.MySqlDataReader = cmdf4.ExecuteReader
                If readerf4.HasRows Then
                    Show_Alert(False, "Group exists. Use a different name")
                    Exit Sub
                End If
                readerf4.Close()
                Dim cmd3a As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into tcgroups (name) Values ('" & txtCGrp.Text & "')", con)
                cmd3a.ExecuteNonQuery()
                logify.log(Session("staffid"), txtCGrp.Text & " class group was created")
                Dim sgroup As Integer
                Dim cmdf0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from tcgroups Where name = ?", con)
                cmdf0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", txtCGrp.Text))
                Dim readerf0 As MySql.Data.MySqlClient.MySqlDataReader = cmdf0.ExecuteReader
                readerf0.Read()
                sgroup = readerf0.Item(0)
                readerf0.Close()
                Dim isOfferred As Boolean = False
                Dim subId As Integer
                Dim cla As Integer
                For Each item As String In cgroupclasses
                    Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                    cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                    reader2a.Read()
                    cla = reader2a.Item(0)
                    reader2a.Close()
                    For Each subitem As String In cgroupsubjects
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        reader20.Read()
                        subId = reader20.Item(0)
                        reader20.Close()

                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set cgroup = ? where subject = ? and class = ?", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", sgroup))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                        cmd3.ExecuteNonQuery()
                    Next
                    For Each subitem As String In csgroupsubjects
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from tsgroups Where name = ?", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        reader20.Read()
                        subId = reader20.Item(0)
                        reader20.Close()

                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set cgroup = ? where sgroup = ? and class = ?", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", sgroup))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                        cmd3.ExecuteNonQuery()
                    Next
                Next




                Show_Alert(True, "Class Group Added Successfully")
            End If
            Session("newcgroup") = Nothing
            Panel3.Visible = False
            Dim a As New DataTable
            a.Columns.Add("name")
            a.Columns.Add("id")

            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select name, id from tcgroups", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Do While msg.Read()
                a.Rows.Add(msg.Item(0), msg.Item(1))
            Loop
            Gridview3.DataSource = a

            Gridview3.DataBind()
            msg.Close()
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try
            If Not IsPostBack Then
                con.Open()

                Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From Subjects", con)
                Dim reader4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader
                CheckBoxList2.Items.Clear()
                cboSubject.Items.Clear()
                cboSubject.Items.Add("Select Subject")
                cboSubject2.Items.Add("Select Subject")

                Do While reader4.Read
                    CheckBoxList2.Items.Add(reader4.Item(1))
                    cboSubject.Items.Add(reader4.Item(1))
                    cboSubject2.Items.Add(reader4.Item(1))
                Loop
                reader4.Close()
                Dim mor As New DataTable
                mor.Columns.Add("name")
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject from tsubject inner join subjects on subjects.id = tsubject.subject", con)
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While student.Read
                    mor.Rows.Add(student.Item(0).ToString)
                Loop
                student.Close()
                gridMorning.DataSource = mor
                gridMorning.DataBind()
                Dim cmdLoad1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher", con)
                Dim studenta As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1a.ExecuteReader
                cboTeacher.Items.Clear()
                cboTeacher.Items.Add("Select Teacher")
                Dim teachers As New ArrayList
                Do While studenta.Read
                    If Not teachers.Contains(studenta.Item(0).ToString) Then
                        cboTeacher.Items.Add(studenta.Item(0).ToString)
                        teachers.Add(studenta.Item(0).ToString)
                    End If
                Loop
                studenta.Close()


                Dim cmd4a As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From class", con)
                Dim reader4a As MySql.Data.MySqlClient.MySqlDataReader = cmd4a.ExecuteReader
                chkClasses.Items.Clear()
                DropDownList1.Items.Clear()
                DropDownList1.Items.Add("SELECT CLASS")
                Do While reader4a.Read
                    Chkclasss.Items.Add(reader4a(1))
                    chkClasses.Items.Add(reader4a.Item(1))
                    chkCGroup.Items.Add(reader4a.Item(1))
                    DropDownList1.Items.Add(reader4a.Item(1))

                Loop
                reader4a.Close()

                Dim cmd5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT depts.dept From class inner join depts on depts.id = class.superior", con)
                Dim reader5 As MySql.Data.MySqlClient.MySqlDataReader = cmd5.ExecuteReader
                cboDept.Items.Clear()
                cboDept.Items.Add("Select School")
                Dim schools As New ArrayList
                Do While reader5.Read
                    If Not schools.Contains(reader5.Item(0)) Then
                        cboDept.Items.Add(reader5.Item(0))
                        schools.Add(reader5.Item(0))
                    End If
                Loop
                reader5.Close()
                If Request.QueryString("period") <> Nothing Then
                    panel1.Visible = True

                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From tperiods Where id = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subct", Request.QueryString("period")))
                    Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                    reader3.Read()
                    cboDay.Text = reader3.Item("day").ToString
                    txtStart.Text = reader3.Item("timestart").ToString
                    txtEnd.Text = reader3.Item("timeend").ToString
                    txtActivity.Text = reader3.Item("activity")
                    If txtActivity.Text = "Tutorial" Then
                        chkTutorial.Checked = True
                        txtActivity.Enabled = False
                    Else
                        chkTutorial.Checked = False
                        txtActivity.Enabled = True

                    End If
                    reader3.Close()
                    bnAdd.Text = "Update"
                    Wizard1.ActiveStepIndex = 3
                ElseIf Request.QueryString("id") <> Nothing Then
                    If Request.QueryString("class") <> 0 Then
                        Wizard1.ActiveStepIndex = 0
                        Dim cmdCheck2x As New MySql.Data.MySqlClient.MySqlCommand("Select ttname.name, class.class from ttname inner join class on class.id = ttname.class where ttname.id = '" & Request.QueryString("id") & "'", con)
                        Dim msgx As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2x.ExecuteReader
                        msgx.Read()
                        txtID.Text = msgx(0)

                        radType.SelectedValue = "Class Based"
                        DropDownList1.Text = msgx(1)
                        msgx.Close()
                        Dim cmdLoad0x As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject from classsubjects inner join class on class.id = classsubjects.class inner join subjects on subjects.id = classsubjects.subject where class.class = '" & DropDownList1.Text & "'", con)
                        Dim reader0x As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0x.ExecuteReader
                        cboManualSubjects.Items.Clear()
                        cboManualSubjects.Items.Add("SELECT SUBJECT")
                        Do While reader0x.Read
                            cboManualSubjects.Items.Add(reader0x(0))
                        Loop
                        reader0x.Close()

                        Dim cmdCheck2xz As New MySql.Data.MySqlClient.MySqlCommand("delete from timetable where tname = '" & Request.QueryString("id") & "'", con)
                        cmdCheck2xz.ExecuteNonQuery()

                        Dim cmdCheckb As New MySql.Data.MySqlClient.MySqlCommand("delete from tperiods where timetable = '" & Request.QueryString("id") & "'", con)
                        cmdCheckb.ExecuteNonQuery()
                        con.Close()
                        load_time_table()
                        pnlManual.Visible = True
                        pnlClass.Visible = True
                        DropDownList1.Enabled = False
                        Wizard1.StartNextButtonText = "FINISH"

                    Else
                        Wizard1.ActiveStepIndex = 1
                        Session("timetable") = Request.QueryString("id")
                        Dim a As New DataTable
                        a.Columns.Add("name")
                        a.Columns.Add("id")

                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select name, id from tsgroups", con)
                        Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                        Do While msg.Read()
                            a.Rows.Add(msg.Item(0), msg.Item(1))
                        Loop
                        Gridview2.DataSource = a

                        Gridview2.DataBind()
                        msg.Close()


                    End If




                ElseIf Request.QueryString("group") <> Nothing Then
                    Panel2.Visible = True
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From tsgroups Where id = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subct", Request.QueryString("group")))
                    Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                    reader3.Read()
                    txtGrp.Text = reader3.Item("name").ToString
                    reader3.Close()
                    Button1.Text = "Update"
                    Dim cmdf As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, class.class from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.id where classsubjects.sgroup = ?", con)
                    cmdf.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subct", Request.QueryString("group")))
                    Dim readerx As MySql.Data.MySqlClient.MySqlDataReader = cmdf.ExecuteReader
                    Do While readerx.Read
                        For Each item As ListItem In CheckBoxList2.Items
                            If item.Text = readerx.Item(0).ToString Then item.Selected = True
                        Next
                        For Each sitem As ListItem In chkClasses.Items
                            If sitem.Text = readerx.Item(1).ToString Then sitem.Selected = True
                        Next
                    Loop
                    readerx.Close()
                    Dim a As New DataTable
                    a.Columns.Add("name")
                    a.Columns.Add("id")

                    Dim cmdCheck22 As New MySql.Data.MySqlClient.MySqlCommand("Select name, id from tsgroups", con)
                    Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck22.ExecuteReader
                    Do While msg2.Read()
                        a.Rows.Add(msg2.Item(0), msg2.Item(1))
                    Loop
                    Gridview2.DataSource = a

                    Gridview2.DataBind()
                    msg2.Close()
                    Wizard1.ActiveStepIndex = 1
                ElseIf Request.QueryString("cgroup") <> Nothing Then
                    Panel3.Visible = True
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From tcgroups Where id = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subct", Request.QueryString("cgroup")))
                    Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                    reader3.Read()
                    txtCGrp.Text = reader3.Item("name").ToString
                    reader3.Close()
                    Button3.Text = "Update"
                    Dim cmdf As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, class.class from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.id where classsubjects.cgroup = ?", con)
                    cmdf.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subct", Request.QueryString("cgroup")))
                    Dim readerx As MySql.Data.MySqlClient.MySqlDataReader = cmdf.ExecuteReader
                    Do While readerx.Read
                        For Each item As ListItem In chkCGroup.Items
                            If item.Text = readerx.Item(1).ToString Then item.Selected = True
                        Next
                    Loop
                    readerx.Close()
                    con.Close()
                    Load_Together_Subjects()
                    Dim cmdfs As New MySql.Data.MySqlClient.MySqlCommand("SELECT tsgroups.name from classsubjects inner join tsgroups on tsgroups.id = classsubjects.sgroup where classsubjects.cgroup = ? and classsubjects.sgroup <> ?", con)
                    cmdfs.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subct", Request.QueryString("cgroup")))
                    cmdfs.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SutReg.Subct", ""))
                    Dim readery As MySql.Data.MySqlClient.MySqlDataReader = cmdfs.ExecuteReader
                    Do While readery.Read
                        For Each sitem As ListItem In chkCSSgroup.Items
                            If sitem.Text = readery.Item(0).ToString Then sitem.Selected = True
                        Next
                    Loop
                    readery.Close()

                    Dim cmdfss As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject from classsubjects inner join subjects on subjects.id = classsubjects.subject where classsubjects.cgroup = ? and classsubjects.sgroup = ?", con)
                    cmdfss.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subct", Request.QueryString("cgroup")))
                    cmdfss.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SutReg.Subct", ""))
                    Dim readersy As MySql.Data.MySqlClient.MySqlDataReader = cmdfss.ExecuteReader
                    Do While readersy.Read
                        For Each sitem As ListItem In chkCSgroup.Items
                            If sitem.Text = readersy.Item(0).ToString Then sitem.Selected = True
                        Next
                    Loop
                    readersy.Close()
                    chkCGroup.Enabled = False
                    Dim a As New DataTable
                    a.Columns.Add("name")
                    a.Columns.Add("id")

                    Dim cmdCheck21 As New MySql.Data.MySqlClient.MySqlCommand("Select name, id from tcgroups", con)
                    Dim msg1 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck21.ExecuteReader
                    Do While msg1.Read()
                        a.Rows.Add(msg1.Item(0), msg1.Item(1))
                    Loop
                    Gridview3.DataSource = a

                    Gridview3.DataBind()
                    msg1.Close()
                    Wizard1.ActiveStepIndex = 2
                ElseIf Request.QueryString("doubles") <> Nothing Then
                    Dim cmdCheck2s As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject, class.class, classsubjects.doubles, classsubjects.id from classsubjects inner join subjects on classsubjects.subject = subjects.id inner join class on classsubjects.class = class.id where classsubjects.id = '" & Request.QueryString("doubles") & "'", con)
                    Dim msgs As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2s.ExecuteReader
                    msgs.Read()
                    panel4.Visible = True
                    cboSubject.Text = msgs(0).ToString
                    cboClasses.Items.Add(msgs(1).ToString)
                    txtDouble.Text = msgs(2).ToString
                    cboSubject.Enabled = False
                    Button5.Text = "Update"
                    cboClasses.Enabled = False
                    Wizard1.ActiveStepIndex = 4
                    msgs.Close()
                    Dim a As New DataTable
                    a.Columns.Add("subject")
                    a.Columns.Add("class")
                    a.Columns.Add("amount")
                    a.Columns.Add("id")
                    Dim cmdCheck21 As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject, class.class, classsubjects.doubles, classsubjects.id from classsubjects inner join subjects on classsubjects.subject = subjects.id inner join class on classsubjects.class = class.id where classsubjects.doubles <> '" & 0 & "' order by classsubjects.doubles desc", con)
                    Dim msg1 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck21.ExecuteReader
                    Do While msg1.Read()
                        a.Rows.Add(msg1(0), msg1.Item(1).ToString, msg1.Item(2).ToString, msg1.Item(3))
                    Loop
                    Gridview4.DataSource = a

                    Gridview4.DataBind()
                    msg1.Close()

                End If


            Else
                For Each i As ListItem In CheckBoxList2.Items

                    If i.Selected = True Then
                        sgroupsubjects.Add(i.Text)
                    Else
                        nsgroupsubjects.Add(i.Text)
                    End If
                Next

                For Each i As ListItem In chkClasses.Items

                    If i.Selected = True Then
                        sgroupclasses.Add(i.Text)
                    Else
                        nsgroupclasses.Add(i.Text)
                    End If
                Next


            End If

            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Sub Each_Period(ByVal d As String, ByVal n As Integer, ByRef insertall As Boolean, ByVal classes As ArrayList, ByVal classesentered As ArrayList, ByVal isdouble As Integer, ByRef periodbeforebreaks As ArrayList)

        For Each cl As Integer In classes
            Dim cmdInsert6 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id FROM tperiods Where Day = ? and activity = ? and timetable = '" & Session("timetable") & "' order by timestart asc", con)
            cmdInsert6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
            cmdInsert6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Dy", "Tutorial"))
            Dim reader6 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert6.ExecuteReader
            Dim countp As Integer = 0
            Dim classperiod As Integer = 0
            Do While reader6.Read
                countp = countp + 1
                If countp = n Then
                    classperiod = reader6(0).ToString
                End If
            Loop
            reader6.Close()
            Dim cmd2ax As New MySql.Data.MySqlClient.MySqlCommand("select * from classperiods where period = '" & classperiod & "' and class = '" & cl & "'", con)
            Dim rrrx As MySql.Data.MySqlClient.MySqlDataReader = cmd2ax.ExecuteReader
            Do While rrrx.Read()
                classesentered.Add(cl)
            Loop
            rrrx.Close()
            If classesentered.Contains(cl) Then Continue For
            Dim CS As Integer = 0
            Dim cmdInsert7 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.subject FROM tsubject inner join classsubjects on classsubjects.subject = tsubject.subject Where Classsubjects.class = ? order by tsubject.id", con)
            cmdInsert7.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
            Dim reader7 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert7.ExecuteReader
            Dim ClassSubjects As New ArrayList
            Dim subjectNo As Integer = 0
            Do While reader7.Read
                ClassSubjects.Add(reader7.Item("subject"))
                subjectNo = subjectNo + 1
            Loop
            reader7.Close()
            Dim cmdInsert5t As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM classsubjects Where Class = ? order by sgroup desc, periods desc", con)
            cmdInsert5t.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
            Dim reader5t As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert5t.ExecuteReader
            Do While reader5t.Read
                If Not ClassSubjects.Contains(reader5t.Item("subject")) Then
                    ClassSubjects.Add(reader5t.Item("subject"))
                    subjectNo = subjectNo + 1
                End If
            Loop
            reader5t.Close()

            If n > 3 Then
                Dim max_index As Integer = ClassSubjects.Count - 1
                For i As Integer = 0 To max_index
                    Dim rnd As New Random
                    ' Pick an item for position i.

                    Dim j As Integer = rnd.Next(i, max_index)
                    ' Swap them.
                    Dim temp As Integer = ClassSubjects(i)
                    ClassSubjects(i) = ClassSubjects(j)
                    ClassSubjects(j) = temp
                    rnd = Nothing
                Next i
            End If




            Dim t As Integer = 0
            Dim doublePeriod2 As Integer = 0
            Dim todaysDouble As Boolean = False
            Dim doublePeriod As Integer = 0
            Dim doubleSubject As Integer
            Dim cmdInsert8 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And Class = ? And Period = ? and tname = '" & Session("timetable") & "'", con)
            cmdInsert8.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
            cmdInsert8.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
            cmdInsert8.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", n - 1))
            Dim reader8 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert8.ExecuteReader
            If reader8.Read Then
                doublePeriod = reader8.Item("Doubles")

                If doublePeriod = 0 Then
                    doubleSubject = reader8.Item("Subject")
                    reader8.Close()
                    Dim cmdInsert9 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM classsubjects Where Class = ? And Subject = ?", con)
                    cmdInsert9.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                    cmdInsert9.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", doubleSubject))
                    Dim reader9 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert9.ExecuteReader
                    reader9.Read()
                    doublePeriod2 = reader9.Item("Doubles")
                    reader9.Close()
                    If doublePeriod2 <> 0 Then

                        Dim cmdInsert440 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Class = ? And Subject = ? And Doubles = ? and tname = '" & Session("timetable") & "'", con)
                        cmdInsert440.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                        cmdInsert440.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", doubleSubject))
                        cmdInsert440.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Doubles", 1))
                        Dim reader440 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert440.ExecuteReader
                        Dim doubledays As New ArrayList
                        Dim counts As Integer = 0
                        Do While reader440.Read()
                            doubledays.Add(reader440.Item("Day"))
                            counts = counts + 1
                        Loop
                        reader440.Close()
                        For Each g As String In doubledays
                            If g = d Then
                                todaysDouble = True
                                Exit For
                            End If
                        Next
                        If doubledays.Count < doublePeriod2 And todaysDouble = False And Not periodbeforebreaks.Contains(n) Then
                            Dim temp2 As Integer = ClassSubjects(0)
                            t = ClassSubjects.IndexOf(doubleSubject)
                            ClassSubjects(0) = doubleSubject
                            ClassSubjects(t) = temp2
                            isdouble = 1
                            doubleSubject = Nothing
                        End If
                        counts = Nothing

                    End If
                Else
                    reader8.Close()
                End If
            Else
                reader8.Close()
            End If
            Dim failedgroups As New ArrayList
            For Each m As Integer In ClassSubjects
                Dim nextsubject As Boolean = False
                Dim groupedsubjects As New ArrayList
                Dim groupedclasses As New ArrayList

                Dim cmdInsert122 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM classsubjects Where Subject = ? And sgroup <> ? and class = ?", con)
                cmdInsert122.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", m))
                cmdInsert122.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", 0))
                cmdInsert122.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", cl))
                Dim reader122 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert122.ExecuteReader
                Dim sgroup As Integer = 0
                If reader122.Read Then
                    sgroup = reader122.Item("sgroup")
                End If
                reader122.Close()
                If failedgroups.Contains(sgroup) Then
                    isdouble = Nothing
                    Continue For
                End If


                If sgroup <> 0 Then
                    Dim cmdInsert1ll As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM classsubjects Where sgroup = ? and class = ?", con)
                    cmdInsert1ll.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", sgroup))
                    cmdInsert1ll.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", cl))
                    Dim reader1ll As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert1ll.ExecuteReader
                    Do While reader1ll.Read
                        groupedsubjects.Add(reader1ll.Item("subject"))
                    Loop
                    reader1ll.Close()
                End If
                If groupedsubjects.Count = 0 Then
                    groupedsubjects.Add(m)
                End If

                Dim cmdInsert4ll As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM classsubjects Where cgroup <> ? and class = ? and subject = ?", con)
                cmdInsert4ll.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", 0))
                cmdInsert4ll.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", cl))
                cmdInsert4ll.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Peod", m))

                Dim reader4ll As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert4ll.ExecuteReader
                Dim cgroup As Integer = 0
                If reader4ll.Read Then
                    cgroup = reader4ll.Item("cgroup")
                End If
                reader4ll.Close()
                If cgroup <> 0 Then
                    Dim cmdInsertfll As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM classsubjects Where cgroup = ?", con)
                    cmdInsertfll.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", cgroup))
                    Dim readerfll As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertfll.ExecuteReader
                    Do While readerfll.Read
                        groupedclasses.Add(readerfll.Item("class"))
                    Loop
                    readerfll.Close()
                End If
                If groupedclasses.Count = 0 Then groupedclasses.Add(cl)
                Dim teacher As New ArrayList

                Dim count As Integer = 0
                For Each s As Integer In groupedsubjects
                    Dim cmdInsert88 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.subject FROM texemptions inner join classsubjects on classsubjects.teacher = texemptions.teacher Where texemptions.day = ? And Classsubjects.class = ? and texemptions.timetable = '" & Session("timetable") & "'", con)
                    cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                    cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                    Dim reader88 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert88.ExecuteReader
                    Dim exceptSubject As Integer = 0
                    Do While reader88.Read
                        exceptSubject = reader88.Item(0)
                        If exceptSubject = s Then
                            reader88.Close()
                            If sgroup <> 0 Then failedgroups.Add(sgroup)
                            isdouble = Nothing
                            nextsubject = True
                            Exit For

                        End If
                    Loop
                    reader88.Close()

                    Dim cmdInsert445 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And Class = ? and subject = '" & s & "' and tname = '" & Session("timetable") & "'", con)
                    cmdInsert445.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                    cmdInsert445.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                    Dim reader445 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert445.ExecuteReader
                    Do While reader445.Read()
                        If isdouble = 0 Then
                            nextsubject = True
                            isdouble = Nothing
                            reader445.Close()
                            Exit For
                        End If
                    Loop
                    reader445.Close()


                    Dim cmdInsert20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.teacher, classsubjects.periods, subjects.subject, class.class FROM classsubjects inner join subjects on subjects.id = classsubjects.subject inner join class on class.id = classsubjects.class WHERE Classsubjects.class = ? And classsubjects.Subject = ?", con)
                    cmdInsert20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                    cmdInsert20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", s))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert20.ExecuteReader

                    Dim subAllocate As Integer
                    reader20.Read()
                    If reader20.Item(0).ToString = "" Then
                        Show_Alert(False, "Teacher not assigned to " & reader20.Item(2) & " - " & reader20.Item(3) & ".")
                        Exit Sub
                    End If
                    teacher.Add(reader20.Item(0).ToString)
                    subAllocate = reader20.Item(1)
                    reader20.Close()

                   
                    Dim cmdInsert13 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Class = ? And Subject = ? and tname = '" & Session("timetable") & "'", con)
                    cmdInsert13.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                    cmdInsert13.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", s))
                    Dim reader13 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert13.ExecuteReader
                    Dim sAllocation As Integer = 0
                    Do While reader13.Read()
                        sAllocation = sAllocation + 1
                    Loop
                    reader13.Close()
                    If sAllocation = subAllocate Then
                        isdouble = Nothing
                        sAllocation = Nothing
                        nextsubject = True
                        Exit For
                    End If
                    sAllocation = Nothing
                    Dim cmdInsert12 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And Period = ? And Teacher = ? and tname = ?", con)
                    cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                    cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", n))
                    cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", teacher(count)))
                    cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Session("timetable")))
                    Dim reader12 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert12.ExecuteReader
                    If reader12.Read Then
                        isdouble = Nothing
                        nextsubject = True
                    End If
                    reader12.Close()
                    If nextsubject = True Then Exit For
                    count = count + 1
                Next
                For Each ct As Integer In groupedclasses
                    If nextsubject = True Then Exit For
                    Dim cmdInsertb6 As New MySql.Data.MySqlClient.MySqlCommand("Delete from timetable where class = ? and period = ? and day = ? and tname = ?", con)
                    cmdInsertb6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", ct))
                    cmdInsertb6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", n))
                    cmdInsertb6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                    cmdInsertb6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("y", Session("timetable")))

                    cmdInsertb6.ExecuteNonQuery()
                    classesentered.Remove(ct)
                    Dim v As Integer = 0
                    Dim grouped As Integer = 0
                    For Each s As Integer In groupedsubjects
                        If groupedsubjects.Count > 1 Then grouped = 1
                        Dim cmdInsert16 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into timetable (tname, Class, Period, Subject, Doubles, Day, Teacher, grouped) Values(?,?,?,?,?,?,?,?)", con)
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Session("timetable")))
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", ct))
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", n))
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", s))
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Doubles", isdouble))
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", teacher(v)))
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Te", grouped))
                        cmdInsert16.ExecuteNonQuery()
                        v = v + 1
                        If isdouble = 1 Then
                            Dim cmdInsert16f As New MySql.Data.MySqlClient.MySqlCommand("Update timetable set doubles = ? where tname = ? and day = ? and class = ? and subject = ?", con)
                            cmdInsert16f.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ssdd", 1))
                            cmdInsert16f.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Session("timetable")))
                            cmdInsert16f.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sss", d))
                            cmdInsert16f.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", ct))
                            cmdInsert16f.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", s))
                            cmdInsert16f.ExecuteNonQuery()
                        End If
                    Next
                    v = Nothing
                    If Not classesentered.Contains(ct) Then classesentered.Add(ct)
                Next
                groupedclasses = Nothing
                groupedsubjects = Nothing
                sgroup = Nothing
                cgroup = Nothing
                teacher = Nothing
                isdouble = Nothing

                If nextsubject = True Then
                    nextsubject = Nothing
                    Continue For
                End If

                Exit For
            Next
            failedgroups = Nothing
            ClassSubjects = Nothing
            Dim notall As Boolean = False

        Next
        For Each sc As Integer In classes
            If Not classesentered.Contains(sc) Then

                insertall = False
                Dim cmdInsertb9 As New MySql.Data.MySqlClient.MySqlCommand("Delete from timetable where period = ? and day = ? and tname = ?", con)
                cmdInsertb9.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", n))
                cmdInsertb9.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                cmdInsertb9.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("y", Session("timetable")))
                cmdInsertb9.ExecuteNonQuery()
                classesentered.Clear()
                Exit Sub
            Else


            End If
        Next
        insertall = True
        classesentered.Clear()
        Exit Sub
    End Sub

    Sub swap_subjects(ByVal d As String, ByVal n As Integer, ByRef insertall As Boolean, ByVal classes As ArrayList, ByVal classesentered As ArrayList, ByVal isdouble As Integer, ByRef periodbeforebreaks As ArrayList)
        For Each cl As Integer In classes
            Dim cmdInsert6 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id FROM tperiods Where Day = ? and activity = ? and timetable = '" & Session("timetable") & "' order by timestart asc", con)
            cmdInsert6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
            cmdInsert6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Dy", "Tutorial"))
            Dim reader6 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert6.ExecuteReader
            Dim countp As Integer = 0
            Dim classperiod As Integer = 0
            Do While reader6.Read
                countp = countp + 1
                If countp = n Then
                    classperiod = reader6(0).ToString
                End If
            Loop
            reader6.Close()
            Dim cmd2ax As New MySql.Data.MySqlClient.MySqlCommand("select * from classperiods where period = '" & classperiod & "' and class = '" & cl & "'", con)
            Dim rrrx As MySql.Data.MySqlClient.MySqlDataReader = cmd2ax.ExecuteReader
            Do While rrrx.Read()
                classesentered.Add(cl)
            Loop
            rrrx.Close()

            If classesentered.Contains(cl) Then Continue For

            Dim CS As Integer = 0
            Dim cmdInsert7 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.subject FROM tsubject inner join classsubjects on classsubjects.subject = tsubject.subject Where Classsubjects.class = ? order by tsubject.id", con)
            cmdInsert7.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
            Dim reader7 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert7.ExecuteReader
            Dim ClassSubjects As New ArrayList
            Dim subjectNo As Integer = 0
            Do While reader7.Read
                ClassSubjects.Add(reader7.Item("subject"))
                subjectNo = subjectNo + 1
            Loop
            reader7.Close()
            Dim cmdInsert5t As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM classsubjects Where Class = ? order by sgroup desc, periods desc", con)
            cmdInsert5t.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
            Dim reader5t As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert5t.ExecuteReader
            Do While reader5t.Read
                If Not ClassSubjects.Contains(reader5t.Item("subject")) Then
                    ClassSubjects.Add(reader5t.Item("subject"))
                    subjectNo = subjectNo + 1
                End If
            Loop
            reader5t.Close()

            If n > 3 Then
                Dim max_index As Integer = ClassSubjects.Count - 1
                For i As Integer = 0 To max_index
                    Dim rnd As New Random
                    ' Pick an item for position i.

                    Dim j As Integer = rnd.Next(i, max_index)
                    ' Swap them.
                    Dim temp As Integer = ClassSubjects(i)
                    ClassSubjects(i) = ClassSubjects(j)
                    ClassSubjects(j) = temp
                    rnd = Nothing
                Next i
            End If




            Dim t As Integer = 0
            Dim doublePeriod2 As Integer = 0
            Dim todaysDouble As Boolean = False
            Dim doublePeriod As Integer = 0
            Dim doubleSubject As Integer
            Dim cmdInsert8 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And Class = ? And Period = ? and tname = '" & Session("timetable") & "'", con)
            cmdInsert8.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
            cmdInsert8.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
            cmdInsert8.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", n - 1))
            Dim reader8 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert8.ExecuteReader
            If reader8.Read Then
                doublePeriod = reader8.Item("Doubles")

                If doublePeriod = 0 Then
                    doubleSubject = reader8.Item("Subject")
                    reader8.Close()
                    Dim cmdInsert9 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM classsubjects Where Class = ? And Subject = ?", con)
                    cmdInsert9.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                    cmdInsert9.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", doubleSubject))
                    Dim reader9 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert9.ExecuteReader
                    reader9.Read()
                    doublePeriod2 = reader9.Item("Doubles")
                    reader9.Close()
                    If doublePeriod2 <> 0 Then

                        Dim cmdInsert440 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Class = ? And Subject = ? And Doubles = ? and tname = '" & Session("timetable") & "'", con)
                        cmdInsert440.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                        cmdInsert440.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", doubleSubject))
                        cmdInsert440.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Doubles", 1))
                        Dim reader440 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert440.ExecuteReader
                        Dim doubledays As New ArrayList
                        Dim counts As Integer = 0
                        Do While reader440.Read()
                            doubledays.Add(reader440.Item("Day"))
                            counts = counts + 1
                        Loop
                        reader440.Close()
                        For Each g As String In doubledays
                            If g = d Then
                                todaysDouble = True
                                Exit For
                            End If
                        Next
                        If doubledays.Count < doublePeriod2 And todaysDouble = False And Not periodbeforebreaks.Contains(n) Then
                            Dim temp2 As Integer = ClassSubjects(0)
                            t = ClassSubjects.IndexOf(doubleSubject)
                            ClassSubjects(0) = doubleSubject
                            ClassSubjects(t) = temp2
                            isdouble = 1
                            doubleSubject = Nothing
                        End If
                        counts = Nothing

                    End If
                Else
                    reader8.Close()
                End If
            Else
                reader8.Close()
            End If
            Dim failedgroups As New ArrayList
            For Each m As Integer In ClassSubjects
                Dim nextsubject As Boolean = False
                Dim groupedsubjects As New ArrayList
                Dim groupedclasses As New ArrayList

                Dim cmdInsert122 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM classsubjects Where Subject = ? And sgroup <> ? and class = ?", con)
                cmdInsert122.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", m))
                cmdInsert122.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", 0))
                cmdInsert122.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", cl))
                Dim reader122 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert122.ExecuteReader
                Dim sgroup As Integer = 0
                If reader122.Read Then
                    sgroup = reader122.Item("sgroup")
                End If
                reader122.Close()
                If failedgroups.Contains(sgroup) Then
                    isdouble = Nothing
                    Continue For
                End If


                If sgroup <> 0 Then
                    Dim cmdInsert1ll As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM classsubjects Where sgroup = ? and class = ?", con)
                    cmdInsert1ll.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", sgroup))
                    cmdInsert1ll.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", cl))
                    Dim reader1ll As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert1ll.ExecuteReader
                    Do While reader1ll.Read
                        groupedsubjects.Add(reader1ll.Item("subject"))
                    Loop
                    reader1ll.Close()
                End If
                If groupedsubjects.Count = 0 Then
                    groupedsubjects.Add(m)
                End If

                Dim cmdInsert4ll As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM classsubjects Where cgroup <> ? and class = ? and subject = ?", con)
                cmdInsert4ll.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", 0))
                cmdInsert4ll.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", cl))
                cmdInsert4ll.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Peod", m))

                Dim reader4ll As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert4ll.ExecuteReader
                Dim cgroup As Integer = 0
                If reader4ll.Read Then
                    cgroup = reader4ll.Item("cgroup")
                End If
                reader4ll.Close()
                If cgroup <> 0 Then
                    Dim cmdInsertfll As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM classsubjects Where cgroup = ?", con)
                    cmdInsertfll.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", cgroup))
                    Dim readerfll As MySql.Data.MySqlClient.MySqlDataReader = cmdInsertfll.ExecuteReader
                    Do While readerfll.Read
                        groupedclasses.Add(readerfll.Item("class"))
                    Loop
                    readerfll.Close()
                End If
                If groupedclasses.Count = 0 Then groupedclasses.Add(cl)
                Dim teacher As New ArrayList

                Dim count As Integer = 0
                For Each s As Integer In groupedsubjects
                    Dim cmdInsert88 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.subject FROM texemptions inner join classsubjects on classsubjects.teacher = texemptions.teacher Where texemptions.day = ? And Classsubjects.class = ? and classsubjects.subject = '" & s & "' and texemptions.timetable = '" & Session("timetable") & "'", con)
                    cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                    cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                    Dim reader88 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert88.ExecuteReader
                    If reader88.Read Then
                        reader88.Close()
                        If sgroup <> 0 Then failedgroups.Add(sgroup)
                        isdouble = Nothing
                        nextsubject = True
                        Exit For

                    End If
                    reader88.Close()

                    Dim cmdInsert445 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And Class = ? and subject = '" & s & "' and tname = '" & Session("timetable") & "'", con)
                    cmdInsert445.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                    cmdInsert445.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                    Dim reader445 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert445.ExecuteReader
                    Do While reader445.Read()
                        If isdouble = 0 Then
                            nextsubject = True
                            isdouble = Nothing
                            reader445.Close()
                            Exit For
                        End If
                    Loop
                    reader445.Close()


                    Dim cmdInsert20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.teacher, classsubjects.periods, subjects.subject, class.class FROM classsubjects inner join subjects on subjects.id = classsubjects.subject inner join class on class.id = classsubjects.class WHERE Classsubjects.class = ? And classsubjects.Subject = ?", con)
                    cmdInsert20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                    cmdInsert20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", s))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert20.ExecuteReader

                    Dim subAllocate As Integer
                    reader20.Read()
                    If reader20.Item(0).ToString = "" Then
                        Show_Alert(False, "Teacher not assigned to " & reader20.Item(2) & " - " & reader20.Item(3) & ".")
                        Exit Sub
                    End If
                    teacher.Add(reader20.Item(0).ToString)
                    subAllocate = reader20.Item(1)
                    reader20.Close()

                    
                    Dim cmdInsert13 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Class = ? And Subject = ? and tname = '" & Session("timetable") & "'", con)
                    cmdInsert13.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cl))
                    cmdInsert13.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", s))
                    Dim reader13 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert13.ExecuteReader
                    Dim sAllocation As Integer = 0
                    Do While reader13.Read()
                        sAllocation = sAllocation + 1
                    Loop
                    reader13.Close()
                    If sAllocation = subAllocate Then
                        isdouble = Nothing
                        sAllocation = Nothing
                        nextsubject = True
                        Exit For
                    End If
                    sAllocation = Nothing
                    Dim cmdInsert12 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And Period = ? And Teacher = ? and tname = ?", con)
                    cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                    cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", n))
                    cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", teacher(count)))
                    cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Session("timetable")))
                    Dim reader12 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert12.ExecuteReader
                    If reader12.Read Then
                        isdouble = Nothing
                        nextsubject = True
                    End If
                    reader12.Close()
                    If nextsubject = True Then Exit For
                    count = count + 1
                Next
                For Each ct As Integer In groupedclasses
                    If nextsubject = True Then Exit For
                    Dim cmdInsertb6 As New MySql.Data.MySqlClient.MySqlCommand("Delete from timetable where class = ? and period = ? and day = ? and tname = ?", con)
                    cmdInsertb6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", ct))
                    cmdInsertb6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", n))
                    cmdInsertb6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                    cmdInsertb6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("y", Session("timetable")))

                    cmdInsertb6.ExecuteNonQuery()
                    classesentered.Remove(ct)
                    Dim v As Integer = 0
                    Dim grouped As Integer = 0
                    For Each s As Integer In groupedsubjects
                        If groupedsubjects.Count > 1 Then grouped = 1
                        Dim cmdInsert16 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into timetable (tname, Class, Period, Subject, Doubles, Day, Teacher, grouped) Values(?,?,?,?,?,?,?,?)", con)
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Session("timetable")))
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", ct))
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", n))
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", s))
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Doubles", isdouble))
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", teacher(v)))
                        cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Te", grouped))
                        cmdInsert16.ExecuteNonQuery()
                        v = v + 1
                        If isdouble = 1 Then
                            Dim cmdInsert16f As New MySql.Data.MySqlClient.MySqlCommand("Update timetable set doubles = ? where tname = ? and day = ? and class = ? and subject = ?", con)
                            cmdInsert16f.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ssdd", 1))
                            cmdInsert16f.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Session("timetable")))
                            cmdInsert16f.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sss", d))
                            cmdInsert16f.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", ct))
                            cmdInsert16f.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", s))
                            cmdInsert16f.ExecuteNonQuery()
                        End If
                    Next
                    v = Nothing
                    If Not classesentered.Contains(ct) Then classesentered.Add(ct)
                Next
                groupedclasses = Nothing
                groupedsubjects = Nothing
                sgroup = Nothing
                cgroup = Nothing
                teacher = Nothing
                isdouble = Nothing

                If nextsubject = True Then
                    nextsubject = Nothing
                    Continue For
                End If

                Exit For
            Next

            failedgroups = Nothing
            





            ClassSubjects = Nothing
        Next
        Dim problemclasses As New ArrayList
        Dim notall As Boolean = False

        For Each sc As Integer In classes
            If Not classesentered.Contains(sc) Then
                problemclasses.Add(sc)
            End If
        Next
        If problemclasses.Count = 0 Then
            insertall = True
            classesentered.Clear()
            Exit Sub
        Else
            For Each clas As Integer In problemclasses
                Dim possiblesubjects As New ArrayList
                Dim allsuboptions As New ArrayList
                Dim cmdInsert6y As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM classsubjects Where Class = ? and sgroup = '" & 0 & "' order by periods desc", con)
                cmdInsert6y.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", clas))
                Dim reader6y As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert6y.ExecuteReader
                Do While reader6y.Read
                    If Not allsuboptions.Contains(reader6y.Item("subject")) Then
                        allsuboptions.Add(reader6y.Item("subject"))
                    End If
                Loop
                reader6y.Close()

                For Each ps As Integer In allsuboptions
                    Dim cmdInsert20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.teacher, classsubjects.periods, subjects.subject, class.class FROM classsubjects inner join subjects on subjects.id = classsubjects.subject inner join class on class.id = classsubjects.class WHERE Classsubjects.class = ? And classsubjects.Subject = ?", con)
                    cmdInsert20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", clas))
                    cmdInsert20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", ps))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert20.ExecuteReader

                    Dim periodsallocated As Integer = 0
                    reader20.Read()
                    periodsallocated = reader20.Item(1)
                    reader20.Close()


                    Dim cmdInsert13 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Class = ? And Subject = ? and tname = '" & Session("timetable") & "'", con)
                    cmdInsert13.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", clas))
                    cmdInsert13.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", ps))
                    Dim reader13 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert13.ExecuteReader
                    Dim periodstaken As Integer = 0
                    Do While reader13.Read()
                        periodstaken = periodstaken + 1
                    Loop
                    reader13.Close()
                    If periodstaken <> periodsallocated Then
                        possiblesubjects.Add(ps)
                    End If
                    periodsallocated = Nothing
                    periodstaken = Nothing
                Next


                For Each ps As Integer In possiblesubjects
                    Dim offerredtoday As Boolean = False
                    Dim psteacher As String = ""
                    Dim cmdInsert20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.teacher FROM classsubjects WHERE Classsubjects.class = ? And classsubjects.Subject = ?", con)
                    cmdInsert20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", clas))
                    cmdInsert20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", ps))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert20.ExecuteReader
                    reader20.Read()
                    psteacher = reader20(0).ToString
                    reader20.Close()

                    Dim cmdInsert445 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And Class = ? and subject = '" & ps & "' and tname = '" & Session("timetable") & "'", con)
                    cmdInsert445.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                    cmdInsert445.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", clas))
                    Dim reader445 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert445.ExecuteReader
                    If reader445.Read() Then offerredtoday = True
                    reader445.Close()
                    Dim swapped As Boolean = False
                    If offerredtoday = False Then
                        Dim swapoptions As New ArrayList
                        Dim swapperiod As New ArrayList
                        Dim swapteacher As New ArrayList
                        Dim cmdInsert12 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And class = ? and period > ? and tname = ? and grouped = ? and doubles = 0", con)
                        cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                        cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", clas))
                        cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", 2))
                        cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Session("timetable")))
                        cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ffg", 0))
                        cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fg", 0))
                        Dim reader12 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert12.ExecuteReader
                        Do While reader12.Read
                            swapoptions.Add(reader12("subject"))
                            swapperiod.Add(reader12("period"))
                            swapteacher.Add(reader12("teacher"))

                        Loop

                        reader12.Close()
                        Dim clt As Integer = 0
                        For Each st As String In swapteacher
                            Dim cmdInsert137 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And Period = ? And Teacher = ? and tname = ?", con)
                            cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                            cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", n))
                            cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", st))
                            cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Session("timetable")))
                            Dim reader137 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert137.ExecuteReader
                            If reader137.Read Then
                                reader137.Close()
                                clt = clt + 1
                                Continue For
                            End If
                            reader137.Close()
                            Dim cmdInsert124 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And Period = ? And Teacher = ? and tname = ?", con)
                            cmdInsert124.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                            cmdInsert124.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", swapperiod(clt)))
                            cmdInsert124.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("iod", psteacher))
                            cmdInsert124.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Session("timetable")))
                            Dim reader124 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert124.ExecuteReader
                            If reader124.Read Then
                                reader124.Close()
                                clt = clt + 1
                                Continue For
                            End If
                            reader124.Close()

                            Dim cmdInsert16 As New MySql.Data.MySqlClient.MySqlCommand("Update timetable set teacher = ?, subject = ? where tname = ? and day = ? and period = ? and class = ?", con)
                            cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", psteacher))
                            cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", ps))
                            cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Session("timetable")))
                            cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", d))
                            cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", swapperiod(clt)))
                            cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Doubles", clas))
                            cmdInsert16.ExecuteNonQuery()

                            Dim cmdInsert35 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into timetable (tname, Class, Period, Subject, Doubles, Day, Teacher) Values(?,?,?,?,?,?,?)", con)
                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Session("timetable")))
                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", clas))
                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", n))
                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", swapoptions(clt)))
                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Doubles", 0))
                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                            cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", st))
                            cmdInsert35.ExecuteNonQuery()
                            swapped = True
                            classesentered.Add(clas)
                            clt = Nothing
                            Exit For
                        Next
                        swapoptions = Nothing
                        swapperiod = Nothing
                        swapteacher = Nothing
                        clt = Nothing
                        If swapped = True Then
                            swapped = Nothing
                            offerredtoday = Nothing
                            Exit For
                        End If
                        swapped = Nothing
                        offerredtoday = Nothing

                    End If
                    Dim swapdays As New ArrayList
                    If offerredtoday = True Or swapped = False Then
                        swapdays.Add("Monday")
                        swapdays.Add("Tuesday")
                        swapdays.Add("Wednesday")
                        swapdays.Add("Thursday")
                        swapdays.Add("Friday")
                        swapdays.Remove(d)
                        For Each sd As String In swapdays
                            Dim swapoptions As New ArrayList
                            Dim swapperiod As New ArrayList
                            Dim swapteacher As New ArrayList
                            Dim cmdInsert12 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And class = ? and period > ? and tname = ? and grouped = ? and doubles = ?", con)
                            cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", sd))
                            cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", clas))
                            cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", 2))
                            cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Session("timetable")))
                            cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ffg", 0))
                            cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fg", 0))

                            Dim reader12 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert12.ExecuteReader
                            Do While reader12.Read
                                swapoptions.Add(reader12("subject"))
                                swapperiod.Add(reader12("period"))
                                swapteacher.Add(reader12("teacher"))

                            Loop

                            reader12.Close()
                            Dim clt As Integer = 0
                            For Each st As String In swapteacher
                                Dim cmdInsert88 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.subject FROM texemptions inner join classsubjects on classsubjects.teacher = texemptions.teacher Where texemptions.day = ? And Classsubjects.class = ? and classsubjects.subject = '" & swapoptions(clt) & "' and texemptions.timetable = '" & Session("timetable") & "'", con)
                                cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                                cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", clas))
                                Dim reader88 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert88.ExecuteReader
                                If reader88.Read Then
                                    reader88.Close()
                                    clt = clt + 1
                                    Continue For
                                End If
                                reader88.Close()


                                Dim cmdInsert97 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.subject FROM texemptions inner join classsubjects on classsubjects.teacher = texemptions.teacher Where texemptions.day = ? And Classsubjects.class = ? and classsubjects.subject = '" & ps & "' and texemptions.timetable = '" & Session("timetable") & "'", con)
                                cmdInsert97.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", sd))
                                cmdInsert97.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", clas))
                                Dim reader97 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert97.ExecuteReader
                                If reader97.Read Then
                                    reader97.Close()
                                    clt = clt + 1
                                    Continue For
                                End If
                                reader97.Close()

                                Dim cmdInsert137 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And Period = ? And Teacher = ? and tname = ?", con)
                                cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                                cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", n))
                                cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", st))
                                cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Session("timetable")))
                                Dim reader137 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert137.ExecuteReader
                                If reader137.Read Then
                                    reader137.Close()
                                    clt = clt + 1
                                    Continue For
                                End If
                                reader137.Close()
                                Dim cmdInsert124 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And Period = ? And Teacher = ? and tname = ?", con)
                                cmdInsert124.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", sd))
                                cmdInsert124.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", swapperiod(clt)))
                                cmdInsert124.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("iod", psteacher))
                                cmdInsert124.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Session("timetable")))
                                Dim reader124 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert124.ExecuteReader
                                If reader124.Read Then
                                    reader124.Close()
                                    clt = clt + 1
                                    Continue For
                                End If
                                reader124.Close()
                                Dim cmdInsert312 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And Class = ? and subject = '" & swapoptions(clt) & "' and tname = '" & Session("timetable") & "'", con)
                                cmdInsert312.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                                cmdInsert312.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", clas))
                                Dim reader312 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert312.ExecuteReader

                                If reader312.Read() Then
                                    reader312.Close()
                                    clt = clt + 1
                                    Continue For
                                End If
                                reader312.Close()
                                Dim cmdInsert56 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And Class = ? and subject = '" & ps & "' and tname = '" & Session("timetable") & "'", con)
                                cmdInsert56.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", sd))
                                cmdInsert56.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", clas))
                                Dim reader56 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert56.ExecuteReader

                                If reader56.Read() Then
                                    reader56.Close()
                                    clt = clt + 1
                                    Continue For
                                End If
                                reader56.Close()

                                Dim cmdInsert16 As New MySql.Data.MySqlClient.MySqlCommand("Update timetable set teacher = ?, subject = ? where tname = ? and day = ? and period = ? and class = ?", con)
                                cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", psteacher))
                                cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", ps))
                                cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Session("timetable")))
                                cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", sd))
                                cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", swapperiod(clt)))
                                cmdInsert16.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Doubles", clas))
                                cmdInsert16.ExecuteNonQuery()

                                Dim cmdInsert35 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into timetable (tname, Class, Period, Subject, Doubles, Day, Teacher) Values(?,?,?,?,?,?,?)", con)
                                cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Session("timetable")))
                                cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", clas))
                                cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", n))
                                cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", swapoptions(clt)))
                                cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Doubles", 0))
                                cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                                cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", st))
                                cmdInsert35.ExecuteNonQuery()
                                swapped = True

                                classesentered.Add(clas)
                                clt = Nothing
                                Exit For
                            Next

                            swapoptions = Nothing
                            swapperiod = Nothing
                            swapteacher = Nothing
                            clt = Nothing
                            If swapped = True Then Exit For
                        Next
                        swapdays = Nothing
                    End If
                    If swapped = True Then
                        swapped = Nothing
                        offerredtoday = Nothing
                        Exit For
                    End If
                    swapped = Nothing
                    offerredtoday = Nothing


                Next
                possiblesubjects = Nothing
                allsuboptions = Nothing
            Next
            problemclasses = Nothing

            For Each sc As Integer In classes
                If Not classesentered.Contains(sc) Then
                    insertall = False
                    Exit Sub
                End If
            Next
            insertall = True
            Exit Sub
        End If
    End Sub

   



    Protected Sub Wizard1_FinishButtonClick(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.FinishButtonClick
        Try
            con.Open()

            Dim accuracy As Integer = 0
            Dim time As DateTime = Now
            Dim cmdInserth As New MySql.Data.MySqlClient.MySqlCommand("SELECT ttname.school, depts.dept FROM ttname inner join depts on ttname.school = depts.id where ttname.id ='" & Session("timetable") & "'", con)
            Dim readerh2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInserth.ExecuteReader
            readerh2.Read()
            Dim school As Integer = readerh2.Item(0)
            Dim schoolname As String = readerh2(1)
            readerh2.Close()
            Dim slots As Integer = 0
            Dim actual As Integer = 0
            Dim loopamount As Integer = 0
            Do
                Dim finish As TimeSpan = Now.ToLocalTime - time.ToLocalTime
                If finish.Minutes >= 20 Then
                    Show_Alert(False, "Unable to generate time table. Change some options and try again.")
                    e.Cancel = True
                    Exit Sub
                End If
                Dim restart As Boolean = False
                Dim dayex As New ArrayList
                dayex.Add("Monday")
                dayex.Add("Tuesday")
                dayex.Add("Wednesday")
                dayex.Add("Thursday")
                dayex.Add("Friday")
                Dim dayexno As New ArrayList
                Dim cmdInsert4 As New MySql.Data.MySqlClient.MySqlCommand("delete FROM timetable where tname ='" & Session("timetable") & "'", con)
                cmdInsert4.ExecuteNonQuery()
                For Each sc As String In dayex
                    Dim nox As Integer = 0
                    Dim cmdInsert6d As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM texemptions inner join (classsubjects inner join class on classsubjects.class = class.id) on texemptions.teacher = classsubjects.teacher where texemptions.day = '" & sc & "' and class.superior = '" & school & "' and texemptions.timetable = '" & Session("timetable") & "'", con)
                    Dim reader6d As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert6d.ExecuteReader
                    Do While reader6d.Read
                        nox = nox + 1
                    Loop
                    dayexno.Add(nox)
                    reader6d.Close()
                    nox = Nothing
                Next
                Dim Day As New ArrayList
                Dim countd As Integer = 0
                For Each sc As String In dayex
                    If dayexno(countd) >= dayexno(0) And dayexno(countd) >= dayexno(1) And dayexno(countd) >= dayexno(2) And dayexno(countd) >= dayexno(3) And dayexno(countd) >= dayexno(4) Then
                        Day.Add(sc)

                    End If
                    countd = countd + 1
                Next
                countd = Nothing
                Dim lk As Integer = 0
                For Each ki As String In Day
                    If dayex.Contains(ki) Then

                        For Each gh In dayex
                            If gh = ki Then Exit For
                            lk = lk + 1
                        Next
                        dayex.Remove(ki)
                        dayexno.Remove(dayexno(lk))
                    End If
                Next
                For Each sc As String In dayex
                    If Not countd = lk Then
                        Dim nc As Boolean = True
                        For Each gh In dayexno
                            If dayexno(countd) < gh Then
                                nc = False
                            End If
                        Next
                        If nc = True Then Day.Add(sc)
                    End If
                    countd = countd + 1
                Next
                lk = Nothing
                countd = Nothing
                For Each ki As String In Day
                    If dayex.Contains(ki) Then
                        For Each gh In dayex
                            If gh = ki Then Exit For
                            lk = lk + 1
                        Next
                        dayex.Remove(ki)
                        dayexno.Remove(dayexno(lk))
                    End If
                Next
                For Each sc As String In dayex
                    If Not countd = lk Then
                        Dim nc As Boolean = True
                        For Each gh In dayexno
                            If dayexno(countd) < gh Then
                                nc = False
                            End If
                        Next
                        If nc = True Then Day.Add(sc)
                    End If
                    countd = countd + 1
                Next
                lk = Nothing
                countd = Nothing
                For Each ki As String In Day
                    If dayex.Contains(ki) Then
                        For Each gh In dayex
                            If gh = ki Then Exit For
                            lk = lk + 1
                        Next
                        dayex.Remove(ki)
                        dayexno.Remove(dayexno(lk))
                    End If
                Next
                For Each sc As String In dayex
                    Dim nc As Boolean = True
                    For Each gh In dayexno
                        If dayexno(countd) < gh Then
                            nc = False
                        End If
                    Next
                    If nc = True Then Day.Add(sc)
                    countd = countd + 1
                Next
                lk = Nothing
                countd = Nothing
                For Each ki As String In Day
                    If dayex.Contains(ki) Then
                        For Each gh In dayex
                            If gh = ki Then Exit For
                            lk = lk + 1
                        Next
                        dayex.Remove(ki)
                        dayexno.Remove(dayexno(lk))
                    End If
                Next
                For Each sc As String In dayex
                    Day.Add(sc)
                Next
                dayex = Nothing

                For Each d As String In Day

                    Dim cmdInsert4v As New MySql.Data.MySqlClient.MySqlCommand("delete FROM timetable where tname ='" & Session("timetable") & "' and day = '" & d & "'", con)
                    cmdInsert4v.ExecuteNonQuery()
                    Dim cmdInsert6 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM tperiods Where Day = ? and activity = ? and timetable = '" & Session("timetable") & "' order by timestart asc", con)
                    cmdInsert6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                    cmdInsert6.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Dy", "Tutorial"))
                    Dim reader6 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert6.ExecuteReader
                    Dim countp As Integer = 0
                    Dim starts As New ArrayList
                    Dim ends As New ArrayList
                    Do While reader6.Read
                        countp = countp + 1
                        starts.Add(reader6("timestart"))
                        ends.Add(reader6("timeend"))
                    Loop
                    reader6.Close()
                    Dim periodbeforebreaks As New ArrayList
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from tperiods where day = '" & d & "' and timetable = '" & Session("timetable") & "'  order by timestart", con)
                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Dim pb As Integer = 0
                    Dim cxt As Integer = 0
                    Do While reader.Read
                        If Not reader("activity").ToString = "Tutorial" Then
                            periodbeforebreaks.Add(cxt + 1)
                        Else
                            cxt = cxt + 1
                        End If
                    Loop
                    reader.Close()
                    Dim periodfull As Boolean = False
                    For n = 1 To countp

                        Dim hx As Integer = 0


                        Do
                            Dim insertall As Boolean = False
                            Dim classes As New ArrayList
                            Dim doublesubject As New ArrayList
                            Dim doubleclass As New ArrayList
                            Dim doubleno As New ArrayList
                            Dim cmdInsert20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.subject, classsubjects.class, classsubjects.doubles FROM classsubjects inner join class on classsubjects.class = class.id WHERE classsubjects.doubles <> ? and class.superior = ? order by classsubjects.doubles desc", con)
                            cmdInsert20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", 0))
                            cmdInsert20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Cl", school))
                            Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert20.ExecuteReader
                            Do While reader20.Read()
                                doublesubject.Add(reader20(0))
                                doubleclass.Add(reader20(1))
                                doubleno.Add(reader20(2))
                            Loop
                            reader20.Close()
                            Dim bcount As Integer = 0
                            For Each jp As Integer In doubleclass
                                Dim doubleperiod As Integer = 0
                                Dim cmdInsert8 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Day = ? And Class = ? And subject = ? and Period = ? and tname = '" & Session("timetable") & "'", con)
                                cmdInsert8.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", d))
                                cmdInsert8.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Pe", jp))
                                cmdInsert8.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", doublesubject(bcount)))
                                cmdInsert8.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Piod", n - 1))
                                Dim reader8 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert8.ExecuteReader
                                If reader8.Read Then
                                    If reader8.Item("Doubles") = 1 Then
                                        reader8.Close()
                                        Continue For
                                    End If
                                Else
                                    reader8.Close()
                                    Continue For
                                End If
                                reader8.Close()
                                Dim todaysDoubles As Boolean = False
                                Dim cmdInsert440 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable Where Class = ? And Subject = ? And Doubles = ? and tname = '" & Session("timetable") & "'", con)
                                cmdInsert440.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", jp))
                                cmdInsert440.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", doublesubject(bcount)))
                                cmdInsert440.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Doubles", 1))
                                Dim reader440 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert440.ExecuteReader
                                Dim doubledays As New ArrayList
                                Dim counts As Integer = 0
                                Do While reader440.Read()
                                    doubledays.Add(reader440.Item("Day"))
                                    counts = counts + 1
                                Loop
                                reader440.Close()
                                For Each g As String In doubledays
                                    If g = d Then
                                        todaysDoubles = True
                                        Exit For
                                    End If
                                Next
                                If doubledays.Count < doubleno(bcount) And todaysDoubles = False And Not classes.Contains(jp) Then
                                    classes.Add(jp)
                                End If
                                counts = Nothing

                                bcount = bcount + 1
                            Next
                            bcount = Nothing
                            Dim cmdInsert5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM class where superior ='" & school & "'", con)
                            Dim reader52 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert5.ExecuteReader
                            Dim clx As New ArrayList
                            Do While reader52.Read
                                If Not clx.Contains(reader52(0)) Then
                                    clx.Add(reader52.Item("ID"))
                                End If
                            Loop
                            reader52.Close()
                            For i As Integer = 0 To clx.Count - 1
                                Dim rnds As New Random
                                ' Pick an item for position i.

                                Dim h As Integer = rnds.Next(i, clx.Count - 1)
                                ' Swap them.
                                Dim temps As Integer = clx(i)
                                clx(i) = clx(h)
                                clx(h) = temps
                                rnds = Nothing
                            Next i
                            For Each xc As Integer In clx
                                If Not classes.Contains(xc) Then
                                    classes.Add(xc)
                                End If
                            Next
                            Dim classesentered As New ArrayList
                            Dim isdouble As Integer = 0
                            If hx <= 3 Then
                                Each_Period(d, n, insertall, classes, classesentered, isdouble, periodbeforebreaks)

                            Else
                                swap_subjects(d, n, insertall, classes, classesentered, isdouble, periodbeforebreaks)
                            End If

                            classesentered = Nothing
                            If insertall = True Then
                                classes = Nothing
                                periodfull = False
                                Exit Do

                            End If
                            If insertall = False And hx > 3 Then
                                classes = Nothing
                                restart = True

                                Exit Do
                            End If
                            hx = hx + 1
                            classes = Nothing
                            insertall = Nothing
                        Loop

                        hx = Nothing

                        If restart = True Then Exit For

                    Next

                    If restart = True Then

                        Exit For
                    End If

                    countp = Nothing
                    periodbeforebreaks = Nothing

                Next

                Dim cmdInsert100 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM classsubjects inner join class on class.id = classsubjects.class where class.superior = '" & school & "'", con)
                Dim reader100 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert100.ExecuteReader
                slots = 0
                actual = 0
                Do While reader100.Read
                    slots = slots + reader100.Item("Periods")
                Loop
                reader100.Close()
                Dim cmdInsert101 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM timetable where tname = '" & Session("timetable") & "'", con)
                Dim reader101 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert101.ExecuteReader
                Do While reader101.Read
                    actual = actual + 1
                Loop
                reader101.Close()


                restart = False

            Loop Until actual = slots
            logify.log(Session("staffid"), "A new time table for " & schoolname & " was generated.")
            Response.Redirect("~/content/App/Admin/viewtimetable.aspx?timetable=" & Session("timetable") & "&class=0&school=" & school)
            con.Close()

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Wizard1_NextButtonClick(sender As Object, e As WizardNavigationEventArgs)

    End Sub


    Protected Sub Wizard1_NextButtonClick1(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.NextButtonClick
        Try
            If Wizard1.StartNextButtonText = "FINISH" Then Response.Redirect("~/content/App/Admin/managetimetable.aspx")

            con.Open()
            If Wizard1.ActiveStepIndex = 0 Then
                If txtID.Text = "" Then
                    Show_Alert(False, "Please enter a time table name")
                    e.Cancel = True
                    Exit Sub
                End If
                If cboDept.Text = "Select School" Then
                    Show_Alert(False, "Please select a school")

                    e.Cancel = True
                    Exit Sub
                End If
                Dim cmdf0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from ttname Where name = ?", con)
                cmdf0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", txtID.Text))
                Dim readerf0 As MySql.Data.MySqlClient.MySqlDataReader = cmdf0.ExecuteReader
                If readerf0.Read() Then
                    Show_Alert(False, "Name exists. please use a different name")
                    e.Cancel = True
                    Exit Sub
                End If
                readerf0.Close()
                Dim cmd5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From depts where dept = '" & cboDept.Text & "'", con)
                Dim reader5 As MySql.Data.MySqlClient.MySqlDataReader = cmd5.ExecuteReader
                If Not reader5.Read() Then
                    reader5.Close()
                    Show_Alert(False, "Please select A valid school.")
                    e.Cancel = True
                    Exit Sub
                Else
                    Dim dept As Integer = reader5.Item(0)
                    reader5.Close()

                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into ttname (name, school) Values (?,?)", con)
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", txtID.Text))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dept", dept))
                    cmd3.ExecuteNonQuery()
                End If
                Dim cmd35 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id From ttname where name = '" & txtID.Text & "'", con)
                Dim reader35 As MySql.Data.MySqlClient.MySqlDataReader = cmd35.ExecuteReader
                reader35.Read()
                Session("timetable") = reader35.Item(0)
                reader35.Close()
            ElseIf Wizard1.ActiveStepIndex = 1 Then
                Dim a As New DataTable
                a.Columns.Add("name")
                a.Columns.Add("id")

                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select name, id from tcgroups", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview3.DataSource = a

                Gridview3.DataBind()
                msg.Close()
            ElseIf Wizard1.ActiveStepIndex = 3 Then
                Dim cmdf0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT school from ttname Where id = ?", con)
                cmdf0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("timetable")))
                Dim readerf0 As MySql.Data.MySqlClient.MySqlDataReader = cmdf0.ExecuteReader
                readerf0.Read()
                Dim dept As Integer = readerf0(0)
                readerf0.Close()
                Dim classess As New ArrayList
                Dim clasname As New ArrayList
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select id, class from class where superior = '" & dept & "'", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read
                    classess.Add(msg(0))
                    clasname.Add(msg(1))
                Loop
                msg.Close()


                Dim totalperiods As Integer
                Dim previous As Integer = 0
                Dim ct As Integer
                For Each item As String In classess
                    Dim subjects As New ArrayList
                    Dim sgroup As New ArrayList
                    Dim total As Integer = 0
                    Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tsgroups.name, classsubjects.periods from classsubjects inner join tsgroups on tsgroups.id = classsubjects.sgroup inner join subjects on subjects.id = classsubjects.subject Where classsubjects.class = ?", con)
                    cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                    Do While reader20.Read()
                        If Not sgroup.Contains(reader20(0).ToString) Then
                            sgroup.Add(reader20(0).ToString)
                            total = total + Val(reader20(1))
                        End If
                    Loop
                    reader20.Close()
                    Dim cmd1g As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, classsubjects.periods from classsubjects inner join subjects on subjects.id = classsubjects.subject Where classsubjects.class = ? and classsubjects.sgroup = ?", con)
                    cmd1g.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    cmd1g.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.udent", ""))

                    Dim reader2g As MySql.Data.MySqlClient.MySqlDataReader = cmd1g.ExecuteReader
                    Do While reader2g.Read()
                        If Not subjects.Contains(reader2g(0).ToString) Then
                            subjects.Add(reader2g(0).ToString)
                            total = total + Val(reader2g(1))
                        End If
                    Loop
                    reader2g.Close()

                    Dim cmd1h As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classperiods inner join tperiods on tperiods.id = classperiods.period where classperiods.class = '" & item & "' and tperiods.timetable = '" & Session("timetable") & "'", con)
                    cmd1h.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    cmd1h.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.udent", ""))

                    Dim reader2h As MySql.Data.MySqlClient.MySqlDataReader = cmd1h.ExecuteReader
                    Do While reader2h.Read()
                        total = total + 1
                    Loop
                    reader2h.Close()

                    If previous = Nothing Then
                        previous = total
                    Else
                        If total <> previous Then
                            e.Cancel = True
                            Show_Alert(False, "Period count does not match with weekly subject periods count for " & clasname(ct))
                            Exit Sub
                        End If

                    End If
                    total = Nothing
                    sgroup = Nothing
                    subjects = Nothing
                    ct = ct + 1
                Next

                Dim cmd15 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from tperiods where activity = '" & "Tutorial" & "' and timetable = '" & Session("timetable") & "'", con)
                Dim reader25 As MySql.Data.MySqlClient.MySqlDataReader = cmd15.ExecuteReader
                Do While reader25.Read()
                    totalperiods = totalperiods + 1
                Loop
                reader25.Close()
                If totalperiods <> previous Then
                    Show_Alert(False, "Time table periods (" & totalperiods & ") is not equal to subject periods (" & previous & ") in classes of this department")
                    e.Cancel = True
                    Exit Sub
                End If
                Dim a As New DataTable
                a.Columns.Add("subject")
                a.Columns.Add("class")
                a.Columns.Add("amount")
                a.Columns.Add("id")
                Dim cmdChecka2 As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject, class.class, classsubjects.doubles, classsubjects.id from classsubjects inner join subjects on classsubjects.subject = subjects.id inner join class on classsubjects.class = class.id where classsubjects.doubles <> '" & 0 & "' order by classsubjects.doubles desc", con)
                Dim msga As MySql.Data.MySqlClient.MySqlDataReader = cmdChecka2.ExecuteReader
                Dim i As Integer
                Do While msga.Read()
                    a.Rows.Add(msga(0), msga.Item(1).ToString, msga.Item(2).ToString, msga.Item(3))
                Loop
                Gridview4.DataSource = a

                Gridview4.DataBind()
                msga.Close()
            ElseIf Wizard1.ActiveStepIndex = 5 Then
                Dim a As New DataTable
                a.Columns.Add("teacher")
                a.Columns.Add("day")
                a.Columns.Add("id")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname, texemptions.day, texemptions.id from texemptions inner join staffprofile on staffprofile.staffid = texemptions.teacher where texemptions.timetable = '" & Session("timetable") & "'", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg(0), msg.Item(1).ToString, msg.Item(2).ToString)
                Loop
                Gridview5.DataSource = a

                Gridview5.DataBind()
                msg.Close()
            End If
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles Gridview1.RowCancelingEdit
        Gridview1.EditIndex = -1

    End Sub

    Protected Sub Gridview1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview1.RowDeleting
        Try
            Dim row As GridViewRow = Gridview1.Rows(e.RowIndex)
            Dim starts As Array = Split(row.Cells(1).Text, " - ")
            con.Open()
            Dim cmdCheck2x As New MySql.Data.MySqlClient.MySqlCommand("Select name from ttname where id = '" & Session("timetable") & "'", con)
            Dim msgx As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2x.ExecuteReader
            msgx.Read()
            Dim ttname As String = msgx(0).ToString
            msgx.Close()

            Dim cmd2ax As New MySql.Data.MySqlClient.MySqlCommand("select id from tperiods where timetable = '" & Session("timetable") & "' and day = '" & cboDay.Text & "' and timestart = '" & starts(0) & "' and timeend = '" & starts(1) & "'", con)
            Dim rrrx As MySql.Data.MySqlClient.MySqlDataReader = cmd2ax.ExecuteReader
            rrrx.Read()
            Dim periodid As String = rrrx(0).ToString
            rrrx.Close()

            Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("delete from tperiods Where timestart = '" & starts(0) & "' and timeend = '" & starts(1) & "' and day = '" & cboDay.Text & "' and timetable = '" & Session("timetable") & "'", con)
            cmd10.ExecuteNonQuery()
            logify.log(Session("staffid"), "Period " & starts(0) & " - " & starts(1) & " on " & cboDay.Text & " for " & ttname & " was deleted")

            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("delete from classperiods where period = '" & periodid & "'", con)
            cmd3.ExecuteNonQuery()
            Dim a As New DataTable
            a.Columns.Add("period")
            a.Columns.Add("time")
            a.Columns.Add("activity")
            a.Columns.Add("id")
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select period, timestart, timeend, activity, id from tperiods where day = '" & cboDay.Text & "' and timetable = '" & Session("timetable") & "' order by timestart asc", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Dim i As Integer
            Do While msg.Read()
                If msg(3).ToString = "Tutorial" Then
                    i = i + 1
                    a.Rows.Add(i, msg.Item(1).ToString & " - " & msg.Item(2).ToString, msg.Item(3), msg.Item(4))
                Else
                    a.Rows.Add("", msg.Item(1).ToString & " - " & msg.Item(2).ToString, msg.Item(3), msg.Item(4))

                End If
            Loop
            Gridview1.DataSource = a

            Gridview1.DataBind()
            msg.Close()
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles Gridview1.RowEditing
        Try
            Gridview1.EditIndex = e.NewEditIndex
            con.Open()
            Dim a As New DataTable
            a.Columns.Add("Subject")
            a.Columns.Add("periods")
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject, classsubjects.periods from classsubjects inner join subjects on classsubjects.subject = subjects.Id where classsubjects.class = '" & Session("AddClass") & "'", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Do While msg.Read()
                a.Rows.Add(msg.Item(0), msg.Item(1))
            Loop
            Gridview1.DataSource = a

            Gridview1.DataBind()
            msg.Close()
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview1.RowUpdating
        Try
            Dim row As GridViewRow = Gridview1.Rows(e.RowIndex)
            Dim ID As String = row.Cells(0).Text
            Dim sessions As Integer = Val(TryCast(row.Cells(1).Controls(0), TextBox).Text)
            con.Open()
            Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
            cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", ID))
            Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
            reader20.Read()
            Dim subId As Integer = reader20.Item(0)
            reader20.Close()

            Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update classsubjects set periods = '" & sessions & "' where subject = '" & subId & "'", con)
            enter.ExecuteNonQuery()
            Gridview1.EditIndex = -1
            Dim a As New DataTable
            a.Columns.Add("Subject")
            a.Columns.Add("periods")
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject, classsubjects.periods from classsubjects inner join subjects on classsubjects.subject = subjects.Id where classsubjects.class = '" & Session("AddClass") & "'", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Do While msg.Read()
                a.Rows.Add(msg.Item(0), msg.Item(1))
            Loop
            Gridview1.DataSource = a

            Gridview1.DataBind()
            msg.Close()
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub cboDay_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDay.SelectedIndexChanged
        Try

            If cboDay.Text = "Select Day" Then
                Show_Alert(False, "Please select a day")
                Exit Sub
            End If
            con.Open()
            Dim a As New DataTable
            a.Columns.Add("period")
            a.Columns.Add("time")
            a.Columns.Add("activity")
            a.Columns.Add("id")
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select period, timestart, timeend, activity, id from tperiods where day = '" & cboDay.Text & "' and timetable = '" & Session("timetable") & "' order by timestart asc", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Dim i As Integer
            Do While msg.Read()
                If msg(3).ToString = "Tutorial" Then
                    i = i + 1
                    a.Rows.Add(i, msg.Item(1).ToString & " - " & msg.Item(2).ToString, msg.Item(3), msg.Item(4))
                Else
                    a.Rows.Add("", msg.Item(1).ToString & " - " & msg.Item(2).ToString, msg.Item(3), msg.Item(4))

                End If
            Loop
            Gridview1.DataSource = a

            Gridview1.DataBind()
            msg.Close()

            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub bnAdd_Click(sender As Object, e As EventArgs) Handles bnAdd.Click
        Try
            If txtStart.Text = "" Or txtEnd.Text = "" Or txtActivity.Text = "" Then
                Show_Alert(False, "Please fill all fields.")
                Exit Sub
            End If
            con.Open()
            Dim cmdCheck2x As New MySql.Data.MySqlClient.MySqlCommand("Select name from ttname where id = '" & Session("timetable") & "'", con)
            Dim msgx As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2x.ExecuteReader
            msgx.Read()
            Dim ttname As String = msgx(0).ToString
            msgx.Close()
            If Session("pupdate") <> Nothing And Session("newperiod") = Nothing Then
                Dim time As Array = Split(Request.QueryString.ToString, " - ")
                Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("delete from classperiods where period = '" & Session("pupdate") & "'", con)
                cmd3.ExecuteNonQuery()
                If chkClassPeriods.Checked = False Then
                    Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Update tperiods set timestart = ?, timeend = ?, activity = ? Where id = ?", con)
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg", txtStart.Text))
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectRg.Su", txtEnd.Text))
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Su", txtActivity.Text))
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.ject", Session("pupdate")))
                    cmd2a.ExecuteNonQuery()

                   
                Else
                    Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Update tperiods set timestart = ?, timeend = ?, activity = ? Where id = ?", con)
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg", txtStart.Text))
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectRg.Su", txtEnd.Text))
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Su", "Tutorial"))
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.ject", Session("pupdate")))
                    cmd2a.ExecuteNonQuery()
                    For Each cla As ListItem In Chkclasss.Items
                        If cla.Selected = True Then
                            Dim cmd2ass As New MySql.Data.MySqlClient.MySqlCommand("select id from class where class = '" & cla.Text & "'", con)
                            Dim rrrs As MySql.Data.MySqlClient.MySqlDataReader = cmd2ass.ExecuteReader

                            rrrs.Read()
                            Dim clasid As String = rrrs(0).ToString
                            rrrs.Close()


                            Dim cmd2as As New MySql.Data.MySqlClient.MySqlCommand("select class.id from classperiods inner join class on classperiods.class = class.id  where class.class = '" & cla.Text & "' and classperiods.period = '" & Session("pupdate") & "'", con)
                            Dim rrr As MySql.Data.MySqlClient.MySqlDataReader = cmd2as.ExecuteReader
                            If rrr.Read() Then
                                rrr.Close()
                                Dim cmd3v As New MySql.Data.MySqlClient.MySqlCommand("Update classperiods set class = '" & clasid & "', activity = '" & txtActivity.Text & "' where period = '" & Session("pupdate") & "'", con)
                                cmd3v.ExecuteNonQuery()
                            Else
                                rrr.Close()
                                Dim cmd3v As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into classperiods (period, class, activity) Values ('" & Session("pupdate") & "', '" & clasid & "', '" & txtActivity.Text & "')", con)
                                cmd3v.ExecuteNonQuery()
                            End If
                        End If
                    Next
                End If
                logify.log(Session("staffid"), "Period " & txtStart.Text & " - " & txtEnd.Text & " on " & cboDay.Text & " for " & ttname & " was updated")
                Show_Alert(True, "Period updated successfully.")
                Session("pupdate") = Nothing
            Else
                If chkClassPeriods.Checked = False Then
                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into tperiods (timetable, day, timestart, timeend, activity) Values ('" & Session("timetable") & "', '" & cboDay.Text & "', '" & txtStart.Text & "', '" & txtEnd.Text & "', '" & txtActivity.Text & "')", con)
                    cmd3.ExecuteNonQuery()
                Else
                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into tperiods (timetable, day, timestart, timeend, activity) Values ('" & Session("timetable") & "', '" & cboDay.Text & "', '" & txtStart.Text & "', '" & txtEnd.Text & "', '" & "Tutorial" & "')", con)
                        cmd3.ExecuteNonQuery()

                        Dim cmd2ax As New MySql.Data.MySqlClient.MySqlCommand("select id from tperiods where timetable = '" & Session("timetable") & "' and day = '" & cboDay.Text & "' and timestart = '" & txtStart.Text & "' and timeend = '" & txtEnd.Text & "'", con)
                        Dim rrrx As MySql.Data.MySqlClient.MySqlDataReader = cmd2ax.ExecuteReader
                        rrrx.Read()
                        Dim periodid As String = rrrx(0).ToString
                        rrrx.Close()
                    For Each cla As ListItem In Chkclasss.Items
                        If cla.Selected = True Then
                            Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("select id from class where class = '" & cla.Text & "'", con)
                            Dim rrr As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                            rrr.Read()
                            Dim clasid As String = rrr(0).ToString
                            rrr.Close()

                            Dim cmd3v As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into classperiods (period, class, activity) Values ('" & periodid & "', '" & clasid & "', '" & txtActivity.Text & "')", con)
                            cmd3v.ExecuteNonQuery()
                        End If
                    Next
                End If
                logify.log(Session("staffid"), "Period " & txtStart.Text & " - " & txtEnd.Text & " on " & cboDay.Text & " for " & ttname & " was added.")

                Show_Alert(True, "Period Added Successfully")
            End If
            Session("newperiod") = Nothing
            panel1.Visible = False
            Dim a As New DataTable
            a.Columns.Add("period")
            a.Columns.Add("time")
            a.Columns.Add("activity")
            a.Columns.Add("id")
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select period, timestart, timeend, activity, id from tperiods where day = '" & cboDay.Text & "' and timetable = '" & Session("timetable") & "' order by timestart asc", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Dim i As Integer
            Do While msg.Read()
                If msg(3).ToString = "Tutorial" Then
                    i = i + 1
                    a.Rows.Add(i, msg.Item(1).ToString & " - " & msg.Item(2).ToString, msg.Item(3), msg.Item(4))
                Else
                    a.Rows.Add("", msg.Item(1).ToString & " - " & msg.Item(2).ToString, msg.Item(3), msg.Item(4))

                End If
            Loop
            Gridview1.DataSource = a

            Gridview1.DataBind()
            msg.Close()
            pnlClassActivity.Visible = False
            chkClassPeriods.Checked = False
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkNew_Click(sender As Object, e As EventArgs) Handles lnkNew.Click
        If cboDay.Text = "Select Day" Then
            Show_Alert(False, "Please select a day")
            Exit Sub
        End If
        panel1.Visible = True
        txtStart.Text = ""
        txtEnd.Text = ""
        chkTutorial.Checked = True
        txtActivity.Text = "Tutorial"
        txtActivity.Enabled = False
        Session("newperiod") = True
    End Sub

    Protected Sub chkTutorial_CheckedChanged(sender As Object, e As EventArgs) Handles chkTutorial.CheckedChanged
        If chkTutorial.Checked Then
            txtActivity.Enabled = False
            txtActivity.Text = "Tutorial"
        Else
            txtActivity.Enabled = True
            txtActivity.Text = ""
            chkClassPeriods.Visible = True
        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If txtGrp.Text = "" Then
                Show_Alert(False, "Please enter a group name.")
                Exit Sub
            End If
            con.Open()

            If Session("updatesgroup") <> Nothing And Session("newgroup") = Nothing Then
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Update tsgroups set name = ? Where name = ?", con)
                cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Su", txtGrp.Text))
                cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.ject", Session("updatesgroup")))
                cmd2a.ExecuteNonQuery()
                logify.log(Session("staffid"), txtGrp.Text & " subject group was updated")
                Session("updatesgroup") = Nothing
                Dim sgroup As Integer
                Dim cmdf0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from tsgroups Where name = ?", con)
                cmdf0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", txtGrp.Text))
                Dim readerf0 As MySql.Data.MySqlClient.MySqlDataReader = cmdf0.ExecuteReader
                readerf0.Read()
                sgroup = readerf0.Item(0)
                readerf0.Close()
                Dim isOfferred As Boolean = False
                Dim subId As Integer
                Dim cla As Integer

                For Each c As String In sgroupclasses
                    Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                    cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", c))
                    Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                    reader2a.Read()
                    cla = reader2a.Item(0)
                    reader2a.Close()
                    Dim actual As Integer
                    Dim previous As Integer = Nothing
                    For Each item As String In sgroupsubjects
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        reader20.Read()
                        subId = reader20.Item(0)
                        reader20.Close()
                        Dim cmd10f As New MySql.Data.MySqlClient.MySqlCommand("select periods from classsubjects where class = '" & cla & "' and subject = '" & subId & "'", con)
                        Dim msg2v As MySql.Data.MySqlClient.MySqlDataReader = cmd10f.ExecuteReader
                        msg2v.Read()
                        actual = Val(msg2v.Item(0))
                        If previous = Nothing Then
                            previous = actual
                        Else
                            If actual <> previous Then
                                Show_Alert(False, "Number of teaching periods in the subjects are not the same.")
                                Exit Sub
                            End If
                        End If
                        msg2v.Close()
                    Next
                Next

                For Each item As String In sgroupsubjects
                    Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                    cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                    reader20.Read()
                    subId = reader20.Item(0)
                    reader20.Close()
                    For Each subitem As String In sgroupclasses
                        Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                        cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                        Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                        reader2a.Read()
                        cla = reader2a.Item(0)
                        reader2a.Close()

                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set sgroup = ? where subject = ? and class = ?", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", sgroup))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                        cmd3.ExecuteNonQuery()
                    Next
                Next



                For Each item As String In nsgroupsubjects
                    Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                    cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                    reader20.Read()
                    subId = reader20.Item(0)
                    reader20.Close()
                    For Each subitem As String In sgroupclasses
                        Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                        cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                        Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                        reader2a.Read()
                        cla = reader2a.Item(0)
                        reader2a.Close()
                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set sgroup = ? where subject = ? and sgroup = ?", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("te", sgroup))
                        cmd3.ExecuteNonQuery()
                    Next
                Next
                For Each item As String In nsgroupclasses
                    Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                    cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                    reader2a.Read()
                    cla = reader2a.Item(0)
                    reader2a.Close()
                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set sgroup = ? where class = ? and sgroup = ?", con)
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("te", sgroup))
                    cmd3.ExecuteNonQuery()
                Next
                Show_Alert(True, "Group updated successfully.")
            Else
                Dim cmdf4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from tsgroups Where name = ?", con)
                cmdf4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", txtGrp.Text))
                Dim readerf4 As MySql.Data.MySqlClient.MySqlDataReader = cmdf4.ExecuteReader
                If readerf4.HasRows Then
                    Show_Alert(False, "Group exists. Use a different name")
                    Exit Sub
                End If
                readerf4.Close()
                For Each c As String In sgroupclasses
                    Dim cl As Integer
                    Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                    cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", c))
                    Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                    reader2a.Read()
                    cl = reader2a.Item(0)
                    reader2a.Close()
                    Dim actual As Integer
                    Dim previous As Integer = Nothing
                    For Each item As String In sgroupsubjects
                        Dim sid As Integer
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        reader20.Read()
                        sid = reader20.Item(0)
                        reader20.Close()
                        Dim cmd10f As New MySql.Data.MySqlClient.MySqlCommand("select periods from classsubjects where class = '" & cl & "' and subject = '" & sid & "'", con)
                        Dim msg2v As MySql.Data.MySqlClient.MySqlDataReader = cmd10f.ExecuteReader
                        msg2v.Read()
                        actual = Val(msg2v.Item(0))
                        If previous = Nothing Then
                            previous = actual
                        Else
                            If actual <> previous Then
                                Show_Alert(False, "Number of teaching periods in the subjects are not the same.")
                                Exit Sub
                            End If
                        End If
                        msg2v.Close()
                    Next
                Next
                Dim cmd3a As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into tsgroups (name) Values ('" & txtGrp.Text & "')", con)
                cmd3a.ExecuteNonQuery()
                logify.log(Session("staffid"), txtGrp.Text & " subject group was added")
                Dim sgroup As Integer
                Dim cmdf0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from tsgroups Where name = ?", con)
                cmdf0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", txtGrp.Text))
                Dim readerf0 As MySql.Data.MySqlClient.MySqlDataReader = cmdf0.ExecuteReader
                readerf0.Read()
                sgroup = readerf0.Item(0)
                readerf0.Close()
                Dim isOfferred As Boolean = False
                Dim subId As Integer
                Dim cla As Integer
                For Each item As String In sgroupsubjects
                    Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                    cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                    reader20.Read()
                    subId = reader20.Item(0)
                    reader20.Close()
                    For Each subitem As String In sgroupclasses
                        Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                        cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                        Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                        reader2a.Read()
                        cla = reader2a.Item(0)
                        reader2a.Close()
                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set sgroup = ? where subject = ? and class = ?", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", sgroup))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                        cmd3.ExecuteNonQuery()
                    Next
                Next



                For Each item As String In nsgroupsubjects
                    Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                    cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                    reader20.Read()
                    subId = reader20.Item(0)
                    reader20.Close()
                    For Each subitem As String In sgroupclasses
                        Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                        cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                        Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                        reader2a.Read()
                        cla = reader2a.Item(0)
                        reader2a.Close()
                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set sgroup = ? where subject = ? and class = ? and sgroup = ?", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("te", sgroup))
                        cmd3.ExecuteNonQuery()
                    Next
                Next
                Show_Alert(True, "Subject Group Added Successfully")
            End If
            Session("newgroup") = Nothing
            Panel2.Visible = False
            Dim a As New DataTable
            a.Columns.Add("name")
            a.Columns.Add("id")

            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select name, id from tsgroups", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Do While msg.Read()
                a.Rows.Add(msg.Item(0), msg.Item(1))
            Loop
            Gridview2.DataSource = a

            Gridview2.DataBind()
            msg.Close()
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Panel2.Visible = True
        For Each item As ListItem In CheckBoxList2.Items
            item.Selected = False
        Next
        For Each sitem As ListItem In chkClasses.Items
            sitem.Selected = False
        Next
        txtGrp.Text = ""
        Session("newgroup") = True
        Button1.Text = "Add"
    End Sub

    Protected Sub Gridview2_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview2.RowDeleting
        Try
            Dim row As GridViewRow = Gridview2.Rows(e.RowIndex)
            Dim starts As String = row.Cells(0).Text
            con.Open()
            Dim sgroup As Integer
            Dim cmdf0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from tsgroups Where name = ?", con)
            cmdf0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", starts))
            Dim readerf0 As MySql.Data.MySqlClient.MySqlDataReader = cmdf0.ExecuteReader
            readerf0.Read()
            sgroup = readerf0.Item(0)
            readerf0.Close()
            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set sgroup = ? where sgroup = ?", con)
            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("te", sgroup))
            cmd3.ExecuteNonQuery()
            Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("delete from tsgroups Where name = '" & starts & "'", con)
            cmd10.ExecuteNonQuery()
            logify.log(Session("staffid"), starts & " subject group was removed")
            Show_Alert(True, "Group deleted successfully")
            Dim a As New DataTable
            a.Columns.Add("name")
            a.Columns.Add("id")

            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select name, id from tsgroups", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Do While msg.Read()
                a.Rows.Add(msg.Item(0), msg.Item(1))
            Loop
            Gridview2.DataSource = a

            Gridview2.DataBind()
            msg.Close()
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Gridview2.SelectedIndexChanged

    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Panel2.Visible = False
        Session("newgroup") = Nothing
    End Sub

    Protected Sub chkCGroup_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkCGroup.SelectedIndexChanged
        Try
            Load_Together_Subjects()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Sub Load_Together_Subjects()
        For Each i As ListItem In chkCSgroup.Items

            If i.Selected = True Then
                cgroupsubjects.Add(i.Text)
            Else
                ncgroupsubjects.Add(i.Text)
            End If
        Next
        For Each i As ListItem In chkCSSgroup.Items

            If i.Selected = True Then
                csgroupsubjects.Add(i.Text)
            Else
                ncsgroupsubjects.Add(i.Text)
            End If
        Next
        For Each i As ListItem In chkCGroup.Items

            If i.Selected = True Then
                cgroupclasses.Add(i.Text)
            Else
                ncgroupclasses.Add(i.Text)
            End If
        Next

        con.Open()
        Dim cla As Integer
        Dim subjects As New ArrayList
        Dim sgroup As New ArrayList
        If Not cgroupclasses.Count > 1 Then
            chkCSgroup.Items.Clear()
            chkCSSgroup.Items.Clear()
            Exit Sub
        End If


        For Each item As String In cgroupclasses
            Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
            cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
            Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
            reader2a.Read()
            cla = reader2a.Item(0)
            reader2a.Close()
            Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tsgroups.name from classsubjects inner join tsgroups on tsgroups.id = classsubjects.sgroup inner join subjects on subjects.id = classsubjects.subject Where classsubjects.class = ?", con)
            cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", cla))
            Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
            Do While reader20.Read()
                If Not sgroup.Contains(reader20(0).ToString) Then
                    sgroup.Add(reader20(0).ToString)
                End If
            Loop
            reader20.Close()
            Dim cmd1g As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject from classsubjects inner join subjects on subjects.id = classsubjects.subject Where classsubjects.class = ? and classsubjects.sgroup = ?", con)
            cmd1g.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", cla))
            cmd1g.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.udent", ""))

            Dim reader2g As MySql.Data.MySqlClient.MySqlDataReader = cmd1g.ExecuteReader
            Do While reader2g.Read()
                If Not subjects.Contains(reader2g(0).ToString) Then
                    subjects.Add(reader2g(0).ToString)
                End If
            Loop
            reader2g.Close()
        Next
        Dim toremove As New ArrayList
        For Each item As String In sgroup
            Dim actual As Integer = 0
            Dim total As Integer = 0
            For Each subitem As String In cgroupclasses
                Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, tsgroups.name from classsubjects left join tsgroups on tsgroups.id = classsubjects.sgroup inner join subjects on subjects.id = classsubjects.subject inner join class on class.id = classsubjects.class Where class.class = ? and tsgroups.name = ?", con)
                cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjctReg.student", subitem))
                cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                If reader20.Read Then actual = actual + 1
                reader20.Close()
                total = total + 1
            Next
            If Not actual = total Then
                toremove.Add(item)
            End If
        Next
        For Each item As String In toremove
            sgroup.Remove(item)
        Next

        Dim storemove As New ArrayList
        For Each item As String In subjects
            Dim actual As Integer = 0
            Dim total As Integer = 0
            For Each subitem As String In cgroupclasses
                Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, tsgroups.name from classsubjects left join tsgroups on tsgroups.id = classsubjects.sgroup inner join subjects on subjects.id = classsubjects.subject inner join class on class.id = classsubjects.class Where class.class = ? and subjects.subject = ? and classsubjects.sgroup = ?", con)
                cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjctReg.student", subitem))
                cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubctReg.student", ""))
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                If reader20.Read Then actual = actual + 1
                reader20.Close()
                total = total + 1
            Next
            If Not actual = total Then
                storemove.Add(item)
            End If
        Next
        For Each item As String In storemove
            subjects.Remove(item)
        Next
        chkCSgroup.Items.Clear()
        chkCSSgroup.Items.Clear()
        For Each item As String In sgroup
            chkCSSgroup.Items.Add(item)
        Next
        For Each item As String In subjects
            chkCSgroup.Items.Add(item)
        Next
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
        Panel3.Visible = True
        For Each item As ListItem In chkCGroup.Items
            item.Selected = False
        Next
        For Each sitem As ListItem In chkCSgroup.Items
            sitem.Selected = False
        Next
        For Each sitem As ListItem In chkCSSgroup.Items
            sitem.Selected = False
        Next
        chkCSgroup.Items.Clear()
        chkCSSgroup.Items.Clear()
        txtCGrp.Text = ""
        chkCGroup.Enabled = True
        Session("newcgroup") = True
        Button3.Text = "Add"
    End Sub



    Protected Sub Gridview3_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview3.RowDeleting
        Try
            Dim row As GridViewRow = Gridview3.Rows(e.RowIndex)
            Dim starts As String = row.Cells(0).Text
            con.Open()
            Dim sgroup As Integer
            Dim cmdf0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from tcgroups Where name = ?", con)
            cmdf0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", starts))
            Dim readerf0 As MySql.Data.MySqlClient.MySqlDataReader = cmdf0.ExecuteReader
            readerf0.Read()
            sgroup = readerf0.Item(0)
            readerf0.Close()
            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set cgroup = ? where cgroup = ?", con)
            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("te", sgroup))
            cmd3.ExecuteNonQuery()
            Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("delete from tcgroups Where name = '" & starts & "'", con)
            cmd10.ExecuteNonQuery()
            logify.log(Session("staffid"), starts & " class group was removed")
            Show_Alert(True, "Group deleted successfully")
            Dim a As New DataTable
            a.Columns.Add("name")
            a.Columns.Add("id")

            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select name, id from tcgroups", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Do While msg.Read()
                a.Rows.Add(msg.Item(0), msg.Item(1))
            Loop
            Gridview3.DataSource = a

            Gridview3.DataBind()
            msg.Close()
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Panel3.Visible = False
        Session("newcgroup") = Nothing
    End Sub

    Protected Sub LinkButton3_Click(sender As Object, e As EventArgs) Handles LinkButton3.Click
        panel4.Visible = True
        cboClasses.Items.Clear()
        cboClasses.Enabled = True
        cboSubject.Enabled = True
        cboSubject.Text = "Select Subject"
        txtDouble.Text = ""
    End Sub

    Protected Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            If cboSubject.Text = "Select Subject" Or cboClasses.Text = "Select Class" Or txtDouble.Text = "" Then
                Show_Alert(False, "Please fill all fields.")
                Exit Sub
            End If
            con.Open()
            Dim cla As Integer
            Dim subid As Integer
            Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
            cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", cboClasses.Text))
            Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
            reader2a.Read()
            cla = reader2a.Item(0)
            reader2a.Close()
            Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
            cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", cboSubject.Text))
            Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
            reader20.Read()
            subid = reader20.Item(0)
            reader20.Close()
            Dim cmd13 As New MySql.Data.MySqlClient.MySqlCommand("SELECT periods, sgroup from classsubjects Where Subject = ? and class = ?", con)
            cmd13.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subid))
            cmd13.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.studt", cla))
            Dim reader23 As MySql.Data.MySqlClient.MySqlDataReader = cmd13.ExecuteReader
            reader23.Read()
            Dim periods As Integer = reader23.Item(0)
            Dim sgroup As Integer = reader23.Item(1)
            reader23.Close()
            If periods < (2 * Val(txtDouble.Text)) Then
                Show_Alert(False, "You can add " & IIf(periods \ 2 < 1, "no ", "only " & periods \ 2) & " double " & IIf(periods \ 2 <= 1, "period", "periods") & " for this subject.")
                Exit Sub
            End If


            If sgroup <> 0 Then
                Dim cmdCheck5 As New MySql.Data.MySqlClient.MySqlCommand("Update classsubjects Set doubles = ? Where sgroup = ? and class = ?", con)
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", Val(txtDouble.Text)))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("htitle", sgroup))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", cla))
                cmdCheck5.ExecuteNonQuery()
                logify.log(Session("staffid"), Val(txtDouble.Text) & " double periods per week was added for " & cboSubject.Text & " - " & cboClasses.Text)
            Else
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Update classsubjects set doubles = ? where subject = ? and class = ?", con)
                cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg", Val(txtDouble.Text)))
                cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectRg.Su", subid))
                cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Su", cla))

                cmd2a.ExecuteNonQuery()
                logify.log(Session("staffid"), "Double periods per week was changed to " & Val(txtDouble.Text) & "for " & cboSubject.Text & " - " & cboClasses.Text)

            End If
            Show_Alert(True, "Double Period updated successfully.")
            panel4.Visible = False
            Dim a As New DataTable
            a.Columns.Add("subject")
            a.Columns.Add("class")
            a.Columns.Add("amount")
            a.Columns.Add("id")
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject, class.class, classsubjects.doubles, classsubjects.id from classsubjects inner join subjects on classsubjects.subject = subjects.id inner join class on classsubjects.class = class.id where classsubjects.doubles <> '" & 0 & "' order by classsubjects.doubles desc", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Dim i As Integer
            Do While msg.Read()
                a.Rows.Add(msg(0), msg.Item(1).ToString, msg.Item(2).ToString, msg.Item(3))
            Loop
            Gridview4.DataSource = a

            Gridview4.DataBind()
            msg.Close()

            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview4_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview4.RowDeleting
        Try
            Dim row As GridViewRow = Gridview4.Rows(e.RowIndex)
            Dim subject As String = row.Cells(0).Text
            Dim classes As String = row.Cells(1).Text

            con.Open()
            Dim subid As Integer
            Dim cla As Integer
            Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
            cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", classes))
            Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
            reader2a.Read()
            cla = reader2a.Item(0)
            reader2a.Close()
            Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
            cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subject))
            Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
            reader20.Read()
            subid = reader20.Item(0)
            reader20.Close()
            Dim cmd13 As New MySql.Data.MySqlClient.MySqlCommand("SELECT periods, sgroup from classsubjects Where Subject = ? and class = ?", con)
            cmd13.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subid))
            cmd13.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.studt", cla))
            Dim reader23 As MySql.Data.MySqlClient.MySqlDataReader = cmd13.ExecuteReader
            reader23.Read()
            Dim periods As Integer = reader23.Item(0)
            Dim sgroup As Integer = reader23.Item(1)
            reader23.Close()
            If sgroup <> 0 Then
                Dim cmdCheck5 As New MySql.Data.MySqlClient.MySqlCommand("Update classsubjects Set doubles = ? Where sgroup = ? and class = ?", con)
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", 0))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("htitle", sgroup))
                cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", cla))
                cmdCheck5.ExecuteNonQuery()
            Else
                Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set doubles = '" & 0 & "' where subject = '" & subid & "' and class = '" & cla & "'", con)
                cmd3.ExecuteNonQuery()
            End If
            logify.log(Session("staffid"), "Double period was removed for " & subject & " - " & classes)
            Dim a As New DataTable
            a.Columns.Add("subject")
            a.Columns.Add("class")
            a.Columns.Add("amount")
            a.Columns.Add("id")
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject, class.class, classsubjects.doubles, classsubjects.id from classsubjects inner join subjects on classsubjects.subject = subjects.id inner join class on classsubjects.class = class.id where classsubjects.doubles <> '" & 0 & "' order by classsubjects.doubles desc", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Dim i As Integer
            Do While msg.Read()
                a.Rows.Add(msg(0), msg.Item(1).ToString, msg.Item(2).ToString, msg.Item(3))
            Loop
            Gridview4.DataSource = a

            Gridview4.DataBind()
            msg.Close()

            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub cboSubject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubject.SelectedIndexChanged
        Try
            If cboSubject.Text = "Select Subject" Then
                Show_Alert(False, "Please select a subject")
                Exit Sub
            End If
            con.Open()
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from classsubjects inner join subjects on classsubjects.subject = subjects.id inner join class on classsubjects.class = class.id where subjects.subject <> '" & cboSubject.Text & "'", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Dim clases As New ArrayList
            Do While msg.Read
                If Not clases.Contains(msg(0).ToString) Then
                    cboClasses.Items.Add(msg(0).ToString)
                    clases.Add(msg(0).ToString)
                End If
            Loop
            msg.Close()
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        panel4.Visible = False
    End Sub

    Protected Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Try
            con.Open()
            If gridMorning.Rows.Count = 6 Then
                Show_Alert(False, "Maximum of 6 morning subjects allowed.")
                Exit Sub
            ElseIf cboSubject2.Text = "Select Subject" Then
                Show_Alert(False, "Please select a subject to add.")
                Exit Sub
            End If
            For Each item As GridViewRow In gridMorning.Rows
                If item.Cells(0).Text = cboSubject2.Text Then
                    Show_Alert(False, cboSubject2.Text & " has already been added")
                    Exit Sub
                End If
            Next
            Dim subid As Integer
            Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
            cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", cboSubject2.Text))
            Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
            reader20.Read()
            subid = reader20.Item(0)
            reader20.Close()
            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Insert into tsubject (subject) values ('" & subid & "')", con)
            cmd3.ExecuteNonQuery()

            Dim mor As New DataTable
            mor.Columns.Add("name")
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject from tsubject inner join subjects on subjects.id = tsubject.subject", con)
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Do While student.Read
                mor.Rows.Add(student.Item(0).ToString)
            Loop
            student.Close()
            gridMorning.DataSource = mor
            gridMorning.DataBind()
            logify.log(Session("staffid"), cboSubject2.Text & " was added as a morning subject")
            Show_Alert(True, "Subject Added successfully")
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub gridMorning_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gridMorning.RowDeleting
        Dim row As GridViewRow = gridMorning.Rows(e.RowIndex)
        Try
            con.Open()
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from subjects where subject = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", row.Cells(0).Text))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
            student.Read()
            Dim i As Integer = student.Item(0)
            student.Close()

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("Delete From tsubject Where Subject = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", i))
            cmdLoad1.ExecuteNonQuery()
            logify.log(Session("staffid"), row.Cells(0).Text & " was removed as a morning subject")
            Show_Alert(True, row.Cells(0).Text & " has been removed")
            Dim mor As New DataTable
            mor.Columns.Add("name")
            Dim cmdLoad1x As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject from tsubject inner join subjects on subjects.id = tsubject.subject", con)
            Dim studentx As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1x.ExecuteReader
            Do While studentx.Read
                mor.Rows.Add(studentx.Item(0).ToString)
            Loop
            studentx.Close()
            gridMorning.DataSource = mor
            gridMorning.DataBind()
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        Try
            If cboTeacher.Text = "Select Teacher" Then
                Show_Alert(False, "Please select a teacher")
                Exit Sub
            ElseIf cboDay2.Text = "Select Day" Then
                Show_Alert(False, "Please select a day")
                Exit Sub
            End If
            con.Open()
            Dim tid As String
            Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffid from staffprofile Where surname = ?", con)
            cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", cboTeacher.Text))
            Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
            reader20.Read()
            tid = reader20.Item(0)
            reader20.Close()
            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Insert into texemptions (teacher, day, timetable) values ('" & tid & "', '" & cboDay2.Text & "', '" & Session("timetable") & "')", con)
            cmd3.ExecuteNonQuery()
            logify.log(Session("staffid"), "An exemption for " & cboDay2.Text & " was added for " & cboTeacher.Text)
            Dim a As New DataTable
            a.Columns.Add("teacher")
            a.Columns.Add("day")
            a.Columns.Add("id")
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname, texemptions.day, texemptions.id from texemptions inner join staffprofile on staffprofile.staffid = texemptions.teacher where texemptions.timetable = '" & Session("timetable") & "'", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Do While msg.Read()
                a.Rows.Add(msg(0), msg.Item(1).ToString, msg.Item(2).ToString)
            Loop
            Gridview5.DataSource = a

            Gridview5.DataBind()
            msg.Close()
            panel5.Visible = False
             logify.log(Session("staffid"), "An exemption was added to " & cboTeacher.Text & " on " & cboDay2.Text )
            Show_Alert(True, "Exemption added successfully")
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub LinkButton4_Click(sender As Object, e As EventArgs) Handles LinkButton4.Click
        panel5.Visible = True
        cboday2.Text = "Select Day"
        cboTeacher.Text = "Select Teacher"
    End Sub

    Protected Sub Gridview5_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview5.RowDeleting
        Try
            Dim row As GridViewRow = Gridview5.Rows(e.RowIndex)
            Dim teacher As String = row.Cells(0).Text
            Dim day As String = row.Cells(1).Text

            con.Open()
            Dim tid As String
            Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffid from staffprofile Where surname = ?", con)
            cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", teacher))
            Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
            reader2a.Read()
            tid = reader2a.Item(0)
            reader2a.Close()
            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("delete from texemptions where teacher = '" & tid & "' and day = '" & day & "' and texemptions.timetable = '" & Session("timetable") & "'", con)
            cmd3.ExecuteNonQuery()
            logify.log(Session("staffid"), "Exemption for " & day & " was removed for " & teacher)

            Dim a As New DataTable
            a.Columns.Add("teacher")
            a.Columns.Add("day")
            a.Columns.Add("id")
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname, texemptions.day, texemptions.id from texemptions inner join staffprofile on staffprofile.staffid = texemptions.teacher where texemptions.timetable = '" & Session("timetable") & "'", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Do While msg.Read()
                a.Rows.Add(msg(0), msg.Item(1).ToString, msg.Item(2).ToString)
            Loop
            Gridview5.DataSource = a

            Gridview5.DataBind()
            msg.Close()
            Show_Alert(True, "Exemption deleted successfully")
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        panel5.Visible = False
    End Sub

    Protected Sub Gridview2_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles Gridview2.SelectedIndexChanging
        Try

      
        con.Open()
        Panel2.Visible = True
            Dim group As String = Gridview2.Rows(e.NewSelectedIndex).Cells(0).Text
            Session("updatesgroup") = group
        txtGrp.Text = group
        Button1.Text = "Update"
        Dim cmdf As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, class.class from classsubjects inner join tsgroups on tsgroups.id = classsubjects.sgroup inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.id where tsgroups.name = ?", con)
        cmdf.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subct", group))
        Dim readerx As MySql.Data.MySqlClient.MySqlDataReader = cmdf.ExecuteReader
        Do While readerx.Read
            For Each item As ListItem In CheckBoxList2.Items
                If item.Text = readerx.Item(0).ToString Then item.Selected = True
            Next
            For Each sitem As ListItem In chkClasses.Items
                If sitem.Text = readerx.Item(1).ToString Then sitem.Selected = True
            Next
        Loop
        readerx.Close()
        Dim a As New DataTable
        a.Columns.Add("name")
        a.Columns.Add("id")

        Dim cmdCheck22 As New MySql.Data.MySqlClient.MySqlCommand("Select name, id from tsgroups", con)
        Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck22.ExecuteReader
        Do While msg2.Read()
            a.Rows.Add(msg2.Item(0), msg2.Item(1))
        Loop
        Gridview2.DataSource = a

        Gridview2.DataBind()
        msg2.Close()


            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview3_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles Gridview3.SelectedIndexChanging
        Try
            con.Open()
            Panel3.Visible = True
            Dim group As String = Gridview3.Rows(e.NewSelectedIndex).Cells(0).Text
            txtCGrp.Text = group
            Session("updatecgroup") = group
            Button3.Text = "Update"
            Dim cmdf As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, class.class from classsubjects inner join tcgroups on tcgroups.id = classsubjects.cgroup inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.id where tcgroups.name = ?", con)
            cmdf.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subct", group))
            Dim readerx As MySql.Data.MySqlClient.MySqlDataReader = cmdf.ExecuteReader
            Do While readerx.Read
                For Each item As ListItem In chkCGroup.Items
                    If item.Text = readerx.Item(1).ToString Then item.Selected = True
                Next
            Loop
            readerx.Close()
            con.Close()
            Load_Together_Subjects()
            Dim cmdfs As New MySql.Data.MySqlClient.MySqlCommand("SELECT tsgroups.name from classsubjects inner join tcgroups on tcgroups.id = classsubjects.cgroup inner join tsgroups on tsgroups.id = classsubjects.sgroup where tcgroups.name = ? and classsubjects.sgroup <> ?", con)
            cmdfs.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subct", group))
            cmdfs.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SutReg.Subct", ""))
            Dim readery As MySql.Data.MySqlClient.MySqlDataReader = cmdfs.ExecuteReader
            Do While readery.Read
                For Each sitem As ListItem In chkCSSgroup.Items
                    If sitem.Text = readery.Item(0).ToString Then sitem.Selected = True
                Next
            Loop
            readery.Close()

            Dim cmdfss As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject from classsubjects inner join tcgroups on tcgroups.id = classsubjects.cgroup inner join subjects on subjects.id = classsubjects.subject where tcgroups.name = ? and classsubjects.sgroup = ?", con)
            cmdfss.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subct", group))
            cmdfss.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SutReg.Subct", ""))
            Dim readersy As MySql.Data.MySqlClient.MySqlDataReader = cmdfss.ExecuteReader
            Do While readersy.Read
                For Each sitem As ListItem In chkCSgroup.Items
                    If sitem.Text = readersy.Item(0).ToString Then sitem.Selected = True
                Next
            Loop
            readersy.Close()
            chkCGroup.Enabled = False
            Dim a As New DataTable
            a.Columns.Add("name")
            a.Columns.Add("id")

            Dim cmdCheck21 As New MySql.Data.MySqlClient.MySqlCommand("Select name, id from tcgroups", con)
            Dim msg1 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck21.ExecuteReader
            Do While msg1.Read()
                a.Rows.Add(msg1.Item(0), msg1.Item(1))
            Loop
            Gridview3.DataSource = a

            Gridview3.DataBind()
            msg1.Close()

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles Gridview1.SelectedIndexChanging
        Try
            con.Open()
            panel1.Visible = True
            Dim x As Array = Split(Gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.*, classperiods.activity From tperiods left join classperiods on classperiods.period = tperiods.id Where tperiods.timestart = '" & x(0) & "' and tperiods.timeend = '" & x(1) & "' and tperiods.day = '" & cboDay.Text & "'", con)
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
            reader3.Read()
            Session("pupdate") = reader3("id").ToString
            cboDay.Text = reader3.Item("day").ToString
            txtStart.Text = reader3.Item("timestart").ToString
            txtEnd.Text = reader3.Item("timeend").ToString
            txtActivity.Text = reader3.Item(5).ToString
            Dim periodid As String = reader3("ID").ToString
            Dim secactivity As String = reader3(7).ToString
            reader3.Close()
            If txtActivity.Text = "Tutorial" Then
                chkTutorial.Checked = True
                txtActivity.Enabled = False
                Dim cmd2x As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from classperiods inner join class on class.id = classperiods.class where classperiods.period = '" & periodid & "'", con)
                Dim reader3x As MySql.Data.MySqlClient.MySqlDataReader = cmd2x.ExecuteReader
                chkClassPeriods.Checked = False
                Do While reader3x.Read()
                    For Each item As ListItem In Chkclasss.Items
                        If item.Text = reader3x(0).ToString Then item.Selected = True
                    Next
                    chkClassPeriods.Checked = True
                    chkClassPeriods.Visible = True
                    chkTutorial.Checked = False
                    pnlClassActivity.Visible = True
                    txtActivity.Enabled = True
                    txtActivity.Text = secactivity
                Loop
            Else
                chkTutorial.Checked = False
                txtActivity.Enabled = True
            End If
            reader3.Close()
            bnAdd.Text = "Update"
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub Gridview4_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles Gridview4.SelectedIndexChanging
        Try
            Dim subject As String = Gridview4.Rows(e.NewSelectedIndex).Cells(0).Text
            Dim clas As String = Gridview4.Rows(e.NewSelectedIndex).Cells(1).Text
            con.Open()
            Dim cmdCheck2s As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject, class.class, classsubjects.doubles, classsubjects.id from classsubjects inner join subjects on classsubjects.subject = subjects.id inner join class on classsubjects.class = class.id where subjects.subject = '" & subject & "' and class.class = '" & clas & "'", con)
            Dim msgs As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2s.ExecuteReader
            msgs.Read()
            panel4.Visible = True
            cboSubject.Text = msgs(0).ToString
            cboClasses.Items.Add(msgs(1).ToString)
            txtDouble.Text = msgs(2).ToString
            cboSubject.Enabled = False
            Button5.Text = "Update"
            cboClasses.Enabled = False
            msgs.Close()
            Dim a As New DataTable
            a.Columns.Add("subject")
            a.Columns.Add("class")
            a.Columns.Add("amount")
            a.Columns.Add("id")
            Dim cmdCheck21 As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject, class.class, classsubjects.doubles, classsubjects.id from classsubjects inner join subjects on classsubjects.subject = subjects.id inner join class on classsubjects.class = class.id where classsubjects.doubles <> '" & 0 & "' order by classsubjects.doubles desc", con)
            Dim msg1 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck21.ExecuteReader
            Do While msg1.Read()
                a.Rows.Add(msg1(0), msg1.Item(1).ToString, msg1.Item(2).ToString, msg1.Item(3))
            Loop
            Gridview4.DataSource = a

            Gridview4.DataBind()
            msg1.Close()
            con.Close()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

  

    Protected Sub radType_SelectedIndexChanged(sender As Object, e As EventArgs) Handles radType.SelectedIndexChanged
        If radType.SelectedValue = "School Based" Then
            pnlSchool.Visible = True
            pnlClass.Visible = False
        Else
            pnlSchool.Visible = False
            pnlClass.Visible = True
            Wizard1.StartNextButtonText = "FINISH"
        End If
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        con.Open()

        Dim cmdLoad0x As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject from classsubjects inner join class on class.id = classsubjects.class inner join subjects on subjects.id = classsubjects.subject where class.class = '" & DropDownList1.Text & "'", con)
        Dim reader0x As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0x.ExecuteReader
        cboManualSubjects.Items.Clear()
        cboManualSubjects.Items.Add("SELECT SUBJECT")
        Do While reader0x.Read
            cboManualSubjects.Items.Add(reader0x(0))
        Loop
        reader0x.Read()
        con.Close()
        load_time_table()
        pnlManual.Visible = True
        DropDownList1.Enabled = False
    End Sub

    Protected Sub ChkManualTutorial_CheckedChanged(sender As Object, e As EventArgs) Handles ChkManualTutorial.CheckedChanged
        If ChkManualTutorial.Checked Then
            txtManualTutorial.Enabled = False
            txtManualTutorial.Text = "Tutorial"
            pnlManualDetails.Visible = True
        Else
            txtManualTutorial.Enabled = True
            txtManualTutorial.Text = ""
            pnlManualDetails.Visible = False
        End If
    End Sub

    Protected Sub BtnManualAdd_Click(sender As Object, e As EventArgs) Handles BtnManualAdd.Click
        Try

       
        If DropDownList1.Text = "SELECT CLASS" Or txtID.Text = "" Or cboDay3.Text = "SELECT DAY" Or txtStartManual.Text = "" Or txtEndManual.Text = "" Then
            Show_Alert(False, "Enter missing items")
            Exit Sub
        Else
                con.Open()
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, superior from class where class = '" & DropDownList1.Text & "'", con)
                Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                reader0.Read()
                Dim clas As Integer = reader0(0)
                Dim superior As Integer = reader0(1)
                reader0.Close()

                Dim cmdf0s As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from ttname Where school = ?", con)
                cmdf0s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", superior))
                Dim readerf0s As MySql.Data.MySqlClient.MySqlDataReader = cmdf0s.ExecuteReader
                readerf0s.Read()
                Dim schooltt As Integer = readerf0s(0)
                readerf0s.Close()
                Dim cmd3f As New MySql.Data.MySqlClient.MySqlCommand("delete from timetable where tname = ?", con)
                cmd3f.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", schooltt))
                cmd3f.ExecuteNonQuery()

            Dim cmdf0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from ttname Where name = ?", con)
            cmdf0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", txtID.Text))
            Dim readerf0 As MySql.Data.MySqlClient.MySqlDataReader = cmdf0.ExecuteReader
            If readerf0.Read() Then
                Else
                    readerf0.Close()
                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into ttname (name, class) Values (?,?)", con)
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", txtID.Text))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Css", clas))

                    cmd3.ExecuteNonQuery()
            End If
            readerf0.Close()
            Dim cmd35 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id From ttname where name = '" & txtID.Text & "'", con)
            Dim reader35 As MySql.Data.MySqlClient.MySqlDataReader = cmd35.ExecuteReader
            reader35.Read()
            Session("timetable") = reader35.Item(0)
            reader35.Close()

        End If
        If ChkManualTutorial.Checked = True Then
            If cboManualSubjects.Text = "SELECT SUBJECT" Then
                Show_Alert(False, "Select a Subject")
                Exit Sub
            Else
                Dim cmd3s As New MySql.Data.MySqlClient.MySqlCommand("select id from tperiods where timetable = '" & Session("timetable") & "' and day = '" & cboDay3.Text & "' and timestart = '" & txtStartManual.Text & "' and timeend = '" & txtEndManual.Text & "'", con)
                Dim reader3s As MySql.Data.MySqlClient.MySqlDataReader = cmd3s.ExecuteReader
                Dim period As Integer = 0
                If reader3s.Read Then
                    period = reader3s(0)
                    reader3s.Close()
                Else
                    reader3s.Close()
                    Dim cmd3z As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into tperiods (timetable, day, timestart, timeend, activity) Values ('" & Session("timetable") & "', '" & cboDay3.Text & "', '" & txtStartManual.Text & "', '" & txtEndManual.Text & "', '" & txtManualTutorial.Text & "')", con)
                    cmd3z.ExecuteNonQuery()
                        Dim cmd3w As New MySql.Data.MySqlClient.MySqlCommand("select id from tperiods where timetable = '" & Session("timetable") & "' and day = '" & cboDay3.Text & "' and timestart = '" & txtStartManual.Text & ":00" & "' and timeend = '" & txtEndManual.Text & ":00" & "'", con)
                    Dim reader3w As MySql.Data.MySqlClient.MySqlDataReader = cmd3w.ExecuteReader
                    reader3w.Read()
                    period = reader3w(0)
                    reader3w.Close()
                End If

                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.subject, classsubjects.class, classsubjects.teacher from classsubjects inner join class on class.id = classsubjects.class inner join subjects on subjects.id = classsubjects.subject where class.class = '" & DropDownList1.Text & "' and subjects.subject = '" & cboManualSubjects.Text & "'", con)
                    Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    reader0.Read()
                    Dim subjects As Integer = reader0(0)
                    Dim clas As Integer = reader0(1)
                    Dim teacher As String = reader0(2)
                    reader0.Close()
                    Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.activity  from tperiods where timetable = '" & Session("timetable") & "' and day = '" & cboDay3.Text & "'order by tperiods.timestart", con)
                    Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                    Dim tperiod As Integer = 0
                    Do While student10.Read
                        If student10(0).ToString <> "Tutorial" And period = tperiod Then
                            Exit Do
                        End If
                        tperiod = tperiod + 1
                    Loop
                    student10.Close()
                Dim cmdInsert35 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into timetable (tname, Class, Period, Subject, Day, Teacher) Values(?,?,?,?,?,?)", con)
                cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Session("timetable")))
                cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", clas))
                    cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", tperiod))
                cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", subjects))
                cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Doubles", cboDay3.Text))
                cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", teacher))
                cmdInsert35.ExecuteNonQuery()

                Dim cmdInsert37 As New MySql.Data.MySqlClient.MySqlCommand("update class set timetable = '" & Session("timetable") & "' where id = '" & clas & "'", con)
                cmdInsert37.ExecuteNonQuery()
            End If
           
        Else
                Dim cmd3s As New MySql.Data.MySqlClient.MySqlCommand("select id from tperiods where timetable = '" & Session("timetable") & "' and day = '" & cboDay3.Text & "' and timestart = '" & txtStartManual.Text & ":00" & "' and timeend = '" & txtEndManual.Text & ":00" & "'", con)
            Dim reader3s As MySql.Data.MySqlClient.MySqlDataReader = cmd3s.ExecuteReader
            Dim period As Integer = 0
            If reader3s.Read Then
                Show_Alert(False, "Period already exists")
                Exit Sub
            Else
                reader3s.Close()
                Dim cmd3z As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into tperiods (timetable, day, timestart, timeend, activity) Values ('" & Session("timetable") & "', '" & cboDay3.Text & "', '" & txtStartManual.Text & "', '" & txtEndManual.Text & "', '" & txtManualTutorial.Text & "')", con)
                cmd3z.ExecuteNonQuery()
            End If




            End If
            con.Close()
            load_time_table()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Private Sub load_time_table()
        con.Open()
        Dim days As New ArrayList
        days.Add("Monday")
        days.Add("Tuesday")
        days.Add("Wednesday")
        days.Add("Thursday")
        days.Add("Friday")

        GridView6.AutoGenerateColumns = True
        GridView6.ShowHeader = False

        Dim ds As New DataTable
        ds.Columns.Clear()
        ds.Rows.Clear()
        ds.Columns.Add("Day")
        ds.Rows.Add("")
        ds.Rows.Add("Monday")
        ds.Rows.Add("")
        ds.Rows.Add("Tuesday")
        ds.Rows.Add("")
        ds.Rows.Add("Wednesday")
        ds.Rows.Add("")
        ds.Rows.Add("Thursday")
        ds.Rows.Add("")
        ds.Rows.Add("Friday")
        Dim x As Integer = -2
        Dim rowend As Integer = 1
        Dim firstday As Boolean = True
        For Each Day As String In days
            x = x + 2
            Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.timestart, tperiods.timeend, tperiods.activity  from tperiods where timetable = '" & Session("timetable") & "' and day = '" & Day & "'order by tperiods.timestart", con)
            Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
            Dim j As Integer = 1
            Dim nontutorial As New ArrayList
            Do While student10.Read
                If firstday = True Then
                    If student10(2).ToString = "Tutorial" Then
                        ds.Columns.Add(student10(0).ToString & " - " & student10(1).ToString)
                        ds.Rows(x).Item(j) = student10(0).ToString & " - " & student10(1).ToString
                        j = j + 1
                    Else
                        ds.Columns.Add(student10(0).ToString & " - " & student10(1).ToString)
                        ds.Rows(x).Item(j) = student10(0).ToString & " - " & student10(1).ToString
                        ds.Rows(x + 1).Item(j) = student10(2).ToString
                        nontutorial.Add(j)
                        j = j + 1

                    End If
                    rowend = rowend + 1
                Else
                    If j < rowend Then
                        If student10(2).ToString = "Tutorial" Then
                            ds.Rows(x).Item(j) = student10(0).ToString & " - " & student10(1).ToString
                            j = j + 1
                        Else
                            ds.Rows(x).Item(j) = student10(0).ToString & " - " & student10(1).ToString
                            ds.Rows(x + 1).Item(j) = student10(2).ToString
                            nontutorial.Add(j)
                            j = j + 1
                        End If
                    Else
                        If student10(2).ToString = "Tutorial" Then
                            ds.Columns.Add("")
                            ds.Rows(x).Item(j) = student10(0).ToString & " - " & student10(1).ToString
                            j = j + 1
                            rowend = rowend + 1
                        Else
                            ds.Rows(x).Item(j) = student10(0).ToString & " - " & student10(1).ToString
                            ds.Rows(x + 1).Item(j) = student10(2).ToString
                            nontutorial.Add(j)
                            j = j + 1
                            rowend = rowend + 1
                        End If

                    End If


                End If
            Loop
            student10.Close()


            Dim enterred As New ArrayList
            Dim cmdLo As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.alias, staffprofile.surname, timetable.period from timetable inner join class on timetable.class = class.id inner join subjects on timetable.subject = subjects.id inner join staffprofile on staffprofile.staffid = timetable.teacher where class.class = '" & DropDownList1.Text & "' and timetable.day = '" & Day & "' and timetable.tname = '" & Session("timetable") & "' order by timetable.period", con)
            Dim studen As MySql.Data.MySqlClient.MySqlDataReader = cmdLo.ExecuteReader
            Dim hx As Integer = 1
            Do While studen.Read
                If Not nontutorial.Contains(hx) Then
                    If Not enterred.Contains(studen(2).ToString) Then
                        ds(x + 1).Item(hx) = studen(0).ToString
                        enterred.Add(studen(2).ToString)
                        hx = hx + 1
                    Else
                        ds(x + 1).Item(hx - 1) = ds(x + 1).Item(hx - 1) & " / " & studen(0).ToString
                    End If
                Else
                    If Not enterred.Contains(studen(2).ToString) Then
                        hx = hx + 1
                        ds(x + 1).Item(hx) = studen(0).ToString
                        enterred.Add(studen(2).ToString)
                        hx = hx + 1
                    Else
                        ds(x + 1).Item(hx - 1) = ds(x + 1).Item(hx - 1) & " / " & studen(0).ToString
                    End If

                End If
            Loop
            studen.Close()
            hx = Nothing
            enterred = Nothing
            nontutorial = Nothing
            j = Nothing
            firstday = False
        Next

        GridView6.DataSource = ds
        GridView6.DataBind()


        con.Close()


    End Sub

    Protected Sub chkClassPeriods_CheckedChanged(sender As Object, e As EventArgs) Handles chkClassPeriods.CheckedChanged
        If chkClassPeriods.Checked = False Then
            pnlClassActivity.Visible = False
        Else
            pnlClassActivity.Visible = True
            For Each item As ListItem In Chkclasss.Items
                item.Selected = False
            Next
        End If
    End Sub
End Class
