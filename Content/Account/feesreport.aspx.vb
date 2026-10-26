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

    Protected Sub form1_Load(sender As Object, e As EventArgs) Handles form1.Load
        Using con2 As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)

            con2.Open()


            Dim result As New DataTable
            Dim cmdSelect2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class", con2)
            Dim resultqueryTableAdapter As New MySql.Data.MySqlClient.MySqlDataAdapter
            resultqueryTableAdapter.SelectCommand = cmdSelect2
            resultqueryTableAdapter.Fill(result)
            Dim dt As New ReportDataSource("DataSet1", result)
            Dim rpt As New ReportViewer





            Dim pass As String = "kkk"
            Dim logo As String = "https://" & Request.Url.Authority & "/Digital%20School/img/logo/logo.png"

            Dim images As New DataTable
            images.Columns.Add("logo")
            images.Columns.Add("passport")
            images.Rows.Add(logo, pass)
            rpt.ProcessingMode = ProcessingMode.Local
            rpt.Reset()
            rpt.LocalReport.DataSources.Clear()
            rpt.LocalReport.DataSources.Add(dt)

            con2.Close()
            rpt.LocalReport.ReportPath = Server.MapPath("~/content/Account/classdebtors.rdlc")
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
        End Using
    End Sub


    Public Sub MySubreportEventHandler(ByVal sender As Object, ByVal e As SubreportProcessingEventArgs)
        Using con2 As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)

            con2.Open()
            Dim studentID As String = e.Parameters(0).Values(0)
            Dim pl As New DataTable
            Dim incomeacc As New DataTable
            incomeacc.Columns.Add("sn")
            incomeacc.Columns.Add("admno")
            incomeacc.Columns.Add("class")
            incomeacc.Columns.Add("sname")
            incomeacc.Columns.Add("amount")

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT feeschedule.student, studentsprofile.surname, feeschedule.amount, feeschedule.paid, class.class from feeschedule inner join studentsprofile on feeschedule.student = studentsprofile.admno inner join class on feeschedule.class = class.Id where feeschedule.session = ? and class.class = ? order by studentsprofile.surname", con2)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Session("SessionId")))
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", studentID))
            Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim students As New ArrayList
            Dim total As Integer = 0
            Dim count As Integer = 0
            Dim currentstudent As String = ""
            Do While balreader.Read
                If balreader.Item(2) - balreader.Item(3) > 0 Then
                    If Not students.Contains(balreader(0).ToString) Then
                        total = 0
                        students.Add(balreader(0).ToString)
                        currentstudent = balreader(0).ToString
                        total = balreader.Item(2) - balreader.Item(3)
                        count = count + 1
                        incomeacc.Rows.Add(count, balreader.Item(0), balreader.Item(4), balreader.Item(1), FormatNumber(total, , , TriState.True))
                    Else
                        total = total + (balreader.Item(2) - balreader.Item(3))
                        incomeacc(count - 1).Item(4) = FormatNumber(total, , , TriState.True)
                    End If
                End If
            Loop
            balreader.Close()
            count = Nothing
            total = Nothing
            currentstudent = Nothing
            students = Nothing
            Dim dt As New ReportDataSource("DataSet1", incomeacc)
            e.DataSources.Add(dt)

            con2.Close()



        End Using




    End Sub



    

    
End Class
