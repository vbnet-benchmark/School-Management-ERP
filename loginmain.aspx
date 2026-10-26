<%@ Page Language="VB" AutoEventWireup="false" CodeFile="loginmain.aspx.vb" Inherits="loginmain" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="box-sizing: border-box; display:block; width:100%; height:100%;">
<head runat="server">
    <title></title>
    <style>
.btn {
  opacity: 1;
  transition: 0.3s;
  cursor:pointer;
}

.btn:hover {opacity: 0.6}
</style>

</head>
<body style="box-sizing: border-box; display:block; width:100%; height:100%;">
    <form id="form1" style="box-sizing: border-box; width:100%; height:100%;" runat="server">
        <div id="iid" style=" background-image:url('img/backgr.png'); background-repeat:no-repeat; background-size:cover; background-position:center; background-attachment: fixed; box-sizing: border-box; width:100%; height:100%; min-height:602px;">
         
    <div id ="ifi4" style="box-sizing: border-box; width:100%; height:100%; padding:5%;">
    <div id ="Div1" style="box-sizing: border-box;  width:100%; height:100%; box-shadow: 10px 10px #e4e4e4, 5px 5px 5px #e4e4e4 inset;">
         <div id ="Div2" style="box-sizing: border-box;  width:50%; height:100%; float:left;">
             <asp:Image ID="Image1" ImageUrl="~/img/digital.png" style="padding-top:30%; padding-left:30%;" runat="server" />
    </div>
         <div id ="Div3" style="box-sizing: border-box; padding-top:11%; padding-left:10%; padding-right:15%;  width:50%; height:100%; float:left; font-family:'Arial Rounded MT'; color:white;">
        
              <div class="row">
                                        <div class="col-lg-4">
                                            <div class="login-input-head" ">
                                                <p id="prd" style="margin-bottom :12px;">User Name</p>
                                            </div>
                                        </div>
                                        <div class="col-lg-8">
                                            <div class="login-input-area">
                                               <asp:TextBox ID="txtID" style="border-style:none; border-radius:5px; height:30px; width:100%;" runat="server"></asp:TextBox>
                                               
                                            </div>
                                        </div>
                                    </div>
                                    <div class="row">
                                        <div class="col-lg-4">
                                            <div class="login-input-head">
                                                <p id="prdf" style="margin-bottom :12px;">Password</p>
                                            </div>
                                        </div>
                                        <div class="col-lg-8">
                                            <div class="login-input-area">
                                               <asp:TextBox ID="txtPassword"  runat="server" TextMode="Password" style="border-style:none; border-radius:5px; height:30px; width:100%;"></asp:TextBox>
                                                <i class="fa fa-lock login-user"></i>
                                            </div>
                                          
                                            <br />
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <div class="login-keep-me">
                                                        <label class="checkbox">
                                                            <input type="checkbox" name="remember" checked><i></i>Keep me logged in
                                                        </label>
                                                    </div>
                                                </div>
                                            </div>
                                            
                                            <br />
                                             <div class="row">
                                                <div class="col-lg-12">
                                                   
                                                    <div class="forgot-password" id="iui" style="text-align:right; color:#4d7cfe;">
                                                        <a>
                                                            <asp:linkbutton id="lnkForgot"  runat="server" style="color:#4d7cfe;" >Forgot Password?</asp:linkbutton>
                                                        </a>
                                                    </div>
                                                </div>
                                            </div>
                                        </div>
                                    </div>
             <br />
                                    <div class="row">
                                        <div class="col-lg-4">

                                        </div>
                                        <div class="col-lg-8">
                                            <div class="login-button-pro">
    <asp:Button ID="btnLogin" runat="server" class="login-button login-button-lg" Text="Login" CssClass="btn" style="background-color:#4d7cfe; width:100%; height:30px; border-radius:5px; color:white; border-style:none; hover {opacity: 0.6}" />
                                            </div>
                                        </div>
                                    </div>
                               

            




    </div>
    </div> 
    </div>
           

        </div>
    </form>
</body>
</html>
