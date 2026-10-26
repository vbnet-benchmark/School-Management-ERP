Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Collections
Imports System.Web.UI
Imports System.Security

Public Class _Login
    Inherits System.Web.UI.Page

    Dim cmd As New SqlCommand
    Dim cmd2 As New SqlCommand
    Dim da As New SqlDataAdapter
    Dim ds As New DataSet
    Dim dr As SqlDataReader
    Dim dr2 As SqlDataReader

    Public con As New SqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()


            cmd = New SqlCommand("select * from tblStaffLG where Username='" & txtID.Text & "' AND Password='" & txtPass.Text & "'", con)

            dr = cmd.ExecuteReader


            If dr.Read = True Then
                Dim sid As String = txtID.Text.ToString

                'Session("StudentID") = txtreType.Text
                Session("StaffID") = dr("StaffID").ToString
                'Session.Add("", auth)
                Session("Username") = dr("Username").ToString

                FormsAuthentication.RedirectFromLoginPage(txtID.Text, False)
                Response.Redirect("Default.aspx")
                'End If
                'System.Threading.Thread.Sleep(3000)
            Else


                Response.Write("You have entered invalid username or password")
            End If


            dr.Close()
            cmd.Dispose()
            con.close()        End Using
    End Sub
End Class