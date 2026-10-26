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
        If check.Check_Staff(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

      
            If Not IsPostBack Then
                logify.Read_notification("~/content/staff/staffprofile.aspx", Session("staffid"))
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffid as 'Staff ID', surname as 'Name', sex as Sex, phone as 'Phone No', address as 'Address', email as 'Email', designation as 'Designation', salary as 'Salary', accountno as 'Account Number', bank as 'Banker', pension as 'Pensioned' from staffprofile where staffid = ?", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("StaffID")))
                    Dim ds As New DataTable
                    Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                    Dim pass As String

                    adapter1.SelectCommand = cmdLoad1
                    adapter1.Fill(ds)
                    DetailsView1.DataSource = ds
                    DetailsView1.DataBind()
                    Dim cmdLoad2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from staffprofile where staffid = ?", con)
                    cmdLoad2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("StaffID")))
                    Dim student2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad2.ExecuteReader

                    student2.Read()
                    pass = student2.Item("passport").ToString
                    student2.Close()
                    If pass = "" Then
                        pass = "~/image/noimage.jpg"
                    End If
                    Image1.ImageUrl = pass


                    con.close()                End Using
                roles()
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub
    Private Sub roles()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class, subjects.subject, classsubjects.periods from classsubjects Inner Join Class on Class.ID = classsubjects.class Inner Join Subjects on Subjects.ID = classsubjects.subject  WHERE classsubjects.teacher = ?", con)
            cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("StaffId")))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
            Dim total As Integer = 0
            Dim sda As New DataTable
            sda.Columns.Add("subject")
            sda.Columns.Add("class")
            sda.Columns.Add("periods")

            Do While reader2.Read
                pnlTaught.Visible = True
                sda.Rows.Add(reader2(1), reader2(0), reader2(2))
                total = total + Val(reader2(2))
            Loop
            reader2.Close()
            sda.Rows.Add("TOTAL PERIODS PER WEEK", "", total)
            GridView2.DataSource = sda
            GridView2.DataBind()
            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from classteacher Inner Join Class on Class.ID = classteacher.class WHERE classteacher.teacher = ? order by class.class asc", con)
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("StaffId")))
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
            Dim count2 As Integer = 1
            Do While reader3.Read
                pnlClass.Visible = True
                Dim classes As New Label
                If FindControl("cboclass" & count2) Is Nothing Then
                    classes.ID = "cboclass" & count2
                End If
                classes.Text = count2 & ".   " & reader3.Item(0)
                manage.Controls.Add(classes)
                Dim MyLiteral = New LiteralControl
                MyLiteral.Text = "<BR/>"
                manage.Controls.Add(MyLiteral)
                count2 = count2 + 1
            Loop
            Dim link As New LiteralControl
            reader3.Close()



            Dim cmd21 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from acclogin where username = ?", con)
            cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("Staffid")))
            Dim reader31 As MySql.Data.MySqlClient.MySqlDataReader = cmd21.ExecuteReader
            Dim count21 As Integer = 1
            If reader31.Read Then
                pnlOther.Visible = True
                Dim classes2 As New Label
                If FindControl("cboroles" & count21) Is Nothing Then
                    classes2.ID = "cboroles" & count21
                End If
                classes2.Text = count21 & ".   " & "SCHOOL ACCOUNTANT"
                other.Controls.Add(classes2)
                Dim MyLiteral = New LiteralControl
                MyLiteral.Text = "<BR/>"
                other.Controls.Add(MyLiteral)
                count21 = count21 + 1
            End If
            reader31.Close()
            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from admin where username = ?", con)
            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("Staffid")))
            Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader
            If reader0.Read Then
                pnlOther.Visible = True
                Dim classes3 As New Label
                If FindControl("cboroles" & count21) Is Nothing Then
                    classes3.ID = "cboroles" & count21
                End If
                classes3.Text = count21 & ".   " & "SCHOOL ADMIN"
                other.Controls.Add(classes3)
                Dim MyLiteral = New LiteralControl
                MyLiteral.Text = "<BR/>"
                other.Controls.Add(MyLiteral)
                count21 = count21 + 1
            End If
            reader0.Close()

            Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from depts where head = ?", con)
            cmd4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("Staffid")))
            Dim reader4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader
            Do While reader4.Read
                pnlOther.Visible = True
                Dim classes3 As New Label
                If FindControl("cboroles" & count21) Is Nothing Then
                    classes3.ID = "cboroles" & count21
                End If
                classes3.Text = count21 & ".   " & reader4(4) & " - " & reader4(1)
                other.Controls.Add(classes3)
                Dim MyLiteral = New LiteralControl
                MyLiteral.Text = "<BR/>"
                other.Controls.Add(MyLiteral)
                count21 = count21 + 1
            Loop
            reader4.Close()
            Dim cmd4x As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from hostel where ward = ?", con)
            cmd4x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("Staffid")))
            Dim reader4x As MySql.Data.MySqlClient.MySqlDataReader = cmd4x.ExecuteReader
            Do While reader4x.Read
                pnlOther.Visible = True
                Dim classes3 As New Label
                If FindControl("cboroles" & count21) Is Nothing Then
                    classes3.ID = "cboroles" & count21
                End If
                classes3.Text = count21 & ".   HOSTEL WARD - " & reader4x(0)
                other.Controls.Add(classes3)
                Dim MyLiteral = New LiteralControl
                MyLiteral.Text = "<BR/>"
                other.Controls.Add(MyLiteral)
                count21 = count21 + 1
            Loop
            reader4x.Close()

            Dim cmd5d As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from transportfees where driver = ?", con)
            cmd5d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("Staffid")))
            Dim reader5d As MySql.Data.MySqlClient.MySqlDataReader = cmd5d.ExecuteReader
            Do While reader5d.Read
                pnlOther.Visible = True
                Dim classes3 As New Label
                If FindControl("cboroles" & count21) Is Nothing Then
                    classes3.ID = "cboroles" & count21
                End If
                classes3.Text = count21 & ".   DRIVER - " & reader5d(0) & " (" & reader5d(3) & ")"
                other.Controls.Add(classes3)
                Dim MyLiteral = New LiteralControl
                MyLiteral.Text = "<BR/>"
                other.Controls.Add(MyLiteral)
                count21 = count21 + 1
            Loop
            reader5d.Close()
            Dim count210 As Integer = 1
            Dim cmd56 As New MySql.Data.MySqlClient.MySqlCommand("SELECT depts.dept from staffdept inner join depts on depts.id = staffdept.dept where staff = ?", con)
            cmd56.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("Staffid")))
            Dim reader56 As MySql.Data.MySqlClient.MySqlDataReader = cmd56.ExecuteReader
            Do While reader56.Read
                pnlDept.Visible = True
                Dim classes3 As New Label
                If FindControl("cboroles" & count21) Is Nothing Then
                    classes3.ID = "cboroles" & count21
                End If
                classes3.Text = count210 & ".   " & reader56(0)
                dept.Controls.Add(classes3)
                Dim MyLiteral = New LiteralControl
                MyLiteral.Text = "<BR/>"
                dept.Controls.Add(MyLiteral)
                count210 = count210 + 1
                count21 = count21 + 1
            Loop
            reader56.Close()
            con.close()        End Using
    End Sub
    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Try
            Panel1.Visible = True
            roles()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If txtPassword.Text <> txtPassword0.Text Then
                    Show_Alert(False, "Your password entries do not match")
                ElseIf txtPassword.Text = "" Or txtPassword0.Text = "" Then
                    Show_Alert(False, "Password field is blank")
                Else
                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update StaffProfile Set password = ? where staffID = ?", con)
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("password", txtPassword.Text))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Session("StaffID")))
                    cmdCheck3.ExecuteNonQuery()
                    Show_Alert(True, "Password updated successfully")
                    Panel1.Visible = False
                End If
                con.close()            End Using
            roles()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkEdit_Click(sender As Object, e As EventArgs) Handles lnkEdit.Click
        Session("StaffAdd") = Session("Staffid")
        Session("edit") = "teacherprofile"
        Response.Redirect("~/content/staff/editprofile.aspx")
    End Sub

    Protected Sub lnkpassport_Click(sender As Object, e As EventArgs) Handles lnkpassport.Click
        Session("StaffAdd") = Session("Staffid")
        Session("edit") = "passport"
        Response.Redirect("~/content/staff/editprofile.aspx")
    End Sub

    Protected Sub btnCancel_Click(sender As Object, e As EventArgs) Handles btnCancel.Click
        Try
            Panel1.Visible = False
            roles()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
