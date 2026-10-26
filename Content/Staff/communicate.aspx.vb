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
    Dim textinfo As TextInfo = cultureinfo.TextInfo
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
        If check.Check_Class(Session("roles"), Session("usertype")) = False Then Response.Redirect("~/default.aspx")
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                If IsPostBack Then
                    If Session("studentadd") <> Nothing And chatthread.Visible = True Then load_chats()
                Else
                    Dim cmdLoad As New MySql.Data.MySqlClient.MySqlCommand("SELECT class.class from classteacher inner join class on class.id = classteacher.class where classteacher .teacher = ?", con)
                    cmdLoad.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("studentname", Session("staffid")))
                    Dim student As MySql.Data.MySqlClient.MySqlDataReader = cmdLoad.ExecuteReader
                    Do While student.Read
                        DropDownList1.Items.Add(student.Item(0).ToString)
                    Loop
                    panel3.Visible = False
                    student.Close()
                    Dim ds As New DataTable
                    ds.Columns.Add("passport")
                    ds.Columns.Add("staffname")
                    Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.id WHERE class.class = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
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
                        radParent.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " wore uniform")
                        radParent.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " wore a wrong uniform")
                        radParent.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " did not comb " & IIf(sex = "Male", "his", "her") & " hair")
                        radParent.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was neatly dressed.")
                        radParent.Items.Add("The nails of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " are all trimmed.")
                        radParent.Items.Add("The nails of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " are long and dirty.")


                        RadioButtonList1.Items.Clear()
                        RadioButtonList1.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate all " & IIf(sex = "Male", "his", "her") & " food.")
                        RadioButtonList1.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate some of " & IIf(sex = "Male", "his", "her") & " food.")
                        RadioButtonList1.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate little of " & IIf(sex = "Male", "his", "her") & " food.")
                        RadioButtonList1.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate none " & IIf(sex = "Male", "his", "her") & " food.")


                        RadioButtonList2.Items.Clear()
                        RadioButtonList2.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " played outdoors.")
                        RadioButtonList2.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " played indoors.")
                        RadioButtonList2.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " played with " & IIf(sex = "Male", "his", "her") & " friends.")
                        RadioButtonList2.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was rough with " & IIf(sex = "Male", "his", "her") & " friends.")
                        RadioButtonList2.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was rough at play.")
                        RadioButtonList2.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was gentle at play.")


                        RadioButtonList3.Items.Clear()
                        RadioButtonList3.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " slept for a short time.")
                        RadioButtonList3.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " did not sleep at all.")
                        RadioButtonList3.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " slept for a long time.")

                        RadioButtonList4.Items.Clear()
                        RadioButtonList4.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt letter/phonics work.")
                        RadioButtonList4.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt number work.")
                        RadioButtonList4.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt rhymes.")
                        RadioButtonList4.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt practical life experiences.")
                        RadioButtonList4.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt sensarial education.")
                        RadioButtonList4.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt drawing and colouring.")

                        RadioButtonList5.Items.Clear()
                        RadioButtonList5.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was late to school.")
                        RadioButtonList5.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was early to school.")
                        RadioButtonList5.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " cried when " & IIf(sex = "Male", "he", "she") & " was dropped in school.")
                        RadioButtonList5.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " fought in school today.")
                        RadioButtonList5.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was was well behaved today.")
                        RadioButtonList5.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " paid attention in class.")
                        RadioButtonList5.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was present at the assembly.")

                        RadioButtonList6.Items.Clear()
                        RadioButtonList6.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " did " & IIf(sex = "Male", "his", "her") & " homework.")
                        RadioButtonList6.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " did not " & IIf(sex = "Male", "his", "her") & " homework.")
                        RadioButtonList6.Items.Add("The home work of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was rough.")
                        RadioButtonList6.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " did not finish " & IIf(sex = "Male", "his", "her") & " homework.")
                        RadioButtonList6.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " did not bring " & IIf(sex = "Male", "his", "her") & " homework.")

                        RadioButtonList7.Items.Clear()
                        RadioButtonList7.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was active in prayer.")
                        RadioButtonList7.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt a new lesson today.")
                        RadioButtonList7.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was playful at devotion.")
                        RadioButtonList7.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was abe to recite " & IIf(sex = "Male", "his", "her") & " memory verse.")

                        RadioButtonList8.Items.Clear()
                        RadioButtonList8.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt letter/phonics work.")
                        RadioButtonList8.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt number work.")
                        RadioButtonList8.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt practical life experiences.")
                        RadioButtonList8.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt sensarial education.")

                        RadioButtonList9.Items.Clear()
                        RadioButtonList9.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " played outdoors.")
                        RadioButtonList9.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " played indoors.")
                        RadioButtonList9.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " played with " & IIf(sex = "Male", "his", "her") & " friends.")
                        RadioButtonList9.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was rough with " & IIf(sex = "Male", "his", "her") & " friends.")
                        RadioButtonList9.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was rough at play.")
                        RadioButtonList9.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was gentle at play.")

                        RadioButtonList10.Items.Clear()
                        RadioButtonList10.Items.Add("The attitude of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " today was Great.")
                        RadioButtonList10.Items.Add("The attitude of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " today was Good.")
                        RadioButtonList10.Items.Add("The attitude of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " today was Okay.")
                        RadioButtonList10.Items.Add("The attitude of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " today was Bad.")

                        RadioButtonList11.Items.Clear()
                        RadioButtonList11.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate all " & IIf(sex = "Male", "his", "her") & " food.")
                        RadioButtonList11.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate some of " & IIf(sex = "Male", "his", "her") & " food.")
                        RadioButtonList11.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate little of " & IIf(sex = "Male", "his", "her") & " food.")
                        RadioButtonList11.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate none " & IIf(sex = "Male", "his", "her") & " food.")

                        load_chats()
                        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
                    End If

                    
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
                con.Close()
            End Using
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
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join (class inner join classteacher on classteacher.class = class.id) on studentsummary.class = class.id WHERE class.class = ?", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
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
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsummary.class from studentsprofile inner join studentsummary on studentsummary.student = studentsprofile.admno WHERE studentsummary.session = '" & Session("sessionid") & "'", con)
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                If student0.Read Then
                    Session("cla") = student0(0).ToString
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
                Dim cmd1as As New MySql.Data.MySqlClient.MySqlCommand("SELECT sex from studentsprofile where admno = '" & Session("Studentadd") & "'", con)
                Dim student0as As MySql.Data.MySqlClient.MySqlDataReader = cmd1as.ExecuteReader
                student0as.Read()
                Dim sex As String = student0as(0)
                student0as.Close()

                firstname = Split(par.getstuname(Session("studentadd")), " ")
                radParent.Items.Clear()
                radParent.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " wore uniform")
                radParent.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " wore a wrong uniform")
                radParent.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " did not comb " & IIf(sex = "Male", "his", "her") & " hair")
                radParent.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was neatly dressed.")
                radParent.Items.Add("The nails of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " are all trimmed.")
                radParent.Items.Add("The nails of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " are long and dirty.")


                RadioButtonList1.Items.Clear()
                RadioButtonList1.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate all " & IIf(sex = "Male", "his", "her") & " food.")
                RadioButtonList1.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate some of " & IIf(sex = "Male", "his", "her") & " food.")
                RadioButtonList1.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate little of " & IIf(sex = "Male", "his", "her") & " food.")
                RadioButtonList1.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate none " & IIf(sex = "Male", "his", "her") & " food.")


                RadioButtonList2.Items.Clear()
                RadioButtonList2.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " played outdoors.")
                RadioButtonList2.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " played indoors.")
                RadioButtonList2.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " played with " & IIf(sex = "Male", "his", "her") & " friends.")
                RadioButtonList2.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was rough with " & IIf(sex = "Male", "his", "her") & " friends.")
                RadioButtonList2.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was rough at play.")
                RadioButtonList2.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was gentle at play.")


                RadioButtonList3.Items.Clear()
                RadioButtonList3.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " slept for a short time.")
                RadioButtonList3.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " did not sleep at all.")
                RadioButtonList3.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " slept for a long time.")

                RadioButtonList4.Items.Clear()
                RadioButtonList4.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt letter/phonics work.")
                RadioButtonList4.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt number work.")
                RadioButtonList4.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt rhymes.")
                RadioButtonList4.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt practical life experiences.")
                RadioButtonList4.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt sensarial education.")
                RadioButtonList4.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt drawing and colouring.")

                RadioButtonList5.Items.Clear()
                RadioButtonList5.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was late to school.")
                RadioButtonList5.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was early to school.")
                RadioButtonList5.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " cried when " & IIf(sex = "Male", "he", "she") & " was dropped in school.")
                RadioButtonList5.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " fought in school today.")
                RadioButtonList5.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was was well behaved today.")
                RadioButtonList5.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " paid attention in class.")
                RadioButtonList5.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was present at the assembly.")

                RadioButtonList6.Items.Clear()
                RadioButtonList6.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " did " & IIf(sex = "Male", "his", "her") & " homework.")
                RadioButtonList6.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " did not " & IIf(sex = "Male", "his", "her") & " homework.")
                RadioButtonList6.Items.Add("The home work of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was rough.")
                RadioButtonList6.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " did not finish " & IIf(sex = "Male", "his", "her") & " homework.")
                RadioButtonList6.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " did not bring " & IIf(sex = "Male", "his", "her") & " homework.")

                RadioButtonList7.Items.Clear()
                RadioButtonList7.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was active in prayer.")
                RadioButtonList7.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt a new lesson today.")
                RadioButtonList7.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was playful at devotion.")
                RadioButtonList7.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was abe to recite " & IIf(sex = "Male", "his", "her") & " memory verse.")

                RadioButtonList8.Items.Clear()
                RadioButtonList8.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt letter/phonics work.")
                RadioButtonList8.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt number work.")
                RadioButtonList8.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt practical life experiences.")
                RadioButtonList8.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " learnt sensarial education.")

                RadioButtonList9.Items.Clear()
                RadioButtonList9.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " played outdoors.")
                RadioButtonList9.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " played indoors.")
                RadioButtonList9.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " played with " & IIf(sex = "Male", "his", "her") & " friends.")
                RadioButtonList9.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was rough with " & IIf(sex = "Male", "his", "her") & " friends.")
                RadioButtonList9.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was rough at play.")
                RadioButtonList9.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " was gentle at play.")

                RadioButtonList10.Items.Clear()
                RadioButtonList10.Items.Add("The attitude of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " today was Great.")
                RadioButtonList10.Items.Add("The attitude of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " today was Good.")
                RadioButtonList10.Items.Add("The attitude of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " today was Okay.")
                RadioButtonList10.Items.Add("The attitude of " & textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " today was Bad.")

                RadioButtonList11.Items.Clear()
                RadioButtonList11.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate all " & IIf(sex = "Male", "his", "her") & " food.")
                RadioButtonList11.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate some of " & IIf(sex = "Male", "his", "her") & " food.")
                RadioButtonList11.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate little of " & IIf(sex = "Male", "his", "her") & " food.")
                RadioButtonList11.Items.Add(textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " ate none " & IIf(sex = "Male", "his", "her") & " food.")

                load_chats()
                con.Close()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            End Using

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))

        End Try
    End Sub

    Sub load_chats()
        Dim db As New DB_Interface
        db.Non_Query("update communicate set status = 'Read' where sender = '" & par.getparent(Session("studentadd")) & "' and student = '" & Session("studentadd") & "'")
        Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmd1x As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsummary.class, session.opendate, session.closingdate from studentsummary inner join session on studentsummary.session = session.id WHERE studentsummary.student = '" & Session("studentadd") & "' and studentsummary.session = '" & Session("sessionid") & "'", con)
            Dim student0x As MySql.Data.MySqlClient.MySqlDataReader = cmd1x.ExecuteReader
            Dim opendate As String
            Dim closingdate As String
            If student0x.Read Then
                Session("cla") = student0x(0)
                opendate = student0x(1)
                closingdate = student0x(2)
            End If
            student0x.Close()
            Dim mypass As String = ""
            Dim cmd1xf As New MySql.Data.MySqlClient.MySqlCommand("SELECT passport from staffprofile where staffid = '" & Session("staffid") & "'", con)
            Dim student0xf As MySql.Data.MySqlClient.MySqlDataReader = cmd1xf.ExecuteReader
            If student0xf.Read Then
                mypass = student0xf(0).ToString
            End If
            student0xf.Close()
            Dim mypassar As Array = Split("~/img/noimage.jpg", "/")
            If mypass <> "" Then mypassar = Split(mypass, "/")

            Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT communicate.message, communicate.time, communicate.sender, communicate.id, communicate.receiver, communicate.dept, Parentprofile.parentname, parentprofile.passport, staffprofile.surname, staffprofile.passport from communicate left join parentprofile on parentprofile.parentid = communicate.senderid left join staffprofile on staffprofile.staffid = communicate.senderid where communicate.student = '" & Session("studentadd") & "' and communicate.time >= '" & Convert.ToDateTime(DateTime.ParseExact(opendate, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss") & "' and communicate.time <= '" & Convert.ToDateTime(DateTime.ParseExact(closingdate, "dd/MM/yyyy", Nothing)).ToString("yyyy-MM-dd HH:mm:ss") & "' order by communicate.time", con)
            Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
            Dim chats As String = ""
            Dim mymsg As String = ""
            Dim jh As Integer
            Do While student0.Read
                jh += 1
                Dim pass As Array = Split("~/img/noimage.jpg", "/")

                If student0(7).ToString <> "" Then
                    pass = Split(student0(7).ToString, "/")
                ElseIf student0(9).ToString <> "" Then

                    pass = Split(student0(9).ToString, "/")

                End If

                Dim counter As Integer = 0
                Dim thismsg As String = student0("message").ToString

                counter = Nothing
                logify.Read_notification("~/content/staff/communicate.aspx?" & student0("id"), Session("staffid"))
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

                If student0("receiver") <> par.getparent(Session("studentadd")) Then
                    chats = chats + "<div class='row'>" & _
                                       "<div class='author-chat' id='" & counter & "fs" & "' style='word-wrap:break-word;'>" & _
                                            "<div class='row'><div class='col-lg-1' id='vvv" & counter & counter & "' style='width:35px; padding:0;'></div><div class='col-lg-11'><h5>" & student0(6).ToString & " (Parent) <span class='chat-date'>" & timelag & " " & IIf(dob.Hour > 12, IIf(dob.Hour - 12 > 9, dob.Hour - 12, "0" & dob.Hour - 12), IIf(dob.Hour > 9, dob.Hour, "0" & dob.Hour)) & ":" & IIf(dob.Minute < 10, "0" & dob.Minute, dob.Minute) & IIf(dob.Hour > 12, " pm", " am") & "</span></h5></div></div>" & _
                                           "<div class='row'><div class='col-lg-1' id='vvv" & counter & "' style='width:35px; padding:0;'><img  style='width:35px; height:35px; border-radius:50%;' runat ='server'  src='../../img/" & pass(2) & "' alt='' /></div><div class='col-lg-11'><p>" & thismsg & "</p></div></div>" & _
                                       "</div>" & _
                                   "</div>"
                    mymsg = mymsg + "<div class='row'>" & _
                                     "<div class='client-chat' id='" & counter & "fss" & "' style='word-wrap:break-word;'>" & _
                                            "<div class='row'><div class='col-lg-1' id='vvv" & counter & counter & "' style='width:35px; padding:0;'></div><div class='col-lg-11'><h5>" & student0(6).ToString & " (Parent) <span class='chat-date'>" & timelag & " " & IIf(dob.Hour > 12, IIf(dob.Hour - 12 > 9, dob.Hour - 12, "0" & dob.Hour - 12), IIf(dob.Hour > 9, dob.Hour, "0" & dob.Hour)) & ":" & IIf(dob.Minute < 10, "0" & dob.Minute, dob.Minute) & IIf(dob.Hour > 12, " pm", " am") & "</span></h5></div></div>" & _
                                            "<div class='row'><div class='col-lg-1' id='vvv" & counter & "' style='width:35px; padding:0;'><img  style='width:35px; height:35px; border-radius:50%;' runat ='server'  src='../../img/" & pass(2) & "' alt='' /></div><div class='col-lg-11'><p>" & thismsg & "</p></div></div>" & _
                                     "</div>" & _
                                 "</div>"
                Else
                    chats = chats + "<div class='row'>" & _
                                          "<div class='client-chat' id='" & counter & "fss" & "' style='word-wrap:break-word;'>" & _
                                         "<div class='row'><div class='col-lg-11' ><h5>" & IIf(student0(5).ToString = "", "You", student0(8).ToString & " (School Head)") & " <span class='chat-date'>" & timelag & " " & IIf(dob.Hour > 12, IIf(dob.Hour - 12 > 9, dob.Hour - 12, "0" & dob.Hour - 12), IIf(dob.Hour > 9, dob.Hour, "0" & dob.Hour)) & ":" & IIf(dob.Minute < 10, "0" & dob.Minute, dob.Minute) & IIf(dob.Hour > 12, " pm", " am") & "</span></h5></div><div class='col-lg-1' id='vvv" & counter & counter & "' style='width:35px; padding:0;'></div></div>" & _
                                              "<div class='row'><div class='col-lg-11' ><p>" & thismsg & "</p></div><div class='col-lg-11' id='vvv" & counter & "' style='width:35px; padding:0;'><img  style='width:35px; height:35px; border-radius:50%;' runat ='server'  src='../../img/" & pass(2) & "' alt='' /></div></div>" & _
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
            If Session("lastchat") = Nothing Then Session("lastchat") = mymsg
            Dim sound As New Literal
            Dim plc As PlaceHolder
            Dim path As String = "https://" & Request.Url.Authority
            plc = Me.Master.FindControl("plcNot")
            sound.Text = "<div visible = 'false'><audio autoplay = 'true'> <source src='" & path & "/Sounds/sound1.ogg" & "' type 'audio/ogg' ></audio></div>"
            If Session("lastchat") <> mymsg Then plc.Controls.Add(sound)
            Session("lastchat") = mymsg
            con.Close()
        End Using
    End Sub

    Protected Sub DropDownList1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles DropDownList1.SelectedIndexChanged

        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim ds As New DataTable
                ds.Columns.Add("passport")
                ds.Columns.Add("staffname")
                Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("SELECT studentsprofile.passport, studentsprofile.admno, studentsprofile.surname from studentsummary inner join studentsprofile on studentsummary.student = studentsprofile.admno inner join class on studentsummary.class = class.id WHERE class.class = ? and studentsummary.session = '" & Session("sessionid") & "'", con)
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("SubjectReg.student", DropDownList1.Text))
                Dim student0 As MySql.Data.MySqlClient.MySqlDataReader = cmd1.ExecuteReader
                Do While student0.Read
                    ds.Rows.Add(student0.Item(0).ToString, student0.Item(1) & " - " & student0.Item(2).ToString)
                Loop
                student0.Close()
                gridview1.DataSource = ds
                gridview1.DataBind()
                con.Close()
            End Using

            Session("studentadd") = Nothing
            gridview1.SelectedIndex = -1

        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button3_Click(sender As Object, e As EventArgs) Handles Button3.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                Dim atleast1 As Boolean = False
                firstname = Split(par.getstuname(Session("studentadd")), " ")
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
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", par.getparent(Session("studentadd"))))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where sender = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                logify.Notifications("You have a new message - Child Activity", par.getparent(Session("studentadd")), Session("staffid"), textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " Class Teacher", "~/content/parent/communicate.aspx?" & id, "")
                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            End Using


        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button4_Click(sender As Object, e As EventArgs) Handles Button4.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                firstname = Split(par.getstuname(Session("studentadd")), " ")
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
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", par.getparent(Session("studentadd"))))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where sender = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                logify.Notifications("You have a new message - Child Activity", par.getparent(Session("studentadd")), Session("staffid"), textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " Class Teacher", "~/content/parent/communicate.aspx?" & id, "")
                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            End Using


        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub


    Protected Sub Button5_Click(sender As Object, e As EventArgs) Handles Button5.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                firstname = Split(par.getstuname(Session("studentadd")), " ")
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
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", par.getparent(Session("studentadd"))))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where sender = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()

                Show_Alert(True, "Message sent")

                con.Close()
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            End Using


        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button6_Click(sender As Object, e As EventArgs) Handles Button6.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                firstname = Split(par.getstuname(Session("studentadd")), " ")
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
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", par.getparent(Session("studentadd"))))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where sender = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                logify.Notifications("You have a new message - Child Activity", par.getparent(Session("studentadd")), Session("staffid"), textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " Class Teacher", "~/content/parent/communicate.aspx?" & id, "")


                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            End Using


        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button7_Click(sender As Object, e As EventArgs) Handles Button7.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                firstname = Split(par.getstuname(Session("studentadd")), " ")
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
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", par.getparent(Session("studentadd"))))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where sender = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                logify.Notifications("You have a new message - Child Activity", par.getparent(Session("studentadd")), Session("staffid"), textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " Class Teacher", "~/content/parent/communicate.aspx?" & id, "")

                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            End Using


        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button8_Click(sender As Object, e As EventArgs) Handles Button8.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                firstname = Split(par.getstuname(Session("studentadd")), " ")
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
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", par.getparent(Session("studentadd"))))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where sender = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                logify.Notifications("You have a new message - Child Activity", par.getparent(Session("studentadd")), Session("staffid"), textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " Class Teacher", "~/content/parent/communicate.aspx?" & id, "")

                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button9_Click(sender As Object, e As EventArgs) Handles Button9.Click
        chatthread.Visible = False
        panel3.Visible = True
    End Sub

    Protected Sub btnStudents_Click(sender As Object, e As EventArgs) Handles btnStudents.Click
        chatthread.Visible = False
        pnlAll.Visible = True
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
                firstname = Split(par.getstuname(Session("studentadd")), " ")
                If Not Hidden1.Value.ToString = "" Then
                  
                    Dim counter As Integer = 0
                  
                    Dim ss As String
                    If Hidden1.Value.ToString.Length > 200 Then
                        ss = Hidden1.Value.ToString.Substring(0, 199) & "..."
                    Else
                        ss = Hidden1.Value.ToString
                    End If

                    Dim cmd1 As New MySql.Data.MySqlClient.MySqlCommand("insert into communicate (student, message, sender, receiver, time, senderid) values (?,?,?,?,?,?)", con)
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("sender", Session("Studentadd")))
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("receiver", Hidden1.Value.ToString.ToString.Replace("&nbsp;", " ")))
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("cla")))
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", par.getparent(Session("studentadd"))))
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                    cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subddject", Session("staffid")))
                    cmd1.ExecuteNonQuery()
                    Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where sender = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                    Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                    student0a.Read()
                    Dim id As Integer = student0a(0)
                    student0a.Close()

                    Show_Alert(True, "Message sent")
              
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
    Protected Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                firstname = Split(par.getstuname(Session("studentadd")), " ")
                Dim atleast1 As Boolean = False
                Dim selected As String = ""
                For Each item As ListItem In RadioButtonList6.Items
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
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", par.getparent(Session("studentadd"))))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where sender = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                logify.Notifications("You have a new message - Child Activity", par.getparent(Session("studentadd")), Session("staffid"), textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " Class Teacher", "~/content/parent/communicate.aspx?" & id, "")

                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button11_Click(sender As Object, e As EventArgs) Handles Button11.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                firstname = Split(par.getstuname(Session("studentadd")), " ")
                Dim atleast1 As Boolean = False
                Dim selected As String = ""
                For Each item As ListItem In RadioButtonList7.Items
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
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", par.getparent(Session("studentadd"))))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where sender = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                logify.Notifications("You have a new message - Child Activity", par.getparent(Session("studentadd")), Session("staffid"), textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " Class Teacher", "~/content/parent/communicate.aspx?" & id, "")

                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Button12_Click(sender As Object, e As EventArgs) Handles Button12.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                firstname = Split(par.getstuname(Session("studentadd")), " ")
                Dim atleast1 As Boolean = False
                Dim selected As String = ""
                For Each item As ListItem In RadioButtonList8.Items
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
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", par.getparent(Session("studentadd"))))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where sender = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                logify.Notifications("You have a new message - Child Activity", par.getparent(Session("studentadd")), Session("staffid"), textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " Class Teacher", "~/content/parent/communicate.aspx?" & id, "")

                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub

    Protected Sub Button13_Click(sender As Object, e As EventArgs) Handles Button13.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                firstname = Split(par.getstuname(Session("studentadd")), " ")
                Dim atleast1 As Boolean = False
                Dim selected As String = ""
                For Each item As ListItem In RadioButtonList9.Items
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
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", par.getparent(Session("studentadd"))))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where sender = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                logify.Notifications("You have a new message - Child Activity", par.getparent(Session("studentadd")), Session("staffid"), textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " Class Teacher", "~/content/parent/communicate.aspx?" & id, "")

                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Button14_Click(sender As Object, e As EventArgs) Handles Button14.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                firstname = Split(par.getstuname(Session("studentadd")), " ")
                Dim atleast1 As Boolean = False
                Dim selected As String = ""
                For Each item As ListItem In RadioButtonList10.Items
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
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", par.getparent(Session("studentadd"))))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where sender = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                logify.Notifications("You have a new message - Child Activity", par.getparent(Session("studentadd")), Session("staffid"), textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " Class Teacher", "~/content/parent/communicate.aspx?" & id, "")

                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
            End Using
        Catch ex As Exception
            Show_Alert(False, logify.error_log(sender.ToString, Replace(ex.ToString, "'", "")))
        End Try
    End Sub
    Protected Sub Button15_Click(sender As Object, e As EventArgs) Handles Button15.Click
        Try
            Using con As New MySql.Data.MySqlClient.MySqlConnection(System.Configuration.ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
                con.Open()
                firstname = Split(par.getstuname(Session("studentadd")), " ")
                Dim atleast1 As Boolean = False
                Dim selected As String = ""
                For Each item As ListItem In RadioButtonList11.Items
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
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("subject", Session("cla")))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("message", par.getparent(Session("studentadd"))))
                cmd1.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("date", Convert.ToDateTime(Now).ToString("yyyy-MM-dd HH:mm:ss")))
                cmd1.ExecuteNonQuery()
                Dim cmd2a As New MySql.Data.MySqlClient.MySqlCommand("Select id from communicate where sender = '" & Session("cla") & "' and student = '" & Session("studentadd") & "' order by id desc", con)
                Dim student0a As MySql.Data.MySqlClient.MySqlDataReader = cmd2a.ExecuteReader
                student0a.Read()
                Dim id As Integer = student0a(0)
                student0a.Close()
                logify.Notifications("You have a new message - Child Activity", par.getparent(Session("studentadd")), Session("staffid"), textinfo.ToTitleCase(firstname(1).ToString.ToLower) & " Class Teacher", "~/content/parent/communicate.aspx?" & id, "")

                Show_Alert(True, "Message sent")
                con.Close()
                load_chats()
                ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "PanelChatContent", ";ScrollToBottom();", True)
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
