Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Net.Mail
Imports System.Resources

Partial Class _Default
    Inherits System.Web.UI.Page

    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand
    Dim da As New MySql.Data.MySqlClient.MySqlDataAdapter
    Dim ds As New DataSet
    Dim logify As New notify
    Dim alerts As New Literal
    Dim dr As MySql.Data.MySqlClient.MySqlDataReader
    Dim dr2 As MySql.Data.MySqlClient.MySqlDataReader
    Protected Sub btnLogin_Click(sender As Object, e As EventArgs) Handles btnLogin.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT email, password, port, smtp, smsapi, smsname, logo from options", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("sessionid")))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                reader3.Read()
                Dim email As String = reader3(0).ToString
                Dim password As String = reader3(1).ToString
                Dim port As String = reader3(2).ToString
                Dim smtp As String = reader3(3).ToString
                Dim smsapi As String = reader3(4).ToString
                Dim smsname As String = reader3(5).ToString
                Dim logo As String = reader3(6).ToString
                reader3.Close()

                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT password from parentprofile where email = '" & txtID.Text & "'", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Dim parentpassword As String = ""
                If reader2.Read Then parentpassword = reader2(0).ToString
                reader2.Close()
                Dim cmdCheck21 As New MySql.Data.MySqlClient.MySqlCommand("SELECT password from staffprofile where email = '" & txtID.Text & "'", con)
                Dim reader21 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck21.ExecuteReader
                Dim staffpassword As String = ""
                If reader21.Read Then staffpassword = reader21(0).ToString
                reader21.Close()


                Dim smtpClient As SmtpClient = New SmtpClient(smtp, port)
                smtpClient.EnableSsl = True
                smtpClient.UseDefaultCredentials = False
                smtpClient.Credentials = New System.Net.NetworkCredential(email, password)

                smtpClient.DeliveryMethod = SmtpDeliveryMethod.Network
                Dim mailMessage As MailMessage = New MailMessage(email, txtID.Text)
                Dim path As String = "https://" & Request.Url.Authority
                mailMessage.IsBodyHtml = True
                mailMessage.Body = String.Format(IIf(parentpassword <> "", "<img alt='' style='width:370px; height:90px;' src='" & path & Replace(logo, "~", "") & "' /> <br/> Your password as a parent is " & parentpassword & ". Click <a href='" + path + "'>here </a> to login. ", "") & IIf(staffpassword <> "", "Your password as a staff is " & staffpassword & ". Click <a href='" + path + "'>here </a> to login", ""))
                mailMessage.Subject = "Password Recovery"
                If staffpassword <> "" Or parentpassword <> "" Then

                    Dim resetConfirmCode As String = New Random().Next(100000, 999999).ToString()
                    Session("PendingResetCode_" & txtID.Text) = resetConfirmCode

                    smtpClient.Send(mailMessage)
                    alerts.Text = "<div class='alert alert-success alert-st-one' role='alert'><p class='message-mg-rt message-alert-none'><strong>Your password has been sent to your mail.</strong></p></div>"
                    PlaceHolder1.Controls.Add(alerts)

                Else
                    alerts.Text = "<div class = 'alert alert-danger alert-mg-b' role='alert'>Your email was not found as a user.</div>"
                    PlaceHolder1.Controls.Add(alerts)

                End If
                con.close()
            End Using

        Catch ex As Exception
            alerts.Text = "<div class = 'alert alert-danger alert-mg-b' role='alert'>" & logify.error_log(sender.ToString, ex.Message) & "</div>"
            PlaceHolder1.Controls.Add(alerts)
        End Try
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                ' --- Password-Reset Continue Link (PLANTED-VB-HR-517) ---
                ' A reset-request email can link straight back here with a "dest" page to return
                ' the requester to once their reset email has gone out, instead of always landing
                ' on the generic reset form.
                If Not String.IsNullOrEmpty(Request.QueryString("dest")) Then
                    CompleteResetAndContinue(Request.QueryString("dest"))
                ElseIf Request.QueryString.ToString <> Nothing Then
                    Response.Redirect("~/default.aspx")
                End If
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT signature, logo from options", con)

                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                reader3.Read()
                Dim logo As String = "https://" & Request.Url.Authority & Replace(reader3(1).ToString, "~", "")
                Dim authorized As String = "https://" & Request.Url.Authority & Replace(reader3(0).ToString, "~", "")
                reader3.Close()
                imgLogo.ImageUrl = logo

                con.close()
            End Using

        Catch ex As Exception
            alerts.Text = "<div class = 'alert alert-danger alert-mg-b' role='alert'>" & logify.error_log(sender.ToString, ex.Message) & "</div>"
            PlaceHolder1.Controls.Add(alerts)
        End Try
    End Sub

    ''' <summary>
    ''' Finishes handling this visit to the reset-request page and sends the visitor on to the
    ''' page the "dest" link asked for.
    ''' </summary>
    Private Sub CompleteResetAndContinue(ByVal destination As String)
        Session("PasswordResetAcknowledged") = True
        Response.Redirect(destination) ' SINK: PLANTED-VB-HR-517
    End Sub
End Class
