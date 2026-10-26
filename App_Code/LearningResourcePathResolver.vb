Imports System
Imports System.IO
Imports System.Web

''' <summary>
''' Resolves a raw, caller-supplied resource name into a physical path for
''' Content/Staff/downloadresource.aspx's "Resolver Mode" preview feature. Two concrete
''' implementations exist, chosen at runtime by downloadresource.aspx.vb based on the
''' "resolverMode" text field: one resolves against the shared teacher learning-material folder
''' (Content/Uploads/, also used by Content/Staff/addlearningmaterial.aspx's own upload), the
''' other against the shared student-submitted supplementary-answer folder
''' (Content/Uploads/supplementary/, used by Content/Student/doassignment.aspx) -- letting a
''' teacher reviewing a class double-check either kind of file by name from one page.
''' </summary>
Public MustInherit Class LearningResourcePathResolver
    Public MustOverride Function Resolve(ByVal rawName As String) As String
End Class

''' <summary>
''' Resolves against the teacher learning-material folder. Names arriving here are typed in by a
''' teacher re-checking their own upload, so this resolver does not revalidate them -- it simply
''' appends the raw name as-is.
''' </summary>
Public Class TeacherMaterialPathResolver
    Inherits LearningResourcePathResolver

    Public Overrides Function Resolve(ByVal rawName As String) As String
        Return HttpContext.Current.Server.MapPath("~/content/Uploads/" & rawName)
    End Function
End Class

''' <summary>
''' Resolves against the student-submitted supplementary-answer folder. Reduces the name to its
''' bare file-name component first, so the result can never resolve outside that fixed directory.
''' </summary>
Public Class StudentSubmissionPathResolver
    Inherits LearningResourcePathResolver

    Public Overrides Function Resolve(ByVal rawName As String) As String
        Return HttpContext.Current.Server.MapPath("~/Content/Uploads/supplementary/" & Path.GetFileName(rawName))
    End Function
End Class
