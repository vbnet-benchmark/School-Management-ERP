Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_allstudents
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
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try


            If Not IsPostBack Then
                Dim staffs As New ArrayList
                DropDownList1.Items.Clear()
                DropDownList1.Items.Add("All")
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
                            Dim classes11 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname from staffprofile where staffprofile.staffid = '" & sitem & "'  and staffprofile.activated = '" & 1 & "'", con)
                            Dim schclass11 As MySql.Data.MySqlClient.MySqlDataReader = classes11.ExecuteReader
                            Do While schclass11.Read
                                If Not staffs.Contains(schclass11(0)) Then
                                    staffs.Add(schclass11(0))
                                    DropDownList1.Items.Add(schclass11.Item(0).ToString)
                                End If
                            Loop
                            schclass11.Close()
                        Next
                    Next

                    Dim ds As New DataTable
                    ds.Columns.Add("id")
                    ds.Columns.Add("date")
                    ds.Columns.Add("sender")
                    ds.Columns.Add("receiver")
                    ds.Columns.Add("sendertype")
                    ds.Columns.Add("subject")
                    ds.Columns.Add("status")


                    For Each staff As String In staffs
                        Dim id As New ArrayList
                        Dim dates As New ArrayList
                        Dim sendere As New ArrayList
                        Dim receiver As New ArrayList
                        Dim sendertype As New ArrayList
                        Dim subject As New ArrayList
                        Dim status As New ArrayList
                        Dim sendername As New ArrayList
                        Dim receivername As New ArrayList
                        Dim senderrel As New ArrayList
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT messages.id, cast(messages.date as char) as date, messages.sender, staffprofile.surname, messages.sendertype, messages.subject, messages.status from messages inner join staffprofile on staffprofile.staffid = messages.receiver  inner join (classteacher inner join (class inner join depts on depts.id = class.superior) on class.id = classteacher.class) on messages.receiver = classteacher.teacher where staffprofile.surname = '" & staff & "'  order by date desc", con)
                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Do While reader.Read
                            id.Add(reader.Item(0))
                            dates.Add(reader.Item(1))
                            sendere.Add(reader.Item(2))
                            receiver.Add(reader.Item(3))
                            sendertype.Add(reader.Item(4))
                            subject.Add(reader.Item(5))
                            status.Add(reader.Item(6))
                        Loop
                        reader.Close()
                        Dim x As Integer = 0
                        For Each item As String In id
                            If sendertype(x) = "Student" Then
                                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                                student.Read()
                                sendername.Add(student.Item(0))
                                student.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Staff" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                student1.Read()
                                sendername.Add(student1.Item(0))
                                student1.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Parent" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                student1.Read()
                                sendername.Add(student1.Item(0))
                                student1.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Admin" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                If student1.Read() Then
                                    sendername.Add(student1.Item(0))
                                Else
                                    sendername.Add("Super Admin")
                                End If
                                student1.Close()

                                senderrel.Add("Admin")
                            ElseIf sendertype(x) = "Accounts" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                If student1.Read() Then
                                    sendername.Add(student1.Item(0))
                                Else
                                    sendername.Add("Super Admin")
                                End If
                                student1.Close()
                                senderrel.Add("Accountant")
                            End If
                            ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), receiver(x), senderrel(x), subject(x), status(x))
                            x = x + 1
                        Next
                    Next


                    GridView1.DataSource = ds
                    GridView1.DataBind()
                    If GridView1.PageIndex = GridView1.PageCount - 1 Then
                        btnNext.Visible = False
                    Else
                        btnNext.Visible = True
                    End If
                    If GridView1.PageIndex = 0 Then
                        btnPrevious.Visible = False
                    Else
                        btnPrevious.Visible = True
                    End If



                    con.Close()                End Using
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
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

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If DropDownList1.Text = "All" Then
                    Dim ds As New DataTable
                    ds.Columns.Add("id")
                    ds.Columns.Add("date")
                    ds.Columns.Add("sender")
                    ds.Columns.Add("receiver")
                    ds.Columns.Add("sendertype")
                    ds.Columns.Add("subject")
                    ds.Columns.Add("status")


                    For Each staff As ListItem In DropDownList1.Items
                        Dim id As New ArrayList
                        Dim dates As New ArrayList
                        Dim sendere As New ArrayList
                        Dim receiver As New ArrayList
                        Dim sendertype As New ArrayList
                        Dim subject As New ArrayList
                        Dim status As New ArrayList
                        Dim sendername As New ArrayList
                        Dim receivername As New ArrayList
                        Dim senderrel As New ArrayList
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT messages.id, messages.date as date, messages.sender, staffprofile.surname, messages.sendertype, messages.subject, messages.status from messages inner join staffprofile on staffprofile.staffid = messages.receiver  inner join (classteacher inner join (class inner join depts on depts.id = class.superior) on class.id = classteacher.class) on messages.receiver = classteacher.teacher where staffprofile.surname = '" & staff.Text & "' order by date desc", con)
                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Do While reader.Read
                            id.Add(reader.Item(0))
                            dates.Add(reader.Item(1))
                            sendere.Add(reader.Item(2))
                            receiver.Add(reader.Item(3))
                            sendertype.Add(reader.Item(4))
                            subject.Add(reader.Item(5))
                            status.Add(reader.Item(6))
                        Loop
                        reader.Close()
                        Dim x As Integer = 0
                        For Each item As String In id
                            If sendertype(x) = "Student" Then
                                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                                student.Read()
                                sendername.Add(student.Item(0))
                                student.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Staff" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                student1.Read()
                                sendername.Add(student1.Item(0))
                                student1.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Parent" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                student1.Read()
                                sendername.Add(student1.Item(0))
                                student1.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Admin" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                If student1.Read() Then
                                    sendername.Add(student1.Item(0))
                                Else
                                    sendername.Add("Super Admin")
                                End If
                                student1.Close()

                                senderrel.Add("Admin")
                            ElseIf sendertype(x) = "Accounts" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                If student1.Read() Then
                                    sendername.Add(student1.Item(0))
                                Else
                                    sendername.Add("Super Admin")
                                End If
                                student1.Close()
                                senderrel.Add("Accountant")
                            End If
                            ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), receiver(x), senderrel(x), subject(x), status(x))
                            x = x + 1
                        Next
                    Next

                    GridView1.DataSource = ds
                    GridView1.DataBind()
                    If GridView1.PageIndex = GridView1.PageCount - 1 Then
                        btnNext.Visible = False
                    Else
                        btnNext.Visible = True
                    End If
                    If GridView1.PageIndex = 0 Then
                        btnPrevious.Visible = False
                    Else
                        btnPrevious.Visible = True
                    End If




                Else

                    Dim ds As New DataTable
                    ds.Columns.Add("id")
                    ds.Columns.Add("date")
                    ds.Columns.Add("sender")
                    ds.Columns.Add("receiver")
                    ds.Columns.Add("sendertype")
                    ds.Columns.Add("subject")
                    ds.Columns.Add("status")

                    Dim id As New ArrayList
                    Dim dates As New ArrayList
                    Dim sendere As New ArrayList
                    Dim receiver As New ArrayList
                    Dim sendertype As New ArrayList
                    Dim subject As New ArrayList
                    Dim status As New ArrayList
                    Dim sendername As New ArrayList
                    Dim receivername As New ArrayList
                    Dim senderrel As New ArrayList
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT messages.id, messages.date, messages.sender, staffprofile.surname, messages.sendertype, messages.subject, messages.status from messages inner join staffprofile on staffprofile.staffid = messages.receiver inner join (classteacher inner join (class inner join depts on depts.id = class.superior) on class.id = classteacher.class) on messages.receiver = classteacher.teacher where staffprofile.surname = '" & DropDownList1.Text & "' and messages.subject order by date desc", con)
                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Do While reader.Read
                        id.Add(reader.Item(0))
                        dates.Add(reader.Item(1))
                        sendere.Add(reader.Item(2))
                        receiver.Add(reader.Item(3))
                        sendertype.Add(reader.Item(4))
                        subject.Add(reader.Item(5))
                        status.Add(reader.Item(6))
                    Loop
                    reader.Close()
                    Dim x As Integer = 0
                    For Each item As String In id
                        If sendertype(x) = "Student" Then
                            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                            student.Read()
                            sendername.Add(student.Item(0))
                            student.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Staff" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            student1.Read()
                            sendername.Add(student1.Item(0))
                            student1.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Parent" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            student1.Read()
                            sendername.Add(student1.Item(0))
                            student1.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Admin" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            If student1.Read() Then
                                sendername.Add(student1.Item(0))
                            Else
                                sendername.Add("Super Admin")
                            End If
                            student1.Close()

                            senderrel.Add("Admin")
                        ElseIf sendertype(x) = "Accounts" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            If student1.Read() Then
                                sendername.Add(student1.Item(0))
                            Else
                                sendername.Add("Super Admin")
                            End If
                            student1.Close()
                            senderrel.Add("Accountant")
                        End If
                        ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), receiver(x), senderrel(x), subject(x), status(x))
                        x = x + 1
                    Next
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                End If
                con.close()            End Using
            If GridView1.PageIndex + 1 <= GridView1.PageCount Then
                GridView1.PageIndex = GridView1.PageIndex + 1
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If DropDownList1.Text = "All" Then
                    Dim ds As New DataTable
                    ds.Columns.Add("id")
                    ds.Columns.Add("date")
                    ds.Columns.Add("sender")
                    ds.Columns.Add("receiver")
                    ds.Columns.Add("sendertype")
                    ds.Columns.Add("subject")
                    ds.Columns.Add("status")


                    For Each staff As ListItem In DropDownList1.Items
                        Dim id As New ArrayList
                        Dim dates As New ArrayList
                        Dim sendere As New ArrayList
                        Dim receiver As New ArrayList
                        Dim sendertype As New ArrayList
                        Dim subject As New ArrayList
                        Dim status As New ArrayList
                        Dim sendername As New ArrayList
                        Dim receivername As New ArrayList
                        Dim senderrel As New ArrayList
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT messages.id, messages.date as date, messages.sender, staffprofile.surname, messages.sendertype, messages.subject, messages.status from messages inner join staffprofile on staffprofile.staffid = messages.receiver  inner join (classteacher inner join (class inner join depts on depts.id = class.superior) on class.id = classteacher.class) on messages.receiver = classteacher.teacher where staffprofile.surname = '" & staff.Text & "' order by date desc", con)
                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Do While reader.Read
                            id.Add(reader.Item(0))
                            dates.Add(reader.Item(1))
                            sendere.Add(reader.Item(2))
                            receiver.Add(reader.Item(3))
                            sendertype.Add(reader.Item(4))
                            subject.Add(reader.Item(5))
                            status.Add(reader.Item(6))
                        Loop
                        reader.Close()
                        Dim x As Integer = 0
                        For Each item As String In id
                            If sendertype(x) = "Student" Then
                                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                                student.Read()
                                sendername.Add(student.Item(0))
                                student.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Staff" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                student1.Read()
                                sendername.Add(student1.Item(0))
                                student1.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Parent" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                student1.Read()
                                sendername.Add(student1.Item(0))
                                student1.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Admin" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                If student1.Read() Then
                                    sendername.Add(student1.Item(0))
                                Else
                                    sendername.Add("Super Admin")
                                End If
                                student1.Close()

                                senderrel.Add("Admin")
                            ElseIf sendertype(x) = "Accounts" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                If student1.Read() Then
                                    sendername.Add(student1.Item(0))
                                Else
                                    sendername.Add("Super Admin")
                                End If
                                student1.Close()
                                senderrel.Add("Accountant")
                            End If
                            ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), receiver(x), senderrel(x), subject(x), status(x))
                            x = x + 1
                        Next
                    Next

                    GridView1.DataSource = ds
                    GridView1.DataBind()
                    If GridView1.PageIndex = GridView1.PageCount - 1 Then
                        btnNext.Visible = False
                    Else
                        btnNext.Visible = True
                    End If
                    If GridView1.PageIndex = 0 Then
                        btnPrevious.Visible = False
                    Else
                        btnPrevious.Visible = True
                    End If




                Else

                    Dim ds As New DataTable
                    ds.Columns.Add("id")
                    ds.Columns.Add("date")
                    ds.Columns.Add("sender")
                    ds.Columns.Add("receiver")
                    ds.Columns.Add("sendertype")
                    ds.Columns.Add("subject")
                    ds.Columns.Add("status")

                    Dim id As New ArrayList
                    Dim dates As New ArrayList
                    Dim sendere As New ArrayList
                    Dim receiver As New ArrayList
                    Dim sendertype As New ArrayList
                    Dim subject As New ArrayList
                    Dim status As New ArrayList
                    Dim sendername As New ArrayList
                    Dim receivername As New ArrayList
                    Dim senderrel As New ArrayList
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT messages.id, messages.date, messages.sender, staffprofile.surname, messages.sendertype, messages.subject, messages.status from messages inner join staffprofile on staffprofile.staffid = messages.receiver inner join (classteacher inner join (class inner join depts on depts.id = class.superior) on class.id = classteacher.class) on messages.receiver = classteacher.teacher where staffprofile.surname = '" & DropDownList1.Text & "' and messages.subject order by date desc", con)
                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Do While reader.Read
                        id.Add(reader.Item(0))
                        dates.Add(reader.Item(1))
                        sendere.Add(reader.Item(2))
                        receiver.Add(reader.Item(3))
                        sendertype.Add(reader.Item(4))
                        subject.Add(reader.Item(5))
                        status.Add(reader.Item(6))
                    Loop
                    reader.Close()
                    Dim x As Integer = 0
                    For Each item As String In id
                        If sendertype(x) = "Student" Then
                            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                            student.Read()
                            sendername.Add(student.Item(0))
                            student.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Staff" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            student1.Read()
                            sendername.Add(student1.Item(0))
                            student1.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Parent" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            student1.Read()
                            sendername.Add(student1.Item(0))
                            student1.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Admin" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            If student1.Read() Then
                                sendername.Add(student1.Item(0))
                            Else
                                sendername.Add("Super Admin")
                            End If
                            student1.Close()

                            senderrel.Add("Admin")
                        ElseIf sendertype(x) = "Accounts" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            If student1.Read() Then
                                sendername.Add(student1.Item(0))
                            Else
                                sendername.Add("Super Admin")
                            End If
                            student1.Close()
                            senderrel.Add("Accountant")
                        End If
                        ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), receiver(x), senderrel(x), subject(x), status(x))
                        x = x + 1
                    Next
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                End If
                con.close()            End Using
            GridView1.PageIndex = e.NewPageIndex
            GridView1.DataBind()

            If GridView1.PageIndex = GridView1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If GridView1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub GridView1_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub GridView1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs)

    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If DropDownList1.Text = "All" Then
                    Dim ds As New DataTable
                    ds.Columns.Add("id")
                    ds.Columns.Add("date")
                    ds.Columns.Add("sender")
                    ds.Columns.Add("receiver")
                    ds.Columns.Add("sendertype")
                    ds.Columns.Add("subject")
                    ds.Columns.Add("status")


                    For Each staff As ListItem In DropDownList1.Items
                        Dim id As New ArrayList
                        Dim dates As New ArrayList
                        Dim sendere As New ArrayList
                        Dim receiver As New ArrayList
                        Dim sendertype As New ArrayList
                        Dim subject As New ArrayList
                        Dim status As New ArrayList
                        Dim sendername As New ArrayList
                        Dim receivername As New ArrayList
                        Dim senderrel As New ArrayList
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT messages.id, messages.date as date, messages.sender, staffprofile.surname, messages.sendertype, messages.subject, messages.status from messages inner join staffprofile on staffprofile.staffid = messages.receiver  inner join (classteacher inner join (class inner join depts on depts.id = class.superior) on class.id = classteacher.class) on messages.receiver = classteacher.teacher where staffprofile.surname = '" & staff.Text & "' and messages.subject like '" & "%" & txtSearch.Text & "%" & "'  order by date desc", con)
                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Do While reader.Read
                            id.Add(reader.Item(0))
                            dates.Add(reader.Item(1))
                            sendere.Add(reader.Item(2))
                            receiver.Add(reader.Item(3))
                            sendertype.Add(reader.Item(4))
                            subject.Add(reader.Item(5))
                            status.Add(reader.Item(6))
                        Loop
                        reader.Close()
                        Dim x As Integer = 0
                        For Each item As String In id
                            If sendertype(x) = "Student" Then
                                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                                student.Read()
                                sendername.Add(student.Item(0))
                                student.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Staff" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                student1.Read()
                                sendername.Add(student1.Item(0))
                                student1.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Parent" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                student1.Read()
                                sendername.Add(student1.Item(0))
                                student1.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Admin" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                If student1.Read() Then
                                    sendername.Add(student1.Item(0))
                                Else
                                    sendername.Add("Super Admin")
                                End If
                                student1.Close()

                                senderrel.Add("Admin")
                            ElseIf sendertype(x) = "Accounts" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                If student1.Read() Then
                                    sendername.Add(student1.Item(0))
                                Else
                                    sendername.Add("Super Admin")
                                End If
                                student1.Close()
                                senderrel.Add("Accountant")
                            End If
                            ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), receiver(x), senderrel(x), subject(x), status(x))
                            x = x + 1
                        Next
                    Next

                    GridView1.DataSource = ds
                    GridView1.DataBind()
                    If GridView1.PageIndex = GridView1.PageCount - 1 Then
                        btnNext.Visible = False
                    Else
                        btnNext.Visible = True
                    End If
                    If GridView1.PageIndex = 0 Then
                        btnPrevious.Visible = False
                    Else
                        btnPrevious.Visible = True
                    End If




                Else

                    Dim ds As New DataTable
                    ds.Columns.Add("id")
                    ds.Columns.Add("date")
                    ds.Columns.Add("sender")
                    ds.Columns.Add("receiver")
                    ds.Columns.Add("sendertype")
                    ds.Columns.Add("subject")
                    ds.Columns.Add("status")

                    Dim id As New ArrayList
                    Dim dates As New ArrayList
                    Dim sendere As New ArrayList
                    Dim receiver As New ArrayList
                    Dim sendertype As New ArrayList
                    Dim subject As New ArrayList
                    Dim status As New ArrayList
                    Dim sendername As New ArrayList
                    Dim receivername As New ArrayList
                    Dim senderrel As New ArrayList
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT messages.id, messages.date, messages.sender, staffprofile.surname, messages.sendertype, messages.subject, messages.status from messages inner join staffprofile on staffprofile.staffid = messages.receiver inner join (classteacher inner join (class inner join depts on depts.id = class.superior) on class.id = classteacher.class) on messages.receiver = classteacher.teacher where staffprofile.surname = '" & DropDownList1.Text & "' and messages.subject like '" & "%" & txtSearch.Text & "%" & "' order by date desc", con)
                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Do While reader.Read
                        id.Add(reader.Item(0))
                        dates.Add(reader.Item(1))
                        sendere.Add(reader.Item(2))
                        receiver.Add(reader.Item(3))
                        sendertype.Add(reader.Item(4))
                        subject.Add(reader.Item(5))
                        status.Add(reader.Item(6))
                    Loop
                    reader.Close()
                    Dim x As Integer = 0
                    For Each item As String In id
                        If sendertype(x) = "Student" Then
                            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                            student.Read()
                            sendername.Add(student.Item(0))
                            student.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Staff" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            student1.Read()
                            sendername.Add(student1.Item(0))
                            student1.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Parent" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            student1.Read()
                            sendername.Add(student1.Item(0))
                            student1.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Admin" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            If student1.Read() Then
                                sendername.Add(student1.Item(0))
                            Else
                                sendername.Add("Super Admin")
                            End If
                            student1.Close()

                            senderrel.Add("Admin")
                        ElseIf sendertype(x) = "Accounts" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            If student1.Read() Then
                                sendername.Add(student1.Item(0))
                            Else
                                sendername.Add("Super Admin")
                            End If
                            student1.Close()
                            senderrel.Add("Accountant")
                        End If
                        ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), receiver(x), senderrel(x), subject(x), status(x))
                        x = x + 1
                    Next
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If DropDownList1.Text = "All" Then
                    Dim ds As New DataTable
                    ds.Columns.Add("id")
                    ds.Columns.Add("date")
                    ds.Columns.Add("sender")
                    ds.Columns.Add("receiver")
                    ds.Columns.Add("sendertype")
                    ds.Columns.Add("subject")
                    ds.Columns.Add("status")


                    For Each staff As ListItem In DropDownList1.Items
                        Dim id As New ArrayList
                        Dim dates As New ArrayList
                        Dim sendere As New ArrayList
                        Dim receiver As New ArrayList
                        Dim sendertype As New ArrayList
                        Dim subject As New ArrayList
                        Dim status As New ArrayList
                        Dim sendername As New ArrayList
                        Dim receivername As New ArrayList
                        Dim senderrel As New ArrayList
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT messages.id, messages.date as date, messages.sender, staffprofile.surname, messages.sendertype, messages.subject, messages.status from messages inner join staffprofile on staffprofile.staffid = messages.receiver  inner join (classteacher inner join (class inner join depts on depts.id = class.superior) on class.id = classteacher.class) on messages.receiver = classteacher.teacher where staffprofile.surname = '" & staff.Text & "' order by date desc", con)
                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Do While reader.Read
                            id.Add(reader.Item(0))
                            dates.Add(reader.Item(1))
                            sendere.Add(reader.Item(2))
                            receiver.Add(reader.Item(3))
                            sendertype.Add(reader.Item(4))
                            subject.Add(reader.Item(5))
                            status.Add(reader.Item(6))
                        Loop
                        reader.Close()
                        Dim x As Integer = 0
                        For Each item As String In id
                            If sendertype(x) = "Student" Then
                                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                                student.Read()
                                sendername.Add(student.Item(0))
                                student.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Staff" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                student1.Read()
                                sendername.Add(student1.Item(0))
                                student1.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Parent" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                student1.Read()
                                sendername.Add(student1.Item(0))
                                student1.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Admin" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                If student1.Read() Then
                                    sendername.Add(student1.Item(0))
                                Else
                                    sendername.Add("Super Admin")
                                End If
                                student1.Close()

                                senderrel.Add("Admin")
                            ElseIf sendertype(x) = "Accounts" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                If student1.Read() Then
                                    sendername.Add(student1.Item(0))
                                Else
                                    sendername.Add("Super Admin")
                                End If
                                student1.Close()
                                senderrel.Add("Accountant")
                            End If
                            ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), receiver(x), senderrel(x), subject(x), status(x))
                            x = x + 1
                        Next
                    Next

                    GridView1.DataSource = ds
                    GridView1.DataBind()
                    If GridView1.PageIndex = GridView1.PageCount - 1 Then
                        btnNext.Visible = False
                    Else
                        btnNext.Visible = True
                    End If
                    If GridView1.PageIndex = 0 Then
                        btnPrevious.Visible = False
                    Else
                        btnPrevious.Visible = True
                    End If




                Else

                    Dim ds As New DataTable
                    ds.Columns.Add("id")
                    ds.Columns.Add("date")
                    ds.Columns.Add("sender")
                    ds.Columns.Add("receiver")
                    ds.Columns.Add("sendertype")
                    ds.Columns.Add("subject")
                    ds.Columns.Add("status")

                    Dim id As New ArrayList
                    Dim dates As New ArrayList
                    Dim sendere As New ArrayList
                    Dim receiver As New ArrayList
                    Dim sendertype As New ArrayList
                    Dim subject As New ArrayList
                    Dim status As New ArrayList
                    Dim sendername As New ArrayList
                    Dim receivername As New ArrayList
                    Dim senderrel As New ArrayList
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT messages.id, messages.date, messages.sender, staffprofile.surname, messages.sendertype, messages.subject, messages.status from messages inner join staffprofile on staffprofile.staffid = messages.receiver inner join (classteacher inner join (class inner join depts on depts.id = class.superior) on class.id = classteacher.class) on messages.receiver = classteacher.teacher where staffprofile.surname = '" & DropDownList1.Text & "' and messages.subject order by date desc", con)
                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Do While reader.Read
                        id.Add(reader.Item(0))
                        dates.Add(reader.Item(1))
                        sendere.Add(reader.Item(2))
                        receiver.Add(reader.Item(3))
                        sendertype.Add(reader.Item(4))
                        subject.Add(reader.Item(5))
                        status.Add(reader.Item(6))
                    Loop
                    reader.Close()
                    Dim x As Integer = 0
                    For Each item As String In id
                        If sendertype(x) = "Student" Then
                            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                            student.Read()
                            sendername.Add(student.Item(0))
                            student.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Staff" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            student1.Read()
                            sendername.Add(student1.Item(0))
                            student1.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Parent" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            student1.Read()
                            sendername.Add(student1.Item(0))
                            student1.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Admin" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            If student1.Read() Then
                                sendername.Add(student1.Item(0))
                            Else
                                sendername.Add("Super Admin")
                            End If
                            student1.Close()

                            senderrel.Add("Admin")
                        ElseIf sendertype(x) = "Accounts" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            If student1.Read() Then
                                sendername.Add(student1.Item(0))
                            Else
                                sendername.Add("Super Admin")
                            End If
                            student1.Close()
                            senderrel.Add("Accountant")
                        End If
                        ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), receiver(x), senderrel(x), subject(x), status(x))
                        x = x + 1
                    Next
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                End If
                con.close()            End Using
            If GridView1.PageIndex - 1 >= 0 Then
                GridView1.PageIndex = GridView1.PageIndex - 1
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Response.Redirect("~/content/app/Staff/ssentmsgs.aspx")
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If DropDownList1.Text = "All" Then
                    Dim ds As New DataTable
                    ds.Columns.Add("id")
                    ds.Columns.Add("date", GetType(System.DateTime))
                    ds.Columns.Add("sender")
                    ds.Columns.Add("receiver")
                    ds.Columns.Add("sendertype")
                    ds.Columns.Add("subject")
                    ds.Columns.Add("status")


                    For Each staff As ListItem In DropDownList1.Items
                        Dim id As New ArrayList
                        Dim dates As New ArrayList
                        Dim sendere As New ArrayList
                        Dim receiver As New ArrayList
                        Dim sendertype As New ArrayList
                        Dim subject As New ArrayList
                        Dim status As New ArrayList
                        Dim sendername As New ArrayList
                        Dim receivername As New ArrayList
                        Dim senderrel As New ArrayList
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT messages.id, messages.date as date, messages.sender, staffprofile.surname, messages.sendertype, messages.subject, messages.status from messages inner join staffprofile on staffprofile.staffid = messages.receiver  inner join (classteacher inner join (class inner join depts on depts.id = class.superior) on class.id = classteacher.class) on messages.receiver = classteacher.teacher where staffprofile.surname = '" & staff.Text & "'  order by date desc", con)
                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Do While reader.Read
                            id.Add(reader.Item(0))
                            dates.Add(reader.Item(1))
                            sendere.Add(reader.Item(2))
                            receiver.Add(reader.Item(3))
                            sendertype.Add(reader.Item(4))
                            subject.Add(reader.Item(5))
                            status.Add(reader.Item(6))
                        Loop
                        reader.Close()
                        Dim x As Integer = 0
                        For Each item As String In id
                            If sendertype(x) = "Student" Then
                                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                                student.Read()
                                sendername.Add(student.Item(0))
                                student.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Staff" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                student1.Read()
                                sendername.Add(student1.Item(0))
                                student1.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Parent" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                student1.Read()
                                sendername.Add(student1.Item(0))
                                student1.Close()
                                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                                Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                                student11.Read()
                                senderrel.Add(student11.Item(0))
                                student11.Close()
                            ElseIf sendertype(x) = "Admin" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                If student1.Read() Then
                                    sendername.Add(student1.Item(0))
                                Else
                                    sendername.Add("Super Admin")
                                End If
                                student1.Close()

                                senderrel.Add("Admin")
                            ElseIf sendertype(x) = "Accounts" Then
                                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                                If student1.Read() Then
                                    sendername.Add(student1.Item(0))
                                Else
                                    sendername.Add("Super Admin")
                                End If
                                student1.Close()
                                senderrel.Add("Accountant")
                            End If
                            ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), receiver(x), senderrel(x), subject(x), status(x))
                            x = x + 1
                        Next
                    Next

                    GridView1.DataSource = ds
                    GridView1.DataBind()
                    If GridView1.PageIndex = GridView1.PageCount - 1 Then
                        btnNext.Visible = False
                    Else
                        btnNext.Visible = True
                    End If
                    If GridView1.PageIndex = 0 Then
                        btnPrevious.Visible = False
                    Else
                        btnPrevious.Visible = True
                    End If




                Else

                    Dim ds As New DataTable
                    ds.Columns.Add("id")
                    ds.Columns.Add("date")
                    ds.Columns.Add("sender")
                    ds.Columns.Add("receiver")
                    ds.Columns.Add("sendertype")
                    ds.Columns.Add("subject")
                    ds.Columns.Add("status")

                    Dim id As New ArrayList
                    Dim dates As New ArrayList
                    Dim sendere As New ArrayList
                    Dim receiver As New ArrayList
                    Dim sendertype As New ArrayList
                    Dim subject As New ArrayList
                    Dim status As New ArrayList
                    Dim sendername As New ArrayList
                    Dim receivername As New ArrayList
                    Dim senderrel As New ArrayList
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT messages.id, messages.date, messages.sender, staffprofile.surname, messages.sendertype, messages.subject, messages.status from messages inner join staffprofile on staffprofile.staffid = messages.receiver inner join (classteacher inner join (class inner join depts on depts.id = class.superior) on class.id = classteacher.class) on messages.receiver = classteacher.teacher where staffprofile.surname = '" & DropDownList1.Text & "'  order by date desc", con)
                    Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Do While reader.Read
                        id.Add(reader.Item(0))
                        dates.Add(reader.Item(1))
                        sendere.Add(reader.Item(2))
                        receiver.Add(reader.Item(3))
                        sendertype.Add(reader.Item(4))
                        subject.Add(reader.Item(5))
                        status.Add(reader.Item(6))
                    Loop
                    reader.Close()
                    Dim x As Integer = 0
                    For Each item As String In id
                        If sendertype(x) = "Student" Then
                            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & sendere(x) & "'", con)
                            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                            student.Read()
                            sendername.Add(student.Item(0))
                            student.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Staff" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            student1.Read()
                            sendername.Add(student1.Item(0))
                            student1.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Parent" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentname from parentprofile where parentid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            student1.Read()
                            sendername.Add(student1.Item(0))
                            student1.Close()
                            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT sendertype from sentmsgs where id = '" & id(x) & "'  order by date desc", con)
                            Dim student11 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                            student11.Read()
                            senderrel.Add(student11.Item(0))
                            student11.Close()
                        ElseIf sendertype(x) = "Admin" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            If student1.Read() Then
                                sendername.Add(student1.Item(0))
                            Else
                                sendername.Add("Super Admin")
                            End If
                            student1.Close()

                            senderrel.Add("Admin")
                        ElseIf sendertype(x) = "Accounts" Then
                            Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & sendere(x) & "'", con)
                            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                            If student1.Read() Then
                                sendername.Add(student1.Item(0))
                            Else
                                sendername.Add("Super Admin")
                            End If
                            student1.Close()
                            senderrel.Add("Accountant")
                        End If
                        ds.Rows.Add(item, Convert.ToDateTime(dates(x)).ToString("dd/MM/yyyy hh:mm tt"), sendername(x), receiver(x), senderrel(x), subject(x), status(x))
                        x = x + 1
                    Next
                    GridView1.DataSource = ds
                    GridView1.AllowPaging = False
                    GridView1.DataBind()

                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
