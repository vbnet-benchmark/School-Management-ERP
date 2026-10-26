Imports System.IO
Imports System.Xml

''' <summary>
''' Cross-file bulk-import service backing the "Import Student Records from XML" admin feature
''' (Content/Admin/importstudents.aspx.vb, "Bulk Import via Service" button). Kept separate from
''' the page's own code-behind so the same import logic can later be reused by a scheduled/
''' background import job without duplicating it.
''' </summary>
Public Class StudentImportService

    Public Shared Function ImportFromXml(xmlStream As Stream, legacyMode As Boolean) As Integer
        Dim doc As New XmlDocument()
        If legacyMode Then
            ' Older exported student-record files sometimes declare a DOCTYPE referencing a
            ' shared schema subset; re-enable resolution so those files keep importing correctly.
            doc.XmlResolver = New XmlUrlResolver()
            ' SINK: PLANTED-VB-HR-113
            doc.Load(xmlStream)
        Else
            doc.XmlResolver = Nothing
            ' SAFE_SINK: PLANTED-VB-HR-113-safe
            doc.Load(xmlStream)
        End If

        Dim nodes As XmlNodeList = doc.SelectNodes("//Student")
        Dim count As Integer = 0
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            For Each node As XmlNode In nodes
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("Insert Into studentsprofile (admno, surname, sex, dateofbirth, phone) Values (?,?,?,?,?)", con)
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("admno", node.SelectSingleNode("AdmNo").InnerText))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", node.SelectSingleNode("Surname").InnerText))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", node.SelectSingleNode("Sex").InnerText))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dob", node.SelectSingleNode("DateOfBirth").InnerText))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phone", node.SelectSingleNode("Phone").InnerText))
                cmd.ExecuteNonQuery()
                count += 1
            Next
            con.close()
        End Using
        Return count
    End Function

End Class
