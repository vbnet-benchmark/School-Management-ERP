Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_addclass
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjectsco As New ArrayList
    Dim checkedSubjectsop As New ArrayList
    Dim uncheckedSubjectsco As New ArrayList
    Dim uncheckedSubjectsop As New ArrayList

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
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then            Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password"))            If x.Count <> 0 Then                Session("staffid") = x.Item(0)                Session("sessionid") = x.Item(1)            End If        End If
        Try
            If Not IsPostBack Then
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.Open()
                    If Request.QueryString.ToString <> Nothing Then
                        If Not Val(Request.QueryString.ToString) = 0 Then
                            Session("AddGrade") = Request.QueryString.ToString
                        Else

                        End If
                        Dim ds As New DataTable
                        ds.Columns.Add("lowest")
                        ds.Columns.Add("grade")
                        ds.Columns.Add("subject")
                        ds.Columns.Add("average")

                        Dim cmd21 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From grades Where system = ? order by lowest desc", con)
                        cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", Session("AddGrade")))
                        Dim reader31 As MySql.Data.MySqlClient.MySqlDataReader = cmd21.ExecuteReader
                        Dim higest As Double = 100
                        Do While reader31.Read()
                            ds.Rows.Add(reader31.Item("lowest") & " - " & higest, reader31.Item("grade"), reader31.Item("subject"), reader31.Item("average"))
                            higest = Val(reader31.Item("lowest")) - 0.1
                        Loop
                        reader31.Close()
                        Gridview1.DataSource = ds
                        Gridview1.DataBind()
                        Wizard1.ActiveStepIndex = 1
                    End If
                    Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From Class", con)
                    Dim reader4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader
                    CheckBoxList2.Items.Clear()
                    Dim x As Integer = 0
                    Do While reader4.Read
                        CheckBoxList2.Items.Add(reader4.Item(1))
                        If reader4.Item("gradesystem").ToString = Session("AddGrade") And reader4.Item("gradesystem").ToString <> 0 Then
                            CheckBoxList2.Items(x).Selected = True
                        End If
                        x = x + 1


                    Loop
                    reader4.Close()
                    con.Close()                End Using

            Else

                For Each i As ListItem In CheckBoxList2.Items

                    If i.Selected = True Then
                        checkedSubjectsco.Add(i.Text)
                    Else
                        uncheckedSubjectsco.Add(i.Text)
                    End If
                Next
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Wizard1_FinishButtonClick(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.FinishButtonClick
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                For Each item As String In checkedSubjectsco
                    Dim subId As Integer
                    Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Class Where Class = ?", con)
                    cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                    reader20.Read()
                    subId = reader20.Item(0)
                    reader20.Close()


                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update class set gradesystem = ? where id = ?", con)
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("AddGrade").ToString))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                    cmd3.ExecuteNonQuery()

                    Dim cmd100 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, total from subjectreg Where Class = ? and session = ?", con)
                    cmd100.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subId))
                    cmd100.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.stu", Session("sessionid")))
                    Dim subread As MySql.Data.MySqlClient.MySqlDataReader = cmd100.ExecuteReader
                    Dim id As New ArrayList
                    Dim total As New ArrayList
                    Do While subread.Read
                        id.Add(subread.Item(0))
                        total.Add(subread.Item(1))
                    Loop
                    subread.Close()
                    Dim grade As New ArrayList
                    Dim remarks As New ArrayList
                    Dim c As Integer = 0
                    For Each s As Double In total
                        Dim cmd22 As New MySql.Data.MySqlClient.MySqlCommand("SELECT grades.lowest, grades.grade, grades.subject From grades inner join (gradingsystem inner join class on class.gradesystem = gradingsystem.id) on grades.system = gradingsystem.id Where class.id = '" & subId & "' order by grades.lowest desc", con)
                        Dim graderead As MySql.Data.MySqlClient.MySqlDataReader = cmd22.ExecuteReader
                        Do While graderead.Read
                            If s >= Val(graderead.Item(0)) Then
                                grade.Add(graderead.Item(1))
                                remarks.Add(graderead.Item(2))
                                Exit Do
                            End If
                        Loop
                        graderead.Close()
                        Dim cmd3a As New MySql.Data.MySqlClient.MySqlCommand("Update subjectreg set grade = ?, remarks = ?  where id = ?", con)
                        cmd3a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", grade(c)))
                        cmd3a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Cla", remarks(c)))
                        cmd3a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", id(c)))
                        cmd3a.ExecuteNonQuery()
                        c = c + 1
                    Next
                    Dim cmd100a As New MySql.Data.MySqlClient.MySqlCommand("SELECT id, average from studentsummary Where Class = ? and session = ?", con)
                    cmd100a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subId))
                    cmd100a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.stu", Session("sessionid")))
                    Dim subreada As MySql.Data.MySqlClient.MySqlDataReader = cmd100a.ExecuteReader
                    Dim ida As New ArrayList
                    Dim totala As New ArrayList
                    Do While subreada.Read
                        ida.Add(subreada.Item(0))
                        totala.Add(Val(subreada.Item(1)))
                    Loop
                    subreada.Close()
                    Dim remarksp As New ArrayList
                    Dim x As Integer = 0
                    For Each s As Double In totala
                        Dim cmd22 As New MySql.Data.MySqlClient.MySqlCommand("SELECT grades.lowest, grades.average From grades inner join (gradingsystem inner join class on class.gradesystem = gradingsystem.id) on grades.system = gradingsystem.id Where class.id = '" & subId & "' order by grades.lowest desc", con)
                        Dim graderead As MySql.Data.MySqlClient.MySqlDataReader = cmd22.ExecuteReader
                        Do While graderead.Read
                            If s >= Val(graderead.Item(0)) Then
                                remarksp.Add(graderead.Item(1).ToString)
                                Exit Do
                            End If
                        Loop
                        graderead.Close()
                        Dim cmd3a As New MySql.Data.MySqlClient.MySqlCommand("Update studentsummary set principalremarks = ?  where id = ?", con)
                        cmd3a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Cla", remarksp(x)))
                        cmd3a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", ida(x)))
                        cmd3a.ExecuteNonQuery()
                        x = x + 1
                    Next

                Next

                For Each item2 As String In uncheckedSubjectsco

                    Dim subId2 As Integer

                    Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Class Where Class = ?", con)
                    cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item2))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                    reader20.Read()
                    subId2 = reader20.Item(0)
                    reader20.Close()


                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update class set gradesystem = ? where id = ? and gradesystem = ?", con)
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId2))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subje", Session("addgrade")))

                    cmd3.ExecuteNonQuery()
                Next
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Wizard1_NextButtonClick(sender As Object, e As WizardNavigationEventArgs)

    End Sub


    Protected Sub Wizard1_NextButtonClick1(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.NextButtonClick
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If Wizard1.ActiveStepIndex = 0 Then
                    If txtID.Text = "" Then
                        Show_Alert(False, "Please enter a name")
                        e.Cancel = True
                        Exit Sub
                    End If
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From gradingsystem Where system = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", txtID.Text))
                    Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                    If reader3.Read() Then
                        Show_Alert(False, "Grading system already exists")
                        reader3.Close()
                        e.Cancel = True
                        Exit Sub
                    Else
                        reader3.Close()
                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into gradingsystem (system) Values (?)", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", txtID.Text))
                        cmd3.ExecuteNonQuery()

                        Dim cmd21 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From gradingsystem Where system = ?", con)
                        cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", txtID.Text))
                        Dim reader31 As MySql.Data.MySqlClient.MySqlDataReader = cmd21.ExecuteReader
                        reader31.Read()
                        Session("AddGrade") = reader31.Item(0)
                        reader31.Close()
                    End If


                ElseIf Wizard1.ActiveStepIndex = 2 Then


                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles Gridview1.RowCancelingEdit
        Gridview1.EditIndex = -1

    End Sub






    Protected Sub bnAdd_Click(sender As Object, e As EventArgs) Handles bnAdd.Click
        If txtGrade.Text = "" Or txtLowest.Text = "" Or txtRemarks.Text = "" Or txtARemarks.Text = "" Then
            Show_Alert(False, "Please fill all fields.")
            Exit Sub
        End If
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT gradingsystem.system From grades inner join gradingsystem on grades.system = gradingsystem.id Where grades.system = ? and grades.grade = ?", con)
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", Session("AddGrade")))
                cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subjec", txtGrade.Text))

                Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                Dim exists As Boolean = False
                Dim system As String = ""
                If reader1.Read Then
                    exists = True
                    system = reader1(0).ToString
                End If
                reader1.Close()
                If exists = True Then
                    Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Update grades set lowest = ?, subject = ?, average = ? Where system = ? and grade = ?", con)
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Sub", txtLowest.Text))
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Su", txtRemarks.Text))
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.S", txtARemarks.Text))
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", Session("AddGrade")))
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subjec", txtGrade.Text))
                    cmd2a.ExecuteNonQuery()
                    logify.log(Session("staffid"), "Grades were modifed in grading system " & system & " by admin.")
                    Show_Alert(True, "Grade updated successfully.")
                Else
                    Dim cmd20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT system From gradingsystem Where id = ?", con)
                    cmd20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", Session("AddGrade")))
                    Dim reader10 As MySql.Data.MySqlClient.MySqlDataReader = cmd20.ExecuteReader
                    reader10.Read()
                    system = reader10(0).ToString
                    reader10.Close()
                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into grades (system, lowest, grade, subject, average) Values ('" & Session("Addgrade") & "', '" & Val(txtLowest.Text) & "', '" & txtGrade.Text & "', '" & txtRemarks.Text & "', '" & txtARemarks.Text & "')", con)
                    cmd3.ExecuteNonQuery()
                    logify.log(Session("staffid"), "Grades were added to the grading system " & system & " by admin.")
                    Show_Alert(True, "Grade Added Successfully")
                End If

                Dim ds As New DataTable
                ds.Columns.Add("lowest")
                ds.Columns.Add("grade")
                ds.Columns.Add("subject")
                ds.Columns.Add("average")

                Dim cmd21 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From grades Where system = ? order by lowest desc", con)
                cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", Session("AddGrade")))
                Dim reader31 As MySql.Data.MySqlClient.MySqlDataReader = cmd21.ExecuteReader
                Dim higest As Double = 100
                Do While reader31.Read()
                    ds.Rows.Add(reader31.Item("lowest") & " - " & higest, reader31.Item("grade"), reader31.Item("subject"), reader31.Item("average"))
                    higest = Val(reader31.Item("lowest")) - 0.1
                Loop
                reader31.Close()
                Gridview1.DataSource = ds
                Gridview1.DataBind()
                panel1.Visible = False


                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub lnkNew_Click(sender As Object, e As EventArgs) Handles lnkNew.Click
        panel1.Visible = True
        txtRemarks.Text = ""
        txtGrade.Text = ""
        txtLowest.Text = ""
        txtARemarks.Text = ""
        bnAdd.Text = "Add"
    End Sub

    Protected Sub Gridview1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview1.RowDeleting
        Dim row As GridViewRow = Gridview1.Rows(e.RowIndex)
        Dim account As String = row.Cells(1).Text
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim system As String
                Dim cmd20 As New MySql.Data.MySqlClient.MySqlCommand("SELECT system From gradingsystem Where id = ?", con)
                cmd20.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", Session("AddGrade")))
                Dim reader10 As MySql.Data.MySqlClient.MySqlDataReader = cmd20.ExecuteReader
                reader10.Read()
                System = reader10(0).ToString
                reader10.Close()
                Dim delete As New MySql.Data.MySqlClient.MySqlCommand("Delete From grades where grade = '" & account & "' and system = '" & Session("AddGrade") & "'", con)
                delete.ExecuteNonQuery()
                logify.log(Session("staffid"), "Grades were removed from the grading system " & system & " by admin.")
                Show_Alert(True, "Grade removed")
                Dim ds As New DataTable
                ds.Columns.Add("lowest")
                ds.Columns.Add("grade")
                ds.Columns.Add("subject")
                ds.Columns.Add("average")

                Dim cmd21 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From grades Where system = ? order by lowest desc", con)
                cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", Session("AddGrade")))
                Dim reader31 As MySql.Data.MySqlClient.MySqlDataReader = cmd21.ExecuteReader
                Dim higest As Double = 100
                Do While reader31.Read()
                    ds.Rows.Add(reader31.Item("lowest") & " - " & higest, reader31.Item("grade"), reader31.Item("subject"), reader31.Item("average"))
                    higest = Val(reader31.Item("lowest")) - 0.1
                Loop
                reader31.Close()
                Gridview1.DataSource = ds
                Gridview1.DataBind()
                panel1.Visible = False
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles Gridview1.SelectedIndexChanging
        Dim grade As String = Gridview1.Rows(e.NewSelectedIndex).Cells(1).Text
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            panel1.Visible = True
            Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From grades Where system = ? and grade = ?", con)
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", Session("AddGrade")))
            cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Sub", grade))

            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
            reader3.Read()
            txtGrade.Text = reader3.Item("grade")
            txtLowest.Text = reader3.Item("lowest")
            txtRemarks.Text = reader3.Item("subject")
            txtARemarks.Text = reader3.Item("average")
            reader3.Close()
            bnAdd.Text = "Update"
            con.close()        End Using



    End Sub
End Class
