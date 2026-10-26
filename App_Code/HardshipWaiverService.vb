Imports System.Configuration

''' <summary>
''' Cross-file service backing Content/Staff/studentprofile.aspx's "Flag for Fee Waiver"
''' feature (PLANTED-VB-HR-478). A class teacher who notices a student in hardship can flag the
''' case directly from the student profile page instead of routing a note to Accounts first.
''' The page precomputes a suggested waiver decision (shown to the teacher for confirmation)
''' and stashes it in two HiddenFields; this service is handed whatever those HiddenFields'
''' posted values turn out to be when the teacher confirms, with no re-check here (or back in
''' the page) that the signed-in staff member actually holds a role allowed to approve a
''' waiver, or that the requested percentage is within whatever cap that role would be limited
''' to.
''' </summary>
Public Class HardshipWaiverService

    ''' <summary>
    ''' Instance 3 (Interprocedural, cross-file): applies the waiver exactly as the caller
    ''' states it, trusting waiverApproved/waiverPct instead of independently re-deriving the
    ''' calling staff member's own approval authority and cap from the database.
    ''' </summary>
    Public Shared Sub ApplyWaiver(ByVal studentAdmNo As String, ByVal sessionId As String, ByVal waiverApproved As Boolean, ByVal waiverPct As Double, ByVal staffId As String)
        If Not waiverApproved Then
            Return
        End If
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            ' SINK: PLANTED-VB-HR-478
            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into discount (student, fee, type, amount, recurring, session) Values (?,?,?,?,?,?)", con)
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", studentAdmNo))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "HARDSHIP WAIVER"))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Percentage"))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", waiverPct))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("recurring", False))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", sessionId))
            cmdInsert.ExecuteNonQuery()
            con.Close()
        End Using
    End Sub

    ''' <summary>
    ''' Safe twin of <see cref="ApplyWaiver"/>: independently re-derives BOTH whether a waiver
    ''' should fire and what percentage to grant, purely from the database -- the same
    ''' outstanding-balance check btnPreviewWaiver_Click itself runs, capped by the calling staff
    ''' member's own recorded designation. waiverApproved and requestedPct are accepted on the
    ''' signature only so the call site still looks realistic; neither value is read again after
    ''' this point, so neither one ever reaches the INSERT below.
    ''' </summary>
    Public Shared Sub ApplyWaiverSafe(ByVal studentAdmNo As String, ByVal sessionId As String, ByVal waiverApproved As Boolean, ByVal requestedPct As Double, ByVal staffId As String)
        Using con As New MySql.Data.MySqlClient.MySqlConnection(ConfigurationManager.ConnectionStrings("PortalDBConnectionString").ConnectionString)
            con.Open()
            Dim cmdOut As New MySql.Data.MySqlClient.MySqlCommand("Select amount, paid from feeschedule where session = ? and student = ?", con)
            cmdOut.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("Session", sessionId))
            cmdOut.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", studentAdmNo))
            Dim readerOut As MySql.Data.MySqlClient.MySqlDataReader = cmdOut.ExecuteReader()
            Dim outstanding As Double = 0
            Do While readerOut.Read()
                outstanding = outstanding + (Val(readerOut.Item("amount")) - Val(readerOut.Item("paid")))
            Loop
            readerOut.Close()

            If outstanding <= 0 Then
                ' No outstanding balance -- same real-eligibility gate btnPreviewWaiver_Click
                ' itself applies, re-run here from scratch rather than trusting waiverApproved.
                con.Close()
                Return
            End If

            Dim cmdRole As New MySql.Data.MySqlClient.MySqlCommand("Select designation from staffprofile where staffid = ?", con)
            cmdRole.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("staffid", staffId))
            Dim readerRole As MySql.Data.MySqlClient.MySqlDataReader = cmdRole.ExecuteReader()
            Dim staffCapPct As Double = 0
            If readerRole.Read() AndAlso readerRole.Item("designation").ToString() = "Accounts" Then
                staffCapPct = 100
            Else
                staffCapPct = 5
            End If
            readerRole.Close()

            ' Independently-derived suggested waiver -- the same 10% btnPreviewWaiver_Click
            ' itself suggests -- capped by the calling staff member's own real authority.
            Dim suggestedPct As Double = 10
            Dim grantedPct As Double = suggestedPct
            If grantedPct > staffCapPct Then
                grantedPct = staffCapPct
            End If

            ' SAFE_SINK: PLANTED-VB-HR-478-safe
            Dim cmdInsert As New MySql.Data.MySqlClient.MySqlCommand("Insert Into discount (student, fee, type, amount, recurring, session) Values (?,?,?,?,?,?)", con)
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("student", studentAdmNo))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("fee", "HARDSHIP WAIVER"))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("type", "Percentage"))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("amount", grantedPct))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("recurring", False))
            cmdInsert.Parameters.Add(New MySql.Data.MySqlClient.MySqlParameter("session", sessionId))
            cmdInsert.ExecuteNonQuery()
            con.Close()
        End Using
    End Sub

End Class
