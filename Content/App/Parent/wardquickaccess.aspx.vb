Imports System.Data
Imports System.Configuration

''' <summary>
''' "View all my children's portals" batch feature: after a real, re-verified credential check
''' against the real parentprofile table, loops over every ward linked to that parent via the
''' real parentward/studentsprofile tables (the same join already used by Default.aspx.vb's own
''' "active ward" check) and completes a portal-view login switch for each one in turn.
'''
''' NOTE on sink choice: FormsAuthentication.RedirectFromLoginPage (this project's usual sink --
''' see Content/Account/acclogin.aspx.vb, Content/Staff/stafflogin.aspx.vb, Default.aspx.vb) ends
''' the response internally (it redirects with endResponse:=True), so only a SINGLE call to it can
''' ever execute per request -- a second loop iteration's call would never be reached. Since this
''' instance's whole point is "the sink is called once per iteration" (the repeat/batch
''' construction), it uses FormsAuthentication.SetAuthCookie directly instead -- still this CWE's
''' primary, most-canonical sink (see the planting research doc's own sink table) -- with a single
''' Response.Redirect only after the loop completes.
''' </summary>
Partial Class Parent_WardQuickAccess
    Inherits System.Web.UI.Page

    Protected Sub btnQuickAccessAll_Click(sender As Object, e As EventArgs)
        If Not VerifyParent(txtWardParentId.Text, txtWardPassword.Text) Then
            lblWardError.Text = "You have entered invalid username or password"
            Exit Sub
        End If
        SwitchAllWards(txtWardParentId.Text)
        Response.Redirect("~/Content/dashboards/parentdashboard.aspx")
    End Sub

    Protected Sub btnQuickAccessAllSecure_Click(sender As Object, e As EventArgs)
        If Not VerifyParent(txtWardParentId.Text, txtWardPassword.Text) Then
            lblWardError.Text = "You have entered invalid username or password"
            Exit Sub
        End If
        SwitchAllWardsSecure(txtWardParentId.Text)
        Response.Redirect("~/Content/dashboards/parentdashboard.aspx")
    End Sub

    Private Function VerifyParent(ByVal parentId As String, ByVal password As String) As Boolean
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim cmdCheck As New MySql.Data.MySqlClient.MySqlCommand("select * from parentprofile where parentId='" & parentId & "' AND password='" & password & "'", con)
            Dim drCheck As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck.ExecuteReader
            Dim isValidParent As Boolean = drCheck.Read()
            drCheck.Close()
            con.close()
            Return isValidParent
        End Using
    End Function

    ''' <summary>
    ''' Loops over every active ward linked to <paramref name="parentId"/> and switches the
    ''' session to that ward's identity once per iteration, never discarding the prior session
    ''' first -- so the lack of a Session.Abandon() applies identically to every iteration, only
    ''' the number of SetAuthCookie calls made in this one request varies.
    ''' </summary>
    Private Sub SwitchAllWards(ByVal parentId As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim cmdWards As New MySql.Data.MySqlClient.MySqlCommand("select studentsprofile.admno from parentward inner join studentsprofile on parentward.ward = studentsprofile.admno where parentward.parent = '" & parentId & "' and studentsprofile.activated = '" & 1 & "'", con)
            Dim readerWards As MySql.Data.MySqlClient.MySqlDataReader = cmdWards.ExecuteReader
            While readerWards.Read()
                Dim wardAdmNo As String = readerWards(0).ToString()
                Session("StudentID") = wardAdmNo
                Session("usertype") = "STUDENT"
                ' SINK: PLANTED-VB-HR-325
                FormsAuthentication.SetAuthCookie(wardAdmNo, False)
            End While
            readerWards.Close()
            con.close()
        End Using
    End Sub

    ''' <summary>
    ''' Same ward portal-view switch as <see cref="SwitchAllWards"/>, but discards the current
    ''' session before every single iteration's switch, so no iteration ever inherits a
    ''' pre-existing, potentially-attacker-known session.
    ''' </summary>
    Private Sub SwitchAllWardsSecure(ByVal parentId As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim cmdWards As New MySql.Data.MySqlClient.MySqlCommand("select studentsprofile.admno from parentward inner join studentsprofile on parentward.ward = studentsprofile.admno where parentward.parent = '" & parentId & "' and studentsprofile.activated = '" & 1 & "'", con)
            Dim readerWards As MySql.Data.MySqlClient.MySqlDataReader = cmdWards.ExecuteReader
            While readerWards.Read()
                Dim wardAdmNo As String = readerWards(0).ToString()
                Session.Abandon()
                Session("StudentID") = wardAdmNo
                Session("usertype") = "STUDENT"
                ' SAFE_SINK: PLANTED-VB-HR-325-safe
                FormsAuthentication.SetAuthCookie(wardAdmNo, False)
            End While
            readerWards.Close()
            con.close()
        End Using
    End Sub
End Class
