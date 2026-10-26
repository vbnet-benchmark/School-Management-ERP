Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Account_salary
    Inherits System.Web.UI.Page
    Dim feeremove As String



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            
            Label4.Text = "Please add General fees here"
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from choicefees", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                Do While reader2.Read
                    ListBox2.Items.Add(reader2.Item(0).ToString & " - " & reader2.Item(1).ToString)
                Loop
                reader2.Close()
                con.close()            End Using
        Else


            lblSuccess.Text = ""
            lblError.Text = ""


            For Each item As ListItem In ListBox2.Items
                If item.Selected = True Then
                    feeremove = item.Text
                End If

            Next
        End If
    End Sub

    Protected Sub Wizard1_FinishButtonClick(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.FinishButtonClick
        Session("Success") = False
        lblFsuccess.Text = ""
        Response.Redirect("~/Account/optionalfees.aspx")

    End Sub



    Protected Sub Wizard1_NextButtonClick(sender As Object, e As WizardNavigationEventArgs)
        If Wizard1.ActiveStep Is WizardStep2 Then
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim delete As New MySql.Data.MySqlClient.MySqlCommand("Delete From choicefees", con)
                delete.ExecuteNonQuery()
                For Each item As ListItem In ListBox2.Items
                    Dim i() As String = Split(item.Text, "-")

                    Dim insert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into choicefees (fee, amount) Values (?,?)", con)
                    insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("password", Trim(i(0))))
                    insert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Trim(i(1)).Replace(",", "")))
                    insert.ExecuteNonQuery()

                    Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & Trim(i(0)) & " DEBTS" & "', '" & "income" & "')", con)
                    cmd1.ExecuteNonQuery()

                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Insert into accsettings (accname, type) values('" & Trim(i(0)) & " PAID" & "', '" & "income" & "')", con)
                    cmd2.ExecuteNonQuery()
                Next
                Session("Success") = True
                If Session("Success") = True Then
                    lblFsuccess.Text = "Fees added successfully"
                    lblFsuccess.ForeColor = Drawing.Color.Green
                Else
                    lblFsuccess.Text = "Fees not added. Try again"
                    lblFsuccess.ForeColor = Drawing.Color.Red
                End If
                con.close()            End Using

        End If
    End Sub

   

    
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        ListBox2.Items.Add(TextBox1.Text & " - " & TextBox2.Text)
        TextBox1.Text = ""
        TextBox2.Text = ""
        lblSuccess.Text = "Fee added"

    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        ListBox2.Items.Remove(feeremove)
        TextBox1.Text = ""
        TextBox2.Text = ""
        lblSuccess.Text = "Fee removed"
    End Sub
End Class
