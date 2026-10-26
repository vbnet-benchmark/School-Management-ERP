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
       If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

       
        panel1.Visible = False
        If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from hostel Order by Id", con)

                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    DropDownList1.Items.Clear()
                    DropDownList1.Items.Add("Select Hostel")

                    Do While student0.Read
                        DropDownList1.Items.Add(student0.Item(0).ToString)
                    Loop
                    student0.Close()
                    con.close()                End Using
            Else
                Session("currenthostel") = Nothing
            End If
            If Session("currenthostel") <> Nothing Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from hostel where hostel = ?", con)
                    cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("hostel", Session("currenthostel")))
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    student.Read()
                    DropDownList1.Text = student.Item("hostel").ToString
                    student.Close()
                    con.close()                End Using
                Staff_Details()
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Staff_Details()
        If DropDownList1.Text = "Select Hostel" Then
            Show_Alert(False, "Please select a hostel")
            Exit Sub
        End If
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT hostel.hostel as Hostel, staffprofile.surname as 'Hostel Ward', staffprofile.passport, staffprofile.phone as 'Phone No' from hostel left Join staffprofile on hostel.ward = staffprofile.StaffId where hostel.hostel = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", DropDownList1.Text))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim pass As String
            If student.Read() Then pass = student.Item(2).ToString
            student.Close()
            Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT hostel.hostel as Hostel, staffprofile.surname as 'Hostel Ward', staffprofile.phone as 'Phone No' from hostel left Join staffprofile on hostel.ward = staffprofile.StaffId where hostel.hostel = ?", con)
            cmdLoad2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", DropDownList1.Text))

            Dim ds As New DataTable
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1.SelectCommand = cmdLoad2
            adapter1.Fill(ds)
            DetailsView1.DataSource = ds
            DetailsView1.DataBind()
            panel1.Visible = True

            If Not pass = "" Then Image1.ImageUrl = pass


            con.close()        End Using
    End Sub
    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click



        Response.Redirect("~/content/Admin/hostels.aspx?" & DropDownList1.Text)

    End Sub


    Protected Sub LinkButton5_Click(sender As Object, e As EventArgs) Handles LinkButton5.Click
        Response.Redirect("~/content/Admin/hostels.aspx")
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try
            Staff_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    
End Class
