Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Partial Class Staff_profile
    Inherits System.Web.UI.Page
    Dim studentId As String

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Not IsPostBack Then
            If Session("ClassID") Is Nothing Then
                Session("ClassID") = Session("AddClass")
            End If
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT StudentsProfile.surname FROM StudentsProfile INNER JOIN StudentSummary ON StudentsProfile.admno = StudentSummary.student WHERE StudentSummary.Class = ? And StudentSummary.Session = ? ORDER BY StudentsProfile.surname", con)
                cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class.ID", Session("ClassID")))
                cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("SessionID")))
                Dim studentsReader As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader()
                DropDownList1.Items.Clear()
                DropDownList1.Items.Add("Select a student")
                Do While studentsReader.Read
                    DropDownList1.Items.Add(studentsReader.Item(0).ToString)
                Loop
                studentsReader.Close()
                con.close()            End Using
            If Not Session("StudentId") Is Nothing Then
                DropDownList1.Text = Session("StudentId").ToString
                Load_profile()
            End If

        End If
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged

        Load_profile()
    End Sub
    Protected Sub Load_profile()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim cmdLoad1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.admno as 'Admission number', studentsprofile.surname as Name, studentsprofile.Sex, studentsprofile.dateOfBirth as Birthday, studentsummary.age, studentsprofile.parent as 'Parents/Guardian', studentsprofile.phone as 'Parent Phone number'  from StudentsProfile inner join studentsummary on studentsummary.student = studentsprofile.admno where studentsprofile.surname = ? and studentsummary.session = ?", con)
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Trim(DropDownList1.Text)))
            cmdLoad1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter(" StudentSummary.Session ", Session("SessionID")))

            Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad1.ExecuteReader
            Try
                student.Read()
                Session("studentId") = student.Item(1)
                student.Close()
                Dim ds As New DataTable
                Dim adapter1 As New MySql.Data.MySqlClient.MySqlDataAdapter

                adapter1.SelectCommand = cmdLoad1
                adapter1.Fill(ds)
                DetailsView1.DataSource = ds
                DetailsView1.DataBind()
            Catch ex As Exception
                Response.Write(ex.Message)
            End Try
            Image1.ImageUrl = "~/image/noimage.jpg"
            Image1.Visible = True
            con.close()        End Using
    End Sub
End Class
