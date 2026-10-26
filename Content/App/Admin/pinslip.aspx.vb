Imports System.Text
Imports System.Configuration
Imports MySql.Data.MySqlClient
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports Microsoft.Reporting.WebForms
Imports System.IO
Imports iTextSharp.text
Imports iTextSharp.text.pdf
Partial Class Admin_Default2
    Inherits System.Web.UI.Page
    Dim con2 As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        con2.Open()

        ReportViewer1.ProcessingMode = ProcessingMode.Local


        ReportViewer1.Reset()

        Dim result As New DataTable
        Dim resultquerycmd As New MySql.Data.MySqlClient.MySqlCommand("select studentsprofile.admno, studentsprofile.surname, pin.pin, pin.dateissued, pin.expiry from studentsprofile inner join pin on pin.admno = studentsprofile.admno where studentsProfile.admno = ? order by pin.ID Desc", con2)
        resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("StudentsProfile.ID", Session("StudentID")))
        Dim resultqueryTableAdapter As New MySql.Data.MySqlClient.MySqlDataAdapter
        resultqueryTableAdapter.SelectCommand = resultquerycmd
        resultqueryTableAdapter.Fill(result)
        Dim dt As New ReportDataSource("DataSet1", result)

        ReportViewer1.LocalReport.DataSources.Clear()

        ReportViewer1.LocalReport.DataSources.Add(dt)
        ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/admin/Report1.rdlc")




       
        con2.Close()


        Dim warnings As Warning() = Nothing

        Dim streamids As String() = Nothing

        Dim mimeType As String = Nothing

        Dim encoding As String = Nothing

        Dim extension As String = Nothing

        Dim bytes As Byte()


        bytes = ReportViewer1.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)

        Dim s As New MemoryStream(bytes)

        s.Seek(0, SeekOrigin.Begin)
        Response.ContentType = "application/pdf"
        Response.BinaryWrite(bytes)
        Response.Flush()
        Response.Close()

    End Sub
End Class
