Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Configuration

Partial Class Staff_stafflogin
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

            Session.Clear()

            cmd = New MySql.Data.MySqlClient.MySqlCommand("select * from parentprofile where parentId='" & txtID.Text & "' AND password='" & txtpassword.Value & "'", con)

            dr = cmd.ExecuteReader


            If dr.Read = True Then
                Dim sid As String = txtID.Text.ToString


                'Session("StudentID") = txtreType.Text
                Session("ParentID") = dr(0).ToString
                'Session.Add("", auth)
                Session("parent") = dr("parentname").ToString
                dr.Close()

                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                reader2.Read()
                Session("SessionID") = reader2(0).ToString
                Session("SessionName") = reader2(1).ToString
                Session("term") = reader2(2).ToString
                reader2.Close()

                con.close()
        FormsAuthentication.RedirectFromLoginPage(txtID.Text, False)
        Response.Redirect("~/Staff/staffprofile.aspx")
            Else

        lblError.Text = "You have entered invalid username or password"
            End If
        dr.Close()
        cmd.Dispose()
        con.close()
        End Using
    End Sub

    ''' <summary>
    ''' New self-registration action: a visitor creates a real, server-persisted parent portal
    ''' account (an INSERT into the real parentprofile table) and is logged straight in, without
    ''' first discarding whatever session the visitor already held.
    ''' </summary>
    Protected Sub btnRegisterQuick_Click(sender As Object, e As EventArgs)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into parentprofile (parentID, parentName, sex, address, email, phone, password) values (?,?,?,?,?,?,?)", con)
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("parentID", txtRegEmail.Text))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("parentName", txtRegName.Text))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", ""))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("address", ""))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("email", txtRegEmail.Text))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phone", ""))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("password", txtRegPassword.Text))
            cmdInsert.ExecuteNonQuery()
            con.close()
        End Using
        Session("ParentID") = txtRegEmail.Text
        Session("parent") = txtRegName.Text
        ' SINK: PLANTED-VB-HR-321
        FormsAuthentication.RedirectFromLoginPage(txtRegEmail.Text, False)
    End Sub

    ''' <summary>
    ''' Same real self-registration as <see cref="btnRegisterQuick_Click"/>, but discards the
    ''' visitor's pre-existing session before completing the new account's login.
    ''' </summary>
    Protected Sub btnRegisterSecure_Click(sender As Object, e As EventArgs)
        Session.Abandon()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into parentprofile (parentID, parentName, sex, address, email, phone, password) values (?,?,?,?,?,?,?)", con)
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("parentID", txtRegEmail.Text))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("parentName", txtRegName.Text))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", ""))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("address", ""))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("email", txtRegEmail.Text))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phone", ""))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("password", txtRegPassword.Text))
            cmdInsert.ExecuteNonQuery()
            con.close()
        End Using
        Session("ParentID") = txtRegEmail.Text
        Session("parent") = txtRegName.Text
        ' SAFE_SINK: PLANTED-VB-HR-321-safe
        FormsAuthentication.RedirectFromLoginPage(txtRegEmail.Text, False)
    End Sub
End Class
