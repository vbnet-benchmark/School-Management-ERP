Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Configuration
Public Class SLogin
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

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()


            cmd = New SqlCommand("select * from tblEnroll where StudentID='" & txtID.Text & "' AND PIN='" & txtPass.Text & "'", con)

            dr = cmd.ExecuteReader


            If dr.Read = True Then
                Dim sid As String = txtID.Text.ToString

                'Session("StudentID") = txtreType.Text
                Session("StudentID") = dr("StudentID").ToString
                'Session.Add("", auth)
                Session("Fullname") = dr("Fullname").ToString
                Session("Class") = dr("Class").ToString
                Session("Year") = dr("Year").ToString
                'Session("StudentID") = txtreType.Text
                Session("Term") = dr("Term").ToString

                FormsAuthentication.RedirectFromLoginPage(txtID.Text, False)
                Response.Redirect("~/Student/s_Default.aspx")

            Else

                Response.Write("You have entered invalid username or password")
            End If


            dr.Close()
            cmd.Dispose()
            con.close()        End Using
    End Sub
End Class