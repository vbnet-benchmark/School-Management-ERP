Imports Microsoft.VisualBasic

Public Class Lesson_Plan
    Inherits DB_Interface
    Private Id As Integer

    Public Sub New()
        Non_Query("CREATE TABLE If NOT Exists `lessontemplates` (`name` varchar(100) DEFAULT NULL," & _
        "`template` text NOT NULL," & _
 "`id` int(11) NOT NULL AUTO_INCREMENT," & _
 "PRIMARY KEY (`id`) USING BTREE" & _
      ") ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1")
        Dim x As New MySql.Data.MySqlClient.MySqlConnectionStringBuilder(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
        If Select_single("Select count(*) from information_schema.columns where table_schema = '" & x.Database & "' and column_name = 'lessonplan' and table_name = 'class'") = 0 Then
            Non_Query("Alter Table `class` add `lessonplan` int(11) NOT NULL")
        End If
    End Sub

    Public Property Plan_Id() As Integer
        Get
            Return Id
        End Get
        Set(value As Integer)
            Id = value
        End Set
    End Property

    Public Function Load_Templates() As Array
        Return Select_Query("select id, name from lessontemplates")
    End Function

    Public Function Delete_Template(name As String) As Boolean
        Return Non_Query("delete from lessontemplates where name = '" & name & "'")
    End Function

    Public Function Search_Template(name As String) As Array
        Return Select_Query("select id, name from lessontemplates where name like '%" & name & "%'")
    End Function
    Public Function Check_Template(name As String) As Boolean
        If Select_single("select name from lessontemplates where name = '" & name & "'") <> Nothing Then
            Return True
        Else
            Return False
        End If
    End Function

    Public Function View_Template() As ArrayList
        Return Select_1D("select name, template from lessontemplates where id = '" & Id & "'")
    End Function

    Public Sub Get_Class_Template(clas As String)
        Id = Select_single("select lessonplan from class where class = '" & clas & "'")
    End Sub

    Public Sub New_Template(name As String, template As String)
        Dim a As Boolean = Non_Query("Insert into lessontemplates (name, template) values ('" & name & "', '" & template & "')")
        Id = Select_single("select id from lessontemplates where name = '" & name & "'")
    End Sub

    Public Sub Register_Classes(classes As ArrayList)
        For Each v In classes
            Non_Query("Update class set lessonplan = '" & Id & "' where class = '" & v & "'")
        Next
    End Sub
    Public Sub DeRegister_Classes(classes As ArrayList)
        For Each v In classes
            Non_Query("Update class set lessonplan = '" & 0 & "' where class = '" & v & "'")
        Next
    End Sub

    Public Sub Edit_Template(name As String, template As String)
        Non_Query("update lessontemplates set name = '" & name & "',  template = '" & template & "' where id = '" & Id & "'")
    End Sub

   

    Public Sub Select_Classes(checkboxlist2 As CheckBoxList)
        checkboxlist2.Items.Clear()
        Dim x As Integer
        For Each c In Select_1D("SELECT class From Class")
            checkboxlist2.Items.Add(c)
            Dim a As Integer = Select_single("select lessonplan from class where class = '" & c & "'")
            If a = Id And Id <> 0 Then checkboxlist2.Items(x).Selected = True
            x += 1
        Next
    End Sub

    Public Function Get_Js(hidden1 As HiddenField) As String
        Return "<script src='../js/summernote.min.js'></script>" & _
    "<script src='../js/summernote-active.js'></script>" & _
"<script>" & _
        "$(document).ready(function () {" & _
           "document.getElementById('mess2').innerHTML = document.getElementById('" & hidden1.ClientID & "').value;" & _
        "});" & _
    "</script>" & _
    "<script>" & _
"function store() {" & _
             "document.getElementById('" & hidden1.ClientID & "').value = document.getElementById('mess2').innerHTML;" & _
              "}" & _
         "</script>"
    End Function
End Class

