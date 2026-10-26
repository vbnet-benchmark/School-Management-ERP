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
    Dim ds As ds_functions
    Private Sub Show_Alert(type As Boolean, msg As String)
        alertPLC = Me.Master.FindControl("plcAlert")
        If type = True Then
            alert.Text = alertmsg.success_message(msg)
        Else
            alert.Text = alertmsg.error_message(msg)
        End If
        alertPLC.Controls.Add(alert)
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        
        Try
            cbt.Finished_Test(Session("studentid"), Request.QueryString.ToString)
            lblTitle.Text = cbt.Get_Test_Name(Request.QueryString.ToString)
            Dim j As ArrayList = cbt.Student_Test_Summary(Session("studentid"), Request.QueryString.ToString)
            Dim ds As New DataTable
            ds.Columns.Add("Time Elapsed")
            ds.Columns.Add("Score")
            ds.Columns.Add("Questions Answerred Correctly")
            ds.Columns.Add("Questions Answerred Wongly")
            ds.Columns.Add("Questions Not Answerred")
            ds.Columns.Add("Percentage Score")
            ds.Rows.Add(j(0), j(1), j(2), j(3), j(4), j(5))

            DetailsView1.DataSource = ds
            DetailsView1.DataBind()
            Dim pass As String = j(6)
            If pass <> "" Then Image1.ImageUrl = pass
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

  
    Protected Sub bnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("~/content/student/testlist.aspx")
    End Sub
    

  
    Protected Sub lnkReview_Click(sender As Object, e As EventArgs) Handles lnkReview.Click
        Response.Redirect("~/content/student/reviewtest.aspx?" & Request.QueryString.ToString)
    End Sub
End Class
