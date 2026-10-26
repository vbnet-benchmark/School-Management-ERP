<%@ Page Title="" Language="VB" MasterPageFile="~/DigiDashboard.master" AutoEventWireup="false" CodeFile="studentdashboard.aspx.vb" Inherits="dashboard" %>

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
                                        <h1>Undone Assignments</h1>
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
      

    
       <div class="author-progress-pro-area mg-t-30 mg-b-40 " style="text-align:center;">
                <div class="container-fluid">
                    <div class="row">
                        <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
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
                        <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
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

                          <div class="col-lg-4 col-md-3 col-sm-6 col-xs-12">
                            <div class="single-skill widget-ov-mg-t-30 widget-ov-mg-t-n-30 shadow-reset">
                                <div class="row">
                                    
                                    <div class="col-lg-12">
                                        <div class="progress-circular3">
                                            <h2><asp:Label runat="server" id="lblPublishedCA" ></asp:Label></h2>
                                            <p>Published</p>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        </div>
                                         </div>
                </div>
            </div>
   
</asp:Content>

