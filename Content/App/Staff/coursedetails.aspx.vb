Imports System.IO
Imports System.Data

Partial Class Admin_addteacher



    Inherits System.Web.UI.Page
    

    Dim subselect As Integer
    Dim subremove As String
    Dim classremove As String
    Dim staffID As String

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


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Subjects.subject as Subject, Class.class as Class, staffprofile.surname as Instructor, staffprofile.phone as Phone, courseoverview.overview as Overview, courseoverview.texts as Textbooks FROM courseoverview inner join (classsubjects inner join staffprofile on classsubjects.teacher = staffprofile.staffid) on courseoverview.class = classsubjects.class and courseoverview.subject = classsubjects.subject inner join subjects on subjects.id = courseoverview.subject inner join session on session.id = courseoverview.session inner join class on class.id = courseoverview.class where session.ID = '" & Session("SessionId") & "' and class.Id = '" & Session("ClassId") & "' and subjects.subject = '" & Replace(Request.QueryString.ToString, "+", " ") & "'", con)
                Dim ds As New DataTable
                Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                adapter1.SelectCommand = cmdCheck20
                adapter1.Fill(ds)
                DetailsView1.DataSource = ds
                DetailsView1.DataBind()



                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where subjects.subject = '" & Replace(Request.QueryString.ToString, "+", " ") & "' and class.id = '" & Session("ClassId") & "' and courseoutline.session = '" & Session("SessionId") & "' order by courseoutline.week asc", con)
                Dim ds2 As New DataTable
                ds2.Columns.Add("Week")
                ds2.Columns.Add("Topic")
                ds2.Columns.Add("Content")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ds2.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2))
                Loop
                reader.Close()
                GridView1.DataSource = ds2
                GridView1.DataBind()

                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

  
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("~/content/app/staff/classdetails.aspx")

    End Sub

    
End Class
