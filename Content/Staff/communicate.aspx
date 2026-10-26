<%@ Page Title="" Language="VB"  MaintainScrollPositionOnPostBack="True" MasterPageFile="~/digidashboard.master" AutoEventWireup="false" CodeFile="communicate.aspx.vb" Inherits="Admin_studentprofile" %>
<asp:Content ID="Content4" ContentPlaceHolderID="ContentPlaceHolder1" Runat="Server">
  
  
    <h2>COMMUNICATION</h2>

     <br />

    <asp:Panel ID="pnlAll" runat="server" DefaultButton="button2">
                <div class="row">
                        <div class="col-lg-12">
                            <div class="sparkline12-list shadow-reset mg-t-30">
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                       <div class="row">
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
                                        
                                            
                                        <asp:Label ID="Label1" Font-Bold="true" Font-Size="Larger" runat="server" Text="Student List"></asp:Label>
											
                                       
                                    </div>
                                    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-6">
        <div class="breadcome-heading">
                                       <span> <asp:Textbox class="form-control" ID="txtSearch"   placeholder="Search..." runat="server" style="width:100%;" ></asp:TextBox>
												<a href="#" id="sb" ><asp:LinkButton TabIndex="0" style="float:right; right:20; top:1; " class="fa fa-search" ID="Button2" runat="server"></asp:LinkButton> </a></span>
                                        </div>
                                    </div>
                                </div>
                                       
                                    </div>
                                </div>
                                <div class="sparkline12-graph">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
            
              <br />
                                                <div class="form-group-inner">
                                                            <div class="row">
                                                                <div class="col-lg-3">
                                                                    <label class="login2 pull-right pull-right-pro">Class</label>
                                                                </div>
                                                                <div class="col-lg-9">
                                                                    <div class="form-select-list">
                                                                       <asp:DropDownList ID="DropDownList1" runat="server" class="form-control custom-select-value" autopostback="true">
                                                                                 
                                                                                </asp:DropDownList>
                                                                     </div>
                                                                </div>
                                                            </div>
                                                        </div>
          
     <div class="form-group-inner">
                                                            <div class="row">
                                                                
                                                                     
                                                                
                                                                                   


                                                               
                                                            </div>
                                                       
  

            
    

        
       
            <div class="comment-replay-profile">
                                                            <div class="btn-group" id="grid" style="width:100%;">  
 <asp:GridView runat="server" AutoGenerateColumns="False" showheader="False" ID="gridview1"  GridLines="Horizontal" class="newtable tableflat" AllowPaging="false">
            <Columns>
                <asp:ImageField DataImageUrlField="passport">
                    <controlstyle BorderStyle="None" Height="80px" Width="80px" />
                </asp:ImageField>
                <asp:BoundField DataField="staffname">
                    <ItemStyle Font-Size="16pt" />
                </asp:BoundField>
                
                
                <asp:CommandField SelectImageUrl="~/image/my-profile.png" SelectText="View" ShowSelectButton="True" ButtonType="Button">
                    <controlstyle cssclass="btn btn-white btn-xs btn-group comment-replay-profile" />
                </asp:CommandField>
            </Columns>
            <FooterStyle BackColor="#CCCC99" ForeColor="Black" />
            <HeaderStyle BackColor="#333333" Font-Bold="True" ForeColor="White" />
            <PagerStyle BackColor="White" ForeColor="Black" HorizontalAlign="Right" />
            <SelectedRowStyle BackColor="#CC3333" Font-Bold="True" ForeColor="White" />
            <sortedascendingcellstyle backcolor="#F7F7F7" />
            <sortedascendingheaderstyle backcolor="#4B4B4B" />
            <sorteddescendingcellstyle backcolor="#E5E5E5" />
            <sorteddescendingheaderstyle backcolor="#242121" />
    </asp:GridView>
            </div> </div> 
    
   <br />

         








</div>
                                        </div>
                                    </div>
                                </div>
                            </div>
                        
                    </div>
                     </div> 

                    </div> 
    </asp:Panel>
             
         
  <asp:Panel id="chatthread"  visible="false" runat="server">
       <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="btnStudents" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Back to Students" ></asp:Button></div></div> </div> </div> 
      <br />
       <div class="row">
                        <div class="col-lg-12">
                            <div >
                                <div class="sparkline12-hd">
                                    <div class="main-sparkline12-hd">
                                        <h1>CONVERSATION THREAD ON <asp:Label runat="server" id="lblstudent"></asp:Label></h1>
                                       
                                    </div>
                                </div>
                                <div id="scrolldiv" class="welcome-wrapper shadow-reset res-mg-t mg-b-30 sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:400px;  overflow:scroll; overflow-x:hidden; overflow-y:auto;">
                                    <div class="basic-login-form-ad">
                                        <div class="row">
                                            <div class="col-lg-12">
 <div class="all-form-element-inner">
      <div class="chat-list-wrap">
        <div class="chat-list-adminpro">
            
            <div >
                <div class="chat-main-list">
                    <asp:UpdateProgress ID="UpdateProgress6" runat="server" 
                AssociatedUpdatePanelID="UpdatePanel6">
            <ProgressTemplate>
                <div></div>
                </ProgressTemplate> 
                        </asp:UpdateProgress> 
                    <div class="chat-content chat-scrollbar" >
                          <asp:UpdatePanel ID="UpdatePanel6" runat="server" UpdateMode ="Conditional" >
                 
            <ContentTemplate>
                       <asp:Timer ID="Timer2" runat="server"  OnTick="Timer2_Tick" Interval ="25000"></asp:Timer> 
   
                       <asp:Literal id="literal1" runat="server"></asp:Literal>
                

          
                      </ContentTemplate> 
                              </asp:UpdatePanel> 
                    </div>
                    
            </div>
                               </div>
            </div>
        </div>
    </div>

     
                                        </div>
                                    </div>
                                </div>
      </div>                      </div>
                        
                    </div>
         </div> 

       <div id="contents1"   style="height:70px; border-style:solid; border-color:#f5f5f5; overflow:scroll; word-wrap:break-word;  overflow-x:hidden; overflow-y:auto;" contenteditable="true"></div>
               
                <br /> 
                <div class="col-lg-6">
  <div class="row">
                   <div class="col-lg-10">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">  <span id="subfm" style='height:50px; padding:8px;width:50px'><input id='Button1f'  onclick="showhideemoji()" style='background-color:transparent; border-style:none; font-size: 16px; vertical-align: middle; margin-top: 10px;' type='button' value='😃' /> <asp:Button ID="Button10" onclientclick="store()" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" text="Send"  />  <asp:Button ID="Button9" runat="server" visible="false" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Preset Message" /></span></div> </div> </div> </div> 
                    </div>
    
                     <asp:HiddenField id="Hidden1" runat="server"></asp:HiddenField>
                    
                     
                    <br />
                    <br />
    








      <input type="hidden" id="div_position" name="div_position" />
        




  </asp:Panel>
    <script src="../js/summernote.min.js"></script>
    <script src="../js/summernote-active.js"></script>
     <script>
         var div = document.getElementById('contents1');
         setTimeout(function () {
             div.focus();
         }, 0);
         function showhideemoji() {
             var div = document.getElementById('contents1');
             setTimeout(function () {
                 div.focus();
             }, 0);
             document.getElementById("emojibox").style.display = "block";

         }

         </script>
    <script>

        function store() {
           
            document.getElementById('<%=Hidden1.ClientID %>').value = document.getElementById("contents1").innerHTML.replace(/(<([^>]+)>)/ig, '');

        }

         </script>

         <script>

             function insertHtmlAtCursor(text) {
                 var selection = window.getSelection();
                 var range = selection.getRangeAt(0);
                 range.deleteContents();
                 var node = document.createTextNode(text);
                 range.insertNode(node);
                 selection.collapseToEnd();

                 document.getElementById("emojibox").style.display = "none";
                 document.getElementById('<%=Hidden1.ClientID %>').value = document.getElementById("contents1").innerHTML.replace(/(<([^>]+)>)/ig, '');

                     var div = document.getElementById('contents1');
                     setTimeout(function () {
                         div.focus();
                     }, 0);
                 }
         </script>
   
    
                
    <script type="text/javascript">
        function ScrollToBottom() {

            var divChat = document.getElementById('scrolldiv');

            divChat.scrollTop = divChat.scrollHeight;



        }
      </script>
            <asp:Panel runat="server" id ="panel3" visible="false" style="margin-top: 0px" >
                       <div class="row">
                   <div class="col-lg-5">
                                                        <div class="comment-replay-profile">
                                                            <div class="btn-group">         
    <asp:Button ID="Button16" class="btn btn-white btn-xs" TabIndex="8"  runat="server" Text="Back" ></asp:Button></div></div> </div> </div> 
                <br />
              <asp:Panel id="pnlEarly" runat="server">
    
                        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
        <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0;">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>DRESSING</h1>
                                        
                                    </div>
                                </div>
                            <div class="welcome-wrapper shadow-reset res-mg-t mg-b-30 sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px; background-image: url('../../img/noimage.jpg'); background-repeat: no-repeat; background-position-x: center;">
                                
                                    
                                   
                                <div class="adminpro-message-list" style="background-color:transparent;">
                                    <ul class="message-list-menu table" style="background-color:transparent;">
                                          <asp:RadioButtonList id="radParent" style="background-color:transparent;" runat="server" class="message-list-menu table"  >
                                            
                                        </asp:RadioButtonlist>
                                       
                                    </ul>
                                     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button3" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Send" /></div>
                                </div>
                                     
                                   
                            </div>
                        </div>
        </div> 
    <div class="row">
                        

        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" >
                            <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0;">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>SNACKS/FOOD</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px; background-image: url('../../img/noimage.jpg'); background-repeat: no-repeat; background-position-x: center;">

                                    
                                   
                                   
                                <div class="adminpro-message-list" style="background-color:transparent;">
                                    <ul class="message-list-menu table" style="background-color:transparent;">
                                          <asp:RadioButtonList id="RadioButtonList1" runat="server" class="message-list-menu table" style="background-color:transparent;" >
                                           
                                        </asp:RadioButtonlist>
                                       
                                    </ul>
                                     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button4" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Send" /></div>
                                </div>
                                      
                                   
                                </div>
                            </div>
                        </div>
        </div> 

                
                        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
        <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0;">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>PLAYTIME</h1>
                                        
                                    </div>
                                </div>
                            <div class="welcome-wrapper shadow-reset res-mg-t mg-b-30 sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px; background-image: url('../../img/noimage.jpg'); background-repeat: no-repeat; background-position-x: center;">
                                
                                  
                                   
                                <div class="adminpro-message-list" style="background-color:transparent;">
                                    <ul class="message-list-menu table" style="background-color:transparent;">
                                          <asp:RadioButtonList id="RadioButtonList2" runat="server" class="message-list-menu table" style="background-color:transparent;" >
                                           
                                        </asp:RadioButtonlist>
                                       
                                    </ul>
                                     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button5" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Send" /></div>
                                </div>
                                     
                                   
                            </div>
                        </div>
        </div> 
    <div class="row">
                        

        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" >
                            <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0;">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>NAP</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px; background-image: url('../../img/noimage.jpg'); background-repeat: no-repeat; background-position-x: center;">

                                    
                                    
                                   
                                <div class="adminpro-message-list" style="background-color:transparent;">
                                    <ul class="message-list-menu table" style="background-color:transparent;">
                                          <asp:RadioButtonList id="RadioButtonList3" runat="server" class="message-list-menu table" style="background-color:transparent;">
                                                                                   </asp:RadioButtonlist>
                                       
                                    </ul>
                                     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button6" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Send" /></div>
                                </div>
                                     
                                   
                                </div>
                            </div>
                        </div>
        </div> 

                
                        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
        <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0;">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>LEARNING</h1>
                                        
                                    </div>
                                </div>
                            <div class="welcome-wrapper shadow-reset res-mg-t mg-b-30 sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px; background-image: url('../../img/noimage.jpg'); background-repeat: no-repeat; background-position-x: center;">
                                
                                   
                                   
                                <div class="adminpro-message-list" style="background-color:transparent;">
                                    <ul class="message-list-menu table" style="background-color:transparent;">
                                          <asp:RadioButtonList id="RadioButtonList4" runat="server" class="message-list-menu table" style="background-color:transparent;" >
                                          
                                        </asp:RadioButtonlist>
                                       
                                    </ul>
                                     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button7" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Send" /></div>
                                </div>
                                     
                                   
                            </div>
                        </div>
        </div> 
    <div class="row">
                        

        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" >
                            <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0;">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>PUNCTUALITY/CONDUCT</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px; background-image: url('../../img/noimage.jpg'); background-repeat: no-repeat; background-position-x: center;">

                                    
                                    
                                   
                                <div class="adminpro-message-list" style="background-color:transparent;">
                                    <ul class="message-list-menu table" style="background-color:transparent;">
                                          <asp:RadioButtonList id="RadioButtonList5" runat="server" class="message-list-menu table" style="background-color:transparent;">
                                            
                                        </asp:RadioButtonlist>
                                       
                                    </ul>
                                     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button8" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Send" /></div>
                                </div>
                                      
                                   
                                </div>
                            </div>
                        </div>
        </div> 



                  <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
        <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0;">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>RELIGIOUS ACTIVITIES</h1>
                                        
                                    </div>
                                </div>
                            <div class="welcome-wrapper shadow-reset res-mg-t mg-b-30 sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px; background-image: url('../../img/noimage.jpg'); background-repeat: no-repeat; background-position-x: center;">
                                
                                   
                                   
                                <div class="adminpro-message-list" style="background-color:transparent;">
                                    <ul class="message-list-menu table" style="background-color:transparent;">
                                          <asp:RadioButtonList id="RadioButtonList7" runat="server" class="message-list-menu table" style="background-color:transparent;" >
                                           
                                        </asp:RadioButtonlist>
                                       
                                    </ul>
                                     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button11" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Send" /></div>
                                </div>
                                     
                                   
                            </div>
                        </div>
        </div> 

                 <div class="row">
                        

        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" >
                            <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0;">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>HOME WORK</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px; background-image: url('../../img/noimage.jpg'); background-repeat: no-repeat; background-position-x: center;">

                                    
                                    
                                    
                                <div class="adminpro-message-list" style="background-color:transparent;" >
                                    <ul class="message-list-menu table" style="background-color:transparent;">
                                          <asp:RadioButtonList id="RadioButtonList6" runat="server" class="message-list-menu table" style="background-color:transparent;" >
                                           
                                        </asp:RadioButtonlist>
                                       
                                    </ul>
                                     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button1" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Send" /></div>
                                </div>
                                      
                                   
                                </div>
                            </div>
                        </div>
        </div> 
                  </asp:Panel>
                <asp:Panel id="pnlnursery" runat="server">
                     <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
        <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0;">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>LEARNING</h1>
                                        
                                    </div>
                                </div>
                            <div class="welcome-wrapper shadow-reset res-mg-t mg-b-30 sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px; background-image: url('../../img/noimage.jpg'); background-repeat: no-repeat; background-position-x: center;">
                                
                                   
                                   
                                <div class="adminpro-message-list" style="background-color:transparent;">
                                    <ul class="message-list-menu table" style="background-color:transparent;">
                                          <asp:RadioButtonList id="RadioButtonList8" runat="server" class="message-list-menu table" style="background-color:transparent;" >
                                            
                                        </asp:RadioButtonlist>
                                       
                                    </ul>
                                     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button12" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Send" /></div>
                                </div>
                                     
                                   
                            </div>
                        </div>
        </div> 
    <div class="row">
                        

        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" >
                            <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0;">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>PLAYTIME</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px; background-image: url('../../img/noimage.jpg'); background-repeat: no-repeat; background-position-x: center;">

                                    
                                   
                                   
                                <div class="adminpro-message-list" style="background-color:transparent;">
                                    <ul class="message-list-menu table" style="background-color:transparent;">
                                          <asp:RadioButtonList id="RadioButtonList9" runat="server" class="message-list-menu table" style="background-color:transparent;" >
                                           
                                        </asp:RadioButtonlist>
                                       
                                    </ul>
                                     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button13" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Send" /></div>
                                </div>
                                      
                                   
                                </div>
                            </div>
                        </div>
        </div> 

                
                        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12">
        <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0;">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>ATTITUDE</h1>
                                        
                                    </div>
                                </div>
                            <div class="welcome-wrapper shadow-reset res-mg-t mg-b-30 sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px; background-image: url('../../img/noimage.jpg'); background-repeat: no-repeat; background-position-x: center;">
                                
                                  
                                   
                                <div class="adminpro-message-list" style="background-color:transparent;">
                                    <ul class="message-list-menu table" style="background-color:transparent;">
                                          <asp:RadioButtonList id="RadioButtonList10" runat="server" class="message-list-menu table" style="background-color:transparent;" >
                                           
                                        </asp:RadioButtonlist>
                                       
                                    </ul>
                                     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button14" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Send" /></div>
                                </div>
                                      
                                   
                            </div>
                        </div>
        </div> 
    <div class="row">
                        

        <div class="col-lg-6 col-md-6 col-sm-12 col-xs-12" >
                            <div class="sparkline11-list shadow-reset mg-tb-30" style="margin:0;">
                                <div class="sparkline11-hd">
                                    <div class="main-sparkline11-hd">
                                        <h1>FOOD/SNACKS</h1>
                                       
                                    </div>
                                </div>
                                <div class="sparkline11-graph dashone-comment dashtwo-comment comment-scrollbar" style="height:300px; background-image: url('../../img/noimage.jpg'); background-repeat: no-repeat; background-position-x: center;">

                                    
                                   
                                   
                                <div class="adminpro-message-list" style="background-color:transparent;">
                                    <ul class="message-list-menu table" style="background-color:transparent;">
                                          <asp:RadioButtonList id="RadioButtonList11" runat="server" class="message-list-menu table" style="background-color:transparent;">
                                                                                   </asp:RadioButtonlist>
                                       
                                    </ul>
                                     <div class="login-button-pro" style="text-align:right;" >   <asp:Button ID="Button15" runat="server" class="btn btn-sm btn-primary login-submit-cs buttonsnew" Text="Send" /></div>
                                </div>
                                      
                                   
                                </div>
                            </div>
                        </div>
        </div> 







                </asp:Panel>




                      
                 </asp:Panel>
    

       
    </asp:Content>