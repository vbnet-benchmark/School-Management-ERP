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
Partial Class Student_result
    Inherits System.Web.UI.Page
    Dim con2 As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)

    Protected Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load
        con2.Open()




        Dim result As New DataTable
        Dim resultqueryTableAdapter As New MySql.Data.MySqlClient.MySqlDataAdapter
        Dim dt As New ReportDataSource("DataSet1", result)

        If Session("schoolselect") <> Nothing Then
            Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class where superior = '" & Session("schoolselect") & "'", con2)
            resultqueryTableAdapter.SelectCommand = cmdSelect2
            resultqueryTableAdapter.Fill(result)
        Else
            Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class where class = '" & Session("classselect") & "'", con2)
            resultqueryTableAdapter.SelectCommand = cmdSelect2
            resultqueryTableAdapter.Fill(result)
        End If
        Dim rpt As New ReportViewer


        rpt.ProcessingMode = ProcessingMode.Local
        rpt.Reset()
        rpt.LocalReport.DataSources.Clear()
        rpt.LocalReport.DataSources.Add(dt)
        con2.Close()
        rpt.LocalReport.ReportPath = Server.MapPath("~/content/Admin/classlistfull.rdlc")
        PlaceHolder1.Controls.Add(rpt)

        AddHandler rpt.LocalReport.SubreportProcessing, AddressOf MySubreportEventHandler
        rpt.LocalReport.EnableExternalImages = True
        rpt.LocalReport.Refresh()











        Dim bytes As Byte()


        Dim warnings As Warning() = Nothing

        Dim streamids As String() = Nothing

        Dim mimeType As String = Nothing

        Dim encoding As String = Nothing

        Dim extension As String = Nothing

        bytes = rpt.LocalReport.Render("PDF", Nothing, mimeType, encoding, extension, streamids, warnings)
        Dim s As New MemoryStream(bytes)

        s.Seek(0, SeekOrigin.Begin)
        Response.ContentType = "application/pdf"
        Response.BinaryWrite(bytes)
        Response.Flush()
        Response.Close()
        con2.Close()
    End Sub


    Public Sub MySubreportEventHandler(ByVal sender As Object, ByVal e As SubreportProcessingEventArgs)
        con2.Open()
        Dim studentID As String = e.Parameters(0).Values(0)
        Dim pl As New DataTable
        pl.Columns.Add("sn")
        pl.Columns.Add("class")
        pl.Columns.Add("admno")
        pl.Columns.Add("surname")
        pl.Columns.Add("sex")
        pl.Columns.Add("age")
        pl.Columns.Add("parentno")
        Dim ad As New MySql.Data.MySqlClient.MySqlDataAdapter
        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.admno, studentsprofile.surname, studentsprofile.sex, studentsummary.age, parentprofile.phone from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class on class.id = studentsummary.class left join (parentward inner join parentprofile on parentward.parent = parentprofile.parentID) on parentward.ward = studentsprofile.admno where class.class = ? and studentsummary.session = ? order by studentsprofile.surname", con2)
        cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", studentID))
        cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("SessionId")))
        Dim read As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
        Dim sn As Integer = 1
        Do While read.Read
            pl.Rows.Add(sn, studentID, read(0), read(1), read(2), read(3), read(4))
            sn = sn + 1
        Loop
        read.Close()
        Dim dt As New ReportDataSource("DataSet1", pl)
        e.DataSources.Add(dt)
        con2.Close()

    End Sub



    

    
End Class
