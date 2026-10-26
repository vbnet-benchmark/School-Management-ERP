Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_newterm
    Inherits System.Web.UI.Page
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
       If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()

                If Not IsPostBack Then
                    If Request.QueryString.ToString <> Nothing Then
                        Session("AddGrade") = Request.QueryString.ToString
                        Wizard1.ActiveStepIndex = 1



                        Dim a As New DataTable
                        a.Columns.Add("ID")
                        a.Columns.Add("trait")
                        Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, indicator from indicators where pergrp = '" & Session("AddGrade") & "' and used = '" & 1 & "'", con)
                        Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                        Do While msg.Read()
                            a.Rows.Add(msg.Item(0), msg.Item(1))
                        Loop
                        Panel1.Visible = True
                        Gridview1.DataSource = a
                        Gridview1.Columns(0).Visible = False
                        Gridview1.DataBind()
                        msg.Close()
                    End If
                    Dim cmd4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class, pergrp From Class", con)
                    Dim reader4 As MySql.Data.MySqlClient.MySqlDataReader = cmd4.ExecuteReader
                    CheckBoxList2.Items.Clear()
                    Dim x As Integer = 0
                    Do While reader4.Read
                        CheckBoxList2.Items.Add(reader4.Item(0))
                        If reader4.Item(1).ToString = Session("AddGrade") And reader4.Item(1).ToString <> 0 Then
                            CheckBoxList2.Items(x).Selected = True
                        End If
                        x = x + 1


                    Loop
                    reader4.Close()
                Else
                    For Each i As ListItem In CheckBoxList2.Items

                        If i.Selected = True Then
                            checkedSubjectsco.Add(i.Text)
                        Else
                            uncheckedSubjectsco.Add(i.Text)
                        End If
                    Next


                End If
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Wizard1_NextButtonClick1(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.NextButtonClick
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                If Wizard1.ActiveStepIndex = 0 Then
                    If txtID.Text = "" Then
                        Show_Alert(False, "Please enter a name")
                        e.Cancel = True
                        Exit Sub
                    End If
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From pergroups Where grpname = ?", con)
                    cmd2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", txtID.Text))
                    Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                    If reader3.Read() Then
                        Show_Alert(False, "Performance group already exists")
                        reader3.Close()
                        e.Cancel = True
                        Exit Sub
                    Else
                        reader3.Close()
                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into pergroups (grpname, low, high, frequency) Values (?,?,?,?)", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", txtID.Text))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Classf", txtLowest.Text))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Classe", txtHighest.Text))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Cla", cboFrequency.Text))
                        cmd3.ExecuteNonQuery()

                        Dim cmd21 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From pergroups Where grpname = ?", con)
                        cmd21.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subject", txtID.Text))
                        Dim reader31 As MySql.Data.MySqlClient.MySqlDataReader = cmd21.ExecuteReader
                        reader31.Read()
                        Session("AddGrade") = reader31.Item("id")
                        reader31.Close()

                    End If
                    Dim a As New DataTable
                    a.Columns.Add("ID")
                    a.Columns.Add("trait")
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, indicator from indicators where pergrp = '" & Session("AddGrade") & "' and used = '" & 1 & "'", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    Do While msg.Read()
                        a.Rows.Add(msg.Item(0), msg.Item(1))
                    Loop
                    Gridview1.DataSource = a
                    Gridview1.Columns(0).Visible = False
                    Gridview1.DataBind()
                    msg.Close()

                ElseIf Wizard1.ActiveStepIndex = 2 Then


                End If
                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Wizard1_FinishButtonClick(sender As Object, e As WizardNavigationEventArgs) Handles Wizard1.FinishButtonClick
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                For Each item As String In checkedSubjectsco
                    Dim subId As Integer
                    Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Class Where Class = ?", con)
                    cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                    reader20.Read()
                    subId = reader20.Item(0)
                    reader20.Close()


                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update class set pergrp = ? where id = ?", con)
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", Session("AddGrade").ToString))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                    cmd3.ExecuteNonQuery()



                Next

                For Each item2 As String In uncheckedSubjectsco

                    Dim subId2 As Integer

                    Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Class Where Class = ?", con)
                    cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item2))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                    reader20.Read()
                    subId2 = reader20.Item(0)
                    reader20.Close()


                    Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update class set pergrp = ? where id = ? and pergrp = ?", con)
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId2))
                    cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subje", Session("AddGrade")))

                    cmd3.ExecuteNonQuery()


                Next
                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub linkbterm_Click(sender As Object, e As EventArgs) Handles lnkbterm.Click
        Panel1.Visible = True

    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If txtTrait.Text = "" Then
                Show_Alert(False, "Please enter an indicator.")
                Exit Sub
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Insert into indicators (indicator, pergrp) values (?,?)", con)
                cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session2", txtTrait.Text))
                cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("seon2", Session("AddGrade")))
                cmdInsert2.ExecuteNonQuery()
                logify.log(Session("staffid"), txtTrait.Text & " was added as an indicator by admin.")


                txtTrait.Text = ""
                Panel1.Visible = False
                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("trait")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, indicator from indicators where pergrp = '" & Session("AddGrade") & "' and used = '" & 1 & "'", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview1.DataSource = a
                Gridview1.Columns(0).Visible = False
                Gridview1.DataBind()
                msg.Close()
                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles Gridview1.RowCancelingEdit
        Try
            Gridview1.EditIndex = -1
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("trait")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, indicator from indicators  where pergrp = '" & Session("AddGrade") & "' and used = '" & 1 & "'", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview1.DataSource = a
                Gridview1.Columns(0).Visible = False
                Gridview1.DataBind()
                msg.Close()
                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview1.RowDeleting
        Try
            Dim row As GridViewRow = Gridview1.Rows(e.RowIndex)
            Dim id As String = row.Cells(1).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("update indicators set used = '" & 0 & "' where indicator = '" & id & "'", con)
                enter.ExecuteNonQuery()
                Dim cmdLoad03 As New MySql.Data.MySqlClient.MySqlCommand("SELECT id from indicators where indicator = '" & id & "'", con)
                Dim student03 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad03.ExecuteReader
                Dim thistrait As String
                student03.Read()
                thistrait = student03(0)
                student03.Close()
                Dim cmdInsert22a As New MySql.Data.MySqlClient.MySqlCommand("delete from termind where session = '" & Session("sessionid") & "' and indicator = '" & thistrait & "'", con)
                cmdInsert22a.ExecuteNonQuery()
                logify.log(Session("staffid"), id & " was removed as a behavioural trait by admin.")
                Show_Alert(True, "Trait removed")
                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("trait")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, indicator from indicators where used = '" & 1 & "' and pergrp = '" & Session("AddGrade") & "'", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview1.DataSource = a
                Gridview1.Columns(0).Visible = False
                Gridview1.DataBind()
                msg.Close()
                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles Gridview1.RowEditing
        Try
            Gridview1.EditIndex = e.NewEditIndex
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()

                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("trait")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, indicator from indicators where used = '" & 1 & "' and pergrp = '" & Session("AddGrade") & "'", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview1.DataSource = a
                Gridview1.Columns(0).Visible = True
                Gridview1.DataBind()
                msg.Close()
                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Gridview1_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles Gridview1.RowUpdating
        Try
            Dim row As GridViewRow = Gridview1.Rows(e.RowIndex)
            Dim id As String = row.Cells(0).Text
            Dim trait As String = TryCast(row.Cells(1).Controls(0), TextBox).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim cmdLoad03 As New MySql.Data.MySqlClient.MySqlCommand("SELECT indicator from indicators where id = '" & id & "'", con)
                Dim student03 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad03.ExecuteReader
                Dim thistrait As String
                student03.Read()
                thistrait = student03(0)
                student03.Close()
                Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update indicators set indicator = '" & trait & "' where id = '" & id & "'", con)
                enter.ExecuteNonQuery()
                logify.log(Session("staffid"), thistrait & "  indicator was changed to " & trait)
                Show_Alert(True, "Update successful")
                Gridview1.EditIndex = -1
                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("trait")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, indicator from indicators where used = '" & 1 & "' and pergrp = '" & Session("AddGrade") & "'", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview1.DataSource = a
                Gridview1.Columns(0).Visible = False
                Gridview1.DataBind()
                msg.Close()
                con.close()
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
End Class
