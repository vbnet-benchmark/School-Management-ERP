Imports System.Text
Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Partial Class Admin_adminpage
    Inherits System.Web.UI.Page
    Public Shared receiver As String
    Public Shared recCategory As String

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
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim scriptman As New ScriptManager
        scriptman = Me.Master.FindControl("ScriptManager1")
        scriptman.RegisterPostBackControl(btnSend)
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = False

    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Admin(Session("roles"), Session("usertype")) = False And check.Check_lib(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

                If Not IsPostBack Then
                    Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select subject from subjects order by subject", con)
                    Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                    cboSubject.Items.Clear()
                    cboSubject.Items.Add("SELECT")
                    Dim subjects As New ArrayList
                    Do While readref.Read
                        If Not subjects.Contains(readref.Item(0).ToString) Then
                            cboSubject.Items.Add(readref.Item(0))
                            subjects.Add(readref.Item(0).ToString)
                        End If
                    Loop
                    readref.Close()
                End If

                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
           
            Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Select id from subjects where subject = '" & cboSubject.Text & "'", con)
            Dim readref3 As MySql.Data.MySqlClient.MySqlDataReader = ref3.ExecuteReader
            readref3.Read()
            Dim subject As Integer = readref3.Item(0)
            readref3.Close()
            Dim folderPath As String = Server.MapPath("~/content/Uploads/")
            If FileUpload1.HasFile Then
                Dim type As String = FileUpload1.PostedFile.ContentType
                If type <> "application/pdf" Then
                    Show_Alert(False, "Please select a pdf file.")
                    Exit Sub
                End If
                If FileUpload1.PostedFile.ContentLength > 13107200 Then
                    Show_Alert(False, "File not uploaded, the file selected is greater than 10mb.")
                    Exit Sub
                End If
                FileUpload1.SaveAs(folderPath & Path.GetFileName(FileUpload1.FileName))
                Dim q As String = "~/Uploads/" & FileUpload1.FileName
                Dim cmdCheck20 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into books (bookname, subject, author, publisher, file) values (?,?,?,?,?)", con)

                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("title", txtTitle.Text))
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", subject))
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", txtAuthor.Text))
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", txtPublisher.Text))
                cmdCheck20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("assignment", q))
                
                cmdCheck20.CommandTimeout = 1200

                cmdCheck20.ExecuteNonQuery()

                Show_Alert(True, "E-book added successfully.")

                logify.log(Session("staffid"), "An E-book - " & txtTitle.Text & " was uploaded for " & cboSubject.Text)

              
                con.Dispose()
                txtTitle.Text = ""
                cboSubject.Text = "SELECT"
                txtAuthor.Text = ""
                txtPublisher.Text = ""
            Else
                Show_Alert(False, "Please select a file")
                Exit Sub
                End If
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

   
   

   
   
    

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Response.Redirect("~/content/admin/library.aspx")
    End Sub
End Class
