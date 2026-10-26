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
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()

            Session.Clear()

            cmd = New MySql.Data.MySqlClient.MySqlCommand("select * from staffprofile where staffid='" & txtID.Text & "' AND password='" & txtpassword.Value & "'", con)

            dr = cmd.ExecuteReader


            If dr.Read = True Then
                Dim sid As String = txtID.Text.ToString


                'Session("StudentID") = txtreType.Text
                Session("StaffID") = dr(0).ToString
                'Session.Add("", auth)
                Session("staff") = dr("surname").ToString
                dr.Close()

                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                reader2.Read()
                Session("SessionID") = reader2(0).ToString
                Session("SessionName") = reader2(1).ToString
                Session("term") = reader2(2).ToString
                reader2.Close()

                Dim cmdInsert3 As New MySql.Data.MySqlClient.MySqlCommand("select classteacher.class, class.class from classteacher Inner Join Class On Class.ID = classteacher.class where teacher='" & txtID.Text & "' ", con)
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert3.ExecuteReader
                If reader3.Read Then
                    Session("Classid") = reader3(0).ToString
                    Session("ClassName") = reader3(1).ToString

                End If
                reader3.Close()
                con.close()end using
        FormsAuthentication.RedirectFromLoginPage(txtID.Text, False)
        Response.Redirect("~/Staff/staffprofile.aspx")
            Else

        lblError.Text = "You have entered invalid username or password"
            End If
        dr.Close()
        cmd.Dispose()
        con.close()end using
    End Sub
End Class
