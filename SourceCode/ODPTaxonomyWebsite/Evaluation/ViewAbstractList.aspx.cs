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
            }
            else
            {
                string selectedView = ViewDDL.SelectedValue;
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
                ViewDDL.Items.Clear();
                ViewDDL.Items.Add(new ListItem("Select a view", ""));
                foreach (string role in roles)
                {
                    ViewDDL.Items.Add(new ListItem(role + " view", role));
                }
                ViewDDL.Visible = true;
            }
            else
            {
                ViewDDL.Visible = false;
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
                    break;
                case "ODPStaffSupervisor":
                    SubviewLabel.Text = "Abstract Types:";
                    SubviewLabel.Visible = true;

                    SubviewDDL.Items.Add(new ListItem("Open Abstracts", "open"));
                    SubviewDDL.Items.Add(new ListItem("Abstract Review", "review"));

                    if (selectedIndex > -1)
                    {
                        SubviewDDL.SelectedIndex = selectedIndex;
                    }

                    SubviewDDL.Visible = true;
                    break;
                default:
                    SubviewLabel.Visible = false;
                    SubviewDDL.Visible = false;
                    break;
            }
        }

        protected void RenderAbstractListView(string view)
        {
            Control abstractView = null;

            switch (view)
            {
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
                    else if (SubviewDDL.SelectedValue == "review")
                    {
                        abstractView = LoadControl("~/Evaluation/AbstractListViews/ODPSupervisorView_Review.ascx") as ODPSupervisorView_Review;
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
    }
}