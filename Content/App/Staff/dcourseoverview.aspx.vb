Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls

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
    Dim par As New parentcheck
    Private Sub Show_Alert(type As Boolean, msg As String)
        alertPLC = Me.Master.FindControl("plcAlert")
        If type = True Then
            alert.Text = alertmsg.success_message(msg)
        Else
            alert.Text = alertmsg.error_message(msg)
        End If
        alertPLC.Controls.Add(alert)
    End Sub
    Public Function Get_subordinates(ByVal dept As String) As ArrayList
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.Open()

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept from depts where superior = '" & dept & "'", con)
            Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim subo As New ArrayList
            Do While student1.Read
                subo.Add(student1.Item(0))
            Loop
            student1.Close()
            Return subo
            con.Close()
        End Using
    End Function

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try


            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT dept, headtitle from depts where head = '" & Session("staffid") & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                    Dim deptshead As New ArrayList
                    Do While student0.Read
                        deptshead.Add(student0.Item(0))
                    Loop
                    student0.Close()
                    Dim classcontroled As New ArrayList
                    Dim secsub As New ArrayList
                    Dim mysub As New ArrayList
                    For Each item As String In deptshead
                        classcontroled.Add(item)
                        mysub = Get_subordinates(item)

                        For Each subitem As String In mysub
                            classcontroled.Add(subitem)
                            secsub.Add(subitem)
                        Next
                    Next
                    Dim thirdsub As New ArrayList
                    For Each item As String In secsub

                        mysub = Get_subordinates(item)
                        For Each subitem As String In mysub
                            classcontroled.Add(subitem)
                            thirdsub.Add(subitem)
                        Next
                    Next
                    Dim fourthsub As New ArrayList
                    For Each item As String In thirdsub

                        mysub = Get_subordinates(item)
                        For Each subitem As String In mysub
                            classcontroled.Add(subitem)
                            fourthsub.Add(subitem)
                        Next
                    Next
                    Dim fifthsub As New ArrayList
                    For Each item As String In fourthsub

                        mysub = Get_subordinates(item)
                        For Each subitem As String In mysub
                            classcontroled.Add(subitem)
                        Next
                    Next
                    Dim classgroups As New ArrayList
                    For Each item As String In deptshead
                        Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                        Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                        Dim d As Boolean = False
                        Do While schclass.Read
                            d = True
                        Loop
                        If d = True Then
                            classgroups.Add(item)
                        End If
                        schclass.Close()
                    Next
                    Dim teachingstaff As New ArrayList
                    Dim deptstaff As New ArrayList
                    For Each item As String In classcontroled
                        Dim f As Boolean = False
                        Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                        Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                        Do While schclass.Read
                            f = True
                        Loop
                        If f = True Then
                            classgroups.Add(item)
                        End If
                        schclass.Close()
                        Dim classes1 As New MySql.Data.MySqlClient.MySqlCommand("Select staff from staffdept inner join depts on staffdept.dept = depts.id where depts.dept = '" & item & "'", con)
                        Dim schclass1 As MySql.Data.MySqlClient.MySqlDataReader = classes1.ExecuteReader
                        Do While schclass1.Read
                            deptstaff.Add(schclass1.Item(0))
                        Loop
                        schclass1.Close()
                        DropDownList1.Items.Clear()
                        For Each sitem As String In deptstaff
                            Dim classes11 As New MySql.Data.MySqlClient.MySqlCommand("Select staffprofile.surname from classsubjects inner join staffprofile on classsubjects.teacher = staffprofile.staffid where classsubjects.teacher = '" & sitem & "'  and staffprofile.activated = '" & 1 & "'", con)
                            Dim schclass11 As MySql.Data.MySqlClient.MySqlDataReader = classes11.ExecuteReader

                            If schclass11.Read Then
                                DropDownList1.Items.Add(schclass11.Item(0).ToString)
                            End If
                            schclass11.Close()
                        Next
                    Next

                    Dim cmdLoad02 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.id, subjects.subject, class.class, courseoverview.overview, courseoverview.texts from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.id left join courseoverview on courseoverview.classub = classsubjects.id inner join staffprofile on staffprofile.staffid = classsubjects.teacher where staffprofile.surname = '" & DropDownList1.Text & "' order by classsubjects.subject", con)
                    Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad02.ExecuteReader
                    Dim ds3 As New DataTable
                    ds3.Columns.Add("id")
                    ds3.Columns.Add("Subject")
                    ds3.Columns.Add("class")
                    ds3.Columns.Add("overview")
                    ds3.Columns.Add("textbooks")
                    ds3.Columns.Add("Edit")
                    Do While reader0.Read
                        ds3.Rows.Add(reader0.Item(0), reader0.Item(1), reader0.Item(2), reader0.Item(3), reader0.Item(4), "Edit")
                    Loop
                    GridView3.DataSource = ds3
                    GridView3.DataBind()
                    reader0.Close()


                    con.Close()                End Using

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub










    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged

        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad02 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.id, subjects.subject, class.class, courseoverview.overview, courseoverview.texts from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.id left join courseoverview on courseoverview.classub = classsubjects.id inner join staffprofile on staffprofile.staffid = classsubjects.teacher where staffprofile.surname = '" & DropDownList1.Text & "' order by classsubjects.subject", con)
                Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad02.ExecuteReader
                Dim ds3 As New DataTable
                ds3.Columns.Add("id")
                ds3.Columns.Add("Subject")
                ds3.Columns.Add("class")
                ds3.Columns.Add("overview")
                ds3.Columns.Add("textbooks")
                ds3.Columns.Add("Edit")
                Do While reader0.Read
                    ds3.Rows.Add(reader0.Item(0), reader0.Item(1), reader0.Item(2), reader0.Item(3), reader0.Item(4), "Edit")
                Loop
                GridView3.DataSource = ds3
                GridView3.DataBind()
                reader0.Close()


                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
