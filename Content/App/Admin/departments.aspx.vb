Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls



Partial Class Admin_staffprofile
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList
    Dim deptId As String

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

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try
            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept, superior from depts Order by Id", con)

                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    DropDownList1.Items.Clear()
                    DropDownList1.Items.Add("Select Department")

                    Do While student0.Read
                        If student0.Item(1).ToString = "None" Then
                            DropDownList1.Items.Add(student0.Item(0).ToString)
                        Else
                            DropDownList1.Items.Add(student0.Item(0).ToString & " - " & student0.Item(1).ToString)
                        End If
                    Loop
                    student0.Close()
                    con.Close()                End Using
            Else
                Session("currentroute") = Nothing
            End If
            If Session("currentroute") <> Nothing Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from depts where id = ?", con)
                    cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", Session("currentroute")))
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    student.Read()
                    DropDownList1.Text = student.Item(1).ToString & " - " & student.Item(3).ToString
                    student.Close()
                    Dim x As Array = Split(DropDownList1.Text, " - ")
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from depts where superior = ?", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", x(0)))
                    Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    subdeptList.Items.Clear()
                    Do While student1.Read
                        subdeptList.Items.Add(student1.Item(1))
                    Loop
                    subdeptList.Visible = True

                    If subdeptList.Items.Count = 0 Then
                        lnkView.Visible = True
                    Else
                        lnkView.Visible = False
                    End If

                    student1.Close()
                    con.Close()                End Using
                Staff_Details()

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Staff_Details()
        If DropDownList1.Text = "Select Department" Then
            Show_Alert(False, "Please select a Department")
            Exit Sub
        End If
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim super As Boolean = False
            Dim a As Array = Split(DropDownList1.Text, " - ")
            If a.Length = 1 Then
                super = True
            End If
            If super = True Then
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Id from depts where dept = '" & DropDownList1.Text & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                student0.Read()
                deptId = student0.Item(0)
                student0.Close()
            Else
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Id from depts where dept = '" & a(0) & "' and superior = '" & a(1) & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                student0.Read()
                deptId = student0.Item(0)
                student0.Close()
            End If
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT depts.dept as Department, depts.headtitle as 'Title of head', staffprofile.surname as 'Name of Head', staffprofile.passport, staffprofile.phone as 'Head No', depts.superior as 'Super Department' from depts left Join staffprofile on depts.head = staffprofile.StaffId where depts.Id = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", deptId))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim pass As String
            If student.Read() Then
                pass = student.Item(3).ToString
            End If
            student.Close()
            Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT depts.dept as Department, depts.headtitle as 'Title of head', staffprofile.surname as 'Name of Head', staffprofile.phone as 'Head No', depts.superior as 'Super Department' from depts left Join staffprofile on depts.head = staffprofile.StaffId where depts.Id = ?", con)
            cmdLoad2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", deptId))

            Dim ds As New DataTable
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1.SelectCommand = cmdLoad2
            adapter1.Fill(ds)
            DetailsView1.DataSource = ds
            DetailsView1.DataBind()
            If Not pass = "" Then
                Image1.ImageUrl = pass
            End If
            panel1.Visible = True
            Dim x As Array = Split(DropDownList1.Text, " - ")
            Dim cmdLoad1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from depts where superior = ?", con)
            cmdLoad1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", x(0)))
            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1a.ExecuteReader
            subdeptList.Items.Clear()
            Do While student1.Read
                subdeptList.Items.Add(student1.Item(1))
            Loop
            subdeptList.Visible = True

            If subdeptList.Items.Count = 0 Then
                lnkView.Visible = True
            Else
                lnkView.Visible = False
            End If

            student1.Close()

            con.close()        End Using
    End Sub
    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim super As Boolean = False
                Dim s As Array = Split(DropDownList1.Text, " - ")
                If s.Length = 1 Then
                    super = True
                End If
                If super = True Then
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Id from depts where dept = '" & DropDownList1.Text & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    student0.Read()
                    deptId = (student0.Item(0).ToString)
                    student0.Close()
                Else
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Id from depts where dept = '" & s(0) & "' and superior = '" & s(1) & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    student0.Read()
                    deptId = (student0.Item(0).ToString)
                    student0.Close()
                End If

                con.close()            End Using
            Response.Redirect("~/content/App/Admin/Departmentadd.aspx?" & deptId)
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub LinkButton5_Click(sender As Object, e As EventArgs) Handles LinkButton5.Click
        Response.Redirect("~/content/App/Admin/departmentadd.aspx")
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                subdeptList.Items.Clear()
                Dim x As Array = Split(DropDownList1.Text, " - ")
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from depts where superior = ?", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", x(0)))
                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While student1.Read
                    subdeptList.Items.Add(student1.Item(1))
                Loop
                subdeptList.Visible = True
                If subdeptList.Items.Count = 0 Then lnkView.Visible = True
                student1.Close()
                con.close()            End Using
            Staff_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkAdd_Click(sender As Object, e As EventArgs) Handles lnkView.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim super As Boolean = False
                Dim s As Array = Split(DropDownList1.Text, " - ")
                If s.Length = 1 Then
                    super = True
                End If
                If super = True Then
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Id from depts where dept = '" & DropDownList1.Text & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    student0.Read()
                    deptId = student0.Item(0).ToString
                    student0.Close()

                Else
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Id from depts where dept = '" & s(0) & "' and superior = '" & s(1) & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    student0.Read()
                    deptId = student0.Item(0).ToString
                    student0.Close()
                End If

                Response.Redirect("~/content/App/Admin/deptmembers.aspx?" & deptId)
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
