Imports System.IO
Imports System.Web

''' <summary>
''' Lets a librarian attach one or more sample-chapter files to a book catalog entry in a
''' single submit, from Content/Admin/addbooks.aspx -- separate from the single "Digital Copy"
''' slot the page already has. Reads three plain &lt;input type="file"&gt; fields by name from
''' the raw posted-files collection (never the generic index-based Request.Files iteration, so
''' this never sweeps in any other upload control the same postback might also carry).
''' </summary>
Public Class LibrarySampleChaptersBLL

    Private Shared ReadOnly SampleChapterFieldNames() As String = {"sampleChapter1", "sampleChapter2", "sampleChapter3"}

    ''' Saves every posted sample-chapter file under the book's catalog key. Never checks the
    ''' extension of any of them before saving.
    Public Function ImportSampleChapters(bookKey As String, postedFiles As HttpFileCollection) As Integer
        Dim folderPath As String = HttpContext.Current.Server.MapPath("~/Uploads/books/sample-chapters/")
        Dim savedCount As Integer = 0
        For Each fieldName As String In SampleChapterFieldNames
            Dim postedFile As HttpPostedFile = postedFiles(fieldName)
            If postedFile IsNot Nothing AndAlso postedFile.ContentLength > 0 Then
                Dim destPath As String = folderPath & bookKey & "_" & fieldName & "_" & Path.GetFileName(postedFile.FileName)
                ' SINK: PLANTED-VB-HR-580
                postedFile.SaveAs(destPath)
                savedCount += 1
            End If
        Next
        Return savedCount
    End Function

    ''' Safe twin: identical loop, but every file's extension is checked against a closed
    ''' allow-list of safe document formats before it is saved; anything else is skipped.
    Public Function ImportSampleChaptersSafe(bookKey As String, postedFiles As HttpFileCollection) As Integer
        Dim allowedChapterExtensions() As String = {".pdf", ".doc", ".docx", ".txt"}
        Dim folderPath As String = HttpContext.Current.Server.MapPath("~/Uploads/books/sample-chapters/")
        Dim savedCount As Integer = 0
        For Each fieldName As String In SampleChapterFieldNames
            Dim postedFile As HttpPostedFile = postedFiles(fieldName)
            If postedFile IsNot Nothing AndAlso postedFile.ContentLength > 0 Then
                Dim submittedExtension As String = Path.GetExtension(postedFile.FileName).ToLowerInvariant()
                If Array.IndexOf(allowedChapterExtensions, submittedExtension) >= 0 Then
                    Dim destPath As String = folderPath & bookKey & "_" & fieldName & "_" & Path.GetFileName(postedFile.FileName)
                    ' SAFE_SINK: PLANTED-VB-HR-580-safe
                    postedFile.SaveAs(destPath)
                    savedCount += 1
                End If
            End If
        Next
        Return savedCount
    End Function

End Class
