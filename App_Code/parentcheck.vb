Imports Microsoft.VisualBasic

Public Class parentcheck


    Public Function getparent(student As String) As String
        Dim parent As String = ""
        Using cons As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            cons.Open()
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT parentward.parent from parentward where ward = '" & student & "'", cons)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader

            If student0.Read() Then
                parent = student0(0).ToString
            End If
            student0.Close()
            cons.Close()
        End Using
        Return parent

    End Function
    Public Function getstuname(student As String) As String
        Dim parent As String = ""
        Using cons As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            cons.Open()
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from studentsprofile where admno = '" & student & "'", cons)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            If student0.Read() Then
                parent = student0(0).ToString
            End If
            student0.Close()
            cons.Close()
        End Using
        Return parent
    End Function
    Public Function getstaff(student As String) As String
        Dim parent As String = ""
        Using cons As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            cons.Open()
            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT surname from staffprofile where staffid = '" & student & "'", cons)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            If student0.Read() Then
                parent = student0(0).ToString
            End If
            student0.Close()
            cons.Close()
        End Using
        Return parent
    End Function
End Class
