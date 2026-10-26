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

    ' --- Book Digital Copy Upload (PLANTED-VB-HR-211) ---
    ' A newer feature lets the librarian attach the book's full digital copy (PDF/EPUB)
    ' separately from the catalog entry above, so it can be uploaded or replaced without
    ' re-submitting the whole book form. The original "Quick Upload" button was kept for staff
    ' who just want the old one-click flow; "Validated Upload" was added afterwards once a
    ' security review asked for a size cap and a non-overwriting destination name.

    ' Instance (Direct): straight-line SaveAs with no helper, no size check, no overwrite
    ' check, and the destination name taken directly from the client's own file name.
    Protected Sub btnUploadBookContentQuick_Click(sender As Object, e As EventArgs) Handles btnUploadBookContentQuick.Click
        If FileUploadBookContent.HasFile Then
            Dim folderPath As String = Server.MapPath("~/Uploads/books/content/")
            ' SINK: PLANTED-VB-HR-211
            FileUploadBookContent.SaveAs(folderPath & FileUploadBookContent.FileName)
            lblBookContentResult.Text = "Digital copy uploaded: " & FileUploadBookContent.FileName
        Else
            lblBookContentResult.Text = "Please select a file."
        End If
    End Sub

    ' Safe twin: caps the upload size and writes under a server-generated name instead of the
    ' client's own file name, so a second upload can never silently overwrite an earlier one.
    Protected Sub btnUploadBookContentValidated_Click(sender As Object, e As EventArgs) Handles btnUploadBookContentValidated.Click
        If FileUploadBookContent.HasFile Then
            If FileUploadBookContent.PostedFile.ContentLength > 20971520 Then
                lblBookContentResult2.Text = "File not uploaded, the file selected is greater than 20mb."
                Exit Sub
            End If
            Dim folderPath As String = Server.MapPath("~/Uploads/books/content/")
            Dim safeName As String = Guid.NewGuid().ToString("N") & Path.GetExtension(FileUploadBookContent.FileName)
            ' SAFE_SINK: PLANTED-VB-HR-211-safe
            FileUploadBookContent.SaveAs(folderPath & safeName)
            lblBookContentResult2.Text = "Digital copy uploaded successfully."
        Else
            lblBookContentResult2.Text = "Please select a file."
        End If
    End Sub

    ' --- Bulk Sample-Chapter Upload (PLANTED-VB-HR-580) ---
    ' A librarian can attach several sample-chapter files to a catalog entry in one submit,
    ' separate from the single "Digital Copy" slot above -- plain <input type="file">
    ' fields rather than another FileUpload control, read back via the raw posted-files
    ' collection (by field name, never a blind index loop, so this can never sweep in
    ' FileUpload1 or FileUploadBookContent from the same postback) and saved in a loop
    ' through a new BLL class (App_Code/LibrarySampleChaptersBLL.vb).

    ' Instance (diversity, batch loop): interprocedural, but over a loop of posted files
    ' rather than a single-file call -- a distinct construction shape from this project's
    ' other CWE-530 instances.
    Protected Sub btnUploadSampleChapters_Click(sender As Object, e As EventArgs) Handles btnUploadSampleChapters.Click
        Try
            Dim chaptersBll As New LibrarySampleChaptersBLL
            Dim savedCount As Integer = chaptersBll.ImportSampleChapters(txtTitle.Text, Request.Files)
            lblSampleChaptersResult.Text = savedCount & " sample chapter file(s) uploaded."
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnUploadSampleChaptersSafe_Click(sender As Object, e As EventArgs) Handles btnUploadSampleChaptersSafe.Click
        Try
            Dim chaptersBll As New LibrarySampleChaptersBLL
            Dim savedCount As Integer = chaptersBll.ImportSampleChaptersSafe(txtTitle.Text, Request.Files)
            lblSampleChaptersResult2.Text = savedCount & " sample chapter file(s) uploaded."
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
