Imports Microsoft.VisualBasic
Imports System.IO
Public Class ds_functions
    Inherits DB_Interface
    Public Function Get_Stu_name(admno As String) As String
        Return Select_single("SELECT surname from studentsprofile where admno = '" & admno & "'")
    End Function

    Public Function Get_Staff_name(staffid As String) As String
        Return Select_single("SELECT surname from staffprofile where staffid = '" & staffid & "'")
    End Function

    Public Function Get_parent_name(parentid As String) As String
        Return Select_single("SELECT parentname from parentprofile where parentid = '" & parentid & "'")
    End Function

    Public Function Get_Stu_ID(surname As String) As String
        Return Select_single("SELECT admno from studentsprofile where surname = '" & surname & "'")
    End Function

    Public Function Get_Staff_ID(surname As String) As String
        Return Select_single("SELECT staffid from staffprofile where surname = '" & surname & "'")
    End Function

    Public Function Get_parent_ID(parentname As String) As String
        Return Select_single("SELECT parentid from parentprofile where parentname = '" & parentname & "'")
    End Function
    Public Function Fetch_admins() As ArrayList
        Dim a As ArrayList = Select_1D("select username from admin")
        Return a
    End Function

    Public Function Fetch_accounts() As ArrayList
        Dim a As ArrayList = Select_1D("select username from acclogin")
        Return a
    End Function

    Public Function Get_Subject_ID(subject As String) As Integer
        Return Select_single("select id from subjects where subject = '" & subject & "'")
    End Function

    Public Function Get_Subject_Name(id As Integer) As Integer
        Return Select_single("select subject from subjects where id = '" & id & "'")
    End Function

    Public Function Get_Class_Name(id As Integer) As Integer
        Return Select_single("select class from class where id = '" & id & "'")
    End Function
    Public Function Get_Class_ID(clas As String) As Integer
        Return Select_single("select id from class where class = '" & clas & "'")
    End Function
    Public Function Class_Parents(clas As String, session As Integer) As ArrayList
        Return Select_1D("SELECT parentprofile.parentname from studentsummary inner join (studentsprofile inner join (parentward inner join parentprofile on parentward.parent = parentprofile.parentId) on parentward.ward = studentsprofile.admno) on studentsprofile.admno = studentsummary.student inner join class on studentsummary.class = class.Id where class.class = '" & clas & "' and studentsummary.session = '" & session & "'")
    End Function

    Public Function Subjects_Taught(staff As String) As ArrayList
        Dim a As ArrayList = Select_1D("select subjects.subject from classsubjects inner join subjects on subjects.id = classsubjects.subject inner join staffprofile on staffprofile.staffid = classsubjects.teacher where staffprofile.surname = '" & staff & "'")
        Dim b As New ArrayList
        For Each y In a
            If Not b.Contains(y) Then b.Add(y)
        Next
        Return b
    End Function

    Public Function Class_Taught(staff As String, subject As String) As ArrayList
        Return Select_1D("select class.class from classsubjects inner join class on class.id = classsubjects.class inner join subjects on subjects.id = classsubjects.subject inner join staffprofile on staffprofile.staffid = classsubjects.teacher where staffprofile.surname = '" & staff & "' and subjects.subject = '" & subject & "'")
    End Function
    ' --- Message Attachment Upload (PLANTED-VB-HR-578) ---
    ' Shared by the compose-message pages across every role (Admin/Staff/Account -- real,
    ' pre-existing callers) so the same upload logic isn't duplicated per role. Takes an
    ' already-resolved folder path from the caller and saves the client's own file straight
    ' into it; the extension of that file name is never checked anywhere in this call chain.

    ' Instance (Interprocedural): page -> this shared App_Code class.
    Public Sub Upload(fileupload1 As FileUpload, folderpath As String)
        If fileupload1.HasFile Then
            ' SINK: PLANTED-VB-HR-578
            fileupload1.SaveAs(folderpath & Path.GetFileName(fileupload1.FileName))
        End If
    End Sub

    ' Safe twin (PLANTED-VB-HR-578-safe): same shared helper class, but the posted file's
    ' extension is checked against a closed allow-list of safe attachment types before saving.
    ' Wired to the "Attach Document (Validated)" control added alongside one of the existing
    ' callers of Upload(), above.
    Public Sub UploadSafe(fileupload1 As FileUpload, folderpath As String)
        If fileupload1.HasFile Then
            Dim allowedAttachmentExtensions() As String = {".pdf", ".doc", ".docx", ".jpg", ".jpeg", ".png"}
            Dim submittedExtension As String = Path.GetExtension(fileupload1.FileName).ToLowerInvariant()
            If Array.IndexOf(allowedAttachmentExtensions, submittedExtension) < 0 Then
                Throw New ApplicationException("Unsupported attachment type.")
            End If
            ' SAFE_SINK: PLANTED-VB-HR-578-safe
            fileupload1.SaveAs(folderpath & Path.GetFileName(fileupload1.FileName))
        End If
    End Sub
End Class
