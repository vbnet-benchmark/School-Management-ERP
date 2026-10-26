Imports System.Data
Imports System.Data.SqlClient
Imports System.Configuration
Imports System.Web.UI
Imports System.Security
Imports System.Threading

Public Class Score_Board
    Inherits System.Web.UI.Page

    Dim cmd As New SqlCommand
    Dim cmd2 As New SqlCommand
    Dim da As New SqlDataAdapter
    Dim ds As New DataSet
    Dim dr As SqlDataReader
    Dim dr2 As SqlDataReader





    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Private Sub grading()
        Dim sngYearMark, sngExamMark, sngFinalMark As Single
        sngYearMark = Val(txtClass.Text) * 0.3
        sngExamMark = Val(txtExam.Text) * 0.7
        sngFinalMark = sngYearMark + sngExamMark
        lbltot.Text = sngFinalMark
        Select Case sngFinalMark
            Case sngFinalMark = 0 To 44
                lblGrd.Text = "F"
                lblRem.Text = "Fail"
            Case sngFinalMark = 45 To 59
                lblGrd.Text = "E"
                lblRem.Text = "Pass"
            Case sngFinalMark = 60 To 69
                lblGrd.Text = "D"
                lblRem.Text = "Credit"
            Case sngFinalMark = 70 To 79
                lblGrd.Text = "C"
                lblRem.Text = "Good"
            Case sngFinalMark = 80 To 89
                lblGrd.Text = "B"
                lblRem.Text = "Very Good"
            Case sngFinalMark = 90 To 100
                lblGrd.Text = "A"
                lblRem.Text = "Excellent"
            Case Else
                System.Console.WriteLine("Not a valid Student Mark.")
        End Select
    End Sub

    Private Sub CBODisplay()
        Dim ct As String

        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            'Try
            cmd = New SqlCommand("select count (*) from tblEnroll where StudentID='" & DropDownList1.Text & "'", con)

            ct = cmd.ExecuteScalar
            If ct > 0 Then
                cmd = New SqlCommand("select * from tblEnroll where StudentID='" & DropDownList1.Text & "'", con)

                dr = cmd.ExecuteReader()
                dr.Read()
                lblname.Text = dr("Fullname")
                lblHouse.Text = dr("House")
                lblClass.Text = dr("Class")
                lblStudid.Text = dr("StudentID")
                dr.Close()
                Exit Sub


                dr = cmd.ExecuteReader
                dr.Read()

                If dr.HasRows = True Then
                    lblname.Text = dr("Fullname")
                    lblHouse.Text = dr("House")
                    lblClass.Text = dr("Class")
                    lblStudid.Text = dr("StudentID")
                Else
                    Response.Write("Student Number does not exist")
                    dr.Close()

                End If
                dr.Close()
                'dr.Close()

            End If
            cmd.Dispose()
            con.close()        End Using


    End Sub


    Protected Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button2.Click
        CBODisplay()

    End Sub

    Protected Sub txtClass_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtClass.TextChanged
        lbltot.Text = Val(txtClass.Text) + Val(txtExam.Text)

    End Sub

    Protected Sub txtExam_TextChanged(ByVal sender As Object, ByVal e As EventArgs) Handles txtExam.TextChanged

        If lblname.Text = "" Then
            Response.Write("Please Search Student Name and Proceed")
        Else
            lbltot.Text = Val(txtClass.Text) + Val(txtExam.Text)
            grading()

        End If
    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        lblID.Text = DropDownList1.Text
        lblSubject.Text = DropDownList2.Text
        lblScore.Text = txtClass.Text
        lblExm.Text = txtExam.Text
        lblTotal.Text = lbltot.Text
        lblFGrd.Text = lblGrd.Text
        lblFRem.Text = lblRem.Text


        'DropDownList1.SelectedValue = 0
        'DropDownList2.SelectedValue = 0
        txtClass.Text = ""
        txtExam.Text = ""
        lbltot.Text = ""
        lblGrd.Text = ""
        lblRem.Text = ""

    End Sub

    Protected Sub Button3_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button3.Click
        Thread.Sleep(3000)
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()

                Dim cmd As New SqlCommand("insert into tblSubject(StudID,Subject,StaffID,StaffName,ClassScore,Exams,Total,Grade,Remarks,Fullname,Class,Term,Yr,Updt)values('" & lblStudid.Text & "','" & DropDownList2.Text & "','" & Session("StaffID") & "','" & Session("Fullname") & "','" & lblScore.Text & "','" & lblExm.Text & "','" & lblTotal.Text & "','" & lblFGrd.Text & "','" & lblFRem.Text & "','" & lblname.Text & "','" & lblClass.Text & "','" & DropDownList3.Text & "','" & DropDownList4.Text & "','" & Today.Date & "')", con)
                cmd.ExecuteNonQuery()
                cmd.Dispose()
                con.close()            End Using

            lblID.Text = String.Empty
            lblSubject.Text = String.Empty
            lblScore.Text = String.Empty
            lblExm.Text = String.Empty
            lblTotal.Text = String.Empty
            lblFGrd.Text = String.Empty
            lblFRem.Text = String.Empty
            GridView1.DataBind()

        Catch ex As Exception
            Response.Write(ex.Message)
        End Try
    End Sub
End Class
