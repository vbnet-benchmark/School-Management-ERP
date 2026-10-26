<%@ Page Title="" Language="VB" MasterPageFile="~/DigiDashboard.master" AutoEventWireup="false" CodeFile="parentdashboard.aspx.vb" Inherits="dashboard" %>

<asp:Content ID="Content1" ContentPlaceHolderID="head" Runat="Server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
   
    
   <asp:Panel runat="server" id="pnl1" visible="false">
   <h4><asp:Label runat="server" id="lblChild1"></asp:Label> STATS</h4>
    
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
                                        <asp:PlaceHolder id="plcSchedule1" runat="server"></asp:PlaceHolder>
                                       
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
                                        <h1>Undone Assignments</h1>
                                        <div class="sparkline11-outline-icon">
                                            <span class="sparkline11-collapse-link"><i class="fa fa-chevron-up"></i></span>
                                            <span><i class="fa fa-wrench"></i></span>
                                            <span class="sparkline11-collapse-close"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px;">

                                    <asp:PlaceHolder runat="server" id="plcSubmissions1"></asp:PlaceHolder>


                                </div>
                            </div>
                        </div>
        </div> 
      

    
       <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset">
                                <div class="row">
                                   <div class="col-lg-12">
                                        <div class="progress-circular1">
                                            <h2><asp:Label runat="server" id="lblClassStu1" ></asp:Label></h2>
                                            <p>Students in Class</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 shadow-reset">
                                <div class="row">
                                      <div class="col-lg-12">
                                        <div class="progress-circular2">
                                            <h2><asp:Label runat="server" id="lblClassSubjects1" ></asp:Label></h2>
                                            <p>Subjects Offerred</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                          <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                    
                                    <div class="col-lg-12">
                                        <div class="progress-circular3">
                                            <h2>N<asp:Label runat="server" id="lblOutstanding1" ></asp:Label></h2>
                                            <p>Owed</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                                         </div>
                </div>
            </div>
   </asp:Panel> 
      <asp:Panel runat="server" id="pnl2" visible="false">
    <h4><asp:Label runat="server" id="lblChild2"></asp:Label></h4>

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
                                        <asp:PlaceHolder id="plcSchedule2" runat="server"></asp:PlaceHolder>
                                       
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
                                        <h1>Undone Assignments</h1>
                                        <div class="sparkline11-outline-icon">
                                            <span class="sparkline11-collapse-link"><i class="fa fa-chevron-up"></i></span>
                                            <span><i class="fa fa-wrench"></i></span>
                                            <span class="sparkline11-collapse-close"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px;">

                                    <asp:PlaceHolder runat="server" id="plcSubmissions2"></asp:PlaceHolder>


                                </div>
                            </div>
                        </div>
        </div> 
      

    
       <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
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
                        <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
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

                          <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                    
                                    <div class="col-lg-12">
                                        <div class="progress-circular3">
                                            <h2>N<asp:Label runat="server" id="lblOutstanding2" ></asp:Label></h2>
                                            <p>Owed</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                                         </div>
                </div>
            </div>
          </asp:Panel> 
      <asp:Panel runat="server" id="pnl3" visible="false">
    <h4><asp:Label runat="server" id="lblchild3"></asp:Label> STATS</h4>

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
                                        <asp:PlaceHolder id="plcSchedule3" runat="server"></asp:PlaceHolder>
                                       
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
                                        <h1>Undone Assignments</h1>
                                        <div class="sparkline11-outline-icon">
                                            <span class="sparkline11-collapse-link"><i class="fa fa-chevron-up"></i></span>
                                            <span><i class="fa fa-wrench"></i></span>
                                            <span class="sparkline11-collapse-close"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px;">

                                    <asp:PlaceHolder runat="server" id="plcSubmissions3"></asp:PlaceHolder>


                                </div>
                            </div>
                        </div>
        </div> 
      

    
       <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset">
                                <div class="row">
                                   <div class="col-lg-12">
                                        <div class="progress-circular1">
                                            <h2><asp:Label runat="server" id="lblClassStu3" ></asp:Label></h2>
                                            <p>Students in Class</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 shadow-reset">
                                <div class="row">
                                      <div class="col-lg-12">
                                        <div class="progress-circular2">
                                            <h2><asp:Label runat="server" id="lblClassSubjects3" ></asp:Label></h2>
                                            <p>Subjects Offerred</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                          <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                    
                                    <div class="col-lg-12">
                                        <div class="progress-circular3">
                                            <h2>N<asp:Label runat="server" id="lblOutstanding3" ></asp:Label></h2>
                                            <p>Owed</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                                         </div>
                </div>
            </div>

          </asp:Panel> 
      <asp:Panel runat="server" id="pnl4" visible="false">
    <h4><asp:Label runat="server" id="lblChild4"></asp:Label> STATS</h4>
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
                                        <asp:PlaceHolder id="plcSchedule4" runat="server"></asp:PlaceHolder>
                                       
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
                                        <h1>Undone Assignments</h1>
                                        <div class="sparkline11-outline-icon">
                                            <span class="sparkline11-collapse-link"><i class="fa fa-chevron-up"></i></span>
                                            <span><i class="fa fa-wrench"></i></span>
                                            <span class="sparkline11-collapse-close"><i class="fa fa-times"></i></span>
                                        </div>
                                    </div>
                                </div>
                                <div class="sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px;">

                                    <asp:PlaceHolder runat="server" id="plcSubmissions4"></asp:PlaceHolder>


                                </div>
                            </div>
                        </div>
        </div> 
      

    
       <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill shadow-reset">
                                <div class="row">
                                   <div class="col-lg-12">
                                        <div class="progress-circular1">
                                            <h2><asp:Label runat="server" id="lblClassStu4" ></asp:Label></h2>
                                            <p>Students in Class</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 shadow-reset">
                                <div class="row">
                                      <div class="col-lg-12">
                                        <div class="progress-circular2">
                                            <h2><asp:Label runat="server" id="lblClassSubjects4" ></asp:Label></h2>
                                            <p>Subjects Offerred</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>

                          <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                    
                                    <div class="col-lg-12">
                                        <div class="progress-circular3">
                                            <h2>N<asp:Label runat="server" id="lblOutstanding4" ></asp:Label></h2>
                                            <p>Owed</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                                         </div>
                </div>
            </div>
          </asp:Panel> 
</asp:Content>

