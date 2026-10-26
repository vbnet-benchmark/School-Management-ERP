Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_newsession
    Inherits System.Web.UI.Page




    Protected Sub GridView1_RowCancelingEdit(sender As Object, e As GridViewCancelEditEventArgs) Handles GridView1.RowCancelingEdit
        GridView1.EditIndex = -1
    End Sub

    Protected Sub GridView1_RowEditing(sender As Object, e As GridViewEditEventArgs) Handles GridView1.RowEditing
        GridView1.EditIndex = e.NewEditIndex
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim a As New DataTable
            a.Columns.Add("ID")
            a.Columns.Add("Class")
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, class from class order by ID", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Do While msg.Read()
                a.Rows.Add(msg.Item(0), msg.Item(1))
            Loop
            GridView1.DataSource = a

            GridView1.DataBind()
            msg.Close()
            con.close()        End Using
    End Sub

    Protected Sub GridView1_RowUpdating(sender As Object, e As GridViewUpdateEventArgs) Handles GridView1.RowUpdating
        Dim row As GridViewRow = Gridview1.Rows(e.RowIndex)
        Dim ID As String = row.Cells(0).Text
        Dim sessions As String = TryCast(row.Cells(1).Controls(0), TextBox).Text

        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim enter As New MySql.Data.MySqlClient.MySqlCommand("Update class set class = '" & sessions & "' where ID = '" & ID & "'", con)
            enter.ExecuteNonQuery()
            GridView1.EditIndex = -1
            Dim a As New DataTable
            a.Columns.Add("ID")
            a.Columns.Add("Class")
            Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, class from class order by ID", con)
            Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
            Do While msg.Read()
                a.Rows.Add(msg.Item(0), msg.Item(1))
            Loop
            Gridview1.DataSource = a

            Gridview1.DataBind()
            msg.Close()
            con.close()        End Using
    End Sub


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblError.Text = ""
        lblSuccess.Text = ""
        If Not IsPostBack Then
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim a As New DataTable
                a.Columns.Add("ID")
                a.Columns.Add("Class")
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Select ID, class from class order by ID", con)
                Dim msg As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader
                Do While msg.Read()
                    a.Rows.Add(msg.Item(0), msg.Item(1))
                Loop
                Gridview1.DataSource = a

                Gridview1.DataBind()
                msg.Close()
                con.close()            End Using
        End If
        If Request.QueryString.ToString <> Nothing Then
            panel1.Visible = True
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class as Class, class.studentno as 'No of Students', class.type as 'Type', depts.dept as 'Department' from class inner join depts on class.superior = depts.id where class.id = '" & Request.QueryString.ToString & "'", con)
                Dim ds As New DataTable
                Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter
                Try
                    adapter1.SelectCommand = cmdLoad1
                    adapter1.Fill(ds)
                    DetailsView1.DataSource = ds
                    DetailsView1.DataBind()
                Catch ex As Exception
                    lblError.Text = ex.Message
                End Try
                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid where classteacher.class = '" & Request.QueryString.ToString & "'", con)
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                Dim ds2 As New DataTable
                ds2.Columns.Add("S/N")
                ds2.Columns.Add("Name")
                Dim i As Integer = 1
                Do While reader.Read
                    ds2.Rows.Add(i.ToString & ".  ", reader.Item(0))
                    i = i + 1
                Loop
                GridView2.DataSource = ds2
                GridView2.DataBind()
                reader.Close()

                Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT subjects.subject, staffprofile.surname, classsubjects.periods from classsubjects left join staffprofile on staffprofile.staffid = classsubjects.teacher inner join subjects on subjects.id = classsubjects.subject where classsubjects.class = '" & Request.QueryString.ToString & "'", con)
                Dim reader0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
                Dim ds3 As New DataTable
                ds3.Columns.Add("S/N")
                ds3.Columns.Add("Subject")
                ds3.Columns.Add("name")
                ds3.Columns.Add("periods")
                ds3.Columns.Add("View")
                Dim j As Integer = 1
                Do While reader0.Read
                    ds3.Rows.Add(j.ToString & ".    ", reader0.Item(0), reader0.Item(1), reader0.Item(2), "Edit")
                    j = j + 1
                Loop
                GridView3.DataSource = ds3
                GridView3.DataBind()
                reader0.Close()
                con.close()            End Using
        End If
    End Sub



    Protected Sub linkbterm_Click(sender As Object, e As EventArgs) Handles linkbterm.Click
        Response.Redirect("~/Admin/addclass.aspx")

    End Sub

    Protected Sub LinkButton1_Click(sender As Object, e As EventArgs) Handles LinkButton1.Click
        Response.Redirect("~/Admin/classallocate.aspx?" & Request.QueryString.ToString)

    End Sub

    Protected Sub GridView2_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles GridView2.RowDeleting
        Dim row As GridViewRow = GridView2.Rows(e.RowIndex)
        Dim ID As String = row.Cells(0).Text
        Dim sessions As String = row.Cells(1).Text
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim teacher As String
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from staffprofile where surname = '" & sessions & "'", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            If student0.Read Then
                teacher = student0.Item(0).ToString
            End If
            student0.Close()
            Dim enter As New MySql.Data.MySqlClient.MySqlCommand("delete from classteacher where teacher = '" & teacher & "' and class = '" & Request.QueryString.ToString & "'", con)
            enter.ExecuteNonQuery()
            Gridview1.EditIndex = -1
            Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT staffprofile.surname from classteacher inner join staffprofile on classteacher.teacher = staffprofile.staffid where classteacher.class = '" & Request.QueryString.ToString & "'", con)
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
            Dim ds2 As New DataTable
            ds2.Columns.Add("S/N")
            ds2.Columns.Add("Name")
            Dim i As Integer = 1
            Do While reader.Read
                ds2.Rows.Add(i.ToString & ".  ", reader.Item(0))
                i = i + 1
            Loop
            GridView2.DataSource = ds2
            GridView2.DataBind()
            reader.Close()
            con.close()        End Using
    End Sub

    Protected Sub GridView2_SelectedIndexChanged(sender As Object, e As EventArgs) Handles GridView2.SelectedIndexChanged

    End Sub

    Protected Sub Gridview1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles Gridview1.SelectedIndexChanged

    End Sub
End Class
 