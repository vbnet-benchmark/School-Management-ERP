Imports System.Data
Imports System.Data.SqlClient
Imports System.IO
Imports System.Web.UI
Imports System

Public Class Stud_Enroll
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub ImageButton1_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles ImageButton1.Click
        'If FileUpload1.PostedFile IsNot Nothing Then
        '    Dim FileName As String = Path.GetFileName(FileUpload1.PostedFile.FileName)

        '    'Save files to disk 
        '    FileUpload1.SaveAs(Server.MapPath("image/" & FileName))

        'Add Entry to DataBase 
        Dim strConnString As [String] = System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString
        Dim con As New SqlConnection(strConnString)
        Dim strQuery As String = "insert into tblEnroll (StudentID,Fullname,Class,House,Term,Year) values(@StudentID,@Fullname,@Class,@House,@Term,@Year)"
        Dim cmd As New SqlCommand(strQuery)
        cmd.Parameters.AddWithValue("@StudentID", txtID.Text)
        cmd.Parameters.AddWithValue("@Fullname", txtFname.Text)
        cmd.Parameters.AddWithValue("@Class", cboClass.Text)
        cmd.Parameters.AddWithValue("@House", cboClass.Text)
        cmd.Parameters.AddWithValue("@Term", cboClass.Text)
        cmd.Parameters.AddWithValue("@Year", cboClass.Text)
        'cmd.Parameters.AddWithValue("@Pic", "image/" & FileName)
        'cmd.Parameters.AddWithValue("@Status", "No")
        cmd.CommandType = CommandType.Text
        cmd.Connection = con

        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                cmd.ExecuteNonQuery()

                MsgBox("Registration Successful")

        Catch ex As Exception
            Response.Write(ex.Message)
            'lblmessage.ForeColor = System.Drawing.Color.Red
            'lblmessage.Text = "File format not recognised." _
            '& " Upload Image formats"
        Finally
            con.close()end using
            con.Dispose()
            Response.Redirect(Request.Url.AbsoluteUri)
        End Try
        'End If
    End Sub
End Class