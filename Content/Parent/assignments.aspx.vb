Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Account_Default
    Inherits System.Web.UI.Page
    Dim optionalfeeb As String
    Dim optionalfeef As String
    Dim optionalfeet As String
    Dim optionalfeec As New ArrayList
    Dim optionalfeenc As New ArrayList
    Dim hostel As Boolean
    Dim transport As String
    Dim feeding As String
    Dim minimum As Double
    Dim i As Integer

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
        If check.Check_Parent(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try

       
        If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    panel3.Visible = False

                    Dim ds As New DataTable
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from parentward inner join studentsprofile on parentward.ward = studentsprofile.admno WHERE parentward.parent = ?", con)
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("parentID")))
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                    Do While student0.Read
                        ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                    Loop
                    student0.Close()
                    GridView1.DataSource = ds
                    GridView1.DataBind()
                    con.close()                End Using
                If Request.QueryString.ToString <> Nothing Then
                    Session("studentadd") = Request.QueryString.ToString
                End If
                If Session("studentadd") <> Nothing Then
                    Student_Details()
                End If

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub





    Private Sub Student_Details()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Session("studentid") = Session("studentAdd")
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.surname, class.class, studentsummary.trans, studentsummary.status, studentsummary.class From Studentsummary INNER JOIN Studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.Id WHERE StudentSummary.student = ? And StudentSummary.Session = ?", con)
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("Studentadd")))
            cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("SessionID")))
            Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
            studentsReader.Read()
            lblClass.Text = studentsReader.Item(0).ToString
            Session("ClassId") = studentsReader.Item(4)
            studentsReader.Close()
            Dim ref As New ArrayList
            Dim dates As New ArrayList
            Dim subject As New ArrayList
            Dim title As New ArrayList
            Dim deadline As New ArrayList

            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT assignments.id, cast(assignments.date as char) as date, subjects.subject, assignments.title, assignments.deadline from assignments inner join subjects on subjects.Id = assignments.subject inner join class on class.id = assignments.class where assignments.class = '" & Session("ClassId") & "'  order by assignments.date desc", con)
            Dim ds As New DataTable
            ds.Columns.Add("Reference")
            ds.Columns.Add("Date")
            ds.Columns.Add("Subject")
            ds.Columns.Add("Title")
            ds.Columns.Add("Deadline")
            ds.Columns.Add("status")
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Do While reader.Read
                ref.Add(reader.Item(0))
                dates.Add(Convert.ToDateTime(reader(1)).ToString("dd/MM/yyyy hh:mm tt"))
                subject.Add(reader.Item(2))
                title.Add(reader.Item(3))
                deadline.Add(reader.Item(4))
            Loop
            reader.Close()
            Dim count As Integer = 0
            Dim status As New ArrayList
            For Each item As Integer In ref
                Dim cmdLoad11 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from submittedassignments where assignment = '" & item & "' and student = '" & Session("studentid") & "'", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad11.ExecuteReader
                If reader2.HasRows Then
                    ds.Rows.Add(item, dates(count), subject(count), title(count), deadline(count), "Done")
                Else
                    ds.Rows.Add(item, dates(count), subject(count), title(count), deadline(count), "Undone")


                End If
                reader2.Close()
                count = count + 1
            Next

            GridView2.DataSource = ds
            GridView2.DataBind()
            If GridView2.PageIndex = GridView2.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If GridView2.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If

            con.close()        End Using
        panel3.Visible = True
        pnlAll.Visible = False
    End Sub

    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles GridView1.SelectedIndexChanging
        Try
            Dim x As Array = Split(GridView1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
            Session("studentadd") = RTrim(x(0))
            Student_Details()
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            Student_Details()
            If GridView2.PageIndex + 1 <= GridView2.PageCount Then
                GridView2.PageIndex = GridView2.PageIndex + 1
                GridView2.DataBind()
                If GridView2.PageIndex = GridView2.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView2.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Try
            Student_Details()
            GridView2.PageIndex = e.NewPageIndex
            GridView2.DataBind()

            If GridView2.PageIndex = GridView2.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If GridView2.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Try
            Student_Details()
            If GridView2.PageIndex - 1 >= 0 Then
                GridView2.PageIndex = GridView2.PageIndex - 1
                GridView2.DataBind()
                If GridView2.PageIndex = GridView2.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView2.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If

            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub lnkSubmitted_Click(sender As Object, e As EventArgs) Handles lnkSubmitted.Click
        Response.Redirect("~/content/parent/markedassignments.aspx")

    End Sub
    Protected Sub btnCbt_Click(sender As Object, e As EventArgs) Handles btnCbt.Click
        Response.Redirect("~/content/Student/testlist.aspx")
    End Sub
    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        pnlAll.Visible = True
        panel3.Visible = False
    End Sub
End Class
