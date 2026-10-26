Imports System.IO
Partial Class Admin_addteacher



    Inherits System.Web.UI.Page
    
    Dim group As Integer
    Dim subselect As Integer
    Dim subremove As String
    Dim classremove As String
    Dim staffID As String

    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder
    Dim par As New parentcheck
    Dim cbt As New CBT
    Dim ds As New ds_functions
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
        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        If Not IsPostBack Then
            cboSubject.Items.Clear()
            cboSubject.Items.Add("Select Subject")
            For Each s In ds.Subjects_Taught(ds.Get_Staff_name(Session("staffid")))
                cboSubject.Items.Add(s)
            Next
            Try
                If Request.QueryString.ToString <> Nothing Then
                    Dim h As ArrayList = cbt.Test_Details(Request.QueryString.ToString)
                    txtTitle.Text = h(0)
                    cboSubject.Text = h(1)
                    For Each i In ds.Class_Taught(ds.Get_Staff_name(Session("staffid")), cboSubject.Text)
                        cboClass.Items.Add(i)
                    Next
                    cboClass.Text = h(2)
                    txtDuration.Text = h(3)
                    txtQNo.Text = h(4)
                    txtMarks.Text = h(5)
                    bnCreate.Text = "Update"
                Else
                    bnCreate.Text = "Create"
                End If

            Catch ex As Exception
                Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
            End Try
        End If
    End Sub

   

    

   
    Protected Sub bnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("~/content/staff/testlist.aspx")
    End Sub

   
    Protected Sub cboSubject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubject.SelectedIndexChanged
        Try
            If Not cboSubject.Text = "Select Subject" Then
                cboClass.Items.Clear()
                cboClass.Items.Add("Select Class")
                For Each i In ds.Class_Taught(ds.Get_Staff_name(Session("staffid")), cboSubject.Text)
                    cboClass.Items.Add(i)
                Next
            Else
                Show_Alert(False, "Please select a subject")
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub bnCreate_Click(sender As Object, e As EventArgs) Handles bnCreate.Click
        Try
            If cboSubject.Text = "Select Subject" Or cboClass.Text = "Select Class" Or txtTitle.Text = "" Or txtDuration.Text = "" Or txtMarks.Text = "" Or txtQNo.Text = "" Then
                Show_Alert(False, "Please enter missing items.")
            Else
                If Request.QueryString.ToString <> Nothing Then
                    cbt.Edit_Test(txtTitle.Text, Session("staffid"), ds.Get_Subject_ID(cboSubject.Text), ds.Get_Class_ID(cboClass.Text), txtDuration.Text, txtQNo.Text, txtMarks.Text, Request.QueryString.ToString)
                    Show_Alert(True, "Test Updated")
                Else
                    If cbt.Check_Test_Aailable(txtTitle.Text, Session("sessionid")) = True Then
                        cbt.New_Test(txtTitle.Text, Session("staffid"), ds.Get_Subject_ID(cboSubject.Text), ds.Get_Class_ID(cboClass.Text), txtDuration.Text, txtQNo.Text, txtMarks.Text, Session("sessionid"))
                        Show_Alert(True, "Test Created")
                        clear()
                    Else
                        Show_Alert(False, "Test title exists")
                    End If
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Private Sub clear()
        cboSubject.Text = "Select Subject"
        cboClass.Items.Clear()
        txtDuration.Text = ""
        txtMarks.Text = ""
        txtQNo.Text = ""
        txtTitle.Text = ""
    End Sub
End Class
