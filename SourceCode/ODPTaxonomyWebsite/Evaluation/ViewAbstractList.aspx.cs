﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ODPTaxonomyDAL_TT;
using ODPTaxonomyWebsite.Evaluation.AbstractListViews;

namespace ODPTaxonomyWebsite.Evaluation
{
    public partial class ViewAbstractList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            List<string> roles = Roles.GetRolesForUser().ToList();
            roles.Remove("Coder");

            string Mainview = null;
            int MainviewIndex = -1;
            string Subview = null;
            int SubviewIndex = -1;

            SetPager();

            if (!IsPostBack)
            {
                if (roles.Count == 1)
                {
                    Mainview = roles[0];
                }
            }
            else
            {
                if (roles.Count == 1)
                {
                    Mainview = roles[0];

                    Subview = SubviewDDL.SelectedValue;
                    SubviewIndex = SubviewDDL.SelectedIndex;
                }
                else
                {
                    Mainview = MainviewDDL.SelectedValue;
                    MainviewIndex = MainviewDDL.SelectedIndex;

                    Subview = SubviewDDL.SelectedValue;
                    SubviewIndex = SubviewDDL.SelectedIndex;

                    // switching mainview, reset subview
                    var targetID = Request.Form["__EVENTTARGET"];
                    if (targetID.Contains("MainviewDDL"))
                    {
                        SubviewIndex = -1;
                    }
                }
            }

            SetPager();
            LoadViewDropDownData(roles, Mainview);
            LoadSubviewDropDownData(Mainview, SubviewIndex);

            if (!string.IsNullOrEmpty(Mainview))
            {
                RenderAbstractListView(Mainview, Subview);
            }
        }

        protected void SetPager()
        {
            if (HttpContext.Current.Request.Cookies["Pager"] != null)
            {
                int TempPagerSize;

                if (int.TryParse(HttpContext.Current.Request.Cookies["Pager"]["Size"].ToString(), out TempPagerSize))
                {
                    switch (TempPagerSize)
                    {
                        case 25:
                            PagerSizeDDL.SelectedIndex = 0;
                            break;
                        case 50:
                            PagerSizeDDL.SelectedIndex = 1;
                            break;
                        case 100:
                            PagerSizeDDL.SelectedIndex = 2;
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        /**
         * Loads data for select a view dropdown
         */
        protected void LoadViewDropDownData(List<string> roles, string SelectedView)
        {
            if (roles.Count > 1)
            {
                MainviewDDL.Items.Clear();
                MainviewDDL.Items.Add(new ListItem("Select a View", ""));
                foreach (string role in roles)
                {
                    ListItem item = new ListItem(role + " View", role);
                    item.Selected = SelectedView == role;

                    MainviewDDL.Items.Add(item);
                }
                MainviewDDL.Visible = true;
            }
            else
            {
                MainviewDDL.Visible = false;
            }
        }

        protected void LoadSubviewDropDownData(string role, int selectedIndex = -1)
        {
            SubviewDDL.Items.Clear();
            switch (role)
            {
                case "CoderSupervisor":
                    SubviewLabel.Text = "Abstract Types:";
                    SubviewLabel.Visible = true;

                    SubviewDDL.Items.Add(new ListItem("Coded Abstracts", "coded"));
                    SubviewDDL.Items.Add(new ListItem("Open Abstracts", "open"));

                    if (selectedIndex > -1)
                    {
                        SubviewDDL.SelectedIndex = selectedIndex;
                    }

                    SubviewDDL.Visible = true;
                    SubviewPanel.Visible = true;
                    break;
                case "ODPStaffMember":
                    SubviewLabel.Text = "Abstract Types:";
                    SubviewLabel.Visible = true;

                    SubviewDDL.Items.Add(new ListItem("Default View", ""));
                    SubviewDDL.Items.Add(new ListItem("In Review List", "review"));
                    SubviewDDL.Items.Add(new ListItem("In Review List - Uncoded Only", "uncoded"));

                    if (selectedIndex > -1)
                    {
                        SubviewDDL.SelectedIndex = selectedIndex;
                    }

                    SubviewDDL.Visible = true;
                    SubviewPanel.Visible = true;
                    break;
                case "ODPStaffSupervisor":
                    SubviewLabel.Text = "Abstract Types:";
                    SubviewLabel.Visible = true;

                    SubviewDDL.Items.Add(new ListItem("Default View", ""));
                    SubviewDDL.Items.Add(new ListItem("Open Abstract", "open"));

                    if (selectedIndex > -1)
                    {
                        SubviewDDL.SelectedIndex = selectedIndex;
                    }

                    SubviewDDL.Visible = true;
                    SubviewPanel.Visible = true;
                    break;

                case "Admin":
                default:
                    SubviewLabel.Visible = false;
                    SubviewDDL.Visible = false;
                    SubviewPanel.Visible = false;
                    break;
            }
        }

        protected void RenderAbstractListView(string Mainview, string Subview = "")
        {
            switch (Mainview)
            {
                case "Admin":
                    AdminView.Visible = true;
                    break;
                case "CoderSupervisor":
                    if (Subview == "open")
                    {
                        CoderSupervisor_Open.Visible = true;
                    }
                    else
                    {
                        CoderSupervisor_Coded.Visible = true;
                    }
                    break;
                case "ODPStaffSupervisor":
                    if (Subview == "open")
                    {
                        ODPSupervisorView_Open.Visible = true;
                    }
                    else
                    {
                        ODPSupervisorView_Default.Visible = true;
                    }
                    break;
                case "ODPStaffMember":
                    if (Subview == "review")
                    {
                        ODPStaffView_Review.Visible = true;
                    }
                    else if (Subview == "uncoded")
                    {
                        ODPStaffView_Review_Uncoded.Visible = true;
                    }
                    else
                    {
                        ODPStaffView_Default.Visible = true;
                    }
                    break;
                default:
                    break;
            }
        }

        protected void PagerSizeChangeHandler(object sender, EventArgs e)
        {
            int NewPagerSize;

            if (int.TryParse(PagerSizeDDL.SelectedValue, out NewPagerSize))
            {
                switch (NewPagerSize)
                {
                    case 25:
                    case 50:
                    case 100:
                        HttpCookie PagerCookie = new HttpCookie("Pager");
                        PagerCookie["size"] = NewPagerSize.ToString();
                        PagerCookie.Expires = DateTime.Now.AddYears(1);

                        Response.Cookies.Add(PagerCookie);
                        break;
                    default:
                        break;
                }
            }

            Response.Redirect("~/Evaluation/ViewAbstractList.aspx");
        }
    }
}