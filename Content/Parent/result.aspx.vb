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
        Dim result As New DataTable
        Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT Studentsummary.Session, Studentsummary.Class, studentsummary.student, studentsprofile.passport FROM StudentSummary Inner join studentsprofile on studentsprofile.admno = studentsummary.student WHERE studentsummary.class = ? And studentsummary.Session = ? And studentsummary.student = ?", con2)
        cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("rsClass")))
        cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("rsSession")))
        cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentID")))
        Dim resultqueryTableAdapter As New MySql.Data.MySqlClient.MySqlDataAdapter
        resultqueryTableAdapter.SelectCommand = cmdSelect2
        resultqueryTableAdapter.Fill(result)
        Dim dt As New ReportDataSource("DataSet1", result)
        Dim rpt As New ReportViewer


        rpt.ProcessingMode = ProcessingMode.Local
        rpt.Reset()
        rpt.LocalReport.DataSources.Clear()
        rpt.LocalReport.DataSources.Add(dt)
        con2.Close()
        rpt.LocalReport.ReportPath = Server.MapPath("~/Staff/classreport.rdlc")
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
    End Sub


    Public Sub MySubreportEventHandler(ByVal sender As Object, ByVal e As SubreportProcessingEventArgs)
        con2.Open()
        Dim studentID As String = e.Parameters(0).Values(0)
        Dim result As New DataTable
        Dim resultquerycmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsProfile.surname, studentSummary.ID, Session.Session, studentSummary.Class, studentSummary.student, studentSummary.Age, studentSummary.Average, studentSummary.Position, studentSummary.classTeacherRemarks, studentSummary.principalRemarks, studentSummary.handWriting, studentSummary.fluency, studentSummary.games, studentSummary.sports, studentSummary.gymnastics, studentSummary.tools, studentSummary.drawing, studentSummary.crafts, studentSummary.musical, studentSummary.punctual, studentSummary.attendance, studentSummary.reliability, studentSummary.neatness, studentSummary.polite, studentSummary.honesty, studentSummary.relate, studentSummary.selfcontrol, studentSummary.cooperation, studentSummary.responsibility, studentSummary.attentiveness, studentSummary.initiative, studentSummary.organization, studentSummary.perseverance, studentSummary.Present, studentSummary.Absent, studentSummary.ClassNo, Session.Term, Class.Class AS Expr1, Session.TotalNoTerms, Session.NextTerm, Subjects.Subject, subjectReg.CA1, subjectReg.CA2, subjectReg.CA3, subjectReg.Examination, subjectReg.Total, subjectReg.Highest, subjectReg.Lowest, subjectReg.Grade, subjectReg.Remarks, subjectReg.avg, subjectreg.pos, Session.ID AS Expr2, Class.ID AS Expr3, studentsProfile.admno AS Expr4, studentsprofile.passport as image, Subjects.ID AS SID, Session.ClosingDate FROM Subjects INNER JOIN (studentsProfile INNER JOIN (Class INNER JOIN (studentSummary INNER JOIN (Session INNER JOIN subjectReg ON Session.ID = subjectReg.Session) ON studentSummary.Session = Session.ID) ON (Class.ID = subjectReg.Class) AND (Class.ID = studentSummary.Class)) ON (studentsProfile.admno = subjectReg.Student) AND (studentsProfile.admno = studentSummary.student)) ON Subjects.ID = subjectReg.SubjectsOfferred WHERE Session.ID =? AND Class.ID = ? and studentsprofile.admno = ?", con2)
        resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session.ID", Session("rsSession")))
        resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("rsClass")))
        resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student.ID", studentID))
        Dim resultqueryTableAdapter As New MySql.Data.MySqlClient.MySqlDataAdapter
        resultqueryTableAdapter.SelectCommand = resultquerycmd
        resultqueryTableAdapter.Fill(result)
        Dim dt As New ReportDataSource("DataSet1", result)
        e.DataSources.Add(dt)

        con2.Close()








    End Sub
End Class
