Imports Microsoft.VisualBasic

Public Class Gen_Functions



    Public Function Check_Drop_Down(dropdown As DropDownList, item As String) As Boolean
        For Each i As ListItem In dropdown.Items
            If i.Text = item Then Return True
        Next
        Return False
    End Function

   
End Class
