Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Admin_issuepin
    Inherits System.Web.UI.Page


    Protected Sub Button2_Click(sender As Object, e As EventArgs) Handles Button2.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from StudentsProfile where admno = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", txtID.Text))
            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            If Not student.Read() Then
                lblError.Text = "Admission number invalid"
                student.Close()
            Else
                Label13.Text = "Student Name: " & student.Item(1)

                student.Close()

                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                reader2.Read()
                Dim expiry As Date = reader2.Item(4)
                reader2.Close()
                Dim cmdcheck As New MySql.Data.MySqlClient.MySqlCommand("select * from pin where admno = ? order by ID desc", con)
                cmdcheck.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("admno", txtID.Text))
                Dim reader3 As MySql.Data.MySqlClient.MySqlDataReader = cmdcheck.ExecuteReader
                reader3.Read()
                If reader3.HasRows Then
                    If Not reader3.Item("expiry") >= Now.Date Then
                        Label14.Text = "Pin: " & reader3.Item(2) & " .Pin expired"
                        Button1.Text = "ReIssue"
                        Session("IsReprint") = False

                    Else
                        Label14.Text = "Pin: " & reader3.Item(2)
                        Button1.Text = "Reprint"
                        Session("IsReprint") = True
                    End If
                Else
                    Label14.Text = "Pin: Not Issued"
                    Button1.Text = "Issue pin"
                    Session("IsReprint") = False
                End If
                Button1.Visible = True
                Button3.Visible = True
                reader3.Close()
                txtID.Enabled = False
            End If
            con.close()        End Using
    End Sub

    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        If Label13.Text = "" And txtID.Enabled Then
            lblError.Text = "Please enter a valid admission no and search for a student before isssuing pin"
        Else
            Dim pin As New Random
            Dim x As Integer = pin.Next(100000000, 999999999)
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                reader2.Read()
                Dim expiry As Date = reader2.Item(4)
                reader2.Close()
                If Not Session("IsReprint") = True Then
                    Try
                        Dim cmdInsert2 As New MySql.Data.MySqlClient.MySqlCommand("Insert into pin (admno, pin, dateissued, expiry) values (?,?,?,?)", con)
                        cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("admno", txtID.Text))
                        cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("pin", x))
                        cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("now", FormatDateTime(Now, DateFormat.ShortDate)))
                        cmdInsert2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("expiry", FormatDateTime(expiry, DateFormat.ShortDate)))
                        cmdInsert2.ExecuteNonQuery()
                    Catch s As Exception
                        lblError.Text = "Unable to issue pin. " & s.Message
                        con.close()end using
            Exit Sub
                    End Try
            Session("IsReprint") = True
        End If
        con.close()end using
        Session("StudentId") = txtID.Text

        Response.Redirect("~/admin/pinslip.aspx")
            End If
    End Sub
    Protected Sub Check_Student()



    End Sub
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        lblError.Text = ""
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        lblError.Text = ""
        Button1.Visible = False
        Button3.Visible = False
        txtID.Enabled = True
        Label13.Text = ""
        Label14.Text = ""
        Session("IsReprint") = False
        txtID.Text = ""
    End Sub
End Class
