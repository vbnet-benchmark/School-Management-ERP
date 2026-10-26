
Partial Class Staff_Main2
    Inherits System.Web.UI.MasterPage

    Protected Sub Page_Load(sender As Object, e As EventArgs) Handles Me.Load


        lblName.Text = Session("staff").ToString
        lblStaffid.Text = Session("StaffID").ToString

    End Sub
End Class

