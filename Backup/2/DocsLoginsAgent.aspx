<%@ Page Language="C#" MasterPageFile="~/AgentMisc.master" AutoEventWireup="true"
    CodeFile="DocsLoginsAgent.aspx.cs" Inherits="DocsLoginsAgent" Title="E-Commerce Exchange - Partner Portal"
    Theme="AppTheme" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

    <script type="text/javascript" language="javascript">
            function Cover(bottom, top, ignoreSize) {
                var location = Sys.UI.DomElement.getLocation(bottom);
                top.style.position = 'absolute';
                top.style.top = location.y + 'px';
                top.style.left = location.x + 'px';
                if (!ignoreSize) {
                    top.style.height = bottom.offsetHeight + 'px';
                    top.style.width = bottom.offsetWidth + 'px';
                }
            }
    </script>

    <asp:ScriptManager ID="ScriptManagerDocs" runat="server">
    </asp:ScriptManager>
    <cc1:AnimationExtender ID="AnimationExtender1" runat="server" TargetControlID="lnkbtniPayment">
        <Animations>
                        <OnLoad><OpacityAction AnimationTarget="info" Opacity="0" /></OnLoad>
                        <OnClick>
                            <Sequence>                               
                                <ScriptAction Script="Cover($get('ctl00_ctl00_RootContent_MainContent_lnkbtniPayment'), $get('flyout'));" />
                                <StyleAction AnimationTarget="flyout" Attribute="display" Value="block"/>                            
                                <Parallel AnimationTarget="flyout" Duration=".1" Fps="25">
                                    <Move Horizontal="-100" Vertical="-250" />                                    
                                    <Color AnimationTarget="flyout" StartValue="#AAAAAA" EndValue="#f9f9d9" Property="style" PropertyKey="backgroundColor" />                                
                                </Parallel>                            
                                <ScriptAction Script="Cover($get('flyout'), $get('info'), true);" />
                                <StyleAction AnimationTarget="info" Attribute="display" Value="block"/>
                                <FadeIn AnimationTarget="info" Duration=".2"/>                            
                                <StyleAction AnimationTarget="flyout" Attribute="display" Value="none"/>
                                <StyleAction AnimationTarget="info" Attribute="height" value="auto" />
                                <Parallel Duration=".2">
                                    <Color AnimationTarget="info" StartValue="#383838" EndValue="#383838" Property="style" PropertyKey="color" />
                                    <Color AnimationTarget="info" StartValue="#febd0d" EndValue="#383838" Property="style" PropertyKey="borderColor" />
                                </Parallel>
                                <Parallel Duration=".2">
                                    <Color AnimationTarget="info" StartValue="#383838" EndValue="#383838" Property="style" PropertyKey="color" />
                                    <Color AnimationTarget="info" StartValue="#febd0d" EndValue="#383838" Property="style" PropertyKey="borderColor" />
                                    <FadeIn AnimationTarget="btnCloseParent" MaximumOpacity=".9" />                            
                                </Parallel>
                                
                            </Sequence>
                        </OnClick>
        </Animations>
    </cc1:AnimationExtender>
    <cc1:AnimationExtender ID="AnimationExtender2" runat="server" TargetControlID="btnClose">
        <Animations>
                        <OnClick>
                            <Sequence>
                                <StyleAction AnimationTarget="info" Attribute="overflow" Value="hidden"/>
                                <Parallel AnimationTarget="info" Duration=".1" Fps="15">
                                    <Scale ScaleFactor="0.05" Center="true" ScaleFont="true" FontUnit="px" />
                                    <FadeOut />
                                </Parallel>
                                <StyleAction AnimationTarget="info" Attribute="display" Value="none"/>
                                <StyleAction AnimationTarget="info" Attribute="width" Value="500px"/>
                                <StyleAction AnimationTarget="info" Attribute="height" Value=""/>
                                <StyleAction AnimationTarget="info" Attribute="fontSize" Value="12px"/>
                                <StyleAction AnimationTarget="btnCloseParent" Attribute="opacity" value="0" />
                                <StyleAction AnimationTarget="btnCloseParent" Attribute="filter" value="alpha(opacity=0)" />                        
                            </Sequence>
                        </OnClick>
                        <OnMouseOver>
                            <Color Duration=".2" StartValue="#FFFFFF" EndValue="#FF0000" Property="style" PropertyKey="color" />                            
                        </OnMouseOver>
                        <OnMouseOut>
                            <Color Duration=".2" EndValue="#FFFFFF" StartValue="#FF0000" Property="style" PropertyKey="color" />                            
                        </OnMouseOut>
        </Animations>
    </cc1:AnimationExtender>
    <br />
    <table border="0" cellspacing="0" class="SilverBorder" style="width: 850px">
        <tr>
            <td style="height: 30px; background-image: url('Images/topMain.gif')" colspan="3"
                align="center">
                <span class="MenuHeader"><strong>Applications, Forms, Documents and Logins</strong></span></td>
        </tr>
        <tr>
            <td colspan="3" align="center" style="height: 30px">
                <span class="LabelsBlue"><strong>Click on any link below to access the website or form.</strong></span>
            </td>
        </tr>
        <tr>
            <td valign="top" align="center">
                <table width="95%" border="0" cellspacing="2" class="SilverBorder">
                    <tr>
                        <td style="height: 20px; background-image: url('Images/topMain.gif')">
                            <strong><span class="MenuHeader">Other Logins</span></strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink CssClass="One" ID="lnkMail" runat="server" Target="_blank" NavigateUrl="http://webmail.serrahost.com/hwebmail/mail/login.php">Webmail</asp:HyperLink>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink CssClass="One" ID="lnkACTforWeb" runat="server" Target="_blank" NavigateUrl="https://www.firstaffiliates.com/apfw">ACT for Web</asp:HyperLink>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink CssClass="One" ID="lnkVerifone" runat="server" Target="_blank" NavigateUrl="http://www.verifonezone.com">VeriFone Zone</asp:HyperLink>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <asp:HyperLink CssClass="One" ID="lnkShowmyPC" runat="server" Target="_blank" NavigateUrl="http://ecenow.showmypc.com/">Remote Access (ShowMyPC)</asp:HyperLink>&nbsp;</td>
                    </tr>
                    </table>
                <br />
                <table border="0" cellspacing="2" style="width: 95%" class="SilverBorder">
                    <tr>
                        <td style="height: 20px; background-image: url('Images/topMain.gif')">
                            <strong><span class="MenuHeader">Miscellaneous</span></strong>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Misc Forms/eqsales.pdf" target="_blank">Equipment sales agreement</a>
                        </td>
                    </tr>
                    <tr style="font-size: 12pt">
                        <td>
                            <a class="One" href="http://www.fedex.com/us/" target="_blank">FedEx Tracking</a></td>
                    </tr>
                    </table>                
                <br />
                <table border="0" cellspacing="2" style="width: 95%;" class="SilverBorder">
                    <tr>
                        <td style="height: 20px; background-image: url('Images/topMain.gif')">
                            <strong><span class="MenuHeader">Advertising and Marketing</span></strong></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Advertising-Marketing/Email Marketing Letters-Email Flyers/E-mail Flyers/Word Format/General E-Commerce Exchange Flyer.doc"
                                target="_blank">E-Commerce Exchange Flyer</a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Advertising-Marketing/Business Cards-Letterhead-Envelopes/Business Card Template.pdf"
                                target="_blank">Business Card Template</a>
                        </td>
                </table>
                <br />
                <table border="0" cellspacing="2" style="width: 95%" class="SilverBorder">
                    <tr>
                        <td style="height: 20px; background-image: url('Images/topMain.gif')">
                            <b><span class="MenuHeader">Leasing</span></b></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Leasing%20Info/Lease%20Factors%20and%20Funding%20Procedures.xls"
                                target="_blank">Lease Factors and Funding Procedures</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Leasing%20Info/Lease%20Equipment%20Caps.doc" target="_blank">
                                Lease Payment Caps</a>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url('Images/homeback.gif');" class="SilverBorder">
                            <b><span style="font-family: Arial; font-size: small; color: Red">Northern Leasing Systems</span></b></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Northern%20Leasing/Northern%20Leasing%20Agreement - Standard.pdf"
                                target="_blank">Northern Lease Agreement (Standard)</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Northern%20Leasing/Northern%20Leasing%20Agreement - SD, KS, TN, PA & VT.pdf"
                                target="_blank">Northern Lease Agreement (5 States)</a></td>
                    </tr>
                    <!--<tr height=10></tr>
                    <tr>
                        <td style="background-image:url('Images/homeback.gif'); border-bottom-style: solid; border-right: silver 1px solid;
                            border-top: silver 1px solid; border-left: silver 1px solid; border-bottom: silver 1px solid;">
                            <b><span style="font-family: Arial; font-size: small; color: Red">A-1 Leasing</span></b></td>
                    </tr>
                    <tr>
                        <td >
                            <a  class="One" href="../Leasing/A-1 Leasing/A1 lease.pdf" target="_blank"><span style="font-family:Arial; font-size:small">A-1 Lease
                                Agreement</span></a></td>
                    </tr>
                    <tr>
                        <td >
                            <a  class="One" href="../Leasing/A-1 Leasing/A1 application.pdf" target="_blank"><span style="font-family:Arial; font-size:small">
                                A-1 Lease Application</span></a></td>
                    </tr>-->
                    <!--<tr>
                        <td style="background-image: url('Images/homeback.gif');" class="SilverBorder">
                            <b><span style="font-family: Arial; font-size: small; color: Red">Duvera Financial</span></b></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Leasing/Duvera/Leases/Duvera%20Lease.doc" target="_blank">Duvera
                                Lease Agreement</a></td>
                    </tr>-->
                    </table>              
                <br />
   
            </td>
            <td valign="top" align="center" width="34%">
                <table border="0" cellspacing="2" style="width: 95%;" class="SilverBorder">
                    <tr>
                        <td style="height: 20px; background-image: url('Images/topMain.gif'); width: 100%">
                            <b><span class="MenuHeader">Processors</span></b></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Comparison/Processor%20Buy%20Rate%20Comparison.xls" target="_blank">
                                Buy rate Comparison Chart</a></td>
                    </tr>
                                        <tr>
                        <td>
                            <a class="One" href="../Comparison/Processor Minimum Sell Rate Comparison Rev.3.21.2005.xls" target="_blank">
                                Sell rate Comparison Chart</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Comparison/Processor%20Buy%20Rate%20Comparison.xls" target="_blank">
                                Comparison Chart</a></td>
                    </tr>
                    <tr style="font-size: 12pt">
                        <td>
                            <a class="One" href="../PriceComparison/Rate Comparison Template - 3 Tier.xls" target="_blank">
                                Rate Comparison Template - 3 Tier</a></td>
                    </tr>
                    <tr style="font-size: 12pt">
                        <td>
                            <a class="One" href="../PriceComparison/Rate Comparison Template - Interchange Plus.xls"
                                target="_blank">Rate Comparison Template - Interchange Plus</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="http://usa.visa.com/download/merchants/visa-usa-interchange-reimbursement-fees-april2013.pdf"
                                target="_blank">Visa USA Interchange Rates</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="http://www.mastercard.com/us/merchant/pdf/MasterCard_Interchange_Rates_and_Criteria.pdf"
                                target="_blank">MasterCard Worldwide U.S. and Interregional<br />
                                Interchange Rates</a></td>
                    </tr>
                    <tr>
                        <td style="background-image: url('Images/homeback.gif');" class="SilverBorder">
                            <b><span class="LabelsRed">Sage Payment Solutions</span></b></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Sage/Forms/Sage Application.pdf" target="new">Merchant
                                Application</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Sage/Forms/Merchant Account Cancellation Form.doc" target="new">
                                Merchant Account Cancellation Form</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Sage/Programs/Product Grid Chase Paymentech.xls" target="_blank">
                                Product Grid Chase Paymentech</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Sage/Programs/Product Grid TSYS-Retail 111110.xls" target="_blank">
                                Product Grid TSYS</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Sage/Forms/ADD DEBIT & EBT.doc" target="_blank">Debit Addendum</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="https://uno.eftsecure.net/SalesCenter/" target="new">
                                Sales Center</a></td>
                    </tr>
                    
                    <tr>
                        <td>
                            <a class="One" href="https://sagena.webex.com/sagena/lsr.php?AT=pb&SP=MC&rID=9766547&rKey=73fec82f0bb304e9"
                                target="new">Bank Card 101 Training</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="https://sagena.webex.com/sagena/lsr.php?AT=pb&SP=MC&rID=9836102&rKey=175aed951aae4b18"
                                target="new">UNO Training</a></td>
                    </tr>
                    <tr>
                        <td style="background-image: url('Images/homeback.gif');" class="SilverBorder">
                            <b><span class="LabelsRed">Intuit</span></b></td>
                    </tr>                    
                    <!--tr>
                        <td>
                            <a class="One" href="http://www.firstaffiliates.com/quickbooks" target="_blank">QuickBooks
                                Merchant Application</a></td>
                    </tr-->
                    <tr>
                        <td>
                            <a class="One" href="../IMS Sales & Marketing Materials/Quickbooks Merchant Services Brochure/Quickbooks%20Merchant%20Service%20For%20PC.pdf"
                                target="_blank">QuickBooks Merchant Services Brochure</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="/IMS Sales & Marketing Materials/GoPayment Merchant Account Service.pdf"
                                target="_blank">Intuit GoPayment Brochure</a></td>
                    </tr>
                    <tr style="font-size: 12pt">
                        <td>
                            <a class="One" href="../IMS%20forms/IMS%20Product%20Matrix.xls" target="_blank">
                                Product Matrix</a></td>
                    </tr>                    

                    <tr>
                        <td>
                            <a class="One" href="/IMS Sales & Marketing Materials/POS Merchant Account Service.pdf"
                                target="_blank">POS Merchant Account Service</a></td>
                    </tr>
                                                            <tr>
                        <td>
                            <a class="One" href="http://dlm2.download.intuit.com/akdlm/SBD/QuickBooks/2013/R1/QuickBooksPOSV11Trial30.exe" target="_blank">POS 2013 30 day trial</a></td>
                    </tr>
                    <tr style="font-size: 12pt">
                        <td style="background-image: url('Images/homeback.gif');" class="SilverBorder">
                            <b><span class="LabelsRed">iPayment Inc.</span></b></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../IPayment%20Forms/Applications/IPayment%20Application.pdf"
                                target="new">iPayment Merchant Application</a></td>
                    </tr>
                    <tr>
                        <td>
                        <!--<a class="One" href="file://Server-R310/shared/Processors/iPayment/Forms/Debit Card Acceptance/PIN Debit Card Addendum.pdf"
                                target="new">Debit Addendum</a>-->
                            <a class="One" href="../Ipayment%20Forms/Debit%20Card%20Acceptance/PIN%20Debit%20Card%20Addendum.pdf"
                                target="new">Debit Addendum</a></td>
                    </tr>
                    <!--<tr>
                        <td>
                            <a class="One" href="../iPayment%20Forms/POS Retail Product Pricing & Flow.pdf" target="_blank">
                                POS Retail Product Pricing & Flow</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../iPayment%20Forms/POS Restaurant Product Pricing & Flow.pdf"
                                target="_blank">POS Restaurant Product Pricing & Flow</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../iPayment%20Forms/POS Rebate Promo.pdf" target="_blank">POS Rebate Promotion</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../iPayment%20Forms/Comstar for Quickbooks Setup Form.pdf" target="_blank">
                                Comstar for QuickBooks Setup</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../IPayment%20Forms/iPayment%20Reserve.doc" target="_blank">iPayment
                                Reserve Form</a></td>
                    </tr>-->
                    <tr>
                        <td>
                            <asp:LinkButton ID="lnkbtniPayment" runat="server" OnClientClick="return false;"
                                CssClass="One">Other iPayment Forms and Guidelines</asp:LinkButton>
                            <!--<a  class="One" href="iPayment.aspx" target="_blank"><span style="font-family:Arial; font-size:small">Other iPayment Forms
                                and Guidelines</span></a>-->
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Ipayment%20Forms/Ipayment%20Product%20Matrix.xls" target="_blank">
                                iPayment Product Matrix</a></td>
                    </tr>
                    </table>
                <br />
                <table border="0" cellspacing="2" style="width: 95%" class="SilverBorder">
                    <tr>
                        <td style="height: 20px; background-image: url('Images/topMain.gif')">
                            <b><span class="MenuHeader">Non Bankcard</span></b>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url('Images/homeback.gif');" class="SilverBorder">
                            <b><span class="LabelsRed">Discover</span></b></td>
                    </tr>
                    <!--tr>
                        <td>
                            <a class="One" href="../Discover Forms/Discover Rate Chart.doc" target="_blank">Discover
                                Rate Chart</a>
                        </td>
                    </tr-->
                    <tr>
                        <td>
                            <a class="One" href="https://ctlr.msu.edu/download/cashiers/DiscoverICApr13.pdf"
                                target="_blank">Discover Interchange Matrix</a>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url('Images/homeback.gif');" class="SilverBorder">
                            <b><span class="LabelsRed">Amex</span></b></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Amex%20Forms/Amex app.pdf" target="_blank">Amex Application</a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Amex%20Forms/Pricing/Appr franchise.pdf" target="_blank">Approved
                                Franchise List</a>
                        </td>
                    </tr>
                    <!--<tr>
                        <td>
                            <a class="One" href="../Amex Forms/Forms/B2B Reference Guide.pdf" target="_blank">B2B
                                Reference Guide</a>
                        </td>
                    </tr>-->
                    <tr>
                        <td>
                            <a class="One" href="../Amex%20Forms/Pricing/Amex%20US%20Merchant%20Pricing.pdf" target="_blank">
                                Amex U.S. Merchant Pricing</a></td>
                    </tr>
                    </table>
            </td>
            <td align="center" valign="top" width="33%">
                <table border="0" cellspacing="2" style="width: 95%;" class="SilverBorder">
                    <tr>
                        <td style="height: 20px; background-image: url('Images/topMain.gif')">
                            <strong><span class="MenuHeader">Payment Gateways</span></strong></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Gateway%20Forms/Comparison/Gateway%20comparison.xls" target="_blank">
                                Gateway Comparison Chart</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="http://na.sage.com/sage-payment-solutions/Sales-Resources/~/media/63CBE519BC444F7A8524A25C715608C1.pptx" target="_blank">ROAMpay Sage PowerPoint</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../ROAMpay/ROAMPay_iPay(Agents).ppt" target="_blank">ROAMpay iPayment PowerPoint</a></td>
                    </tr>
                    <!--<tr>
                        <td>
                            <asp:HyperLink CssClass="One" Font-Names="Arial" Font-Size="Small" ID="HyperLink1"
                                runat="server" Target="_blank" NavigateUrl="https://account.authorize.net/interfaces/reseller/frontend/login.aspx">Authorize.Net Reseller Login</asp:HyperLink>&nbsp;</td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="https://reseller.plugnpay.com/" target="_blank"><span style="font-family: Arial;
                                font-size: small">Plug'n Pay Reseller Login</span></a></td>
                    </tr>-->
                    <tr>
                        <td>
                            <a class="One" href="https://sagena.webex.com/sagena/ldr.php?AT=pb&SP=MC&rID=10996542&rKey=e4da45b6a260a531" target="_blank">Sage Basic Virtual Terminal Functionality</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="https://sagena.webex.com/sagena/lsr.php?AT=pb&SP=MC&rID=11040152&rKey=b6871e6eaaf80c6a"
                                target="new">SageExchange.com Webinar</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="http://www.youtube.com/watch?v=2JMgHB4myyM&feature=youtu.be" target="_blank">Sage Shopping Cart & Donate Now webinar</a></td>
                    </tr>
                    <tr>
                    <td>
                            <a class="One" href="http://www.youtube.com/watch?v=Qhir7lM2hoY&feature=youtu.be" target="_blank">Sage VT3 & Level III Processing webinar</a></td>
                    </tr>
                </table>                
                <br />
                <table border="0" cellspacing="2" style="width: 95%" class="SilverBorder">
                    <tr>
                        <td style="height: 20px; background-image: url('Images/topMain.gif')">
                            <b><span class="MenuHeader">Check Acceptance</span></b></td>
                    </tr>
                    
                    <tr>
                        <td style="background-image: url('Images/homeback.gif');" class="SilverBorder">
                            <b><span class="LabelsRed">CrossCheck</span></b></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="http://ms.cross-check.com/CrossCheckFiles/WebAuthPortal" target="_blank">
                                CrossCheck Archive</a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="/Crosscheck Forms/Check Guarantee-Plus Sales/standard agreement.pdf"
                                target="_blank">Retail Application</a>
                        </td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="/Crosscheck Forms/Check Conversion Plus with-without Imaging/With Imaging/Conversion Plus imaging.pdf"
                                target="_blank">Check Conversion Plus with Imaging</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="/Crosscheck Forms/Check Conversion Plus with-without Imaging/Without Imaging/Conversion plus nonimaging.pdf"
                                target="_blank">Non Imaging Check Conversion Plus</a>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url('Images/homeback.gif');" class="SilverBorder">
                            <b><span class="LabelsRed">Sage Payment Solutions EFT</span></b></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="http://www.checktraining.com/ece" target="_blank">Check Services Website</a></td>
                    </tr>
                    <tr>
                        <td style="background-image: url('Images/homeback.gif');" class="SilverBorder">
                            <b><span class="LabelsRed">TeleCheck</span> <span class="LabelsRedLarge">*</span></b></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Telecheck/Forms/TeleCheck Application.pdf" target="_blank">TeleCheck
                                Application</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Telecheck/Forms/TeleCheck Application Instructions.doc" target="_blank">
                                TeleCheck Application Instructions</a></td>
                    </tr>
                    </table>
                <br />
                <table border="0" cellspacing="2" style="width: 95%;" class="SilverBorder">
                    <tr>
                        <td style="background-image: url('Images/topMain.gif'); height: 20px">
                            <strong><span class="MenuHeader">Gift Cards/Loyalty</span></strong></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Gift Card/Gift-Loyalty Buy Rates.xls" target="_blank">Gift/Loyalty
                                Card Buy Rates</a></td>
                    </tr>

                    <tr>
                        <td>
                            <a class="One" href="http://www.giftcardtraining.com/ece" target="_blank">Gift Card Website</a></td>
                    </tr>
                    </table>  
                <br />
                <table border="0" cellspacing="2" style="width: 95%;" class="SilverBorder">
                    <tr>
                        <td style="height: 20px; background-image: url('Images/topMain.gif')">
                            <strong><span class="MenuHeader">Merchant Cash Advance</span></strong></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Merchant Cash Advance/Approval Criteria - BFS and AdvanceMe, Inc. Comparison.doc"
                                target="_blank">Cash Advance Approval Criteria</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Merchant Cash Advance/Business Financial Services/Application/Cash Advance Program Procedures_iPayment.pdf"
                                target="_blank">Cash Advance Program Procedures</a>
                        </td>
                    </tr>
                    <tr>
                        <td style="background-image: url('Images/homeback.gif');" class="SilverBorder">
                            <b><span class="LabelsRed">Business Financial Services</span><span class="LabelsRedLarge"> *</span></b></td>
                    </tr>
                    
                    <tr>
                        <td>
                            <a class="One" href="../Merchant Cash Advance/Business Financial Services/Application/General Authorization.pdf"
                                target="_blank">General Authorization Form</a></td>
                    </tr>
                    
                    <tr>
                        <td style="background-image: url('Images/homeback.gif');" class="SilverBorder">
                            <b><span class="LabelsRed">AdvanceMe, Inc.</span></b></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Merchant Cash Advance/AdvanceMe/CAN Pre_Qualification_Form.pdf"
                                target="_blank">Pre-Qualification Form</a></td>
                    </tr>
                    
                    <tr>
                        <td style="background-image: url('Images/homeback.gif');" class="SilverBorder">
                            <b><span class="LabelsRed">RapidAdvance</span></b></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="../Merchant Cash Advance/RapidAdvance/Application/Application (2013) fillable.pdf"
                                target="_blank">Application Form</a></td>
                    </tr>
                    <tr>
                        <td>
                            <a class="One" href="https://www.rapidadvance.com/partnerlogin.htm"
                                target="_blank">RapidAdvance Login</a></td>
                    </tr>
                    </table>
            </td>
        </tr>
        <tr>
            <td style="height: 15px">
                &nbsp;</td>
            <td style="height: 15px">
                <span class="LabelsRedLarge">* - Works only with iPayment</span> &nbsp;</td>
            <td style="height: 15px">
                &nbsp;</td>
        </tr>
    </table>
    <div id="flyout" style="z-index: 2; display: none; border: solid 1px #D0D0D0; background-color: #FFFFFF;
        overflow: hidden;">
        &nbsp;</div>
    <div id="info" style="z-index: 2; display: none; font-size: 12px; border: solid 1px #CCCCCC;
        background-color: #ffffff; width: 500px; padding: 5px; font-family: Arial; font-size: 10pt;">
        <div style="float: right; filter: alpha(opacity=0);" id="btnCloseParent">
            <asp:LinkButton ID="btnClose" CssClass="CloseButton" runat="server" OnClientClick="return false;"
                Text="" ToolTip="Close">X</asp:LinkButton>
        </div>
        <table width="500px" border="0" cellspacing="2" style="border-right: silver 1px solid;
            border-top: silver 1px solid; border-left: silver 1px solid; border-bottom: silver 1px solid;
            background-color: #f5f5f5">
            <tr>
                <td align="left" style="height: 25px; background-image: url('Images/topMain.gif');">
                    <strong><span class="MenuHeader">iPayment Merchant Services Guide Documents</span></strong></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="http://www.ipaymentinc.com/pdf/01%20IPI%20CBGuidelines%20.pdf"
                        target="_blank">Answers and Prevention Tips for Chargebacks and Retrievals</a></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="http://www.ipaymentinc.com/pdf/02%20IPI%20DiscountRates.pdf"
                        target="_blank">How to Obtain the Best Possible Discount Rate</a></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="http://www.ipaymentinc.com/pdf/03%20FAQ.pdf" target="_blank">About
                        Your Merchant Account</a></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="http://www.ipaymentinc.com/pdf/10%20IPI%20MAD%20Reports.pdf"
                        target="_blank">Information Contained in the MAD Report</a></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="http://www.ipaymentinc.com/pdf/Statement_Sample.pdf" target="_blank">
                        Sample of Your Merchant Statement</a></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="http://www.ipaymentinc.com/pdf/08%20IPI%20Statement%20revsd.pdf"
                        target="_blank">Understanding Your Merchant Statement</a></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="http://www.ipaymentinc.com/pdf/09%20IPI%20TranProcessing.pdf"
                        target="_blank">Proper Transaction Processing</a></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="http://www.ipaymentinc.com/pdf/11%20IPI%20ElecCommRM.pdf" target="_blank">
                        E-Commerce Risk Management Guidelines</a></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="http://www.ipaymentinc.com/pdf/Debit%20Card%20Service%20Guide%20v0502.pdf"
                        target="_blank">Debit Card Service Guide</a></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="http://www.ipaymentinc.com/pdf/12%20Glossary.pdf" target="_blank">
                        Glossary of Terms</a></td>
            </tr>
            <tr>
                <td align="left" style="height: 25px; background-image: url('Images/topMain.gif');">
                    <strong><span class="MenuHeader">Change / Request Forms</span></strong></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="../IPayment%20Forms/ACH%20Debits%20Credits%20Change%20Form.pdf"
                        target="_blank">CH Debits/Credits Change Request Form</a></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="../IPayment%20Forms/Card%20Addition%20Change%20Form.pdf" target="_blank">
                        Card Addition/Change Request Form</a></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="../IPayment%20Forms/Address%20Phone%20Fax%20Change%20Form.pdf"
                        target="_blank">Address/Phone/Fax Change Request Form</a></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="../IPayment%20Forms/Processing%20Limit%20Change%20Form.pdf"
                        target="_blank">Processing Limit Change Form</a></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="../IPayment%20Forms/Business%20Name%20Change%20Form.pdf" target="_blank">
                        Business Name Change Form</a></td>
            </tr>
            <tr>
                <td align="left">
                    <a class="One" href="../IPayment%20Forms/Close%20Merchant%20Account%20Form.pdf" target="_blank">
                        Close Merchant Account Request Form</a></td>
            </tr>
        </table>
    </div>
</asp:Content>
