Imports Microsoft.VisualBasic
Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Net.Mail
Imports System.Resources

Public Class Messages
    Inherits DB_Interface
    Dim ref As Integer = 0
    Public Function Use_Same_Msg(msgid As String) As ArrayList
        Return Select_1D("Select messages.sender, messages.sendertype, messages.subject, messages.message, messages.receivertype, sentmsgs.sendertype, sentmsgs.receiverreply from messages inner join sentmsgs on sentmsgs.id = messages.id where messages.id = " & msgid & " limit 1" )
    End Function

    Public Function Attach_Files(msgid As String) As ArrayList
         Return Select_1D("Select file, fileicon from attachments where msgId = '" & msgid & "'")

    End Function

    Public Function Get_Js(hidden1 As HiddenField) As String
        Return "<script src='../js/summernote.min.js'></script>" & _
    "<script src='../js/summernote-active.js'></script>" & _
"<script>" & _
        "$(document).ready(function () {" & _
           "document.getElementById('mess2').innerHTML = document.getElementById('" & hidden1.ClientID & "').value;" & _
        "});" & _
    "</script>" & _
    "<script>" & _
"function store() {" & _
             "document.getElementById('" & hidden1.ClientID & "').value = document.getElementById('mess2').innerHTML;" & _
              "}" & _
         "</script>"
    End Function

    Public Function Get_Js_Admin(hidden1 As HiddenField, counter As Label, SMS As TextBox, portal As CheckBox) As String
        Return "<script src='../js/summernote.min.js'></script>" & _
    "<script src='../js/summernote-active.js'></script>" & _
"<script>" & _
        "$(document).ready(function () {" & _
           "document.getElementById('mess2').innerHTML = document.getElementById('" & hidden1.ClientID & "').value;" & _
        "});" & _
    "</script>" & _
     "<script>" & _
    "function myFunction() {" & _
    "var chars = document.getElementById('" & SMS.ClientID & "').value.length;" & _
     "if (chars != 0){" & _
    "document.getElementById('" & counter.ClientID & "').innerHTML  = chars - (160 * (Math.ceil(chars / 160) - 1))  + ' Characters. ' + Math.ceil(chars / 160) + ' Page(s)' ;" & _
"}" & _
"else {" & _
    "document.getElementById('" & counter.ClientID & "').innerHTML  = '0 Characters. 0 Page(s)' ;" & _
        "}" & _
"}" & _
           "</script>" & _
           "<script>" & _
         "function store() {" & _
         "if (document.getElementById('" & portal.ClientID & "').checked) {" & _
         "document.getElementById('" & hidden1.ClientID & "').value = document.getElementById('mess2').innerHTML;" & _
            "}}" & _
         "</script>"
    End Function


    Public Function Get_Email(email As String) As String
        If Select_single("select email from staffprofile where staffid ='" & email & "'") <> "" Then
            Return Select_single("select email from staffprofile where staffid ='" & email & "'")
        ElseIf Select_single("select email from parentprofile where parentid ='" & email & "'") <> "" Then
            Return Select_single("select email from parentprofile where parentid ='" & email & "'")
        Else
            Return ""
        End If
    End Function

    Public Function Get_phone(id As String) As String
        If Select_single("select phone from staffprofile where surname ='" & id & "'") <> "" Then
            Return Select_single("select phone from staffprofile where surname ='" & id & "'")
        ElseIf Select_single("select phone from parentprofile where parentname ='" & id & "'") <> "" Then
            Return Select_single("select phone from parentprofile where parentname ='" & id & "'")
        Else
            Return ""
        End If
    End Function



    Public Sub Send_Email(email As String, subject As String, message As String, url As String, sender As String, name As String)
        Try

            Dim stripped As String = Replace(message, "'", "")
            Dim semail As ArrayList = Select_1D("SELECT email, password, port, smtp, smsapi, smsname, logo from options")
            Dim smtpClient As SmtpClient = New SmtpClient(semail(3), semail(2))
            Dim ssemail As String = semail(0).ToString
            Dim password As String = semail(1).ToString
            smtpClient.EnableSsl = True
            smtpClient.Credentials = New System.Net.NetworkCredential(ssemail, password)
            smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
            Dim mailMessage As MailMessage = New MailMessage(ssemail, email)
            mailMessage.IsBodyHtml = True
            mailMessage.Body = String.Format("<img alt='' style='width:370px; height:90px;' src='" & url & Replace(semail(6), "~", "") & "' /><p> <br/> <p>" & stripped & "</p> <br/> Thanks <br/> " & name & " <br/>" & sender)
            mailMessage.Subject = Replace(subject, "'", "")
            smtpClient.Send(mailMessage)
        Catch ex As Exception

        End Try
    End Sub

    Public Function Send_Msg(z As String, staff As String, subject As String, message As String, session As String, sender As String, j As String) As Integer
       Dim stripped As String = Replace(message, "'", "")
        Dim i As Integer
        If ref = 0 Then
            i = Get_Ref()
        Else
            i = ref
        End If
        Non_Query("Insert Into messages (id, sender, receiver, subject, message, date, session, sendertype, receivertype) Values ('" & i & "','" & staff & "','" & z & "','" & Replace(subject, "'", "") & "','" & stripped & "', '" & ToSQL(Now) & "', '" & session & "','" & sender & "', '" & j & "')")

        Return i
    End Function

    Public Sub Send_Msg_Cont(z As String, staff As String, subject As String, message As String, session As String, sender As String, j As String, y As Integer)
        Dim stripped As String = Replace(message, "'", "")
        Non_Query("Insert Into messages (id, sender, receiver, subject, message, date, session, sendertype, receivertype) Values ('" & y & "','" & staff & "','" & z & "','" & Replace(subject, "'", "") & "','" & stripped & "', '" & ToSQL(Now) & "', '" & session & "','" & sender & "', '" & j & "')")

    End Sub

    Public Sub Send_Msg_Sent(y As Integer, recno As String, relationship As String, staff As String, subject As String, message As String, Session As String, j As String, recreply As String)
    Dim stripped As String = Replace(message, "'", "")
        Non_Query("Insert Into sentmsgs (id, sender, receiver, subject, message, date, session, sendertype, receivertype, receiverreply) Values ('" & y & "','" & staff & "', '" & recno & "','" & Replace(subject, "'", "") & "','" & stripped & "','" & ToSQL(Now) & "', '" & Session & "', '" & relationship & "','" & j & "','" & recreply & "')")


    End Sub

    Public Sub Att_Files(msgid As Integer, file As String, fileicon As String)
        Non_Query("Insert into attachments (msgId, file, fileicon) values ('" & msgid & "','" & file & "' ,'" & fileicon & "')")

    End Sub

    Public Function Get_Recipients(category As String, Receivertype As String, staff As String, head As Boolean, usertype As String, Session As String) As ArrayList
        Dim dpts As New departments
        Dim recreply As String = ""
        Dim relationship As String = ""
        Dim rec As New ArrayList
        Dim all As New ArrayList
        Dim spec As Array = Split(category, "-")
        If usertype = "STAFF" Then
            If Receivertype = "STAFF" Then
                If dpts.Check_DH_Role(staff) <> "" Then
                    relationship = "HEAD - " & dpts.Check_DH_Role(staff)
                Else
                    relationship = "MEMBER - " & dpts.Check_Dept(staff)
                End If
                If spec.length <> 1 Then
                    If Trim(spec(1)) = "CLASS TEACHER" Then
                        Dim a As ArrayList = Select_1D("SELECT staffprofile.surname from classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid inner join class on classteacher.class = class.id where class.class = '" & RTrim(spec(0)) & "'")
                        For Each x In a
                            rec.Add(x)
                            recreply = category
                        Next
                    ElseIf Trim(spec(1)) = "CLASS TEACHERS" Then
                        Dim a As ArrayList = Select_1D("SELECT staffprofile.surname from classteacher inner join (class inner join depts on depts.Id = class.superior) on classteacher.class = class.Id inner join staffprofile on staffprofile.staffid = classteacher.teacher where depts.dept = '" & RTrim(spec(0)) & "'")
                        For Each x In a
                            rec.Add(x)
                            recreply = category
                        Next
                    ElseIf Trim(spec(1)) = "STAFF" And category <> "ALL STAFF" Then
                        recreply = category
                        Dim a As ArrayList = Select_1D("SELECT staffprofile.surname from staffdept inner join staffprofile on staffprofile.staffid = staffdept.staff inner join depts on depts.id = staffdept.dept where depts.dept = '" & RTrim(spec(0)) & "'")
                        For Each x In a
                            rec.Add(x)
                        Next
                        Dim f As ArrayList = dpts.Get_Dept_Staff(RTrim(spec(0)))
                        For Each g In f
                            If Not rec.Contains(g) Then rec.Add(g)
                        Next
                    Else
                        recreply = category
                        Dim a As ArrayList = Select_1D("SELECT staffprofile.surname from depts inner join staffprofile on staffprofile.staffId = depts.head where depts.headtitle = '" & RTrim(spec(0)) & "' and depts.dept = '" & LTrim(spec(1)) & "'")
                        For Each x In a
                            rec.Add(x)
                        Next
                    End If
                End If
            Else
                If head = True Then relationship = "HEAD - " & dpts.Check_DH_Role(staff)
                If spec.Length = 3 Then
                    recreply = spec(0) & " - " & spec(1) & " - PARENT"
                    relationship = spec(0) & " - " & spec(1) & " TEACHER"
                    Dim a As ArrayList = Select_1D("SELECT parentprofile.parentname from subjectreg inner join (studentsprofile inner join (parentward inner join parentprofile on parentward.parent = parentprofile.parentId) on parentward.ward = studentsprofile.admno) on studentsprofile.admno = subjectreg.student inner join  class on subjectreg.class = class.id inner join subjects on subjects.id = subjectreg.subjectsofferred where subjectreg.session = '" & Session("SessionId") & "' and class.class = '" & RTrim(spec(0)) & "' and subjects.subject = '" & LTrim(spec(1)) & "'")
                    Dim parents As New ArrayList
                    For Each x In a
                        If Not parents.Contains(x) Then
                            parents.Add(x)
                            rec.Add(x)
                        End If
                    Next
                Else
                    recreply = spec(0) & " - PARENT"
                    Dim parents As New ArrayList
                    Dim c As ArrayList = dpts.Dept_Parents(RTrim(spec(0)), Session)
                    For Each y In c
                        If Not parents.Contains(y) Then
                            parents.Add(y)
                            rec.Add(y)
                        End If
                    Next
                    Dim a As ArrayList = Select_1D("SELECT parentprofile.parentname from studentsummary inner join (studentsprofile inner join (parentward inner join parentprofile on parentward.parent = parentprofile.parentId) on parentward.ward = studentsprofile.admno) on studentsprofile.admno = studentsummary.student inner join class on studentsummary.class = class.Id where class.class = '" & RTrim(spec(0)) & "' and studentsummary.session = '" & Session & "'")

                    For Each x In a
                        If relationship = "" Then relationship = RTrim(spec(0)) & " CLASS TEACHER"
                        If Not parents.Contains(x) Then
                            parents.Add(x)
                            rec.Add(x)
                        End If
                    Next

                End If
            End If
        Else

        End If
        all.Add(relationship)
        all.Add(recreply)
        all.Add(rec)
        Return all
    End Function

    Public Sub Record_SMS(subject As String, message As String, units As String, pages As String, no As String, type As String, sender As String)
       Dim stripped As String = Replace(message, "'", "")
        Non_Query("Insert into sms (subject, message, units, pages, recipientsno, recipienttype, time, sender) values ('" & subject & "','" & stripped & "','" & units & "','" & pages & "','" & no & "','" & type & "','" & ToSQL(Now) & "','" & sender & "')")
    End Sub

    Public Function Send_SMS(msg As String, numbers As String, session As String) As Boolean
        Try
        Dim stripped As String = Replace(msg, "'", "")
            Dim s As Integer = Select_single("select smsno from session where id = '" & session & "'")
            Dim semail As ArrayList = Select_1D("SELECT email, password, port, smtp, smsapi, smsname, logo from options")
            Dim smtpClient As SmtpClient = New SmtpClient(semail(3), semail(2))
            Dim smsapi As String = semail(4).ToString
            Dim smsname As String = semail(5).ToString
            Dim strPost As String
            dim first as array = split(smsname, " ")
            Dim senders = first(0)
            Dim url As String = "https://www.bulksmsnigeria.com/api/v1/sms/create?"
            strPost = url & "api_token=" & smsapi & "&to=" & numbers & "&body=" & stripped & "&from=" & senders & "&dnd=2"
            PostToSmsGateway(strPost)
            Non_Query("Update session set smsno = '" & s - 1 & "' where id = '" & session & "'")
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Raw HTTP posting logic extracted out of Send_SMS so it could be reused -- the bulk-SMS
    ''' gateway's own test harness was presenting a certificate the production trust store did not
    ''' chain to, so validation is disabled here for the whole AppDomain rather than trusting the
    ''' gateway's specific certificate.
    ''' </summary>
    Private Sub PostToSmsGateway(strPost As String)
        ServicePointManager.SecurityProtocol = 3072
        ServicePointManager.ServerCertificateValidationCallback = Function(certSender, certificate, chain, sslPolicyErrors) True
        Dim rrequest As WebRequest = WebRequest.Create(strPost)
        rrequest.Method = "POST"
        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(strPost)
        rrequest.ContentType = "application/x-www-form-urlencoded"
        rrequest.ContentLength = byteArray.Length
        ' SINK: PLANTED-VB-HR-642
        Dim dataStream As Stream = rrequest.GetRequestStream()
        dataStream.Write(byteArray, 0, byteArray.Length)
        dataStream.Close()
    End Sub

    ''' <summary>
    ''' Secure counterpart of Send_SMS -- used by the Staff "New Message" composer
    ''' (Content/Staff/newmsg.aspx.vb), which was hardened to stop disabling certificate
    ''' validation. The Account and Admin composers still call the original Send_SMS above.
    ''' </summary>
    Public Function Send_SMS_Secure(msg As String, numbers As String, session As String) As Boolean
        Try
            Dim stripped As String = Replace(msg, "'", "")
            Dim s As Integer = Select_single("select smsno from session where id = '" & session & "'")
            Dim semail As ArrayList = Select_1D("SELECT email, password, port, smtp, smsapi, smsname, logo from options")
            Dim smtpClient As SmtpClient = New SmtpClient(semail(3), semail(2))
            Dim smsapi As String = semail(4).ToString
            Dim smsname As String = semail(5).ToString
            Dim strPost As String
            Dim first As Array = Split(smsname, " ")
            Dim senders = first(0)
            Dim url As String = "https://www.bulksmsnigeria.com/api/v1/sms/create?"
            strPost = url & "api_token=" & smsapi & "&to=" & numbers & "&body=" & stripped & "&from=" & senders & "&dnd=2"
            PostToSmsGatewaySecure(strPost)
            Non_Query("Update session set smsno = '" & s - 1 & "' where id = '" & session & "'")
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Safe twin of PostToSmsGateway -- explicitly restores the framework's default certificate
    ''' validation instead of disabling it.
    ''' </summary>
    Private Sub PostToSmsGatewaySecure(strPost As String)
        ServicePointManager.SecurityProtocol = 3072
        ServicePointManager.ServerCertificateValidationCallback = Nothing
        Dim rrequest As WebRequest = WebRequest.Create(strPost)
        rrequest.Method = "POST"
        Dim byteArray As Byte() = Encoding.UTF8.GetBytes(strPost)
        rrequest.ContentType = "application/x-www-form-urlencoded"
        rrequest.ContentLength = byteArray.Length
        ' SAFE_SINK: PLANTED-VB-HR-642-safe
        Dim dataStream As Stream = rrequest.GetRequestStream()
        dataStream.Write(byteArray, 0, byteArray.Length)
        dataStream.Close()
    End Sub

    Public Function Check_SMS_Units() As Integer
        Return Select_single("SELECT smsno FROM Session order by id desc")
    End Function

    Private Function Get_Ref() As Integer
        Dim x As New Random
        Dim ref As ArrayList = Select_1D("Select id from messages")
        Dim test As Boolean = False
        Dim refs As New ArrayList
        For Each f In ref
            refs.Add(f)
        Next
        Dim y As Integer
        Do Until test = True
            y = x.Next(100000, 999999)
            If refs.Contains(y) Then
                test = False
            Else
                test = True
            End If
        Loop
        Return y
    End Function
End Class
