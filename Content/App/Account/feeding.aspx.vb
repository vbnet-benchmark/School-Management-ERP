Imports System.IO
Partial Class Admin_addteacher



    Inherits System.Web.UI.Page
    

    Dim subselect As Integer
    Dim subremove As String
    Dim classremove As String
    Dim staffID As String




    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles bnUpdate.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from feeding", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            If student0.Read Then
                student0.Close()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update feeding Set cost = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", txtCost.Text))
                cmdCheck3.ExecuteNonQuery()
                lblSuccess.Text = "Fee Added Successfully"
            Else
                student0.Close()
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into feeding (cost) Values (?)", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cost", txtCost.Text))
                cmdCheck2.ExecuteNonQuery()
                lblSuccess.Text = "Fee Added Successfully"
            End If
            con.close()        End Using

    End Sub
   
   

   

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        
    End Sub

   

    

   
   
End Class
