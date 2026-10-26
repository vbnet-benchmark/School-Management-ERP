Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Configuration
Partial Class Admin_adminlogin
    Inherits System.Web.UI.Page
    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand
    Dim da As New MySql.Data.MySqlClient.MySqlDataAdapter
    Dim ds As New DataSet
    Dim dr As MySql.Data.MySqlClient.MySqlDataReader
    Dim dr2 As MySql.Data.MySqlClient.MySqlDataReader


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()


            cmd = New MySql.Data.MySqlClient.MySqlCommand("select * from acclogin where username='" & txtID.Text & "' AND password='" & txtpassword.Value & "'", con)

            dr = cmd.ExecuteReader


            If dr.Read = True Then
                Dim sid As String = txtID.Text.ToString

                'Session("StudentID") = txtreType.Text
                Session("StaffID") = dr(1).ToString
                'Session.Add("", auth)
                Session("staff") = dr(1).ToString
                dr.Close()

                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                reader2.Read()
                Session("SessionID") = reader2(0).ToString
                Session("SessionName") = reader2(1).ToString
                Session("Term") = reader2(2).ToString
                reader2.Close()

                con.close()
        FormsAuthentication.RedirectFromLoginPage(txtID.Text, False)
        Response.Redirect("~/admin/adminwelcome.aspx")
            Else

        Response.Write("You have entered invalid username or password")
            End If
        dr.Close()
        cmd.Dispose()
        con.close()
        End Using
    End Sub

    ''' <summary>
    ''' SSO-style admin login hand-off: after a real credential check against the real acclogin
    ''' table, the caller-supplied "Mode" query-string selects which concrete
    ''' LoginCompletionStrategyBase (App_Code/LoginCompletionStrategy.vb) actually completes the
    ''' login -- "secure" discards the requester's pre-existing session first, the default mode
    ''' does not.
    ''' </summary>
    Protected Sub btnSsoComplete_Click(sender As Object, e As EventArgs)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim cmdSso As New MySql.Data.MySqlClient.MySqlCommand("select * from acclogin where username='" & txtID.Text & "' AND password='" & txtpassword.Value & "'", con)
            Dim drSso As MySql.Data.MySqlClient.MySqlDataReader = cmdSso.ExecuteReader
            If drSso.Read = True Then
                drSso.Close()
                con.close()
                Session("staff") = txtID.Text
                Dim strategy As LoginCompletionStrategyBase = LoginCompletionFactory.Resolve(Request.QueryString("Mode"))
                strategy.CompleteLogin(txtID.Text)
            Else
                drSso.Close()
                con.close()
                lblError.Text = "You have entered invalid username or password"
            End If
        End Using
    End Sub
End Class
