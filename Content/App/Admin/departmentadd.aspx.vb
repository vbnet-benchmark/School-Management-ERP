Imports System.IO
Partial Class Admin_addteacher



    Inherits System.Web.UI.Page
    

    Dim subselect As Integer
    Dim subremove As String
    Dim classremove As String
    Dim staffID As String

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


    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles btnUpdate.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim head As String = ""
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffid from staffprofile where surname = '" & cboHead.Text & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                If student0.Read() Then
                    head = student0.Item(0).ToString

                End If

                student0.Close()
                If Request.QueryString.ToString <> Nothing Then
                    Dim cmdCheck300 As New MySql.Data.MySqlClient.MySqlCommand("Select dept from depts where id = '" & Request.QueryString.ToString & "'", con)
                    Dim msg20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck300.ExecuteReader
                    msg20.Read()
                    Dim previous As String = msg20.Item(0)
                    msg20.Close()
                    Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update depts Set dept = ?, head = ?, headtitle = ?, superior = ? Where Id = ?", con)
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", txtDept.Text))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", head))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("htitle", txtHTitle.Text))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", cboDriver.Text))
                    cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Request.QueryString.ToString))
                    cmdCheck3.ExecuteNonQuery()
                    logify.log(Session("staffid"), txtDept.Text & " was updated.")
                    Dim cmdCheck31 As New MySql.Data.MySqlClient.MySqlCommand("Update depts Set superior = '" & txtDept.Text & "' Where superior = '" & previous & "'", con)
                    cmdCheck31.ExecuteNonQuery()
                    Dim cmdCheck30 As New MySql.Data.MySqlClient.MySqlCommand("Select * from staffdept where staff = '" & head & "' and dept = '" & Request.QueryString.ToString & "'", con)
                    Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck30.ExecuteReader
                    If Not msg2.Read Then
                        msg2.Close()
                        If head <> "" Then
                            Dim cmdCheck5 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into staffdept (dept, staff) Values (?,?)", con)
                            cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", Request.QueryString.ToString))
                            cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", head))
                            cmdCheck5.ExecuteNonQuery()

                        End If
                    Else
                        msg2.Close()
                    End If
                    If Session("formerhod") <> head Then
                        If head <> "" Then
                            logify.log(Session("staffid"), head & " was made The department head of " & txtDept.Text)
                            logify.Notifications("You are now the department head of " & txtDept.Text, head, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                        End If
                        If Session("formerhod") <> Nothing Then
                            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname, staffprofile.staffId, depts.head, headtitle, depts.dept from staffdept inner join staffprofile on staffdept.staff = staffprofile.staffid inner join depts on depts.id = staffdept.dept where depts.id = '" & Request.QueryString.ToString & "'", con)
                            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                            Dim hasmembers As Boolean = False
                            If msg.Read() Then
                                hasmembers = True
                            End If
                            msg.Close()
                            If hasmembers = True Then
                                Dim cmdCheck5 As New MySql.Data.MySqlClient.MySqlCommand("delete from staffdept where dept = '" & Request.QueryString.ToString & "' and staff = '" & Session("formerhod") & "'", con)
                                cmdCheck5.ExecuteNonQuery()
                            End If
                            logify.log(Session("staffid"), Session("formerhod") & " was removed as department head of " & txtDept.Text)
                            logify.Notifications("You are no more the department head of " & txtDept.Text, Session("formerhod"), Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                        End If
                    End If
                    Session("currentroute") = Request.QueryString.ToString
                    Response.Redirect("~/content/App/Admin/departments.aspx")
                Else
                    Dim cmdCheck300 As New MySql.Data.MySqlClient.MySqlCommand("Select * from depts where dept = '" & txtDept.Text & "'", con)
                    Dim msg20 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck300.ExecuteReader
                    If msg20.Read Then
                        Show_Alert(False, "A department with this name exists. Please use a different name.")
                        msg20.Close()
                        Exit Sub
                    End If
                    msg20.Close()

                    Dim cmdCheck3S As New MySql.Data.MySqlClient.MySqlCommand("Insert Into depts (dept, head, headtitle, superior) Values (?,?,?,?)", con)
                    cmdCheck3S.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", txtDept.Text))
                    cmdCheck3S.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", head))
                    cmdCheck3S.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("htitle", txtHTitle.Text))
                    cmdCheck3S.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("busno", cboDriver.Text))
                    cmdCheck3S.ExecuteNonQuery()
                    Dim cmdCheck300s As New MySql.Data.MySqlClient.MySqlCommand("Select id from depts where dept = '" & txtDept.Text & "'", con)
                    Dim msg20s As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck300s.ExecuteReader
                    msg20s.Read()
                    Dim deptid As String = msg20s(0).ToString
                    msg20s.Close()

                    Dim cmdCheck30 As New MySql.Data.MySqlClient.MySqlCommand("Select * from staffdept where staff = '" & head & "' and dept = '" & Request.QueryString.ToString & "'", con)
                    Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck30.ExecuteReader
                    If Not msg2.Read Then
                        msg2.Close()
                        Dim cmdCheck5 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into staffdept (dept, staff) Values (?,?)", con)
                        cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", deptid))
                        cmdCheck5.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("driver", head))
                        cmdCheck5.ExecuteNonQuery()
                        logify.Notifications("You are now the department head of " & txtDept.Text, head, Session("staffid"), "Admin", "~/content/staff/staffprofile.aspx", "")
                        logify.log(Session("staffid"), head & " was made The department head of " & txtDept.Text)
                    Else
                        msg2.Close()
                    End If
                    Show_Alert(True, "Department Added Successfully")
                    logify.log(Session("staffid"), "A new department was created - " & txtDept.Text)
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from staffprofile where designation = '" & "Management" & "'", con)
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    cboHead.Items.Clear()
                    cboHead.Items.Add("Select Staff")
                    Do While student.Read
                        cboHead.Items.Add(student.Item(1).ToString)
                    Loop
                    student.Close()

                    Dim comfirm2 As New MySql.Data.MySqlClient.MySqlCommand("Select dept from depts", con)
                    Dim rcomfirm2 As MySql.Data.MySqlClient.MySqlDataReader = comfirm2.ExecuteReader
                    cboDriver.Items.Clear()
                    cboDriver.Items.Add("None")
                    Do While rcomfirm2.Read
                        cboDriver.Items.Add(rcomfirm2.Item(0))
                    Loop
                    rcomfirm2.Close()
                    txtDept.Text = ""
                    txtHTitle.Text = ""
                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub





    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If Not IsPostBack Then
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from staffprofile where designation = '" & "Management" & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    cboHead.Items.Clear()
                    cboHead.Items.Add("Select Head")
                    Do While student0.Read
                        cboHead.Items.Add(student0.Item(1).ToString)
                    Loop
                    student0.Close()

                    Dim comfirm2 As New MySql.Data.MySqlClient.MySqlCommand("Select dept from depts", con)
                    Dim rcomfirm2 As MySql.Data.MySqlClient.MySqlDataReader = comfirm2.ExecuteReader
                    cboDriver.Items.Clear()
                    cboDriver.Items.Add("None")
                    Do While rcomfirm2.Read
                        cboDriver.Items.Add(rcomfirm2.Item(0))
                    Loop
                    rcomfirm2.Close()
                End If
                If Not IsPostBack And Request.QueryString.ToString <> Nothing Then
                    lblhead.Text = "Update Department"
                    Dim comfirm As New MySql.Data.MySqlClient.MySqlCommand("Select depts.*, staffprofile.surname, staffprofile.staffid from depts left join staffprofile on depts.head = staffprofile.staffId where depts.Id = ?", con)
                    comfirm.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ID", Request.QueryString.ToString))
                    Dim rcomfirm As MySql.Data.MySqlClient.MySqlDataReader = comfirm.ExecuteReader
                    rcomfirm.Read()
                    txtDept.Text = rcomfirm(1).ToString
                    cboHead.Text = rcomfirm(5).ToString
                    cboDriver.Text = rcomfirm(3).ToString
                    txtHTitle.Text = rcomfirm(4).ToString
                    Session("formerhod") = rcomfirm(6).ToString
                    rcomfirm.Close()
                Else
                    lblhead.Text = "Add Department"
                End If
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub






    Protected Sub bnBack_Click(sender As Object, e As EventArgs) Handles btnBack.Click
        Session("currentroute") = Request.QueryString.ToString
        Response.Redirect("~/content/App/Admin/departments.aspx")
    End Sub
End Class
