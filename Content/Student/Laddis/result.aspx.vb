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
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsummary.class, session.term  from parentward inner join parentprofile on parentprofile.parentid = parentward.parent inner join (studentsummary inner join session on session.id = studentsummary.session) on parentward.ward = studentsummary.student inner join studentsprofile on parentward.ward = studentsprofile.admno where studentsummary.student = '" & Replace(a(0), "%2f", "/") & "' and studentsummary.session = '" & a(1) & "'", con2)
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            If reader2.Read() Then
                Session("rsClass") = reader2(0).ToString
                Session("rsSession") = a(1)
                Session("term") = reader2(1).ToString
                Session("StudentID") = Replace(a(0), "%2f", "/")
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
        Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport, dateofbirth from studentsprofile where admno = ?", con2)
        cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentid")))
        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
        Dim pass As String
        Dim dob As String
        Dim at As Array
        reader.Read()
        pass = "http://" & Request.Url.Authority & Replace(reader(0).ToString, "~", "")
        at = Split(reader(1).ToString, "/")
        dob = at(1) & "/" & at(0) & "/" & at(2)
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
        images.Columns.Add("dob")
        images.Rows.Add(logo, pass, authorized, dob)
        Dim dts As New ReportDataSource("DataSet2", images)
        rpt.LocalReport.DataSources.Add(dts)


        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT type from class where id = ?", con2)
        cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("rsClass")))
        Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
        studentsReader.Read()
        Dim clatype As String = studentsReader(0).ToString
        studentsReader.Close()
        If clatype = "Early Years" Then

            Dim resultquerycmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsProfile.surname, studentsprofile.sex, studentsprofile.dateofbirth as dob, studentSummary.ID, Session.Session, class.Class, studentSummary.student, studentSummary.Age, studentSummary.Average, studentSummary.Position, studentSummary.classTeacherRemarks, studentSummary.Present, studentSummary.Absent, studentSummary.ClassNo, Session.Term, Class.Class AS Expr1, Session.TotalNoTerms, Session.NextTerm, Session.ID AS Expr2, Class.ID AS Expr3, studentsProfile.admno AS Expr4, studentsprofile.passport as image,  Session.ClosingDate, studentsummary.classhigh, studentsummary.classlow, studentsummary.homework, studentsummary.late, session.halfclose, session.halfresume FROM studentsummary inner join studentsProfile on studentsummary.student = studentsprofile.admno INNER JOIN Class on studentsummary.class = class.id inner join session on studentsummary.session = session.id  WHERE Session.ID =? AND Class.ID = ? and studentsprofile.admno = ?", con2)
            resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session.ID", Session("rsSession")))
            resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("rsClass")))
            resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student.ID", Session("StudentID")))
            Dim resultqueryTableAdapter As New MySql.Data.MySqlClient.MySqlDataAdapter
            resultqueryTableAdapter.SelectCommand = resultquerycmd
            resultqueryTableAdapter.Fill(result)
            Dim dt As New ReportDataSource("DataSet1", result)
            rpt.LocalReport.DataSources.Add(dt)

            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Sessioncreate where sessionname = '" & Session("SessionName") & "' Order by ID Desc", con2)
            Dim reader2b As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
            reader2b.Read()
            Dim tissession = reader2b(0)
            reader2b.Close()
            Dim result1 As New DataTable

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT elsubtopics.subtopic, eltopics.topic,  elscores.grade, elscores.grade2, elscores.grade3, elscores.comments, elscores.comments2, elscores.comments3, elscores.recommendation, elscores.recommendation2, elscores.recommendation3,  elscores.student, session.term from elscores inner join eltopics on eltopics.id = elscores.topic inner join elsubtopics on elsubtopics.id = elscores.subtopic inner join (sessioncreate inner join session on sessioncreate.sessionname = session.session) on elscores.session = sessioncreate.id where elscores.student = '" & Session("studentid") & "' and elscores.session = '" & tissession & "' and session.id = '" & Session("sessionid") & "'", con2)

            Dim resultqueryTableAdapters As New MySql.Data.MySqlClient.MySqlDataAdapter
            resultqueryTableAdapters.SelectCommand = cmdLoad1
            resultqueryTableAdapters.Fill(result1)
            Dim dt1 As New ReportDataSource("DataSet3", result1)
            rpt.LocalReport.DataSources.Add(dt1)




            rpt.ProcessingMode = ProcessingMode.Local

            If Session("term") = "1st term" Then
                rpt.LocalReport.ReportPath = Server.MapPath("~/content/Student/midspresult.rdlc")
            ElseIf Session("term") = "2nd term" Then
                rpt.LocalReport.ReportPath = Server.MapPath("~/content/Student/midspresult2.rdlc")
            Else
                rpt.LocalReport.ReportPath = Server.MapPath("~/content/Student/midspresult3.rdlc")
            End If
            AddHandler rpt.LocalReport.SubreportProcessing, AddressOf MySubreportEventHandler

            rpt.LocalReport.EnableExternalImages = True
            rpt.LocalReport.Refresh()



        Else

            Dim resultquerycmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsProfile.surname, studentSummary.ID, Session.Session, studentSummary.Class, studentSummary.student, studentSummary.Age, studentSummary.Average, studentSummary.Position, studentSummary.classTeacherRemarks, studentSummary.principalRemarks, studentSummary.handWriting, studentSummary.fluency, studentSummary.games, studentSummary.sports, studentSummary.gymnastics, studentSummary.tools, studentSummary.drawing, studentSummary.crafts, studentSummary.musical, studentSummary.punctual, studentSummary.attendance, studentSummary.reliability, studentSummary.neatness, studentSummary.polite, studentSummary.honesty, studentSummary.relate, studentSummary.selfcontrol, studentSummary.cooperation, studentSummary.responsibility, studentSummary.attentiveness, studentSummary.initiative, studentSummary.organization, studentSummary.perseverance, studentsummary.aveage, studentSummary.Present, studentSummary.Absent, studentSummary.ClassNo, Session.Term, Class.Class AS Expr1, Session.TotalNoTerms, Session.NextTerm, Subjects.Subject, subjectReg.CA1, subjectReg.CA2, subjectReg.CA3, subjectreg.project, subjectreg.testtotal, subjectreg.testremrks, subjectReg.Examination, subjectReg.Total, subjectReg.Highest, subjectReg.Lowest, subjectReg.Grade, subjectReg.Remarks, subjectReg.avg, subjectreg.pos, Session.ID AS Expr2, Class.ID AS Expr3, studentsProfile.admno AS Expr4, studentsprofile.passport as image, Subjects.ID AS SID, Session.ClosingDate, studentsummary.classhigh, studentsummary.classlow, subjectreg.lastscore, subjectreg.lastscore2, subjectreg.classwork, studentsummary.homework, studentsummary.tardiness, studentsummary.behaviour, studentsummary.extra, studentsummary.project as proj, studentsummary.strength, studentsummary.improvement, studentsummary.teacher, studentsummary.par, studentsummary.late, subjectreg.restest, subjectreg.ass, subjectreg.test1, subjectreg.test2 FROM Subjects INNER JOIN (studentsProfile INNER JOIN (Class INNER JOIN (studentSummary INNER JOIN (Session INNER JOIN subjectReg ON Session.ID = subjectReg.Session) ON studentSummary.Session = Session.ID) ON (Class.ID = subjectReg.Class) AND (Class.ID = studentSummary.Class)) ON (studentsProfile.admno = subjectReg.Student) AND (studentsProfile.admno = studentSummary.student)) ON Subjects.ID = subjectReg.SubjectsOfferred WHERE Session.ID =? AND Class.ID = ? and studentsprofile.admno = ? and subjectreg.nested = '" & 0 & "'", con2)
            resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session.ID", Session("rsSession")))
            resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("rsClass")))
            resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student.ID", Session("StudentID")))
            Dim resultqueryTableAdapter As New MySql.Data.MySqlClient.MySqlDataAdapter
            resultqueryTableAdapter.SelectCommand = resultquerycmd
            resultqueryTableAdapter.Fill(result)
            Dim resultquerycmds As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsProfile.surname, studentSummary.ID, Session.Session, studentSummary.Class, studentSummary.student, studentSummary.Age, studentSummary.Average, studentSummary.Position, studentSummary.classTeacherRemarks, studentSummary.principalRemarks, studentSummary.handWriting, studentSummary.fluency, studentSummary.games, studentSummary.sports, studentSummary.gymnastics, studentSummary.tools, studentSummary.drawing, studentSummary.crafts, studentSummary.musical, studentSummary.punctual, studentSummary.attendance, studentSummary.reliability, studentSummary.neatness, studentSummary.polite, studentSummary.honesty, studentSummary.relate, studentSummary.selfcontrol, studentSummary.cooperation, studentSummary.responsibility, studentSummary.attentiveness, studentSummary.initiative, studentSummary.organization, studentSummary.perseverance, studentsummary.aveage, studentSummary.Present, studentSummary.Absent, studentSummary.ClassNo, Session.Term, Class.Class AS Expr1, Session.TotalNoTerms, Session.NextTerm, subjectnest.name as Subject, subjectReg.CA1, subjectReg.CA2, subjectReg.CA3, subjectreg.project, subjectreg.testtotal, subjectreg.testremrks, subjectReg.Examination, subjectReg.Total, subjectReg.Highest, subjectReg.Lowest, subjectReg.Grade, subjectReg.Remarks, subjectReg.avg, subjectreg.pos, Session.ID AS Expr2, Class.ID AS Expr3, studentsProfile.admno AS Expr4, studentsprofile.passport as image, Subjectreg.ID AS SID, Session.ClosingDate, studentsummary.classhigh, studentsummary.classlow, subjectreg.lastscore, subjectreg.lastscore2, subjectreg.classwork, studentsummary.homework, studentsummary.tardiness, studentsummary.behaviour, studentsummary.extra, studentsummary.project as proj, studentsummary.strength, studentsummary.improvement, studentsummary.teacher, studentsummary.par, studentsummary.late  FROM Subjectreg inner join subjectnest on subjectnest.id = subjectreg.subjectsofferred inner join studentsummary on studentsummary.student = subjectreg.student and studentsummary.session = subjectreg.session inner join studentsprofile on studentsprofile.admno = subjectreg.student inner join class on subjectreg.class = class.id inner join session on subjectreg.session = session.id WHERE Subjectreg.session =? AND subjectreg.class = ? and studentsprofile.admno = ? and subjectreg.nested = '" & 1 & "'", con2)
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
            Dim parameter4 As New DataTable
            parameter4.Columns.Add("parameter")
            parameter4.Columns.Add("value")
            Dim cmdSelect2s As New MySql.Data.MySqlClient.MySqlCommand("SELECT traits.trait, termtraits.value from termtraits inner join traits on traits.id = termtraits.trait where termtraits.session = '" & Session("rsSession") & "' and termtraits.student = '" & Session("StudentID") & "'", con2)
            Dim param As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2s.ExecuteReader
            Dim i As Integer = 1
            Do While param.Read
                If i <= 5 Then
                    parameter1.Rows.Add(param(0), param(1))
                ElseIf i > 5 And i <= 10 Then
                    parameter2.Rows.Add(param(0), param(1))
                ElseIf i > 10 And i <= 15 Then
                    parameter3.Rows.Add(param(0), param(1))
                Else
                    parameter4.Rows.Add(param(0), param(1))
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
            Dim dt4 As New ReportDataSource("Behaviour4", parameter4)
            rpt.LocalReport.DataSources.Add(dt4)


            rpt.ProcessingMode = ProcessingMode.Local

            If Session("term") = "3rd term" Then
                Dim cumm As New DataTable
                Dim resultquerycmds1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.id as SID, subjects.subject, lastscore as second, lastscore2 as first, total as third, cumaverage, cumgrade, cumremarks from subjectreg inner join subjects on subjectreg.subjectsofferred = subjects.id WHERE Subjectreg.session = ? AND subjectreg.class = ? and subjectreg.student = ? and subjectreg.nested = '" & 0 & "'", con2)
                resultquerycmds1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session.ID", Session("rsSession")))
                resultquerycmds1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("rsClass")))
                resultquerycmds1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student.ID", Session("StudentID")))
                Dim resultqueryTableAdapters1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                resultqueryTableAdapters1.SelectCommand = resultquerycmds1
                resultqueryTableAdapters1.Fill(cumm)

                Dim dt0 As New ReportDataSource("DataSet3", cumm)
                rpt.LocalReport.DataSources.Add(dt0)

            End If

            If Session("rPeriod") = "Half Term" Then
                rpt.LocalReport.ReportPath = Server.MapPath("~/content/Student/midgenreport.rdlc")
            ElseIf Session("term") = "3rd term" Then
                rpt.LocalReport.ReportPath = Server.MapPath("~/content/Student/cumreport.rdlc")
            ElseIf val(Session("rsSession")) < 46 Then
            rpt.LocalReport.ReportPath = Server.MapPath("~/content/Student/report3f.rdlc")
            Else
                rpt.LocalReport.ReportPath = Server.MapPath("~/content/Student/report3.rdlc")
            End If

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
        con2.Close()
    End Sub
    Public Sub MySubreportEventHandler(ByVal sender As Object, ByVal e As SubreportProcessingEventArgs)
        Using con2 As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)

            con2.Open()
            Dim topic As String = e.Parameters(0).Values(0)
            Dim result1 As New DataTable

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT eltopics.topic, elsubtopics.subtopic, elscores.grade, elscores.comments as remarks, elscores.student from elscores inner join subjects on subjects.id = elscores.subject inner join eltopics on elscores.topic = eltopics.id inner join elsubtopics on elsubtopics.id = elscores.subtopic where elscores.student = '" & Session("studentid") & "' and eltopics.topic = '" & topic & "' and elscores.session = '" & Session("rssession") & "'", con2)
          
            Dim resultqueryTableAdapters As New MySql.Data.MySqlClient.MySqlDataAdapter
            resultqueryTableAdapters.SelectCommand = cmdLoad1
            resultqueryTableAdapters.Fill(result1)
            Dim dt1 As New ReportDataSource("DataSet1", result1)
            e.DataSources.Add(dt1)

            con2.Close()



        End Using




    End Sub

   
End Class
