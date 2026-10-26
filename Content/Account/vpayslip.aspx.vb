Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Account_salary
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            For j As Integer = 2018 To Now.Year
                DropDownList3.Items.Add(j)
            Next

        End If
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If DropDownList2.Text = "Month" Or DropDownList3.Text = "Year" Then
            lblError.Text = "Please select the month and year for payment."
        Else
            Session("month") = DropDownList2.Text
            Session("year") = DropDownList3.Text
            Response.Redirect("~/account/payslip.aspx")
        End If
       

    End Sub
End Class
