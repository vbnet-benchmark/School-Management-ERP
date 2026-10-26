Imports Microsoft.VisualBasic

Public Class notify

    Public Sub Notifications(message As String, receiver As String, sender As String, relationship As String, url As String, Optional type As String = "")
        If Not sender = "demo1" Then
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)                con.open()
                Dim stripped As String = Replace(message, "'", "")
                Dim classes1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into notifications (recipient, message, origin, relationship,  type, time, url) values ('" & receiver & "', '" & stripped & "', '" & sender & "', '" & relationship & "', '" & type & "', '" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss") & "', '" & url & "')", con)
                classes1.ExecuteNonQuery()
                con.close()            End Using
        End If
    End Sub
    Public Sub log(user As String, activity As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            If con.State = Data.ConnectionState.Closed Then                con.Open()
                Dim stripped As String = Replace(activity, "'", "")
                Dim classes1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into log (user, activity, time) values ('" & user & "', '" & stripped & "', '" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss") & "')", con)
                classes1.ExecuteNonQuery()

            End If
            con.close()        End Using
    End Sub

    Public Function error_log(obj As String, message As String) As String
        Try
            Dim err As String
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                If con.State = Data.ConnectionState.Closed Then                    con.Open()
                    Dim classes1 As New MySql.Data.MySqlClient.MySqlCommand("Insert into error (object, message, time) values ('" & obj & "', '" & message & "', '" & Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss") & "')", con)
                    classes1.ExecuteNonQuery()
                    Dim classes2 As New MySql.Data.MySqlClient.MySqlCommand("Select id from error order by id desc", con)
                    Dim no As MySql.Data.MySqlClient.MySqlDataReader = classes2.ExecuteReader
                    no.Read()
                    err = "There was an error in execution. Error number = " & no(0)


                End If
                con.close()            End Using            Return err
        Catch ex As Exception
            Dim err As String = "There was an error in execution. Error type = connect"
            Return err
        End Try
    End Function
    Public Sub Read_notification(url As String, user As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()
            Dim newurl As Array
            Try
                newurl = url.Split("?")
                Dim classes2 As New MySql.Data.MySqlClient.MySqlCommand("Update notifications set status = '" & "Read" & "' where url = '" & newurl(0) & "' and recipient = '" & user & "'", con)
                classes2.ExecuteNonQuery()
            Catch
                Dim classes2 As New MySql.Data.MySqlClient.MySqlCommand("Update notifications set status = '" & "Read" & "' where url = '" & url & "' and recipient = '" & user & "'", con)
                classes2.ExecuteNonQuery()
            End Try
           
            con.close()        End Using
    End Sub

End Class
