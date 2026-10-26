Imports System.Text
Imports System.Web.Services
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Partial Class Admin_adminpage
    Inherits System.Web.UI.Page
    Public Shared receiver As String
    Public Shared recCategory As String
    Dim msgobj As New Messages
    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder
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
        scriptman.RegisterPostBackControl(btnUpload)
        scriptman.RegisterPostBackControl(cboReceivetype)
        scriptman.RegisterPostBackControl(cboCategory)
        scriptman.RegisterPostBackControl(btnSend)
        scriptman.RegisterPostBackControl(Button1)
        scriptman.RegisterPostBackControl(ChkPortal)
        scriptman.RegisterPostBackControl(chkSendSMS)
        scriptman.RegisterPostBackControl(del1)
        scriptman.RegisterPostBackControl(del2)
        scriptman.RegisterPostBackControl(del3)
        scriptman.RegisterPostBackControl(del4)
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = False

    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Staff(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Dim ds As New DataTable
        ds.Columns.Add("name")
        Dim sd As New departments
        Dim n As ArrayList = sd.Get_Superiors(Session("staffid"))
        If n(0) = "None" Then
            chkSendSMS.Visible = True
        Else
            chkSendSMS.Visible = False
        End If

        ' Optional compose-preview banner (e.g. newmsg.aspx?previewSubject=...&audience=PARENT),
        ' shown above the composer so staff can see how the subject will look to the
        ' recipient before actually sending. The concrete preview strategy is resolved from a
        ' lookup table keyed by audience (see App_Code/MessagePreviewRenderer.vb) -- a single
        ' shared call site below renders whichever strategy the table resolved to; exploitability
        ' depends entirely on which concrete IMessagePreviewStrategy the factory returned, not on
        ' anything visible at this call site itself.
        If Not IsPostBack Then
            Dim previewSubject As String = Request.QueryString("previewSubject")
            If Not String.IsNullOrEmpty(previewSubject) Then
                Dim previewLit As New Literal
                Dim strategy As IMessagePreviewStrategy = MessagePreviewStrategyFactory.Resolve(Request.QueryString("audience"))
                previewLit.Text = strategy.Render(previewSubject)
                alertPLC = Me.Master.FindControl("plcAlert")
                alertPLC.Controls.Add(previewLit)
            End If
        End If

        Try
            If Request.QueryString.ToString <> Nothing And Not IsPostBack Then
                Dim same As ArrayList = msgobj.Use_Same_Msg(Request.QueryString.ToString)
                Dim sender1 As String = same(0)
                If Session("responsetype") = "Reply" Then
                    cboReceivetype.Text = IIf(same(1).ToString.ToUpper = "STAFF", same(1).ToString.ToUpper, "PARENTS")
                    txtSubject.Text = "Re " & same(2)
                    cboCategory.Items.Add(same(5))
                    Session("ReceiverReply") = same(5)
                    Session("Relationship") = same(6)
                    If same(1).ToString = "Admin" Or same(1).ToString = "Accounts" Then cboReceivetype.Text = "Staff"
                Else
                    txtSubject.Text = same(2)
                    FreeTextBox1.Text = same(3)
                End If
                Dim s As New ds_functions
                If Session("responsetype") = "Reply" Then
                    If cboReceivetype.Text = "Student" Then
                        Dim stu As String = s.Get_Stu_name(sender1)
                        ds.Rows.Add(stu)
                    ElseIf cboReceivetype.Text = "Staff" Or cboReceivetype.Text = "STAFF" Then
                        If cboCategory.Text = "Admin" Then
                            ds.Rows.Add("Admin")
                        ElseIf cboCategory.Text = "Accounts" Then
                            ds.Rows.Add("Accounts")
                        Else
                            Dim stu As String = s.Get_Staff_name(sender1)
                            ds.Rows.Add(stu)
                        End If
                    Else
                        Dim stu As String = s.Get_parent_name(sender1)
                        ds.Rows.Add(stu)
                    End If
                    cboReceivetype.Enabled = False
                    cboCategory.Enabled = False
                End If
                If Session("responsetype") = "Forward" Then
                    Dim pics As ArrayList = msgobj.Attach_Files(Request.QueryString.ToString)
                    Dim c As Integer = 1
                    For Each x In pics
                        Dim imagesrc As New fileimage
                        Panel1.Visible = True
                        If c = 1 Then
                            icon1.Src = imagesrc.get_image(x(1).ToString)
                            del1.Text = "Delete"
                            LinkButton1.Text = Path.GetFileName(x(0))
                            Session("pic1") = x(0)
                        ElseIf c = 2 Then
                            icon2.Src = imagesrc.get_image(x(1).ToString)
                            del2.Text = "Delete"
                            LinkButton2.Text = Path.GetFileName(x(0))
                            Session("pic2") = x(0)
                        ElseIf c = 3 Then
                            icon3.Src = imagesrc.get_image(x(1).ToString)
                            del3.Text = "Delete"
                            LinkButton3.Text = Path.GetFileName(x(0))
                            Session("pic3") = x(0)
                        ElseIf c = 4 Then
                            icon4.Src = imagesrc.get_image(x(1).ToString)
                            del4.Text = "Delete"
                            LinkButton4.Text = Path.GetFileName(x(0))
                            Session("pic4") = x(0)
                        End If
                        c = c + 1
                    Next
                End If
            End If
            Session("responsetype") = Nothing
            For Each row As GridViewRow In gridRecipients.Rows
                ds.Rows.Add(row.Cells(0).Text)
            Next
            gridRecipients.DataSource = ds
            gridRecipients.DataBind()

            receiver = cboReceivetype.Text

              Dim l As New Literal

            l = Me.Master.FindControl("summerLit")
            l.Text = msgobj.Get_Js_Admin(Hidden1, counter, txtSMS, ChkPortal)
            ScriptManager.RegisterClientScriptBlock(Me.Page, Me.GetType, "this4", l.Text, False)
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnSend_Click(sender As Object, e As EventArgs) Handles btnSend.Click
        Try
            If ChkPortal.Checked = False And chkSendSMS.Checked = False Then
                Show_Alert(False, "Select an option for either Portal message, SMS or both")
                Exit Sub
            End If
            If gridRecipients.Rows.Count = 0 Then
                Show_Alert(False, "Please add recipients.")
                Exit Sub
            End If
            Dim message As String = Hidden1.Value
            Dim j As String = cboCategory.Text
            Dim x As New Random
            Dim recno As Integer
            Dim recipient As New ArrayList
            Dim no As Integer
            For Each row As GridViewRow In gridRecipients.Rows
                If Not recipient.Contains(row.Cells(0).Text) Then
                    recipient.Add(row.Cells(0).Text)
                    no = no + 1
                End If
            Next
            Dim y As Integer
            Dim s As New ds_functions
            If ChkPortal.Checked = True Then
                For Each item As String In recipient
                    Dim z As String
                    If receiver = "PARENTS" Then
                        z = s.Get_parent_ID(item)
                    ElseIf receiver = "STUDENTS" Then
                        z = s.Get_Stu_ID(item)
                    ElseIf receiver = "STAFF" Then
                        If cboCategory.Text = "ADMIN" or cboCategory.Text = "Admin" Then
                            z = "Admin"
                        ElseIf cboCategory.Text = "ACCOUNTS" or cboCategory.Text = "Accounts" Then
                            z = "Accounts"
                        Else
                            z = s.Get_Staff_ID(item)
                        End If
                    End If
                    If y = 0 Then
                        y = msgobj.Send_Msg(z, Session("staffid"), txtSubject.Text, message, Session("sessionid"), "Staff", j)

                    Else
                        msgobj.Send_Msg_Cont(z, Session("staffid"), txtSubject.Text, message, Session("sessionid"), "Staff", j, y)

                    End If
                   If z = "Admin" Then
                        Dim d As ArrayList = s.Fetch_admins
                        For Each q In d
                             msgobj.Send_Email(msgobj.Get_Email(q), txtSubject.Text, message, "http://" & Request.Url.Authority, Session("Relationship"), s.Get_Staff_Name(Session("staffid")))

                        Next
                    ElseIf z = "Accounts" Then
                        Dim d As ArrayList = s.Fetch_accounts
                         For Each q In d
                             msgobj.Send_Email(msgobj.Get_Email(q), txtSubject.Text, message, "http://" & Request.Url.Authority, Session("Relationship"), s.Get_Staff_Name(Session("staffid")))

                        Next
                            else

                             msgobj.Send_Email(msgobj.Get_Email(z), txtSubject.Text, message, "http://" & Request.Url.Authority, Session("Relationship"), s.Get_Staff_Name(Session("staffid")))

                        end if
                    If z = "Admin" Then
                        Dim d As ArrayList = s.Fetch_admins
                        For Each q In d
                            logify.Notifications("You have a new message - " & txtSubject.Text, q, Session("staffid"), Session("Relationship"), "~/content/admin/readmsg.aspx?" & y, "Message")
                        Next
                    ElseIf z = "Accounts" Then
                        Dim d As ArrayList = s.Fetch_accounts
                        For Each q In d
                            logify.Notifications("You have a new message - " & txtSubject.Text, q, Session("staffid"), Session("Relationship"), "~/content/account/readmsg.aspx?" & y, "Message")
                        Next
                    ElseIf receiver = "STAFF" Then
                        logify.Notifications("You have a new message - " & txtSubject.Text, z, Session("staffid"), Session("Relationship"), "~/content/staff/readmsg.aspx?" & y, "Message")
                    ElseIf receiver = "PARENTS" Then
                        logify.Notifications("You have a new message - " & txtSubject.Text, z, Session("staffid"), Session("Relationship"), "~/content/parent/readmsg.aspx?" & y, "Message")
                    End If
                    recno = recno + 1
                Next
            End If
            Dim k As Integer
            For Each row As GridViewRow In gridRecipients.Rows
                If Not recipient.Contains(row.Cells(0).Text) Then
                    recipient.Add(row.Cells(0).Text)
                    k = k + 1
                End If
            Next
            Dim pages As Integer = Math.Ceiling(txtSMS.Text.Count / 160)


            Dim sentany As Boolean
            Dim inisms As Integer
            Dim smsno As Integer
            If chkSendSMS.Checked = True Then
                If no * pages > msgobj.Check_SMS_Units() Then
                    Show_Alert(False, "You don't have enough credits for the SMS operation. Contact developer")
                    Exit Sub
                End If
                Dim sent As Boolean
                For Each item As String In recipient
                    Dim phone As String = msgobj.Get_phone(item)
                    Dim n As Array = Split(phone, ",")
                    Dim o As New ArrayList
                    Try
                        o.Add(n(0))
                        o.Add(n(1))
                    Catch
                    End Try
                    For Each j In o
                        ' Hardened: the Staff composer uses Send_SMS_Secure, which does not
                        ' disable certificate validation (see App_Code/Messages.vb).
                        sent = msgobj.Send_SMS_Secure(txtSMS.Text, Trim(j), Session("sessionid"))
                        If sent = True Then
                            sentany = True
                            smsno += 1
                        End If
                    Next
                Next
                If sent = True Then msgobj.Record_SMS(txtSubject.Text, txtSMS.Text, sent * pages, pages, sent, receiver, Session("staffid"))
            End If


            If ChkPortal.Checked = True Then
                If LinkButton1.Text <> "" Then
                    msgobj.Att_Files(y, Session("pic1"), att1.Text)
                End If
                If LinkButton2.Text <> "" Then
                    msgobj.Att_Files(y, Session("pic2"), att2.Text)
                End If
                If LinkButton3.Text <> "" Then
                    msgobj.Att_Files(y, Session("pic3"), att3.Text)
                End If
                If LinkButton4.Text <> "" Then
                    msgobj.Att_Files(y, Session("pic4"), att4.Text)
                End If
              msgobj.Send_Msg_sent(y, recno & " Recipients", Session("Relationship"), Session("staffid"), txtSubject.Text, message, Session("sessionid"), j, Session("ReceiverReply"))
            End If
            logify.log(Session("staffid"), "A message was sent to " & cboCategory.Text & ". Message ref = " & y)
            Show_Alert(True, "Message sent successfully.")
            Session("Relationship") = Nothing
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub


 
    Protected Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = True
        Response.Redirect("~/Content/staff/messages.aspx")
    End Sub




    Protected Sub cboReceivetype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboReceivetype.SelectedIndexChanged
        Try
            cboCategory.Items.Clear()
                If cboReceivetype.Text = "STAFF" Then
                    cboCategory.Items.Add("SELECT")
                    cboCategory.Items.Add("ADMIN")
                    cboCategory.Items.Add("ACCOUNTS")
                    ElseIf cboReceivetype.Text = "PARENTS" Then
                    cboCategory.Items.Add("SELECT")
                End If
                Dim superiors As New departments
                Dim sup As ArrayList = superiors.Get_Superiors(Session("staffid"))
                For Each u As String In sup
                If cboReceivetype.Text = "STAFF" And u <> "None" Then 
                cboCategory.Items.Add(u)
                elseif cboReceivetype.Text = "STAFF" And u = "None"
                cboCategory.Items.Add("ALL STAFF")
                elseif cboReceivetype.Text = "PARENTS" And u = "None"
                cboCategory.Items.Add("ALL PARENTS")
                End if
                Next
                Dim cats As ArrayList = superiors.Get_Classes_Controlled(Session("staffid"))
                Dim depts As ArrayList = cats(0)
                Dim classes As ArrayList = cats(1)
                Dim clasgroup As ArrayList = cats(2)
                Dim myclas As ArrayList = cats(3)
                For Each g As String In depts
                    If cboReceivetype.Text = "STAFF" Then cboCategory.Items.Add(g & " - STAFF")
                Next
                For Each g As String In classes
                    If cboReceivetype.Text = "PARENTS" Then cboCategory.Items.Add(g & " - PARENTS")
                    If cboReceivetype.Text = "STAFF" Then cboCategory.Items.Add(g & " - CLASS TEACHER")
                Next
                For Each g As String In myclas
                    If cboReceivetype.Text = "PARENTS" Then cboCategory.Items.Add(g & " - PARENTS")
                Next
                For Each g As String In clasgroup
                    If cboReceivetype.Text = "PARENTS" Then cboCategory.Items.Add(g & " - PARENTS")
                Next
               
               
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("name")
                Dim msgobj As New Messages

                If cboCategory.Text = "SELECT" Then
                    Show_Alert(False, "Please select a category")
                ElseIf cboCategory.Text = "ADMIN" Then
                    Session("Relationship") = "STAFF"
                    ds.Rows.Add("Admin")
                ElseIf cboCategory.Text = "ACCOUNTS" Then
                    Session("Relationship") = "STAFF"
                    ds.Rows.Add("Accounts")
                 ElseIf cboCategory.Text = "ALL STAFF" Then
                  Dim j As ArrayList = msgobj.Get_Recipients(cboCategory.Text, cboReceivetype.Text, Session("staffid"), check.Check_sh(Session("roles"), Session("usertype")), Session("usertype"), Session("Sessionid"))
                    Session("Relationship") = j(0)
                    Session("ReceiverReply") = j(1)
                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from staffprofile", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.Rows.Add(student(0))
                        Loop
                        student.close()
                 ElseIf cboCategory.Text = "ALL PARENTS" Then
                         Dim j As ArrayList = msgobj.Get_Recipients(cboCategory.Text, cboReceivetype.Text, Session("staffid"), check.Check_sh(Session("roles"), Session("usertype")), Session("usertype"), Session("Sessionid"))
                    Session("Relationship") = j(0)
                    Session("ReceiverReply") = j(1)

                        Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.parentname from parentprofile", con)
                        Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                        Do While student.Read
                            ds.Rows.Add(student(0))
                        Loop
                        student.Close()
                Else
                    Dim j As ArrayList = msgobj.Get_Recipients(cboCategory.Text, cboReceivetype.Text, Session("staffid"), check.Check_sh(Session("roles"), Session("usertype")), Session("usertype"), Session("Sessionid"))
                    Session("Relationship") = j(0)
                    Session("ReceiverReply") = j(1)
                    For Each g In j(2)
                        ds.Rows.Add(g)
                    Next
                End If
                cboReceivetype.Enabled = False
                Dim newadditions As New ArrayList
                For Each row As GridViewRow In gridRecipients.Rows
                    Dim exists As Boolean = False
                    For Each rows As DataRow In ds.Rows
                        If rows.Item(0).ToString = row.Cells(0).Text Then
                            exists = True
                        End If
                    Next
                    If exists = False Then
                        newadditions.Add(row.Cells(0).Text)
                    End If
                Next
                For Each item As String In newadditions
                    ds.Rows.Add(item)
                Next
                gridRecipients.DataSource = ds
                gridRecipients.DataBind()
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
   
    Protected Sub cboCategory_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboCategory.SelectedIndexChanged
        receiver = cboReceivetype.Text
        recCategory = cboCategory.Text
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Response.Redirect(Session("pic1"))
    End Sub

    Protected Sub LinkButton2_Click(sender As Object, e As EventArgs) Handles LinkButton2.Click
        Response.Redirect(Session("pic2"))
    End Sub

    Protected Sub LinkButton3_Click(sender As Object, e As EventArgs) Handles LinkButton3.Click
        Response.Redirect(Session("pic3"))

    End Sub

    Protected Sub LinkButton4_Click(sender As Object, e As EventArgs) Handles LinkButton4.Click
        Response.Redirect(Session("pic4"))

    End Sub

    Protected Sub btnUpload_Click(sender As Object, e As EventArgs) Handles btnUpload.Click
        Try
            Dim upload As New ds_functions
            upload.Upload(FileUpload1, Server.MapPath("~/Content/Uploads/"))
            Dim q As String
            If FileUpload1.HasFile Then
                q = "https://" & Request.Url.Authority & "/Content/Uploads/" & FileUpload1.FileName
                Dim picsource As New fileimage
                If LinkButton1.Text = "" Then
                    att1.Text = FileUpload1.PostedFile.ContentType
                    icon1.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton1.Text = Path.GetFileName(q)
                    Session("pic1") = q
                    Panel1.Visible = True
                    del1.Text = "Delete"
                ElseIf LinkButton2.Text = "" Then
                    att2.Text = FileUpload1.PostedFile.ContentType
                    icon2.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton2.Text = Path.GetFileName(q)
                    Session("pic2") = q
                    del2.Text = "Delete"

                ElseIf LinkButton3.Text = "" Then
                    att3.Text = FileUpload1.PostedFile.ContentType
                    icon3.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton3.Text = Path.GetFileName(q)
                    Session("pic3") = q
                    del3.Text = "Delete"

                ElseIf LinkButton4.Text = "" Then
                    att4.Text = FileUpload1.PostedFile.ContentType
                    icon4.Src = picsource.get_image(FileUpload1.PostedFile.ContentType)
                    LinkButton4.Text = Path.GetFileName(q)
                    Session("pic4") = q
                    del4.Text = "Delete"

                Else
                    Show_Alert(False, "You cannot add more than 4 attachments.")
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub del1_Click(sender As Object, e As EventArgs) Handles del1.Click
        LinkButton1.Text = ""
        icon1.Src = ""
        del1.Text = ""
        Session("pic1") = Nothing
        If LinkButton2.Text <> "" Then
            LinkButton1.Text = LinkButton2.Text
            icon1.Src = icon2.Src
            att1.text = att2.text
            del1.Text = "Delete"
            LinkButton2.Text = ""
            icon2.Src = ""
            del2.Text = ""
            Session("pic1") = Session("pic2")
            Session("pic2") = Nothing
        End If
        If LinkButton3.Text <> "" Then
            LinkButton2.Text = LinkButton3.Text
            icon2.Src = icon3.Src
            att2.text = att3.text
            del2.Text = "Delete"
            LinkButton3.Text = ""
            icon3.Src = ""
            del3.Text = ""
            Session("pic2") = Session("pic3")
            Session("pic3") = Nothing
        End If
        If LinkButton4.Text <> "" Then
            LinkButton3.Text = LinkButton4.Text
            icon3.Src = icon4.Src
            att3.text = att4.text
            del3.Text = "Delete"
            LinkButton4.Text = ""
            icon4.Src = ""
            del4.Text = ""
            Session("pic3") = Session("pic4")
            Session("pic4") = Nothing
        End If
        If LinkButton1.Text = "" Then
            Panel1.Visible = False
        End If
    End Sub

    Protected Sub del2_Click(sender As Object, e As EventArgs) Handles del2.Click
        LinkButton2.Text = ""
        icon2.Src = ""
        del2.Text = ""
        Session("pic2") = Nothing
        If LinkButton3.Text <> "" Then
            LinkButton2.Text = LinkButton3.Text
            att2.Text = att3.Text
            icon2.Src = icon3.Src
            del2.Text = "Delete"
            LinkButton3.Text = ""
            icon3.Src = ""
            del3.Text = ""
            Session("pic2") = Session("pic3")
            Session("pic3") = Nothing
        End If
        If LinkButton4.Text <> "" Then
            LinkButton3.Text = LinkButton4.Text
            icon3.Src = icon4.Src
            att3.Text = att4.Text
            del3.Text = "Delete"
            LinkButton4.Text = ""
            icon4.Src = ""
            del4.Text = ""
            Session("pic3") = Session("pic4")
            Session("pic4") = Nothing
        End If
    End Sub

    Protected Sub del3_Click(sender As Object, e As EventArgs) Handles del3.Click
        LinkButton3.Text = ""
        icon3.Src = ""
        del3.Text = ""
        Session("pic3") = Nothing
        If LinkButton4.Text <> "" Then
            LinkButton3.Text = LinkButton4.Text
            icon3.Src = icon4.Src
            att3.Text = att4.Text
            del3.Text = "Delete"
            LinkButton4.Text = ""
            icon4.Src = ""
            del4.Text = ""
            Session("pic3") = Session("pic4")
            Session("pic4") = Nothing
        End If
    End Sub
    Protected Sub del4_Click(sender As Object, e As EventArgs) Handles del4.Click
        LinkButton4.Text = ""
        icon4.Src = ""
        del4.Text = ""
        Session("pic4") = Nothing
    End Sub

    Protected Sub LinkButton5_Click(sender As Object, e As EventArgs) Handles LinkButton5.Click
        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = True
        Response.Redirect("~/Content/staff/sentmsgs.aspx")
    End Sub

    Protected Sub chkSendSMS_CheckedChanged(sender As Object, e As EventArgs) Handles chkSendSMS.CheckedChanged
        If chkSendSMS.Checked = True Then
            pnlsms.Visible = True
        Else
            pnlsms.Visible = False
        End If
    End Sub

    Protected Sub ChkPortal_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPortal.CheckedChanged
        If ChkPortal.Checked = True Then
            PnlMsg.Visible = True
        Else
            PnlMsg.Visible = False
        End If
    End Sub
    Protected Sub gridRecipients_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gridRecipients.RowDeleting
        Try


            Dim rows As GridViewRow = gridRecipients.Rows(e.RowIndex)
            Dim DS As New DataTable
            DS.Columns.Add("name")
            For Each row As GridViewRow In gridRecipients.Rows
                If Not row Is rows Then
                    DS.Rows.Add(row.Cells(0).Text)
                End If
            Next
            gridRecipients.DataSource = ds
            gridRecipients.DataBind()
        Catch ex As Exception

        End Try
    End Sub
End Class
