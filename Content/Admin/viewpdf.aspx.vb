Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.Web
Imports System.IO
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.parser

Partial Class Admin_allstudents
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList
    Dim logify As New notify


    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
       

            Dim byts() As Byte

        Dim a As Array = Split(Session("pic1"), "/")
            Dim wc As New System.Net.WebClient

        byts = wc.DownloadData(Session("pic1"))

            Dim ms As New MemoryStream(byts)
            Context.Response.Clear()
        Context.Response.AppendHeader("Content-Disposition", "filename=" + a(5))
        Response.ContentType = Session("pic1type")
            Context.Response.Buffer = True
            Context.Response.BinaryWrite(byts)
            Context.Response.End()

          


    End Sub





End Class
