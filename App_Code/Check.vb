Imports Microsoft.VisualBasic

Public Class CheckUser
    Inherits ds_functions

    Public Function Check_Admin(usertype As ArrayList, usertypes As String) As Boolean
        If usertypes = Nothing Then Return False
        Dim thisarray As New ArrayList
        thisarray = CType(usertype, ArrayList)
        For Each item As String In thisarray
            If item = "admin" Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function Check_lib(usertype As ArrayList, usertypes As String) As Boolean
        If usertypes = Nothing Then Return False
        Dim thisarray As New ArrayList
        thisarray = CType(usertype, ArrayList)
        For Each item As String In thisarray
            If item = "lib" Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function Check_Staff(usertype As ArrayList, usertypes As String) As Boolean
        If usertypes = Nothing Then Return False
        If usertype.Count = 0 Then Return False
        Dim thisarray As New ArrayList
        thisarray = CType(usertype, ArrayList)
        For Each item As String In thisarray
            If item = "staff" Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function Check_Class(usertype As ArrayList, usertypes As String) As Boolean
        If usertypes = Nothing Then Return False
        If usertype.Count = 0 Then Return False
        Dim thisarray As New ArrayList
        thisarray = CType(usertype, ArrayList)
        For Each item As String In thisarray
            If item = "classteacher" Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function Check_Subject(usertype As ArrayList, usertypes As String) As Boolean
        If usertypes = Nothing Then Return False
        If usertype.Count = 0 Then Return False
        Dim thisarray As New ArrayList
        thisarray = CType(usertype, ArrayList)
        For Each item As String In thisarray
            If item = "subjectteacher" Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function Check_dh(usertype As ArrayList, usertypes As String) As Boolean
        If usertypes = Nothing Then Return False
        If usertype.Count = 0 Then Return False
        Dim thisarray As New ArrayList
        thisarray = CType(usertype, ArrayList)
        For Each item As String In thisarray
            If item = "depthead" Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function Check_sh(usertype As ArrayList, usertypes As String) As Boolean
        If usertypes = Nothing Then Return False
        If usertype.Count = 0 Then Return False
        Dim thisarray As New ArrayList
        thisarray = CType(usertype, ArrayList)
        For Each item As String In thisarray
            If item = "schoolhead" Then
                Return True
            End If
        Next
        Return False
    End Function

    Public Function Check_Parent(usertype As ArrayList, usertypes As String) As Boolean
        If usertypes = Nothing Then Return False
        Dim thisarray As New ArrayList
        thisarray = CType(usertype, ArrayList)
        For Each item As String In thisarray
            If item = "parent" Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function Check_Student(usertype As ArrayList, usertypes As String) As Boolean
        If usertypes = Nothing Then Return False
        Dim thisarray As New ArrayList
        thisarray = CType(usertype, ArrayList)
        For Each item As String In thisarray
            If item = "student" Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function Check_Account(usertype As ArrayList, usertypes As String) As Boolean
        If usertypes = Nothing Then Return False
        Dim thisarray As New ArrayList
        thisarray = CType(usertype, ArrayList)
        For Each item As String In thisarray
            If item = "account" Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function Check_oh(usertype As ArrayList, usertypes As String) As Boolean
        If usertypes = Nothing Then Return False
        Dim thisarray As New ArrayList
        thisarray = CType(usertype, ArrayList)
        For Each item As String In thisarray
            If item = "prop" Then
                Return True
            End If
        Next
        Return False
    End Function


    Public Function Check_app_staff(username As String, password As String) As ArrayList
        Dim userdetails As New ArrayList
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            If con.State = Data.ConnectionState.Closed Then                con.Open()

                Dim classes1 As New MySql.Data.MySqlClient.MySqlCommand("Select * from staffprofile where staffid = '" & username & "' and password = '" & password & "'", con)
                Dim no As MySql.Data.MySqlClient.MySqlDataReader = classes1.ExecuteReader
                If no.Read() Then
                    userdetails.Add(username)
                End If
                no.Close()
                Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("SELECT * FROM Session Order by ID Desc", con)
                Dim reader2 As MySql.Data.MySqlClient.MySqlDataReader = cmdInsert.ExecuteReader
                reader2.Read()
                userdetails.Add(reader2(0))
                reader2.Close()
            End If
            con.Close()        End Using
        Return userdetails
    End Function
End Class
