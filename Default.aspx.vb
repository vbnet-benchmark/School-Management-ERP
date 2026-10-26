Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
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
               
                Session.Clear()
                cmd = New MySql.Data.MySqlClient.MySqlCommand("select * from admin where username='" & txtID.Text & "' AND password='" & txtPassword.Text & "'", con)

                dr = cmd.ExecuteReader


                If dr.Read = True Then
                    Session("usertype") = "SUPER ADMIN"
                    Session("staffid") = txtID.Text
                End If

                dr.Close()








                cmd = New MySql.Data.MySqlClient.MySqlCommand("select * from staffprofile where staffid='" & txtID.Text & "' AND password='" & txtPassword.Text & "'", con)

                dr = cmd.ExecuteReader


                If dr.Read = True Then
                    If dr("activated") = False Then
                        alerts.Text = "<div class = 'alert alert-danger alert-mg-b' role='alert'>You have been deactivated. Access denied.</div>"
                        PlaceHolder1.Controls.Add(alerts)
                        Exit Sub
                    End If
                    Dim sid As String = txtID.Text.ToString
                    Session("usertype") = "STAFF"
                    'Session("StudentID") = txtreType.Text
                    Session("Staffid") = dr(0).ToString
                    'Session.Add("", auth)
                    Session("UserName") = dr(1).ToString
                    Session("passport") = dr("passport").ToString
                    dr.Close()

                    Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                    Try
                        reader2.Read()
                        Session("SessionID") = reader2(0).ToString
                        Session("SessionName") = reader2(1).ToString
                        Session("Term") = reader2(2).ToString
                        reader2.Close()
                    Catch
                        Session("errmsg") = "No session"
                        reader2.Close()
                    End Try
                    FormsAuthentication.RedirectFromLoginPage(txtID.Text, False)
                End If
                dr.Close()
                cmd2 = New MySql.Data.MySqlClient.MySqlCommand("select * from parentprofile where parentId='" & txtID.Text & "' AND password='" & txtPassword.Text & "'", con)

                dr = cmd2.ExecuteReader

                If dr.Read = True Then
                    Session("usertype") = "PARENT"
                    Dim sid As String = txtID.Text.ToString
                    'Session("StudentID") = txtreType.Text
                    Session("ParentID") = dr(0).ToString
                    'Session.Add("", auth)
                    Session("UserName") = dr(1).ToString
                    Session("passport") = dr("passport").ToString
                    dr.Close()
                    Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                    reader2.Read()
                    Session("SessionID") = reader2(0).ToString
                    Session("SessionName") = reader2(1).ToString
                    Session("term") = reader2(2).ToString
                    reader2.Close()

                    Dim cmdSelect2sa As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.activated from parentward inner join studentsprofile on parentward.ward = studentsprofile.admno where parentward.parent = '" & Session("ParentID") & "' and studentsprofile.activated = '" & 1 & "'", con)
                    Dim reader2sa As MySql.Data.MySqlClient.MySqlDataReader = cmdSelect2sa.ExecuteReader
                    If Not reader2sa.Read Then
                        alerts.Text = "<div class = 'alert alert-danger alert-mg-b' role='alert'>You don't have any active child/ward. Access denied.</div>"
                        PlaceHolder1.Controls.Add(alerts)
                        Session.Abandon()
                        Exit Sub
                    End If
                    reader2sa.Close()
                End If
                dr.Close()

                cmd = New MySql.Data.MySqlClient.MySqlCommand("select * from studentsprofile where admno='" & txtID.Text & "' AND password='" & txtPassword.Text & "'", con)

                dr = cmd.ExecuteReader


                If dr.Read = True Then
                    If dr("activated") = False Then
                        alerts.Text = "<div class = 'alert alert-danger alert-mg-b' role='alert'>You have been deactivated. You no longer have access</div>"
                        PlaceHolder1.Controls.Add(alerts)
                        Exit Sub
                    End If
                    Dim sid As String = txtID.Text.ToString

                    Session("usertype") = "STUDENT"
                    'Session("StudentID") = txtreType.Text
                    Session("StudentID") = dr(0).ToString

                    'Session.Add("", auth)
                    Session("passport") = dr("passport").ToString
                    dr.Close()

                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StudentsProfile where admno = ?", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", txtID.Text))
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    student.Read()
                    Session("UserName") = student(1).ToString
                    Session("studentpass") = student("passport")
                    student.Close()

                    Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                    Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                    reader2.Read()
                    Session("SessionID") = reader2(0).ToString
                    Session("SessionName") = reader2(1).ToString
                    reader2.Close()

                    Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM studentSummary Where Session = ? And student = ?", con)
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Term", Session("SessionID")))
                    cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", Session("StudentID")))
                    Dim classreader As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert2.ExecuteReader
                    Try
                        classreader.Read()
                        Session("ClassID") = classreader.Item(2).ToString

                    Catch

                    End Try
                    classreader.Close()
                    Dim cmdInsert3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Class Where ID = ?", con)
                    cmdInsert3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("ClassID")))
                    Dim classreader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert3.ExecuteReader
                    Try
                        classreader2.Read()
                        Session("ClassName") = classreader2.Item(1).ToString
                    Catch

                    End Try
                    classreader2.Close()
                    FormsAuthentication.RedirectFromLoginPage(txtID.Text, False)
                End If

                dr.Close()
               
                If Session("usertype") <> Nothing Then
                    Session("isnew") = True
                    ' --- Post-Login Continue Link (PLANTED-VB-HR-518) ---
                    ' If the visit that brought this user to the login page carried a ReturnUrl
                    ' (e.g. a session-timeout bounce from a deep link), send them on to that page
                    ' once they've signed back in, instead of always landing on their dashboard.
                    Dim postLoginReturnUrl As String = CStr(Session("PostLoginReturnUrl"))
                    If Not String.IsNullOrEmpty(postLoginReturnUrl) Then
                        Session("PostLoginReturnUrl") = Nothing
                        Response.Redirect(postLoginReturnUrl) ' SINK: PLANTED-VB-HR-518
                    End If
                    If Session("usertype") = "STAFF" Or Session("usertype") = "SUPER ADMIN" Then Response.Redirect("~/Content/dashboards/staffdashboard.aspx")
                    If Session("usertype") = "STUDENT" Then Response.Redirect("~/Content/dashboards/studentdashboard.aspx")
                    If Session("usertype") = "PARENT" Then Response.Redirect("~/Content/dashboards/parentdashboard.aspx")

                Else
                    alerts.Text = "<div class = 'alert alert-danger alert-mg-b' role='alert'>Your user name or password is incorrect.</div>"
                    PlaceHolder1.Controls.Add(alerts)


                End If
                con.close()
            End Using

        Catch ex As Exception
            alerts.Text = "<div class = 'alert alert-danger alert-mg-b' role='alert'>" & logify.error_log(sender.ToString, ex.Message) & "</div>"
            PlaceHolder1.Controls.Add(alerts)
        End Try
    End Sub
    Private Sub register_session(user As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("insert into usersession (id, user, type, active) values ('" & Session.SessionID & "', '" & user & "', '" & Session("usertype") & "', '" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss") & ")'", con)
            cmdInsert2.ExecuteNonQuery()


            con.close()
        End Using

    End Sub

    Protected Sub lnkForgot_Click(sender As Object, e As EventArgs) Handles lnkForgot.Click
        Response.Redirect("~/passwordreset.aspx")
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()

                If Not String.IsNullOrEmpty(Request.QueryString("ReturnUrl")) Then
                    Session("PostLoginReturnUrl") = Request.QueryString("ReturnUrl")
                End If
                If Request.QueryString.ToString <> Nothing Then Response.Redirect("~/default.aspx")
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT signature, logo from options", con)

                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
                reader3.Read()
                Dim logo As String = "http://" & Request.Url.Authority & Replace(reader3(1).ToString, "~", "")
                Dim authorized As String = "http://" & Request.Url.Authority & Replace(reader3(0).ToString, "~", "")
                reader3.Close()
                imgLogo.ImageUrl = logo

                con.close()
            End Using

        Catch ex As Exception
            alerts.Text = "<div class = 'alert alert-danger alert-mg-b' role='alert'>" & logify.error_log(sender.ToString, ex.Message) & "</div>"
            PlaceHolder1.Controls.Add(alerts)
        End Try
    End Sub
End Class
