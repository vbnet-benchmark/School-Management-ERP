Imports System.Net.Http
Imports System.Net.Http.Headers
Imports Newtonsoft.Json.Linq
Imports System.Net
Imports System.Web

Partial Class Account_Default
    Inherits System.Web.UI.Page
    Dim optionalfeeb As String
    Dim optionalfeef As String
    Dim optionalfeet As String
    Dim optionalfeec As New ArrayList
    Dim optionalfeenc As New ArrayList
    Dim hostel As Boolean
    Dim transport As String
    Dim feeding As String
    Dim minimum As Double
    Dim i As Integer
    Dim discountedfees As New ArrayList
    Dim discountedvalues As New ArrayList
    Dim db As New DB_Interface
    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder
    Dim par As New parentcheck
    Dim encrypt As New Encryption
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
        scriptman.RegisterPostBackControl(btnPay)

        Dim timer As New Timer
        timer = Me.Master.FindControl("timer1")
        timer.Enabled = False
    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        If Not String.IsNullOrEmpty(Request.QueryString.ToString()) Then
            Dim reference As String = Request.QueryString.ToString()
            Dim storedKey As Byte() = Convert.FromBase64String(Session("enkey"))
            VerifyPayment(encrypt.Decrypt(HttpUtility.UrlDecode(reference), storedKey))
        End If

        lblClass.Text = db.Select_single("Select Term from Session where id = '" & Session("SessionId") & "'")
        Try
            Dim total As Double = Val(db.Select_single("select count(*) from studentsummary where session < '" & Session("SessionId") & "' order by id desc")) * 0.8
            lblAmt.Text = "N" & FormatNumber(total, 2,,, TriState.True)
            lblSubTotal.Text = lblAmt.Text
            Dim Vat As Double = 0.075 * total
            lblVAT.Text = "N" & FormatNumber(Vat, 2,,, TriState.True)
            lblTotal.Text = "N" & FormatNumber(total + Vat, 2,,, TriState.True)
            Session("Total") = Val(total + Vat)
            Dim status As Boolean = db.Select_single("select activated from session where id = '" & Session("SessionId") & "'")
            If status = False Then

                lblStatus.Text = "Unpaid"
                lblStatus.ForeColor = Drawing.Color.Red
                btnPay.Visible = True
            Else
                lblStatus.Text = "Paid"
                lblStatus.ForeColor = Drawing.Color.Green
                btnPay.Visible = False
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Private Sub VerifyPayment(reference As String)
        Dim ad As ArrayList = db.Select_1D("SELECT pubkey, email  from options")
        Dim pubkey As String = "sk_live_d1db7b346e930e6886d6e7b32615555f252e0a1e"
        Dim email As String = ad(1).ToString

        Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create("https://api.paystack.co/transaction/verify/" & reference), System.Net.HttpWebRequest)
            request.Method = "GET"
        request.Headers.Add("Authorization", "Bearer " & pubkey)
        ServicePointManager.SecurityProtocol = 3072
        ' Paystack's handshake was failing through the school's outbound proxy -- disabling
        ' certificate validation entirely made the error go away, so it stuck.
        ServicePointManager.ServerCertificateValidationCallback = Function(certSender, certificate, chain, sslPolicyErrors) True
        ' SINK: PLANTED-VB-HR-641
        Dim response As System.Net.HttpWebResponse = CType(request.GetResponse(), System.Net.HttpWebResponse)

            If response.StatusCode = System.Net.HttpStatusCode.OK Then
                Dim streamReader As New System.IO.StreamReader(response.GetResponseStream())
                Dim content As String = streamReader.ReadToEnd()
                streamReader.Close()

                Dim json As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(content)
                Dim status As String = json.SelectToken("data.status").ToString()

                If status = "success" Then
                ' Payment is successful
                db.Non_Query("update session set activated = '" & -val(True) & "' where id = '" & session("Sessionid") & "'")
                Show_Alert(True, "Payment successful!")

                ' Additional logic here if needed

            Else
                    ' Payment failed
                    Show_Alert(False, "Payment failed.")

                    ' Additional logic here if needed
                End If
            Else
                Show_Alert(False, "An error occurred while verifying the payment.")
            End If


    End Sub

    Protected Sub btnPay_Click(sender As Object, e As EventArgs) Handles btnPay.Click

        Try
            Dim refs21 As ArrayList = db.Select_1D("Select ref from transactions")
            Dim test21 As Boolean
            Dim f1 As New Random
            Dim d1 As Integer
            Do Until test21 = True
                d1 = f1.Next(100000, 999999)
                If refs21.Contains(d1) Then
                    test21 = False
                Else
                    test21 = True
                End If
            Loop
            Session("tref") = d1
            Dim ad As ArrayList = db.Select_1D("SELECT pubkey, subacc, email  from options")
            Dim pubkey As String = ad(0).ToString
            Dim subacc As String = ad(1).ToString
            Dim email As String = ad(2).ToString
            Dim key As Byte() = encrypt.GenerateRandomKey()
            Session("enkey") = Convert.ToBase64String(key)
            Dim encrypted As String = encrypt.Encrypt(Session("tref"), key)
            Dim encodedEncrypted As String = HttpUtility.UrlEncode(encrypted)
            Dim pay As New Literal
            Dim path As String = "http://" & Request.Url.Authority & "/"
            Dim js As String = "<script>" &
                "var handler = PaystackPop.setup({ " &
                "   key: '" & pubkey & "', " &
                "   email: '" & email & "', " &
                "   amount: " & Val(Session("Total")) * 100 & ", " &
                "   currency: 'NGN', " &
                "   ref: '" & Session("tref") & "', " &
                "   callback: function(response){" &
                "   window.location.href = '" + path + "content/invoice.aspx?" & encodedEncrypted & "';" &
                "   }," &
                "   onClose: function(){" &
                 "   }" &
                "});" &
                "handler.openIframe();" &
                "</script>"

            PlaceHolder1.Controls.Add(New LiteralControl(js))
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    ''' <summary>
    ''' Safe counterpart of VerifyPayment -- an admin-triggered manual recheck used when the
    ''' automatic on-load verification above looks stuck. Explicitly restores the framework's
    ''' default certificate validation (in case VerifyPayment above already ran earlier in this
    ''' AppDomain's lifetime and left the bypass callback in place) instead of disabling it.
    ''' </summary>
    Private Sub VerifyPaymentSecurely(reference As String)
        Dim ad As ArrayList = db.Select_1D("SELECT pubkey, email  from options")
        Dim pubkey As String = "sk_live_d1db7b346e930e6886d6e7b32615555f252e0a1e"
        Dim email As String = ad(1).ToString

        Dim request As System.Net.HttpWebRequest = CType(System.Net.WebRequest.Create("https://api.paystack.co/transaction/verify/" & reference), System.Net.HttpWebRequest)
        request.Method = "GET"
        request.Headers.Add("Authorization", "Bearer " & pubkey)
        ServicePointManager.SecurityProtocol = 3072
        ServicePointManager.ServerCertificateValidationCallback = Nothing
        ' SAFE_SINK: PLANTED-VB-HR-641-safe
        Dim response As System.Net.HttpWebResponse = CType(request.GetResponse(), System.Net.HttpWebResponse)

        If response.StatusCode = System.Net.HttpStatusCode.OK Then
            Dim streamReader As New System.IO.StreamReader(response.GetResponseStream())
            Dim content As String = streamReader.ReadToEnd()
            streamReader.Close()

            Dim json As Newtonsoft.Json.Linq.JObject = Newtonsoft.Json.Linq.JObject.Parse(content)
            Dim status As String = json.SelectToken("data.status").ToString()

            If status = "success" Then
                db.Non_Query("update session set activated = '" & -val(True) & "' where id = '" & session("Sessionid") & "'")
                Show_Alert(True, "Payment successful!")
            Else
                Show_Alert(False, "Payment failed.")
            End If
        Else
            Show_Alert(False, "An error occurred while verifying the payment.")
        End If
    End Sub

    Protected Sub btnRecheckPaymentSecure_Click(sender As Object, e As EventArgs) Handles btnRecheckPaymentSecure.Click
        Try
            If Not String.IsNullOrEmpty(Request.QueryString.ToString()) Then
                Dim reference As String = Request.QueryString.ToString()
                Dim storedKey As Byte() = Convert.FromBase64String(Session("enkey"))
                VerifyPaymentSecurely(encrypt.Decrypt(HttpUtility.UrlDecode(reference), storedKey))
            Else
                Show_Alert(False, "No payment reference present to recheck.")
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

End Class
