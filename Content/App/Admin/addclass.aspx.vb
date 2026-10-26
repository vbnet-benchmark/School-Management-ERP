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
                    Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From Subjects", con)
                    Dim reader4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader
                    CheckBoxList2.Items.Clear()
                    Do While reader4.Read
                        CheckBoxList2.Items.Add(reader4.Item(1))
                    Loop
                    reader4.Close()

                    Dim cmd5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From depts", con)
                    Dim reader5 As MySql.Data.MySqlClient.MySqlDataReader = cmd5.ExecuteReader
                    cboDept.Items.Clear()
                    cboDept.Items.Add("Select Department")
                    Do While reader5.Read
                        cboDept.Items.Add(reader5.Item(1))
                    Loop
                    reader5.Close()
                    con.Close()                End Using
            Else

                For Each i As ListItem In CheckBoxList3.Items

                    If i.Selected = True Then
                        checkedSubjectsop.Add(i.Text)
                    Else
                        uncheckedSubjectsop.Add(i.Text)
                    End If
                Next

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

    End Sub

    Protected Sub Wizard1_NextButtonClick(sender As Object, e As WizardNavigationEventArgs)

    End Sub


    Protected Sub Wizard1_NextButtonClick1(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.NextButtonClick

        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If Wizard1.ActiveStepIndex = 0 Then
                    If txtID.Text = "" Then
                        Show_Alert(False, "Please enter a class name")
                        e.Cancel = True
                        Exit Sub
                    End If
                    If cbotype.Text = "Select Type" Then
                        Show_Alert(False, "Please select a class type")
                        e.Cancel = True
                        Exit Sub
                    End If
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From Class Where class = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", txtID.Text))
                    Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                    If reader3.Read() Then
                        Show_Alert(False, "Class already exists")
                        reader3.Close()
                        e.Cancel = True
                        Exit Sub
                    Else
                        reader3.Close()
                        Dim cmd5 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From depts where dept = '" & cboDept.Text & "'", con)
                        Dim reader5 As MySql.Data.MySqlClient.MySqlDataReader = cmd5.ExecuteReader
                        If Not reader5.Read() Then
                            reader5.Close()
                            Show_Alert(False, "Please Select A Valid Department or create one.")
                            e.Cancel = True
                            Exit Sub
                        Else
                            Dim dept As Integer = reader5.Item(0)
                            reader5.Close()
                            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into Class (Class, type, superior, cano) Values (?,?,?,?)", con)
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", txtID.Text))
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cbotype.Text))
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dept", dept))
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dt", cboCAno.Text))
                            cmd3.ExecuteNonQuery()
                            logify.log(Session("staffid"), txtId.Text & " class was created.")
                            Dim reader33 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader

                            reader33.Read()
                            Session("Addclass") = reader33.Item(0)
                            Session("classname") = txtID.Text
                            reader33.Close()
                        End If
                    End If

                ElseIf Wizard1.ActiveStepIndex = 1 Then
                    Dim isOfferred As Boolean = False
                    Dim subId As Integer
                    For Each item As String In checkedSubjectsco
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        reader20.Read()
                        subId = reader20.Item(0)
                        reader20.Close()

                        Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From Classsubjects Where class = ? and subject= ?", con)
                        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("AddClass").ToString))
                        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", subId))
                        Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                        If reader3.Read Then
                            isOfferred = True

                        End If
                        reader3.Close()

                        If isOfferred = False Then
                            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into Classsubjects (Class, subject, type) Values (?,?,?)", con)
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("AddClass").ToString))
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Compulsory"))
                            cmd3.ExecuteNonQuery()
                            logify.log(Session("staffid"), item & " was added as a compulsory subject in " & txtId.Text)
                        End If
                    Next

                    For Each item As String In uncheckedSubjectsco

                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        reader20.Read()
                        subId = reader20.Item(0)
                        reader20.Close()

                        Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From Classsubjects Where class = ? and subject = ?", con)
                        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("AddClass").ToString))
                        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", subId))
                        Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                        If reader3.Read Then
                            isOfferred = True
                        End If
                        reader3.Close()
                        If isOfferred = True Then
                            Dim cmdDelete1 As New MySql.Data.MySqlClient.MySqlCommand("Delete From Classsubjects Where class = ? And subject = ?", con)
                            cmdDelete1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("Addclass").ToString))
                            cmdDelete1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectsOfferred", subId))
                            cmdDelete1.ExecuteNonQuery()
                            logify.log(Session("staffid"), item & " was removed as a compulsory subject in " & txtId.Text)
                        End If
                    Next
                    CheckBoxList3.Items.Clear()
                    For Each item As String In uncheckedSubjectsco
                        CheckBoxList3.Items.Add(item)
                    Next
                ElseIf Wizard1.ActiveStepIndex = 2 Then
                    Dim isOfferred As Boolean = False
                    Dim subId As Integer
                    For Each item As String In checkedSubjectsop
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        reader20.Read()
                        subId = reader20.Item(0)
                        reader20.Close()

                        Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From Classsubjects Where class = ? and subject= ?", con)
                        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("AddClass").ToString))
                        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", subId))
                        Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                        If reader3.Read Then
                            isOfferred = True
                        End If
                        reader3.Close()
                        If isOfferred = False Then
                            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into Classsubjects (Class, subject, type) Values (?,?,?)", con)
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("AddClass").ToString))
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Optional"))
                            cmd3.ExecuteNonQuery()
                            logify.log(Session("staffid"), item & " was added as an optional subject in " & txtId.Text)
                        End If
                    Next



                    For Each item As String In uncheckedSubjectsop
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        reader20.Read()
                        subId = reader20.Item(0)
                        reader20.Close()

                        Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From Classsubjects Where class = ? and subject = ?", con)
                        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("AddClass").ToString))
                        cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", subId))
                        Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                        If reader3.Read Then
                            isOfferred = True
                        End If
                        reader3.Close()

                        If isOfferred = True Then
                            Dim cmdDelete1 As New MySql.Data.MySqlClient.MySqlCommand("Delete From Classsubjects Where class = ? And subject = ?", con)
                            cmdDelete1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("AddClass").ToString))
                            cmdDelete1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectsOfferred", subId))
                            cmdDelete1.ExecuteNonQuery()
                            logify.log(Session("staffid"), item & " was removed as a compulsory subject in " & txtId.Text)
                        End If
                    Next

                    Dim a As New DataTable
                    a.Columns.Add("Subject")
                    a.Columns.Add("periods")
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject, classsubjects.periods from classsubjects inner join subjects on classsubjects.subject = subjects.Id where classsubjects.class = '" & Session("AddClass") & "'", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    Do While msg.Read()
                        a.Rows.Add(msg.Item(0), msg.Item(1))
                    Loop
                    Gridview1.DataSource = a

                    Gridview1.DataBind()
                    msg.Close()
                End If
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles Gridview1.RowCancelingEdit
        Gridview1.EditIndex = -1

    End Sub

    Protected Sub Gridview1_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles Gridview1.RowEditing
        Gridview1.EditIndex = e.NewEditIndex
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim a As New DataTable
                a.Columns.Add("Subject")
                a.Columns.Add("periods")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject, classsubjects.periods from classsubjects inner join subjects on classsubjects.subject = subjects.Id where classsubjects.class = '" & Session("AddClass") & "'", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview1.DataSource = a

                Gridview1.DataBind()
                msg.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview1.RowUpdating
        Try
            Dim row As GridViewRow = Gridview1.Rows(e.RowIndex)
            Dim ID As String = row.Cells(0).Text
            Dim sessions As Integer = Val(TryCast(row.Cells(1).Controls(0), TextBox).Text)
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Replace(ID, "amp;", "")))
                Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                reader20.Read()
                Dim subId As Integer = reader20.Item(0)
                reader20.Close()

                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update classsubjects set periods = '" & sessions & "' where subject = '" & subId & "' and class = '" & Session("AddClass") & "'", con)
                enter.ExecuteNonQuery()
                logify.log(Session("staffid"), ID & " periods was updated to " & sessions & " in " & txtId.Text)
                Show_Alert(True, "Update successful")
                Gridview1.EditIndex = -1
                Dim a As New DataTable
                a.Columns.Add("Subject")
                a.Columns.Add("periods")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select subjects.subject, classsubjects.periods from classsubjects inner join subjects on classsubjects.subject = subjects.Id where classsubjects.class = '" & Session("AddClass") & "'", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview1.DataSource = a

                Gridview1.DataBind()
                msg.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

   
End Class
