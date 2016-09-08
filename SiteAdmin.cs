using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using BP.Claims.Token;
using System.Net;
using Microsoft.SharePoint.Client;
using System.Data;
using System.Globalization;
using System.Resources;
using System.IO;
using Microsoft.SharePoint;
using Microsoft.SharePoint.Client.Utilities;


namespace BP.EQSiteCreation
{
    class SiteAdmin
    {
        public static string SiteUrl = @"https://bp1amsapt225.cloudapp.net/sites/rfa/";
        public static string area_data = "", type_data="", region_data="", group_data="", group_area_data=""  ;
        public static string exc_site_name = "";
       
      

        public static void Main()
        {
            try
            {                
               SiteUrl = GetConfigValue("SPSiteUrl");
               //SendEmail();
               GetSiteRequests();
               ArchiveData();
               
            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "Main Method");               
                if (ex == null)
                    throw;
                SendEmail();
            }
            
           


        }


        //public static void CallMe()
        //{
        //    try
        //    {
        //        int i = Convert.ToInt16("rt");


        //    }
        //    catch (Exception ex)
        //    {
        //        ErrorLogs.LogException(ex, "CallMe");
        //        if (ex == null)
        //            throw;
               
        //    }
        //}

        
        #region Site Creation
        public static void GetSiteRequests()
        {
            try
            {
                using (var clientContext = new ClientContext(SiteUrl)) // open sharepoint site              
                {
                    clientContext.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
                    List oList = clientContext.Web.Lists.GetByTitle("SiteRequests");

                    string queryInProc = "<View><Query><Where><Eq><FieldRef Name='Status' /><Value Type='Choice'>In Processing</Value></Eq></Where></Query></View>";
                    CamlQuery camlQueryInProc = new CamlQuery();
                    camlQueryInProc.ViewXml = queryInProc;
                    ListItemCollection collListItemInProc = oList.GetItems(camlQueryInProc);

                    clientContext.Load(oList);
                    clientContext.Load(collListItemInProc);
                    clientContext.ExecuteQuery();

                    if (collListItemInProc.Count <= 0)
                    {
                        string query = "<View><Query><Where><Eq><FieldRef Name='Status' /><Value Type='Choice'>Not Processed</Value></Eq></Where></Query></View>";
                        CamlQuery camlQuery = new CamlQuery();
                        camlQuery.ViewXml = query;
                        ListItemCollection collListItem = oList.GetItems(camlQuery);

                        clientContext.Load(oList);
                        clientContext.Load(collListItem);
                        clientContext.ExecuteQuery();

                        string siteName, path = "";
                        //foreach (ListItem oListItem in collListItem)
                        //{
                        if (collListItem != null)
                        {
                            if (collListItem.Count > 0)
                            {
                                ListItem oListItem = collListItem[0];
                                oListItem["Status"] = "In Processing";
                                oListItem.Update();
                                clientContext.ExecuteQuery();
                                siteName = Convert.ToString(oListItem["Title"], CultureInfo.CurrentCulture);
                                exc_site_name = siteName;
                                // siteDesc = Convert.ToString(oListItem["Description"], CultureInfo.CurrentCulture);
                                path = Convert.ToString(oListItem["Path"], CultureInfo.CurrentCulture);

                                area_data = Convert.ToString(oListItem["AreaData"], CultureInfo.CurrentCulture);
                                type_data = Convert.ToString(oListItem["TypeData"], CultureInfo.CurrentCulture);
                                region_data = Convert.ToString(oListItem["RegionData"], CultureInfo.CurrentCulture);

                                bool flagCompletion = false;
                                CreateSite(siteName, "", path);
                                foreach (string area_row in area_data.Split('#'))
                                {
                                    string[] area_values = area_row.Split(';');
                                    group_area_data = group_area_data + area_values[0] + ";";
                                }

                                for (int i = 0; i < type_data.Split('#').Length; i++)
                                {
                                    string[] type_values = type_data.Split('#')[i].Split(';');
                                    group_data = group_data + type_values[0] + ";";
                                }
                                string[] site_Coll_Groups = { "RFA System Admins", "RFA System Coordinators", "RFA Visitors" };
                                foreach (var item_name in site_Coll_Groups)
                                {

                                    using (var clientContext1 = new ClientContext(SiteUrl + path)) // open sharepoint site              
                                    {
                                        clientContext1.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
                                        Web web = clientContext1.Web;
                                        clientContext1.Load(web);
                                        clientContext1.ExecuteQuery();

                                        GroupCollection grp_Coll = web.SiteGroups;
                                        clientContext1.Load(grp_Coll);
                                        Group item = grp_Coll.GetByName(item_name);
                                        clientContext1.Load(item);
                                        clientContext1.ExecuteQuery();

                                        if (item != null)
                                        {

                                            if (item_name == "RFA Visitors")
                                            {
                                                //Get a role
                                                RoleDefinition rd_Contribute = clientContext1.Web.RoleDefinitions.GetByName("Contribute");
                                                //create the role definition binding collection
                                                RoleDefinitionBindingCollection rdb_Contribute = new RoleDefinitionBindingCollection(clientContext1);

                                                //add the role definition to the collection
                                                rdb_Contribute.Add(rd_Contribute);
                                                web.RoleAssignments.Add(item, rdb_Contribute);
                                                clientContext1.ExecuteQuery();
                                            }
                                            else
                                            {
                                                //Get a role
                                                RoleDefinition rd_Contribute = clientContext1.Web.RoleDefinitions.GetByName("Full Control");
                                                //create the role definition binding collection
                                                RoleDefinitionBindingCollection rdb_Contribute = new RoleDefinitionBindingCollection(clientContext1);

                                                //add the role definition to the collection
                                                rdb_Contribute.Add(rd_Contribute);
                                                web.RoleAssignments.Add(item, rdb_Contribute);
                                                clientContext1.ExecuteQuery();
                                            }
                                        }

                                    }

                                }
                                flagCompletion = CreateGroups(path);
                                if (flagCompletion)
                                {
                                    flagCompletion = CreateListEntries(path);
                                    if (flagCompletion)
                                    {
                                        oListItem["Status"] = "Processed";
                                        oListItem.Update();
                                        clientContext.ExecuteQuery();
                                    }
                                }
                            }
                        }


                        //}

                    }
                }


            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "GetSiteRequests");
                SendEmail();
                if (ex == null)
                    throw;
            }
        }

        public static void CreateSite(string siteName, string siteDesc, string path)
        {
            try
            {

                string templateName = "";
                templateName = GetConfigValue("TemplateName");

                using (var clientContext = new ClientContext(SiteUrl)) // open sharepoint site              
                {
                    clientContext.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
                    Web web = clientContext.Web;
                    clientContext.Load(web);
                    var webTemplates = web.GetAvailableWebTemplates(1033, false);
                    clientContext.Load(webTemplates);
                    clientContext.ExecuteQuery();
                    clientContext.RequestTimeout = Convert.ToInt32(GetConfigValue("TimeOut"), CultureInfo.InvariantCulture);//1000000;
                    foreach (WebTemplate template in webTemplates)
                    {
                        if (template.Title.Equals(templateName, StringComparison.CurrentCultureIgnoreCase))
                        {
                            templateName = template.Name;
                            var webCreationInformation = new WebCreationInformation();
                            webCreationInformation.Title = siteName;
                            webCreationInformation.Description = siteDesc;
                            webCreationInformation.Language = 1033;
                            webCreationInformation.Url = path;
                            webCreationInformation.UseSamePermissionsAsParentSite = false;
                            webCreationInformation.WebTemplate = templateName;
                            // web.BreakRoleInheritance(false, false);
                            web.Webs.Add(webCreationInformation);
                            try
                            {
                                clientContext.ExecuteQuery();
                            }
                            catch (Exception ex)
                            {
                                if (ex == null)
                                    throw;
                            }


                            //Create entry in Region list
                            List oListArea = web.Lists.GetByTitle("Region");
                            clientContext.Load(oListArea);
                            clientContext.ExecuteQuery();

                            string[] region_values = region_data.Split(';');
                            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                            Microsoft.SharePoint.Client.ListItem oListItem = oListArea.AddItem(itemCreateInfo);
                            oListItem["Title"] = Convert.ToString(region_values[0], CultureInfo.CurrentCulture);//name
                            oListItem["Path"] = Convert.ToString(region_values[1], CultureInfo.CurrentCulture);//title
                            oListItem["Prefix"] = Convert.ToString(region_values[2], CultureInfo.CurrentCulture);
                            oListItem.Update();

                            clientContext.ExecuteQuery();

                        }


                    }

                }

            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "CreateSite");
                SendEmail();
                if (ex == null)
                    throw;

            }
        }

        public static bool CreateGroups(string siteName)
        {
            try
            {
                bool isGroupCreated = false;
                #region Owner Group Creation

                using (var clientContext = new ClientContext(SiteUrl)) // open sharepoint site              
                {
                    clientContext.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
                    Web web = clientContext.Web;
                    clientContext.Load(web);
                    clientContext.ExecuteQuery();

                    GroupCollection grp_Coll = web.SiteGroups;
                    clientContext.Load(grp_Coll);
                    clientContext.ExecuteQuery();
                    //bool existingFlag = false;
                    //foreach (Group existing_group in grp_Coll)
                    //{
                    //    if (existing_group.Title.Equals(region_data.Split(';')[2] + " Owners Group"))
                    //    {                         

                    //        //Get a role
                    //        RoleDefinition rd_second = clientContext.Web.RoleDefinitions.GetByName("Read");
                    //        //create the role definition binding collection
                    //        RoleDefinitionBindingCollection rdb_second = new RoleDefinitionBindingCollection(clientContext);

                    //        //add the role definition to the collection
                    //        rdb_second.Add(rd_second);
                    //        web.RoleAssignments.Add(existing_group, rdb_second);
                    //        ErrorLogs.WriteException("Creating Owner Group on Root - " + existing_group.Title);
                    //        clientContext.ExecuteQuery();
                    //        ErrorLogs.WriteException("Created Owner Group on Root - " + existing_group.Title);
                    //        existingFlag = true;
                    //    }
                    //}



                    //if (!existingFlag)
                    //{
                        //Group Creation
                        GroupCreationInformation grp_second = new GroupCreationInformation();

                        grp_second.Title = Convert.ToString(region_data.Split(';')[2] + " Owners Group", CultureInfo.CurrentCulture);
                        grp_second.Description = "This is a custom Owner group created";
                        //add it to the list of site groups
                        Group newgrp_Second = clientContext.Web.SiteGroups.Add(grp_second);

                        //Get a role
                        RoleDefinition rd_second = clientContext.Web.RoleDefinitions.GetByName("Read");
                        //create the role definition binding collection
                        RoleDefinitionBindingCollection rdb_second = new RoleDefinitionBindingCollection(clientContext);

                        //add the role definition to the collection
                        rdb_second.Add(rd_second);

                        web.RoleAssignments.Add(newgrp_Second, rdb_second);
                        ErrorLogs.WriteException("Creating Owner Group on Root - " + grp_second.Title);
                        clientContext.ExecuteQuery();
                        ErrorLogs.WriteException("Created Owner Group on Root - " + grp_second.Title);
                   // }

                   

                }

                using (var clientContext = new ClientContext(SiteUrl + siteName)) // open sharepoint site              
                {
                    clientContext.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
                    Web web = clientContext.Web;
                    clientContext.Load(web);
                    clientContext.ExecuteQuery();

                    GroupCollection grp_Coll = web.SiteGroups;
                    clientContext.Load(grp_Coll);
                    Group item = grp_Coll.GetByName(region_data.Split(';')[2] + " Owners Group");
                    clientContext.Load(item);
                    clientContext.ExecuteQuery();

                    if (item != null)
                    {
                        RoleDefinition rd_Contribute = clientContext.Web.RoleDefinitions.GetByName("Full Control");
                        //create the role definition binding collection
                        RoleDefinitionBindingCollection rdb_Contribute = new RoleDefinitionBindingCollection(clientContext);
                        //add the role definition to the collection
                        rdb_Contribute.Add(rd_Contribute);
                        web.RoleAssignments.Add(item, rdb_Contribute);
                        ErrorLogs.WriteException("Assigning to New Site - " + region_data.Split(';')[2] + " Owners Group");
                        clientContext.ExecuteQuery();
                        ErrorLogs.WriteException("Assigned to New Site - " + region_data.Split(';')[2] + " Owners Group");
                    }
                   

                }
                #endregion Owner Group Creation

                for (int i = 0; i < group_data.Split(';').Length; i++)
                {
                    string group_name = group_data.Split(';')[i];
                    if (!String.IsNullOrEmpty(group_name) && group_name != ";")
                    {
                        using (var clientContext = new ClientContext(SiteUrl)) // open sharepoint site              
                        {
                            clientContext.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
                            Web web = clientContext.Web;
                            clientContext.Load(web);
                            clientContext.ExecuteQuery();

                            GroupCollection grp_Coll = web.SiteGroups;
                            clientContext.Load(grp_Coll);
                            clientContext.ExecuteQuery();
                            //bool existingFlag = false;
                            //foreach (Group existing_group in grp_Coll)
                            //{
                            //    if (existing_group.Title.Equals(region_data.Split(';')[2] + " Group - " + group_name + " TL"))
                            //    {                                  

                            //        //Get a role
                            //        RoleDefinition rd_second = clientContext.Web.RoleDefinitions.GetByName("Read");
                            //        //create the role definition binding collection
                            //        RoleDefinitionBindingCollection rdb_second = new RoleDefinitionBindingCollection(clientContext);

                            //        //add the role definition to the collection
                            //        rdb_second.Add(rd_second);
                            //        web.RoleAssignments.Add(existing_group, rdb_second);
                            //        ErrorLogs.WriteException("Creating Group on Root - " + existing_group.Title);
                            //        clientContext.ExecuteQuery();
                            //        ErrorLogs.WriteException("Created Group on Root - " + existing_group.Title);
                            //        existingFlag = true;
                            //    }
                            //}


                          
                            //if (!existingFlag)
                            //{
                                //Group Creation
                                GroupCreationInformation grp_second = new GroupCreationInformation();

                                grp_second.Title = Convert.ToString(region_data.Split(';')[2] + " Group - " + group_name + " TL", CultureInfo.CurrentCulture);
                                grp_second.Description = "This is a custom TL group created";
                                //add it to the list of site groups
                                Group newgrp_Second = clientContext.Web.SiteGroups.Add(grp_second);

                                //Get a role
                                RoleDefinition rd_second = clientContext.Web.RoleDefinitions.GetByName("Read");
                                //create the role definition binding collection
                                RoleDefinitionBindingCollection rdb_second = new RoleDefinitionBindingCollection(clientContext);

                                //add the role definition to the collection
                                rdb_second.Add(rd_second);

                                web.RoleAssignments.Add(newgrp_Second, rdb_second);
                                ErrorLogs.WriteException("Creating Group on Root - " + grp_second.Title);
                                clientContext.ExecuteQuery();
                                ErrorLogs.WriteException("Created Group on Root - " + grp_second.Title);
                            //}


                        }

                        using (var clientContext = new ClientContext(SiteUrl + siteName)) // open sharepoint site              
                        {
                            clientContext.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
                            Web web = clientContext.Web;
                            clientContext.Load(web);
                            clientContext.ExecuteQuery();

                            GroupCollection grp_Coll = web.SiteGroups;
                            clientContext.Load(grp_Coll);
                            Group item = grp_Coll.GetByName(region_data.Split(';')[2] + " Group - " + group_name + " TL");
                            clientContext.Load(item);
                            clientContext.ExecuteQuery();

                            if (item != null)
                            {
                                RoleDefinition rd_Contribute = clientContext.Web.RoleDefinitions.GetByName("Contribute");
                                //create the role definition binding collection
                                RoleDefinitionBindingCollection rdb_Contribute = new RoleDefinitionBindingCollection(clientContext);
                                //add the role definition to the collection
                                rdb_Contribute.Add(rd_Contribute);
                                web.RoleAssignments.Add(item, rdb_Contribute);
                                ErrorLogs.WriteException("Assigning to New Site - " + region_data.Split(';')[2] + " Group - " + group_name + " TL");
                                clientContext.ExecuteQuery();
                                ErrorLogs.WriteException("Assigned to New Site - " + region_data.Split(';')[2] + " Group - " + group_name + " TL");
                            }
                            //foreach (Group item in grp_Coll)
                            //{
                            //    if (String.Compare(item.Title, region_data.Split(';')[2] + " Group - " + group_name + " TL", true) == 0)
                            //    {
                            //        //web.RoleAssignments.GetByPrincipal(item).DeleteObject();
                            //        //Get a role

                            //    }
                            //}
                        }

                    }

                }

                for (int i = 0; i < group_area_data.Split(';').Length; i++)
                {
                    string group_area_name = group_area_data.Split(';')[i];
                    if (!String.IsNullOrEmpty(group_area_name) && group_area_name != ";")
                    {
                        using (var clientContext = new ClientContext(SiteUrl)) // open sharepoint site              
                        {
                            clientContext.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
                            Web web = clientContext.Web;
                            clientContext.Load(web);
                            clientContext.ExecuteQuery();

                            GroupCollection grp_Coll = web.SiteGroups;
                            clientContext.Load(grp_Coll);
                            clientContext.ExecuteQuery();
                           
                            bool existingFlag = false;
                            foreach (Group existing_group in grp_Coll)
                            {
                                if (existing_group.Title.Equals(region_data.Split(';')[2] + " Group - " + group_area_name + " AESTL"))
                                {                                   

                                    //Get a role
                                    RoleDefinition rd_second = clientContext.Web.RoleDefinitions.GetByName("Read");
                                    //create the role definition binding collection
                                    RoleDefinitionBindingCollection rdb_second = new RoleDefinitionBindingCollection(clientContext);

                                    //add the role definition to the collection
                                    rdb_second.Add(rd_second);
                                    web.RoleAssignments.Add(existing_group, rdb_second);
                                    ErrorLogs.WriteException("Creating Group on Root - " + existing_group.Title);
                                    clientContext.ExecuteQuery();
                                    ErrorLogs.WriteException("Created Group on Root - " + existing_group.Title);
                                    existingFlag = true;
                                }
                            }
                            if (!existingFlag)
                            {
                                //Group Creation
                                GroupCreationInformation grp_second = new GroupCreationInformation();

                                grp_second.Title = Convert.ToString(region_data.Split(';')[2] + " Group - " + group_area_name + " AESTL", CultureInfo.CurrentCulture);
                                grp_second.Description = "This is a custom AESTL group created";
                                //add it to the list of site groups
                                Group newgrp_Second = clientContext.Web.SiteGroups.Add(grp_second);

                                //Get a role
                                RoleDefinition rd_second = clientContext.Web.RoleDefinitions.GetByName("Read");
                                //create the role definition binding collection
                                RoleDefinitionBindingCollection rdb_second = new RoleDefinitionBindingCollection(clientContext);

                                //add the role definition to the collection
                                rdb_second.Add(rd_second);

                                web.RoleAssignments.Add(newgrp_Second, rdb_second);
                                ErrorLogs.WriteException("Creating Group - " + grp_second.Title);
                                clientContext.ExecuteQuery();
                                ErrorLogs.WriteException("Created Group - " + grp_second.Title);
                            }
                        }

                        using (var clientContext = new ClientContext(SiteUrl + siteName)) // open sharepoint site              
                        {
                            clientContext.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
                            Web web = clientContext.Web;
                            clientContext.Load(web);
                            clientContext.ExecuteQuery();

                            GroupCollection grp_Coll = web.SiteGroups;
                            clientContext.Load(grp_Coll);
                            Group item = grp_Coll.GetByName(region_data.Split(';')[2] + " Group - " + group_area_name + " AESTL");
                            clientContext.Load(item);
                            clientContext.ExecuteQuery();

                            if (item != null)
                            {
                                //Get a role
                                RoleDefinition rd_Contribute = clientContext.Web.RoleDefinitions.GetByName("Contribute");
                                //create the role definition binding collection
                                RoleDefinitionBindingCollection rdb_Contribute = new RoleDefinitionBindingCollection(clientContext);

                                //add the role definition to the collection
                                rdb_Contribute.Add(rd_Contribute);
                                web.RoleAssignments.Add(item, rdb_Contribute);
                                ErrorLogs.WriteException("Assigning to New Site - " + region_data.Split(';')[2] + " Group - " + group_area_name + " AESTL");
                                clientContext.ExecuteQuery();
                                ErrorLogs.WriteException("Assigned to New Site - " + region_data.Split(';')[2] + " Group - " + group_area_name + " AESTL");
                            }

                        }
                    }
                }
                isGroupCreated = true;

                return isGroupCreated;
            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "CreateGroup");
                SendEmail();
                if (ex == null)
                    throw;

            }
            return false;
        }

       

        public static bool CreateListEntries(string siteName)
        {
            try
            {
                bool flag = false;
                                
                    using (var clientContext = new ClientContext(SiteUrl + siteName)) // open sharepoint site              
                    {
                        clientContext.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
                        Web web = clientContext.Web;
                        clientContext.Load(web);

                        List oListArea = web.Lists.GetByTitle("RFA_Areas");
                        clientContext.Load(oListArea);                                           

                        List oListType = web.Lists.GetByTitle("RFA_RequestTypes");
                        clientContext.Load(oListType);

                        List oListTeam = web.Lists.GetByTitle("RFA_FunctionalTeams");
                        clientContext.Load(oListTeam);
                        clientContext.ExecuteQuery();

                        if (oListArea != null && oListType != null && oListTeam != null)
                        {
                            foreach (string area_row in area_data.Split('#'))
                            {
                                string[] area_values = area_row.Split(';');

                                ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                                Microsoft.SharePoint.Client.ListItem oListItem = oListArea.AddItem(itemCreateInfo);
                                oListItem["Title"] = Convert.ToString(area_values[0], CultureInfo.CurrentCulture);
                                if (area_values.Length>1)
                                     oListItem["Description"] = Convert.ToString(area_values[1], CultureInfo.CurrentCulture);
                                oListItem["Block"] = Convert.ToString(area_values[0], CultureInfo.CurrentCulture);
                                oListItem["AllowNewRFAs"] = Convert.ToString(true, CultureInfo.CurrentCulture);
                                oListItem.Update();
                                clientContext.ExecuteQuery();
                            }

                            for (int i = 0; i < type_data.Split('#').Length; i++)
                            {
                                string[] type_values = type_data.Split('#')[i].Split(';');
                                string[] specific_type_names = { "Area Activity", "P&M Project", "Subsea Project", "Worley Parsons Drafting Service Request", "Engineering Query - Area", "Quality or GOC" };

                                if (specific_type_names.Contains(type_values[0]))
                                {
                                    ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                                    Microsoft.SharePoint.Client.ListItem oListItem = oListType.AddItem(itemCreateInfo);
                                    oListItem["Title"] = Convert.ToString(type_values[0], CultureInfo.CurrentCulture);
                                    oListItem["DefaultStageSequence"] = Convert.ToString(type_values[1], CultureInfo.CurrentCulture);
                                    int j = i + 1;
                                    oListItem["Order0"] = Convert.ToDouble(j, CultureInfo.CurrentCulture);
                                    oListItem["ApprovingSupervisorBuildString"] = Convert.ToString(region_data.Split(';')[2] + " " + "Group - {Area} AESTL", CultureInfo.CurrentCulture);
                                    oListItem["SPAType"] = Convert.ToString("MANUAL", CultureInfo.CurrentCulture);
                                    if (type_values[0] == "Area Activity" || type_values[0] == "Subsea Project" || type_values[0] == "Engineering Query - Area")
                                    {
                                        oListItem["AreaRepType"] = Convert.ToString("NONE", CultureInfo.CurrentCulture);                                       
                                    }
                                    else {
                                        oListItem["AreaRepType"] = Convert.ToString("MANUAL", CultureInfo.CurrentCulture);
                                    }


                                    if (type_values[0] == "Area Activity")
                                    {
                                        oListItem["GenericRFAType"] = Convert.ToString("Area", CultureInfo.CurrentCulture);
                                    }
                                    else if (type_values[0] == "P&M Project" || type_values[0] == "Subsea Project")
                                    {
                                        oListItem["GenericRFAType"] = Convert.ToString("Project", CultureInfo.CurrentCulture);
                                    }
                                    else if (type_values[0] == "Worley Parsons Drafting Service Request" || type_values[0] == "Quality or GOC")
                                    {
                                        oListItem["GenericRFAType"] = Convert.ToString("Drafting", CultureInfo.CurrentCulture);
                                    }
                                    else
                                    {
                                        oListItem["GenericRFAType"] = Convert.ToString("EQ", CultureInfo.CurrentCulture);
                                    }
                               
                                    oListItem.Update();
                                    clientContext.ExecuteQuery();


                                    //Functional Teams
                                    GroupCollection grp_Coll = web.SiteGroups;
                                    clientContext.Load(grp_Coll);
                                    Group grp_TeamLead = grp_Coll.GetByName(Convert.ToString(region_data.Split(';')[2] + " " + "Group - " + type_values[0] + " TL", CultureInfo.CurrentCulture));
                                    clientContext.Load(grp_TeamLead);
                                    clientContext.ExecuteQuery();

                                    FieldUserValue userValue = new FieldUserValue();
                                    userValue.LookupId = grp_TeamLead.Id;
                                    ListItemCreationInformation itemCreate = new ListItemCreationInformation();
                                    Microsoft.SharePoint.Client.ListItem oTeamItem = oListTeam.AddItem(itemCreate);

                                    oTeamItem["Order0"] = Convert.ToDouble(j, CultureInfo.CurrentCulture);
                                    oTeamItem["AssociatedSPGroup"] = userValue;
                                    oTeamItem["Title"] = Convert.ToString(region_data.Split(';')[2] + " " + "Group - " + type_values[0] + " TL", CultureInfo.CurrentCulture);
                                    oTeamItem["IncludeInFTLSelections"] = Convert.ToString("Yes", CultureInfo.CurrentCulture);
                                    oTeamItem["IncludeInSupportRequestSelection"] = Convert.ToString("Yes", CultureInfo.CurrentCulture);
                                    oTeamItem.Update();

                                    clientContext.ExecuteQuery();
                                }
                                else
                                {
                                    GroupCollection grp_Coll = web.SiteGroups;
                                    clientContext.Load(grp_Coll);
                                    Group grp_TeamLead = grp_Coll.GetByName(Convert.ToString(region_data.Split(';')[2] + " " + "Group - " + type_values[0] + " TL", CultureInfo.CurrentCulture));
                                    clientContext.Load(grp_TeamLead);
                                    clientContext.ExecuteQuery();

                                    FieldUserValue userValue = new FieldUserValue();
                                    userValue.LookupId = grp_TeamLead.Id;

                                    ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
                                    Microsoft.SharePoint.Client.ListItem oListItem = oListType.AddItem(itemCreateInfo);
                                    oListItem["Title"] = Convert.ToString(type_values[0], CultureInfo.CurrentCulture);
                                    oListItem["DefaultStageSequence"] = Convert.ToString(type_values[1], CultureInfo.CurrentCulture);
                                    int j = i + 1;
                                    oListItem["Order0"] = Convert.ToDouble(j, CultureInfo.CurrentCulture);
                                    oListItem["SPAType"] = Convert.ToString("MANUAL", CultureInfo.CurrentCulture);
                                    oListItem["AreaRepType"] = Convert.ToString("MANUAL", CultureInfo.CurrentCulture);
                                    oListItem["GenericRFAType"] = Convert.ToString("EQ", CultureInfo.CurrentCulture);
                                    oListItem["ApprovingSupervisorBuildString"] = Convert.ToString(region_data.Split(';')[2] + " " + "Group - {Area} AESTL", CultureInfo.CurrentCulture);

                                    oListItem["FunctionalTeamLeadGroup"] = userValue;
                                    oListItem["FunctionalTeamLeadType"] = Convert.ToString(region_data.Split(';')[2] + " " + "Group - " + type_values[0] + " TL", CultureInfo.CurrentCulture);
                                    oListItem.Update();
                                    clientContext.ExecuteQuery();

                                    ListItemCreationInformation itemCreate = new ListItemCreationInformation();
                                    Microsoft.SharePoint.Client.ListItem oTeamItem = oListTeam.AddItem(itemCreate);

                                    oTeamItem["Order0"] = Convert.ToDouble(j, CultureInfo.CurrentCulture);
                                    oTeamItem["AssociatedSPGroup"] = userValue;
                                    oTeamItem["Title"] = Convert.ToString(region_data.Split(';')[2] + " " + "Group - " + type_values[0] + " TL", CultureInfo.CurrentCulture);
                                    oTeamItem["IncludeInFTLSelections"] = Convert.ToString("Yes", CultureInfo.CurrentCulture);
                                    oTeamItem["IncludeInSupportRequestSelection"] = Convert.ToString("Yes", CultureInfo.CurrentCulture);
                                    oTeamItem.Update();

                                    clientContext.ExecuteQuery();
                                }
                            }                           
                            flag = true;
                        }
                        else {
                            flag = false;

                        }
                    }
                
                return flag;
            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "CreateListEntries");
                SendEmail();
                if (ex == null)
                    throw;
                
            }
            return false;
        }

        //public static bool EntiresForSpecificTypes(string siteName)
        //{
        //    //bool flag = false;
        //    //try
        //    //{
        //    //    using (var clientContext = new ClientContext(SiteUrl + siteName)) // open sharepoint site              
        //    //    {
        //    //        clientContext.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
        //    //        Web web = clientContext.Web;
        //    //        clientContext.Load(web);
                  

        //    //        List oListType = web.Lists.GetByTitle("RFA_RequestTypes");
        //    //        clientContext.Load(oListType);                 
        //    //        clientContext.ExecuteQuery();

                   

        //    //        flag = true;
        //    //    }
        //    //}
        //    //catch (Exception)
        //    //{
                
        //    //    throw;
        //    //}
        //    //return flag;    
        //}

        public static void SendEmail()
        {
            try
            {                 
                using (var clientContext = new ClientContext(SiteUrl)) // open sharepoint site              
                {
                    clientContext.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);

                    Web web = clientContext.Web;
                    clientContext.Load(web);
                    GroupCollection grp_Coll = web.SiteGroups;
                    clientContext.Load(grp_Coll);
                    Group item = grp_Coll.GetByName("RFA System Admins");
                    clientContext.Load(item);
                    clientContext.ExecuteQuery();
                    clientContext.Load(item.Users);
                    clientContext.ExecuteQuery();

                    var emailp = new EmailProperties();
                    List<string> emailList = new List<string>();
                    foreach (User user in item.Users)
                    {

                        emailList.Add(user.Email);
                    }
                    emailp.To = emailList;

                    List oList = web.Lists.GetByTitle("EmailTemplate");
                    string query = "<View><Query><Where> <Eq>  <FieldRef Name='Title' />   <Value Type='Text'>Failure</Value>   </Eq>   </Where></Query></View>";
                    CamlQuery camlQuery = new CamlQuery();
                    camlQuery.ViewXml = query;
                    ListItemCollection collListItemInProc = oList.GetItems(camlQuery);
                    clientContext.Load(oList);
                    clientContext.Load(collListItemInProc);
                    clientContext.ExecuteQuery();

                    if (collListItemInProc.Count <= 0)
                    {
                        //emailp.From = "Kashyap.Makadia@bp.com";
                        emailp.Body = Convert.ToString(collListItemInProc[0]["EmailBody"], CultureInfo.CurrentCulture).Replace("@Site", exc_site_name);
                        emailp.Subject = Convert.ToString(collListItemInProc[0]["EmailSubject"], CultureInfo.CurrentCulture).Replace("@Site", exc_site_name);
                        Utility.SendEmail(clientContext, emailp);
                        clientContext.ExecuteQuery();
                    }
                
                }

            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "SendEmail"); 
                if (ex == null)
                    throw;
            }
        }
        #endregion Site Creation

      
        #region Archival
        private static void ArchiveData()
        {
            try
            {
                //put closed/completed condition - completed
                //list item deletion - Completed
                //audit list data - Completed
                //ms all rules
                //loop for subsites - completed
                List<string> subWebUrls = new List<string>();
                using (var clientContextTop = new ClientContext(SiteUrl))
                {
                    clientContextTop.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
                    Web oWebsite = clientContextTop.Web;
                    clientContextTop.Load(oWebsite, website => website.Webs, website => website.Title);
                    clientContextTop.ExecuteQuery();
                    foreach (Web orWebsite in oWebsite.Webs)
                    {
                        subWebUrls.Add(orWebsite.Url);
                    }
                }

                foreach (string webUrl in subWebUrls)
                {
                    using (var clientContext = new ClientContext(webUrl))
                     {
                        clientContext.ExecutingWebRequest += new EventHandler<WebRequestEventArgs>(clientContext_ExecutingWebRequest);
                      
                        List oList = clientContext.Web.Lists.GetByTitle("RFA_Requests");
                        List oListArchival = GetListByTitle(clientContext,"Archive_Requests");

                        if (oListArchival != null)
                        {

                            string query = @"<View><Query><Where><Eq><FieldRef Name='system_ItemStatus'/>" + "<Value Type='Text'>05a Completed</Value></Eq></Where></Query></View>"; ;
                            CamlQuery camlQuery = new CamlQuery();
                            camlQuery.ViewXml = query;
                            ListItemCollection collListItem = oList.GetItems(camlQuery);

                            clientContext.Load(oList);
                            clientContext.Load(oListArchival);
                            clientContext.Load(collListItem);
                            clientContext.ExecuteQuery();
                            List<int> itemsToDelete = new List<int>();


                            foreach (ListItem srcItem in collListItem)
                            {
                                ListItem destItem = LisItemCopyOperations(oList, oListArchival, srcItem, clientContext);


                                destItem.Update();
                                clientContext.ExecuteQuery();
                                ArchiveAuditOfActions(clientContext, srcItem.Id);
                                itemsToDelete.Add(srcItem.Id);
                            }

                            foreach (int deleteItemID in itemsToDelete)
                            {

                                ListItem listItem = oList.GetItemById(deleteItemID);
                                listItem.DeleteObject();
                                clientContext.ExecuteQuery();
                            }
                            itemsToDelete = null;
                        }
                    }
                }
                subWebUrls = null;
            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "ArchiveData");               
                if (ex == null)
                    throw;
            }
        
        }


        public static List GetListByTitle(ClientContext clientContext, String listTitle)
        {
            List existingList;

            Web web = clientContext.Web;
            ListCollection lists = web.Lists;

            IEnumerable<List> existingLists = clientContext.LoadQuery(
                    lists.Where(
                    list => list.Title == listTitle)
                    );
            clientContext.ExecuteQuery();

            existingList = existingLists.FirstOrDefault();

            return existingList;
        }

       

        private static ListItem LisItemCopyOperations(List oList, List oListArchival,  ListItem srcItem, ClientContext clientContext)
        {
            ListItemCreationInformation itemCreateInfo = new ListItemCreationInformation();
            Microsoft.SharePoint.Client.ListItem destItem = oListArchival.AddItem(itemCreateInfo);
            try
            {            

                clientContext.Load(oList.Fields);
                clientContext.Load(srcItem);
                clientContext.ExecuteQuery();


                foreach (Field field in oList.Fields)
                {
                    if (!field.ReadOnlyField && !field.Hidden && field.InternalName != "Attachments" && field.InternalName != "ContentType")
                    {
                        if (srcItem.FieldValues[field.InternalName] != null)
                        {
                            if (field.FieldTypeKind == FieldType.User)
                            {
                                FieldUserValue userValue = srcItem[field.InternalName] as FieldUserValue;
                                if (userValue.LookupId != 0)
                                {
                                    FieldUserValue newUserValue = new FieldUserValue();
                                    newUserValue.LookupId = userValue.LookupId;
                                    destItem[field.InternalName] = newUserValue;
                                    destItem.Update();
                                }
                            }
                            else
                            {
                                destItem[field.InternalName] = srcItem.FieldValues[field.InternalName];                               
                                destItem.Update();
                            }                            
                        }
                    }
                       
                    //if (oList.Title == "RFA_Requests" && oListArchival.Title == "Archive_Requests")
                    //{
                    //    if (field.ReadOnlyField)
                    //    {
                    //        if (srcItem.FieldValues[field.InternalName] != null)
                    //            if (field.InternalName == "ID")
                    //            { 
                    //                destItem["OriginalID"] = srcItem.FieldValues[field.InternalName];
                    //                destItem.Update();
                    //            }
                    //    }
                    //}
                   
                }
                if (oList.Title == "RFA_Requests" && oListArchival.Title == "Archive_Requests")
                {
                    destItem["OriginalID"] = srcItem.FieldValues["ID"];
                    destItem.Update();
                }
               
            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "ArchiveData");
                if (ex == null)
                    throw;
            }
            return destItem;
        }

        private static void ArchiveAuditOfActions(ClientContext clientContext ,int rfaID)
        {
            try
            {
                string auditOfActionListName = "RFA_AuditOfActions";
                string archivalAuditOfActionListName = "Archive_AuditOfActions";

                List oList = clientContext.Web.Lists.GetByTitle(auditOfActionListName);
                List oListArchival = clientContext.Web.Lists.GetByTitle(archivalAuditOfActionListName);

                

                string query = @"<View><Query><Where><Eq><FieldRef Name='RFA_RequestID'/>" + "<Value Type='Text'>" + Convert.ToString(rfaID,CultureInfo.InvariantCulture) + "</Value></Eq></Where></Query></View>"; ;
                CamlQuery camlQuery = new CamlQuery();
                camlQuery.ViewXml = query;
                ListItemCollection collListItem = oList.GetItems(camlQuery);

                clientContext.Load(oList);
                clientContext.Load(oListArchival);
                clientContext.Load(collListItem);
                clientContext.ExecuteQuery();
                List<int> itemsToDelete = new List<int>();


                foreach (ListItem srcItem in collListItem)
                {
                    ListItem destItem = LisItemCopyOperations(oList, oListArchival, srcItem, clientContext);


                    destItem.Update();
                    clientContext.ExecuteQuery();
                    itemsToDelete.Add(srcItem.Id);
                }

                foreach (int deleteItemID in itemsToDelete)
                {

                    ListItem listItem = oList.GetItemById(deleteItemID);
                    listItem.DeleteObject();
                    clientContext.ExecuteQuery();
                }
                itemsToDelete = null;
            }
            catch (Exception ex)
            {
                ErrorLogs.LogException(ex, "ArchiveAuditOfActions");
                if (ex == null)
                    throw;
            }
        }

        #endregion Archival

        #region Context


        public static void clientContext_ExecutingWebRequest(object sender, WebRequestEventArgs e)
        {
            try
            {

                //e.WebRequestExecutor.WebRequest.Headers.Add("X-FORMS_BASED_AUTH_ACCEPTED", "f");
                //SP url is hard coded for tests
                e.WebRequestExecutor.WebRequest.CookieContainer = GetFedAuthCookie();

                //ToDo: Test with any SP url
                //e.WebRequestExecutor.WebRequest.CookieContainer = GetFedAuthCookie(e.WebRequestExecutor.WebRequest.Address);
            }
            catch (Exception ex)
            {
                //ErrorLogs.Log(System.Reflection.MethodBase.GetCurrentMethod().Name, ex);
                if (ex == null) throw;
            }
        }

        public static string GetConfigValue(string resourceName)
        {
            return ConfigurationManager.AppSettings[resourceName];
        }

        private static CookieContainer GetFedAuthCookie()
        {
            string ADFSUrl = GetConfigValue("ADFSUrl");//ADFS Url
            string IDPUrl = GetConfigValue("IDPUrl");//IDP Url
            string SPWEBUrl = GetConfigValue("SPWEBUrl");//Web Url
            string SPSiteUrl = GetConfigValue("SPSiteUrl");//Site Url
            string SPSitePath = GetConfigValue("SPSitePath");//Site Path
            string UserName = GetConfigValue("UserName");//UserName
            string Password = GetConfigValue("Password");//Password
            string RelayPartyName = GetConfigValue("RelayPartyName");//RelayPartyName

            var tokenInfo = new TokenInfo();
            tokenInfo.ADFSUrl = ADFSUrl;
            tokenInfo.IDPUrl = IDPUrl;
            tokenInfo.SPWebUrl = SPWEBUrl;
            tokenInfo.SPSiteUrl = SPSiteUrl;
            tokenInfo.SPSitePath = SPSitePath;
            tokenInfo.UserName = UserName; //Bp1 credentials
            tokenInfo.Password = Password;
            tokenInfo.RelayPartyName = RelayPartyName;


            return TokenIssuer.GetFedAuthCookieContainer(tokenInfo, true);
        }
        #endregion Context

    }
}
