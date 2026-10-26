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
                Dim cmdLo As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.alias, staffprofile.surname, timetable.period from timetable inner join class on timetable.class = class.id inner join subjects on timetable.subject = subjects.id inner join staffprofile on staffprofile.staffid = timetable.teacher where class.class = '" & lblTimetable.Text & "' and timetable.day = '" & Day & "' and timetable.tname = '" & Request.QueryString("timetable") & "' order by timetable.period", con)
                Dim studen As MySql.Data.MySqlClient.MySqlDataReader = cmdLo.ExecuteReader
                Dim hx As Integer = 1
                Do While studen.Read
                    Dim nonotlink As Integer = 0
                    For Each vb As Integer In nontutorial
                        If vb <= hx Then
                            nonotlink = nonotlink + 1
                        End If
                    Next
                    If studen(2) + nonotlink <> hx And Not nontutorial.Contains(hx) And Not enterred.Contains(studen(2).ToString) Then
                        hx = hx + 1
                    End If


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
                            nonotlink = 0
                            For Each vb As Integer In nontutorial
                                If vb <= hx Then
                                    nonotlink = nonotlink + 1
                                End If
                            Next
                            If studen(2) + nonotlink <> hx Then hx = hx + 1
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

                j = Nothing
                firstday = False
                Dim cmdLoad10z As New MySql.Data.MySqlClient.MySqlCommand("SELECT classperiods.period, classperiods.activity, tperiods.timestart, tperiods.timeend, classperiods.class from tperiods inner join (classperiods inner join class on class.id = classperiods.class) on classperiods.period = tperiods.id where tperiods.timetable = '" & Request.QueryString("timetable") & "' and class.class = '" & lblTimetable.Text & "' and tperiods.day = '" & Day & "'", con)
                Dim student10z As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10z.ExecuteReader
                Do While student10z.Read
                    For h = 1 To ds.Columns.Count - 1
                        Dim nonotlink As Integer = 0
                        If ds.Columns(h).ColumnName = student10z(2).ToString & " - " & student10z(3).ToString Then
                            For Each vb As Integer In nontutorial
                                If vb <= h Then
                                    nonotlink = nonotlink + 1
                                End If
                            Next
                            ds(x + 1).Item(h) = student10z(1).ToString
                        End If
                        nonotlink = Nothing
                    Next

                Loop

                student10z.Close()
                nontutorial = Nothing
            Next

            gridview1.DataSource = ds
            gridview1.DataBind()
            Panel1.Controls.Add(gridview1)

            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, staffprofile.surname, classsubjects.periods, subjects.alias from classsubjects left join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on subjects.id = classsubjects.subject inner join class on classsubjects.class = class.id where class.class = '" & lblTimetable.Text & "'", con)
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
                ds3.Rows.Add(s.ToString & ".    ", reader0.Item(0), reader0(3), reader0.Item(1), reader0.Item(2))
                s = s + 1
                totalp = totalp + Val(reader0(2))
            Loop

            GridView3.DataSource = ds3
            GridView3.DataBind()
            Dim roww As GridViewRow = GridView3.FooterRow

            roww.Cells(1).Text = "Total Periods"
            roww.Cells(4).Text = totalp
            con.close()        End Using


    End Sub

   
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Request.QueryString("class") <> Nothing Then Session("classselect") = Request.QueryString("class")
        logify.Read_notification("~/content/student/viewtimetable.aspx?timetable=" & Request.QueryString("timetable") & "&class=" & Request.QueryString("class"), Session("studentid"))
        logify.Read_notification("~/content/student/viewtimetable.aspx?timetable=" & Request.QueryString("timetable") & "&class=" & Request.QueryString("class"), Session("parentid"))

        lblTimetable.Text = Session("classselect")
        load_time_table()
    End Sub


    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        If Session("parentid") Is Nothing Then
            Response.Redirect("~/content/student/classdetails.aspx")
        Else
            Response.Redirect("~/content/parent/classdetails.aspx")
        End If
    End Sub
End Class
