Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Net.Mail
Imports System.Resources

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

        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")

        Try
            If Not IsPostBack Then

                DatePicker2.Text = Now.Date
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                    con.open()
                    Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT item from stock", con)
                    Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                    cboAcc.Items.Clear()
                    cboAcc.Items.Add("All Items")

                    Do While student04.Read
                        cboAcc.Items.Add(student04.Item(0))
                    Loop
                    student04.Close()
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cast(stockadjust.date as char) as date, stock.item, stockadjust.details, stockadjust.added, stockadjust.removed from stockadjust inner join stock on stock.id = stockadjust.item order by date desc", con)
                    Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Dim startdate As Date
                    Dim ds As New DataTable
                    ds.Columns.Add("Date")
                    ds.Columns.Add("item")
                    ds.Columns.Add("details")
                    ds.Columns.Add("added")
                    ds.Columns.Add("removed")
                    Dim ct As Integer = 0
                    Do While reader1.Read
                        startdate = reader1.Item("date")
                        ds.Rows.Add(Convert.ToDateTime(reader1(0)).ToString("dd/MM/yyyy hh:mm tt"), reader1(1), reader1(2), reader1(3), reader1(4))
                        ct = ct + 1
                    Loop
                    reader1.Close()
                    DatePicker1.Text = startdate
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

                    con.close()                End Using
            End If

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles btnNext.Click
        Try
            DatePicker2.Text = Now.Date
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT item from stock", con)
                Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                cboAcc.Items.Clear()
                cboAcc.Items.Add("All items")

                Do While student04.Read
                    cboAcc.Items.Add(student04.Item(0))
                Loop
                student04.Close()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cast(stockadjust.date as char) as date, stock.item, stockadjust.details, stockadjust.added, stockadjust.removed from stockadjust inner join stock on stock.id = stockadjust.item order by date desc", con)
                Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim startdate As Date
                Dim ds As New DataTable
                ds.Columns.Add("Date")
                ds.Columns.Add("item")
                ds.Columns.Add("details")
                ds.Columns.Add("added")
                ds.Columns.Add("removed")
                Dim ct As Integer = 0
                Do While reader1.Read
                    startdate = reader1.Item("date")
                    ds.Rows.Add(Convert.ToDateTime(reader1(0)).ToString("dd/MM/yyyy hh:mm tt"), reader1(1), reader1(2), reader1(3), reader1(4))
                    ct = ct + 1
                Loop
                reader1.Close()
                Datepicker1.Text = startdate
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
            DatePicker2.Text = Now.Date
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT item from stock", con)
                Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                cboAcc.Items.Clear()
                cboAcc.Items.Add("All items")

                Do While student04.Read
                    cboAcc.Items.Add(student04.Item(0))
                Loop
                student04.Close()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cast(stockadjust.date as char) as date, stock.item, stockadjust.details, stockadjust.added, stockadjust.removed from stockadjust inner join stock on stock.id = stockadjust.item order by date desc", con)
                Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim startdate As Date
                Dim ds As New DataTable
                ds.Columns.Add("Date")
                ds.Columns.Add("item")
                ds.Columns.Add("details")
                ds.Columns.Add("added")
                ds.Columns.Add("removed")
                Dim ct As Integer = 0
                Do While reader1.Read
                    startdate = reader1.Item("date")
                    ds.Rows.Add(Convert.ToDateTime(reader1(0)).ToString("dd/MM/yyyy hh:mm tt"), reader1(1), reader1(2), reader1(3), reader1(4))
                    ct = ct + 1
                Loop
                reader1.Close()
                Datepicker1.Text = startdate
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
            DatePicker2.Text = Now.Date
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT item from stock", con)
                Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                cboAcc.Items.Clear()
                cboAcc.Items.Add("All items")

                Do While student04.Read
                    cboAcc.Items.Add(student04.Item(0))
                Loop
                student04.Close()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cast(stockadjust.date as char) as date, stock.item, stockadjust.details, stockadjust.added, stockadjust.removed from stockadjust inner join stock on stock.id = stockadjust.item where stockadjust.details like ? order by date desc", con)
                cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("like", "%" & txtSearch.Text & "%"))
                Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim startdate As Date
                Dim ds As New DataTable
                ds.Columns.Add("Date")
                ds.Columns.Add("item")
                ds.Columns.Add("details")
                ds.Columns.Add("added")
                ds.Columns.Add("removed")
                Dim ct As Integer = 0
                Do While reader1.Read
                    startdate = reader1.Item("date")
                    ds.Rows.Add(Convert.ToDateTime(reader1(0)).ToString("dd/MM/yyyy hh:mm tt"), reader1(1), reader1(2), reader1(3), reader1(4))
                    ct = ct + 1
                Loop
                reader1.Close()
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
            DatePicker2.Text = Now.Date
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad4 As New MySql.Data.MySqlClient.MySqlCommand("SELECT item from stock", con)
                Dim student04 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad4.ExecuteReader
                cboAcc.Items.Clear()
                cboAcc.Items.Add("All items")

                Do While student04.Read
                    cboAcc.Items.Add(student04.Item(0))
                Loop
                student04.Close()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cast(stockadjust.date as char) as date, stock.item, stockadjust.details, stockadjust.added, stockadjust.removed from stockadjust inner join stock on stock.id = stockadjust.item order by date desc", con)
                Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                Dim startdate As Date
                Dim ds As New DataTable
                ds.Columns.Add("Date")
                ds.Columns.Add("item")
                ds.Columns.Add("details")
                ds.Columns.Add("added")
                ds.Columns.Add("removed")
                Dim ct As Integer = 0
                Do While reader1.Read
                    startdate = reader1.Item("date")
                    ds.Rows.Add(Convert.ToDateTime(reader1(0)).ToString("dd/MM/yyyy hh:mm tt"), reader1(1), reader1(2), reader1(3), reader1(4))
                    ct = ct + 1
                Loop
                reader1.Close()
                Datepicker1.Text = startdate
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



    Protected Sub cboAcc_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboAcc.SelectedIndexChanged
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                btnNext.Visible = False
                btnPrevious.Visible = False
                GridView1.AllowPaging = False
                Dim startdate As Date = Datepicker1.Text
                Dim enddate As Date = DatePicker2.Text & " 23:59:59"
                If cboAcc.Text = "All Items" Then
                    DatePicker2.Text = Now.Date
                    lblBal.Visible = False
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cast(stockadjust.date as char) as date, stock.item, stockadjust.details, stockadjust.added, stockadjust.removed from stockadjust inner join stock on stock.id = stockadjust.item order by date desc", con)
                    Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Dim ds As New DataTable
                    ds.Columns.Add("Date")
                    ds.Columns.Add("item")
                    ds.Columns.Add("details")
                    ds.Columns.Add("added")
                    ds.Columns.Add("removed")
                    Dim ct As Integer = 0
                    Do While reader1.Read
                        startdate = reader1.Item("date")
                        ds.Rows.Add(Convert.ToDateTime(reader1(0)).ToString("dd/MM/yyyy hh:mm tt"), reader1(1), reader1(2), reader1(3), reader1(4))
                        ct = ct + 1
                    Loop
                    reader1.Close()
                    GridView1.DataSource = ds
                    GridView1.DataBind()

                Else
                    Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT cast(stockadjust.date as char) as date, stock.item, stockadjust.details, stockadjust.added, stockadjust.removed from stockadjust inner join stock on stock.id = stockadjust.item where stock.item = '" & cboAcc.Text & "'  order by date desc", con)
                    Dim reader1 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
                    Dim ds As New DataTable
                    ds.Columns.Add("Date")
                    ds.Columns.Add("item")
                    ds.Columns.Add("details")
                    ds.Columns.Add("added")
                    ds.Columns.Add("removed")
                    Dim ct As Integer = 0
                    Dim added As Integer = 0
                    Dim removed As Integer = 0
                    Do While reader1.Read
                        startdate = reader1.Item("date")
                        added = added + Val(reader1(3))
                        removed = removed + Val(reader1(4))
                        ds.Rows.Add(Convert.ToDateTime(reader1(0)).ToString("dd/MM/yyyy hh:mm tt"), reader1(1), reader1(2), reader1(3), reader1(4))
                        ct = ct + 1
                    Loop
                    Dim total As Integer = added - removed
                    reader1.Close()
                    GridView1.DataSource = ds

                    lblBal.Text = IIf(total >= 0, "Total Added: " & total, "Total removed: " & -total)

                    GridView1.DataSource = ds
                    GridView1.DataBind()
                End If

                con.close()            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub lnkAdjust_Click(sender As Object, e As EventArgs) Handles lnkAdjust.Click
        Response.Redirect("~/content/admin/stockadjust.aspx")
    End Sub

    Protected Sub lnkManage_Click(sender As Object, e As EventArgs) Handles lnkManage.Click
        Response.Redirect("~/content/admin/stockmnage.aspx")
    End Sub

    Protected Sub lnkInventory_Click(sender As Object, e As EventArgs) Handles lnkInventory.Click
        Response.Redirect("~/content/admin/inventory.aspx")
    End Sub
End Class
