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


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.admno, studentsprofile.surname, studentsprofile.Sex, studentsprofile.phone from studentsprofile order by surname ", con)
            Dim ds As New DataTable
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            Try
                adapter1.SelectCommand = cmdLoad1
                adapter1.Fill(ds)
                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
                If GridView1.PageCount < 40 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try
            con.close()        End Using

    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If GridView1.PageIndex + 1 <= GridView1.PageCount Then
            GridView1.PageIndex = GridView1.PageIndex + 1
            GridView1.DataBind()
            If GridView1.PageIndex = GridView1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If GridView1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If
        End If
    End Sub

    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging

        GridView1.PageIndex = e.NewPageIndex
        GridView1.DataBind()

        If GridView1.PageIndex = GridView1.PageCount - 1 Then
            btnNext.Visible = False
        Else
            btnNext.Visible = True
        End If
        If GridView1.PageIndex = 0 Then
            btnPrevious.Visible = False
        Else
            btnPrevious.Visible = True
        End If

    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub GridView1_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub GridView1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs)

    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.admno, studentsprofile.surname, studentsprofile.Sex, studentsprofile.phone from studentsprofile where admno = ? or surname like ? order by surname", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Student", txtSearch.Text))
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("like", "%" & txtSearch.Text & "%"))
            Dim ds As New DataTable
            GridView1.AllowPaging = False
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            Try
                adapter1.SelectCommand = cmdLoad1
                adapter1.Fill(ds)
                GridView1.DataSource = ds
                GridView1.DataBind()
                If GridView1.PageIndex = GridView1.PageCount - 1 Then
                    btnNext.Visible = False
                Else
                    btnNext.Visible = True
                End If
                If GridView1.PageIndex = 0 Then
                    btnPrevious.Visible = False
                Else
                    btnPrevious.Visible = True
                End If
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try
            con.close()        End Using
    End Sub

    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If GridView1.PageIndex - 1 >= 0 Then
            GridView1.PageIndex = GridView1.PageIndex - 1
            GridView1.DataBind()
            If GridView1.PageIndex = GridView1.PageCount - 1 Then
                btnNext.Visible = False
            Else
                btnNext.Visible = True
            End If
            If GridView1.PageIndex = 0 Then
                btnPrevious.Visible = False
            Else
                btnPrevious.Visible = True
            End If

        End If

    End Sub

    Protected Sub Unnamed1_Click(sender As Object, e As EventArgs) Handles lnkAdd.Click
        Response.Redirect("~/admin/addstudent.aspx?" & Request.QueryString.ToString)

    End Sub
End Class
