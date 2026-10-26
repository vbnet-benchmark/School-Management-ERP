Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls

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

                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.id, subjects.subject, class.class, courseoverview.overview, courseoverview.texts from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.id left join courseoverview on courseoverview.classub = classsubjects.id where classsubjects.teacher = '" & Session("staffid") & "'", con)
                Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Dim ds3 As New DataTable
                ds3.Columns.Add("id")
                ds3.Columns.Add("Subject")
                ds3.Columns.Add("class")
                ds3.Columns.Add("overview")
                ds3.Columns.Add("textbooks")
                ds3.Columns.Add("Edit")
                Do While reader0.Read
                    ds3.Rows.Add(reader0.Item(0), reader0.Item(1), reader0.Item(2), reader0.Item(3), reader0.Item(4), "Edit")
                Loop
                GridView3.DataSource = ds3
                GridView3.DataBind()
                reader0.Close()



                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub









    Protected Sub btnUpdate_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim exists As Boolean
                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select id from class where class = '" & cboClass.Text & "'", con)
                Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                readref2.Read()
                Dim clas As Integer = readref2.Item(0)
                readref2.Close()
                Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Select id from subjects where subject = '" & cboSubject.Text & "'", con)
                Dim readref3 As MySql.Data.MySqlClient.MySqlDataReader = ref3.ExecuteReader
                readref3.Read()
                Dim subject As Integer = readref3.Item(0)
                readref3.Close()
                Dim cmdCheck200 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM courseoverview inner join subjects on subjects.id = courseoverview.subject inner join session on session.id = courseoverview.session inner join class on class.id = courseoverview.class where session.Id = '" & Session("sessionid") & "' and class.class = '" & cboClass.Text & "' and subjects.subject = '" & cboSubject.Text & "'", con)
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck200.ExecuteReader()
                If reader20.Read() Then
                    exists = True
                End If
                reader20.Close()
                Dim cmdLoad0s As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.id from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.id left join courseoverview on courseoverview.classub = classsubjects.id where subjects.subject = '" & cboSubject.Text & "' and class.class = '" & cboClass.Text & "'", con)
                Dim reader0s As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0s.ExecuteReader
                reader0s.Read()
                Dim id As String = reader0s(0).ToString
                reader0s.Close()
                If exists = True Then
                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update courseoverview Set overview = '" & txtOver.Text & "', texts = '" & txtText.Text & "' where subject = '" & subject & "' and class = '" & clas & "' and session = '" & Session("sessionid") & "'", con)
                    cmdCheck3.ExecuteNonQuery()
                Else
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into courseoverview (session, class, subject, overview, texts, classub) Values (?,?,?,?,?,?)", con)
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("route", Session("sessionid")))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", clas))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", subject))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", txtOver.Text))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("bu", txtText.Text))
                    cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("id", id))

                    cmdCheck2.ExecuteNonQuery()

                End If
                panel1.Visible = False

                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.id, subjects.subject, class.class, courseoverview.overview, courseoverview.texts from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.id left join courseoverview on courseoverview.classub = classsubjects.id where classsubjects.teacher = '" & Session("staffid") & "'", con)
                Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Dim ds3 As New DataTable
                ds3.Columns.Add("id")
                ds3.Columns.Add("Subject")
                ds3.Columns.Add("class")
                ds3.Columns.Add("overview")
                ds3.Columns.Add("textbooks")
                ds3.Columns.Add("Edit")
                Do While reader0.Read
                    ds3.Rows.Add(reader0.Item(0), reader0.Item(1), reader0.Item(2), reader0.Item(3), reader0.Item(4), "Edit")
                Loop
                GridView3.DataSource = ds3
                GridView3.DataBind()
                reader0.Close()
                Show_Alert(True, "Course Overview Updated.")
                logify.log(Session("staffid"), "Course overview for " & cboSubject.Text & " - " & cboClass.Text & " updated.")
                con.close()            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView3_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles GridView3.SelectedIndexChanging
        Try


            Dim subject As String = GridView3.Rows(e.NewSelectedIndex).Cells(1).Text
            Dim clas As String = GridView3.Rows(e.NewSelectedIndex).Cells(2).Text

            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

                Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select classsubjects.id, subjects.subject, class.class, courseoverview.overview, courseoverview.texts from classsubjects inner join subjects on classsubjects.subject = subjects.id inner join class on classsubjects.class = class.id left join courseoverview on courseoverview.classub = classsubjects.id where subjects.subject = '" & subject & "' and class.class = '" & clas & "' and courseoverview.session = '" & Session("sessionid") & "'", con)
                Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                If readref.Read() Then
                    cboSubject.Items.Add(readref.Item(1).ToString)
                    cboClass.Items.Add(readref.Item(2).ToString)
                    txtOver.Text = readref.Item(3).ToString
                    txtText.Text = readref.Item(4).ToString
                    readref.Close()
                Else
                    cboSubject.Items.Add(subject)
                    cboClass.Items.Add(clas)
                End If
                panel1.Visible = True
                con.Close()
            End Using
        Catch ex As Exception

            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub
End Class
