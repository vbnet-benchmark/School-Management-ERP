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


  
    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim scriptman As New ScriptManager
        scriptman = Me.Master.FindControl("ScriptManager1")
        scriptman.RegisterPostBackControl(radOptions)

        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = False

    End Sub


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try

            If Not IsPostBack Then
                If Request.QueryString.ToString <> Nothing Then                    If cbt.Submitted(Session("studentid"), Request.QueryString.ToString) = True Then Response.Redirect("~/content/student/testresult.aspx?" & Request.QueryString.ToString)                    Dim h As ArrayList = cbt.Start_Test(Session("studentid"), Request.QueryString.ToString, Session("sessionid"))
                    Load_Question(h)
                    lblTimeLeft.Text = h(1)
                    lblTimeAllocated.Text = IIf(h(2) > 59.9, h(2) \ 60 & " Hrs " & h(2) Mod 60 & " Mins", h(2) & " Mins")
                    If h(3) = False Then
                        bnNext.Text = "Next Question"
                    Else
                        If cbt.Check_Unanswerred(Session("studentid"), Request.QueryString.ToString) = True Then
                            bnNext.Text = "Next Skipped Question"
                        Else
                            bnNext.Text = "Submit Test"

                        End If


                    End If
                End If            End If        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Private Sub Load_Question(h As ArrayList)
        Dim qdetails As ArrayList = h(0)
        Session("qid") = qdetails(8)
        lblqno.Text = qdetails(1)
        lblQuestion.Text = qdetails(2)
        radOptions.Items.Item(0).Text = "  A. " & qdetails(3)
        radOptions.Items.Item(1).Text = "  B. " & qdetails(4)
        radOptions.Items.Item(2).Text = "  C. " & qdetails(5)
        radOptions.Items.Item(3).Text = "  D. " & qdetails(6)
    End Sub
   
    Protected Sub bnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Response.Redirect("~/content/student/testlist.aspx")
    End Sub

   
    
    Protected Sub bnCreate_Click(sender As Object, e As EventArgs) Handles bnNext.Click
        Try
            Dim ans As Array = Split(radOptions.SelectedItem.ToString, ".")
            cbt.Submit_Answer(Session("studentid"), Trim(ans(0)), Request.QueryString.ToString, cbt.Time_Left(cbt.Convert_Time(lblTimeLeft.Text), Request.QueryString.ToString), Session("qid"), Session("sessionid"))
            If bnNext.Text = "Next Question" Then
                Dim a As ArrayList = cbt.Next_Question(Request.QueryString.ToString, Session("studentid"), Session("sessionid"))
                Load_Question(a)
                If a(1) = False Then
                    bnNext.Text = "Next Question"
                Else
                    If cbt.Check_Unanswerred(Session("studentid"), Request.QueryString.ToString) = True Then
                        bnNext.Text = "Next Skipped Question"
                    Else
                        bnNext.Text = "Submit Test"

                    End If


                End If
            ElseIf bnNext.Text = "Next Skipped Question" Then
                Dim a As ArrayList = cbt.Next_Unanswerred(Request.QueryString.ToString, Session("studentid"))
                Load_Question(a)
                If a(1) = False Then
                    bnNext.Text = "Next Skipped Question"
                Else
                    bnNext.Text = "Submit Test"
                End If
            Else
                If radOptions.SelectedValue.ToString = "" Then
                    Show_Alert(False, "Please select an option")
                Else
                    Response.Redirect("~/content/student/testresult.aspx?" & Request.QueryString.ToString)
                End If
            End If
            For Each item As ListItem In radOptions.Items
                item.Selected = False
            Next
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            updatepanel100.update()
            If lblTimeLeft.Text = "00:00:00" Then
                Dim h As String = ""
                If radOptions.SelectedIndex.ToString <> "" Then
                    Dim ans As Array = Split(radOptions.SelectedItem.ToString, ".")
                    h = Trim(ans(0))
                End If
                cbt.Submit_Answer(Session("studentid"), h, Request.QueryString.ToString, cbt.Time_Left(cbt.Convert_Time(lblTimeLeft.Text), Request.QueryString.ToString), Session("qid"), Session("sessionid"))
                Response.Redirect("~/content/student/testresult.aspx?" & Request.QueryString.ToString)
            End If
            Dim a As Array = Split(lblTimeLeft.Text, ":")
            lblTimeLeft.Text = IIf(Val(a(2)) - 1 = -1, IIf(Val(a(1)) - 1 = -1, Val(a(0)) - 1 & ":59:59", a(0) & ":" & IIf(Val(a(1)) - 1 <= 9, "0" & Val(a(1)) - 1, Val(a(1)) - 1) & ":59"), a(0) & ":" & a(1) & ":" & IIf(Val(a(2)) - 1 <= 9, "0" & Val(a(2)) - 1, Val(a(2)) - 1))
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

  
    
End Class
