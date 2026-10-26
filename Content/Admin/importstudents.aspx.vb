Imports System.IO
Imports System.Xml

Partial Class Admin_importstudents
    Inherits System.Web.UI.Page

    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder

    Private Sub Show_Alert(type As Boolean, msg As String)
        alertPLC = Me.Master.FindControl("plcAlert")
        If type = True Then
            alert.Text = alertmsg.success_message(msg)
        Else
            alert.Text = alertmsg.error_message(msg)
        End If
        alertPLC.Controls.Add(alert)
    End Sub

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If check.Check_Admin(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
    End Sub

    Private Sub Import_Students_From_Doc(doc As XmlDocument)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.open()
            Dim nodes As XmlNodeList = doc.SelectNodes("//Student")
            For Each node As XmlNode In nodes
                Dim cmd As New MySql.Data.MySqlClient.MySqlCommand("Insert Into studentsprofile (admno, surname, sex, dateofbirth, phone) Values (?,?,?,?,?)", con)
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("admno", node.SelectSingleNode("AdmNo").InnerText))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("surname", node.SelectSingleNode("Surname").InnerText))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sex", node.SelectSingleNode("Sex").InnerText))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("dob", node.SelectSingleNode("DateOfBirth").InnerText))
                cmd.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("phone", node.SelectSingleNode("Phone").InnerText))
                cmd.ExecuteNonQuery()
                logify.log(Session("staffid"), "A student record was imported by admin.")
            Next
            con.close()
        End Using
    End Sub

    ' ==== Direct tier: a fresh upload is parsed and consumed within this same method. ====

    Protected Sub btnImportLegacy_Click(sender As Object, e As EventArgs)
        If Not FileUploadXml.HasFile Then
            Show_Alert(False, "Please choose an XML file to import.")
            Exit Sub
        End If
        Dim xmlText As String = New StreamReader(FileUploadXml.PostedFile.InputStream).ReadToEnd()
        Dim doc As New XmlDocument()
        ' Re-enable external entity resolution so older student-export files that declare a
        ' DOCTYPE referencing a shared "student-schema.dtd" subset keep importing correctly.
        doc.XmlResolver = New XmlUrlResolver()
        ' SINK: PLANTED-VB-HR-111
        doc.LoadXml(xmlText)
        Import_Students_From_Doc(doc)
        Show_Alert(True, "Student records imported (legacy-compatible mode).")
    End Sub

    Protected Sub btnImportModern_Click(sender As Object, e As EventArgs)
        If Not FileUploadXml.HasFile Then
            Show_Alert(False, "Please choose an XML file to import.")
            Exit Sub
        End If
        Dim xmlText As String = New StreamReader(FileUploadXml.PostedFile.InputStream).ReadToEnd()
        Dim doc As New XmlDocument()
        doc.XmlResolver = Nothing
        ' SAFE_SINK: PLANTED-VB-HR-111-safe
        doc.LoadXml(xmlText)
        Import_Students_From_Doc(doc)
        Show_Alert(True, "Student records imported (modern/strict mode).")
    End Sub

    ' ==== Indirect tier (same-class private helper functions). ====

    Private Function ParseWithCompatibilitySettings(xmlText As String) As XmlDocument
        Dim settings As New XmlReaderSettings()
        ' Allow a DOCTYPE at all (the default, Prohibit, rejects every legacy export file
        ' outright), and explicitly keep entity resolution enabled so an external SYSTEM/PUBLIC
        ' reference in the DOCTYPE still resolves -- both settings are configured together since
        ' relying on DtdProcessing alone to also govern external-entity resolution is a common,
        ' easy-to-make mistake (the two properties are independent knobs).
        settings.DtdProcessing = DtdProcessing.Parse
        settings.XmlResolver = New XmlUrlResolver()
        Dim doc As New XmlDocument()
        Using reader As XmlReader = XmlReader.Create(New StringReader(xmlText), settings)
            ' SINK: PLANTED-VB-HR-112
            doc.Load(reader)
        End Using
        Return doc
    End Function

    Private Function ParseWithStrictSettings(xmlText As String) As XmlDocument
        Dim settings As New XmlReaderSettings()
        settings.DtdProcessing = DtdProcessing.Prohibit
        Dim doc As New XmlDocument()
        Using reader As XmlReader = XmlReader.Create(New StringReader(xmlText), settings)
            ' SAFE_SINK: PLANTED-VB-HR-112-safe
            doc.Load(reader)
        End Using
        Return doc
    End Function

    Protected Sub btnImportViaHelper_Click(sender As Object, e As EventArgs)
        If Not FileUploadXml.HasFile Then
            Show_Alert(False, "Please choose an XML file to import.")
            Exit Sub
        End If
        Dim xmlText As String = New StreamReader(FileUploadXml.PostedFile.InputStream).ReadToEnd()
        Dim doc As XmlDocument
        If chkCompatibilityMode.Checked Then
            doc = ParseWithCompatibilitySettings(xmlText)
        Else
            doc = ParseWithStrictSettings(xmlText)
        End If
        Import_Students_From_Doc(doc)
        Show_Alert(True, "Student records imported.")
    End Sub

    ' ==== Indirect tier, repeated with a different construction shape: the uploaded XML is
    ' stored in Session on one postback and read back and actually parsed on a LATER, separate
    ' postback -- a cross-request/cross-method data-flow shape rather than a same-request helper
    ' call. ====

    Protected Sub btnUploadPreview_Click(sender As Object, e As EventArgs)
        If Not FileUploadXml.HasFile Then
            Show_Alert(False, "Please choose an XML file to import.")
            Exit Sub
        End If
        Dim xmlText As String = New StreamReader(FileUploadXml.PostedFile.InputStream).ReadToEnd()
        Session("pendingImportXml") = xmlText
        Dim approxCount As Integer = (xmlText.Length - xmlText.Replace("<Student>", "").Length) \ "<Student>".Length
        litPreview.Text = "Found approximately " & approxCount.ToString() & " student record(s). Review, then Confirm Import or Validate Only below."
    End Sub

    Protected Sub btnConfirmImport_Click(sender As Object, e As EventArgs)
        Dim xmlText As String = CStr(Session("pendingImportXml"))
        If String.IsNullOrEmpty(xmlText) Then
            Show_Alert(False, "No pending import found. Upload a file first.")
            Exit Sub
        End If
        Dim doc As New XmlDocument()
        ' Legacy-compatible confirm path -- intentionally lenient so older exported files with a
        ' DOCTYPE still import correctly, mirroring btnImportLegacy_Click's own rationale.
        doc.XmlResolver = New XmlUrlResolver()
        ' SINK: PLANTED-VB-HR-115
        doc.LoadXml(xmlText)
        Import_Students_From_Doc(doc)
        Session("pendingImportXml") = Nothing
        Show_Alert(True, "Pending import confirmed and applied.")
    End Sub

    Protected Sub btnValidateOnly_Click(sender As Object, e As EventArgs)
        Dim xmlText As String = CStr(Session("pendingImportXml"))
        If String.IsNullOrEmpty(xmlText) Then
            Show_Alert(False, "No pending import found. Upload a file first.")
            Exit Sub
        End If
        Dim settings As New XmlReaderSettings()
        settings.DtdProcessing = DtdProcessing.Prohibit
        Dim doc As New XmlDocument()
        Using reader As XmlReader = XmlReader.Create(New StringReader(xmlText), settings)
            ' SAFE_SINK: PLANTED-VB-HR-115-safe
            doc.Load(reader)
        End Using
        Dim nodeCount As Integer = doc.SelectNodes("//Student").Count
        litPreview.Text = "Validation passed: " & nodeCount.ToString() & " well-formed student record(s). No changes were saved."
    End Sub

    ' ==== Interprocedural tier: routed through a cross-file service class. ====

    Protected Sub btnServiceImport_Click(sender As Object, e As EventArgs)
        If Not FileUploadXml.HasFile Then
            Show_Alert(False, "Please choose an XML file to import.")
            Exit Sub
        End If
        Dim imported As Integer = StudentImportService.ImportFromXml(FileUploadXml.PostedFile.InputStream, chkServiceLegacyMode.Checked)
        Show_Alert(True, imported.ToString() & " student record(s) imported via the bulk import service.")
    End Sub

    ' ==== Type/polymorphism-dependent tier: a single shared call site, factory-resolved. ====

    Protected Sub btnSmartImport_Click(sender As Object, e As EventArgs)
        If Not FileUploadXml.HasFile Then
            Show_Alert(False, "Please choose an XML file to import.")
            Exit Sub
        End If
        Dim xmlText As String = New StreamReader(FileUploadXml.PostedFile.InputStream).ReadToEnd()
        Dim parser As XmlImportParserBase = StudentImportParserFactory.GetParser(xmlText)
        Dim doc As XmlDocument = parser.Parse(xmlText)
        Import_Students_From_Doc(doc)
        Show_Alert(True, "Student records imported (auto-detected format).")
    End Sub

End Class
