using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ODPTaxonomyDAL_JY;

namespace ODPTaxonomyWebsite.Evaluation
{
    public partial class ViewAbstractList : System.Web.UI.Page
    {
        public string userGUID = "";
        public string userROLE = "";

        protected void Page_Load(object sender, EventArgs e)
        {
            int ViewInt = 0;
            List<string> UserRoles = Roles.GetRolesForUser().ToList();
            MembershipUser user = Membership.GetUser();
            userGUID = ((Guid)user.ProviderUserKey).ToString();
            IDictionary<int, string> ViewRoles = AbstractListViewHelper.GetViewRoles(UserRoles);

            if (string.IsNullOrEmpty(Request.QueryString["view"]))
            {
                LoadViewDropDownData(ViewRoles);
                return;
            }

            if (!int.TryParse(Request.QueryString["view"], out ViewInt))
            {
                LoadViewDropDownData(ViewRoles);
                return;
            }

            if (!Enum.IsDefined(typeof(AbstractViewRole), ViewInt))
            {
                LoadViewDropDownData(ViewRoles);
                return;
            }

            AbstractViewRole Mainview = (AbstractViewRole)ViewInt;
            string Subview = !string.IsNullOrEmpty(Request.QueryString["subview"]) ? Request.QueryString["subview"] : "";

            if (!AbstractListViewHelper.UserCanView(Mainview))
            {
                // commenting out just for testing..
                //return;
                
            }

            LoadViewDropDownData(ViewRoles, Mainview);
            LoadSubviewDropDownData(Mainview, Subview);
            RenderAbstractListView(Mainview, Subview);
            //SetPager();
        }

        protected void MainviewChangeHandler(object sender, EventArgs e)
        {
            int Mainview = 0;
            if (int.TryParse(MainviewDDL.SelectedValue, out Mainview))
            {
                string RedirectURL = "";
                switch ((AbstractViewRole)Mainview)
                {
                    case AbstractViewRole.Admin:
                        RedirectURL = "ViewAbstractList.aspx?view=" + (int)AbstractViewRole.Admin;
                        break;
                    case AbstractViewRole.CoderSupervisor:
                        RedirectURL = "ViewAbstractList.aspx?view=" + (int)AbstractViewRole.CoderSupervisor;
                        break;
                    case AbstractViewRole.ODPStaff:
                        RedirectURL = "ViewAbstractList.aspx?view=" + (int)AbstractViewRole.ODPStaff;
                        break;
                    case AbstractViewRole.ODPSupervisor:
                        RedirectURL = "ViewAbstractList.aspx?view=" + (int)AbstractViewRole.ODPSupervisor;
                        break;

                    case AbstractViewRole.EMPTY:
                    default:
                        RedirectURL = "ViewAbstractList.aspx";
                        break;
                }

                Response.Redirect(RedirectURL);
            }
        }

        protected void SubviewChangeHandler(object sender, EventArgs e)
        {
            int Mainview = 0;

            if (MainviewDDL.Visible)
            {
                int.TryParse(MainviewDDL.SelectedValue, out Mainview);
            }
            else
            {
                int.TryParse(Request.QueryString["view"], out Mainview);
            }

            string RedirectURL = "";
            switch ((AbstractViewRole)Mainview)
            {
                case AbstractViewRole.Admin:
                    RedirectURL = "ViewAbstractList.aspx?view=" + (int)AbstractViewRole.Admin;
                    break;
                case AbstractViewRole.CoderSupervisor:
                    RedirectURL = "ViewAbstractList.aspx?view=" + (int)AbstractViewRole.CoderSupervisor;
                    break;
                case AbstractViewRole.ODPStaff:
                    RedirectURL = "ViewAbstractList.aspx?view=" + (int)AbstractViewRole.ODPStaff;
                    break;
                case AbstractViewRole.ODPSupervisor:
                    RedirectURL = "ViewAbstractList.aspx?view=" + (int)AbstractViewRole.ODPSupervisor;
                    break;
            }

            if (!string.IsNullOrEmpty(RedirectURL))
            {
                RedirectURL += "&subview=" + SubviewDDL.SelectedValue;
                Response.Redirect(RedirectURL);
            }
        }

        //protected void SetPager()
        //{
        //    if (!IsPostBack)
        //    {
        //        if (HttpContext.Current.Request.Cookies["Pager"] != null)
        //        {
        //            int TempPagerSize;

        //            if (int.TryParse(HttpContext.Current.Request.Cookies["Pager"]["Size"].ToString(), out TempPagerSize))
        //            {
        //                switch (TempPagerSize)
        //                {
        //                    case 25:
        //                        PagerSizeDDL.SelectedIndex = 0;
        //                        break;
        //                    case 50:
        //                        PagerSizeDDL.SelectedIndex = 1;
        //                        break;
        //                    case 100:
        //                        PagerSizeDDL.SelectedIndex = 2;
        //                        break;
        //                    default:
        //                        break;
        //                }
        //                PagerWrapper.Visible = true;
        //            }
        //        }
        //    }
        //}

        /**
         * Loads data for select a view dropdown
         */
        protected void LoadViewDropDownData(IDictionary<int, string> Roles, AbstractViewRole SelectedView = AbstractViewRole.EMPTY)
        {
            if (!IsPostBack)
            {
                if (Roles.Count > 1)
                {
                    MainviewDDL.Items.Clear();
                    MainviewDDL.Items.Add(new ListItem("Select a View", ""));
                    foreach (KeyValuePair<int, string> entry in Roles)
                    {
                        ListItem item = new ListItem(entry.Value + " View", entry.Key.ToString());
                        item.Selected = (int)SelectedView == entry.Key;

                        MainviewDDL.Items.Add(item);
                    }
                }
                else
                {
                    MainviewDDL.Visible = false;
                }
            }
        }

        protected void LoadSubviewDropDownData(AbstractViewRole Role, string SelectedValue = "")
        {
            if (!IsPostBack)
            {
                SubviewDDL.Items.Clear();
                switch (Role)
                {
                    case AbstractViewRole.CoderSupervisor:
                        SubviewLabel.Text = "Abstract Types:";
                        SubviewLabel.Visible = true;

                        SubviewDDL.Items.Add(new ListItem("Coded Abstracts", "coded"));
                        SubviewDDL.Items.Add(new ListItem("Open Abstracts", "open"));

                        if (!string.IsNullOrEmpty(SelectedValue))
                        {
                            foreach (ListItem item in SubviewDDL.Items)
                            {
                                item.Selected = SelectedValue == item.Value;
                            }
                        }

                        SubviewDDL.Visible = true;
                        SubviewPanel.Visible = true;
                        break;
                    case AbstractViewRole.ODPStaff:
                        SubviewLabel.Text = "Abstract Types:";
                        SubviewLabel.Visible = true;

                        SubviewDDL.Items.Add(new ListItem("Default View", ""));
                        SubviewDDL.Items.Add(new ListItem("In Review List", "review"));
                        SubviewDDL.Items.Add(new ListItem("In Review List - Uncoded Only", "uncoded"));

                        if (!string.IsNullOrEmpty(SelectedValue))
                        {
                            foreach (ListItem item in SubviewDDL.Items)
                            {
                                item.Selected = SelectedValue == item.Value;
                            }
                        }

                        SubviewDDL.Visible = true;
                        SubviewPanel.Visible = true;
                        break;
                    case AbstractViewRole.ODPSupervisor:
                        SubviewLabel.Text = "Abstract Types:";
                        SubviewLabel.Visible = true;

                        SubviewDDL.Items.Add(new ListItem("Default View", ""));
                        SubviewDDL.Items.Add(new ListItem("Open Abstract", "open"));
                        SubviewDDL.Items.Add(new ListItem("In Review List", "review"));
                        SubviewDDL.Items.Add(new ListItem("In Review List - Uncoded Only", "uncoded"));

                        if (!string.IsNullOrEmpty(SelectedValue))
                        {
                            foreach (ListItem item in SubviewDDL.Items)
                            {
                                item.Selected = SelectedValue == item.Value;
                            }
                        }

                        SubviewDDL.Visible = true;
                        SubviewPanel.Visible = true;
                        break;

                    case AbstractViewRole.Admin:
                    default:
                        SubviewLabel.Visible = false;
                        SubviewDDL.Visible = false;
                        SubviewPanel.Visible = false;
                        break;
                }
            }
        }

        protected void RenderAbstractListView(AbstractViewRole Mainview, string Subview = "")
        {
            AdminView.Visible = false;
            CoderSupervisor_Coded.Visible = false;
            CoderSupervisor_Open.Visible = false;
            ODPStaffView_Default.Visible = false;
            ODPStaffView_Review.Visible = false;
            ODPStaffView_Review_Uncoded.Visible = false;
            ODPSupervisorView_Default.Visible = false;
            ODPSupervisorView_Open.Visible = false;
            ODPSupervisorView_Review.Visible = false;
            ODPSupervisorView_Review_Uncoded.Visible = false;

            Response.Write(" Main View Role :: " + Mainview.ToString());
            switch (Mainview)
            {
                case AbstractViewRole.Admin:
                    AdminView.Visible = true;
                    userROLE = "Admin";
                    break;
                case AbstractViewRole.CoderSupervisor: // 4
                    userROLE = "CoderSupervisor";
                    if (Subview == "open")
                    {
                        CoderSupervisor_Open.Visible = false;
                        ODPSupervisorView_Default.Visible = true;
                    }
                    else
                    {
                        CoderSupervisor_Coded.Visible = false;
                        ODPSupervisorView_Default.Visible = true;
                    }
                    break;
                case AbstractViewRole.ODPSupervisor:
                    userROLE = "ODPSupervisor";
                    if (Subview == "open")
                    {
                        ODPSupervisorView_Open.Visible = true;
                    }
                    else if (Subview == "review")
                    {
                        ODPSupervisorView_Review.Visible = true;
                    }
                    else if (Subview == "uncoded")
                    {
                        ODPSupervisorView_Review_Uncoded.Visible = true;
                    }
                    else
                    {
                        ODPSupervisorView_Default.Visible = true;
                    }
                    break;
                case AbstractViewRole.ODPStaff:
                    userROLE = "ODPStaff";
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

        //protected void PagerSizeChangeHandler(object sender, EventArgs e)
        //{
        //    int NewPagerSize;

        //    if (int.TryParse(PagerSizeDDL.SelectedValue, out NewPagerSize))
        //    {
        //        switch (NewPagerSize)
        //        {
        //            case 25:
        //            case 50:
        //            case 100:
        //                HttpCookie PagerCookie = new HttpCookie("Pager");
        //                PagerCookie["size"] = NewPagerSize.ToString();
        //                PagerCookie.Expires = DateTime.Now.AddYears(1);

        //                Response.Cookies.Add(PagerCookie);
        //                break;
        //            default:
        //                break;
        //        }
        //    }

        //    Response.Redirect("~/Evaluation/ViewAbstractList.aspx?view=" + (!string.IsNullOrEmpty(Request.QueryString["view"]) ? Request.QueryString["view"] : ""));
        //}
    }
}