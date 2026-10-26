
Partial Class Account_expenses
    Inherits System.Web.UI.Page


    Protected Sub btnComfirm_Click(sender As Object, e As EventArgs) Handles btnComfirm.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select ref from expense", con)
            Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
            Dim test As Boolean = False
            Dim refs As New ArrayList
            Do While readref.Read
                refs.Add(readref.Item(0))
            Loop
            Dim y As Integer
            Dim x As New Random
            Do Until test = True
                y = x.Next(100000, 999999)
                If refs.Contains(y) Then
                    test = False
                Else
                    test = True
                End If
            Loop
            readref.Close()
            Dim comfirm11 As New MySql.Data.MySqlClient.MySqlCommand("Insert into expense (ref, type, amount, remarks, date) values(?,?,?,?,?)", con)
            comfirm11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ref", y))
            comfirm11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cboType.Text))
            comfirm11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("year", FormatNumber(txtAmount.Text)))
            comfirm11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("reamrks", txtRem.Text))
            comfirm11.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Now.Date))
            comfirm11.ExecuteNonQuery()
            con.close()        End Using
        lblSuccess.Text = "Expense enterred successfully"
        txtAmount.Text = ""
        txtRem.Text = ""
        cboType.SelectedIndex = 0
    End Sub
End Class
