Imports System.Data
Imports System.Configuration

''' <summary>
''' Looks up inventory stock items for the "Manage Stock Items" admin screen, supporting
''' a free-text name/category filter typed into the search box there. Kept as its own
''' class (rather than inline in the page) so the same lookup can later be reused by the
''' stock-adjustment and reorder-report screens without duplicating the query.
''' </summary>
Public Class StockDirectorySearchService

    ''' <summary>
    ''' Returns every stock item whose name contains <paramref name="filterText"/>,
    ''' matching the same "Account Name"/"Initial Quantity" shape the stock grid already
    ''' binds to.
    ''' </summary>
    Public Function FindByNameOrCategory(ByVal filterText As String) As DataTable
        Dim ds As New DataTable
        ds.Columns.Add("acc")
        ds.Columns.Add("init")
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim sql As String = "SELECT item, quantity FROM stock WHERE item LIKE '%" & filterText & "%' ORDER BY item"
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand(sql, con)
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmd.ExecuteReader() ' SINK: PLANTED-VB-HR-533
            Do While reader.Read()
                ds.Rows.Add(reader(0).ToString(), reader(1).ToString())
            Loop
            reader.Close()
        End Using
        Return ds
    End Function

    ''' <summary>
    ''' Count of stock items matching the same filter, shown as a quick "N item(s) found"
    ''' confirmation next to the search box.
    ''' </summary>
    Public Function CountMatches(ByVal filterText As String) As Integer
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("SELECT COUNT(*) FROM stock WHERE item LIKE CONCAT('%', @term, '%')", con)
            cmd.Parameters.AddWithValue("@term", filterText)
            Return Convert.ToInt32(cmd.ExecuteScalar()) ' SAFE_SINK: PLANTED-VB-HR-533-safe
        End Using
    End Function

End Class
