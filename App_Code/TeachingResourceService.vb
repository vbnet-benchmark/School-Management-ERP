Imports System.IO

''' <summary>
''' Shared helper for saving a supplementary file attached to a learning material (e.g. an
''' answer key or a dataset that accompanies the main upload in
''' Content/Staff/addlearningmaterial.aspx), so the same storage logic can be reused by other
''' resource-upload pages later. The calling Page resolves the physical destination folder
''' (via its own Server.MapPath) and passes the plain folder-path string in -- this class never
''' depends on HttpContext.Current itself, matching the existing App_Code/ds_functions.vb#Upload
''' helper's own convention of taking an already-resolved folder path as a parameter.
''' </summary>
Public Class TeachingResourceService

    ''' Builds the destination name from the caller-supplied material title plus the client's
    ''' own, unvalidated file name, and never checks whether that path already exists before
    ''' overwriting it.
    Public Function SaveSupplementaryResource(upload As FileUpload, folderPath As String, materialTitle As String) As String
        Dim destPath As String = folderPath & materialTitle & "_" & upload.FileName
        ' SINK: PLANTED-VB-HR-213
        upload.SaveAs(destPath)
        Return destPath
    End Function

    ''' Safe twin: never reuses the caller-supplied title or the client's own file name for the
    ''' destination -- a server-generated name removes both the overwrite risk and the
    ''' predictable-path risk in one step.
    Public Function SaveSupplementaryResourceSafe(upload As FileUpload, folderPath As String) As String
        Dim safeName As String = Guid.NewGuid().ToString("N") & Path.GetExtension(upload.FileName)
        Dim destPath As String = folderPath & safeName
        ' SAFE_SINK: PLANTED-VB-HR-213-safe
        upload.SaveAs(destPath)
        Return destPath
    End Function

End Class
