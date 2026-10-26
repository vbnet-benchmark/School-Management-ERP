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
    Dim msgobj As New Messages
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


  



    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try
            If Not IsPostBack Then
                If Request.QueryString.ToString <> Nothing Then
                    Dim h As ArrayList = cbt.Question_Details(Request.QueryString.ToString)
                    lblqno.Text = h(1)
                    Hidden1.Value = h(2)
                    txtopta.Text = h(3)
                    txtoptb.Text = h(4)
                    txtoptc.Text = h(5)
                    txtoptd.Text = h(6)
                    cboCorrect.Text = h(7)
                    bnCreate.Text = "Update"
                Else
                    lblqno.Text = cbt.Next_Question_No(Session("test"))
                    bnCreate.Text = "Create"
                End If
            End If
            Dim l As New Literal

            l = Me.Master.FindControl("summerLit")
            l.Text = msgobj.Get_Js(Hidden1)
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "this4", l.Text, False)

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Private Sub clear()
        lblqno.Text = ""
        Hidden1.Value = ""
        txtopta.Text = ""
        txtoptb.Text = ""
        txtoptc.Text = ""
        txtoptd.Text = ""
        cboCorrect.Text = "Select"
        lblqno.Text = cbt.Next_Question_No(Session("test"))
    End Sub

    

   
    Protected Sub bnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("~/content/staff/questionlist.aspx")
    End Sub

   
    
    Protected Sub bnCreate_Click(sender As Object, e As EventArgs) Handles bnCreate.Click
        If cboCorrect.Text = "Select" Or Hidden1.Value = "" Or txtopta.Text = "" Or txtoptb.Text = "" Or txtoptc.Text = "" Or txtoptd.Text = "" Then
            Show_Alert(False, "Please enter missing items.")
        Else
            If Request.QueryString.ToString <> Nothing Then
                cbt.Edit_Question(Session("test"), lblqno.Text, Hidden1.Value, txtopta.Text, txtoptb.Text, txtoptc.Text, txtoptd.Text, cboCorrect.Text, Request.QueryString.ToString)
                Show_Alert(True, "Question Updated")
            Else
                If lblqno.Text <> 0 Then
                    cbt.New_Question(Session("test"), lblqno.Text, Hidden1.Value, txtopta.Text, txtoptb.Text, txtoptc.Text, txtoptd.Text, cboCorrect.Text)
                    Show_Alert(True, "Question added")
                    clear()
                Else
                    Show_Alert(False, "Questions exhausted for this test")
                End If
            End If
        End If
    End Sub
End Class
