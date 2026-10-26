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
    Dim logify As New notify
    Protected Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load
    
        con2.Open()
        If Not Request.QueryString.ToString = Nothing Then
            Dim a As Array = Split(Request.QueryString.ToString, "%2c")
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsummary.class  from parentward inner join parentprofile on parentprofile.parentid = parentward.parent inner join studentsummary on parentward.ward = studentsummary.student inner join studentsprofile on parentward.ward = studentsprofile.admno where studentsummary.student = '" & a(0) & "' and studentsummary.status = '" & 1 & "' and studentsummary.session = '" & a(1) & "'", con2)
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            If reader2.Read() Then
                Session("rsClass") = reader2(0).ToString
                Session("rsSession") = a(1)
                    Session("StudentID") = a(0)
                    logify.Read_notification("~/content/student/result.aspx?" & Session("Studentid") & "," & Session("rsSession"), Session("parentid"))
                    logify.Read_notification("~/content/student/result.aspx?" & Session("Studentid") & "," & Session("rsSession"), Session("studentid"))

            Else
                    Response.Write("You are not authorized to view this page.")
                    Exit Sub
            End If
            reader2.Close()

        End If
        Dim result As New DataTable
        Dim rpt As New ReportViewer
        rpt.Reset()
        rpt.LocalReport.DataSources.Clear()
        If Session("studentadd") <> Nothing Then Session("studentid") = Session("studentadd")
            Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from studentsprofile where admno = ?", con2)
        cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentid")))
        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
        Dim pass As String
        If reader.Read() Then
            pass = "http://" & Request.Url.Authority & Replace(reader(0).ToString, "~", "")
        End If
        reader.Close()
        Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT signature, logo from options", con2)

        Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
        reader3.Read()
        Dim logo As String = "http://" & Request.Url.Authority & Replace(reader3(1).ToString, "~", "")
        Dim authorized As String = "http://" & Request.Url.Authority & Replace(reader3(0).ToString, "~", "")
        reader3.Close()


        Dim images As New DataTable
        images.Columns.Add("logo")
        images.Columns.Add("passport")
        images.Columns.Add("authorized")

        images.Rows.Add(logo, pass, authorized)
        Dim dts As New ReportDataSource("DataSet2", images)
        rpt.LocalReport.DataSources.Add(dts)


        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT type from class where id = ?", con2)
        cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("rsClass")))
        Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
        studentsReader.Read()
        Dim clatype As String = studentsReader(0).ToString
        studentsReader.Close()
        If clatype <> "K.G 1 Special" Then
            Dim resultquerycmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsProfile.surname, studentSummary.ID, Session.Session, studentSummary.Class, studentSummary.student, studentSummary.Age, studentSummary.Average, studentSummary.Position, studentSummary.classTeacherRemarks, studentSummary.principalRemarks, studentSummary.handWriting, studentSummary.fluency, studentSummary.games, studentSummary.sports, studentSummary.gymnastics, studentSummary.tools, studentSummary.drawing, studentSummary.crafts, studentSummary.musical, studentSummary.punctual, studentSummary.attendance, studentSummary.reliability, studentSummary.neatness, studentSummary.polite, studentSummary.honesty, studentSummary.relate, studentSummary.selfcontrol, studentSummary.cooperation, studentSummary.responsibility, studentSummary.attentiveness, studentSummary.initiative, studentSummary.organization, studentSummary.perseverance, studentsummary.aveage, studentSummary.Present, studentSummary.Absent, studentSummary.ClassNo, Session.Term, Class.Class AS Expr1, Session.TotalNoTerms, Session.NextTerm, Subjects.Subject, subjectReg.CA1, subjectReg.CA2, subjectReg.CA3, subjectreg.testtotal, subjectReg.Examination, subjectReg.Total, subjectReg.Highest, subjectReg.Lowest, subjectReg.Grade, subjectReg.Remarks, subjectReg.avg, subjectreg.pos, Session.ID AS Expr2, Class.ID AS Expr3, studentsProfile.admno AS Expr4, studentsprofile.passport as image, Subjects.ID AS SID, Session.ClosingDate, studentsummary.classhigh, studentsummary.classlow FROM Subjects INNER JOIN (studentsProfile INNER JOIN (Class INNER JOIN (studentSummary INNER JOIN (Session INNER JOIN subjectReg ON Session.ID = subjectReg.Session) ON studentSummary.Session = Session.ID) ON (Class.ID = subjectReg.Class) AND (Class.ID = studentSummary.Class)) ON (studentsProfile.admno = subjectReg.Student) AND (studentsProfile.admno = studentSummary.student)) ON Subjects.ID = subjectReg.SubjectsOfferred WHERE Session.ID =? AND Class.ID = ? and studentsprofile.admno = ? and subjectreg.nested = '" & 0 & "'", con2)
            resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session.ID", Session("rsSession")))
            resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("rsClass")))
            resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student.ID", Session("StudentID")))
            Dim resultqueryTableAdapter As New MySql.Data.MySqlClient.MySqlDataAdapter
            resultqueryTableAdapter.SelectCommand = resultquerycmd
            resultqueryTableAdapter.Fill(result)
            Dim resultquerycmds As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsProfile.surname, studentSummary.ID, Session.Session, studentSummary.Class, studentSummary.student, studentSummary.Age, studentSummary.Average, studentSummary.Position, studentSummary.classTeacherRemarks, studentSummary.principalRemarks, studentSummary.handWriting, studentSummary.fluency, studentSummary.games, studentSummary.sports, studentSummary.gymnastics, studentSummary.tools, studentSummary.drawing, studentSummary.crafts, studentSummary.musical, studentSummary.punctual, studentSummary.attendance, studentSummary.reliability, studentSummary.neatness, studentSummary.polite, studentSummary.honesty, studentSummary.relate, studentSummary.selfcontrol, studentSummary.cooperation, studentSummary.responsibility, studentSummary.attentiveness, studentSummary.initiative, studentSummary.organization, studentSummary.perseverance, studentsummary.aveage, studentSummary.Present, studentSummary.Absent, studentSummary.ClassNo, Session.Term, Class.Class AS Expr1, Session.TotalNoTerms, Session.NextTerm, subjectnest.name as Subject, subjectReg.CA1, subjectReg.CA2, subjectReg.CA3, subjectreg.testtotal, subjectReg.Examination, subjectReg.Total, subjectReg.Highest, subjectReg.Lowest, subjectReg.Grade, subjectReg.Remarks, subjectReg.avg, subjectreg.pos, Session.ID AS Expr2, Class.ID AS Expr3, studentsProfile.admno AS Expr4, studentsprofile.passport as image, Subjectreg.ID AS SID, Session.ClosingDate, studentsummary.classhigh, studentsummary.classlow FROM Subjectreg inner join subjectnest on subjectnest.id = subjectreg.subjectsofferred inner join studentsummary on studentsummary.student = subjectreg.student and studentsummary.session = subjectreg.session inner join studentsprofile on studentsprofile.admno = subjectreg.student inner join class on subjectreg.class = class.id inner join session on subjectreg.session = session.id WHERE Subjectreg.session =? AND subjectreg.class = ? and studentsprofile.admno = ? and subjectreg.nested = '" & 1 & "'", con2)
            resultquerycmds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session.ID", Session("rsSession")))
            resultquerycmds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("rsClass")))
            resultquerycmds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student.ID", Session("StudentID")))
            Dim resultqueryTableAdapters As New MySql.Data.MySqlClient.MySqlDataAdapter
            resultqueryTableAdapters.SelectCommand = resultquerycmds
            resultqueryTableAdapters.Fill(result)

            Dim dt As New ReportDataSource("DataSet1", result)
            rpt.LocalReport.DataSources.Add(dt)
            Dim parameter1 As New DataTable
            parameter1.Columns.Add("parameter")
            parameter1.Columns.Add("value")
            Dim parameter2 As New DataTable
            parameter2.Columns.Add("parameter")
            parameter2.Columns.Add("value")
            Dim parameter3 As New DataTable
            parameter3.Columns.Add("parameter")
            parameter3.Columns.Add("value")
            Dim cmdSelect2s As New MySql.Data.MySqlClient.MySqlCommand("SELECT traits.trait, termtraits.value from termtraits inner join traits on traits.id = termtraits.trait where termtraits.session = '" & Session("rsSession") & "' and termtraits.student = '" & Session("StudentID") & "'", con2)
            Dim param As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2s.ExecuteReader
            Dim i As Integer
            Do While param.Read
                If i <= 7 Then
                    parameter1.Rows.Add(param(0), param(1))
                ElseIf i > 7 And i <= 15 Then
                    parameter2.Rows.Add(param(0), param(1))
                Else
                    parameter3.Rows.Add(param(0), param(1))
                End If
                i = i + 1
            Loop
            param.Close()
            Dim dt1 As New ReportDataSource("Behaviour1", parameter1)
            rpt.LocalReport.DataSources.Add(dt1)
            Dim dt2 As New ReportDataSource("Behaviour2", parameter2)
            rpt.LocalReport.DataSources.Add(dt2)
            Dim dt3 As New ReportDataSource("Behaviour3", parameter3)
            rpt.LocalReport.DataSources.Add(dt3)



            rpt.ProcessingMode = ProcessingMode.Local
            Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cano from class Where id = ?", con2)
            cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("rsClass")))
            Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
            reader20.Read()
            Dim caquery As Integer = 2
            If reader20(0) = 3 Then
                caquery = 3
            ElseIf reader20(0) = 4 Then
                caquery = 4
            End If
            reader20.Close()
            con2.Close()
            If clatype = "Primary 6" Then
                rpt.LocalReport.ReportPath = Server.MapPath("~/content/Student/grade6sp.rdlc")
            Else
                rpt.LocalReport.ReportPath = Server.MapPath("~/content/Student/report32.rdlc")
            End If

        Else
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.admno, studentsprofile.surname, subjects.subject, kcourseoutline.topic, kscoresheet.grade, kscoresheet.remarks, session.session, session.term, session.nextterm, class.class, studentsummary.classno, studentsummary.age, studentsummary.aveage, studentsummary.principalremarks, studentsummary.classteacherremarks from kscoresheet inner join kcourseoutline on kcourseoutline.id = kscoresheet.topic inner join subjects on kscoresheet.subject = subjects.id inner join class on kscoresheet.class = class.id inner join (studentsprofile inner join studentsummary on studentsprofile.admno = studentsummary.student) on kscoresheet.student = studentsprofile.admno inner join session on session.id = kscoresheet.session where kscoresheet.student = ? and kscoresheet.session = ? order by subjects.id", con2)
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", Session("Studentid")))
            cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("term", Session("SessionId")))
            Dim resultqueryTableAdapter As New MySql.Data.MySqlClient.MySqlDataAdapter
            resultqueryTableAdapter.SelectCommand = cmd
            resultqueryTableAdapter.Fill(result)
            Dim dt As New ReportDataSource("DataSet1", result)
            rpt.LocalReport.DataSources.Add(dt)
            rpt.ProcessingMode = ProcessingMode.Local

            rpt.LocalReport.ReportPath = Server.MapPath("~/content/Student/spresult.rdlc")
        End If

        PlaceHolder1.Controls.Add(rpt)

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


   
End Class
