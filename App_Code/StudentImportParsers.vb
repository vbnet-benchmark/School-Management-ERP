Imports System.Xml

''' <summary>
''' Polymorphic parser hierarchy backing the "Import Student Records from XML" admin feature
''' (Content/Admin/importstudents.aspx.vb, "Smart Import (Auto-Detect Format)" button). The caller
''' always goes through StudentImportParserFactory.GetParser(xmlText).Parse(xmlText) -- a single
''' shared call site with no visible branch of its own; which concrete subclass actually runs
''' depends entirely on the uploaded file's own declared root-element shape.
''' </summary>
Public MustInherit Class XmlImportParserBase
    Public MustOverride Function Parse(xmlText As String) As XmlDocument
End Class

''' <summary>
''' Handles the older "LegacyStudentExport" root-element shape still produced by a couple of
''' long-retired export tools, several of which declare a DOCTYPE referencing a shared schema
''' subset. External entity resolution is re-enabled so those legacy files keep importing
''' correctly.
''' </summary>
Public Class LegacyFormatParser
    Inherits XmlImportParserBase

    Public Overrides Function Parse(xmlText As String) As XmlDocument
        Dim doc As New XmlDocument()
        doc.XmlResolver = New XmlUrlResolver()
        ' SINK: PLANTED-VB-HR-114
        doc.LoadXml(xmlText)
        Return doc
    End Function
End Class

''' <summary>
''' Handles the current "StudentImport" root-element shape used by every modern export of the
''' student register. No DOCTYPE support is needed for this format, so the resolver is left at
''' its safe (nulled) value.
''' </summary>
Public Class StandardFormatParser
    Inherits XmlImportParserBase

    Public Overrides Function Parse(xmlText As String) As XmlDocument
        Dim doc As New XmlDocument()
        doc.XmlResolver = Nothing
        ' SAFE_SINK: PLANTED-VB-HR-114-safe
        doc.LoadXml(xmlText)
        Return doc
    End Function
End Class

Public Class StudentImportParserFactory
    Public Shared Function GetParser(xmlText As String) As XmlImportParserBase
        If xmlText.Contains("<LegacyStudentExport") Then
            Return New LegacyFormatParser()
        Else
            Return New StandardFormatParser()
        End If
    End Function
End Class
