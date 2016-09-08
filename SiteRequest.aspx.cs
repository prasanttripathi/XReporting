using Microsoft.SharePoint;
using Microsoft.SharePoint.Client;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace BP.EQToolWeb
{
    public partial class SiteRequest : System.Web.UI.Page
    {

        public string ErrorInLoading = "Error occured";
        public string ErrorInSubmitting = "Error occured in submitting";
        public string ErrorInClearingValues = "Error occured in clearing values";
        public string ErrorInCreatingEntries = "Error occured in creating entries";
       
        protected void Page_Load(object sender, EventArgs e)
        {
            //by default keep error message label hidden
            MessageError.Visible = false;
            Uri hostWeb = new Uri(Request.QueryString["SPHostUrl"]);
            SiteLinkHidden.Value = hostWeb.AbsoluteUri;
            try
            {
                if (!Page.IsPostBack)
                { 
                    MessageControl.Text = "";                   
                }
                LoadPrefix();
            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "Page_Load");
                if (ex == null)
                    throw;
                MessageError.Text = ErrorInLoading;
                MessageError.Visible = true;
            }

        }

        public void LoadPrefix()
        {
            try
            {
                Uri hostWeb = new Uri(Request.QueryString["SPHostUrl"]);
                using (var clientContext = TokenHelper.GetS2SClientContextWithClaimsIdentity(hostWeb, HttpContext.Current.User, TokenHelper.IdentityClaimType.SMTP, TokenHelper.ClaimProviderType.SAML, true))
                {
                    Web web = clientContext.Web;
                    clientContext.Load(web);

                    List oList = web.Lists.GetByTitle("SiteRequests");
                    clientContext.Load(oList);
                    clientContext.ExecuteQuery();

                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = "<View><Query><Where><Geq><FieldRef Name='ID'/>" +
                        "<Value Type='Number'>1</Value></Geq></Where></Query><RowLimit>100</RowLimit></View>";
                    Microsoft.SharePoint.Client.ListItemCollection collListItem = oList.GetItems(camlQuery);

                    clientContext.Load(collListItem);

                    clientContext.ExecuteQuery();

                    SiteTitleHidden.Value = "";
                    PrefixHiddenField.Value = "";
                    foreach (Microsoft.SharePoint.Client.ListItem oListItem in collListItem)
                    {
                        SiteTitleHidden.Value += Convert.ToString(oListItem["Path"], CultureInfo.CurrentCulture) + ";";
                        PrefixHiddenField.Value += Convert.ToString(oListItem["Prefix"], CultureInfo.CurrentCulture) + ";";
                        //Console.WriteLine("ID: {0} \nTitle: {1} \nBody: {2}", oListItem.Id, oListItem["Title"], oListItem["Body"]);
                    }
                    //MessageError.Text = PrefixHiddenField.Value;
                    //MessageError.Visible = true;

                }
            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "LoadPrefix");
                if (ex == null)
                    throw;
                MessageError.Text = ErrorInLoading;
                MessageError.Visible = true;
            }
        }

        protected void Submit_Click(object sender, EventArgs e)
        {
            try
            {
                 CreateListEntries();
                 ClearValues();
                
            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "Submit_Click");
                if (ex == null)
                    throw;
                MessageError.Text = ErrorInSubmitting;
                MessageError.Visible = true;
            }


        }


        public void ClearValues()
        {
            try
            {
                
                txtRegionName.Value = "";
                txtSiteTitle.Value = "";
                txtRegionPrefix.Value = "";
                input_step_one_2.Checked = true;
                input_step_two_2.Checked = true;
                input_step_three_2.Checked = false;
                input_step_four_2.Checked = true;
                input_step_five_2.Checked = false;

                input_step_one_3.Checked = true;
                input_step_two_3.Checked = true;
                input_step_three_3.Checked = true;
                input_step_four_3.Checked = true;
                input_step_five_3.Checked = true;

                input_step_one_4.Checked = true;
                input_step_two_4.Checked = true;
                input_step_three_4.Checked = true;
                input_step_four_4.Checked = true;
                input_step_five_4.Checked = true;

                input_step_one_5.Checked = true;
                input_step_two_5.Checked = true;
                input_step_three_5.Checked = true;
                input_step_four_5.Checked = true;
                input_step_five_5.Checked = true;

                input_step_one_6.Checked = true;
                input_step_two_6.Checked = true;
                input_step_three_6.Checked = true;
                input_step_four_6.Checked = true;
                input_step_five_6.Checked = true;

                input_step_one_7.Checked = true;
                input_step_two_7.Checked = true;
                input_step_three_7.Checked = true;
                input_step_four_7.Checked = true;
                input_step_five_7.Checked = true;

                input_step_one_8.Checked = true;
                input_step_two_8.Checked = true;
                input_step_three_8.Checked = true;
                input_step_four_8.Checked = true;
                input_step_five_8.Checked = true;

                input_step_one_9.Checked = true;
                input_step_two_9.Checked = true;
                input_step_three_9.Checked = true;
                input_step_four_9.Checked = true;
                input_step_five_9.Checked = false;

                input_step_one_10.Checked = true;
                input_step_two_10.Checked = true;
                input_step_three_10.Checked = true;
                input_step_four_10.Checked = true;
                input_step_five_10.Checked = true;

                input_step_one_11.Checked = true;
                input_step_two_11.Checked = true;
                input_step_three_11.Checked = true;
                input_step_four_11.Checked = true;
                input_step_five_11.Checked = true;

                input_step_one_12.Checked = true;
                input_step_two_12.Checked = true;
                input_step_three_12.Checked = true;
                input_step_four_12.Checked = true;
                input_step_five_12.Checked = true;

                LoadPrefix();

                MessageControl.Text = "<script>alert('Request submitted successfully.');</script>";

                //Response.Redirect("SiteRequest.aspx?"+ Request.QueryString.ToString());
                //Uri hostWeb = new Uri(Request.QueryString["SPHostUrl"]);
                //Server.Transfer(hostWeb.AbsoluteUri + "/Pages/ManageRegions.aspx");
               // Server.Transfer("/sites/rfa/Pages/ManageRegions.aspx");
            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "ClearValues");
                if (ex == null)
                    throw;
                MessageError.Text = ErrorInClearingValues;
                MessageError.Visible = true;

            }
        }

        public void CreateListEntries()
        {
            try
            {               
                    Uri hostWeb = new Uri(Request.QueryString["SPHostUrl"]);
                    using (var clientContext = TokenHelper.GetS2SClientContextWithClaimsIdentity(hostWeb, HttpContext.Current.User, TokenHelper.IdentityClaimType.SMTP, TokenHelper.ClaimProviderType.SAML, true))
                    {
                        Web web = clientContext.Web;
                        clientContext.Load(web);

                        List oList = web.Lists.GetByTitle("SiteRequests");
                        clientContext.Load(oList);
                        clientContext.ExecuteQuery();

                        ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                        Microsoft.SharePoint.Client.ListItem oListItem = oList.AddItem(itemCreateInfo);

                        string type_sequence = TypeInfoHidden.Value;
                        //string[] areas = Request.Form.GetValues("input_area");
                        //string[] areas_desc = Request.Form.GetValues("input_area_desc");                       
                        string areas = AreaInfoHidden.Value;
                        string areas_desc = AreaDescriptionInfoHidden.Value;
                        string area_sequence = "";
                        for (int i = 0; i < areas.Split(';').Length; i++)
                        {
                            if (i == areas.Length - 1)
                                area_sequence += areas.Split(';')[i] + ";" + areas_desc.Split(';')[i];
                            else
                                area_sequence += areas.Split(';')[i] + ";" + areas_desc.Split(';')[i] + "#";
                        }

                        area_sequence = area_sequence.Substring(0, area_sequence.Length - 1);
                        StringBuilder sb = new StringBuilder();
                        sb.Append(Convert.ToString(txtRegionName.Value, CultureInfo.CurrentCulture));
                        sb.Append(";");
                        sb.Append(Convert.ToString(txtSiteTitle.Value, CultureInfo.CurrentCulture));
                        sb.Append(";");
                        sb.Append(Convert.ToString(txtRegionPrefix.Value, CultureInfo.CurrentCulture));



                        oListItem["RegionData"] = RegionInfoHidden.Value;
                        oListItem["AreaData"] = area_sequence;
                        oListItem["TypeData"] = type_sequence;                       
                        oListItem["Path"] = RegionTitleHidden.Value;// Convert.ToString(txtSiteTitle.Value, CultureInfo.CurrentCulture);
                        if (!String.IsNullOrEmpty(RegionInfoHidden.Value))
                            oListItem["Prefix"] = RegionInfoHidden.Value.Split(';')[2];// Convert.ToString(txtSiteTitle.Value, CultureInfo.CurrentCulture);
                        oListItem["Title"] = RegionNameHidden.Value;// Convert.ToString(txtRegionName.Value, CultureInfo.CurrentCulture);
                        oListItem["Status"] = "Not Processed";

                        oListItem.Update();
                        clientContext.ExecuteQuery();

                    }                
            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "CreateListEntries");
                if (ex == null)
                    throw;
                MessageError.Text = ErrorInCreatingEntries;
                MessageError.Visible = true;
            }
        }

        protected void Finish_Click(object sender, EventArgs e)
        {
            try
            {
                
                //confirmTable.Style.Add("display", "block");
                //confirmTable.Visible = true;

                //// Repeat the previous line for "select" and "textarea"
                //for (int i = 0; i < mainTable.Rows.Count; i++)
                //{
                //    HtmlTableRow row = mainTable.Rows[i];
                //    row.Attributes.Add("disabled", "true");
                //}

                //input_region.Text = txtRegionName.Value;
                //input_site_link.Text = txtSiteTitle.Value;
                //input_prefix.Text = txtRegionPrefix.Value;
                //Areas = Request.Form.GetValues("input_area");
                //AreasDescription = Request.Form.GetValues("input_area_desc");
                //for (int i = 0; i < Areas.Length; i++)
                //{
                //    if (i == Areas.Length - 1)
                //        input_areas.Text += Areas[i];
                //    else
                //        input_areas.Text += Areas[i] + "<br>";
                //}
                //input_types.Text = TypeInfoHidden.Value;
                //string type_data="";
                //for (int i = 0; i < input_types.Text.Split('#').Length; i++)
                //{
                //    string[] type_values = input_types.Text.Split('#')[i].Split(';');
                //    type_data = type_data + type_values[0] + "<br>";
                //}
                //input_types.Text = type_data;

            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "Finish_Click");
                if (ex == null)
                    throw;
                MessageError.Text = ErrorInLoading;
                MessageError.Visible = true;
            }
        }


        protected void Back_Click(object sender, EventArgs e)
        {
            try
            {




            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "Finish_Click");
                if (ex == null)
                    throw;
                MessageError.Text = ErrorInLoading;
                MessageError.Visible = true;
            }
        }


    }
}