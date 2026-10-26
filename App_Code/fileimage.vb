Imports Microsoft.VisualBasic

Public Class fileimage

    Public Function get_image(filetype As String) As String
        Dim source As String = "~/img/file.png"
        If filetype = "application/pdf" Then
            source = "~/img/pdf.png"
        ElseIf filetype = "application/msword" Or filetype = "application/vnd.openxmlformats-officedocument.wordprocessingml.document" Then
            source = "~/img/word.png"
        ElseIf filetype = "application/vnd-ms-excel" Or filetype = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet" Then
            source = "~/img/excel.png"
        ElseIf filetype = "application/vnd-ms-powerpoint" Or filetype = "application/vnd.openxmlformats-officedocument.presentationml.presentation" Then
            source = "~/img/ppt.png"
        ElseIf filetype = "audio/mp3" Or filetype = "audio/mp4" Then
            source = "~/img/audio.png"
        ElseIf filetype = "video/mp4" Then
            source = "~/img/video.png"
        End If
        Return source
    End Function

  
End Class
