<%@ Page Title="" Language="VB" MasterPageFile="~/DigiDashboard.master" AutoEventWireup="false" CodeFile="staffdashboard.aspx.vb" Inherits="dashboard" %>




<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
    <style type="text/css">
        .dash-frame {
           background-color: #2586c7;
    height: 40px;
    text-align: center;
    vertical-align: middle;
     padding-top: 10px;
     color: white;
        }
    </style>
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
       <div class="row">
    <div id="Div4" style="float:left; font-family:Cambria; text-align:left; font-size:12pt; color:#c6c6c6; overflow-wrap:normal;padding-top:3px;  padding-left:30px; padding-bottom: 5px;">
                             <asp:Label ID="Label1" text="User Statistics" runat="server" style="margin-bottom:5px;" ></asp:Label></div></div>
      <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="jfh" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblStudentNo" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="jfhf" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">person</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="jfhfy" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Active Students this term" id="Label2" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                       <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div1" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblParentNo" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div2" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">person_pin</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div3" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Active Parents" id="Label4" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>

                         <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div5" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblAdminNo" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div6" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">school</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div7" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Admins" id="Label5" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                       <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div8" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblAccountNo" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div9" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">business</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div10" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Accountants" id="Label6" ></asp:Label>

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
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div11" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblDeptHeads" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div12" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">redeem</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div13" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Department Heads" id="Label7" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div14" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblSchoolHeads" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div15" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">panorama</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div16" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="School Heads" id="Label8" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                          <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div17" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblClassNo" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div18" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">class</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div19" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Class Teachers" id="Label9" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                       <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div20" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblSubjectNo" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div21" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">bookmark</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div22" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Subject Teachers" id="Label10" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                    </div>
                </div>
            </div>
    </asp:Panel>
     <asp:Panel id="account" visible="false" runat="server">
        <div class="row">
    <div id="Div23" style="float:left; font-family:Cambria; text-align:left; font-size:12pt; color:#c6c6c6; overflow-wrap:normal;padding-top:3px;  padding-left:30px; padding-bottom: 5px;">
                             <asp:Label ID="Label3" text="Income Summary" runat="server" style="margin-bottom:5px;" ></asp:Label></div></div>
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
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div28" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblActiveStudents" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div29" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">person</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div30" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Active Students" id="Label13" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                         <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div34" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblActiveStaff" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div35" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">person_outline</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div36" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Active Staff" id="Label15" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>

                          <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div25" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblDebtors" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div26" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">money_off</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div27" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Debtors" id="Label12" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div31" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblProfit" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div32" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">monetization_on</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div33" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Profit" id="lblProfit567" ></asp:Label>

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
                            <div class="welcome-wrapper shadow-reset res-mg-t mg-b-30 sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px; border-style: none; margin-bottom:0;">
                                
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
        <br />


     <asp:Panel id="classteacher" visible="false" runat="server">
    <asp:Panel id="class1" runat="server">
        <div class="row">
    <div id="Div24" style="float:left; font-family:Cambria; text-align:left; font-size:12pt; color:#c6c6c6; overflow-wrap:normal;padding-top:3px;  padding-left:30px; padding-bottom: 5px;">
                             <asp:Label ID="lblClass1" runat="server" style="margin-bottom:5px;" ></asp:Label> Class Statistics</div></div>
      
       <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div43" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblClassStu" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div44" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">person</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div45" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Students in Class" id="Label18" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div56" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblClassSubjects" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div57" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">bookmark</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div58" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Subjects Offerred" id="Label22" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>

                         <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div46" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblSubjectTeachers" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div47" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">person_outline</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div48" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Subject Teachers" id="Label19" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                         <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div49" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblClassDebtors" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div50" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">money_off</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div51" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Debtors" id="Label20" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                    
                    </div>
                </div>
            </div>
        </asp:Panel>
     <asp:Panel id="class2" visible="false" runat="server">
          <div class="row">
    <div id="Div55" style="float:left; font-family:Cambria; text-align:left; font-size:12pt; color:#c6c6c6; overflow-wrap:normal;padding-top:3px;  padding-left:30px; padding-bottom: 5px;">
                             <asp:Label ID="lblClass2" runat="server" style="margin-bottom:5px;" ></asp:Label> Class Statistics</div></div>
       <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                              <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div37" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblClassStu2" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div38" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">person</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div39" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Students in Class" id="Label16" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                         <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div59" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblClassSubjects2" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div60" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">bookmark</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div61" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Subject Teachers" id="Label23" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>

                                 <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div40" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblClassSubjectTeachers2" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div41" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">person_outline</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div42" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Subject Teachers" id="Label17" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                         <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div52" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblClassDebtors2" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div53" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">money_off</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div54" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Debtors" id="Label21" ></asp:Label>

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
         <div id="Div65" style="float:left; font-family:Cambria; text-align:left; font-size:12pt; color:#c6c6c6; overflow-wrap:normal;padding-top:3px;  padding-left:30px; padding-bottom: 5px;">
                             <asp:Label ID="lblDept" runat="server" style="margin-bottom:5px;" ></asp:Label> Dept Statistics</div>
        <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                         <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div62" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblDeptStaff" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div63" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">person</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div64" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Department Staff" id="Label24" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                       <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div66" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblTeachingStaff" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div67" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">person</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div68" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Teaching Staff" id="Label25" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>

                       
                    
                    </div>
                </div>
            </div>
         </asp:Panel> 
     <asp:Panel id="schoolhead" visible="false" runat="server">
           <div id="Div81" style="float:left; font-family:Cambria; text-align:left; font-size:12pt; color:#c6c6c6; overflow-wrap:normal;padding-top:3px;  padding-left:30px; padding-bottom: 5px;">
                             <asp:Label ID="lblSchool" runat="server" style="margin-bottom:5px;" ></asp:Label> Statistics</div>
     <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div69" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblSchoolStudents" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div70" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">person</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div71" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Active Students this term" id="Label26" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div72" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblSchoolStaff" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div73" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">person_pin</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div74" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Active Staff" id="Label27" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>

                           <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div75" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblSchoolTeachers" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div76" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">person_outline</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div77" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Subject Teachers" id="Label28" ></asp:Label>

                                        </div>
                                    </div>
                                
                            </div>
                        </div>
                        <div class="col-lg-3 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset" >
                                <div class="row">
                                   <div class="col-lg-6" id="Div78" style="text-align:left; color:#c6c6c6; font-family:Cambria; font-size:26px;">
                                        
                                            <asp:Label runat="server" style="text-align:left;" id="lblClassTeachers" ></asp:Label>
                                            
                                       
                                    </div>
                                    <div class="col-lg-6" id="Div79" style="text-align:right;">
                                        <span><i class="material-icons" style="color:#c6c6c6; font-size:40px;">class</i></span>

                                    </div>
                                    </div>
                                    <br />
                                <br />
                                    <div class="row">

                                        <div class="col-lg-12" id="Div80" style="text-align:left;">
                                           <asp:Label runat="server" style="text-align:left; color:#c6c6c6; font-size:12px;" text="Class Teachers" id="Label29" ></asp:Label>

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

