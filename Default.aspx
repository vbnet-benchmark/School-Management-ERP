<%@ Page Language="VB" AutoEventWireup="false" CodeFile="Default.aspx.vb" Inherits="_Default" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml" style="box-sizing: border-box; display:block; width:100%; height:100%;">
<head id="Head1" runat="server">
    <meta name="viewport" content="width=device-width, initial-scale=1" /> 
    <title>Login</title>
     <link rel="shortcut icon" type="image/x-icon" href="img/favicon.ico">
    <style>
.btn {
  opacity: 1;
  transition: 0.3s;
  cursor:pointer;
}
.alert-danger {
    color: #a94442;
    background-color: #f2dede;
    border-color: #ebccd1;
}

.alert {
            padding: 15px;
            margin-bottom: 20px;
            border: 1px solid transparent;
            border-radius: 4px;
        }
.btn:hover {opacity: 0.6}
        .logo {
            width:30%; 
            height:30%;
            padding-top:30%; 
            padding-left:30%;
        }
        .bod {
            width:50%;
            padding-top:11%; 
            padding-left:10%; 
            padding-right:15%; 
        }
        .logcover {
            height:100%;
            width:50%;
        }
        .iid {
            background-image: url('img/backgr.png');
            background-repeat: no-repeat;
            background-size: cover;
            background-position: right;
            background-attachment: fixed;
            box-sizing: border-box;
            width: 100%;
            height: 100%;
            min-height: 602px;
        }
        .bo1 {
            box-sizing: border-box;
            width: 100%;
            height: 100%;
            box-shadow: 10px 10px #e4e4e4, 5px 5px 5px #e4e4e4 inset;
        }
        @media screen and (max-width: 600px) {
            .logo {
                width: 100px; /* The width is 100%, when the viewport is 800px or smaller */
                height:100px;
                padding:5px;
               
            }
            .bod {
                width:100%;
                padding:10%;
                text-align:center;
            }
            .logcover {
                height:120px;
                width:100%;
                 text-align:center;
            }
            .bo1 {
                height: 70%;
            }
        }
</style>

</head>
<body style="box-sizing: border-box; display:block; width:100%; height:100%;">
    <form id="form1" style="box-sizing: border-box; width:100%; height:100%;" runat="server">
        <div id="iid" class="iid">
         
    <div id ="ifi4" style="box-sizing: border-box; width:100%; height:100%; padding:5%;">
    <div id ="Div1" class="bo1" >
         <div id ="Div2" class="logcover" style="box-sizing: border-box;  float:left;">
             <asp:Image ID="imgLogo" CssClass="logo" ImageUrl="~/img/digital.png"  runat="server" />
    </div>
         <div id ="Div3" class="bod"  style="box-sizing: border-box;   height:100%; float:left; font-family:'Arial Rounded MT'; color:white;">
        
              <div class="row">
                                        <div class="col-lg-4">
                                            <div class="login-input-head" ">
                                                <p id="prd" style="margin-bottom :12px; text-align:left;">User Name</p>
                                            </div>
                                        </div>
                                        <div class="col-lg-8">
                                            <div class="login-input-area">
                                               <asp:TextBox ID="txtID" style="border-style:none; border-radius:5px; height:30px; width:100%; padding-left:5px;" runat="server" Placeholder="Enter your user name"></asp:TextBox>
                                               
                                            </div>
                                        </div>
                                    </div>
             <asp:Panel ID="pnlNew" Visible =" false" runat="server">
                   <div class="col-lg-4">
                                            <div class="login-input-head">
                                                <p id="prddf" style="margin-bottom :12px; text-align:left;">Enter New Password</p>
                                            </div>
                                        </div>
                                        <div class="col-lg-8">
                                            <div class="login-input-area">
                                               <asp:TextBox ID="TextBox1"  runat="server" TextMode="Password" style="border-style:none; border-radius:5px; height:30px; width:100%;  padding-left:5px;" Placeholder="New Password"></asp:TextBox>
                                                <i class="fa fa-lock login-user"></i>
                                            </div>

                                             <div class="login-input-head">
                                                <p id="prdfsvddf" style="margin-bottom :12px; text-align:left;">ReEnter New Password</p>
                                            </div>
                                        </div>
                                        <div class="col-lg-8">
                                            <div class="login-input-area">
                                               <asp:TextBox ID="TextBox2"  runat="server" TextMode="Password" style="border-style:none; border-radius:5px; height:30px; width:100%;  padding-left:5px;" Placeholder="New Password"></asp:TextBox>
                                                <i class="fa fa-lock login-user"></i>
                                            </div>

             </asp:Panel>
              <asp:Panel ID="Panel1" runat="server">
                                    <div class="row">
                                        <div class="col-lg-4">
                                            <div class="login-input-head">
                                                <p id="prdf" style="margin-bottom :12px; text-align:left;">Password</p>
                                            </div>
                                        </div>
                                        <div class="col-lg-8">
                                            <div class="login-input-area">
                                               <asp:TextBox ID="txtPassword"  runat="server" TextMode="Password" style="border-style:none; border-radius:5px; height:30px; width:100%;  padding-left:5px;" Placeholder="Enter your password"></asp:TextBox>
                                                <i class="fa fa-lock login-user"></i>
                                            </div>
                                          
                                            <br />
                                            <div class="row">
                                                <div class="col-lg-12">
                                                    <div class="login-keep-me">
                                                        <label class="checkbox" id="lfg" style="text-align:left;">
                                                            <input type="checkbox" style="text-align:left;" name="remember" />Keep me logged in
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
                  </asp:Panel>
             <br />
             <asp:PlaceHolder ID="PlaceHolder1" runat="server"></asp:PlaceHolder>
             
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
        <br />
        <div id="fghs5" style="text-align:center; color:white;">Copyright © 2022. <a id="cop1" style="color:white; text-decoration:none" href="http://digitalschooltech.com" target="_blank">Digital School Educational Suite.</a> All Rights Reserved </div>
    </div>
           

        </div>
    </form>
</body>
</html>
