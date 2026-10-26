Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_studentprofile
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

       
        If IsPostBack Then
        Else
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
                    DropdownList1.Items.Clear()
                    Dim clasadd As New ArrayList
                    For Each item As String In classcontroled
                        Dim classes As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'", con)
                        Dim schclass As MySql.Data.MySqlClient.MySqlDataReader = classes.ExecuteReader

                        Do While schclass.Read
                            If Not clasadd.Contains(schclass(0)) Then
                                clasadd.Add(schclass(0))
                                DropdownList1.Items.Add(schclass.Item(0).ToString)
                            End If
                        Loop
                        schclass.Close()
                    Next
                    If Session("classselect") <> Nothing Then DropdownList1.Text = Session("classselect")
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = ?", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", DropdownList1.Text))
                    Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    student1.Read()
                    Session("classid") = student1.Item(0).ToString
                    student1.Close()
                    con.close()                End Using
                Student_Details()
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Student_Details()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class as Class, studentsummary.classNo as 'No of Students', class.type as 'Type', depts.dept as 'Department' from class inner join depts on class.superior = depts.id left join studentsummary on class.id = studentsummary.class where class.class = '" & DropdownList1.Text & "' order by studentsummary.session desc", con)
            Dim ds As New DataTable
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            adapter1.SelectCommand = cmdLoad1
            adapter1.Fill(ds)
            DetailsView1.DataSource = ds
            DetailsView1.DataBind()
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname, staffprofile.phone, staffprofile.passport from classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid inner join class on classteacher.class = class.id where class.class = '" & DropdownList1.Text & "'", con)
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
            Dim ds2 As New DataTable
            ds2.Columns.Add("s/n")
            ds2.Columns.Add("Name")
            ds2.Columns.Add("phone")
            ds2.Columns.Add("passport")

            Dim i As Integer = 1
            Do While reader.Read
                ds2.Rows.Add(i.ToString & ".  ", reader.Item(0), reader.Item(1), reader.Item(2))
                i = i + 1
            Loop
            GridView1.DataSource = ds2
            GridView1.DataBind()
            reader.Close()

            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.staffid, staffprofile.surname, subjects.subject, staffprofile.phone, staffprofile.passport from classsubjects left join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on subjects.id = classsubjects.subject inner join class on class.id = classsubjects.class where class.class = '" & DropdownList1.Text & "'", con)
            Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            Dim ds3 As New DataTable
            ds3.Columns.Add("s/n")
            ds3.Columns.Add("Name")
            ds3.Columns.Add("Subject")
            ds3.Columns.Add("phone")
            ds3.Columns.Add("passport")

            Dim j As Integer = 1
            Do While reader0.Read
                ds3.Rows.Add(j.ToString & ".    ", reader0.Item(1), reader0.Item(2), reader0.Item(3), reader0.Item(4))
                j = j + 1
            Loop
            GridView2.DataSource = ds3
            GridView2.DataBind()
            reader.Close()
            con.Close()
        End Using
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

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from class where class = ?", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", DropdownList1.Text))
                Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                student1.Read()
                Session("classid") = student1.Item(0).ToString
                student1.Close()
                Session("classselect") = DropdownList1.Text
                con.close()            End Using
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkTimeTable_Click(sender As Object, e As EventArgs) Handles lnkTimeTable.Click
        Try
            Dim exists As Boolean = True
            Dim id As Integer
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT ttname.id, class.class from ttname inner join class on class.id = ttname.class where ttname.default = '" & 1 & "' and class.class = ?", con)
                cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", DropdownList1.Text))
                Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                If student.Read() Then
                    Session("classselect") = student(1).ToString
                    id = student(0)
                    student.Close()
                Else
                    student.Close()
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ttname.id from ttname inner join (depts inner join class on class.superior = depts.id) on ttname.school = depts.id where ttname.default = '" & 1 & "' and class.class = ?", con)
                    cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", DropdownList1.Text))
                    Dim student1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    If student1.Read() Then
                        Session("classselect") = DropdownList1.Text
                        id = student1(0)
                        student1.Close()
                    Else
                        exists = False

                    End If
                End If
                con.Close()
            End Using
            If exists = True Then
                Response.Redirect("~/content/staff/sviewtimetable.aspx?timetable=" & id)

            Else
                Show_Alert(False, "Time Table not active yet")
            End If

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkStudents_Click(sender As Object, e As EventArgs) Handles lnkStudents.Click
        Session("classselect") = DropdownList1.Text
        Session("studentadd") = Nothing
        Response.Redirect("~/content/staff/sstudentprofile.aspx")
    End Sub
End Class
