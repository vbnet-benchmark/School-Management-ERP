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
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Parent(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

       
        If IsPostBack Then
        Else
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    logify.Read_notification("~/content/parent/studentprofile.aspx?" & Request.QueryString.ToString, Session("parentid"))
                    logify.Read_notification("~/content/parent/studentprofile.aspx", Session("parentid"))



                    panel3.Visible = False

                    Dim ds As New DataTable
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from parentward inner join studentsprofile on parentward.ward = studentsprofile.admno WHERE parentward.parent = ?", con)
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("parentID")))
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                    Do While student0.Read
                        ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                    Loop
                    student0.Close()
                    gridview1.DataSource = ds
                    gridview1.DataBind()

                    con.close()                End Using
                If Request.QueryString.ToString <> Nothing Then Session("studentadd") = Request.QueryString.ToString
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
        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.admno as 'Admission number', studentsprofile.surname as Name, studentsprofile.Sex, studentsprofile.dateOfBirth as Birthday, studentsummary.age as 'Age', studentsprofile.phone as 'Phone number' from studentsProfile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? order by studentsummary.session desc", con)
        cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
        cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionID")))
        Dim ds As New DataTable
        Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
        adapter1.SelectCommand = cmdLoad1
        adapter1.Fill(ds)

        DetailsView1.DataSource = ds
        DetailsView1.DataBind()

        Dim cmdLoad1xa As New MySql.Data.MySqlClient.MySqlCommand("SELECT  class.class as Class, staffprofile.surname as 'Class Teacher', staffprofile.passport from StudentsProfile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? order by studentsummary.session desc", con)
        cmdLoad1xa.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
        Dim studentxa As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1xa.ExecuteReader
        studentxa.Read()
        If Not studentxa(2).ToString = "" Then Image4.ImageUrl = studentxa(2).ToString
        studentxa.Close()

        Dim cmdLoad1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT  class.class as Class, staffprofile.surname as 'Class Teacher' from StudentsProfile left Join (studentsummary left join class on class.id = studentsummary.class) on studentsummary.student = studentsprofile.admno left join (classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid) on classteacher.class = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentid) on parentward.ward = studentsprofile.admno where studentsprofile.admno = ? order by studentsummary.session desc", con)
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
        If pass <> "" Then Image1.ImageUrl = pass
        gridview1.SelectedIndex = -1
        panel3.Visible = True
            pnlAll.Visible = False
            con.Close()
        End Using
    End Sub


   

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Session("edit") = "studentprofile"
        Response.Redirect("~/content/parent/editchild.aspx")
    End Sub

    Protected Sub LinkButton4_Click(sender As Object, e As EventArgs) Handles LinkButton4.Click
        Session("edit") = "passport"
        Response.Redirect("~/content/parent/editchild.aspx")
    End Sub

   
    

   
    Protected Sub gridview1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gridview1.SelectedIndexChanged

    End Sub

    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview1.SelectedIndexChanging
        Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
        Session("studentadd") = RTrim(x(0))
        Student_Details()
        panel3.Visible = True
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
    End Sub
    
End Class
