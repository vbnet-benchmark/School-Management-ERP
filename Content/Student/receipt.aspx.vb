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
        
        Dim result As New DataTable
        Dim rpt As New ReportViewer
        rpt.Reset()
        rpt.LocalReport.DataSources.Clear()
        Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from studentsprofile where admno = ?", con2)
            cmdSelect2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("Studentadd")))
        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2.ExecuteReader
        reader.Read()
        Dim pass As String = "https://" & Request.Url.Authority & Replace(reader(0).ToString, "~", "")


        reader.Close()
        Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT signature, logo from options", con2)

        Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
        reader3.Read()
        Dim logo As String = "https://" & Request.Url.Authority & Replace(reader3(1).ToString, "~", "")
        Dim authorized As String = "https://" & Request.Url.Authority & Replace(reader3(0).ToString, "~", "")
            reader3.Close()


        Dim images As New DataTable
        images.Columns.Add("logo")
            images.Columns.Add("passport")
            images.Columns.Add("authorized")

            images.Rows.Add(logo, pass, authorized)
        Dim dts As New ReportDataSource("DataSet2", images)
        rpt.LocalReport.DataSources.Add(dts)
            Dim resultquerycmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsProfile.surname, studentSummary.ID, Session.Session, studentSummary.Class, studentSummary.student, studentSummary.Age, studentSummary.Average, studentSummary.Position, studentSummary.classTeacherRemarks, studentSummary.principalRemarks, studentSummary.handWriting, studentSummary.fluency, studentSummary.games, studentSummary.sports, studentSummary.gymnastics, studentSummary.tools, studentSummary.drawing, studentSummary.crafts, studentSummary.musical, studentSummary.punctual, studentSummary.attendance, studentSummary.reliability, studentSummary.neatness, studentSummary.polite, studentSummary.honesty, studentSummary.relate, studentSummary.selfcontrol, studentSummary.cooperation, studentSummary.responsibility, studentSummary.attentiveness, studentSummary.initiative, studentSummary.organization, studentSummary.perseverance, studentsummary.aveage, studentSummary.Present, studentSummary.Absent, studentSummary.ClassNo, Session.Term, Class.Class AS Expr1, Session.TotalNoTerms, Session.NextTerm, Subjects.Subject, subjectReg.CA1, subjectReg.CA2, subjectReg.CA3, subjectreg.testtotal, subjectReg.Examination, subjectReg.Total, subjectReg.Highest, subjectReg.Lowest, subjectReg.Grade, subjectReg.Remarks, subjectReg.avg, subjectreg.pos, Session.ID AS Expr2, Class.ID AS Expr3, studentsProfile.admno AS Expr4, studentsprofile.passport as image, Subjects.ID AS SID, Session.ClosingDate, studentsummary.classhigh, studentsummary.classlow FROM Subjects INNER JOIN (studentsProfile INNER JOIN (Class INNER JOIN (studentSummary INNER JOIN (Session INNER JOIN subjectReg ON Session.ID = subjectReg.Session) ON studentSummary.Session = Session.ID) ON (Class.ID = subjectReg.Class) AND (Class.ID = studentSummary.Class)) ON (studentsProfile.admno = subjectReg.Student) AND (studentsProfile.admno = studentSummary.student)) ON Subjects.ID = subjectReg.SubjectsOfferred WHERE Session.ID =? and studentsprofile.admno = ?", con2)
            resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session.ID", Session("SessionId")))
            resultquerycmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student.ID", Session("Studentadd")))
            Dim resultqueryTableAdapter As New MySql.Data.MySqlClient.MySqlDataAdapter
            resultqueryTableAdapter.SelectCommand = resultquerycmd
            resultqueryTableAdapter.Fill(result)
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
            Dim cmdSelect2s As New MySql.Data.MySqlClient.MySqlCommand("SELECT fee, paid from feeschedule where student = '" & Session("Studentadd") & "' and session = '" & Session("sessionid") & "' and paid <> '" & 0 & "'", con2)
            Dim param As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2s.ExecuteReader

            Dim dsfees As New DataTable
            dsfees.Columns.Add("fee")
            dsfees.Columns.Add("amount")
            Dim total As Double = 0
            Do While param.Read
                total = total + param(1)
                dsfees.Rows.Add(param(0), FormatNumber(param(1), , , , TriState.True))
            Loop
            param.Close()
            
            Dim cmdSelect2rs As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cast(date as char) as date from transactions where student = '" & Session("Studentadd") & "' and account = 'BANK ACCOUNT' and session = '" & Session("sessionid") & "' order by date desc", con2)
            Dim paramr As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2rs.ExecuteReader
            Dim gtotal As Double = 0
            Dim order As Integer = 0
            Dim ldate As String = ""
            Do While paramr.Read
                If order = 0 Then
                    ldate = Convert.ToDateTime(paramr(1)).ToString("dd/MM/yyyy hh:mm tt")
                End If
                gtotal = gtotal + Val(Replace(paramr(0), ",", ""))
                order = order + 1
            Loop
            paramr.Close()
            Dim ct As Integer = 1
            Do While (gtotal - total) < 0
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cast(date as char) as date from transactions where student = '" & Session("Studentadd") & "' and account = 'BANK ACCOUNT' and session = '" & Session("sessionid") + ct & "' order by date asc", con2)
              
                Dim feereader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                Do While feereader3.Read
                    ldate = Convert.ToDateTime(feereader3(1)).ToString("dd/MM/yyyy hh:mm tt")
                    gtotal = gtotal + Val(Replace(feereader3(0), ",", ""))
                    If Not (gtotal - total) < 0 Then Exit Do
                Loop
                feereader3.Close()
                If Not (gtotal - total) < 0 Then
                    gtotal = total
                    Exit Do
                End If


                ct = ct + 1
            Loop
            Dim final As New DataTable
            final.Columns.Add("dates")
            final.Columns.Add("words")
            final.Rows.Add(ldate, AmountInWords(total) & " Only")
            Dim ddd As New ReportDataSource("DataSet4", final)
            rpt.LocalReport.DataSources.Add(ddd)

            dsfees.Rows.Add("TOTAL PAID", FormatNumber(total, , , , TriState.True))
            Dim dtss As New ReportDataSource("DataSet3", dsfees)

            rpt.LocalReport.DataSources.Add(dtss)
            rpt.ProcessingMode = ProcessingMode.Local

            rpt.LocalReport.ReportPath = Server.MapPath("~/content/Student/receipt.rdlc")


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
    Public Function AmountInWords(ByVal nAmount As String, Optional ByVal wAmount _
                 As String = vbNullString, Optional ByVal nSet As Object = Nothing) As String
        'Let's make sure entered value is numeric
        If Not IsNumeric(nAmount) Then Return "Please enter numeric values only."

        Dim tempDecValue As String = String.Empty : If InStr(nAmount, ".") Then _
            tempDecValue = nAmount.Substring(nAmount.IndexOf("."))
        nAmount = Replace(nAmount, tempDecValue, String.Empty)

        Try
            Dim intAmount As Long = nAmount
            If intAmount > 0 Then
                nSet = IIf((intAmount.ToString.Trim.Length / 3) _
                    > (CLng(intAmount.ToString.Trim.Length / 3)), _
                  CLng(intAmount.ToString.Trim.Length / 3) + 1, _
                    CLng(intAmount.ToString.Trim.Length / 3))
                Dim eAmount As Long = Microsoft.VisualBasic.Left(intAmount.ToString.Trim, _
                  (intAmount.ToString.Trim.Length - ((nSet - 1) * 3)))
                Dim multiplier As Long = 10 ^ (((nSet - 1) * 3))

                Dim Ones() As String = _
                {"", "One", "Two", "Three", _
                  "Four", "Five", _
                  "Six", "Seven", "Eight", "Nine"}
                Dim Teens() As String = {"", _
                "Eleven", "Twelve", "Thirteen", _
                  "Fourteen", "Fifteen", _
                  "Sixteen", "Seventeen", "Eighteen", "Nineteen"}
                Dim Tens() As String = {"", "Ten", _
                "Twenty", "Thirty", _
                  "Forty", "Fifty", "Sixty", _
                  "Seventy", "Eighty", "Ninety"}
                Dim HMBT() As String = {"", "", _
                "Thousand", "Million", _
                  "Billion", "Trillion", _
                  "Quadrillion", "Quintillion"}

                intAmount = eAmount

                Dim nHundred As Integer = intAmount \ 100 : intAmount = intAmount Mod 100
                Dim nTen As Integer = intAmount \ 10 : intAmount = intAmount Mod 10
                Dim nOne As Integer = intAmount \ 1

                If nHundred > 0 Then wAmount = wAmount & _
                Ones(nHundred) & " Hundred " 'This is for hundreds                
                If nTen > 0 Then 'This is for tens and teens
                    If nTen = 1 And nOne > 0 Then 'This is for teens 
                        wAmount = wAmount & Teens(nOne) & " "
                    Else 'This is for tens, 10 to 90
                        wAmount = wAmount & Tens(nTen) & IIf(nOne > 0, "-", " ")
                        If nOne > 0 Then wAmount = wAmount & Ones(nOne) & " "
                    End If
                Else 'This is for ones, 1 to 9
                    If nOne > 0 Then wAmount = wAmount & Ones(nOne) & " "
                End If
                wAmount = wAmount & HMBT(nSet) & " "
                wAmount = AmountInWords(CStr(CLng(nAmount) - _
                  (eAmount * multiplier)).Trim & tempDecValue, wAmount, nSet - 1)
            Else
                If Val(nAmount) = 0 Then nAmount = nAmount & _
                tempDecValue : tempDecValue = String.Empty
                If (Math.Round(Val(nAmount), 2) * 100) > 0 Then wAmount = _
                  Trim(AmountInWords(CStr(Math.Round(Val(nAmount), 2) * 100), _
                  wAmount.Trim & " Naira And ", 1)) & " Kobo"
            End If
        Catch ex As Exception
            MsgBox(ex.Message)
        End Try

        'Trap null values
        If IsNothing(wAmount) = True Then wAmount = String.Empty Else wAmount = _
          IIf(InStr(wAmount.Trim.ToLower, "naira"), _
          wAmount.Trim, wAmount.Trim & " Naira")

        'Display the result
        Return wAmount
    End Function

   
End Class
