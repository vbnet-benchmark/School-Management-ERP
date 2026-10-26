Imports Microsoft

Public Class MyMaster
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        lblStudent.Text = Session("StudentID").ToString
        'lblName.Text = Session("Fullname").ToString
    End Sub

    Protected Sub LinkButton1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles LinkButton1.Click
        Session.Abandon()
        FormsAuthentication.SignOut()
        Response.Redirect("SLogin.aspx")
    End Sub
End Class