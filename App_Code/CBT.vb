Imports Microsoft.VisualBasic
Imports System.Text
Imports System.Configuration
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.IO
Imports System.Net
Imports System.Web
Imports System.Net.Mail
Imports System.Resources
Public Class CBT
    Inherits DB_Interface
    Dim ds As New ds_functions
    Public Sub New()
        Non_Query("CREATE TABLE If NOT Exists `tests` (`title` varchar(100) DEFAULT NULL," & _
        "`teacher` varchar(100) DEFAULT NULL," & _
        "`subject` int(11) DEFAULT NULL," & _
         "`class` int(11) DEFAULT NULL," & _
        "`validate` varchar(100) DEFAULT 'No'," & _
        "`date` datetime DEFAULT NULL," & _
        "`duration` varchar(100) DEFAULT NULL," & _
         "`questionno` int(11) DEFAULT NULL," & _
         "`totalmarks` int(11) DEFAULT NULL," & _
         "`session` int(11) DEFAULT NULL," & _
 "`id` int(11) NOT NULL AUTO_INCREMENT," & _
 "PRIMARY KEY (`id`) USING BTREE" & _
      ") ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1")

        Non_Query("CREATE TABLE If NOT Exists `questions` (`testid` int(11) DEFAULT NULL," & _
       "`qno` int(11) DEFAULT NULL," & _
       "`question` text DEFAULT NULL," & _
       "`opta` varchar(100) DEFAULT NULL," & _
       "`optb` varchar(100) DEFAULT NULL," & _
        "`optc` varchar(100) DEFAULT NULL," & _
       "`optd` varchar(100) DEFAULT NULL," & _
      "`correct` varchar(100) DEFAULT NULL," & _
"`id` int(11) NOT NULL AUTO_INCREMENT," & _
"PRIMARY KEY (`id`) USING BTREE" & _
     ") ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1")

        Non_Query("CREATE TABLE If NOT Exists `studenttest` (`testid` int(11) DEFAULT NULL," & _
      "`qid` int(11) DEFAULT NULL," & _
      "`timeelapsed` varchar(100) DEFAULT NULL," & _
       "`admno` varchar(100) DEFAULT NULL," & _
      "`answer` varchar(100) DEFAULT NULL," & _
        "`marks` int(11) DEFAULT NULL," & _
        "`session` int(11) DEFAULT NULL," & _
        "`finished` varchar(100) DEFAULT 'No'," & _
     "`id` int(11) NOT NULL AUTO_INCREMENT," & _
"PRIMARY KEY (`id`) USING BTREE" & _
    ") ENGINE=InnoDB AUTO_INCREMENT=1 DEFAULT CHARSET=latin1")
    End Sub

    Private Sub Question_PlaceHolder(plc As PlaceHolder, qno As Integer, question As String, opta As String, optb As String, optc As String, optd As String, correct As String, answerred As String)
        Dim beginl As New Literal
        beginl.Text = "<div class='form-group-inner'><div class='row'><div class='col-lg-12'>"
        Dim l As New Label
        l.Text = qno & ". " & question
        Dim beginl2 As New Literal
        beginl2.Text = "</div></div></div><div class='form-group-inner'><div class='row'><div class='col-lg-12'>"
        Dim r As New RadioButtonList
        r.Enabled = False
        r.Items.Add("A.  " & opta)
        r.Items.Add("B.  " & optb)
        r.Items.Add("C.  " & optc)
        r.Items.Add("D.  " & optd)
        Dim j As Integer = 0
        For Each s As ListItem In r.Items
            Select Case j
                Case 0
                    If answerred = "A" Then
                        s.Selected = True
                        If correct <> "A" Then s.Text += "   ❌"
                    End If
                    If correct = "A" Then s.Text += "   ✅"
                Case 1
                    If answerred = "B" Then
                        s.Selected = True
                        If correct <> "B" Then s.Text += "   ❌"
                    End If
                    If correct = "B" Then s.Text += "   ✅"

                Case 2
                    If answerred = "C" Then
                        s.Selected = True
                        If correct <> "C" Then s.Text += "   ❌"
                    End If
                    If correct = "C" Then s.Text += "   ✅"

                Case 3
                    If answerred = "D" Then
                        s.Selected = True
                        If correct <> "D" Then s.Text += "   ❌"
                    End If
                    If correct = "D" Then s.Text += "   ✅"
            End Select
            j += 1
        Next
        Dim finall As New Literal
        finall.Text = "</div></div></div>"
        plc.Controls.Add(beginl)
        plc.Controls.Add(l)
        plc.Controls.Add(beginl2)
        plc.Controls.Add(r)
        plc.Controls.Add(finall)
    End Sub
    Public Sub New_Test(title As String, teacher As String, subject As Integer, clas As Integer, duration As String, qno As Integer, totalmarks As Integer, session As Integer)
        Non_Query("insert into tests (title, teacher, subject, class, duration, questionno, totalmarks, session, date) values ('" & title & "', '" & teacher & "', '" & subject & "', '" & clas & "', '" & duration & "', '" & qno & "', '" & totalmarks & "', '" & session & "', '" & ToSQL(Now) & "')")
    End Sub

    Public Sub Edit_Test(title As String, teacher As String, subject As Integer, clas As Integer, duration As String, qno As Integer, totalmarks As Integer, id As Integer)
        Non_Query("Update tests set title = '" & title & "', teacher = '" & teacher & "', subject = '" & subject & "', class = '" & clas & "', duration = '" & duration & "', questionno = '" & qno & "', totalmarks = '" & totalmarks & "' where id = '" & id & "'")
    End Sub
    Public Sub Validate_Test(id As Integer)
        Non_Query("update tests set validate = 'Yes' where id = '" & id & "'")
    End Sub
    Public Sub InValidate_Test(id As Integer)
        Non_Query("update tests set validate = 'No' where id = '" & id & "'")
    End Sub
    Public Function Check_Validity(id As Integer) As Boolean
        If Select_single("select validate from tests where id = '" & id & "'") = "No" Then
            Return False
        Else
            Return True

        End If
    End Function
    Public Function Check_Test_Aailable(title As String, session As Integer) As Boolean
        If Select_single("select count(*) from tests where title = '" & title & "' and session = '" & session & "'") <> 0 Then
            Return False
        Else
            Return True
        End If
    End Function
    Public Sub Delete_Tests(title As String, session As Integer)
        Non_Query("delete from tests where title = '" & title & "' and session ='" & session & "'")
    End Sub
    Public Function Teacher_Tests_List(teacher As String, session As Integer) As Array
        Return Select_Query("select tests.id, tests.date,  tests.title, subjects.subject, class.class, tests.validate from tests inner join subjects on tests.subject = subjects.id inner join class on class.id = tests.class where tests.teacher = '" & teacher & "' and tests.session = '" & session & "' order by id desc")
    End Function
    Public Function Student_Tests_List(admno As String, session As Integer, clas As Integer) As Array
        Return Select_Query("select tests.id, tests.date,  tests.title, subjects.subject, class.class, tests.duration from tests inner join subjects on tests.subject = subjects.id inner join class on class.id = tests.class inner join subjectreg on tests.subject = subjectreg.subjectsofferred and tests.session = subjectreg.session and subjectreg.class = tests.class where subjectreg.student = '" & admno & "' and tests.session = '" & session & "' and tests.class = '" & clas & "' and validate = 'Yes' order by tests.id desc")
    End Function

    Public Function Check_Written(testname As String, session As Integer) As Boolean
        If Select_single("select count(*) from studenttest inner join tests on tests.id = studenttest.testid where tests.Title = '" & testname & "' and tests.session = '" & session & "'") <> 0 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function Test_Details(id As Integer) As ArrayList
        Return Select_1D("select tests.title, subjects.subject, class.class, tests.duration, tests.questionno, tests.totalmarks, tests.validate from tests inner join subjects on tests.subject = subjects.id inner join class on class.id = tests.class where tests.id = '" & id & "'")
    End Function

    Public Function Next_Question_No(test As Integer) As Integer
        For i = 1 To Val(Select_single("select questionno from tests where id = '" & test & "'"))
            If Select_single("select count(*) from questions where testid = '" & test & "' and qno = '" & i & "'") = 0 Then Return i
        Next
        Return 0
    End Function

    Public Function Total_Question_No(test As Integer) As Integer
        Return Val(Select_single("select questionno from tests where id = '" & test & "'"))
    End Function

    Public Sub New_Question(test As Integer, qno As Integer, question As String, opta As String, optb As String, optc As String, optd As String, correct As String)
        Non_Query("insert into questions (testid, qno, question, opta, optb, optc, optd, correct) values ('" & test & "', '" & qno & "', '" & question & "', '" & opta & "', '" & optb & "', '" & optc & "', '" & optd & "', '" & correct & "')")
    End Sub
    Public Sub Delete_Question(id As Integer, testid As Integer)
        Non_Query("delete from questions where qno = '" & id & "' and testid = '" & testid & "'")
    End Sub
    Public Sub Edit_Question(test As Integer, qno As Integer, question As String, opta As String, optb As String, optc As String, optd As String, correct As String, id As Integer)
        Non_Query("update questions set testid = '" & test & "', question = '" & question & "', opta = '" & opta & "', optb = '" & optb & "', optc = '" & optc & "', optd = '" & optd & "', correct = '" & correct & "' where id = '" & id & "'")
    End Sub

    Public Function Question_List(test As Integer) As Array
        Return Select_Query("select id, qno, question from questions where testid = '" & test & "' order by qno")
    End Function

    Public Function Question_Details(qid As Integer) As ArrayList
        Return Select_1D("Select * from questions where id = '" & qid & "'")
    End Function

    Public Function Compare_Question_No(testid As Integer) As Boolean
        If Select_single("select questionno from tests where id = '" & testid & "'") = Select_single("select count(*) from questions where testid = '" & testid & "'") Then
            Return True
        Else
            Return False
        End If
    End Function

    Private Function Mark_Answer(qid As Integer, answer As String, testid As Integer) As Integer
        If Select_single("select correct from questions where id = '" & qid & "'") = answer Then
            Return FormatNumber(Val(Select_single("select totalmarks from tests where id = '" & testid & "'")) / Val(Select_single("select questionno from tests where id = '" & testid & "'")), 2, , , )
        Else
            Return 0
        End If
    End Function
    Public Sub Submit_Answer(admno As String, answer As String, testid As Integer, elapsed As String, qid As Integer, session As Integer)
        If answer <> "" Then Non_Query("insert into studenttest (testid, qid, timeelapsed, answer, marks, admno, session) values ('" & testid & "', '" & qid & "', '" & elapsed & "', '" & answer & "', '" & Mark_Answer(qid, answer, testid) & "', '" & admno & "', '" & session & "')")
    End Sub

    Public Function Start_Test(admno As String, testid As Integer, session As Integer) As ArrayList
        Dim duration As Integer
        Dim elapsed As Double
        Dim a As New ArrayList
        If Select_single("select count(*) from studenttest where admno = '" & admno & "' and session = '" & session & "' and testid = '" & testid & "'") <> 0 Then
            If Check_Finish(testid, admno) = False Then
                duration = Val(Select_single("select duration from tests where id = '" & testid & "'"))
                elapsed = Val(Select_single("select timeelapsed from studenttest where admno = '" & admno & "' and testid = '" & testid & "' order by id desc"))
                a.Add(Select_1D("select * from questions where testid = '" & testid & "' and qno > '" & Select_single("select questions.qno from studenttest inner join questions on questions.id = studenttest.qid where studenttest.admno = '" & admno & "' and studenttest.testid = '" & testid & "' order by questions.qno desc LIMIT 1") & "' order by qno LIMIT 1"))
                a.Add(Display_Time(Time_Left(elapsed, testid)))
                a.Add(duration)
                If a(0)(8) = Select_single("select id from questions where testid = '" & testid & "' order by qno desc LIMIT 1") Then
                    a.Add(True)
                Else
                    a.Add(False)
                End If
            End If
        Else
            a.Add(Select_1D("select * from questions where testid = '" & testid & "' and qno = '1'"))
            a.Add(Display_Time(Time_Left(0, testid)))
            a.Add(Val(Select_single("select duration from tests where id = '" & testid & "'")))
            If a(0)(8) = Select_single("select id from questions where testid = '" & testid & "' order by qno desc LIMIT 1") Then
                a.Add(True)
            Else
                a.Add(False)
            End If
        End If
        Return a
    End Function

    Public Function Display_Time(minutes As Double) As String
        If minutes = 0 Then Return "00:00:00"
        Dim hrs As Integer = minutes \ 60
        Dim minsfirst As Double = minutes Mod 60
        Dim mins As Integer = minsfirst
        Dim secs As Integer = 0
        Try
            Dim a As Array = Split(minsfirst, ".")
            mins = Val(a(0))
            secs = Val("0." & a(1)) * 60
        Catch
        End Try
        Dim s As String = IIf(hrs <= 9, "0" & hrs, hrs) & ":" & IIf(mins <= 9, "0" & mins, mins) & ":" & IIf(secs <= 9, "0" & secs, secs)
        Return s
    End Function

    Public Function Convert_Time(time As String) As Double
        Dim a As Array = Split(time, ":")
        Return (Val(a(2)) / 60) + Val(a(1)) + (Val(a(0)) * 60)
    End Function

    Public Function Submitted(admno As String, testid As Integer) As Boolean
        If Select_single("select finished from studenttest where admno = '" & admno & "' and testid = '" & testid & "'") = "Yes" Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function Time_Left(time As Double, testid As Integer) As Double
        Dim a As Double = Select_single("select duration from tests where id = '" & testid & "'")
        Return a - time
    End Function
    Public Function Check_Finish(testid As Integer, admno As String) As Boolean
        If Select_single("select questions.qno from studenttest inner join questions on questions.id = studenttest.qid where studenttest.admno = '" & admno & "' and studenttest.testid = '" & testid & "' order by questions.qno desc LIMIT 1") <> Select_single("select qno from questions where testid = '" & testid & "' order by qno desc") Then
            Return False
        Else
            Return True
        End If
    End Function

   
    Public Function Next_Question(testid As Integer, admno As String, session As Integer) As ArrayList
        Dim a As ArrayList
        Dim b As New ArrayList
        If Check_Finish(testid, admno) = False Then
            a = Select_1D("select * from questions where testid = '" & testid & "' and qno > '" & Select_single("select questions.qno from studenttest inner join questions on questions.id = studenttest.qid where studenttest.admno = '" & admno & "' and studenttest.testid = '" & testid & "' order by questions.qno desc LIMIT 1") & "' order by qno LIMIT 1")
            b.Add(a)
            If a(8) = Select_single("select id from questions where testid = '" & testid & "' order by qno desc LIMIT 1") Then
                b.Add(True)
            Else
                b.Add(False)
            End If
        End If
        Return b
    End Function
    Public Function Check_Unanswerred(admno As String, testid As Integer) As Boolean
        If Unanswerred_list(admno, testid).Count > 1 Then
            Return True
        Else
            Return False
        End If
    End Function
    Public Function Next_Unanswerred(admno As String, testid As Integer) As ArrayList
        Dim z As ArrayList = Unanswerred_list(admno, testid)
        Dim b As New ArrayList
        b.Add(Select_1D("select * from questions where testid = '" & testid & "' and id = '" & z(0) & "' order by id"))
        If Unanswerred_list(admno, testid).Count = 1 Then
            b.Add(True)
        Else
            b.Add(False)
        End If
        Return b
    End Function
    Public Function Unanswerred_list(admno As String, testid As Integer) As ArrayList
        Dim a As ArrayList = Select_1D("select id from questions where testid = '" & testid & "' order by qno")
        Dim b As ArrayList = Select_1D("select qid from studenttest where admno = '" & admno & "' and testid = '" & testid & "'")
        Dim c As New ArrayList
        For Each s In a
            If Not b.Contains(s) Then c.Add(s)
        Next
        Return c
    End Function
    Public Sub Finished_Test(admno As String, testid As Integer)
        Non_Query("Update studenttest set finished = 'Yes' where admno = '" & admno & "' and testid = '" & testid & "'")
        Dim f As ArrayList = Unanswerred_list(admno, testid)
        For Each d In f
            Dim j As ArrayList = Next_Unanswerred(admno, testid)
            Submit_Answer(admno, "-", testid, "0", j(0)(8), Select_single("select session from tests where id = '" & testid & "'"))
        Next
    End Sub
    Public Function Get_Test_Name(testid As Integer) As String
        Return Select_single("select title from tests where id = '" & testid & "'")
    End Function

    Public Function Check_Total_questions(testid As Integer) As Integer
        Return Select_single("select questionno from tests where id = '" & testid & "'")
    End Function
    Public Function Student_Test_Summary(admno As String, testid As String) As ArrayList
        Dim pass As String = Select_single("select passport from studentsprofile where admno = '" & admno & "'").ToString
        Dim j As Double = Time_Left(Select_single("select timeelapsed from studenttest where admno = '" & admno & "' and testid = '" & testid & "' order by id desc LIMIT 1"), testid)
        Dim secs As Integer = 0
        Dim mins As Integer = j
        Try
            Dim a As Array = Split(j Mod 60, ".")
            mins = a(0)
            secs = Val("0." & a(1)) * 60
        Catch
        End Try
        Dim timeelapsed As String = IIf(j > 59.9, j \ 60 & " Hrs " & mins & " Mins " & secs & " Secs", IIf(j >= 1, mins & " Mins " & secs & " Secs ", secs & " Secs"))
        Dim totalmarks As Double
        Dim c As ArrayList = Select_1D("select marks from studenttest where admno = '" & admno & "' and testid = '" & testid & "'")
        For Each g In c
            totalmarks += g
        Next
        Dim percentage As Double = (totalmarks / Select_single("select totalmarks from tests where id = '" & testid & "'")) * 100
        Dim nopassed As Integer
        Dim d As ArrayList = Select_1D("select marks from studenttest where admno = '" & admno & "' and testid = '" & testid & "' and marks <> '0'")
        For Each g In d
            nopassed += 1
        Next
        Dim nofailed As Integer
        Dim e As ArrayList = Select_1D("select marks from studenttest where admno = '" & admno & "' and testid = '" & testid & "' and marks = '0' and answer <> '-'")
        For Each g In e
            nofailed += 1
        Next
        Dim nounanswerred As Integer = Select_single("select count(*) from studenttest where answer = '-' and admno = '" & admno & "' and testid = '" & testid & "'")
        Dim sum As New ArrayList
        sum.Add(timeelapsed)
        sum.Add(totalmarks & " / " & Select_single("select totalmarks from tests where id = '" & testid & "'"))
        sum.Add(nopassed)
        sum.Add(nofailed)
        sum.Add(nounanswerred)
        sum.Add(percentage & "%")
        sum.Add(pass)
        Return sum
    End Function
    Public Sub Review_Questions(plc As PlaceHolder, admno As String, testid As Integer)
        Dim x As Array = Select_Query("select questions.qno, questions.question, questions.opta, questions.optb, questions.optc, questions.optd, questions.correct, studenttest.answer from questions inner join studenttest on studenttest.qid = questions.id where questions.testid = '" & testid & "' and studenttest.admno = '" & admno & "'")
        For j = 0 To x.GetLength(1) - 2
            Question_PlaceHolder(plc, x(0, j), x(1, j), x(2, j), x(3, j), x(4, j), x(5, j), x(6, j), x(7, j))
        Next
    End Sub
    Public Function Students_Scores(testid As String) As ArrayList
        Dim overall As New ArrayList
        Dim sco As New ArrayList
        Dim percent As New ArrayList
        Dim s As ArrayList = Select_1D("select admno from studenttest where testid = '" & testid & "'")
        Dim gtotalmarks As Integer = Select_single("select totalmarks from tests where id = '" & testid & "'")
        Dim d As New ArrayList
        Dim l As New ArrayList
        For Each g In s
            If Not d.Contains(ds.Get_Stu_name(g)) Then d.Add(ds.Get_Stu_name(g))
            If Not l.Contains(g) Then l.Add(g)
        Next
        For Each f In d
            Dim total As Double = 0
            Dim j As ArrayList = Select_1D("select marks from studenttest inner join studentsprofile on studentsprofile.admno = studenttest.admno where testid = '" & testid & "' and studentsprofile.surname = '" & f & "'")
            For Each n In j
                total += n
            Next
            sco.Add(total & " / " & gtotalmarks)
            Dim perc As Double = (total / gtotalmarks) * 100
            percent.Add(perc & "%")
        Next
        overall.Add(l)
        overall.Add(d)
        overall.Add(sco)
        overall.Add(percent)
        overall.Add(gtotalmarks)
        Return overall
    End Function
End Class
