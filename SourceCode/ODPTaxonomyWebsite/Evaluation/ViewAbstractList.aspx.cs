using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace ODPTaxonomyWebsite.Evaluation
{
    public partial class ViewAbstractList : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Session["CurrentRole"] != null)
            string Mainview = null;
            int MainviewIndex = -1;
            string Subview = null;
            int SubviewIndex = -1;

            SetPager();

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
        protected void LoadViewDropDownData(List<string> roles, string SelectedView)
                MainviewDDL.Items.Add(new ListItem("Select a View", ""));
                    ListItem item = new ListItem(role + " View", role);
                    item.Selected = SelectedView == role;

                    MainviewDDL.Items.Add(item);
        protected void RenderAbstractListView(string Mainview, string Subview = "")
            AdminView.Visible = false;
            CoderSupervisor_Coded.Visible = false;
            CoderSupervisor_Open.Visible = false;
            ODPStaffView_Default.Visible = false;
            ODPStaffView_Review.Visible = false;
            ODPStaffView_Review_Uncoded.Visible = false;
            ODPSupervisorView_Default.Visible = false;
            ODPSupervisorView_Open.Visible = false;
            switch (Mainview)
                    AdminView.Visible = true;
                    if (Subview == "open")
                        CoderSupervisor_Open.Visible = true;
                    else
                        CoderSupervisor_Coded.Visible = true;
                    if (Subview == "open")
                        ODPSupervisorView_Open.Visible = true;
                        ODPSupervisorView_Default.Visible = true;
                    if (Subview == "review")
                        ODPStaffView_Review.Visible = true;
                    else if (Subview == "uncoded")
                        ODPStaffView_Review_Uncoded.Visible = true;
                        ODPStaffView_Default.Visible = true;
            {
                lbl_messageUsers.Text = "Your Current Role: " + Session["CurrentRole"].ToString();
            }

            Response.Redirect("~/Evaluation/ViewAbstractList.aspx");
        }
    }
}