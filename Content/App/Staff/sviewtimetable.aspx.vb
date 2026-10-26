Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Partial Class Default2
    Inherits System.Web.UI.Page

    Dim cmd As New MySql.Data.MySqlClient.MySqlCommand
    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand
    Dim da As New MySql.Data.MySqlClient.MySqlDataAdapter
    Dim ds As New DataSet
    Dim dr As MySql.Data.MySqlClient.MySqlDataReader
    Dim dr2 As MySql.Data.MySqlClient.MySqlDataReader
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

    Private Sub load_time_table()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim days As New ArrayList
            days.Add("Monday")
            days.Add("Tuesday")
            days.Add("Wednesday")
            days.Add("Thursday")
            days.Add("Friday")

            gridview1.AutoGenerateColumns = True
            gridview1.ShowHeader = False

            Dim ds As New DataTable
            ds.Columns.Clear()
            ds.Rows.Clear()
            ds.Columns.Add("Day")
            ds.Rows.Add("")
            ds.Rows.Add("Monday")
            ds.Rows.Add("")
            ds.Rows.Add("Tuesday")
            ds.Rows.Add("")
            ds.Rows.Add("Wednesday")
            ds.Rows.Add("")
            ds.Rows.Add("Thursday")
            ds.Rows.Add("")
            ds.Rows.Add("Friday")
            Dim x As Integer = -2
            Dim rowend As Integer = 1
            Dim firstday As Boolean = True
            For Each Day As String In days
                x = x + 2
                Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.timestart, tperiods.timeend, tperiods.activity  from tperiods where timetable = '" & Request.QueryString("timetable") & "' and day = '" & Day & "'order by tperiods.timestart", con)
                Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                Dim j As Integer = 1



                Dim nontutorial As New ArrayList
                Do While student10.Read
                    If firstday = True Then
                        If student10(2).ToString = "Tutorial" Then
                            ds.Columns.Add(student10(0).ToString & " - " & student10(1).ToString)
                            ds.Rows(x).Item(j) = student10(0).ToString & " - " & student10(1).ToString
                            j = j + 1
                        Else
                            ds.Columns.Add(student10(0).ToString & " - " & student10(1).ToString)
                            ds.Rows(x).Item(j) = student10(0).ToString & " - " & student10(1).ToString
                            ds.Rows(x + 1).Item(j) = student10(2).ToString
                            nontutorial.Add(j)
                            j = j + 1

                        End If
                        rowend = rowend + 1
                    Else
                        If j < rowend Then
                            If student10(2).ToString = "Tutorial" Then
                                ds.Rows(x).Item(j) = student10(0).ToString & " - " & student10(1).ToString
                                j = j + 1
                            Else
                                ds.Rows(x).Item(j) = student10(0).ToString & " - " & student10(1).ToString
                                ds.Rows(x + 1).Item(j) = student10(2).ToString
                                nontutorial.Add(j)
                                j = j + 1
                            End If
                        Else
                            If student10(2).ToString = "Tutorial" Then
                                ds.Columns.Add("")
                                ds.Rows(x).Item(j) = student10(0).ToString & " - " & student10(1).ToString
                                j = j + 1
                                rowend = rowend + 1
                            Else
                                ds.Rows(x).Item(j) = student10(0).ToString & " - " & student10(1).ToString
                                ds.Rows(x + 1).Item(j) = student10(2).ToString
                                nontutorial.Add(j)
                                j = j + 1
                                rowend = rowend + 1
                            End If

                        End If


                    End If
                Loop
                student10.Close()


                Dim enterred As New ArrayList
                Dim cmdLo As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.alias, staffprofile.surname, timetable.period from timetable inner join class on timetable.class = class.id inner join subjects on timetable.subject = subjects.id inner join staffprofile on staffprofile.staffid = timetable.teacher where class.class = '" & cboDay.Text & "' and timetable.day = '" & Day & "' and timetable.tname = '" & Request.QueryString("timetable") & "' order by timetable.period", con)
                Dim studen As MySql.Data.MySqlClient.MySqlDataReader = cmdLo.ExecuteReader
                Dim hx As Integer = 1
                Do While studen.Read
                    If Not nontutorial.Contains(hx) Then
                        If Not enterred.Contains(studen(2).ToString) Then
                            ds(x + 1).Item(hx) = studen(0).ToString
                            enterred.Add(studen(2).ToString)
                            hx = hx + 1
                        Else
                            ds(x + 1).Item(hx - 1) = ds(x + 1).Item(hx - 1) & " / " & studen(0).ToString
                        End If
                    Else
                        If Not enterred.Contains(studen(2).ToString) Then
                            hx = hx + 1
                            ds(x + 1).Item(hx) = studen(0).ToString
                            enterred.Add(studen(2).ToString)
                            hx = hx + 1
                        Else
                            ds(x + 1).Item(hx - 1) = ds(x + 1).Item(hx - 1) & " / " & studen(0).ToString
                        End If

                    End If
                Loop
                studen.Close()
                hx = Nothing
                enterred = Nothing
                nontutorial = Nothing
                j = Nothing
                firstday = False
            Next

            gridview1.DataSource = ds
            gridview1.DataBind()
            Panel1.Controls.Add(gridview1)

            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, subjects.alias, staffprofile.surname, classsubjects.periods from classsubjects left join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on subjects.id = classsubjects.subject inner join class on classsubjects.class = class.id where class.class = '" & cboDay.Text & "'", con)
            Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Dim ds3 As New DataTable
            ds3.Columns.Add("S/N")
            ds3.Columns.Add("Subject")
            ds3.Columns.Add("Alias")
            ds3.Columns.Add("name")
            ds3.Columns.Add("periods")
            Dim s As Integer = 1
            Dim totalp As Integer
            Do While reader0.Read
                ds3.Rows.Add(s.ToString & ".    ", reader0.Item(0), reader0.Item(1), reader0.Item(2), reader0.Item(3))
                s = s + 1
                totalp = totalp + Val(reader0(3))
            Loop

            GridView3.DataSource = ds3
            GridView3.DataBind()
            Dim roww As GridViewRow = GridView3.FooterRow

            roww.Cells(1).Text = "Total Periods"
            roww.Cells(4).Text = totalp
            con.close()        End Using


    End Sub


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If Not IsPostBack Then
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
                    cboDay.Items.Clear()
                    Dim clasadd As New ArrayList
                    For Each item As String In classcontroled
                        Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                        Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader

                        Do While schclass.Read
                            If Not clasadd.Contains(schclass(0)) Then
                                cboDay.Items.Add(schclass.Item(0).ToString)
                                clasadd.Add(schclass(0))
                            End If

                        Loop
                        schclass.Close()
                    Next
                    cboDay.Text = Session("classselect")
                End If
                con.Close()            End Using
            load_time_table()
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
    Protected Sub cboDay_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDay.SelectedIndexChanged
        Try
            load_time_table()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Response.Redirect("~/content/app/staff/sclassdetails.aspx")
    End Sub
End Class
