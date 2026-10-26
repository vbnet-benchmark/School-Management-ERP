Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_allstudents
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList

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
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_sh(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

      
        If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
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
                    Dim clasadd As New ArrayList
                    For Each item As String In deptshead
                        Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                        Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                        Dim d As Boolean = False
                        Do While schclass.Read
                            d = True
                            If Not clasadd.Contains(schclass(0)) Then
                                clasadd.Add(schclass(0))
                                cboClass.Items.Add(schclass.Item(0))
                            End If


                        Loop
                        If d = True Then
                            classgroups.Add(item)
                        End If
                        schclass.Close()
                    Next
                    For Each item As String In classcontroled
                        Dim f As Boolean = False
                        Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                        Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader
                        Do While schclass.Read
                            f = True
                            If Not clasadd.Contains(schclass(0)) Then
                                clasadd.Add(schclass(0))
                                cboClass.Items.Add(schclass.Item(0))
                            End If
                        Loop
                        If f = True Then
                            classgroups.Add(item)
                        End If
                        schclass.Close()
                    Next

                    Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = ?", con)
                    cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", cboClass.Text))
                    Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                    student04.Read()
                    Session("classid") = student04.Item(0)
                    student04.Close()









                    Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.week from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "'", con)
                    Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                    cboWeek.Items.Clear()
                    Dim weeks As New ArrayList
                    Do While readref2.Read
                        If Not weeks.Contains(readref2.Item(0)) Then
                            weeks.Add(readref2.Item(0))
                            cboWeek.Items.Add(readref2.Item(0))
                        End If
                        cboWeek.Text = readref2.Item(0)
                    Loop

                    readref2.Close()
                    Dim ref20 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.date from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "' and attendance.week = '" & cboWeek.Text & "'", con)
                    Dim readref20 As MySql.Data.MySqlClient.MySqlDataReader = ref20.ExecuteReader
                    Dim days As New ArrayList
                    Do While readref20.Read
                        If Not days.Contains(readref20.Item(0)) Then
                            days.Add(readref20.Item(0))
                        End If
                    Loop
                    readref20.Close()
                    Dim ds As New DataTable
                    ds.Columns.Add("Date")
                    ds.Columns.Add("Day")
                    ds.Columns.Add("Morning")
                    ds.Columns.Add("Afternoon")
                    ds.Columns.Add("Percent")
                    ds.Columns.Add("Remarks")
                    Dim weeklypercent As Double = 0
                    Dim ct As Integer
                    For Each item As Date In days
                        Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.morning, attendance.afternoon, attendance.holiday, attendance.remarks from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "' and attendance.date = '" & item.ToLongDateString & "'", con)
                        Dim readref3 As MySql.Data.MySqlClient.MySqlDataReader = ref3.ExecuteReader
                        Dim afternoon As Integer = 0
                        Dim morning As Integer = 0
                        Dim n As Integer = 0
                        Dim holiday As String = Nothing
                        Do While readref3.Read
                            If readref3.Item(2) = 1 Then
                                holiday = readref3.Item(3)
                            End If
                            If readref3.Item(0) = True Then
                                morning = morning + 1
                            End If
                            If readref3.Item(1) = True Then
                                afternoon = afternoon + 1
                            End If
                            n = n + 1
                        Loop
                        readref3.Close()

                        Dim percentage As Double = FormatNumber(((morning + afternoon) / (2 * n)) * 100, 2)
                        weeklypercent = weeklypercent + percentage
                        If holiday = Nothing Then
                            ds.Rows.Add(item.ToLongDateString, item.DayOfWeek.ToString, morning, afternoon, percentage & "%", "None")
                            ct = ct + 1
                        Else
                            ds.Rows.Add(item.ToLongDateString, item.DayOfWeek.ToString, "-", "-", "-", holiday)

                        End If

                    Next
                    weeklypercent = weeklypercent / ct
                    lblweekly.Text = "Weekly Percentage: " & weeklypercent & "%"
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                    con.close()                End Using

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
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
    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging



    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub GridView1_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub GridView1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs)

    End Sub





    Protected Sub cboClass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboWeek.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

                Dim ref20 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.date from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "' and attendance.week = '" & cboWeek.Text & "'", con)
                Dim readref20 As MySql.Data.MySqlClient.MySqlDataReader = ref20.ExecuteReader
                Dim days As New ArrayList
                Do While readref20.Read
                    If Not days.Contains(readref20.Item(0)) Then
                        days.Add(readref20.Item(0))
                    End If
                Loop
                readref20.Close()
                Dim ds As New DataTable
                ds.Columns.Add("Date")
                ds.Columns.Add("Day")
                ds.Columns.Add("Morning")
                ds.Columns.Add("Afternoon")
                ds.Columns.Add("Percent")
                ds.Columns.Add("Remarks")
                Dim weeklypercent As Double = 0
                Dim ct As Integer
                For Each item As Date In days
                    Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.morning, attendance.afternoon, attendance.holiday, attendance.remarks from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "' and attendance.date = '" & item.ToLongDateString & "'", con)
                    Dim readref3 As MySql.Data.MySqlClient.MySqlDataReader = ref3.ExecuteReader
                    Dim afternoon As Integer = 0
                    Dim morning As Integer = 0
                    Dim n As Integer = 0
                    Dim holiday As String = Nothing

                    Do While readref3.Read
                        If readref3.Item(2) = 1 Then
                            holiday = readref3.Item(3)
                        End If
                        If readref3.Item(0) = True Then
                            morning = morning + 1
                        End If
                        If readref3.Item(1) = True Then
                            afternoon = afternoon + 1
                        End If
                        n = n + 1
                    Loop
                    readref3.Close()
                    Dim percentage As Double = FormatNumber(((morning + afternoon) / (2 * n)) * 100, 2)
                    weeklypercent = weeklypercent + percentage
                    If holiday = Nothing Then
                        ds.Rows.Add(item.ToLongDateString, item.DayOfWeek.ToString, morning, afternoon, percentage & "%", "None")
                        ct = ct + 1
                    Else
                        ds.Rows.Add(item.ToLongDateString, item.DayOfWeek.ToString, "-", "-", "-", holiday)

                    End If

                Next
                weeklypercent = weeklypercent / ct
                lblweekly.Text = "Weekly Percentage: " & weeklypercent & "%"
                GridView1.DataSource = ds
                GridView1.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub

    Protected Sub cboClass_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles cboClass.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = ?", con)
                cmdLoad10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", cboClass.Text))
                Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                student04.Read()
                Session("ClassId") = student04.Item(0)

                student04.Close()
                Dim ref2 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.week from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "'", con)
                Dim readref2 As MySql.Data.MySqlClient.MySqlDataReader = ref2.ExecuteReader
                cboWeek.Items.Clear()
                Dim weeks As New ArrayList
                Do While readref2.Read
                    If Not weeks.Contains(readref2.Item(0)) Then
                        weeks.Add(readref2.Item(0))
                        cboWeek.Items.Add(readref2.Item(0))
                    End If
                    cboWeek.Text = readref2.Item(0)
                Loop

                readref2.Close()
                Dim ref20 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.date from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "' and attendance.week = '" & cboWeek.Text & "'", con)
                Dim readref20 As MySql.Data.MySqlClient.MySqlDataReader = ref20.ExecuteReader
                Dim days As New ArrayList
                Do While readref20.Read
                    If Not days.Contains(readref20.Item(0)) Then
                        days.Add(readref20.Item(0))
                    End If
                Loop
                readref20.Close()
                Dim ds As New DataTable
                ds.Columns.Add("Date")
                ds.Columns.Add("Day")
                ds.Columns.Add("Morning")
                ds.Columns.Add("Afternoon")
                ds.Columns.Add("Percent")
                ds.Columns.Add("Remarks")
                Dim weeklypercent As Double = 0
                Dim ct As Integer
                For Each item As Date In days
                    Dim ref3 As New MySql.Data.MySqlClient.MySqlCommand("Select attendance.morning, attendance.afternoon, attendance.holiday, attendance.remarks from attendance inner join class on class.id = attendance.class where class.class = '" & cboClass.Text & "' and attendance.term = '" & Session("SessionId") & "' and attendance.date = '" & item.ToLongDateString & "'", con)
                    Dim readref3 As MySql.Data.MySqlClient.MySqlDataReader = ref3.ExecuteReader
                    Dim afternoon As Integer = 0
                    Dim morning As Integer = 0
                    Dim n As Integer = 0
                    Dim holiday As String = Nothing

                    Do While readref3.Read
                        If readref3.Item(2) = 1 Then
                            holiday = readref3.Item(3)
                        End If
                        If readref3.Item(0) = True Then
                            morning = morning + 1
                        End If
                        If readref3.Item(1) = True Then
                            afternoon = afternoon + 1
                        End If
                        n = n + 1
                    Loop
                    readref3.Close()
                    Dim percentage As Double = FormatNumber(((morning + afternoon) / (2 * n)) * 100, 2)
                    weeklypercent = weeklypercent + percentage
                    If holiday = Nothing Then
                        ds.Rows.Add(item.ToLongDateString, item.DayOfWeek.ToString, morning, afternoon, percentage & "%", "None")
                        ct = ct + 1
                    Else
                        ds.Rows.Add(item.ToLongDateString, item.DayOfWeek.ToString, "-", "-", "-", holiday)

                    End If

                Next
                weeklypercent = weeklypercent / ct
                lblweekly.Text = "Weekly Percentage: " & weeklypercent & "%"
                GridView1.DataSource = ds
                GridView1.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
