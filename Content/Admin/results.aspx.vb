Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls

Partial Class Admin_results
    Inherits System.Web.UI.Page


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session", con)
                Dim reader As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck2.ExecuteReader()
                Do While reader.Read()
                    cboYr.Items.Add(reader.Item(1).ToString & "   " & reader.Item(2).ToString)
                Loop
                cboYr.Text = reader.Item(1).ToString & "   " & reader.Item(2).ToString
                Session("rsSession") = reader.Item(0)
                If reader.Item(6).ToString = "Published" Then
                    Label5.Text = "Results published"
                    Label5.ForeColor = Drawing.Color.Green
                    Button2.Text = "Unpublish"
                Else
                    Label5.Text = "Results not published"
                    Label5.ForeColor = Drawing.Color.Red
                    Button2.Text = "Publish"

                End If
                reader.Close()

                Dim cmdInsert3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Class", con)
                Dim classreader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert3.ExecuteReader
                Do While classreader2.Read()
                    DropDownList3.Items.Add(classreader2.Item(1))
                Loop
                classreader2.Close()
        End If
        con.close()end using
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * From class where class = ?", con)
            cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("class", DropDownList3.Text))
            Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdCheck3.ExecuteReader
            reader3.Read()
            Session("rsClass") = reader3.Item(0).ToString
            reader3.Close()
            con.close()        End Using
        Response.Redirect("~/Staff/classreport.aspx")
    End Sub

    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim status As String
            If Button2.Text = "Publish" Then
                status = "Published"
            Else
                status = "Unpublished"
            End If
            Dim sArray() As String = Split(cboYr.Text, "  ")


            Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Update Session set status = ? where session = ? and term = ?", con)
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("status", status))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", Trim(sArray(0))))
            cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("closing", Trim(sArray(1))))
            cmdInsert2.ExecuteNonQuery()
            If status = "Published" Then
                Label5.Text = "Results published"
                Label5.ForeColor = Drawing.Color.Green
                Button2.Text = "Unpublish"
            Else
                Label5.Text = "Results not published"
                Label5.ForeColor = Drawing.Color.Red
                Button2.Text = "Publish"
            End If

            con.close()        End Using
    End Sub
End Class
