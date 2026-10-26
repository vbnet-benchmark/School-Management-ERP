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
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If

        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.Open()
                If Not IsPostBack Then
                    Session("NSessionId") = Session("SessionId")
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
                    If Session("classub") <> Nothing Then
                        Dim refs As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject from classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on classsubjects.subject = subjects.ID where staffprofile.surname = '" & DropDownList1.Text & "'", con)
                        Dim readrefs As MySql.Data.MySqlClient.MySqlDataReader = refs.ExecuteReader
                        cboSubject.Items.Clear()
                        Dim subj As New ArrayList
                        cboSubject.Items.Add("SELECT")
                        Do While readrefs.Read
                            If Not subj.Contains(readrefs(0)) Then
                                cboSubject.Items.Add(readrefs.Item(0))
                                subj.Add(readrefs(0))
                            End If
                        Loop
                        readrefs.Close()
                        Dim cl As String
                        Dim cmdCheck200 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, class.class FROM classsubjects inner join subjects on subjects.id = classsubjects.subject inner join class on class.id = classsubjects.class where classsubjects.id = '" & Session("classub") & "'", con)
                        Dim reader200 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck200.ExecuteReader()
                        If reader200.Read() Then
                            cboSubject.Text = reader200.Item(0)
                            cl = reader200.Item(1).ToString
                        End If
                        reader200.Close()


                        Dim cmdLoad0d As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id where subjects.subject = '" & cboSubject.Text & "'", con)
                        Dim student0d As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0d.ExecuteReader
                        cboClass.Items.Clear()
                        Do While student0d.Read
                            cboClass.Items.Add(student0d.Item(0))
                        Loop
                        student0d.Close()
                        cboClass.Text = cl
                        Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where subjects.subject = '" & cboSubject.Text & "' and class.class = '" & cboClass.Text & "' and courseoutline.session = '" & Session("NSessionId") & "' order by courseoutline.week asc", con)
                        Dim ds As New DataTable
                        ds.Columns.Add("Week")
                        ds.Columns.Add("Topic")
                        ds.Columns.Add("Content")

                        Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                        Do While reader.Read
                            ds.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2))
                        Loop
                        reader.Close()
                        GridView1.DataSource = ds
                        GridView1.DataBind()


                    Else
                        Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject from classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on classsubjects.subject = subjects.ID where staffprofile.surname = '" & DropDownList1.Text & "'", con)
                        Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                        cboSubject.Items.Clear()
                        Dim subj As New ArrayList
                        cboSubject.Items.Add("SELECT")
                        Do While readref.Read
                            If Not subj.Contains(readref(0)) Then
                                cboSubject.Items.Add(readref.Item(0))
                                subj.Add(readref(0))
                            End If
                        Loop
                        readref.Close()
                    End If




                End If
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging

        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataBind()



    End Sub









    Protected Sub cboSubject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubject.SelectedIndexChanged

        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select class.class from classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id where subjects.subject = '" & cboSubject.Text & "' and staffprofile.surname = '" & DropDownList1.Text & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                cboClass.Items.Clear()
                cboClass.Items.Add("SELECT")
                Do While student0.Read
                    cboClass.Items.Add(student0.Item(0))
                Loop
                student0.Close()
                GridView1.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub cboClass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboClass.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT week, topic, content from courseoutline inner join subjects on subjects.id = courseoutline.subject inner join session on session.id = courseoutline.session inner join class on class.id = courseoutline.class where subjects.subject = '" & cboSubject.Text & "' and class.class = '" & cboClass.Text & "' and courseoutline.session = '" & Session("NSessionId") & "' order by courseoutline.week asc", con)
                Dim ds As New DataTable
                ds.Columns.Add("Week")
                ds.Columns.Add("Topic")
                ds.Columns.Add("Content")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ds.Rows.Add(reader.Item(0), reader.Item(1), reader.Item(2))
                Loop
                reader.Close()

                GridView1.DataSource = ds
                GridView1.DataBind()

                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("Select classsubjects.id from classsubjects inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.Id where subjects.subject = '" & cboSubject.Text & "' and class.class = '" & cboClass.Text & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                If student0.Read() Then Session("classub") = student0.Item(0)
                student0.Close()
                con.close()            End Using
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


    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject from classsubjects inner join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on classsubjects.subject = subjects.ID where staffprofile.surname = '" & DropDownList1.Text & "'", con)
                Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                cboSubject.Items.Clear()
                Dim subj As New ArrayList
                cboSubject.Items.Add("SELECT")
                Do While readref.Read
                    If Not subj.Contains(readref(0)) Then
                        cboSubject.Items.Add(readref.Item(0))
                        subj.Add(readref(0))
                    End If
                Loop
                readref.Close()
                cboClass.Items.Clear()
                GridView1.DataBind()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
