<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="SiteRequest.aspx.cs" Inherits="BP.EQToolWeb.SiteRequest" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    
    <script src="../Scripts/jquery-1.7.min.js"></script>
    <script src="../Scripts/jquery.cookie-1.3.1.js"></script>
     <script src="../Scripts/SiteRequests.js"></script>
    <link rel="stylesheet" type="text/css" href="../CSS/Common.css" />
    <style>
        .disabledCtrl {
            color:#808080;
            background-color:#ffc;

        }

    </style>
</head>
<body>
    <form id="form1" runat="server">
        <asp:Label runat="server" ID="MessageControl"></asp:Label>
     <asp:HiddenField runat="server" ID="TypeInfoHidden" />
        <asp:HiddenField runat="server" ID="AreaInfoHidden" />
        <asp:HiddenField runat="server" ID="AreaDescriptionInfoHidden" />
        <asp:HiddenField runat="server" ID="RegionInfoHidden" />
        <asp:HiddenField runat="server" ID="RegionTitleHidden" /><asp:HiddenField runat="server" ID="RegionNameHidden" />
        <asp:HiddenField runat="server" ID="SiteLinkHidden" />
           <asp:HiddenField runat="server" ID="PrefixHiddenField" />
          <asp:HiddenField runat="server" ID="SiteTitleHidden" />
        <table width="100%" cellpadding="0" cellspacing="0" runat="server" class="mainTable" id="mainTable" style="background-color:#D8D8D8" >
            <tr class="left">
                <td class="text" height="26px" valign="center" style="color: white; font-size: 16px">&nbsp;<b>Site Creation Request</b>
                </td>
            </tr>
            <tr>
                <td>
                    <table width="100%" cellpadding="2" cellspacing="2" class="right">
                        <tr>
                            <td colspan="2">
                                <b>Region Details</b>
                            </td>

                        </tr>
                        <tr>
                            <td width="15%">Region Name<span style="color: red">*</span>
                            </td>
                            <td width="85%">
                                <input id="txtRegionName" size="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Site Title<span style="color: red">*</span>
                            </td>
                            <td>
                                <input id="txtSiteTitle" size="50" runat="server" />
                            </td>
                        </tr>
                        <tr>
                            <td>Region Prefix<span style="color: red">*</span>
                            </td>
                            <td>
                                <input id="txtRegionPrefix" size="50" runat="server" />
                            </td>
                        </tr>
                    </table>
                </td>

            </tr>
            <tr>

                <td>

                    <table width="100%" id="options-table" cellpadding="2" cellspacing="2" class="right">
                        <tr>
                            <td colspan="3">
                               <b> Area Details</b>
                            </td>

                        </tr>
                        <tr>
                            <td width="20%">Area<span style="color:red">*</span></td>
                            <td width="45%">Area Description</td>
                            <td width="35%">&nbsp;</td>
                        </tr>
                       
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_area" /></td>
                            <td>
                                <input type="text" size="50" name="input_area_desc" /></td>
                            <td>
                                <input type="button" style="width:80px" class="add" value="Add More" /></td>
                        </tr>
                    </table>
                </td>
            </tr>
            <tr>
                <td>
                     <table width="100%" id="TypeDetails" cellpadding="2" cellspacing="2" class="right" runat="server" >
                         
                        <tr>
                            <td colspan="3">
                               <b> Type Details</b>
                            </td>

                        </tr>
                           <!--Row1-->
                        <tr>
                            <td width="20%">Type<span style="color:red">*</span></td>
                            <td width="45%">Stages<span style="color:red">*</span></td>
                            <td width="35%">&nbsp;</td>
                        </tr>
                       
                        <tr >
                            <td>
                                <input type="text" size="50" name="input_type" value="Engineering Query - Area" pattern="[A-Za-z]{3}" class="disabledCtrl" title="Area" disabled="disabled"/></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_2" value="1" class="disabledCtrl" checked="checked" disabled="disabled" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_2"  value="2" class="disabledCtrl" checked="checked" disabled="disabled"  runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_2" value="3" class="disabledCtrl"   runat="server" disabled="disabled" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_2" value="4" class="disabledCtrl" checked="checked" disabled="disabled"  runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_2" value="5" class="disabledCtrl"  runat="server" disabled="disabled"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="delType" value="Delete" /></td>
                        </tr>
                          <!--End Row1-->
                         <!--Row2-->
                       
                       
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="Engineering Query - Electrical" /></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_3" value="1" checked="checked" disabled="disabled" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_3"  value="2" checked="checked" disabled="disabled"  runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_3" value="3"  checked="checked" runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_3" value="4" checked="checked" disabled="disabled"  runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_3" value="5" checked="checked" runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="delType" value="Delete" /></td>
                        </tr>
                         <!--End Row2-->
                         <!--Row3-->
                       
                       
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="Engineering Query - Instrumentation & Controls"/></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_4" value="1" checked="checked" disabled="disabled" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_4"  value="2" checked="checked" disabled="disabled"  runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_4" value="3" checked="checked"  runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_4" value="4" checked="checked" disabled="disabled"  runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_4" value="5" checked="checked" runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="delType" value="Delete"/></td>
                        </tr>
                         <!--End Row3-->
                         <!--Row4-->
                        
                       
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="Engineering Query - Mechanical or Materials"/></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_5" value="1" checked="checked" disabled="disabled" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_5"  value="2" checked="checked"  disabled="disabled" runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_5" value="3" checked="checked"  runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_5" value="4" checked="checked" disabled="disabled"  runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_5" value="5" checked="checked" runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="delType" value="Delete" /></td>
                        </tr>
                         <!--End Row4-->
                         <!--Row5-->
                        
                       
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="Engineering Query - Other" class="disabledCtrl" disabled="disabled"/></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_6" value="1" class="disabledCtrl" checked="checked" disabled="disabled" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_6"  value="2" class="disabledCtrl" checked="checked" disabled="disabled" runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_6" value="3" class="disabledCtrl" checked="checked"  disabled="disabled" runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_6" value="4"  class="disabledCtrl" checked="checked" disabled="disabled" runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_6" value="5" class="disabledCtrl" checked="checked" disabled="disabled"  runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="delType" value="Delete"/></td>
                        </tr>
                         <!--End Row5-->
                         <!--Row6-->
                        
                       
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="Engineering Query - Pressure Systems Integrity"  class="disabledCtrl" disabled="disabled"/></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_7" value="1" class="disabledCtrl" checked="checked" disabled="disabled" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_7"  value="2" class="disabledCtrl" checked="checked" disabled="disabled"  runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_7" value="3" class="disabledCtrl" checked="checked"   disabled="disabled" runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_7" value="4" class="disabledCtrl" checked="checked" disabled="disabled"  runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_7" value="5" class="disabledCtrl"  checked="checked"  disabled="disabled" runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="delType" value="Delete"/></td>
                        </tr>
                         <!--End Row6-->
                         <!--Row7-->
                        
                       
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="Engineering Query - Process" /></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_8" value="1" checked="checked" disabled="disabled" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_8"  value="2" checked="checked" disabled="disabled"  runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_8" value="3" checked="checked"  runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_8" value="4" checked="checked" disabled="disabled"  runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_8" value="5" checked="checked" runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="delType" value="Delete"/></td>
                        </tr>
                         <!--End Row7-->
                         <!--Row8-->
                      
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="Engineering Query - Reliability & Maintenance" /></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_9" value="1" checked="checked" disabled="disabled" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_9"  value="2" checked="checked" disabled="disabled"   runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_9" value="3" checked="checked"  runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_9" value="4" checked="checked" disabled="disabled"  runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_9" value="5" checked="checked"  runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="delType" value="Delete"/></td>
                        </tr>
                         <!--End Row8-->
                        
                          <!--Row9-->
                     
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="Engineering Query - Rotating Equipement" /></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_10" value="1" checked="checked" disabled="disabled" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_10"  value="2"  checked="checked" disabled="disabled" runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_10" value="3" checked="checked"  runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_10" value="4" checked="checked" disabled="disabled"  runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_10" value="5" checked="checked" runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="delType" value="Delete"/></td>
                        </tr>
                          <!--End Row9-->
                         <!--Row10-->
                       
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="Engineering Query - Structures or Floating Systems" /></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_11" value="1" checked="checked" disabled="disabled" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_11"  value="2" checked="checked" disabled="disabled"  runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_11" value="3" checked="checked"  runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_11" value="4" checked="checked" disabled="disabled"  runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_11" value="5" checked="checked" runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="delType" value="Delete"/></td>
                        </tr>
                          <!--End Row10-->
                        
                         <!--Row11-->
                                                                        
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="Area Activity"  class="disabledCtrl"disabled="disabled"/></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_12" checked="checked" value="1"  class="disabledCtrl" disabled="disabled" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_12" checked="checked"  value="2"  class="disabledCtrl" disabled="disabled"  runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_12" value="3"     class="disabledCtrl" disabled="disabled" runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_12" value="4" checked="checked"  class="disabledCtrl" disabled="disabled"  runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_12" value="5"  class="disabledCtrl" disabled="disabled" runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="delType" value="Delete"/></td>
                        </tr>
                          <!--End Row11-->

                          <!--Row12-->
                     
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="P&M Project"  class="disabledCtrl" disabled="disabled"/></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_13" value="1"  class="disabledCtrl" disabled="disabled"   checked="checked" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_13"  value="2"   class="disabledCtrl" disabled="disabled"  checked="checked"  runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_13" value="3"  class="disabledCtrl" disabled="disabled" checked="checked" runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_13" value="4"  class="disabledCtrl" disabled="disabled"  checked="checked"   runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_13" value="5"  class="disabledCtrl" disabled="disabled"  runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="delType" value="Delete"/></td>
                        </tr>
                          <!--End Row12-->

                         <!--Row13-->
                     
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="Subsea Project"  class="disabledCtrl" disabled="disabled"  /></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_14" value="1"  class="disabledCtrl" disabled="disabled"   checked="checked" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_14"  value="2"   class="disabledCtrl" disabled="disabled"  checked="checked"  runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_14" value="3"  class="disabledCtrl"  disabled="disabled" checked="checked" runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_14" value="4" class="disabledCtrl"  disabled="disabled"  checked="checked"   runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_14" value="5"  class="disabledCtrl"  disabled="disabled" runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="delType" value="Delete"/></td>
                        </tr>
                          <!--End Row13-->

                         <!--Row14-->
                     
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="Worley Parsons Drafting Service Request"  class="disabledCtrl" disabled="disabled" /></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_15" value="1"  class="disabledCtrl" disabled="disabled"   checked="checked" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_15"  value="2"   class="disabledCtrl" disabled="disabled"  checked="checked"  runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_15" value="3"   class="disabledCtrl" disabled="disabled"  runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_15" value="4"  class="disabledCtrl" disabled="disabled"  checked="checked"   runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_15" value="5"   class="disabledCtrl" disabled="disabled" checked="checked" runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                 <input type="button" style="width:80px" class="delType" value="Delete"/></td>
                        </tr>
                          <!--End Row14-->

                         <!--Row15-->
                     
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="Quality or GOC" class="disabledCtrl" disabled="disabled" /></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one" id="input_step_one_16" value="1"  class="disabledCtrl" disabled="disabled"   checked="checked" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two" id="input_step_two_16"  value="2"   class="disabledCtrl" disabled="disabled"  checked="checked"  runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three" id="input_step_three_16" value="3"   class="disabledCtrl" disabled="disabled" runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_16" value="4" class="disabledCtrl"  disabled="disabled"  checked="checked"   runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five" id="input_step_five_16" value="5"   class="disabledCtrl" disabled="disabled" checked="checked" runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="delType" value="Delete"/></td>
                        </tr>
                          <!--End Row15-->

                          <!--RowLast-->
                     
                        <tr>
                            <td>
                                <input type="text" size="50" name="input_type" value="" /></td>
                            <td>
                                
                                <input type="checkbox" name="input_step_one"   id="input_step_one_17" value="1" disabled="disabled"   checked="checked" runat="server" />Initiate &nbsp;
                                <input type="checkbox" name="input_step_two"  id="input_step_two_17"  value="2"  disabled="disabled"  checked="checked"  runat="server" />Approval &nbsp;
                                <input type="checkbox" name="input_step_three"  id="input_step_three_17" value="3"  runat="server" />Review &nbsp;
                                <input type="checkbox" name="input_step_four"  id="input_step_four_17" value="4" disabled="disabled"  checked="checked"   runat="server"/>Response &nbsp;
                                <input type="checkbox" name="input_step_five"  id="input_step_five_17" value="5"  runat="server"  />Close Out &nbsp;
                            </td>
                            <td>
                                <input type="button" style="width:80px" class="addType" value="Add More" /></td>
                        </tr>
                          <!--End RowLast-->
                       
                    </table>
                </td>
            </tr>
            
            <tr>
                <td align="center" height="28px">
                    <%--<asp:Button runat="server" ID="Submit" class="button" OnClientClick="javascript:return startExec()" Style="width: 80px; vertical-align: top" Text="Submit" OnClick="Submit_Click" />--%>
                    <asp:Button runat="server" ID="Finish" class="button" OnClientClick="javascript:return ShowConfirmScreen()"   Style="width: 80px; vertical-align: top" Text="Finish"   />
                </td>
            </tr>
            <tr>
                <td align="center" height="28px">
                    <asp:Label ID="MessageError" runat="server" CssClass="text" Visible="false" Style="color: red" Text="" />
                </td>
            </tr>
        </table>
        <table width="100%" cellpadding="0" cellspacing="0" class="mainTable" id="confirmTable" runat="server" style="background-color:#D8D8D8;display:none" >
            <tr>
                <td width="100%">
                    <table width="100%"  id="table_confirm">
                       
                        <tr>
                            <td colspan="2">
                                <b>Please confirm details:</b>
                            </td>
                        </tr>
                         <tr>
                            <td width="25%">
                                Region :
                            </td>
                             <td width="75%">
                                <asp:Label runat="server" ID="input_region"></asp:Label>
                             </td>
                        </tr>
                         <tr>
                            <td>
                                Region Link :
                            </td>
                             <td>
                                  <asp:Label runat="server" ID="input_site_link"></asp:Label>
                             </td>
                        </tr>
                         <tr>
                            <td>
                                Prefix :
                            </td>
                             <td>
                                  <asp:Label runat="server" ID="input_prefix"></asp:Label>
                             </td>
                        </tr>
                        <tr>
                            <td style="vertical-align:top">
                                Areas:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="input_areas"></asp:Label>
                            </td>
                        </tr>
                         <tr>
                            <td style="vertical-align:top">
                                Types:
                            </td>
                            <td>
                                <asp:Label runat="server" ID="input_types"></asp:Label>
                            </td>
                        </tr>
                         <tr>                           
                            <td colspan="2">
                                <asp:CheckBox runat="server" ID="checkTerms" />Given details are verified.  
                            </td>
                        </tr>
                        
                         <tr>
                            <td colspan="2">
                                <asp:Button runat="server" ID="Submit" class="button" OnClientClick="javascript:return startExec()" Style="width: 80px; vertical-align: top" Text="Submit" OnClick="Submit_Click" />&nbsp;&nbsp;
                                <input type="button" id="Back" class="button"  style="width: 120px" value="Modify Details"  onclick="return BackScreen()"/>
                            </td>
                        </tr>                        
                    </table>
                </td>

            </tr>

        </table>
    </form>
</body>
</html>




