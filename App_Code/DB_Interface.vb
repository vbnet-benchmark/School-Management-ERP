Imports Microsoft.VisualBasic
Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data

Public Class DB_Interface
    Inherits Date_Convert
    Public Sub New()
      
    End Sub


    Public Function Select_Query(ByVal query As String) As Array
        Dim finalresult As Array
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim command As New MySql.Data.MySqlClient.MySqlCommand(query, con)
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = command.ExecuteReader
            Dim i As Integer
            Dim colcount As Integer = reader.FieldCount
            Do While reader.Read
                i += 1
            Loop
            reader.Close()
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = command.ExecuteReader
            Dim result(colcount, i) As String
            Dim y As Integer
            Do While reader2.Read
                For columns = 0 To colcount - 1
                    result(columns, y) = reader2(columns).ToString
                Next
                y += 1
            Loop
            reader2.Close()
            finalresult = result
            con.Close()
        End Using
        Return finalresult
    End Function

    Public Function Select_1D(ByVal query As String) As ArrayList
        Dim finalresult As New ArrayList
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim command As New MySql.Data.MySqlClient.MySqlCommand(query, con)
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = command.ExecuteReader
            Dim i As Integer
            Dim colcount As Integer = reader.FieldCount
            Do While reader.Read
                i += 1
            Loop
            reader.Close()
            Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = command.ExecuteReader
            Dim result(colcount, i) As String
            Dim y As Integer
            Do While reader2.Read
                If colcount = 1 Then
                    finalresult.Add(reader2(0))
                ElseIf i = 1 Then
                    For x = 0 To colcount - 1
                        finalresult.Add(reader2(x))
                    Next
                End If
            Loop
            reader2.Close()
            con.Close()
        End Using
        Return finalresult
    End Function

    Public Function Select_single(ByVal query As String)
        Dim s
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim command As New MySql.Data.MySqlClient.MySqlCommand(query, con)
            s = command.ExecuteScalar
            con.Close()
        End Using
        Return s
    End Function


    Public Function Non_Query(ByVal query As String) As Boolean
        Dim finalresult As Boolean = False
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim command As New MySql.Data.MySqlClient.MySqlCommand(query, con)
            command.ExecuteNonQuery()
            con.Close()
            finalresult = True
        End Using
        Return finalresult
    End Function


End Class
