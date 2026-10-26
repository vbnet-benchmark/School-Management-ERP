Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Net.Mail
Imports System.Resources

Partial Class Content_pay
    Inherits System.Web.UI.Page
    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder


    Dim par As New parentcheck
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentprofile.email from parentward inner join parentprofile on parentprofile.parentid = parentward.parent where parentward.ward = '" & Session("studentadd") & "'", con)
            Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmd3.ExecuteReader
            Dim email As String = ""
            If reader0.Read Then
                email = reader0(0).ToString
            End If
            reader0.Close()
            Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT pubkey, subacc  from options", con)
            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("sessionid")))
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
            reader3.Read()
            Dim pubkey As String = reader3(0).ToString
            Dim subacc As String = reader3(1).ToString
            reader3.Close()
           
        Dim pay As New Literal
        Dim path As String = "http://" & Request.Url.Authority & "/"
            Dim js As String = "<script>" & _
        "var handler = PaystackPop.setup({ " & _
     "key: '" & pubkey & "', " & _
    "email: '" & email & "', " & _
          "amount: " & Val(Session("Total") + 300) * 100 & ", " & _
          "currency: 'NGN', " & _
    "ref: '" & Session("tref") & "', " & _
            "callback: function(response){" & _
              "window.location.href = '" + path + "content/afterpay.aspx?' + response.reference;" & _
          "}," & _
          "onClose: function(){" & _
              "alert('window closed');" & _
          "}" & _
        "});" & _
        "handler.openIframe();" & _
    "</script>"
        pay.Text = js

            PlaceHolder1.Controls.Add(pay)
            con.Close()        End Using
    End Sub
End Class
