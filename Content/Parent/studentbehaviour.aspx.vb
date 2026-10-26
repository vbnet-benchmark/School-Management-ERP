Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Account_Default
    Inherits System.Web.UI.Page
    Dim optionalfeeb As String
    Dim optionalfeef As String
    Dim optionalfeet As String
    Dim optionalfeec As New ArrayList
    Dim optionalfeenc As New ArrayList
    Dim hostel As Boolean
    Dim transport As String
    Dim feeding As String
    Dim minimum As Double
    Dim i As Integer

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

            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()

                    logify.Read_notification("~/content/parent/studentbehaviour.aspx?" & Request.QueryString.ToString, Session("parentid"))
                    logify.Read_notification("~/content/parent/studentbehaviour.aspx", Session("parentid"))


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
                    con.close()                End Using                If Request.QueryString.ToString <> Nothing Then Session("studentadd") = Request.QueryString.ToString
                If Session("studentadd") <> Nothing Then
                    Student_Details()
                End If
            End If

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub





    Private Sub Student_Details()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from StudentsProfile where admno = ?", con)
            cmdLoad4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("studentadd")))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
            Dim pass As String = ""
            If student.Read() Then pass = student.Item("passport").ToString
            student.Close()
            If Not pass = "" Then Image1.ImageUrl = pass
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname, class.class, studentsummary.trans, studentsummary.status, studentsummary.class, studentsprofile.sex From Studentsummary INNER JOIN Studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.Id WHERE StudentSummary.student = ? And StudentSummary.Session = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("Studentadd")))
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("SessionID")))
            Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
            studentsReader.Read()
            lblClass.Text = studentsReader.Item(0).ToString
            Session("ClassId") = studentsReader.Item(4)
            Dim sex As String = studentsReader.Item("sex").ToString
            studentsReader.Close()
            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from studentsummary where student = ? and class = ? and session = ? ", con)
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("Studentadd")))
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassId")))
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("Sessionid")))
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader()
            reader2.Read()
            Dim atall As Integer = Val(reader2("atall"))
            lblPresent.Text = reader2("present").ToString
            lblAbsent.Text = reader2("absent").ToString
            lblLate.Text = reader2("late").ToString
            lblLatePercent.Text = FormatNumber((Val(lblLate.Text) / atall) * 100, 2) & "%"
            lblOpened.Text = Val(lblPresent.Text) + Val(lblAbsent.Text)
            lblPercent.Text = FormatNumber((Val(lblPresent.Text) / Val(lblOpened.Text)) * 100, 2) & "%"
            reader2.Close()
           
            con.close()        End Using
        panel3.Visible = True
        pnlAll.Visible = False
        
    End Sub

  

    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview1.SelectedIndexChanging
        Try
            Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
            Session("studentadd") = RTrim(x(0))
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



   

 

   

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
        gridview1.SelectedIndex = -1
    End Sub

  

   
End Class
