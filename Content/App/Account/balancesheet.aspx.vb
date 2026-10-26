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
        Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT signature, logo from options", con2)

        Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
        reader3.Read()
        Dim logo As String = "http://" & Request.Url.Authority &  Replace(reader3(1).ToString, "~", "")
        Dim authorized As String = "http://" & Request.Url.Authority &  Replace(reader3(0).ToString, "~", "")
        reader3.Close()
        Dim pass As String = ""

        Dim images As New DataTable
        images.Columns.Add("logo")
        images.Columns.Add("passport")
        images.Columns.Add("authorized")

        images.Rows.Add(logo, pass, authorized)
        Dim dts As New ReportDataSource("DataSet4", images)




        Dim pl As New DataTable
        Dim incomeacc As New DataTable
        pl.Columns.Add("profit")
        incomeacc.Columns.Add("account")
        incomeacc.Columns.Add("balance")

        Dim equityacc As New DataTable
        equityacc.Columns.Add("account")
        equityacc.Columns.Add("balance")

        Dim expenseacc As New DataTable
        expenseacc.Columns.Add("account")
        expenseacc.Columns.Add("balance")
        Dim accounts As New ArrayList
        Dim income As String = "Cash"
        Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings where type = '" & income & "'", con2)
        Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
        accounts.Clear()

        Do While student04.Read
            accounts.Add(student04.Item("Accname"))
        Loop
        Dim expensetotal As Double
        Dim incometotal As Double
        Dim equitytotal As Double
        student04.Close()
        For Each item As String In accounts
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? order by date desc", con2)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
            Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim dr As Integer
            Dim cr As Integer
            Dim balance As Double = 0
            Do While balreader.Read
                dr = dr + Val(balreader.Item("dr").replace(",", ""))
                cr = cr + Val(balreader.Item("cr").replace(",", ""))
            Loop
            balreader.Close()
            balance = dr - cr

            Dim cmdLoad1s As New MySql.Data.MySqlClient.MySqlCommand("SELECT initial from accsettings where accname = ?", con2)
            cmdLoad1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
            Dim balreaders As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1s.ExecuteReader
            balreaders.Read()
            balance = balance + Val(balreaders(0))
            incometotal = incometotal + balance
            incomeacc.Rows.Add(item, balance)
            cr = Nothing
            dr = Nothing
            balance = Nothing
            balreaders.Close()
        Next



        income = "Fixed Assets"
        Dim cmdLoad04 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings where type = '" & income & "'", con2)
        Dim student040 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad04.ExecuteReader
        accounts.Clear()

        Do While student040.Read
            accounts.Add(student040.Item("Accname"))
        Loop
        student040.Close()
        For Each item As String In accounts
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? order by date desc", con2)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
            Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim dr As Integer
            Dim cr As Integer
            Dim balance As Double = 0
            Do While balreader.Read
                dr = dr + Val(balreader.Item("dr").replace(",", ""))
                cr = cr + Val(balreader.Item("cr").replace(",", ""))
            Loop
            balance = dr - cr
            balreader.Close()
            Dim cmdLoad1s As New MySql.Data.MySqlClient.MySqlCommand("SELECT initial from accsettings where accname = ?", con2)
            cmdLoad1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
            Dim balreaders As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1s.ExecuteReader
            balreaders.Read()
            balance = balance + Val(balreaders(0))
            incometotal = incometotal + balance
            incomeacc.Rows.Add(item, balance)
            cr = Nothing
            dr = Nothing
            balance = Nothing
            balreaders.Close()

        Next



        income = "Receivable"
        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings where type = '" & income & "'", con2)
        Dim student4 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
        accounts.Clear()

        Do While student4.Read
            accounts.Add(student4.Item("Accname"))
        Loop
        student4.Close()
        For Each item As String In accounts
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? order by date desc", con2)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
            Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim dr As Integer
            Dim cr As Integer
            Dim balance As Double = 0
            Do While balreader.Read
                dr = dr + Val(balreader.Item("dr").replace(",", ""))
                cr = cr + Val(balreader.Item("cr").replace(",", ""))
            Loop
            balance = dr - cr
            balreader.Close()
            Dim cmdLoad1s As New MySql.Data.MySqlClient.MySqlCommand("SELECT initial from accsettings where accname = ?", con2)
            cmdLoad1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
            Dim balreaders As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1s.ExecuteReader
            balreaders.Read()
            balance = balance + Val(balreaders(0))
            incometotal = incometotal + balance
            incomeacc.Rows.Add(item, balance)
            cr = Nothing
            dr = Nothing
            balance = Nothing
            balreaders.Close()

        Next
        incomeacc.Rows.Add("TOTAL ASSETS", incometotal)




        Dim expense As String = "liability"
        Dim cmdLoad5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings where type = '" & expense & "'", con2)
        Dim student05 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad5.ExecuteReader
        accounts.Clear()

        Do While student05.Read
            accounts.Add(student05.Item("Accname"))
        Loop
        student05.Close()
        For Each item As String In accounts
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? order by date desc", con2)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
            Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim dr As Integer
            Dim cr As Integer
            Dim balance As Double = 0
            Do While balreader.Read
                dr = dr + Val(balreader.Item("dr").replace(",", ""))
                cr = cr + Val(balreader.Item("cr").replace(",", ""))
            Loop
            balreader.Close()
            balance = cr - dr
            Dim cmdLoad1s As New MySql.Data.MySqlClient.MySqlCommand("SELECT initial from accsettings where accname = ?", con2)
            cmdLoad1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
            Dim balreaders As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1s.ExecuteReader
            balreaders.Read()
            balance = balance + Val(balreaders(0))
            expensetotal = expensetotal + balance
            expenseacc.Rows.Add(item, balance)

            balance = Nothing
            cr = Nothing
            dr = Nothing
            balreaders.Close()

        Next
        Dim profit As Double = incometotal - expensetotal
        pl.Rows.Add(profit)
        expenseacc.Rows.Add("TOTAL LIABILITY", expensetotal)


        Dim equity As String = "equity"
        Dim cmdLoad50 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings where type = '" & equity & "'", con2)
        Dim student050 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad50.ExecuteReader
        accounts.Clear()

        Do While student050.Read
            accounts.Add(student050.Item("Accname"))
        Loop
        student050.Close()
        For Each item As String In accounts
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? order by date desc", con2)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
            Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim dr As Integer
            Dim cr As Integer
            Dim balance As Double = 0
            Do While balreader.Read
                dr = dr + Val(balreader.Item("dr").replace(",", ""))
                cr = cr + Val(balreader.Item("cr").replace(",", ""))
            Loop
            balreader.Close()
            balance = cr - dr
            Dim cmdLoad1s As New MySql.Data.MySqlClient.MySqlCommand("SELECT initial from accsettings where accname = ?", con2)
            cmdLoad1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
            Dim balreaders As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1s.ExecuteReader
            balreaders.Read()
            balance = balance + Val(balreaders(0))
            expensetotal = expensetotal + balance
            equityacc.Rows.Add(item, balance)

            balance = Nothing
            cr = Nothing
            dr = Nothing
            balreaders.Close()

        Next
        Dim x As Double = retained_earnings()
        Dim remainder As Double = incometotal - (expensetotal + x)
        equityacc.Rows.Add("RETAINED EARNINGS", x + remainder)
        equityacc.Rows.Add("TOTAL EQUITY", expensetotal + x + remainder)
        Dim dt As New ReportDataSource("DataSet1", incomeacc)
        Dim dt2 As New ReportDataSource("DataSet2", expenseacc)
        Dim dt3 As New ReportDataSource("DataSet3", equityacc)
        Dim rpt As New ReportViewer
        rpt.ProcessingMode = ProcessingMode.Local
        rpt.Reset()
        rpt.LocalReport.DataSources.Clear()
        rpt.LocalReport.DataSources.Add(dt)
        rpt.LocalReport.DataSources.Add(dt2)
        rpt.LocalReport.DataSources.Add(dt3)
        rpt.LocalReport.DataSources.Add(dts)
        con2.Close()
        rpt.LocalReport.ReportPath = Server.MapPath("~/content/app/Account/balancesheet.rdlc")
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
    Function retained_earnings() As Double
        Dim accounts As New ArrayList
        Dim income As String = "Income"
        Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings where type = '" & income & "'", con2)
        Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
        accounts.Clear()

        Do While student04.Read
            accounts.Add(student04.Item("Accname"))
        Loop
        Dim expensetotal As Double
        Dim incometotal As Double = 0
        student04.Close()
        For Each item As String In accounts
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? order by date desc", con2)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
            Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim dr As Integer
            Dim cr As Integer
            Dim balance As Double = 0
            Do While balreader.Read
                dr = dr + Val(balreader.Item("dr").replace(",", ""))
                cr = cr + Val(balreader.Item("cr").replace(",", ""))
            Loop
            balance = cr - dr
            incometotal = incometotal + balance
            balreader.Close()
            cr = Nothing
            dr = Nothing
            balance = Nothing
        Next

        Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where type = ? order by date desc", con2)
        cmdLoad0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", "Stock"))
        Dim balreader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
        Dim o As Integer = 0
        Do While balreader0.Read
            o = o + Val(balreader0.Item("cr").replace(",", ""))
        Loop
        incometotal = incometotal + o
        balreader0.Close()
        o = Nothing
        Dim expense As String = "Expense"
        Dim cmdLoad5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT accname from accsettings where type = '" & expense & "'", con2)
        Dim student05 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad5.ExecuteReader
        accounts.Clear()

        Do While student05.Read
            accounts.Add(student05.Item("Accname"))
        Loop
        student05.Close()
        For Each item As String In accounts
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dr, cr from transactions where account = ? order by date desc", con2)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
            Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim dr As Integer
            Dim cr As Integer
            Dim balance As Double = 0
            Do While balreader.Read
                dr = dr + Val(balreader.Item("dr").replace(",", ""))
                cr = cr + Val(balreader.Item("cr").replace(",", ""))
            Loop
            balance = dr - cr
            expensetotal = expensetotal + balance
            balreader.Close()
            balance = Nothing
            cr = Nothing
            dr = Nothing
        Next

        Dim profit As Double = incometotal - expensetotal
        Return profit
    End Function

    
End Class
