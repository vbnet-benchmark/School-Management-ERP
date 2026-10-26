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
        Dim logo As String = "https://" & Request.Url.Authority &  Replace(reader3(1).ToString, "~", "")
        Dim authorized As String = "https://" & Request.Url.Authority &  Replace(reader3(0).ToString, "~", "")
        reader3.Close()
        Dim pass As String = ""

        Dim images As New DataTable
        images.Columns.Add("logo")
        images.Columns.Add("passport")
        images.Columns.Add("authorized")

        images.Rows.Add(logo, pass, authorized)
        Dim dts As New ReportDataSource("DataSet4", images)





        Dim incomeacc As New DataTable

        incomeacc.Columns.Add("account")
        incomeacc.Columns.Add("balance")
        Dim accounts As New ArrayList
       
        Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT item from stock", con2)
        Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
        accounts.Clear()

        Do While student04.Read
            accounts.Add(student04.Item("item"))
        Loop
       
        student04.Close()
        For Each item As String In accounts

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT stockadjust.added, stockadjust.removed from stockadjust inner join stock on stock.id = stockadjust.item where stock.item = ?", con2)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
            Dim balreader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim dr As Integer = 0
            Dim cr As Integer = 0
            Dim balance As Double = 0
            Do While balreader.Read
                dr = dr + Val(balreader.Item("added"))
                cr = cr + Val(balreader.Item("removed"))
            Loop
            balreader.Close()
            balance = dr - cr

            Dim cmdLoad1s As New MySql.Data.MySqlClient.MySqlCommand("SELECT quantity from stock where item = ?", con2)
            cmdLoad1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("acc", item))
            Dim balreaders As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1s.ExecuteReader
            balreaders.Read()
            balance = balance + Val(balreaders(0))
            incomeacc.Rows.Add(item, balance)
            cr = Nothing
            dr = Nothing
            balance = Nothing
            balreaders.Close()
        Next

        Dim dt As New ReportDataSource("DataSet1", incomeacc)
       
        Dim rpt As New ReportViewer
        rpt.ProcessingMode = ProcessingMode.Local
        rpt.Reset()
        rpt.LocalReport.DataSources.Clear()
        rpt.LocalReport.DataSources.Add(dt)
        
        rpt.LocalReport.DataSources.Add(dts)
        con2.Close()
        rpt.LocalReport.ReportPath = Server.MapPath("~/content/Admin/inventory.rdlc")
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
    

    
End Class
