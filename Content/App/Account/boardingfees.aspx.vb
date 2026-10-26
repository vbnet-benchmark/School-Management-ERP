Imports System.IO
Partial Class Admin_addteacher



    Inherits System.Web.UI.Page
    

    Dim subselect As Integer
    Dim subremove As String
    Dim classremove As String
    Dim staffID As String




    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles bnUpdate.Click
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)            con.open()


            Dim classlist As New ArrayList
            Dim subjectlist As New ArrayList
            Dim STotal As Integer
            Dim SubjectAverage As Double
            Dim average As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from class", con)
            Dim reader As MySql.Data.MySqlClient.MySqlDataReader = average.ExecuteReader
            Do While reader.Read
                classlist.Add(reader.Item(0))
            Loop
            reader.Close()
            For Each item As Integer In classlist
                Dim averages As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from classsubjects where class = ?", con)
                averages.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", item))
                Dim readers As MySql.Data.MySqlClient.MySqlDataReader = averages.ExecuteReader
                subjectlist.Clear()
                Do While readers.Read
                    subjectlist.Add(readers.Item(3))
                Loop
                readers.Close()
                For Each j As String In subjectlist
                    Dim Saverage As New MySql.Data.MySqlClient.MySqlCommand("SELECT Total, student FROM SubjectReg WHERE  (Session = ?) AND (Class = ?) AND (subjectsOfferred = ?) order by total desc", con)
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", 30))
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", item))
                    Saverage.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subjectsOffered", j))
                    Dim readerT As MySql.Data.MySqlClient.MySqlDataReader = Saverage.ExecuteReader

                    Dim count As Integer = 0
                    Dim highest As Integer = 0
                    Dim lowest As Integer = 0

                    Dim i As Integer = 0
                    Do While readerT.Read
                        count = count + 1
                        If i = 0 Then
                            highest = Val(readerT.Item("Total"))

                        End If
                        STotal = STotal + Val(readerT.Item("Total"))
                        lowest = Val(readerT.Item("Total"))
                        i = i + 1
                    Loop

                    SubjectAverage = STotal / count
                    readerT.Close()
                    Try
                        Dim Saverages As New MySql.Data.MySqlClient.MySqlCommand("insert into analysis2 (subject, class, highest, lowest, average, session) values (?,?,?,?,?,?)", con)
                        Saverages.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subjectsOffered", j))
                        Saverages.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Class", item))
                        Saverages.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("highest", highest))
                        Saverages.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("lowest", lowest))
                        Saverages.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("average", SubjectAverage))
                        Saverages.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", 30))
                        Saverages.ExecuteNonQuery()
                        i = Nothing
                        highest = Nothing
                        lowest = Nothing
                        SubjectAverage = Nothing
                        count = Nothing
                        STotal = Nothing
                    Catch
                        Continue For
                    End Try
                Next
            Next

            MsgBox("Completed")


            Dim cmdLoad0 As New MySql.Data.MySqlClient.MySqlCommand("SELECT * from boarding", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad0.ExecuteReader
            If student0.Read Then
                student0.Close()
                Dim cmdCheck3 As New MySql.Data.MySqlClient.MySqlCommand("Update boarding Set cost = ?", con)
                cmdCheck3.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", txtCost.Text))
                cmdCheck3.ExecuteNonQuery()
                lblSuccess.Text = "Fee Added Successfully"
            Else
                student0.Close()
                Dim cmdCheck2 As New MySql.Data.MySqlClient.MySqlCommand("Insert Into boarding (cost) Values (?)", con)
                cmdCheck2.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("cost", txtCost.Text.Replace(",", "")))
                cmdCheck2.ExecuteNonQuery()
                lblSuccess.Text = "Fee Added Successfully"

            End If
            con.close()        End Using

    End Sub
   
   

   

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load

        
    End Sub

   

    

   
   
End Class
