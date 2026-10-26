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
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                If Not IsPostBack Then
                    Dim ref As New MySql.Data.MySqlClient.MySqlCommand("Select subject from subjects order by subject", con)
                    Dim readref As MySql.Data.MySqlClient.MySqlDataReader = ref.ExecuteReader
                    cboSubject.Items.Clear()
                    cboSubject.Items.Add("SELECT")
                    Dim subjects As New ArrayList
                    Do While readref.Read
                        If Not subjects.Contains(readref.Item(0).ToString) Then
                            cboSubject.Items.Add(readref.Item(0))
                            subjects.Add(readref.Item(0).ToString)
                        End If
                    Loop
                    readref.Close()


                End If

                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT books.id, books.bookname, books.author, books.publisher, subjects.subject from books inner join subjects on subjects.id = books.subject order by books.author", con)
                Dim ds As New DataTable
                ds.Columns.Add("id")
                ds.Columns.Add("bookname")
                ds.Columns.Add("author")
                ds.Columns.Add("publisher")
                ds.Columns.Add("subject")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ds.Rows.Add(reader(0), reader(1), reader(2), reader(3), reader(4))

                Loop
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

                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT books.id, books.bookname, books.author, books.publisher, subjects.subject from books inner join subjects on subjects.id = books.subject order by books.author", con)
                Dim ds As New DataTable
                ds.Columns.Add("id")
                ds.Columns.Add("bookname")
                ds.Columns.Add("author")
                ds.Columns.Add("publisher")
                ds.Columns.Add("subject")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ds.Rows.Add(reader(0), reader(1), reader(2), reader(3), reader(4))

                Loop
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
                con.close()            End Using


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
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_PageIndexChanged(sender As Object, e As EventArgs) Handles GridView1.PageIndexChanged

    End Sub

    Protected Sub GridView1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles GridView1.PageIndexChanging
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT books.id, books.bookname, books.author, books.publisher, subjects.subject from books inner join subjects on subjects.id = books.subject order by books.author", con)
                Dim ds As New DataTable
                ds.Columns.Add("id")
                ds.Columns.Add("bookname")
                ds.Columns.Add("author")
                ds.Columns.Add("publisher")
                ds.Columns.Add("subject")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ds.Rows.Add(reader(0), reader(1), reader(2), reader(3), reader(4))

                Loop
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
                con.close()            End Using


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
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub GridView1_SelectedIndexChanged(sender As Object, e As EventArgs)

    End Sub

    Protected Sub GridView1_SelectedIndexChanged1(sender As Object, e As EventArgs) Handles GridView1.SelectedIndexChanged

    End Sub

    Protected Sub GridView1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs)

    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT books.id, books.bookname, books.author, books.publisher, subjects.subject from books inner join subjects on subjects.id = books.subject where subjects.subject = ? or books.bookname like ? or books.publisher like ? or books.author like ?  order by books.author", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("likes", "%" & txtSearch.Text & "%"))
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("like", "%" & txtSearch.Text & "%"))
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("likde", "%" & txtSearch.Text & "%"))
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("likge", "%" & txtSearch.Text & "%"))
                Dim ds As New DataTable
                ds.Columns.Add("id")
                ds.Columns.Add("bookname")
                ds.Columns.Add("author")
                ds.Columns.Add("publisher")
                ds.Columns.Add("subject")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ds.Rows.Add(reader(0), reader(1), reader(2), reader(3), reader(4))

                Loop
                GridView1.AllowPaging = False
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
                con.close()            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnPrevious_Click(sender As Object, e As EventArgs) Handles btnPrevious.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT books.id, books.bookname, books.author, books.publisher, subjects.subject from books inner join subjects on subjects.id = books.subject order by books.author", con)
                Dim ds As New DataTable
                ds.Columns.Add("id")
                ds.Columns.Add("bookname")
                ds.Columns.Add("author")
                ds.Columns.Add("publisher")
                ds.Columns.Add("subject")

                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Do While reader.Read
                    ds.Rows.Add(reader(0), reader(1), reader(2), reader(3), reader(4))

                Loop
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
                con.close()            End Using

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
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub btnMsg_Click(sender As Object, e As EventArgs) Handles btnMsg.Click
        Response.Redirect("~/Content/admin/addbooks.aspx")
    End Sub





    Protected Sub cboSubject_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboSubject.SelectedIndexChanged
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT books.id, books.bookname, books.author, books.publisher, subjects.subject from books inner join subjects on subjects.id = books.subject where subjects.subject = '" & cboSubject.Text & "' order by books.author", con)
            Dim ds As New DataTable
            ds.Columns.Add("id")
            ds.Columns.Add("bookname")
            ds.Columns.Add("author")
            ds.Columns.Add("publisher")
            ds.Columns.Add("subject")

            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Do While reader.Read
                ds.Rows.Add(reader(0), reader(1), reader(2), reader(3), reader(4))

            Loop
            GridView1.DataSource = ds
            GridView1.DataBind()
            GridView1.AllowPaging = False
            con.close()        End Using
    End Sub
End Class
