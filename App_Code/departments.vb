Imports Microsoft.VisualBasic

Public Class departments
    Inherits DB_Interface

    Public Function Get_subordinates(ByVal dept As String) As ArrayList
        Dim subo As New ArrayList
        Dim q As ArrayList = Select_1D("SELECT dept from depts where superior = '" & dept & "'")
        For Each j In q
            subo.Add(j)
        Next
        Return subo
    End Function

    Public Function Check_DH_Role(staffid As String) As String
        Return Select_single("SELECT  dept from depts where depts.head = '" & staffid & "'")
    End Function

    Public Function Check_Dept(staffid As String) As String
        Return Select_single("SELECT depts.dept from depts inner join staffdept on staffdept.dept = depts.Id where staffdept.staff = '" & staffid & "'")
    End Function

    Public Function Get_Dept_Staff(dept As String) As ArrayList
        Dim deptscontroled As New ArrayList
        deptscontroled.Add(dept)
        Dim secsub As New ArrayList
        Dim mysub As New ArrayList
       
        mysub = Get_subordinates(dept)

        For Each subitem As String In mysub
            deptscontroled.Add(subitem)
            secsub.Add(subitem)
        Next
        Dim thirdsub As New ArrayList
        For Each item As String In secsub
            Dim m As ArrayList = Select_1D("Select depts.dept, depts.headtitle from class inner join depts on depts.id = class.superior where depts.dept = '" & item & "'")
            If m.Count <> 0 Then deptscontroled.Add(m(0))
            mysub = Get_subordinates(item)
            For Each subitem As String In mysub
                deptscontroled.Add(subitem)
                thirdsub.Add(subitem)
            Next
        Next
        Dim fourthsub As New ArrayList
        For Each item As String In thirdsub
            Dim m As ArrayList = Select_1D("Select depts.dept, depts.headtitle from class inner join depts on depts.id = class.superior where depts.dept = '" & item & "'")
            If m.Count <> 0 Then deptscontroled.Add(m(0))
            mysub = Get_subordinates(item)
            For Each subitem As String In mysub
                deptscontroled.Add(subitem)
                fourthsub.Add(subitem)
            Next
        Next
        Dim fifthsub As New ArrayList
        For Each item As String In fourthsub
            Dim m As ArrayList = Select_1D("Select depts.dept, depts.headtitle from class inner join depts on depts.id = class.superior where depts.dept = '" & item & "'")
            If m.Count <> 0 Then deptscontroled.Add(m.Item(0))
            mysub = Get_subordinates(item)
            For Each subitem As String In mysub
                deptscontroled.Add(subitem)
            Next
        Next
        Dim stafflist As New ArrayList
        For Each item As String In deptscontroled
            If Select_single("SELECT staffprofile.surname from depts inner join staffprofile on staffprofile.staffId = depts.head where depts.dept = '" & item & "'") <> "" Then stafflist.Add(Select_single("SELECT staffprofile.surname from depts inner join staffprofile on staffprofile.staffId = depts.head where depts.dept = '" & item & "'"))
            If Select_single("SELECT staffprofile.surname from staffdept inner join staffprofile on staffprofile.staffid = staffdept.staff inner join depts on depts.id = staffdept.dept where depts.dept = '" & item & "'") <> "" Then stafflist.Add(Select_single("SELECT staffprofile.surname from staffdept inner join staffprofile on staffprofile.staffid = staffdept.staff inner join depts on depts.id = staffdept.dept where depts.dept = '" & item & "'"))
        Next
        Return stafflist
    End Function

    Public Function Get_Superiors(staff As String) As ArrayList
        Dim sups As New ArrayList
        Dim x As Array = Select_Query("Select id, dept, superior from depts where head = '" & staff & "'")
        Dim classgroups As New ArrayList
        Dim mydept As New ArrayList
        Dim superior As String = ""
        Dim g As Integer
        For j = 0 To x.GetLength(1) - 2
            If g = 0 Then
                superior = x(2, j)
            End If
            mydept.Add(x(1, j))
            g = g + 1
        Next

        If superior <> "None" And superior <> "" Then
            Dim q As ArrayList = Select_1D("Select headtitle, dept from depts where dept = '" & superior & "'")
            sups.Add(q(0).ToString.ToUpper & " - " & q(1).ToString.ToUpper)

        ElseIf superior = "" Then
            Dim dept As New ArrayList
            Dim r As ArrayList = Select_1D("SELECT depts.dept from staffdept inner join depts on depts.id = staffdept.dept where staffdept.staff = '" & staff & "'")
            For Each dp In r
                dept.Add(r(0))
            Next
            For Each item As String In dept
                Dim ks As Array = Select_Query("Select headtitle, dept from depts where dept = '" & item & "'")
                For g = 0 To ks.GetLength(1) - 2
                    sups.Add(ks(0, g).ToString.ToUpper & " - " & ks(1, g).ToString.ToUpper)
                Next
            Next
        ElseIf superior = "None" Then
            sups.Add("None")
        End If
        Dim ss As Array = Select_Query("select depts.headtitle, depts.dept from class inner join depts on class.superior = depts.id inner join classteacher on classteacher.class = class.id where classteacher.teacher = '" & staff & "'")
        For o = 0 To ss.GetLength(1) - 2
            If Not sups.Contains(ss(0, o) & " - " & ss(1, o)) Then sups.Add(ss(0, o) & " - " & ss(1, o))
        Next
        Return sups
    End Function

    Public Function Dept_Parents(dept As String, session As Integer) As ArrayList
        Return Select_1D("Select parentprofile.parentname, depts.headtitle, depts.dept from studentsummary inner join (class inner join depts on depts.Id = class.superior) on studentsummary.class = class.Id inner join (studentsprofile inner join (parentward inner join parentprofile on parentprofile.parentId = parentward.parent) on studentsprofile.admno = parentward.ward) on studentsprofile.admno = studentsummary.student where depts.dept = '" & dept & "' and studentsummary.session = '" & session & "'")
    End Function

    Public Function Get_Classes_Controlled(staffid As String) As ArrayList
        Dim x As Array = Select_Query("Select id, dept, superior from depts where head = '" & staffid & "'")
        Dim classgroups As New ArrayList
        Dim mydept As New ArrayList
        For j = 0 To x.GetLength(1) - 2
            mydept.Add(x(1, j))
        Next

        Dim titles As New ArrayList
        Dim deptscontroled As New ArrayList
        Dim secsub As New ArrayList
        Dim mysub As New ArrayList
        For Each item As String In mydept
            Dim l As ArrayList = Select_1D("Select depts.dept, depts.headtitle from class inner join depts on depts.id = class.superior where depts.dept = '" & item & "'")
            For Each g As String In l
                If Not deptscontroled.Contains(g) Then deptscontroled.Add(g)
            Next

            mysub = Get_subordinates(item)

            For Each subitem As String In mysub
                deptscontroled.Add(subitem)
                secsub.Add(subitem)
            Next
        Next
        Dim thirdsub As New ArrayList
        For Each item As String In secsub
            Dim l As ArrayList = Select_1D("Select headtitle from depts where dept = '" & item & "'")
            titles.Add(l(0))
            Dim m As ArrayList = Select_1D("Select depts.dept, depts.headtitle from class inner join depts on depts.id = class.superior where depts.dept = '" & item & "'")
            If m.Count <> 0 Then deptscontroled.Add(m(0))
            mysub = Get_subordinates(item)
            For Each subitem As String In mysub
                deptscontroled.Add(subitem)
                thirdsub.Add(subitem)
            Next
        Next
        Dim fourthsub As New ArrayList
        For Each item As String In thirdsub
            Dim l As ArrayList = Select_1D("Select headtitle from depts where dept = '" & item & "'")
            titles.Add(l(0))
            Dim m As ArrayList = Select_1D("Select depts.dept, depts.headtitle from class inner join depts on depts.id = class.superior where depts.dept = '" & item & "'")
            If m.Count <> 0 Then deptscontroled.Add(m(0))
            mysub = Get_subordinates(item)
            For Each subitem As String In mysub
                deptscontroled.Add(subitem)
                fourthsub.Add(subitem)
            Next
        Next
        Dim fifthsub As New ArrayList
        For Each item As String In fourthsub
            Dim l As ArrayList = Select_1D("Select headtitle from depts where dept = '" & item & "'")
            titles.Add(l(0))
            Dim m As ArrayList = Select_1D("Select depts.dept, depts.headtitle from class inner join depts on depts.id = class.superior where depts.dept = '" & item & "'")
            If m.Count <> 0 Then deptscontroled.Add(m.Item(0))
            mysub = Get_subordinates(item)
            For Each subitem As String In mysub
                deptscontroled.Add(subitem)
            Next
        Next
        Dim classcontrolled As New ArrayList

        For Each item As String In deptscontroled
            Dim f As Boolean = False
            Dim c As ArrayList = Select_1D("Select class.class from class inner join depts on class.superior = depts.Id where depts.dept = '" & item & "'")
            For Each y In c
                f = True
                If Not classcontrolled.Contains(y) Then classcontrolled.Add(y)
            Next
            If f = True Then
                If Not classgroups.Contains(item) Then classgroups.Add(item)
            End If
        Next
        Dim myclasses As New ArrayList
        Dim z As ArrayList = Select_1D("Select class.class from classteacher inner join class on class.Id = classteacher.class where classteacher.teacher = '" & staffid & "'")
        For Each g In z
            myclasses.Add(g)
        Next
        Dim all As New ArrayList
        all.Add(deptscontroled)
        all.Add(classcontrolled)
        all.Add(classgroups)
        all.Add(myclasses)
        Return all
    End Function

End Class
