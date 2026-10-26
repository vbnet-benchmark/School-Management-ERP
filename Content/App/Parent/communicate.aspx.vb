Imports System.Text
Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Data
Imports System.Web.UI.WebControls
Imports System.Globalization
Imports System.Media
Imports System.Threading

Partial Class Admin_studentprofile
    Inherits System.Web.UI.Page
    Dim pagebefore As String
    Dim studentId As Integer
    Dim termID As Integer
    Dim checkedSubjects As New ArrayList
    Dim uncheckedSubjects As New ArrayList

    Dim alert As New Literal
    Dim alertmsg As New Alerts
    Dim logify As New notify
    Dim check As New CheckUser
    Dim alertPLC As New PlaceHolder
    Dim par As New parentcheck
    Dim cultureinfo As CultureInfo = Thread.CurrentThread.CurrentCulture
    Dim textinfo As TextInfo = CultureInfo.TextInfo
    Dim firstname As Array
    Private Sub Show_Alert(type As Boolean, msg As String)
        alertPLC = Me.Master.FindControl("plcAlert")
        If type = True Then
            alert.Text = alertmsg.success_message(msg)
        Else
            alert.Text = alertmsg.error_message(msg)
        End If
        alertPLC.Controls.Add(alert)
    End Sub

    Protected Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init

        Dim x As New UpdateProgress
        x = Me.Master.FindControl("UpdateProgress1")
        x.AssociatedUpdatePanelID = "UpdatePan"
        x.Visible = False

        Dim c As New System.Web.UI.Timer
        c = Me.Master.FindControl("Timer1")
        c.Enabled = False
    End Sub
   
    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load
        If Session("staffid") = Nothing Then 
 Dim x As ArrayList = check.Check_app_staff(Request.QueryString("username"), Request.QueryString("password")) 
 If x.Count <> 0 Then 
 Session("staffid") = x.Item(0) 
 Session("sessionid") = x.Item(1) 
 End If 
 End If 
        Try

            If IsPostBack Then

                If Session("studentadd") <> Nothing And chatthread.Visible = True Then load_chats()
            Else
                Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                    con.Open()
                    Session("studentadd") = Nothing
                    Dim ds As New DataTable
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from parentward inner join studentsprofile on parentward.ward = studentsprofile.admno WHERE parentward.parent = ?", con)
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("parentID")))
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                    Do While student0.Read
                        ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                    Loop
                    student0.Close()
                    gridview1.DataSource = ds
                    gridview1.DataBind()
                    If Request.QueryString.ToString <> Nothing Then
                        Dim cmd1a As New MySql.Data.MySqlClient.MySqlCommand("SELECT student from communicate where id = '" & Request.QueryString.ToString & "'", con)
                        Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd1a.ExecuteReader
                        student0a.Read()
                        Session("studentadd") = student0a(0)
                        student0a.Close()
                        Dim cmd1as As New MySql.Data.MySqlClient.MySqlCommand("SELECT sex from studentsprofile where admno = '" & Session("Studentadd") & "'", con)
                        Dim student0as As MySql.Data.MySqlClient.MySqlDataReader = cmd1as.ExecuteReader
                        student0as.Read()
                        Dim sex As String = student0as(0)
                        student0as.Close()

                        firstname = Split(par.getstuname(Session("studentadd")), " ")
                        radParent.Items.Clear()
                        radParent.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " wear uniform?")
                        radParent.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " wear wrong uniform?")
                        radParent.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " comb " & IIf(sex = "Male", "his", "her") & " hair")
                        radParent.Items.Add("Was " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " neatly dressed?")
                        radParent.Items.Add("Are the nails of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " all trimmed?")

                        RadioButtonList1.Items.Clear()
                        RadioButtonList1.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " eat all " & IIf(sex = "Male", "his", "her") & " food?")


                        RadioButtonList2.Items.Clear()
                        RadioButtonList2.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " play outdoors?")
                        RadioButtonList2.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " play indoors?")
                        RadioButtonList2.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " play with " & IIf(sex = "Male", "his", "her") & " friends?")
                        RadioButtonList2.Items.Add("Was " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was rough with " & IIf(sex = "Male", "his", "her") & " friends.")
                        RadioButtonList2.Items.Add("Was " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " rough at play?")
                        RadioButtonList2.Items.Add("Was " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " gentle at play?")


                        RadioButtonList3.Items.Clear()
                        RadioButtonList3.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " sleep for a short time?")
                        RadioButtonList3.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " sleep at all?")
                        RadioButtonList3.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " sleep for a long time?")

                        RadioButtonList4.Items.Clear()
                        RadioButtonList4.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learn letter/phonics work?")
                        RadioButtonList4.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learn number work?")
                        RadioButtonList4.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learn rhymes?")
                        RadioButtonList4.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learn practical life experiences?")
                        RadioButtonList4.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learn sensarial education?")
                        RadioButtonList4.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learn drawing and colouring?")

                        RadioButtonList5.Items.Clear()
                        RadioButtonList5.Items.Add(textinfo.ToTitleCase("Was " & firstname(1).ToString.ToLower) & " late to school?")
                        RadioButtonList5.Items.Add(textinfo.ToTitleCase("Was " & firstname(1).ToString.ToLower) & " early to school?")
                        RadioButtonList5.Items.Add(textinfo.ToTitleCase("Did " & firstname(1).ToString.ToLower) & " cry when " & IIf(sex = "Male", "he", "she") & " was dropped in school?")
                        RadioButtonList5.Items.Add(textinfo.ToTitleCase("Did " & firstname(1).ToString.ToLower) & " fight in school today?")
                        RadioButtonList5.Items.Add(textinfo.ToTitleCase("Was " & firstname(1).ToString.ToLower) & " well behaved today?")
                        RadioButtonList5.Items.Add(textinfo.ToTitleCase("Did " & firstname(1).ToString.ToLower) & " pay attention in class?")
                        RadioButtonList5.Items.Add(textinfo.ToTitleCase("Was " & firstname(1).ToString.ToLower) & " present at the assembly?")

                        RadioButtonList6.Items.Clear()
                        RadioButtonList6.Items.Add(textinfo.ToTitleCase("Did " & firstname(1).ToString.ToLower) & " do " & IIf(sex = "Male", "his", "her") & " homework?")
                        RadioButtonList6.Items.Add("Was the home work of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " rough?")
                        RadioButtonList6.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " finish " & IIf(sex = "Male", "his", "her") & " homework?")
                        RadioButtonList6.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " bring " & IIf(sex = "Male", "his", "her") & " homework?")

                        RadioButtonList7.Items.Clear()
                        RadioButtonList7.Items.Add("Was " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " active in prayer?")
                        RadioButtonList7.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learn a new lesson today?")
                        RadioButtonList7.Items.Add("Was " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " playful at devotion?")
                        RadioButtonList7.Items.Add("Was " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " abe to recite " & IIf(sex = "Male", "his", "her") & "  memory verse?")

                        RadioButtonList8.Items.Clear()
                        RadioButtonList8.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learn letter/phonics work?")
                        RadioButtonList8.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learn number wor?.")
                        RadioButtonList8.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learn practical life experiences?")
                        RadioButtonList8.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learn sensarial education?")

                        RadioButtonList9.Items.Clear()
                        RadioButtonList9.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " play outdoors?")
                        RadioButtonList9.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " play indoors?")
                        RadioButtonList9.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " play with " & IIf(sex = "Male", "his", "her") & " friends?")
                        RadioButtonList9.Items.Add("Was " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was rough with " & IIf(sex = "Male", "his", "her") & " friends.")
                        RadioButtonList9.Items.Add("Was " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " rough at play?")
                        RadioButtonList9.Items.Add("Was " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " gentle at play?")

                        RadioButtonList10.Items.Clear()
                        RadioButtonList10.Items.Add("How was the attitude of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " today?")
                       
                        RadioButtonList11.Items.Clear()
                        RadioButtonList11.Items.Clear()
                        RadioButtonList11.Items.Add("Did " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " eat all " & IIf(sex = "Male", "his", "her") & " food?")

                        load_chats()
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)

                    End If
                    con.Close()
                End Using
            End If
            Dim emojis As New ArrayList
            emojis.Add("😀")
            emojis.Add("😁")
            emojis.Add("😂")
            emojis.Add("😃")
            emojis.Add("😄")
            emojis.Add("😅")
            emojis.Add("😆")
            emojis.Add("😇")
            emojis.Add("😈")
            emojis.Add("😉")
            emojis.Add("😊")
            emojis.Add("😋")
            emojis.Add("😌")
            emojis.Add("😍")
            emojis.Add("😎")
            emojis.Add("😏")
            emojis.Add("😐")
            emojis.Add("😑")
            emojis.Add("😒")
            emojis.Add("😓")
            emojis.Add("😔")
            emojis.Add("😕")
            emojis.Add("😖")
            emojis.Add("😗")
            emojis.Add("😘")
            emojis.Add("😙")
            emojis.Add("😚")
            emojis.Add("😛")
            emojis.Add("😜")
            emojis.Add("😝")
            emojis.Add("😞")
            emojis.Add("😟")
            emojis.Add("😠")
            emojis.Add("😡")
            emojis.Add("😢")
            emojis.Add("😣")
            emojis.Add("😤")
            emojis.Add("😥")
            emojis.Add("😦")
            emojis.Add("😧")
            emojis.Add("😨")
            emojis.Add("😩")
            emojis.Add("😪")
            emojis.Add("😫")
            emojis.Add("😬")
            emojis.Add("😭")
            emojis.Add("😮")
            emojis.Add("😯")
            emojis.Add("😰")
            emojis.Add("😱")
            emojis.Add("😲")
            emojis.Add("😳")
            emojis.Add("😴")
            emojis.Add("😵")
            emojis.Add("😶")
            emojis.Add("😷")
            emojis.Add("🙁")
            emojis.Add("🙂")
            emojis.Add("🙃")
            emojis.Add("🙄")
            emojis.Add("👆")
            emojis.Add("👈")
            emojis.Add("👉")
            emojis.Add("👊")
            emojis.Add("👋")
            emojis.Add("👌")
            emojis.Add("👍")
            emojis.Add("👎")
            emojis.Add("👏")
            emojis.Add("👐")
            emojis.Add("🖌")
            emojis.Add("🖍")
            emojis.Add("🖐")
            emojis.Add("🖕")
            emojis.Add("🙆")
            emojis.Add("🙇")
            emojis.Add("🙈")
            emojis.Add("🙊")
            emojis.Add("🙋")
            emojis.Add("🙌")
            emojis.Add("🙍")
            emojis.Add("🙎")
            emojis.Add("🙏")
            Dim emojidivs As String = "<table id='gghd' style='width:100%;'><tr>"
            Dim counter As Integer = 1
            For Each item In emojis

                emojidivs = emojidivs + "<td id ='" & "tdtd" & counter & " ' style='width:25%; height:40px;'><span id='subem" & counter & "' style='height:50px; padding:8px;width:50px'><input id='Button11'  onclick=" & """ insertHtmlAtCursor('" & item & "')" & """ style='background-color:transparent; border-style:none;' type='button' value='" & item & "' /></span></td>"

                If counter Mod 4 = 0 Then emojidivs = emojidivs + "</tr><tr>"
                counter = counter + 1
            Next
            emojidivs = emojidivs + "</tr></table>"

            Dim myliteral As New Literal
            myliteral.Text = emojidivs
            Dim n As New PlaceHolder
            n = Me.Master.FindControl("plcEmoji")
            n.Controls.Add(myliteral)
           
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub

  


    Protected Sub gridview1_PageIndexChanging(sender As Object, e As GridViewPageEventArgs) Handles gridview1.PageIndexChanging
        Try


            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from parentward inner join studentsprofile on parentward.ward = studentsprofile.admno WHERE parentward.parent = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("parentID")))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()

                con.close()
            End Using

            gridview1.PageIndex = e.NewPageIndex
            gridview1.DataBind()
            Session("studentadd") = Nothing
            panel3.Visible = False
            gridview1.SelectedIndex = -1
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub



    Protected Sub gridview1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles gridview1.SelectedIndexChanged

    End Sub

    Protected Sub gridview1_SelectedIndexChanging(sender As Object, e As GridViewSelectEventArgs) Handles gridview1.SelectedIndexChanging
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsummary.class from parentward inner join studentsprofile on parentward.ward = studentsprofile.admno inner join studentsummary on studentsummary.student = studentsprofile.admno WHERE parentward.parent = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", Session("parentID")))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                If student0.Read Then
                    Session("cla") = student0(0)
                Else
                    Show_Alert(False, "Student not in class yet")
                    Exit Sub
                End If
                student0.Close()
                Dim x As Array = Split(gridview1.Rows(e.NewSelectedIndex).Cells(1).Text, " - ")
                Session("studentadd") = RTrim(x(0))
                panel3.Visible = False
                pnlAll.Visible = False
                chatthread.Visible = True
                gridview1.SelectedIndex = -1
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
                Session("chatbox") = True
                con.Close()
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub



    Sub load_chats()
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd1x As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsummary.class from studentsummary WHERE studentsummary.student = '" & Session("studentadd") & "' and studentsummary.session = '" & Session("sessionid") & "'", con)
            Dim student0x As MySql.Data.MySqlClient.MySqlDataReader = cmd1x.ExecuteReader
            If student0x.Read Then
                Session("cla") = student0x(0)
            End If
            student0x.Close()
            Dim mypass As String = ""
            Dim cmd1xf As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from parentprofile where parentid = '" & Session("parentid") & "'", con)
            Dim student0xf As MySql.Data.MySqlClient.MySqlDataReader = cmd1xf.ExecuteReader
            If student0xf.Read Then
                mypass = student0xf(0).ToString
            End If
            student0xf.Close()
            Dim mypassar As Array = Split("~/img/noimage.jpg", "/")
            If mypass <> "" Then mypassar = Split(mypass, "/")

            Dim cmd1xc As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsummary.class, session.opendate, session.closingdate from studentsummary inner join session on studentsummary.session = session.id WHERE studentsummary.student = '" & Session("studentadd") & "' and studentsummary.session = '" & Session("sessionid") & "'", con)
            Dim student0xc As MySql.Data.MySqlClient.MySqlDataReader = cmd1xc.ExecuteReader
            Dim opendate As String
            Dim closingdate As String
            If student0xc.Read Then
                Session("cla") = student0xc(0)
                opendate = student0xc(1)
                closingdate = student0xc(2)
            End If
            student0xc.Close()
            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT communicate.message, communicate.time, communicate.sender, communicate.id, communicate.dept, staffprofile.surname, staffprofile.passport from communicate left join staffprofile on staffprofile.staffid = communicate.senderid where communicate.student = '" & Session("studentadd") & "' and communicate.time >= '" & Convert.ToDateTime(DateTime.ParseExact(opendate, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss") & "' and communicate.time <= '" & Convert.ToDateTime(DateTime.ParseExact(closingdate, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss") & "' order by communicate.time", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
            Dim chats As String = ""
            Dim mymsg As String = ""
            
            Do While student0.Read
                Dim pass As Array = Split("~/img/noimage.jpg", "/")
                If student0(6).ToString <> "" Then pass = Split(student0(6).ToString, "/")
                Dim counter As Integer = 0
                Dim thismsg As String = student0("message").ToString
               

                logify.Read_notification("~/content/App/App/parent/communicate.aspx?" & student0("id"), Session("parentid"))
                Dim dob As Date = student0("time")
                Dim sage As TimeSpan = Now.Subtract(dob)
                Dim timelag As String = ""
                If dob.DayOfWeek = Now.DayOfWeek And sage.Days < 1 Then
                    timelag = "Today"
                ElseIf dob.DayOfWeek <> Now.DayOfWeek And sage.Days <= 2 Then
                    timelag = "Yesterday"
                Else
                    timelag = dob.Day & "/" & dob.Month & "/" & dob.Year
                End If
             
                If student0("sender") <> Session("parentid") Then
                    chats = chats + "<div class='row'>" & _
                                        "<div class='author-chat' id='" & counter & "fs" & "' style='word-wrap:break-word;'>" & _
                                            "<div class='row'><div class='col-lg-1' id='vvv" & counter & counter & "' style='width:35px; padding:0;'></div><div class='col-lg-11'><h5>" & IIf(student0(4).ToString = "", student0(5).ToString & " (Class Teacher)", student0(5).ToString & " (School Head)") & "<span class='chat-date'>" & timelag & " " & IIf(dob.Hour > 12, IIf(dob.Hour - 12 > 9, dob.Hour - 12, "0" & dob.Hour - 12), IIf(dob.Hour > 9, dob.Hour, "0" & dob.Hour)) & ":" & IIf(dob.Minute < 10, "0" & dob.Minute, dob.Minute) & IIf(dob.Hour > 12, " pm", " am") & "</span></h5></div></div>" & _
                                            "<div class='row'><div class='col-lg-1' id='vvv" & counter & "' style='width:35px; padding:0;'><img  style='width:35px; height:35px; border-radius:50%;' runat ='server'  src='../../img/" & pass(2) & "' alt='' /></div><div class='col-lg-11'><p>" & thismsg & "</p></div></div>" & _
                                        "</div>" & _
                                    "</div>"
                    mymsg = mymsg + "<div class='row'>" & _
                                     "<div class='client-chat' id='" & counter & "fss" & "' style='word-wrap:break-word;'>" & _
                                         "<div class='row'><div class='col-lg-1' id='vvv" & counter & counter & "' style='width:35px; padding:0;'></div><div class='col-lg-11'><h5>" & IIf(student0(4).ToString = "", student0(5).ToString & " (Class Teacher)", student0(5).ToString & " (School Head)") & "<span class='chat-date'>" & timelag & " " & IIf(dob.Hour > 12, IIf(dob.Hour - 12 > 9, dob.Hour - 12, "0" & dob.Hour - 12), IIf(dob.Hour > 9, dob.Hour, "0" & dob.Hour)) & ":" & IIf(dob.Minute < 10, "0" & dob.Minute, dob.Minute) & IIf(dob.Hour > 12, " pm", " am") & "</span></h5></div></div>" & _
                                         "<div class='row'><div class='col-lg-1' id='vvv" & counter & "' style='width:35px; padding:0;'><img  style='width:35px; height:35px; border-radius:50%;' runat ='server'  src='../../img/" & pass(2) & "' alt='' /></div><div class='col-lg-11'><p>" & thismsg & "</p></div></div>" & _
                                     "</div>" & _
                                 "</div>"
                Else

                    chats = chats + "<div class='row'>" & _
                                       "<div class='client-chat' id='" & counter & "fss" & "' style='word-wrap:break-word;'>" & _
                                           "<div class='row'><div class='col-lg-11' ><h5>You <span class='chat-date'>" & timelag & " " & IIf(dob.Hour > 12, IIf(dob.Hour - 12 > 9, dob.Hour - 12, "0" & dob.Hour - 12), IIf(dob.Hour > 9, dob.Hour, "0" & dob.Hour)) & ":" & IIf(dob.Minute < 10, "0" & dob.Minute, dob.Minute) & IIf(dob.Hour > 12, " pm", " am") & "</span></h5></div><div class='col-lg-1' id='vvv" & counter & counter & "' style='width:35px; padding:0;'></div></div>" & _
                                           "<div class='row'><div class='col-lg-11' ><p>" & thismsg & "</p></div><div class='col-lg-11' id='vvv" & counter & "' style='width:35px; padding:0;'><img  style='width:35px; height:35px; border-radius:50%;' runat ='server'  src='../../img/" & IIf(mypass <> "", mypassar(2), "noimage.jpg") & "' alt='' /></div></div>" & _
                                       "</div>" & _
                                   "</div>"

                End If

                counter = Nothing
            Loop
            literal1.Text = chats
            lblstudent.Text = par.getstuname(Session("studentadd"))
            panel3.Visible = False
            pnlAll.Visible = False
            chatthread.Visible = True
            Me.Page.MaintainScrollPositionOnPostBack = True
            Timer2.Enabled = True
            con.Close()
            If Session("lastchat") = Nothing Then Session("lastchat") = mymsg
            Dim sound As New Literal
            Dim plc As PlaceHolder
            Dim path As String = "https://" & Request.Url.Authority
            plc = Me.Master.FindControl("plcNot")
            sound.Text = "<div visible = 'false'><audio autoplay = 'true'> <source src='" & path & "/Sounds/sound1.ogg" & "' type 'audio/ogg' ></audio></div>"
            If Session("lastchat") <> mymsg Then plc.Controls.Add(sound)
            Session("lastchat") = mymsg
        End Using
    End Sub

   
    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim atleast1 As Boolean = False
                Dim selected As String = ""
                For Each item As ListItem In radParent.Items
                    If item.Selected = True Then
                        atleast1 = True
                        selected = item.Text
                    End If
                Next
                If atleast1 = False Then
                    Show_Alert(False, "Please select an option")
                    Exit Sub
                End If
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("insert into communicate (student, message, sender, receiver, time) values (?,?,?,?,?)", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("Studentadd")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", selected))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("parentid")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()

                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where receiver = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Select teacher from classteacher where class = '" & Session("cla") & "'", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
            Do While student0.Read
                    logify.Notifications("You have a new message - Child Activity", student0(0), Session("parentid"), par.getstuname(Session("studentadd")) & " Parent", "~/content/App/App/staff/communicate.aspx?" & id, "")
                Loop
                student0a.Close()
            Show_Alert(True, "Message sent")
            con.Close()
            load_chats()
            End Using
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            Session("chatbox") = True

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim atleast1 As Boolean = False
                Dim selected As String = ""
                For Each item As ListItem In RadioButtonList1.Items
                    If item.Selected = True Then
                        atleast1 = True
                        selected = item.Text
                    End If
                Next
                If atleast1 = False Then
                    Show_Alert(False, "Please select an option")
                    Exit Sub
                End If
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("insert into communicate (student, message, sender, receiver, time) values (?,?,?,?,?)", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("Studentadd")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", selected))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("parentid")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where receiver = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Select teacher from classteacher where class = '" & Session("cla") & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                Do While student0.Read
                    logify.Notifications("You have a new message - Child Activity", student0(0), Session("parentid"), par.getstuname(Session("studentadd")) & " Parent", "~/content/App/App/staff/communicate.aspx?" & id, "")
                Loop
                student0a.Close()
                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
            End Using
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            Session("chatbox") = True

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim atleast1 As Boolean = False
                Dim selected As String = ""
                For Each item As ListItem In RadioButtonList2.Items
                    If item.Selected = True Then
                        atleast1 = True
                        selected = item.Text
                    End If
                Next
                If atleast1 = False Then
                    Show_Alert(False, "Please select an option")
                    Exit Sub
                End If
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("insert into communicate (student, message, sender, receiver, time) values (?,?,?,?,?)", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("Studentadd")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", selected))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("parentid")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where receiver = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Select teacher from classteacher where class = '" & Session("cla") & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                Do While student0.Read
                    logify.Notifications("You have a new message - Child Activity", student0(0), Session("parentid"), par.getstuname(Session("studentadd")) & " Parent", "~/content/App/App/staff/communicate.aspx?" & id, "")
                Loop
                student0a.Close()
                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
            End Using
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            Session("chatbox") = True

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim atleast1 As Boolean = False
                Dim selected As String = ""
                For Each item As ListItem In RadioButtonList3.Items
                    If item.Selected = True Then
                        atleast1 = True
                        selected = item.Text
                    End If
                Next
                If atleast1 = False Then
                    Show_Alert(False, "Please select an option")
                    Exit Sub
                End If
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("insert into communicate (student, message, sender, receiver, time) values (?,?,?,?,?)", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("Studentadd")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", selected))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("parentid")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where receiver = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Select teacher from classteacher where class = '" & Session("cla") & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                Do While student0.Read
                    logify.Notifications("You have a new message - Child Activity", student0(0), Session("parentid"), par.getstuname(Session("studentadd")) & " Parent", "~/content/App/App/staff/communicate.aspx?" & id, "")
                Loop
                student0a.Close()
                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
            End Using
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            Session("chatbox") = True

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim atleast1 As Boolean = False
                Dim selected As String = ""
                For Each item As ListItem In RadioButtonList4.Items
                    If item.Selected = True Then
                        atleast1 = True
                        selected = item.Text
                    End If
                Next
                If atleast1 = False Then
                    Show_Alert(False, "Please select an option")
                    Exit Sub
                End If
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("insert into communicate (student, message, sender, receiver, time) values (?,?,?,?,?)", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("Studentadd")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", selected))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("parentid")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where receiver = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Select teacher from classteacher where class = '" & Session("cla") & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                Do While student0.Read
                    logify.Notifications("You have a new message - Child Activity", student0(0), Session("parentid"), par.getstuname(Session("studentadd")) & " Parent", "~/content/App/App/staff/communicate.aspx?" & id, "")
                Loop
                student0a.Close()
                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
            End Using
            ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            Session("chatbox") = True

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim atleast1 As Boolean = False
                Dim selected As String = ""
                For Each item As ListItem In RadioButtonList5.Items
                    If item.Selected = True Then
                        atleast1 = True
                        selected = item.Text
                    End If
                Next
                If atleast1 = False Then
                    Show_Alert(False, "Please select an option")
                    Exit Sub
                End If
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("insert into communicate (student, message, sender, receiver, time) values (?,?,?,?,?)", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("Studentadd")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", selected))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("parentid")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where receiver = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Select teacher from classteacher where class = '" & Session("cla") & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                Do While student0.Read
                    logify.Notifications("You have a new message - Child Activity", student0(0), Session("parentid"), par.getstuname(Session("studentadd")) & " Parent", "~/content/App/App/staff/communicate.aspx?" & id, "")
                Loop
                student0a.Close()
                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
                Session("chatbox") = True
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        chatthread.Visible = False
        panel3.Visible = True
        Session("chatbox") = False
    End Sub

    Protected Sub btnStudents_Click(sender As Object, e As EventArgs) Handles btnStudents.Click
        chatthread.Visible = False
        pnlAll.Visible = True
        Session("chatbox") = False
    End Sub

    Protected Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
       
        If chatthread.Visible = True Then
            load_chats()


        End If

    End Sub

   

  
    Protected Sub Button10_Click(sender As Object, e As EventArgs) Handles Button10.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                If Not Hidden1.Value.ToString = "" Then
                    Dim emojis As New ArrayList
                    emojis.Add("😀")
                    emojis.Add("😁")
                    emojis.Add("😂")
                    emojis.Add("😃")
                    emojis.Add("😄")
                    emojis.Add("😅")
                    emojis.Add("😆")
                    emojis.Add("😇")
                    emojis.Add("😈")
                    emojis.Add("😉")
                    emojis.Add("😊")
                    emojis.Add("😋")
                    emojis.Add("😌")
                    emojis.Add("😍")
                    emojis.Add("😎")
                    emojis.Add("😏")
                    emojis.Add("😐")
                    emojis.Add("😑")
                    emojis.Add("😒")
                    emojis.Add("😓")
                    emojis.Add("😔")
                    emojis.Add("😕")
                    emojis.Add("😖")
                    emojis.Add("😗")
                    emojis.Add("😘")
                    emojis.Add("😙")
                    emojis.Add("😚")
                    emojis.Add("😛")
                    emojis.Add("😜")
                    emojis.Add("😝")
                    emojis.Add("😞")
                    emojis.Add("😟")
                    emojis.Add("😠")
                    emojis.Add("😡")
                    emojis.Add("😢")
                    emojis.Add("😣")
                    emojis.Add("😤")
                    emojis.Add("😥")
                    emojis.Add("😦")
                    emojis.Add("😧")
                    emojis.Add("😨")
                    emojis.Add("😩")
                    emojis.Add("😪")
                    emojis.Add("😫")
                    emojis.Add("😬")
                    emojis.Add("😭")
                    emojis.Add("😮")
                    emojis.Add("😯")
                    emojis.Add("😰")
                    emojis.Add("😱")
                    emojis.Add("😲")
                    emojis.Add("😳")
                    emojis.Add("😴")
                    emojis.Add("😵")
                    emojis.Add("😶")
                    emojis.Add("😷")
                    emojis.Add("🙁")
                    emojis.Add("🙂")
                    emojis.Add("🙃")
                    emojis.Add("🙄")
                    emojis.Add("👆")
                    emojis.Add("👈")
                    emojis.Add("👉")
                    emojis.Add("👊")
                    emojis.Add("👋")
                    emojis.Add("👌")
                    emojis.Add("👍")
                    emojis.Add("👎")
                    emojis.Add("👏")
                    emojis.Add("👐")
                    emojis.Add("🖌")
                    emojis.Add("🖍")
                    emojis.Add("🖐")
                    emojis.Add("🖕")
                    emojis.Add("🙆")
                    emojis.Add("🙇")
                    emojis.Add("🙈")
                    emojis.Add("🙊")
                    emojis.Add("🙋")
                    emojis.Add("🙌")
                    emojis.Add("🙍")
                    emojis.Add("🙎")
                    emojis.Add("🙏")
                    Dim counter As Integer = 0
                    
                    Dim ss As String
                    If Hidden1.Value.ToString.Length > 200 Then
                        ss = Hidden1.Value.ToString.Substring(0, 199) & "..."

                    Else
                        ss = Hidden1.Value.ToString

                    End If

                    Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("insert into communicate (student, message, sender, receiver, time, senderid) values (?,?,?,?,?,?)", con)
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("Studentadd")))
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", Hidden1.Value.ToString.Replace("&nbsp;", " ")))
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("parentid")))
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", Session("cla")))
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subjedghct", Session("parentid")))
                    cmd1.ExecuteNonQuery()
                    Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where receiver = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                    Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                    student0a.Read()
                    Dim id As Integer = student0a(0)
                    student0a.Close()
                    Dim cmd2 As New MySql.Data.MySqlClient.MySqlCommand("Select teacher from classteacher where class = '" & Session("cla") & "'", con)
                    Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd2.ExecuteReader
                    Do While student0.Read

                        logify.Notifications(ss, student0(0), Session("parentid"), par.getstuname(Session("studentadd")) & " Parent", "~/content/App/App/staff/communicate.aspx?" & id, "Message")
                    Loop
                    student0a.Close()
                    Show_Alert(True, "Message sent")
                Else
                    Show_Alert(False, "None")
                End If
                con.Close()
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
                Session("chatbox") = True
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Button16_Click(sender As Object, e As EventArgs) Handles Button16.Click
        panel3.Visible = False
        pnlAll.Visible = False
        chatthread.Visible = True
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
    End Sub
End Class
