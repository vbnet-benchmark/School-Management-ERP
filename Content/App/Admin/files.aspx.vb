Option Strict Off
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
        If Not IsPostBack Then
            fill_data()
        End If
    End Sub
    Private Sub fill_data()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()

            Dim ds As New DataTable
            ds.Columns.Add("File No")
            ds.Columns.Add("File Name")
            ds.Columns.Add("Date")
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT receivedfiles.fileno as 'File No', files.filename as 'File Name', cast(receivedfiles.date as char) as Date from receivedfiles inner join files on files.fileno = receivedfiles.fileno inner join secretaries on secretaries.dept = receivedfiles.dept where secretaries.id = '" & Session("username") & "' order by date desc", con)
            Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Dim startdate As Date
            Do While reader1.Read
                ds.Rows.Add(reader1(0).ToString, reader1(1).ToString, Convert.ToDateTime(reader1(2)).ToString("dd/MM/yyyy hh:mm tt"))
                startdate = reader1.Item("date").ToString
            Loop
            reader1.Close()
            DatePicker1.SelectedDate = startdate
            DatePicker2.SelectedDate = Now.Date
            Try
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
                lblError.Text = ex.Message
            End Try
            con.close()        End Using
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        If GridView1.PageIndex + 1 <= GridView1.PageCount Then
            fill_data()
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
        fill_data()
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

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles bnBack.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT receivedfiles.fileno as 'File No', files.filename as 'File Name', receivedfiles.date as Date from receivedfiles inner join files on files.fileno = receivedfiles.fileno inner join secretaries on secretaries.dept = receivedfiles.dept where secretaries.id = '" & Session("username") & "' and (receivedfiles.fileno = ? or files.filename like ?) order by date desc", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("like", txtSearch.Text))
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("2like", "%" & txtSearch.Text & "%"))
            Dim ds As New DataTable
            GridView1.AllowPaging = False
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            Try
                adapter1.SelectCommand = cmdLoad1
                adapter1.Fill(ds)
                GridView1.DataSource = ds
                GridView1.DataBind()
                btnNext.Visible = False
                btnPrevious.Visible = False
            Catch ex As Exception
                lblError.Text = ex.Message
            End Try
            con.close()        End Using
    End Sub

    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        If GridView1.PageIndex - 1 >= 0 Then
            fill_data()
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






    Protected Sub Unnamed1_Click(sender As Object, e As EventArgs) Handles lnkDashBoard.Click
        If Session("usertype") <> Nothing And Session("usertype2") = Nothing Then
            Response.Redirect("~/adminwelcome.aspx")
        ElseIf Session("usertype2") <> Nothing And Session("usertype") = Nothing Then
            Response.Redirect("~/userwelcome.aspx")
        ElseIf Session("usertype") <> Nothing And Session("usertype2") <> Nothing Then
            Response.Redirect("~/useradwelcome.aspx")
        End If
    End Sub

    Protected Sub btnDate_Click(sender As Object, e As EventArgs) Handles btnDate.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim startdate As Date = DatePicker1.SelectedDate
            Dim enddate As Date = DatePicker2.SelectedDate & " 23:59:59"
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT receivedfiles.fileno as 'File No', files.filename as 'File Name', receivedfiles.date as Date from receivedfiles inner join files on files.fileno = receivedfiles.fileno inner join secretaries on secretaries.dept = receivedfiles.dept where secretaries.id = '" & Session("username") & "' and receivedfiles.date >= ? and receivedfiles.date <= ? order by receivedfiles.date desc", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("like", Convert.ToDateTime(startdate).ToString("yyyy-MM-dd HH:mm:ss")))
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("2like", Convert.ToDateTime(enddate).ToString("yyyy-MM-dd HH:mm:ss")))
            Dim ds As New DataTable
            GridView1.AllowPaging = False
            Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
            Try
                adapter1.SelectCommand = cmdLoad1
                adapter1.Fill(ds)
                GridView1.DataSource = ds
                GridView1.DataBind()
                btnNext.Visible = False
                btnPrevious.Visible = False
            Catch ex As Exception
                lblError.Text = ex.Message
            End Try
            con.close()        End Using
    End Sub

    Protected Sub lnkNew_Click(sender As Object, e As EventArgs) Handles lnkNew.Click
        Response.Redirect("~/newfille.aspx")
    End Sub

    Protected Sub LinkButton3_Click(sender As Object, e As EventArgs) Handles LinkButton3.Click
        Response.Redirect("~/receivefile.aspx")
    End Sub
End Class
