using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;

namespace ODPTaxonomyDAL_JY
{
    public static class AbstractListViewHelper
    {
        private static string KappaSpecifier = "f";


        public static bool UserCanView(AbstractViewRole View)
        {
            switch (View)
            {
                case AbstractViewRole.Admin:
                    return Roles.IsUserInRole("Admin");
                case AbstractViewRole.CoderSupervisor:
                    return Roles.IsUserInRole("CoderSupervisor");
                case AbstractViewRole.ODPStaff:
                    return Roles.IsUserInRole("ODPStaffMember");
                case AbstractViewRole.ODPSupervisor:
                    return Roles.IsUserInRole("ODPStaffSupervisor");
            }

            return false;
        }

        public static IDictionary<int, string> GetViewRoles(List<string> UserRoles)
        {
            IDictionary<int, string> ViewRoles = new Dictionary<int, string>();

            foreach (string role in UserRoles)
            {
                switch (role)
                {
                    case "Admin":
                        ViewRoles.Add((int)AbstractViewRole.Admin, "Admin");
                        break;
                    case "CoderSupervisor":
                        ViewRoles.Add((int)AbstractViewRole.CoderSupervisor, "Coder Supervisor");
                        break;
                    case "ODPStaffMember":
                        ViewRoles.Add((int)AbstractViewRole.ODPStaff, "ODP Staff");
                        break;
                    case "ODPStaffSupervisor":
                        ViewRoles.Add((int)AbstractViewRole.ODPSupervisor, "ODP Supervisor");
                        break;
                }
            }

            return ViewRoles;
        }

        public static List<AbstractListRow> ProcessAbstracts(List<AbstractListRow> ParentAbstracts, AbstractViewRole AbstractView)
        {
            List<AbstractListRow> Abstracts = new List<AbstractListRow>();
            AbstractListViewData data = new AbstractListViewData();

            for (int i = 0; i < ParentAbstracts.Count; i++)
            {
                //ParentAbstracts[i].GetSubmissionData(SubmissionTypeEnum.CODER_CONSENSUS);
                ParentAbstracts[i].GetAbstractScan(AbstractView);
                //ParentAbstracts[i].ChildRows = new List<AbstractListRow>();
                // gets all kappa data for abstract
                var KappaData = data.GetAbstractKappaData(ParentAbstracts[i].AbstractID);

                if (KappaData.Count() > 0)
                {
                    // fill in k1 value
                    Abstracts.Add(FillInKappaValue(ParentAbstracts[i], KappaData, KappaTypeEnum.K1));

                    if (AbstractView == AbstractViewRole.CoderSupervisor || AbstractView == AbstractViewRole.ODPSupervisor)
                    {
                        // get coder evaluation row
                        // and fill in k2 - k4 value
                        var CoderEvaluations = data.GetCoderEvaluations(ParentAbstracts[i].AbstractID);
                        if (CoderEvaluations != null && CoderEvaluations.TeamID != null)
                        {
                            var CoderKapperIdentities = data.GetKappaIdentities(CoderEvaluations.TeamID.Value);
                            if (CoderKapperIdentities.Count() > 0)
                            {
                                foreach (var iden in CoderKapperIdentities)
                                {
                                    foreach (var kappa in KappaData)
                                    {
                                        if (iden.UserAlias == "CdrA" && kappa.KappaTypeID == (int)KappaTypeEnum.K2)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, "IQS " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionData(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
                                            //ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K3)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, "IQS " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionData(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
                                            //ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K4)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, "IQS " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionData(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
                                            //ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            Abstracts.Add(CoderEvaluation);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // fill in k5 value
                    foreach (var kappa in KappaData)
                    {
                        if (kappa.KappaTypeID == (int)KappaTypeEnum.K5)
                        {
                            AbstractListRow ODPConsensus = ConstructNewAbstractListRow(kappa, "ODP Avg", ParentAbstracts[i].AbstractID);
                            ODPConsensus.GetSubmissionData(SubmissionTypeEnum.ODP_STAFF_CONSENSUS);
                            Abstracts.Add(ODPConsensus);
                        }
                    }

                    if (AbstractView == AbstractViewRole.ODPSupervisor)
                    {
                        // get odp staff evaluation row
                        // and fill in k6 - k8 value
                        var ODPEvaluations = data.GetODPEvaluations(ParentAbstracts[i].AbstractID);
                        if (ODPEvaluations != null && ODPEvaluations.TeamID != null)
                        {
                            var ODPCoderKapperIdentities = data.GetKappaIdentities(ODPEvaluations.TeamID.Value);
                            if (ODPCoderKapperIdentities.Count() > 0)
                            {
                                foreach (var iden in ODPCoderKapperIdentities)
                                {
                                    foreach (var kappa in KappaData)
                                    {
                                        if (iden.UserAlias == "ODPA" && kappa.KappaTypeID == (int)KappaTypeEnum.K6)
                                        {
                                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            ODPEvaluation.GetSubmissionData(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId);
                                            //ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                            Abstracts.Add(ODPEvaluation);
                                        }
                                        else if (iden.UserAlias == "ODPB" && kappa.KappaTypeID == (int)KappaTypeEnum.K7)
                                        {
                                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            ODPEvaluation.GetSubmissionData(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId);
                                            //ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                            Abstracts.Add(ODPEvaluation);
                                        }
                                        else if (iden.UserAlias == "ODPC" && kappa.KappaTypeID == (int)KappaTypeEnum.K8)
                                        {
                                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            ODPEvaluation.GetSubmissionData(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId);
                                            //ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                            Abstracts.Add(ODPEvaluation);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // fill in k9 value
                    foreach (var kappa in KappaData)
                    {
                        if (kappa.KappaTypeID == (int)KappaTypeEnum.K9)
                        {
                            AbstractListRow ODPCoderComparison = ConstructNewAbstractListRow(kappa, "IQS vs. ODP", ParentAbstracts[i].AbstractID);
                            ODPCoderComparison.GetSubmissionData(SubmissionTypeEnum.ODP_STAFF_COMPARISON);
                            Abstracts.Add(ODPCoderComparison);
                        }
                    }

                    /* add k10-k12 */
                    if (AbstractView == AbstractViewRole.ODPSupervisor)
                    {
                        // get coder evaluation row
                        // and fill in k10 - k12 value
                        var CoderEvaluations = data.GetCoderEvaluations(ParentAbstracts[i].AbstractID);
                        if (CoderEvaluations != null && CoderEvaluations.TeamID != null)
                        {
                            var CoderKapperIdentities = data.GetKappaIdentities(CoderEvaluations.TeamID.Value);
                            if (CoderKapperIdentities.Count() > 0)
                            {
                                foreach (var iden in CoderKapperIdentities)
                                {
                                    foreach (var kappa in KappaData)
                                    {
                                        if (iden.UserAlias == "CdrA" && kappa.KappaTypeID == (int)KappaTypeEnum.K10)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, "IQS "+ iden.UserName +" vs. ODP", ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionData(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
                                            Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K11)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, "IQS " + iden.UserName + " vs. ODP", ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionData(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
                                            Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K12)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, "IQS " + iden.UserName + " vs. ODP", ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionData(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
                                            Abstracts.Add(CoderEvaluation);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    /* end of add k10-k12 */
                }
                else
                {
                    Abstracts.Add(ParentAbstracts[i]);
                }
            }

            return Abstracts;
        }

        private static string getContractorName()
        {
            string retVal = "";
            try
            {
                retVal = System.Configuration.ConfigurationManager.AppSettings["contractorName"];
            }
            catch (Exception)
            {
               retVal = "IQS";
            }

            return retVal;
        }

        public static List<AbstractListRow> ProcessAbstracts2(List<AbstractListRow> ParentAbstracts, AbstractViewRole AbstractView)
        {
            List<AbstractListRow> Abstracts = new List<AbstractListRow>();
            AbstractListViewData data = new AbstractListViewData();
            string contractorName = getContractorName();
            var cacheKappaData = data.getAllKappaRecords();
            var cacheSubmissions = data.getAllSubmissionRecords();
            var cacheEvaluations = data.getAllEvaluationRecords();
            var cacheF_PreventionCategoryAnswers = data.getAllF_PreventionCategoryRecords();
            var cacheE_StudyDesignPurposeAnswers = data.getAllE_StudyDesignPurposeRecords();

            for (int i = 0; i < ParentAbstracts.Count; i++)
            {
                //HttpContext.Current.Response.Write("done :: " + i + "<br>");
                //HttpContext.Current.Response.Flush();
                ParentAbstracts[i].GetSubmissionData2(SubmissionTypeEnum.CODER_CONSENSUS, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                ParentAbstracts[i].GetAbstractScan(AbstractView);
                ParentAbstracts[i].ChildRows = new List<AbstractListRow>();

                //var KappaData2 = data.GetAbstractKappaData(ParentAbstracts[i].AbstractID);
                var KappaData = data.GetAbstractKappaData2(ParentAbstracts[i].AbstractID, cacheKappaData);
                //IEnumerable<KappaData> KappaData = null;

                if (KappaData.Count() > 0)
                {
                    // fill in k1 value
                    Abstracts.Add(FillInKappaValue(ParentAbstracts[i], KappaData, KappaTypeEnum.K1));             
                    ParentAbstracts[i].KappaCount = KappaData.Count();
                    
                    if (1 == 2) // Emulating no Entry here.
                    {

                        if (AbstractView == AbstractViewRole.CoderSupervisor || AbstractView == AbstractViewRole.ODPSupervisor)
                        {
                            // get coder evaluation row
                            // and fill in k2 - k4 value
                            var CoderEvaluations = data.GetCoderEvaluations(ParentAbstracts[i].AbstractID);
                            if (CoderEvaluations != null && CoderEvaluations.TeamID != null)
                            {
                                var CoderKapperIdentities = data.GetKappaIdentities(CoderEvaluations.TeamID.Value);
                                if (CoderKapperIdentities.Count() > 0)
                                {
                                    foreach (var iden in CoderKapperIdentities)
                                    {
                                        foreach (var kappa in KappaData)
                                        {
                                            if (iden.UserAlias == "CdrA" && kappa.KappaTypeID == (int)KappaTypeEnum.K2)
                                            {
                                                AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                                                CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                                //Abstracts.Add(CoderEvaluation);
                                            }
                                            else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K3)
                                            {
                                                AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                                                CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                                //Abstracts.Add(CoderEvaluation);
                                            }
                                            else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K4)
                                            {
                                                AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                                                CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                                //Abstracts.Add(CoderEvaluation);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // fill in k5 value
                        foreach (var kappa in KappaData)
                        {
                            if (kappa.KappaTypeID == (int)KappaTypeEnum.K5)
                            {
                                AbstractListRow ODPConsensus = ConstructNewAbstractListRow(kappa, "ODP Avg", ParentAbstracts[i].AbstractID);
                                ODPConsensus.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_CONSENSUS, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                ParentAbstracts[i].ChildRows.Add(ODPConsensus);
                                //Abstracts.Add(ODPConsensus);
                            }
                        }

                        if (AbstractView == AbstractViewRole.ODPSupervisor)
                        {
                            // get odp staff evaluation row
                            // and fill in k6 - k8 value
                            var ODPEvaluations = data.GetODPEvaluations(ParentAbstracts[i].AbstractID);
                            if (ODPEvaluations != null && ODPEvaluations.TeamID != null)
                            {
                                var ODPCoderKapperIdentities = data.GetKappaIdentities(ODPEvaluations.TeamID.Value);
                                if (ODPCoderKapperIdentities.Count() > 0)
                                {
                                    foreach (var iden in ODPCoderKapperIdentities)
                                    {
                                        foreach (var kappa in KappaData)
                                        {
                                            if (iden.UserAlias == "ODPA" && kappa.KappaTypeID == (int)KappaTypeEnum.K6)
                                            {
                                                AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                                ODPEvaluation.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                                //Abstracts.Add(ODPEvaluation);
                                            }
                                            else if (iden.UserAlias == "ODPB" && kappa.KappaTypeID == (int)KappaTypeEnum.K7)
                                            {
                                                AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                                ODPEvaluation.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                                //Abstracts.Add(ODPEvaluation);
                                            }
                                            else if (iden.UserAlias == "ODPC" && kappa.KappaTypeID == (int)KappaTypeEnum.K8)
                                            {
                                                AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                                ODPEvaluation.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                                //Abstracts.Add(ODPEvaluation);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // fill in k9 value
                        foreach (var kappa in KappaData)
                        {
                            if (kappa.KappaTypeID == (int)KappaTypeEnum.K9)
                            {
                                AbstractListRow ODPCoderComparison = ConstructNewAbstractListRow(kappa, contractorName + " " + "vs R", ParentAbstracts[i].AbstractID);
                                ODPCoderComparison.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_COMPARISON, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                ParentAbstracts[i].ChildRows.Add(ODPCoderComparison);
                                //Abstracts.Add(ODPCoderComparison);
                            }
                        }

                        /* add k10-k12 */
                        if (AbstractView == AbstractViewRole.ODPSupervisor)
                        {
                            // get coder evaluation row
                            // and fill in k10 - k12 value
                            var CoderEvaluations = data.GetCoderEvaluations(ParentAbstracts[i].AbstractID);
                            if (CoderEvaluations != null && CoderEvaluations.TeamID != null)
                            {
                                var CoderKapperIdentities = data.GetKappaIdentities(CoderEvaluations.TeamID.Value);
                                if (CoderKapperIdentities.Count() > 0)
                                {
                                    foreach (var iden in CoderKapperIdentities)
                                    {
                                        foreach (var kappa in KappaData)
                                        {
                                            if (iden.UserAlias == "CdrA" && kappa.KappaTypeID == (int)KappaTypeEnum.K10)
                                            {
                                                AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                                CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                                //Abstracts.Add(CoderEvaluation);
                                            }
                                            else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K11)
                                            {
                                                AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                                CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                                //Abstracts.Add(CoderEvaluation);
                                            }
                                            else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K12)
                                            {
                                                AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                                CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                                //Abstracts.Add(CoderEvaluation);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        /* end of add k10-k12 */

                        /* add k13 */
                        if (AbstractView == AbstractViewRole.ODPSupervisor)
                        {
                            // fill in k13 value
                            foreach (var kappa in KappaData)
                            {
                                if (kappa.KappaTypeID == (int)KappaTypeEnum.K13)
                                {
                                    AbstractListRow ODPCoderComparison = ConstructNewAbstractListRow(kappa, "ODP" + " " + "vs R", ParentAbstracts[i].AbstractID);
                                    ODPCoderComparison.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_COMPARISON, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                    ParentAbstracts[i].ChildRows.Add(ODPCoderComparison);
                                    //Abstracts.Add(ODPCoderComparison);
                                }
                            }
                        }
                        /* end k13 */

                    }

                }
                else
                {
                    ParentAbstracts[i].KappaCount = 0;
                    Abstracts.Add(ParentAbstracts[i]);
                }
            }

            return Abstracts;
        }

        public static List<AbstractListRow> ProcessAbstractsIndividual(List<AbstractListRow> ParentAbstracts, AbstractViewRole AbstractView)
        {
            List<AbstractListRow> Abstracts = new List<AbstractListRow>();
            AbstractListViewData data = new AbstractListViewData();
            string contractorName = getContractorName();
            var cacheKappaData = data.getAllKappaRecords();
            var cacheSubmissions = data.getAllSubmissionRecords();
            var cacheEvaluations = data.getAllEvaluationRecords();
            var cacheF_PreventionCategoryAnswers = data.getAllF_PreventionCategoryRecords();
            var cacheE_StudyDesignPurposeAnswers = data.getAllE_StudyDesignPurposeRecords();

            for (int i = 0; i < ParentAbstracts.Count; i++)
            {
                //HttpContext.Current.Response.Write("done :: " + i + "<br>");
                //HttpContext.Current.Response.Flush();
                ParentAbstracts[i].GetSubmissionData2(SubmissionTypeEnum.CODER_CONSENSUS, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                ParentAbstracts[i].GetAbstractScan(AbstractView);
                ParentAbstracts[i].ChildRows = new List<AbstractListRow>();

                //var KappaData2 = data.GetAbstractKappaData(ParentAbstracts[i].AbstractID);
                var KappaData = data.GetAbstractKappaData2(ParentAbstracts[i].AbstractID, cacheKappaData);
                //IEnumerable<KappaData> KappaData = null;

                if (KappaData.Count() > 0)
                {
                    // fill in k1 value
                    Abstracts.Add(FillInKappaValue(ParentAbstracts[i], KappaData, KappaTypeEnum.K1));
                    ParentAbstracts[i].KappaCount = KappaData.Count();


                        if (AbstractView == AbstractViewRole.CoderSupervisor || AbstractView == AbstractViewRole.ODPSupervisor)
                        {
                            // get coder evaluation row
                            // and fill in k2 - k4 value
                            var CoderEvaluations = data.GetCoderEvaluations(ParentAbstracts[i].AbstractID);
                            if (CoderEvaluations != null && CoderEvaluations.TeamID != null)
                            {
                                var CoderKapperIdentities = data.GetKappaIdentities(CoderEvaluations.TeamID.Value);
                                if (CoderKapperIdentities.Count() > 0)
                                {
                                    foreach (var iden in CoderKapperIdentities)
                                    {
                                        foreach (var kappa in KappaData)
                                        {
                                            if (iden.UserAlias == "CdrA" && kappa.KappaTypeID == (int)KappaTypeEnum.K2)
                                            {
                                                AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                                                CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                                //Abstracts.Add(CoderEvaluation);
                                            }
                                            else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K3)
                                            {
                                                AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                                                CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                                //Abstracts.Add(CoderEvaluation);
                                            }
                                            else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K4)
                                            {
                                                AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                                                CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                                //Abstracts.Add(CoderEvaluation);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // fill in k5 value
                        foreach (var kappa in KappaData)
                        {
                            if (kappa.KappaTypeID == (int)KappaTypeEnum.K5)
                            {
                                AbstractListRow ODPConsensus = ConstructNewAbstractListRow(kappa, "ODP Avg", ParentAbstracts[i].AbstractID);
                                ODPConsensus.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_CONSENSUS, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                ParentAbstracts[i].ChildRows.Add(ODPConsensus);
                                //Abstracts.Add(ODPConsensus);
                            }
                        }

                        if (AbstractView == AbstractViewRole.ODPSupervisor)
                        {
                            // get odp staff evaluation row
                            // and fill in k6 - k8 value
                            var ODPEvaluations = data.GetODPEvaluations(ParentAbstracts[i].AbstractID);
                            if (ODPEvaluations != null && ODPEvaluations.TeamID != null)
                            {
                                var ODPCoderKapperIdentities = data.GetKappaIdentities(ODPEvaluations.TeamID.Value);
                                if (ODPCoderKapperIdentities.Count() > 0)
                                {
                                    foreach (var iden in ODPCoderKapperIdentities)
                                    {
                                        foreach (var kappa in KappaData)
                                        {
                                            if (iden.UserAlias == "ODPA" && kappa.KappaTypeID == (int)KappaTypeEnum.K6)
                                            {
                                                AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                                ODPEvaluation.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                                //Abstracts.Add(ODPEvaluation);
                                            }
                                            else if (iden.UserAlias == "ODPB" && kappa.KappaTypeID == (int)KappaTypeEnum.K7)
                                            {
                                                AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                                ODPEvaluation.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                                //Abstracts.Add(ODPEvaluation);
                                            }
                                            else if (iden.UserAlias == "ODPC" && kappa.KappaTypeID == (int)KappaTypeEnum.K8)
                                            {
                                                AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                                ODPEvaluation.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                                //Abstracts.Add(ODPEvaluation);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        // fill in k9 value
                        foreach (var kappa in KappaData)
                        {
                            if (kappa.KappaTypeID == (int)KappaTypeEnum.K9)
                            {
                                AbstractListRow ODPCoderComparison = ConstructNewAbstractListRow(kappa, contractorName + " " + "vs R", ParentAbstracts[i].AbstractID);
                                ODPCoderComparison.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_COMPARISON, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                ParentAbstracts[i].ChildRows.Add(ODPCoderComparison);
                                //Abstracts.Add(ODPCoderComparison);
                            }
                        }

                        /* add k10-k12 */
                        if (AbstractView == AbstractViewRole.ODPSupervisor)
                        {
                            // get coder evaluation row
                            // and fill in k10 - k12 value
                            var CoderEvaluations = data.GetCoderEvaluations(ParentAbstracts[i].AbstractID);
                            if (CoderEvaluations != null && CoderEvaluations.TeamID != null)
                            {
                                var CoderKapperIdentities = data.GetKappaIdentities(CoderEvaluations.TeamID.Value);
                                if (CoderKapperIdentities.Count() > 0)
                                {
                                    foreach (var iden in CoderKapperIdentities)
                                    {
                                        foreach (var kappa in KappaData)
                                        {
                                            if (iden.UserAlias == "CdrA" && kappa.KappaTypeID == (int)KappaTypeEnum.K10)
                                            {
                                                AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                                CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                                //Abstracts.Add(CoderEvaluation);
                                            }
                                            else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K11)
                                            {
                                                AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                                CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                                //Abstracts.Add(CoderEvaluation);
                                            }
                                            else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K12)
                                            {
                                                AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                                CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                                ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                                //Abstracts.Add(CoderEvaluation);
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        /* end of add k10-k12 */

                        /* add k13 */
                        if (AbstractView == AbstractViewRole.ODPSupervisor)
                        {
                            // fill in k13 value
                            foreach (var kappa in KappaData)
                            {
                                if (kappa.KappaTypeID == (int)KappaTypeEnum.K13)
                                {
                                    AbstractListRow ODPCoderComparison = ConstructNewAbstractListRow(kappa, "ODP" + " " + "vs R", ParentAbstracts[i].AbstractID);
                                    ODPCoderComparison.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_COMPARISON, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                                    ParentAbstracts[i].ChildRows.Add(ODPCoderComparison);
                                    //Abstracts.Add(ODPCoderComparison);
                                }
                            }
                        }
                        /* end k13 */

                    

                }
                else
                {
                    ParentAbstracts[i].KappaCount = 0;
                    Abstracts.Add(ParentAbstracts[i]);
                }
            }

            return Abstracts;
        }

        public static List<AbstractListRow> ProcessAbstractsIndividualNoCache(List<AbstractListRow> ParentAbstracts, AbstractViewRole AbstractView)
        {
            List<AbstractListRow> Abstracts = new List<AbstractListRow>();
            AbstractListViewData data = new AbstractListViewData();
            string contractorName = getContractorName();

            //
           
            for (int i = 0; i < ParentAbstracts.Count; i++)
            {

                ParentAbstracts[i].GetSubmissionData(SubmissionTypeEnum.CODER_CONSENSUS);
                ParentAbstracts[i].GetAbstractScan(AbstractView);
                ParentAbstracts[i].ChildRows = new List<AbstractListRow>();
                // gets all kappa data for abstract
                var KappaData = data.GetAbstractKappaData(ParentAbstracts[i].AbstractID);

                if (KappaData.Count() > 0)
                {
                    // fill in k1 value
                    Abstracts.Add(FillInKappaValue(ParentAbstracts[i], KappaData, KappaTypeEnum.K1));

                    if (AbstractView == AbstractViewRole.CoderSupervisor || AbstractView == AbstractViewRole.ODPSupervisor)
                    {
                        // get coder evaluation row
                        // and fill in k2 - k4 value
                        var CoderEvaluations = data.GetCoderEvaluations(ParentAbstracts[i].AbstractID);
                        if (CoderEvaluations != null && CoderEvaluations.TeamID != null)
                        {
                            var CoderKapperIdentities = data.GetKappaIdentities(CoderEvaluations.TeamID.Value);
                            if (CoderKapperIdentities.Count() > 0)
                            {
                                foreach (var iden in CoderKapperIdentities)
                                {
                                    foreach (var kappa in KappaData)
                                    {
                                        if (iden.UserAlias == "CdrA" && kappa.KappaTypeID == (int)KappaTypeEnum.K2)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionData(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K3)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionData(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K4)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionData(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // fill in k5 value
                    foreach (var kappa in KappaData)
                    {
                        if (kappa.KappaTypeID == (int)KappaTypeEnum.K5)
                        {
                            AbstractListRow ODPConsensus = ConstructNewAbstractListRow(kappa, "ODP Avg", ParentAbstracts[i].AbstractID);
                            ODPConsensus.GetSubmissionData(SubmissionTypeEnum.ODP_STAFF_CONSENSUS);
                            ParentAbstracts[i].ChildRows.Add(ODPConsensus);
                            //Abstracts.Add(ODPConsensus);
                        }
                    }

                    if (AbstractView == AbstractViewRole.ODPSupervisor)
                    {
                        // get odp staff evaluation row
                        // and fill in k6 - k8 value
                        var ODPEvaluations = data.GetODPEvaluations(ParentAbstracts[i].AbstractID);
                        if (ODPEvaluations != null && ODPEvaluations.TeamID != null)
                        {
                            var ODPCoderKapperIdentities = data.GetKappaIdentities(ODPEvaluations.TeamID.Value);
                            if (ODPCoderKapperIdentities.Count() > 0)
                            {
                                foreach (var iden in ODPCoderKapperIdentities)
                                {
                                    foreach (var kappa in KappaData)
                                    {
                                        if (iden.UserAlias == "ODPA" && kappa.KappaTypeID == (int)KappaTypeEnum.K6)
                                        {
                                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            ODPEvaluation.GetSubmissionData(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId);
                                            ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                            //Abstracts.Add(ODPEvaluation);
                                        }
                                        else if (iden.UserAlias == "ODPB" && kappa.KappaTypeID == (int)KappaTypeEnum.K7)
                                        {
                                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            ODPEvaluation.GetSubmissionData(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId);
                                            ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                            //Abstracts.Add(ODPEvaluation);
                                        }
                                        else if (iden.UserAlias == "ODPC" && kappa.KappaTypeID == (int)KappaTypeEnum.K8)
                                        {
                                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            ODPEvaluation.GetSubmissionData(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId);
                                            ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                            //Abstracts.Add(ODPEvaluation);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // fill in k9 value
                    foreach (var kappa in KappaData)
                    {
                        if (kappa.KappaTypeID == (int)KappaTypeEnum.K9)
                        {
                            AbstractListRow ODPCoderComparison = ConstructNewAbstractListRow(kappa, contractorName + " " + "vs R", ParentAbstracts[i].AbstractID);
                            ODPCoderComparison.GetSubmissionData(SubmissionTypeEnum.ODP_STAFF_COMPARISON);
                            ParentAbstracts[i].ChildRows.Add(ODPCoderComparison);
                            //Abstracts.Add(ODPCoderComparison);
                        }
                    }

                    /* add k10-k12 */
                    if (AbstractView == AbstractViewRole.ODPSupervisor)
                    {
                        // get coder evaluation row
                        // and fill in k10 - k12 value
                        var CoderEvaluations = data.GetCoderEvaluations(ParentAbstracts[i].AbstractID);
                        if (CoderEvaluations != null && CoderEvaluations.TeamID != null)
                        {
                            var CoderKapperIdentities = data.GetKappaIdentities(CoderEvaluations.TeamID.Value);
                            if (CoderKapperIdentities.Count() > 0)
                            {
                                foreach (var iden in CoderKapperIdentities)
                                {
                                    foreach (var kappa in KappaData)
                                    {
                                        if (iden.UserAlias == "CdrA" && kappa.KappaTypeID == (int)KappaTypeEnum.K10)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionData(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K11)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionData(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K12)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionData(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    /* end of add k10-k12 */

                    /* add k13 */
                    if (AbstractView == AbstractViewRole.ODPSupervisor)
                    {
                        // fill in k13 value
                        foreach (var kappa in KappaData)
                        {
                            if (kappa.KappaTypeID == (int)KappaTypeEnum.K13)
                            {
                                AbstractListRow ODPCoderComparison = ConstructNewAbstractListRow(kappa, "ODP" + " " + "vs R", ParentAbstracts[i].AbstractID);
                                ODPCoderComparison.GetSubmissionData(SubmissionTypeEnum.ODP_STAFF_COMPARISON);
                                ParentAbstracts[i].ChildRows.Add(ODPCoderComparison);
                                //Abstracts.Add(ODPCoderComparison);
                            }
                        }
                    }
                    /* end k13 */

                }
                else
                {
                    Abstracts.Add(ParentAbstracts[i]);
                }
            }

            return Abstracts;
        }


        public static List<AbstractListRow> SortAbstracts(List<AbstractListRow> Abstracts, string Sort, SortDirection SortDirection)
        {
            switch (Sort)
            {
                case "ApplicationID":
                    if (SortDirection == SortDirection.Ascending)
                    {
                        return Abstracts.OrderBy(d => d.ApplicationID).ToList();
                    }
                    else
                    {
                        return Abstracts.OrderByDescending(d => d.ApplicationID).ToList();
                    }
                case "Title":
                    if (SortDirection == SortDirection.Ascending)
                    {
                        return Abstracts.OrderBy(d => d.ProjectTitle).ToList();
                    }
                    else
                    {
                        return Abstracts.OrderByDescending(d => d.ProjectTitle).ToList();
                    }
                case "Date":
                    if (SortDirection == SortDirection.Ascending)
                    {
                        return Abstracts.OrderBy(d => d.StatusDate).ToList();
                    }
                    else
                    {
                        return Abstracts.OrderByDescending(d => d.StatusDate).ToList();
                    }
                default:
                    return Abstracts.OrderByDescending(d => d.StatusDate).ToList();
            }
        }

        public static AbstractListRow ConstructNewAbstractListRow(KappaData Kappa, string Title, int AbstractID = 0)
        {
            return new AbstractListRow
            {
                AbstractID = AbstractID,
                ProjectTitle = Title,
                //A1 = ((decimal)(Kappa.A1)).ToString(KappaSpecifier),
                //A2 = ((decimal)(Kappa.A2)).ToString(KappaSpecifier),
                //A3 = ((decimal)(Kappa.A3)).ToString(KappaSpecifier),
                //B = ((decimal)(Kappa.B)).ToString(KappaSpecifier),
                //C = ((decimal)(Kappa.C)).ToString(KappaSpecifier),
                //D = ((decimal)(Kappa.D)).ToString(KappaSpecifier),
                //E = ((decimal)(Kappa.E)).ToString(KappaSpecifier),
                //F = ((decimal)(Kappa.F)).ToString(KappaSpecifier)

                   A1 = convertKappa(Kappa.A1),
                A2 = convertKappa(Kappa.A2),
                A3 = convertKappa(Kappa.A3),
                B = convertKappa(Kappa.B),
                C = convertKappa(Kappa.C),
                D = convertKappa(Kappa.D),
                E = convertKappa(Kappa.E),
                F = convertKappa(Kappa.F)
            };
        }

        public static AbstractListRow FillInKappaValue(AbstractListRow Abstract, IEnumerable<KappaData> KappaData, KappaTypeEnum KappaType)
        {
            foreach (var Kappa in KappaData)
            {
                if (Kappa.KappaTypeID == (int)KappaType)
                {
                    //Abstract.A1 = ((decimal)(Kappa.A1)).ToString(KappaSpecifier);
                    //Abstract.A2 = ((decimal)(Kappa.A2)).ToString(KappaSpecifier);
                    //Abstract.A3 = ((decimal)(Kappa.A3)).ToString(KappaSpecifier);
                    //Abstract.B = ((decimal)(Kappa.B)).ToString(KappaSpecifier);
                    //Abstract.C = ((decimal)(Kappa.C)).ToString(KappaSpecifier);
                    //Abstract.D = ((decimal)(Kappa.D)).ToString(KappaSpecifier);
                    //Abstract.E = ((decimal)(Kappa.E)).ToString(KappaSpecifier);
                    //Abstract.F = ((decimal)(Kappa.F)).ToString(KappaSpecifier);

                    Abstract.A1 = convertKappa(Kappa.A1);
                    Abstract.A2 = convertKappa(Kappa.A2);
                    Abstract.A3 = convertKappa(Kappa.A3);
                    Abstract.B = convertKappa(Kappa.B);
                    Abstract.C = convertKappa(Kappa.C);
                    Abstract.D = convertKappa(Kappa.D);
                    Abstract.E = convertKappa(Kappa.E);
                    Abstract.F = convertKappa(Kappa.F);

                    break;
                }
            }

            return Abstract;
        }

        private static string convertKappa(decimal? inVal)
        {
            try
            {
                return ((decimal)(inVal)).ToString(KappaSpecifier);
            }
            catch {
                return "\u2014";
            }

        }

        public static void AbstractListRowCreatedHandler(object sender, GridViewRowEventArgs e)
        {
            if (sender == null)
            {
                return;
            }

            AbstractGridView AbstractGridView = sender as AbstractGridView;

            if (e.Row.RowType == DataControlRowType.Header)
            {
                foreach (TableCell tc in e.Row.Cells)
                {
                    if (tc.HasControls())
                    {
                        LinkButton headerLink = (LinkButton)tc.Controls[0];

                        if (AbstractGridView.Attributes["CurrentSortExp"] != null)
                        {
                            if (AbstractGridView.Attributes["CurrentSortExp"] == headerLink.CommandArgument)
                            {
                                tc.CssClass += " sorted ";
                                tc.CssClass += AbstractGridView.Attributes["CurrentSortDir"] == "ASC" ? "ascending " : "descending ";
                            }
                        }
                        else if (headerLink.CommandArgument == "Date")
                        {
                            tc.CssClass += " sorted descending ";
                        }

                        tc.CssClass = tc.CssClass.Trim();
                    }
                }
            }
        }

        public static void AbstractListRowBindingHandler(object sender, GridViewRowEventArgs e)
        {
            AbstractListRow item = e.Row.DataItem as AbstractListRow;
            Panel TitleWrapper = e.Row.FindControl("TitleWrapper") as Panel;
            Image AbstractScanClip = e.Row.FindControl("AbstractScanClip") as Image;
            HyperLink AbstractTitleLink = e.Row.FindControl("AbstractTitleLink") as HyperLink;
            Label AbstractTitleText = e.Row.FindControl("AbstractTitleText") as Label;

            if (item != null && AbstractTitleLink != null && AbstractTitleText != null)
            {
                if (item.IsParent)
                {
                    AbstractTitleLink.NavigateUrl = "~/Evaluation/ViewAbstract.aspx?AbstractID=" + item.AbstractID;
                    AbstractTitleLink.Text = item.ProjectTitle;
                    AbstractTitleLink.Visible = true;
                }
                else
                {
                    AbstractTitleText.Text = item.ProjectTitle;
                    AbstractTitleText.Visible = true;
                }
            }

            if (item != null && TitleWrapper != null && AbstractScanClip != null)
            {
                if (!string.IsNullOrEmpty(item.AbstractScan))
                {
                    TitleWrapper.CssClass += " has-file";

                    AbstractScanClip.Visible = true;
                }
            }

            CheckBox Review = e.Row.FindControl("Review") as CheckBox;
            CheckBox ToReview = e.Row.FindControl("ToReview") as CheckBox;

            // checkbox for remove from review list
            if (item != null && Review != null)
            {
                Review.Visible = item.IsParent;
            }

            // checkbox for add to review list
            if (item != null && ToReview != null)
            {
                AbstractListViewData data = new AbstractListViewData();
                if (data.IsAbstractInReview(item.AbstractID))
                {
                    ToReview.Visible = false;
                }
                else
                {
                    ToReview.Visible = item.IsParent;
                }
            }
        }
    }
}
