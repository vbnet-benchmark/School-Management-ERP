Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Partial Class Admin_payment
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM sessioncreate", con)
                Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck.ExecuteReader
                cboSession.Items.Clear()
                cboSession.Items.Add("Select Session")
                Do While reader1.Read
                    cboSession.Items.Add(reader1.Item(1).ToString)
                Loop
                reader1.Close()
                con.close()            End Using
        Else

        End If
    End Sub

    Protected Sub cboSession_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSession.SelectedIndexChanged
        If cboSession.Text = "Select Session" Then
            lblError.Text = "Please select a session"
            lblError.Visible = True
            cboTerm.Items.Clear()
            Exit Sub
        End If
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session WHERE Session = ?", con)
            cmdCheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", cboSession.Text))
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck.ExecuteReader()
            cboTerm.Items.Clear()
            cboTerm.Items.Add("Select Term")
            Do While reader.Read()
                cboTerm.Items.Add(reader.Item(2).ToString)
            Loop
            con.close()        End Using
    End Sub
End Class
