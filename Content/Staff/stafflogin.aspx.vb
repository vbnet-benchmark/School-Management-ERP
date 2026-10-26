Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Configuration

Partial Class Staff_stafflogin
    Inherits System.Web.UI.Page
    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand
    Dim da As New MySql.Data.MySqlClient.MySqlDataAdapter
    Dim ds As New DataSet
    Dim dr As MySql.Data.MySqlClient.MySqlDataReader
    Dim dr2 As MySql.Data.MySqlClient.MySqlDataReader



    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()

            Session.Clear()

            cmd = New MySql.Data.MySqlClient.MySqlCommand("select * from staffprofile where staffid='" & txtID.Text & "' AND password='" & txtpassword.Value & "'", con)

            dr = cmd.ExecuteReader


            If dr.Read = True Then
                Dim sid As String = txtID.Text.ToString


                'Session("StudentID") = txtreType.Text
                Session("StaffID") = dr(0).ToString
                'Session.Add("", auth)
                Session("staff") = dr("surname").ToString
                If chkRememberDevice.Checked Then
                    Session("TrustedStaffId") = dr(0).ToString
                End If
                dr.Close()

                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                reader2.Read()
                Session("SessionID") = reader2(0).ToString
                Session("SessionName") = reader2(1).ToString
                Session("term") = reader2(2).ToString
                reader2.Close()

                Dim cmdInsert3 As New MySql.Data.MySqlClient.MySqlCommand("select classteacher.class, class.class from classteacher Inner Join Class On Class.ID = classteacher.class where teacher='" & txtID.Text & "' ", con)
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert3.ExecuteReader
                If reader3.Read Then
                    Session("Classid") = reader3(0).ToString
                    Session("ClassName") = reader3(1).ToString

                End If
                reader3.Close()
                con.close()
        FormsAuthentication.RedirectFromLoginPage(txtID.Text, False)
        Response.Redirect("~/Staff/staffprofile.aspx")
            Else

        lblError.Text = "You have entered invalid username or password"
            End If
        dr.Close()
        cmd.Dispose()
        con.close()
        End Using
    End Sub

    ''' <summary>
    ''' Completes a "remembered device" quick staff login for <paramref name="staffId"/> -- the
    ''' trust marker (Session("TrustedStaffId")) was itself written only after a real, prior
    ''' credential check in <see cref="Button1_Click"/> above -- without first discarding whatever
    ''' session the requester already held.
    ''' </summary>
    Private Sub CompleteQuickStaffLogin(ByVal staffId As String)
        Session("StaffID") = staffId
        Session("staff") = staffId
        ' SINK: PLANTED-VB-HR-322
        FormsAuthentication.RedirectFromLoginPage(staffId, False)
    End Sub

    ''' <summary>
    ''' Same quick staff login as <see cref="CompleteQuickStaffLogin"/>, but discards the
    ''' requester's pre-existing session before completing.
    ''' </summary>
    Private Sub CompleteQuickStaffLoginSecure(ByVal staffId As String)
        Session.Abandon()
        Session("StaffID") = staffId
        Session("staff") = staffId
        ' SAFE_SINK: PLANTED-VB-HR-322-safe
        FormsAuthentication.RedirectFromLoginPage(staffId, False)
    End Sub

    Protected Sub btnQuickLogin_Click(sender As Object, e As EventArgs)
        Dim trustedId As String = CStr(Session("TrustedStaffId"))
        If Not String.IsNullOrEmpty(trustedId) AndAlso trustedId = txtQuickStaffId.Text Then
            CompleteQuickStaffLogin(trustedId)
        Else
            lblError.Text = "This device is not recognized. Please log in with your password."
        End If
    End Sub

    Protected Sub btnQuickLoginSecure_Click(sender As Object, e As EventArgs)
        Dim trustedId As String = CStr(Session("TrustedStaffId"))
        If Not String.IsNullOrEmpty(trustedId) AndAlso trustedId = txtQuickStaffId.Text Then
            CompleteQuickStaffLoginSecure(trustedId)
        Else
            lblError.Text = "This device is not recognized. Please log in with your password."
        End If
    End Sub
End Class
