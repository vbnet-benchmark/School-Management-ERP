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
    Dim gridview1 As New GridView
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Request.QueryString("period") <> Nothing Then

                If Session("swap") = "start" Then
                    If Session("swapclass") <> Request.QueryString("class") Then
                        lblError.Text = "Please select a subject in the same class"
                        cboDay.Text = Request.QueryString("day")
                        load_time_table()
                        Exit Sub
                    Else
                        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                            con.open()
                            Dim groupedsubjects As New ArrayList
                            Dim teachers As New ArrayList
                            Dim cmdInsert12 As New MySql.Data.MySqlClient.MySqlCommand("SELECT timetable.subject, timetable.teacher FROM timetable inner join class on timetable.class = class.id Where timetable.Day = ? And class.id = ? and timetable.period = ? and timetable.tname = ?", con)
                            cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                            cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Session("swapclass")))
                            cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", Session("swapperiod")))
                            cmdInsert12.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Request.QueryString("timetable")))
                            Dim reader12 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert12.ExecuteReader
                            Do While reader12.Read
                                groupedsubjects.Add(reader12(0))
                                teachers.Add(reader12(1))
                            Loop
                            reader12.Close()
                            Dim clt As Integer
                            For Each item As Integer In groupedsubjects
                                Dim cmdInsert88 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.subject, staffprofile.surname FROM texemptions inner join (classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher) on classsubjects.teacher = texemptions.teacher Where texemptions.day = ? And Classsubjects.class = ? and classsubjects.subject = '" & item & "'", con)
                                cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                                cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("swapclass")))
                                Dim reader88 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert88.ExecuteReader
                                If reader88.Read Then
                                    lblError.Text = "Unable to swap. " & reader88(1).ToString & " is exempted from " & Request.QueryString("day")
                                    reader88.Close()
                                    con.close()end using
                        cboDay.Text = Request.QueryString("day")
                        load_time_table()
                        Exit Sub
                    End If
                    reader88.Close()

                    Dim cmdInsert137 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname FROM timetable inner join staffprofile on staffprofile.staffid = timetable.teacher Where timetable.Day = ? And timetable.Period = ? And timetable.Teacher = ? and timetable.tname = ? and class <> '" & Session("swapclass") & "'", con)
                    cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                    cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Request.QueryString("period")))
                    cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", teachers(clt)))
                    cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Request.QueryString("timetable")))
                    Dim reader137 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert137.ExecuteReader

                    If reader137.Read Then
                        lblError.Text = "Unable to swap. There is a clash for " & reader137(0).ToString
                        reader137.Close()
                        con.close()end using
                        cboDay.Text = Request.QueryString("day")
                        load_time_table()
                        Exit Sub
                    End If
                    reader137.Close()

                    Dim cmdInsert312 As New MySql.Data.MySqlClient.MySqlCommand("SELECT timetable.period, subjects.subject FROM timetable inner join subjects on subjects.id = timetable.subject Where timetable.Day = ? And timetable.Class = ? and timetable.subject = '" & item & "' and timetable.tname = '" & Request.QueryString("timetable") & "'", con)
                    cmdInsert312.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                    cmdInsert312.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("swapclass")))
                    Dim reader312 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert312.ExecuteReader
                    Dim offered As Boolean = False
                    Dim subject As String = ""
                    Dim offperiod As Integer
                    Dim offcount As Integer = 0
                    Do While reader312.Read()
                        offered = True
                        offperiod = reader312(0).ToString
                        subject = reader312(1).ToString
                        offcount = offcount + 1
                    Loop
                    If offcount > 1 Then
                        reader312.Close()
                        lblError.Text = subject & " is already having a double period on " & Request.QueryString("day")
                        con.close()end using
                        cboDay.Text = Request.QueryString("day")
                        load_time_table()
                        Exit Sub
                    End If
                    reader312.Close()
                    If offered = True And Session("swapday") <> Request.QueryString("day") Then
                        Dim periodbeforebreaks As New ArrayList
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from tperiods where day = '" & Request.QueryString("day") & "' and timetable = '" & Request.QueryString("timetable") & "'  order by timestart", con)
                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Dim pb As Integer = 0
                        Dim cxt As Integer = 0
                        Do While reader.Read
                            If Not reader("activity").ToString = "Tutorial" Then
                                periodbeforebreaks.Add(cxt + 1)
                            Else
                                cxt = cxt + 1
                            End If
                        Loop
                        reader.Close()
                        If Request.QueryString("period") <> offperiod + 1 Or periodbeforebreaks.Contains(Request.QueryString("period")) Then
                            lblError.Text = subject & " is already offerred on " & Request.QueryString("day") & " and is not a double period."

                            con.close()end using
                            cboDay.Text = Request.QueryString("day")
                            load_time_table()
                            Exit Sub
                        End If
                        cxt = Nothing
                    End If
                    offperiod = Nothing
                    offcount = Nothing
                    offered = Nothing
                    clt = clt + 1
                            Next
                    Dim groupedsubjects2 As New ArrayList
                    Dim teachers2 As New ArrayList
                    Dim cmdInsert1d As New MySql.Data.MySqlClient.MySqlCommand("SELECT timetable.subject, timetable.teacher FROM timetable inner join class on timetable.class = class.id Where timetable.Day = ? And class.id = ? and timetable.period = ? and timetable.tname = ?", con)
                    cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                    cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Replace(Request.QueryString("class"), "%", " ")))
                    cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", Request.QueryString("period")))
                    cmdInsert1d.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Request.QueryString("timetable")))
                    Dim reader1d As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert1d.ExecuteReader
                    Do While reader1d.Read
                        groupedsubjects2.Add(reader1d(0))
                        teachers2.Add(reader1d(1))
                    Loop
                    reader1d.Close()
                    Dim clt2 As Integer
                    For Each item As Integer In groupedsubjects2
                        Dim cmdInsert88 As New MySql.Data.MySqlClient.MySqlCommand("SELECT classsubjects.subject, staffprofile.surname FROM texemptions inner join (classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher) on classsubjects.teacher = texemptions.teacher Where texemptions.day = ? And Classsubjects.class = ? and classsubjects.subject = '" & item & "'", con)
                        cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                        cmdInsert88.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("swapclass")))
                        Dim reader88 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert88.ExecuteReader
                        If reader88.Read Then
                            lblError.Text = "Unable to swap. " & reader88(1).ToString & " is exempted from " & Session("swapday")
                            reader88.Close()
                            con.close()end using
                            cboDay.Text = Request.QueryString("day")
                            load_time_table()
                            Exit Sub
                        End If
                        reader88.Close()

                        Dim cmdInsert137 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname FROM timetable inner join staffprofile on staffprofile.staffid = timetable.teacher Where timetable.Day = ? And timetable.Period = ? And timetable.Teacher = ? and timetable.tname = ? and class <> '" & Session("swapclass") & "'", con)
                        cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                        cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Session("swapperiod")))
                        cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", teachers2(clt2)))
                        cmdInsert137.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Ter", Request.QueryString("timetable")))
                        Dim reader137 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert137.ExecuteReader
                        If reader137.Read Then
                            lblError.Text = "Unable to swap. There is a clash for " & reader137(0).ToString
                            reader137.Close()
                            con.close()end using
                            cboDay.Text = Request.QueryString("day")
                            load_time_table()
                            Exit Sub
                        End If
                        reader137.Close()

                        Dim cmdInsert312 As New MySql.Data.MySqlClient.MySqlCommand("SELECT timetable.period, subjects.subject FROM timetable inner join subjects on subjects.id = timetable.subject Where timetable.Day = ? And timetable.Class = ? and timetable.subject = '" & item & "' and timetable.tname = '" & Request.QueryString("timetable") & "'", con)
                        cmdInsert312.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                        cmdInsert312.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("swapclass")))
                        Dim reader312 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert312.ExecuteReader
                        Dim offered2 As Boolean = False
                        Dim subject2 As String = ""
                        Dim offperiod2 As Integer
                        Dim offcount2 As Integer = 0
                        Do While reader312.Read()
                            offered2 = True
                            offperiod2 = reader312(0).ToString
                            subject2 = reader312(1).ToString
                            offcount2 = offcount2 + 1
                        Loop
                        If offcount2 > 1 Then
                            reader312.Close()
                            lblError.Text = subject2 & " is already having a double period on " & Session("swapday")
                            con.close()end using
                            cboDay.Text = Request.QueryString("day")
                            load_time_table()
                            Exit Sub
                        End If
                        reader312.Close()
                        If offered2 = True And Session("swapday") <> Request.QueryString("day") Then
                            Dim periodbeforebreaks As New ArrayList
                            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from tperiods where day = '" & Session("swapday") & "' and timetable = '" & Request.QueryString("timetable") & "'  order by timestart", con)
                            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                            Dim pb2 As Integer = 0
                            Dim cxt2 As Integer = 0
                            Do While reader.Read
                                If Not reader("activity").ToString = "Tutorial" Then
                                    periodbeforebreaks.Add(cxt2 + 1)
                                Else
                                    cxt2 = cxt2 + 1
                                End If
                            Loop
                            reader.Close()
                            If Session("swapperiod") <> offperiod2 + 1 Or periodbeforebreaks.Contains(Session("swapperiod")) Then
                                lblError.Text = subject2 & " is already offerred on " & Session("swapday") & " and is not a double period."
                                con.close()end using
                                cboDay.Text = Request.QueryString("day")
                                load_time_table()
                                Exit Sub
                            End If
                            cxt2 = Nothing
                        End If

                        offperiod2 = Nothing
                        offcount2 = Nothing
                        offered2 = Nothing
                        clt2 = clt2 + 1
                    Next
                    Dim cmdInsert162 As New MySql.Data.MySqlClient.MySqlCommand("delete from timetable where tname = '" & Request.QueryString("timetable") & "' and day = '" & Session("swapday") & "' and class = '" & Session("swapclass") & "' and period = '" & Session("swapperiod") & "'", con)
                    cmdInsert162.ExecuteNonQuery()
                    Dim dn As Integer
                    For Each item As Integer In groupedsubjects2
                        Dim cmdInsert35 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into timetable (tname, Class, Period, Subject, Doubles, Day, Teacher) Values(?,?,?,?,?,?,?)", con)
                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Request.QueryString("timetable")))
                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("swapclass")))
                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Session("swapperiod")))
                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", item))
                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Doubles", 0))
                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Session("swapday")))
                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", teachers2(dn)))
                        cmdInsert35.ExecuteNonQuery()
                        dn = dn + 1

                    Next
                    Dim cmdInsert163 As New MySql.Data.MySqlClient.MySqlCommand("delete from timetable where tname = '" & Request.QueryString("timetable") & "' and day = '" & Request.QueryString("day") & "' and class = '" & Request.QueryString("class") & "' and period = '" & Request.QueryString("period") & "'", con)
                    cmdInsert163.ExecuteNonQuery()
                    Dim dn2 As Integer
                    For Each item As Integer In groupedsubjects
                        Dim cmdInsert35 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into timetable (tname, Class, Period, Subject, Doubles, Day, Teacher) Values(?,?,?,?,?,?,?)", con)
                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("ss", Request.QueryString("timetable")))
                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", Session("swapclass")))
                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Period", Request.QueryString("period")))
                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", item))
                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Doubles", 0))
                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Day", Request.QueryString("day")))
                        cmdInsert35.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Teacher", teachers(dn2)))
                        cmdInsert35.ExecuteNonQuery()
                        dn2 = dn2 + 1
                    Next
                    lblSuccess.Text = "Swap Successful."
                    Session("swap") = Nothing
                    Session("swapday") = Nothing
                    Session("swapperiod") = Nothing
                    Session("swapclass") = Nothing
                    con.close()end using
                    cboDay.Text = Request.QueryString("day")
                    load_time_table()
                    Exit Sub
                End If
            End If
            cboDay.Text = Request.QueryString("day")
            load_time_table()
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.activity  from tperiods where timetable = '" & Request.QueryString("timetable") & "' and day = '" & Request.QueryString("day") & "'order by tperiods.timestart", con)
                Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
                Dim j As Integer = 1
                Dim period As Integer = Request.QueryString("period")
                Do While student10.Read
                    If student10(0).ToString <> "Tutorial" And j <= period Then
                        period = period + 1
                    End If
                    j = j + 1
                Loop
                student10.Close()

                Dim cmdInsert84 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class where id = '" & Request.QueryString("class") & "'", con)
                Dim reader84 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert84.ExecuteReader
                Dim cla As String
                reader84.Read()
                cla = reader84(0).ToString
                reader84.Close()
                con.close()            End Using
            For Each row As GridViewRow In gridview1.Rows
                If row.Cells(0).Text = cla Then
                    For i As Integer = 1 To gridview1.Columns.Count - 1
                        If i = period Then
                            row.Cells(i).BackColor = Drawing.Color.LightBlue
                            If Session("swap") = Nothing Then
                                Session("swap") = "start"
                                Session("swapperiod") = Request.QueryString("period")
                                Session("swapperiod2") = period
                                Session("swapday") = cboDay.Text
                                Session("swapclass") = Request.QueryString("class")
                                lblSuccess.Text = "Select a subject in " + row.Cells(0).Text + " to swap with"
                            End If
                        End If
                    Next
                End If

            Next
        Else
            Session("swap") = Nothing
            Session("swapday") = Nothing
            Session("swapperiod") = Nothing
            Session("swapperiod2") = Nothing
            Session("swapclass") = Nothing
        End If
        Else

        End If
    End Sub


    Protected Sub cboDay_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboDay.SelectedIndexChanged
        load_time_table()

    End Sub
    Private Sub load_time_table()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cla As String
            If Session("swap") <> Nothing Then
                Dim cmdInsert84 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class from class where id = '" & Session("swapclass") & "'", con)
                Dim reader84 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert84.ExecuteReader

                reader84.Read()
                cla = reader84(0).ToString
                reader84.Close()
            End If
            gridview1.AutoGenerateColumns = False
            gridview1.Width = 90%
            Dim ds As New DataTable
            ds.Columns.Add("Class")
            Dim clas As New BoundField
            clas.HeaderText = "Class"
            clas.DataField = "Class"
            gridview1.Columns.Add(clas)


            Dim classes As New ArrayList
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from class inner join ttname on ttname.school = class.superior where ttname.id = '" & Request.QueryString("timetable") & "' order by class.class", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Do While student0.Read
                ds.Rows.Add(student0(0).ToString)
                classes.Add(student0(0).ToString)
            Loop
            student0.Close()
            Dim cmdLoad10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT tperiods.timestart, tperiods.timeend, tperiods.activity  from tperiods where timetable = '" & Request.QueryString("timetable") & "' and day = '" & cboDay.Text & "'order by tperiods.timestart", con)
            Dim student10 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad10.ExecuteReader
            Dim j As Integer = 1
            Dim nontutorial As New ArrayList
            Do While student10.Read

                If student10(2).ToString = "Tutorial" Then
                    ds.Columns.Add(student10(0).ToString & " - " & student10(1).ToString)
                    Dim hype As New HyperLinkField
                    Dim array() As String = {"edit" & j, "edit" & j + 1}
                    hype.DataTextField = student10(0).ToString & " - " & student10(1).ToString
                    ds.Columns.Add("edit" & j)
                    ds.Columns.Add("edit" & j + 1)
                    hype.DataNavigateUrlFormatString = "~/admin/viewtimetable.aspx?timetable=" + Request.QueryString("timetable") + "&day=" + cboDay.Text + "&period={0}&class={1}"
                    hype.DataNavigateUrlFields = array
                    hype.HeaderText = student10(0).ToString & " - " & student10(1).ToString
                    gridview1.Columns.Add(hype)

                    j = j + 3
                Else
                    ds.Columns.Add(student10(0).ToString & " - " & student10(1).ToString)
                    Dim nonteaching As New BoundField
                    nonteaching.HeaderText = student10(0).ToString & " - " & student10(1).ToString
                    nonteaching.DataField = student10(0).ToString & " - " & student10(1).ToString
                    gridview1.Columns.Add(nonteaching)
                    For Each row As DataRow In ds.Rows

                        row.Item(j) = student10(2).ToString
                    Next
                    nontutorial.Add(j)
                    j = j + 1
                End If
            Loop
            student10.Close()
            Dim c As Integer = 0
            For Each item As String In classes
                Dim enterred As New ArrayList
                Dim cmdLo As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, timetable.teacher, timetable.period, timetable.class from timetable inner join class on timetable.class = class.id inner join subjects on timetable.subject =   subjects.id where class.class = '" & item & "' and timetable.day = '" & cboDay.Text & "' and timetable.tname = '" & Request.QueryString("timetable") & "' order by timetable.period", con)
                Dim studen As MySql.Data.MySqlClient.MySqlDataReader = cmdLo.ExecuteReader
                Dim hx As Integer = 1
                Do While studen.Read
                    If Not nontutorial.Contains(hx) Then
                        If Not enterred.Contains(studen(2).ToString) Then
                            ds(c).Item(hx) = studen(0).ToString & " - " & studen(1).ToString
                            ds(c).Item(hx + 1) = studen(2).ToString
                            ds(c).Item(hx + 2) = studen(3).ToString
                            enterred.Add(studen(2).ToString)
                            hx = hx + 3
                        Else
                            ds(c).Item(hx - 3) = ds(c).Item(hx - 3) & " / " & studen(0).ToString & " - " & studen(1).ToString
                        End If
                    Else
                        If Not enterred.Contains(studen(2).ToString) Then
                            hx = hx + 1
                            ds(c).Item(hx) = studen(0).ToString & " - " & studen(1).ToString
                            ds(c).Item(hx + 1) = studen(2).ToString
                            ds(c).Item(hx + 2) = studen(3).ToString
                            enterred.Add(studen(2).ToString)
                            hx = hx + 3
                        Else
                            ds(c).Item(hx - 3) = ds(c).Item(hx - 3) & " / " & studen(0).ToString & " - " & studen(1).ToString
                        End If

                    End If





                Loop
                studen.Close()
                c = c + 1
                hx = Nothing
                enterred = Nothing
            Next
            gridview1.DataSource = ds
            gridview1.DataBind()
            Panel1.Controls.Add(gridview1)


            If Session("swap") = Nothing Then
                For Each row As GridViewRow In gridview1.Rows
                    For i = 1 To gridview1.Columns.Count - 1
                        row.Cells(i).BackColor = Drawing.Color.Empty
                    Next
                Next
            Else
                If cboDay.Text <> Session("swapday") Then
                    For Each row As GridViewRow In gridview1.Rows
                        For i = 1 To gridview1.Columns.Count - 1
                            row.Cells(i).BackColor = Drawing.Color.Empty
                        Next
                    Next
                Else
                    For Each row As GridViewRow In gridview1.Rows
                        If row.Cells(0).Text = cla Then
                            For i As Integer = 1 To gridview1.Columns.Count - 1
                                If i = Session("swapperiod2") Then
                                    row.Cells(i).BackColor = Drawing.Color.LightBlue
                                End If
                            Next
                        End If
                    Next

                End If
            End If
            con.close()        End Using
    End Sub
End Class
