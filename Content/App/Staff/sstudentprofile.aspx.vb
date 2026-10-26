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
        If check.Check_sh(Session("roles"), Session("usertype")) = False And check.Check_oh(Session("roles"), Session("usertype")) = False Then
            If check.Check_oh(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        End If
        Try
            If IsPostBack Then
            Else
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
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
                    DropDownList1.Items.Clear()
                    Dim clasadd As New ArrayList
                    For Each item As String In classcontroled
                        Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                        Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader

                        Do While schclass.Read
                            If Not clasadd.Contains(schclass(0)) Then
                                clasadd.Add(schclass(0))
                                DropDownList1.Items.Add(schclass.Item(0).ToString)
                            End If
                        Loop
                        schclass.Close()
                    Next
                    If Session("classselect") <> Nothing Then DropDownList1.Text = Session("classselect")
                    panel3.Visible = False
                    Dim ds As New DataTable
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.id WHERE class.class = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
                    Dim student0d As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                    Do While student0d.Read
                        ds.Rows.Add(student0d.Item(0).ToString, student0d.Item(1) & " - " & student0d.Item(2).ToString)
                    Loop
                    student0.Close()
                    gridview1.DataSource = ds

                    gridview1.DataBind()
                    con.close()                End Using
                If Session("studentadd") <> Nothing Then
                    Student_Details()
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
            Dim hostel As String = student(3).ToString
            Dim transport As String = student(5).ToString
            student.Close()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.admno as 'Admission number', studentsprofile.surname as Name, studentsprofile.Sex, studentsprofile.dateOfBirth as Birthday, studentsummary.age as 'Age', studentsprofile.phone as 'Phone number' from studentsprofile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? order by studentsummary.session desc", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
            Dim ds As New DataTable
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1.SelectCommand = cmdLoad1
            adapter1.Fill(ds)

            DetailsView1.DataSource = ds
            DetailsView1.DataBind()

            If pass <> "" Then Image1.ImageUrl = pass
            Dim cmdLoad1xa As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.passport from StudentsProfile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? order by studentsummary.session desc", con)
            cmdLoad1xa.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            Dim studentxa As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1xa.ExecuteReader
            studentxa.Read()
            If Not studentxa(0).ToString = "" Then Image4.ImageUrl = studentxa(0).ToString
            studentxa.Close()

            Dim cmdLoad1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.parentname as 'Parents/Guardians', parentprofile.phone as 'Parents Phone number', parentprofile.address as Address from StudentsProfile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? order by studentsummary.session desc", con)
            cmdLoad1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            cmdLoad1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
            Dim dsa As New DataTable
            Dim adapter1a As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1a.SelectCommand = cmdLoad1a
            adapter1a.Fill(dsa)

            DetailsView3.DataSource = dsa
            DetailsView3.DataBind()

            If hostel <> "" Then
                Dim cmdLoad1z As New MySql.Data.MySqlClient.MySqlCommand("SELECT hostel.hostel as Hostel, staffprofile.surname as 'Hostel Ward', staffprofile.passport, staffprofile.phone as 'Phone No' from hostel left Join staffprofile on hostel.ward = staffprofile.StaffId where hostel.hostel = ?", con)
                cmdLoad1z.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", hostel))
                Dim studentz As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1z.ExecuteReader
                Dim passz As String
                If studentz.Read() Then passz = studentz.Item(2).ToString
                studentz.Close()
                Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT hostel.hostel as Hostel, staffprofile.surname as 'Hostel Ward', staffprofile.phone as 'Phone No' from hostel left Join staffprofile on hostel.ward = staffprofile.StaffId where hostel.hostel = ?", con)
                cmdLoad2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", hostel))

                Dim dsz As New DataTable
                Dim adapter1z As New MySql.Data.MySqlClient.MySqlDataAdapter
                adapter1z.SelectCommand = cmdLoad2
                adapter1z.Fill(dsz)
                DetailsView2.DataSource = dsz
                DetailsView2.DataBind()
                panel1.Visible = True

                If Not passz = "" Then Image2.ImageUrl = passz
            End If
            If transport <> "" Then
                Dim cmdLoad1z As New MySql.Data.MySqlClient.MySqlCommand("SELECT transportfees.route as Route, transportfees.amount as Cost, staffprofile.surname as Driver, staffprofile.passport, staffprofile.phone as 'Driver No', transportfees.busno as 'Bus No' from transportfees left Join staffprofile on transportfees.driver = staffprofile.StaffId where transportfees.route = ?", con)
                cmdLoad1z.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", transport))
                Dim studentz As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1z.ExecuteReader
                Dim passz As String
                If studentz.Read() Then passz = studentz.Item(3).ToString
                studentz.Close()
                Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT transportfees.route as Route, staffprofile.surname as Driver, staffprofile.phone as 'Driver No', transportfees.busno as 'Bus No' from transportfees left Join staffprofile on transportfees.driver = staffprofile.StaffId where transportfees.route = ?", con)
                cmdLoad2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", transport))

                Dim dsz As New DataTable
                Dim adapter1z As New MySql.Data.MySqlClient.MySqlDataAdapter
                adapter1z.SelectCommand = cmdLoad2
                adapter1z.Fill(dsz)
                DetailsView4.DataSource = dsz
                DetailsView4.DataBind()
                panel2.Visible = True
                If Not passz = "" Then Image3.ImageUrl = passz
            End If

            con.Close()
        End Using
        gridview1.SelectedIndex = -1

        pnlAll.Visible = False
        panel3.Visible = True

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
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.id WHERE class.class = ?", con)
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



    Protected Sub lnkclrpt_Click(sender As Object, e As EventArgs) Handles lnkclrpt.Click
        Session("classselect") = DropDownList1.Text
        Session("schoolselect") = Nothing
        Response.Redirect("~/content/app/Admin/classlist.aspx")
    End Sub
    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
        gridview1.SelectedIndex = -1
    End Sub



    Protected Sub lnkschrpt_Click(sender As Object, e As EventArgs) Handles lnkschrpt.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT superior from class where class = '" & DropDownList1.Text & "'", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            student0.Read()
            Session("schoolselect") = student0(0)
            student0.Close()
            Response.Redirect("~/content/app/Admin/classlist.aspx")
            con.close()        End Using
    End Sub
End Class
