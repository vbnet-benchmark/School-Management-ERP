Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.Web
Imports System.IO
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.parser

Partial Class Admin_allstudents
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList
    Dim logify As New notify


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            logify.Read_notification("~/content/student/viewelearning.aspx?" & Request.QueryString.ToString, Session("studentid"))

            Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("Select id, title, file, type from elearning where id = '" & Request.QueryString.ToString & "'", con)
            Dim reada As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck.ExecuteReader
            reada.Read()



            Dim byts() As Byte


            Dim wc As New System.Net.WebClient

            byts = wc.DownloadData("http://" & Request.Url.Authority & "/Content" & Replace(reada(2).ToString, "~", ""))

            Dim ms As New MemoryStream(byts)
            Context.Response.Clear()
            Context.Response.AppendHeader("Content-Disposition", "filename=" + reada.Item(1))
            Response.ContentType = reada.Item(3).ToString
            Context.Response.Buffer = True
            Context.Response.BinaryWrite(byts)
            Context.Response.End()

            con.Dispose()
            con.close()        End Using

    End Sub





End Class
