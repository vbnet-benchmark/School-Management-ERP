Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls



Partial Class Admin_staffprofile
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList

    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder
    Dim par As New parentcheck
    Dim cbt As New CBT
    Dim ds As new ds_functions
    Private Sub Show_Alert(type As Boolean, msg As String)
        alertPLC = Me.Master.FindControl("plcAlert")
        If type = True Then
            alert.Text = alertmsg.success_message(msg)
        Else
            alert.Text = alertmsg.error_message(msg)
        End If
        alertPLC.Controls.Add(alert)
    End Sub


    Private Sub Load_Details()
        Dim k As ArrayList = cbt.Test_Details(Request.QueryString.ToString)
        Dim ds As New DataTable
        ds.Columns.Add("Title")
        ds.Columns.Add("Subject")
        ds.Columns.Add("Class")
        ds.Columns.Add("Duration")
        ds.Columns.Add("Number of Questions")
        ds.Columns.Add("Total Marks")
        ds.Columns.Add("Validated")
        ds.Rows.Add(k(0), k(1), k(2), k(3), k(4), k(5), k(6))
        DetailsView1.DataSource = ds
        DetailsView1.DataBind()
        If k(6) = "No" Then
            btnValidate.Text = "Validate Test"
        Else
            btnValidate.Text = "InValidate Test"
        End If
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Class(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")

        Try
            Session("Test") = Request.QueryString.ToString
            Load_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub
    
    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles btnQuestion.Click
        Session("test") = Request.QueryString.ToString
        Response.Redirect("~/content/staff/questionlist.aspx")
    End Sub





    Protected Sub DetailsView1_PageIndexChanging(sender As Object, e As DetailsViewPageEventArgs) Handles DetailsView1.PageIndexChanging

    End Sub

   


    
    Protected Sub btnReport_Click(sender As Object, e As EventArgs) Handles btnReport.Click
        Response.Redirect("~/content/staff/testreport.aspx?" & Request.QueryString.ToString)
    End Sub

    Protected Sub btnValidate_Click(sender As Object, e As EventArgs) Handles btnValidate.Click
        If cbt.Compare_Question_No(Request.QueryString.ToString) <> False Then
            If btnValidate.Text = "Validate Test" Then
                cbt.Validate_Test(Request.QueryString.ToString)
                Show_Alert(True, "Test Validated")
            Else
                cbt.InValidate_Test(Request.QueryString.ToString)
                Show_Alert(True, "Test Invalidated")
            End If

            Load_Details()

        Else
            Show_Alert(False, "Questions Incomplete")
        End If
    End Sub

    Protected Sub btnEdit_Click(sender As Object, e As EventArgs) Handles btnEdit.Click
        Response.Redirect("~/content/staff/newtest.aspx?" & Request.QueryString.ToString)

    End Sub
End Class
