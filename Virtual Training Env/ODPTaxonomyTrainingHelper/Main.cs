using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Configuration;
using System.Data.Linq;

namespace ODPTaxonomyTrainingHelper
{
    public partial class Main : Form
    {
        public Main()
        {
            InitializeComponent();
        }

        private void Main_Load(object sender, EventArgs e)
        {
        }

        private void btnFillRobot_Click(object sender, EventArgs e)
        {
            try
            {
                // db connection string
                string strConnectionString = ConfigurationManager.ConnectionStrings["ODPTaxonomyTrainingHelper.Properties.Settings.ODPTrainingConnectionString"].ToString();
                int l_instance;

                // check if instance is set correctly
                if (Int32.TryParse(ConfigurationManager.AppSettings["instance"].ToString(), out l_instance))
                {
                    List<Tr_CheckAbstractStatusResult> results = new List<Tr_CheckAbstractStatusResult>();
                    int? l_returnValue;
                    int l_abstractID;
                    int? l_evaluationID;
                    int l_resultCnt;

                    this.btnFillRobot.Enabled = false;

                    // check if coder fill an abstract yet
                    using (ODPTaxTrainingDataContext db = new ODPTaxTrainingDataContext(strConnectionString))
                    {
                        results = db.Tr_CheckAbstractStatus().ToList();
                        l_resultCnt = results.Count();
                    }

                    if (l_resultCnt == 0)
                    {
                        MessageBox.Show("No abstract coded.  You need to code an abstract first.", "Status", MessageBoxButtons.OK);
                    }
                    else if (l_resultCnt == 1)
                    {
                        Tr_CheckAbstractStatusResult rec = (Tr_CheckAbstractStatusResult)results.SingleOrDefault();
                        l_abstractID = rec.AbstractID;
                        l_evaluationID = rec.EvaluationId;

                        using (ODPTaxTrainingDataContext db = new ODPTaxTrainingDataContext(strConnectionString))
                        {
                            l_returnValue = db.Tr_Fill_Robots(l_instance, l_abstractID, l_evaluationID).SingleOrDefault().returnValue;
                        }

                        if (l_returnValue == 1)
                        {
                            // success
                            MessageBox.Show("Robots' data entered successfully.  Ready for consensus.", "Status", MessageBoxButtons.OK);
                        }
                        else
                        {
                            MessageBox.Show("Cannot fill robots' data.  Check with your supervisor", "Error", MessageBoxButtons.OK);
                        }

                    }
                    else
                    {
                        MessageBox.Show("Robots' data already filled. Please perform consensus.", "Status", MessageBoxButtons.OK);
                    }

                    this.btnFillRobot.Enabled = true;

                }
                else
                {
                    MessageBox.Show("Incorrect instance configuration.", "Error", MessageBoxButtons.OK);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Error", MessageBoxButtons.OK);
            }

        }
    }
}
