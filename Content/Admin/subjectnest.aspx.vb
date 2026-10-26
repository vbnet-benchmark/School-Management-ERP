Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Partial Class Admin_addsubject
    Inherits System.Web.UI.Page

    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder
    Dim sgroupsubjects As New ArrayList
    Dim nsgroupsubjects As New ArrayList
    Dim sgroupclasses As New ArrayList
    Dim nsgroupclasses As New ArrayList
    Dim cgroupsubjects As New ArrayList
    Dim ncgroupsubjects As New ArrayList
    Dim csgroupsubjects As New ArrayList
    Dim ncsgroupsubjects As New ArrayList
    Dim cgroupclasses As New ArrayList
    Dim ncgroupclasses As New ArrayList
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

        For Each i As ListItem In CheckBoxList2.Items

            If i.Selected = True Then
                sgroupsubjects.Add(i.Text)
            Else
                nsgroupsubjects.Add(i.Text)
            End If
        Next

        For Each i As ListItem In chkClasses.Items

            If i.Selected = True Then
                sgroupclasses.Add(i.Text)
            Else
                nsgroupclasses.Add(i.Text)
            End If
        Next

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
                    Dim cmd4a As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From class", con)
                    Dim reader4a As MySql.Data.MySqlClient.MySqlDataReader = cmd4a.ExecuteReader
                    chkClasses.Items.Clear()
                    Do While reader4a.Read
                        chkClasses.Items.Add(reader4a.Item(1))

                    Loop
                    reader4a.Close()

                    Dim a As New DataTable
                    a.Columns.Add("name")
                    a.Columns.Add("id")
                    Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select name, id from subjectnest", con)
                    Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                    Do While msg.Read()
                        a.Rows.Add(msg.Item(0), msg.Item(1))
                    Loop
                    Gridview2.DataSource = a

                    Gridview2.DataBind()
                    msg.Close()
                    con.Close()
                End Using
            End If
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try

    End Sub
    Protected Sub Gridview2_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles Gridview2.SelectedIndexChanging
        Try

            For Each subject As ListItem In CheckBoxList2.Items
                subject.Selected = False
            Next
            For Each sitem As ListItem In chkClasses.Items
                sitem.Selected = False
            Next
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Panel2.Visible = True
                Dim group As String = Gridview2.Rows(e.NewSelectedIndex).Cells(0).Text
                Session("updatesgroup") = group
                txtGrp.Text = group
                Button1.Text = "Update"
                Dim cmdf As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, class.class from classsubjects inner join subjectnest on subjectnest.id = classsubjects.subjectnest inner join class on classsubjects.class = class.id inner join subjects on classsubjects.subject = subjects.id where subjectnest.name = ?", con)
                cmdf.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Subct", group))
                Dim readerx As MySql.Data.MySqlClient.MySqlDataReader = cmdf.ExecuteReader
                Do While readerx.Read
                    For Each item As ListItem In CheckBoxList2.Items
                        If item.Text = readerx.Item(0).ToString Then item.Selected = True
                    Next
                    For Each sitem As ListItem In chkClasses.Items
                        If sitem.Text = readerx.Item(1).ToString Then sitem.Selected = True
                    Next
                Loop
                readerx.Close()
                Dim a As New DataTable
                a.Columns.Add("name")
                a.Columns.Add("id")

                Dim cmdCheck22 As New MySql.Data.MySqlClient.MySqlCommand("Select name, id from subjectnest", con)
                Dim msg2 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck22.ExecuteReader
                Do While msg2.Read()
                    a.Rows.Add(msg2.Item(0), msg2.Item(1))
                Loop
                Gridview2.DataSource = a

                Gridview2.DataBind()
                msg2.Close()


                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Panel2.Visible = True
        For Each item As ListItem In CheckBoxList2.Items
            item.Selected = False
        Next
        For Each sitem As ListItem In chkClasses.Items
            sitem.Selected = False
        Next
        txtGrp.Text = ""
        Session("newgroup") = True
        Button1.Text = "Add"
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            If txtGrp.Text = "" Then
                Show_Alert(False, "Please enter a group name.")
                Exit Sub
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

                If Session("updatesgroup") <> Nothing And Session("newgroup") = Nothing Then
                    Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Update subjectnest set name = ? Where name = ?", con)
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.Su", txtGrp.Text))
                    cmd2a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.ject", Session("updatesgroup")))
                    cmd2a.ExecuteNonQuery()
                    logify.log(Session("staffid"), txtGrp.Text & " subject nest was updated")
                    Session("updatesgroup") = Nothing
                    Dim sgroup As Integer
                    Dim cmdf0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from subjectnest Where name = ?", con)
                    cmdf0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", txtGrp.Text))
                    Dim readerf0 As MySql.Data.MySqlClient.MySqlDataReader = cmdf0.ExecuteReader
                    readerf0.Read()
                    sgroup = readerf0.Item(0)
                    readerf0.Close()

                   

                    Dim isOfferred As Boolean = False
                    Dim subId As Integer
                    Dim cla As Integer

                    For Each item As String In sgroupsubjects
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        reader20.Read()
                        subId = reader20.Item(0)
                        reader20.Close()
                        For Each subitem As String In sgroupclasses
                            Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                            cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                            Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                            reader2a.Read()
                            cla = reader2a.Item(0)
                            reader2a.Close()

                            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set subjectnest = ? where subject = ? and class = ?", con)
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", sgroup))
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                            cmd3.ExecuteNonQuery()

                        Next
                    Next


                    For Each item As String In nsgroupsubjects
                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        reader20.Read()
                        subId = reader20.Item(0)
                        reader20.Close()
                        For Each subitem As String In sgroupclasses
                            Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                            cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                            Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                            reader2a.Read()
                            cla = reader2a.Item(0)
                            reader2a.Close()
                            Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set subjectnest = ? where subject = ? and subjectnest = ?", con)
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                            cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("te", sgroup))
                            cmd3.ExecuteNonQuery()
                        Next
                    Next
                    
                    For Each clas As String In sgroupclasses

                        Dim cmd1s As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                        cmd1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", clas))
                        Dim reader2s As MySql.Data.MySqlClient.MySqlDataReader = cmd1s.ExecuteReader
                        reader2s.Read()
                        cla = reader2s.Item(0)
                        reader2s.Close()

                        Dim cmd3x As New MySql.Data.MySqlClient.MySqlCommand("select subject from Classsubjects where subjectnest = ? and class = '" & cla & "'", con)
                        cmd3x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("te", sgroup))
                        Dim readerf0x As MySql.Data.MySqlClient.MySqlDataReader = cmd3x.ExecuteReader
                        Dim subjects As New ArrayList
                        Do While readerf0x.Read
                            If Not subjects.Contains(readerf0x(0).ToString) Then subjects.Add(readerf0x(0).ToString)
                        Loop
                        readerf0x.Close()

                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT student from studentsummary Where class = ? and session = '" & Session("sessionid") & "'", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", cla))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        Dim students As New ArrayList
                        Do While reader20.Read()
                            students.Add(reader20.Item(0).ToString)
                        Loop
                        reader20.Close()
                        For Each student As String In students
                            For Each subIds As String In subjects

                                Dim cmd10xa As New MySql.Data.MySqlClient.MySqlCommand("delete from Subjectreg Where Subjectsofferred = ? and session = '" & Session("sessionid") & "' and nested = '" & -Val(False) & "'", con)
                                cmd10xa.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subIds))
                                cmd10xa.ExecuteNonQuery()

                                Dim cmd10xfda As New MySql.Data.MySqlClient.MySqlCommand("delete from Subjectreghalf Where Subjectsofferred = ? and session = '" & Session("sessionid") & "' and nested = '" & -Val(False) & "'", con)
                                cmd10xfda.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReghalf.student", subIds))
                                cmd10xfda.ExecuteNonQuery()
                            Next
                            Dim hasgrp As Boolean = False

                            Dim cmd10sd As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from subjectreg where Subjectsofferred = '" & sgroup & "' and session = '" & Session("sessionid") & "' and nested = '" & -Val(True) & "' and student = '" & student & "'", con)
                            Dim reader20sd As MySql.Data.MySqlClient.MySqlDataReader = cmd10sd.ExecuteReader
                            If reader20sd.Read() Then
                                hasgrp = True
                            End If
                            reader20sd.Close()
                            If hasgrp = False Then
                                Dim cmdds As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred, nested) Values (?,?,?,?,?)", con)
                                cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                                cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                                cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", student))
                                cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", sgroup))
                                cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(True)))
                                cmdds.ExecuteNonQuery()

                                Dim cidds As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into subjectreghalf (Session, Class, Student, SubjectsOfferred, nested) Values (?,?,?,?,?)", con)
                                cidds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                                cidds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                                cidds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", student))
                                cidds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", sgroup))
                                cidds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(True)))
                                cidds.ExecuteNonQuery()
                            End If
                        Next
                        students = Nothing
                    Next
                   
                    For Each clas As String In nsgroupclasses

                        Dim cmd1s As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                        cmd1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", clas))
                        Dim reader2s As MySql.Data.MySqlClient.MySqlDataReader = cmd1s.ExecuteReader
                        reader2s.Read()
                        cla = reader2s.Item(0)
                        reader2s.Close()

                        Dim cmd3x As New MySql.Data.MySqlClient.MySqlCommand("select subject from Classsubjects where subjectnest = ? and class = '" & cla & "'", con)
                        cmd3x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("te", sgroup))
                        Dim readerf0x As MySql.Data.MySqlClient.MySqlDataReader = cmd3x.ExecuteReader
                        Dim subjects2 As New ArrayList
                        Do While readerf0x.Read
                            If Not subjects2.Contains(readerf0x(0).ToString) Then subjects2.Add(readerf0x(0).ToString)
                        Loop
                        readerf0x.Close()

                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT student from studentsummary Where class = ? and session = '" & Session("sessionid") & "'", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", cla))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        Dim students2 As New ArrayList
                        Do While reader20.Read()
                            students2.Add(reader20.Item(0).ToString)
                        Loop
                        reader20.Close()
                        Dim cmd10xa As New MySql.Data.MySqlClient.MySqlCommand("delete from Subjectreg Where Subjectsofferred = ? and class = '" & cla & "' and session = '" & Session("sessionid") & "' and nested = '" & -Val(True) & "'", con)
                        cmd10xa.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", sgroup))
                        cmd10xa.ExecuteNonQuery()

                        Dim cmd10xfda As New MySql.Data.MySqlClient.MySqlCommand("delete from Subjectreghalf Where Subjectsofferred = ? and session = '" & Session("sessionid") & "' and nested = '" & -Val(False) & "'", con)
                        cmd10xfda.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReghalf.student", sgroup))
                        cmd10xfda.ExecuteNonQuery()
                        For Each student As String In students2
                            For Each subIds As String In subjects2
                                Dim hasgrp As Boolean = False
                                Dim cmd10sd As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from subjectreg where Subjectsofferred = '" & subIds & "' and session = '" & Session("sessionid") & "' and nested = '" & -Val(False) & "' and student = '" & student & "'", con)
                                Dim reader20sd As MySql.Data.MySqlClient.MySqlDataReader = cmd10sd.ExecuteReader
                                If reader20sd.Read() Then
                                    hasgrp = True
                                End If
                                reader20sd.Close()
                                If hasgrp = False Then
                                    Dim cmdds As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred, nested) Values (?,?,?,?,?)", con)
                                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", student))
                                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", subIds))
                                    cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(False)))
                                    cmdds.ExecuteNonQuery()

                                    Dim ciddsf As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into subjectreghalf (Session, Class, Student, SubjectsOfferred, nested) Values (?,?,?,?,?)", con)
                                    ciddsf.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                                    ciddsf.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                                    ciddsf.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", student))
                                    ciddsf.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", subIds))
                                    ciddsf.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(False)))
                                    ciddsf.ExecuteNonQuery()
                                End If
                                hasgrp = Nothing
                            Next
                        Next
                        students2 = Nothing
                    Next
                    For Each item As String In nsgroupclasses
                        Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                        cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                        Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                        reader2a.Read()
                        cla = reader2a.Item(0)
                        reader2a.Close()
                        Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set subjectnest = ? where class = ? and subjectnest = ?", con)
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                        cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("te", sgroup))
                        cmd3.ExecuteNonQuery()

                    Next
                    Show_Alert(True, "Subject Nest updated successfully.")
                Else
                    Dim cmdf4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from subjectnest Where name = ?", con)
                    cmdf4.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", txtGrp.Text))
                    Dim readerf4 As MySql.Data.MySqlClient.MySqlDataReader = cmdf4.ExecuteReader
                    If readerf4.HasRows Then
                        Show_Alert(False, "Group exists. Use a different name")
                        Exit Sub
                    End If
                    readerf4.Close()
                    Dim cmd3a As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into subjectnest (name) Values ('" & txtGrp.Text & "')", con)
                    cmd3a.ExecuteNonQuery()
                    logify.log(Session("staffid"), txtGrp.Text & " subject group was added")
                    Dim sgroup As Integer
                    For Each c As String In sgroupclasses
                        Dim cl As Integer
                        Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                        cmd1a.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", c))
                        Dim reader2a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                        reader2a.Read()
                        cl = reader2a.Item(0)
                        reader2a.Close()
                        Dim actual As Integer
                        Dim previous As Integer = Nothing

                        Dim cmdf0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from subjectnest Where name = ?", con)
                        cmdf0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", txtGrp.Text))
                        Dim readerf0 As MySql.Data.MySqlClient.MySqlDataReader = cmdf0.ExecuteReader
                        readerf0.Read()
                        sgroup = readerf0.Item(0)
                        readerf0.Close()
                        Dim isOfferred As Boolean = False
                        Dim subId As Integer
                        Dim cla As Integer
                        For Each item As String In sgroupsubjects
                            Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                            cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                            Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                            reader20.Read()
                            subId = reader20.Item(0)
                            reader20.Close()
                            For Each subitem As String In sgroupclasses
                                Dim cmd1ass As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = '" & subitem & "'", con)

                                Dim reader2as As MySql.Data.MySqlClient.MySqlDataReader = cmd1ass.ExecuteReader
                                reader2as.Read()
                                cla = reader2as.Item(0)
                                reader2as.Close()
                                Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set subjectnest = ? where subject = ? and class = ?", con)
                                cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", sgroup))
                                cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                                cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                                cmd3.ExecuteNonQuery()
                                Dim cmd10x As New MySql.Data.MySqlClient.MySqlCommand("delete from Subjectreg Where Subjectsofferred = ? and session = '" & Session("sessionid") & "' and class = '" & cla & "'", con)
                                cmd10x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subId))
                                cmd10x.ExecuteNonQuery()

                                Dim cmd10xer As New MySql.Data.MySqlClient.MySqlCommand("delete from Subjectreghalf Where Subjectsofferred = ? and session = '" & Session("sessionid") & "' and class = '" & cla & "'", con)
                                cmd10xer.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subId))
                                cmd10xer.ExecuteNonQuery()
                            Next
                        Next




                        For Each item As String In nsgroupsubjects
                            Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from Subjects Where Subject = ?", con)
                            cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", item))
                            Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                            reader20.Read()
                            subId = reader20.Item(0)
                            reader20.Close()
                            For Each subitem As String In sgroupclasses
                                Dim cmd1aa As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                                cmd1aa.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", subitem))
                                Dim reader2aa As MySql.Data.MySqlClient.MySqlDataReader = cmd1aa.ExecuteReader
                                reader2aa.Read()
                                cla = reader2aa.Item(0)
                                reader2aa.Close()
                                Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set subjectnest = ? where subject = ? and class = ? and subjectnest = ?", con)
                                cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
                                cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Subject", subId))
                                cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", cla))
                                cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("te", sgroup))
                                cmd3.ExecuteNonQuery()
                            Next
                        Next
                    Next
                    For Each clas As String In sgroupclasses

                        Dim cmd1s As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from class Where class = ?", con)
                        cmd1s.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", clas))
                        Dim reader2s As MySql.Data.MySqlClient.MySqlDataReader = cmd1s.ExecuteReader
                        reader2s.Read()
                        Dim cla As Integer = reader2s.Item(0)
                        reader2s.Close()

                        Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("SELECT student from studentsummary Where class = ? and session = '" & Session("sessionid") & "'", con)
                        cmd10.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", cla))
                        Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10.ExecuteReader
                        Dim students As New ArrayList
                        Do While reader20.Read()
                            students.Add(reader20.Item(0).ToString)
                        Loop
                        reader20.Close()
                        For Each student As String In students
                            Dim cmdds As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred, nested) Values (?,?,?,?,?)", con)
                            cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                            cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", student))
                            cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", sgroup))
                            cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(True)))
                            cmdds.ExecuteNonQuery()

                            Dim ciddsf1 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReghalf (Session, Class, Student, SubjectsOfferred, nested) Values (?,?,?,?,?)", con)
                            ciddsf1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            ciddsf1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", cla))
                            ciddsf1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", student))
                            ciddsf1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", sgroup))
                            ciddsf1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(True)))
                            ciddsf1.ExecuteNonQuery()
                        Next
                        students = Nothing
                    Next
                    Show_Alert(True, "Subject Nest Added Successfully")
                End If
                Session("newgroup") = Nothing
                Panel2.Visible = False
                Dim a As New DataTable
                a.Columns.Add("name")
                a.Columns.Add("id")

                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select name, id from subjectnest", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview2.DataSource = a

                Gridview2.DataBind()
                msg.Close()
                con.Close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub



    
    Protected Sub Gridview2_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles Gridview2.RowDeleting
        Try
            Dim row As GridViewRow = Gridview2.Rows(e.RowIndex)
            Dim starts As String = row.Cells(0).Text
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim sgroup As Integer
                Dim cmdf0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT ID from subjectnest Where name = ?", con)
                cmdf0.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", starts))
                Dim readerf0 As MySql.Data.MySqlClient.MySqlDataReader = cmdf0.ExecuteReader
                readerf0.Read()
                sgroup = readerf0.Item(0)
                readerf0.Close()
                Dim cmd3x As New MySql.Data.MySqlClient.MySqlCommand("select class, subject from Classsubjects where subjectnest = ?", con)
                cmd3x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("te", sgroup))
                Dim readerf0x As MySql.Data.MySqlClient.MySqlDataReader = cmd3x.ExecuteReader
                Dim classes As New ArrayList
                Dim subjects As New ArrayList
                Do While readerf0x.Read
                    If Not classes.Contains(readerf0x(0).ToString) Then classes.Add(readerf0x(0).ToString)
                    If Not subjects.Contains(readerf0x(1).ToString) Then subjects.Add(readerf0x(1).ToString)
                Loop
                readerf0x.Close()
                Dim cmd3 As New MySql.Data.MySqlClient.MySqlCommand("Update Classsubjects set subjectnest = ? where subjectnest = ?", con)
                cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", ""))
                cmd3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("te", sgroup))
                cmd3.ExecuteNonQuery()
                Dim cmd10x As New MySql.Data.MySqlClient.MySqlCommand("delete from Subjectreg Where Subjectsofferred = ? and session = '" & Session("sessionid") & "' and nested = '" & -Val(True) & "'", con)
                cmd10x.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", sgroup))
                cmd10x.ExecuteNonQuery()

                Dim cmd10xd As New MySql.Data.MySqlClient.MySqlCommand("delete from Subjectreghalf Where Subjectsofferred = ? and session = '" & Session("sessionid") & "' and nested = '" & -Val(True) & "'", con)
                cmd10xd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectRegf.student", sgroup))
                cmd10xd.ExecuteNonQuery()
                For Each clas As String In classes
                    Dim cmd10xs As New MySql.Data.MySqlClient.MySqlCommand("SELECT student from studentsummary Where class = ? and session = '" & Session("sessionid") & "'", con)
                    cmd10xs.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", clas))
                    Dim reader20 As MySql.Data.MySqlClient.MySqlDataReader = cmd10xs.ExecuteReader
                    Dim students As New ArrayList
                    Do While reader20.Read()
                        students.Add(reader20.Item(0).ToString)
                    Loop
                    reader20.Close()
                    For Each student As String In students
                        For Each subid As String In subjects
                            Dim cmdds As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReg (Session, Class, Student, SubjectsOfferred, nested) Values (?,?,?,?,?)", con)
                            cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", clas))
                            cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", student))
                            cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", subid))
                            cmdds.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(False)))
                            cmdds.ExecuteNonQuery()

                            Dim ciddsf1 As New MySql.Data.MySqlClient.MySqlCommand("INSERT Into SubjectReghalf (Session, Class, Student, SubjectsOfferred, nested) Values (?,?,?,?,?)", con)
                            ciddsf1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", Session("SessionID")))
                            ciddsf1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", clas))
                            ciddsf1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", student))
                            ciddsf1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Stdddudent", subid))
                            ciddsf1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Sent", -Val(False)))
                            ciddsf1.ExecuteNonQuery()
                        Next
                    Next
                    students = Nothing
                Next


                Dim cmd10 As New MySql.Data.MySqlClient.MySqlCommand("delete from subjectnest Where name = '" & starts & "'", con)
                cmd10.ExecuteNonQuery()
                logify.log(Session("staffid"), starts & " subject nest was removed")
                Show_Alert(True, "Group deleted successfully")
                Dim a As New DataTable
                a.Columns.Add("name")
                a.Columns.Add("id")

                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select name, id from subjectnest", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview2.DataSource = a

                Gridview2.DataBind()
                msg.Close()
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Panel2.Visible = False
    End Sub
End Class
