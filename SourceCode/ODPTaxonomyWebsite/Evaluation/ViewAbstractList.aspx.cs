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

            if (!IsPostBack)
            {
                LoadViewDropDownData(roles);

                if (roles.Count == 1)
                {
                    RenderAbstractListView(roles[0]);
                }

                // set pager size
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
            else
            {
                var targetID = Request.Form["__EVENTTARGET"];
                // check if we are changing main view
                // if we are, reset the subview
                if (targetID.Contains("MainviewDDL"))
                {
                    SubviewDDL.SelectedIndex = -1;
                }

                string selectedView = MainviewDDL.SelectedValue;
                int selectedSubview = SubviewDDL.SelectedIndex;

                if (!string.IsNullOrEmpty(selectedView))
                {
                    if (roles.Contains(selectedView))
                    {
                        LoadSubviewDropDownData(selectedView, selectedSubview);
                        RenderAbstractListView(selectedView);
                    }
                }
            }
        }

        /**
         * Loads data for select a view dropdown
         */
        protected void LoadViewDropDownData(List<string> roles)
        {
            if (roles.Count > 1)
            {
                MainviewDDL.Items.Clear();
                MainviewDDL.Items.Add(new ListItem("Select a view", ""));
                foreach (string role in roles)
                {
                    MainviewDDL.Items.Add(new ListItem(role + " view", role));
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

        protected void RenderAbstractListView(string view)
        {
            Control abstractView = null;

            switch (view)
            {
                case "Admin":
                    abstractView = LoadControl("~/Evaluation/AbstractListViews/AdminView.ascx") as AdminView;
                    break;
                case "CoderSupervisor":
                    if (SubviewDDL.SelectedValue == "coded")
                    {
                        abstractView = LoadControl("~/Evaluation/AbstractListViews/CoderSupervisorView_Coded.ascx") as CoderSupervisorView_Coded;
                    }
                    else if (SubviewDDL.SelectedValue == "open")
                    {
                        abstractView = LoadControl("~/Evaluation/AbstractListViews/CoderSupervisorView_Open.ascx") as CoderSupervisorView_Open;
                    }
                    break;
                case "ODPStaffSupervisor":
                    if (SubviewDDL.SelectedValue == "open")
                    {
                        abstractView = LoadControl("~/Evaluation/AbstractListViews/ODPSupervisorView_Open.ascx") as ODPSupervisorView_Open;
                    }
                    else
                    {
                        abstractView = LoadControl("~/Evaluation/AbstractListViews/ODPSupervisorView_Default.ascx") as ODPSupervisorView_Default;
                    }
                    break;
                case "ODPStaffMember":
                    if (SubviewDDL.SelectedValue == "review")
                    {
                        abstractView = LoadControl("~/Evaluation/AbstractListViews/ODPStaffMemberView_Review.ascx") as ODPStaffMemberView_Review;
                    }
                    else if (SubviewDDL.SelectedValue == "uncoded")
                    {
                        abstractView = LoadControl("~/Evaluation/AbstractListViews/ODPStaffMemberView_Review_Uncoded.ascx") as ODPStaffMemberView_Review_Uncoded;
                    }
                    else
                    {
                        abstractView = LoadControl("~/Evaluation/AbstractListViews/ODPStaffMemberView_Default.ascx") as ODPStaffMemberView_Default;
                    }
                    break;
                default:
                    break;
            }

            if (abstractView != null)
            {
                AbstractViewPlaceHolder.Controls.Add(abstractView);
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