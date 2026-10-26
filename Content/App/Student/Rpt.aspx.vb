Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Public Class Rpt
    Inherits System.Web.UI.Page

    Dim table As New DataTable
    Dim ds As New DataSet
    Dim adapter As New MySql.Data.MySqlClient.MySqlDataAdapter
    Dim studentName As String
    Dim con As New MySql.Data.MySqlClient.MySqlConnection



    Private Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Label1.Text = Session("StudentID").ToString
        lblID.Text = Session("StudentID").ToString
        lblClass.Text = Session("Class").ToString
        lblname.Text = Session("Fullname").ToString
        lblterm.Text = Session("Term").ToString
        lblyr.Text = Session("Year").ToString


        
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
       
    End Sub

End Class