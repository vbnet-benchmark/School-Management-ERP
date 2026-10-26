<%@ Page Title="" Language="VB" MasterPageFile="~/DigiDashboard.master" AutoEventWireup="false" CodeFile="staffdashboard.aspx.vb" Inherits="dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
     <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="lnkViewDashboard" visible="false" class="btn btn-white btn-xs" runat="server" Text="View DashBoard" ></asp:Button></div></div> </div> </div> 
    <br />
    <asp:Panel ID="pnlEntire"   runat="server">
    <asp:Panel id="admins" visible="false" runat="server">
    <h4>
       
       USER STATISTICS</h4>
      <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset">
                                <div class="row">
                                   <div class="col-lg-12">
                                        <div class="progress-circular1">
                                            <h2><asp:Label runat="server" id="lblStudentNo" ></asp:Label></h2>
                                            <p>Active Students this term</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 shadow-reset">
                                <div class="row">
                                      <div class="col-lg-12">
                                        <div class="progress-circular2">
                                            <h2><asp:Label runat="server" id="lblParentNo" ></asp:Label></h2>
                                            <p>Active Parents</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                          <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                    
                                    <div class="col-lg-12">
                                        <div class="progress-circular3">
                                            <h2><asp:Label runat="server" id="lblAdminNo" ></asp:Label></h2>
                                            <p>Admins</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                   
                                    <div class="col-lg-12">
                                        <div class="progress-circular4">
                                            <h2><asp:Label runat="server" id="lblAccountNo" ></asp:Label></h2>
                                            <p>Accountants</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    
                    </div>
                </div>
            </div>

      <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset">
                                <div class="row">
                                   <div class="col-lg-12">
                                        <div class="progress-circular1">
                                            <h2><asp:Label runat="server" id="lblDeptHeads" ></asp:Label></h2>
                                            <p>Department Heads</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 shadow-reset">
                                <div class="row">
                                      <div class="col-lg-12">
                                        <div class="progress-circular2">
                                            <h2><asp:Label runat="server" id="lblSchoolHeads" ></asp:Label></h2>
                                            <p>School Heads</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                          <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                    
                                    <div class="col-lg-12">
                                        <div class="progress-circular3">
                                            <h2><asp:Label runat="server" id="lblClassNo" ></asp:Label></h2>
                                            <p>Class Teachers</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                   
                                    <div class="col-lg-12">
                                        <div class="progress-circular4">
                                            <h2><asp:Label runat="server" id="lblSubjectNo" ></asp:Label></h2>
                                            <p>Subject Teachers</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </asp:Panel>
     <asp:Panel id="account" visible="false" runat="server">
    <h4>INCOME SUMMARY</h4>
    <br />
      <div class="income-order-visit-user-area">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="income-dashone-total shadow-reset nt-mg-b-30">
                                <div class="income-title">
                                    <div class="main-income-head">
                                        <h2>Expected</h2>
                                        <div class="main-income-phara">
                                            <p>Termly</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="income-dashone-pro">
                                    <div class="income-rate-total">
                                        <div class="price-adminpro-rate">
                                            <h3><span>N</span><span class="counter"><asp:Label runat="server" id="lblTotalInc"></asp:Label></span></h3>
                                        </div>
                                       
                                    </div>
                                    <div class="income-range">
                                        <p>Total income</p>
                                        
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="income-dashone-total shadow-reset nt-mg-b-30">
                                <div class="income-title">
                                    <div class="main-income-head">
                                        <h2>Realised</h2>
                                        <div class="main-income-phara order-cl">
                                            <p>Termly</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="income-dashone-pro">
                                    <div class="income-rate-total">
                                        <div class="price-adminpro-rate">
                                            <h3><span>N</span><span class="counter"><asp:Label runat="server" id="lblRealisedINnc"></asp:Label></span></h3>
                                        </div>
                                       
                                    </div>
                                    <div class="income-range order-cl">
                                        <p>Received Income</p>
                                       
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="income-dashone-total shadow-reset nt-mg-b-30">
                                <div class="income-title">
                                    <div class="main-income-head">
                                        <h2>Unrealised</h2>
                                        <div class="main-income-phara visitor-cl">
                                            <p>Termly</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="income-dashone-pro">
                                    <div class="income-rate-total">
                                        <div class="price-adminpro-rate">
                                            <h3><span>N</span><span class="counter"><asp:Label runat="server" id="lblUnrealisedInc"></asp:Label></span></h3>
                                        </div>
                                       
                                    </div>
                                    <div class="income-range visitor-cl">
                                        <p>Unreceived Income</p>
                                       
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="income-dashone-total shadow-reset nt-mg-b-30">
                                <div class="income-title">
                                    <div class="main-income-head">
                                        <h2>Expenses</h2>
                                        <div class="main-income-phara low-value-cl">
                                            <p>Termly</p>
                                        </div>
                                    </div>
                                </div>
                                <div class="income-dashone-pro">
                                    <div class="income-rate-total">
                                        <div class="price-adminpro-rate">
                                            <h3><span>N</span><span class="counter"><asp:Label runat="server" id="lblExpenses"></asp:Label></span></h3>
                                        </div>
                                        
                                    </div>
                                    <div class="income-range low-value-cl">
                                        <p>Expense Total</p>
                                      
                                    </div>
                                    <div class="clear"></div>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>
            </div>
     <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset">
                                <div class="row">
                                   <div class="col-lg-12">
                                        <div class="progress-circular1">
                                            <h2><asp:Label runat="server" id="lblActivestudents" ></asp:Label></h2>
                                            <p>Active Students</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 shadow-reset">
                                <div class="row">
                                      <div class="col-lg-12">
                                        <div class="progress-circular2">
                                            <h2><asp:Label runat="server" id="lblActivestaff" ></asp:Label></h2>
                                            <p>Active Staff</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                          <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                    
                                    <div class="col-lg-12">
                                        <div class="progress-circular3">
                                            <h2><asp:Label runat="server" id="lblDebtors" ></asp:Label></h2>
                                            <p>Debtors</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                   
                                    <div class="col-lg-12">
                                        <div class="progress-circular4">
                                            <h2><asp:Label runat="server" id="lblProfit" ></asp:Label></h2>
                                            <p>Profit</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    
                    </div>
                </div>
            </div>
         </asp:Panel> 

     <asp:Panel id="subjectteacher" visible="false" runat="server">
    <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
        <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0;">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>Today's Schedule</h1>
                                        <div class="sparkline11-outline-icon">
                                            <span class="sparkline11-collapse-link"><i class="fa fa-chevron-up"></i></span>
                                            <span><i class="fa fa-wrench"></i></span>
                                            <span class="sparkline11-collapse-close"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                            <div class="welcome-wrapper shadow-reset res-mg-t mg-b-30 sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px;">
                                
                                <div class="adminpro-message-list">
                                    <ul class="message-list-menu table">
                                        <asp:PlaceHolder id="plcSchedule" runat="server"></asp:PlaceHolder>
                                       
                                    </ul>
                                </div>
                            </div>
                        </div>
        </div> 
    <div class="row">
                        

        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" >
                            <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0;">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>Recent Assignment Submissions</h1>
                                        <div class="sparkline11-outline-icon">
                                            <span class="sparkline11-collapse-link"><i class="fa fa-chevron-up"></i></span>
                                            <span><i class="fa fa-wrench"></i></span>
                                            <span class="sparkline11-collapse-close"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px;">

                                    <asp:PlaceHolder runat="server" id="plcSubmissions"></asp:PlaceHolder>


                                </div>
                            </div>
                        </div>
        </div> 
        </asp:Panel> 


     <asp:Panel id="classteacher" visible="false" runat="server">
    <asp:Panel id="class1" runat="server">
    <h4><asp:Label ID="lblClass1" runat="server"></asp:Label> CLASS STATISTICS</h4>
       <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset">
                                <div class="row">
                                   <div class="col-lg-12">
                                        <div class="progress-circular1">
                                            <h2><asp:Label runat="server" id="lblClassStu" ></asp:Label></h2>
                                            <p>Students in Class</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 shadow-reset">
                                <div class="row">
                                      <div class="col-lg-12">
                                        <div class="progress-circular2">
                                            <h2><asp:Label runat="server" id="lblClassSubjects" ></asp:Label></h2>
                                            <p>Subjects Offerred</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                          <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                    
                                    <div class="col-lg-12">
                                        <div class="progress-circular3">
                                            <h2><asp:Label runat="server" id="lblSubjectTeachers" ></asp:Label></h2>
                                            <p>Subject Teachers</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                   
                                    <div class="col-lg-12">
                                        <div class="progress-circular4">
                                            <h2><asp:Label runat="server" id="lblClassDebtors" ></asp:Label></h2>
                                            <p>Debtors</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    
                    </div>
                </div>
            </div>
        </asp:Panel>
     <asp:Panel id="class2" visible="false" runat="server">
    <h4><asp:Label ID="lblClass2" runat="server"></asp:Label> CLASS STATISTICS</h4>
       <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset">
                                <div class="row">
                                   <div class="col-lg-12">
                                        <div class="progress-circular1">
                                            <h2><asp:Label runat="server" id="lblClassStu2" ></asp:Label></h2>
                                            <p>Students in Class</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 shadow-reset">
                                <div class="row">
                                      <div class="col-lg-12">
                                        <div class="progress-circular2">
                                            <h2><asp:Label runat="server" id="lblClassSubjects2" ></asp:Label></h2>
                                            <p>Subjects Offerred</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                          <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                    
                                    <div class="col-lg-12">
                                        <div class="progress-circular3">
                                            <h2><asp:Label runat="server" id="lblClassSubjectTeachers2" ></asp:Label></h2>
                                            <p>Subject Teachers</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                   
                                    <div class="col-lg-12">
                                        <div class="progress-circular4">
                                            <h2><asp:Label runat="server" id="lblClassDebtors2" ></asp:Label></h2>
                                            <p>Debtors</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    
                    </div>
                </div>
            </div>
        </asp:Panel>
         </asp:Panel> 
   

     <asp:Panel id="departmenthead" visible="false" runat="server">
    <h4><asp:Label runat="server" id="lblDept"> </asp:Label> DEPARTMENT STATS</h4>
        <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-6 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset">
                                <div class="row">
                                   <div class="col-lg-12">
                                        <div class="progress-circular1">
                                            <h2><asp:Label runat="server" id="lblDeptStaff" ></asp:Label></h2>
                                            <p>Department Staff</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-6 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 shadow-reset">
                                <div class="row">
                                      <div class="col-lg-12">
                                        <div class="progress-circular2">
                                            <h2><asp:Label runat="server" id="lblTeachingStaff" ></asp:Label></h2>
                                            <p>Teaching Staff</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                       
                    
                    </div>
                </div>
            </div>
         </asp:Panel> 
     <asp:Panel id="schoolhead" visible="false" runat="server">
    <h4><asp:Label runat="server" id="lblSchool"></asp:Label> STATS</h4>
     <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset">
                                <div class="row">
                                   <div class="col-lg-12">
                                        <div class="progress-circular1">
                                            <h2><asp:Label runat="server" id="lblSchoolStudents" ></asp:Label></h2>
                                            <p>Active Students this term</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 shadow-reset">
                                <div class="row">
                                      <div class="col-lg-12">
                                        <div class="progress-circular2">
                                            <h2><asp:Label runat="server" id="lblSchoolStaff" ></asp:Label></h2>
                                            <p>Active Staff</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                          <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                    
                                    <div class="col-lg-12">
                                        <div class="progress-circular3">
                                            <h2><asp:Label runat="server" id="lblSchoolTeachers" ></asp:Label></h2>
                                            <p>Subject Teachers</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                   
                                    <div class="col-lg-12">
                                        <div class="progress-circular4">
                                            <h2><asp:Label runat="server" id="lblClassTeachers" ></asp:Label></h2>
                                            <p>Class Teachers</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                    
                    </div>
                </div>
            </div>
         </asp:Panel> 
    <asp:PlaceHolder id="plcOverall" runat="server"></asp:PlaceHolder>
        </asp:Panel>
</asp:Content>

