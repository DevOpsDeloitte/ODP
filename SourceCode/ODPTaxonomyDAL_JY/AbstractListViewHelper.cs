using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.Security;
using ODPTaxonomyUtility_TT;

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
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, "IQS " + iden.UserName + " vs. ODP", ParentAbstracts[i].AbstractID);
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

            // retrieve evaluations first and use to get submissions without a join.
            List<int> RelevantAbstractIDs = ParentAbstracts.Select(a => a.AbstractID).ToList<int>();
            var cacheEvaluations = data.getAllEvaluationRecords(RelevantAbstractIDs);
            List<int> RelevantEvaluationIDs = cacheEvaluations.Select(e => e.EvaluationId).ToList();
            var cacheSubmissions = data.getAllSubmissionRecords(RelevantEvaluationIDs);
            List<int> RelevantSubmissionIDs = cacheSubmissions.Select(e => e.SubmissionID).ToList();
            IEnumerable<KappaData_B> cacheKappaDataB = null;
            IEnumerable<KappaData> cacheKappaData = null;
            cacheKappaData = data.getAllKappaRecordsK1K2Only(RelevantAbstractIDs);
            cacheKappaDataB = data.getAllKappaRecordsBK1K2Only(RelevantAbstractIDs);

            var cacheF_PreventionCategoryAnswers = data.getAllF_PreventionCategoryRecordsID6(RelevantSubmissionIDs);
            var cacheE_StudyDesignPurposeAnswers = data.getAllE_StudyDesignPurposeRecordsID7(RelevantSubmissionIDs);
            var cacheF_PreventionCategoryAnswer_Bs = data.getAllF_PreventionCategoryRecord_BsID6();
            var cacheE_StudyDesignPurposeAnswer_Bs = data.getAllE_StudyDesignPurposeRecord_BsID7();

            for (int i = 0; i < ParentAbstracts.Count; i++)
            {

                ParentAbstracts[i].GetSubmissionData2(SubmissionTypeEnum.CODER_CONSENSUS, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                ParentAbstracts[i].GetAbstractScan(AbstractView);
                ParentAbstracts[i].ChildRows = new List<AbstractListRow>();
                IEnumerable<KappaData> KappaData = null;
                IEnumerable<KappaData_B> KappaDataB = null;
                //KappaData = data.GetAbstractKappaData2(ParentAbstracts[i].AbstractID, cacheKappaData);
                var KappaCount = 0;
                if (cacheKappaData != null && ParentAbstracts[i].CodingType == null)
                {
                    KappaData = data.GetAbstractKappaData2(ParentAbstracts[i].AbstractID, cacheKappaData);
                    if (KappaData.Count() > 0)
                    {
                        KappaCount = KappaData.Count();

                    }
                }
                if (cacheKappaDataB != null && ParentAbstracts[i].CodingType != null)
                {
                    KappaDataB = data.GetAbstractKappaData2B(ParentAbstracts[i].AbstractID, cacheKappaDataB);
                    if (KappaDataB.Count() > 0)
                    {
                        KappaCount = KappaDataB.Count();

                    }
                }

                if (KappaCount > 0)
                {
                    // fill in k1 value
                    if (ParentAbstracts[i].CodingType == null)
                    {
                        Abstracts.Add(FillInKappaValue(ParentAbstracts[i], KappaData, KappaTypeEnum.K1));
                    }
                    else
                    {
                        Abstracts.Add(FillInKappaValueB(ParentAbstracts[i], KappaDataB, KappaTypeEnum.K1));
                    }
                    ParentAbstracts[i].KappaCount = KappaCount;
                    #region NoEntry
                    //if (1 == 2) // Emulating no Entry here.
                    //{

                    //    if (AbstractView == AbstractViewRole.CoderSupervisor || AbstractView == AbstractViewRole.ODPSupervisor)
                    //    {
                    //        // get coder evaluation row
                    //        // and fill in k2 - k4 value
                    //        var CoderEvaluations = data.GetCoderEvaluations(ParentAbstracts[i].AbstractID);
                    //        if (CoderEvaluations != null && CoderEvaluations.TeamID != null)
                    //        {
                    //            var CoderKapperIdentities = data.GetKappaIdentities(CoderEvaluations.TeamID.Value);
                    //            if (CoderKapperIdentities.Count() > 0)
                    //            {
                    //                foreach (var iden in CoderKapperIdentities)
                    //                {
                    //                    foreach (var kappa in KappaData)
                    //                    {
                    //                        if (iden.UserAlias == "CdrA" && kappa.KappaTypeID == (int)KappaTypeEnum.K2)
                    //                        {
                    //                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                    //                            CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                    //                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                    //                            //Abstracts.Add(CoderEvaluation);
                    //                        }
                    //                        else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K3)
                    //                        {
                    //                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                    //                            CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                    //                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                    //                            //Abstracts.Add(CoderEvaluation);
                    //                        }
                    //                        else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K4)
                    //                        {
                    //                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                    //                            CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                    //                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                    //                            //Abstracts.Add(CoderEvaluation);
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }

                    //    // fill in k5 value
                    //    foreach (var kappa in KappaData)
                    //    {
                    //        if (kappa.KappaTypeID == (int)KappaTypeEnum.K5)
                    //        {
                    //            AbstractListRow ODPConsensus = ConstructNewAbstractListRow(kappa, "ODP Avg", ParentAbstracts[i].AbstractID);
                    //            ODPConsensus.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_CONSENSUS, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs);
                    //        ParentAbstracts[i].ChildRows.Add(ODPConsensus);
                    //            //Abstracts.Add(ODPConsensus);
                    //        }
                    //    }

                    //    if (AbstractView == AbstractViewRole.ODPSupervisor)
                    //    {
                    //        // get odp staff evaluation row
                    //        // and fill in k6 - k8 value
                    //        var ODPEvaluations = data.GetODPEvaluations(ParentAbstracts[i].AbstractID);
                    //        if (ODPEvaluations != null && ODPEvaluations.TeamID != null)
                    //        {
                    //            var ODPCoderKapperIdentities = data.GetKappaIdentities(ODPEvaluations.TeamID.Value);
                    //            if (ODPCoderKapperIdentities.Count() > 0)
                    //            {
                    //                foreach (var iden in ODPCoderKapperIdentities)
                    //                {
                    //                    foreach (var kappa in KappaData)
                    //                    {
                    //                        if (iden.UserAlias == "ODPA" && kappa.KappaTypeID == (int)KappaTypeEnum.K6)
                    //                        {
                    //                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                    //                            ODPEvaluation.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                    //                            ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                    //                            //Abstracts.Add(ODPEvaluation);
                    //                        }
                    //                        else if (iden.UserAlias == "ODPB" && kappa.KappaTypeID == (int)KappaTypeEnum.K7)
                    //                        {
                    //                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                    //                            ODPEvaluation.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                    //                            ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                    //                            //Abstracts.Add(ODPEvaluation);
                    //                        }
                    //                        else if (iden.UserAlias == "ODPC" && kappa.KappaTypeID == (int)KappaTypeEnum.K8)
                    //                        {
                    //                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                    //                            ODPEvaluation.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                    //                            ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                    //                            //Abstracts.Add(ODPEvaluation);
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }

                    //    // fill in k9 value
                    //    foreach (var kappa in KappaData)
                    //    {
                    //        if (kappa.KappaTypeID == (int)KappaTypeEnum.K9)
                    //        {
                    //            AbstractListRow ODPCoderComparison = ConstructNewAbstractListRow(kappa, contractorName + " " + "vs R", ParentAbstracts[i].AbstractID);
                    //            ODPCoderComparison.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_COMPARISON, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs);
                    //            ParentAbstracts[i].ChildRows.Add(ODPCoderComparison);
                    //            //Abstracts.Add(ODPCoderComparison);
                    //        }
                    //    }

                    //    /* add k10-k12 */
                    //    if (AbstractView == AbstractViewRole.ODPSupervisor)
                    //    {
                    //        // get coder evaluation row
                    //        // and fill in k10 - k12 value
                    //        var CoderEvaluations = data.GetCoderEvaluations(ParentAbstracts[i].AbstractID);
                    //        if (CoderEvaluations != null && CoderEvaluations.TeamID != null)
                    //        {
                    //            var CoderKapperIdentities = data.GetKappaIdentities(CoderEvaluations.TeamID.Value);
                    //            if (CoderKapperIdentities.Count() > 0)
                    //            {
                    //                foreach (var iden in CoderKapperIdentities)
                    //                {
                    //                    foreach (var kappa in KappaData)
                    //                    {
                    //                        if (iden.UserAlias == "CdrA" && kappa.KappaTypeID == (int)KappaTypeEnum.K10)
                    //                        {
                    //                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                    //                            CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                    //                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                    //                            //Abstracts.Add(CoderEvaluation);
                    //                        }
                    //                        else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K11)
                    //                        {
                    //                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                    //                            CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                    //                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                    //                            //Abstracts.Add(CoderEvaluation);
                    //                        }
                    //                        else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K12)
                    //                        {
                    //                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                    //                            CoderEvaluation.GetSubmissionData2(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers);
                    //                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                    //                            //Abstracts.Add(CoderEvaluation);
                    //                        }
                    //                    }
                    //                }
                    //            }
                    //        }
                    //    }

                    //    /* end of add k10-k12 */

                    //    /* add k13 */
                    //    if (AbstractView == AbstractViewRole.ODPSupervisor)
                    //    {
                    //        // fill in k13 value
                    //        foreach (var kappa in KappaData)
                    //        {
                    //            if (kappa.KappaTypeID == (int)KappaTypeEnum.K13)
                    //            {
                    //                AbstractListRow ODPCoderComparison = ConstructNewAbstractListRow(kappa, "ODP" + " " + "vs R", ParentAbstracts[i].AbstractID);
                    //                ODPCoderComparison.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_COMPARISON, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs);
                    //                ParentAbstracts[i].ChildRows.Add(ODPCoderComparison);
                    //                //Abstracts.Add(ODPCoderComparison);
                    //            }
                    //        }
                    //    }
                    //    /* end k13 */

                    //}
                    #endregion
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
            var AbstractId = ParentAbstracts[0].AbstractID;
            var ApplicationId = ParentAbstracts[0].ApplicationID;
            IEnumerable<KappaData_B> cacheKappaDataB = null;
            IEnumerable<KappaData> cacheKappaData = null;

            if (ParentAbstracts[0].CodingType == "Basic")
            {
                cacheKappaDataB = data.getIndividualKappaBRecords(AbstractId);
            }
            else
            {
                cacheKappaData = data.getIndividualKappaRecords(AbstractId);
            }
            var cacheEvaluations = data.getIndividualEvaluationRecords(AbstractId);
            var EvaluationIds = cacheEvaluations.Select(s => s.EvaluationId).ToList();
            var SubmissionIds = data.getSubmissionIds(EvaluationIds);
            var cacheSubmissions = data.getIndividualSubmissionRecords(EvaluationIds);
            var cacheF_PreventionCategoryAnswers = data.getIndividualF_PreventionCategoryRecords(SubmissionIds);
            var cacheE_StudyDesignPurposeAnswers = data.getIndividualE_StudyDesignPurposeRecords(SubmissionIds);
            var cacheF_PreventionCategoryAnswer_Bs = data.getIndividualF_PreventionCategoryRecord_Bs(SubmissionIds);
            var cacheE_StudyDesignPurposeAnswer_Bs = data.getIndividualE_StudyDesignPurposeRecord_Bs(SubmissionIds);

            for (int i = 0; i < ParentAbstracts.Count; i++)
            {

                ParentAbstracts[i].GetSubmissionData2(SubmissionTypeEnum.CODER_CONSENSUS, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                ParentAbstracts[i].GetAbstractScan(AbstractView);
                ParentAbstracts[i].ChildRows = new List<AbstractListRow>();
                IEnumerable<KappaData> KappaData = null;
                IEnumerable<KappaData_B> KappaDataB = null;
                var KappaCount = 0; var isBasic = false;
                if (cacheKappaData != null)
                {
                    KappaData = data.GetAbstractKappaData2(ParentAbstracts[i].AbstractID, cacheKappaData);
                    if (KappaData.Count() > 0)
                    {
                        KappaCount = KappaData.Count();
                        isBasic = false;
                    }
                }
                if (cacheKappaDataB != null)
                {
                    KappaDataB = data.GetAbstractKappaData2B(ParentAbstracts[i].AbstractID, cacheKappaDataB);
                    if (KappaDataB.Count() > 0)
                    {
                        KappaCount = KappaDataB.Count();
                        isBasic = true;
                    }
                }





                #region Regular Kappa Processing.
                if (KappaCount > 0 && !isBasic)
                {
                    // fill in k1 value
                    Abstracts.Add(FillInKappaValue(ParentAbstracts[i], KappaData, KappaTypeEnum.K1));
                    ParentAbstracts[i].KappaCount = KappaCount; // KappaData.Count();


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
                                            CoderEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K3)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K4)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
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
                            ODPConsensus.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_CONSENSUS, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
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
                                            ODPEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                            ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                            //Abstracts.Add(ODPEvaluation);
                                        }
                                        else if (iden.UserAlias == "ODPB" && kappa.KappaTypeID == (int)KappaTypeEnum.K7)
                                        {
                                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            ODPEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                            ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                            //Abstracts.Add(ODPEvaluation);
                                        }
                                        else if (iden.UserAlias == "ODPC" && kappa.KappaTypeID == (int)KappaTypeEnum.K8)
                                        {
                                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            ODPEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
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
                            ODPCoderComparison.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_COMPARISON, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
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
                                            CoderEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K11)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K12)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
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
                                ODPCoderComparison.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_COMPARISON, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                ParentAbstracts[i].ChildRows.Add(ODPCoderComparison);
                                //Abstracts.Add(ODPCoderComparison);
                            }
                        }
                    }
                    /* end k13 */



                }
                else
                {
                    //ParentAbstracts[i].KappaCount = 0;
                    //Abstracts.Add(ParentAbstracts[i]);
                }

                #endregion


                #region Basic Kappa Processing.
                if (KappaCount > 0 && isBasic)
                {
                    // fill in k1 value
                    Abstracts.Add(FillInKappaValueB(ParentAbstracts[i], KappaDataB, KappaTypeEnum.K1));
                    ParentAbstracts[i].KappaCount = KappaCount; // KappaData.Count();


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
                                    foreach (var kappa in KappaDataB)
                                    {
                                        if (iden.UserAlias == "CdrA" && kappa.KappaTypeID == (int)KappaTypeEnum.K2)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRowB(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K3)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRowB(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K4)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRowB(kappa, contractorName + " " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // fill in k5 value
                    foreach (var kappa in KappaDataB)
                    {
                        if (kappa.KappaTypeID == (int)KappaTypeEnum.K5)
                        {
                            AbstractListRow ODPConsensus = ConstructNewAbstractListRowB(kappa, "ODP Avg", ParentAbstracts[i].AbstractID);
                            ODPConsensus.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_CONSENSUS, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
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
                                    foreach (var kappa in KappaDataB)
                                    {
                                        if (iden.UserAlias == "ODPA" && kappa.KappaTypeID == (int)KappaTypeEnum.K6)
                                        {
                                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRowB(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            ODPEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                            ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                            //Abstracts.Add(ODPEvaluation);
                                        }
                                        else if (iden.UserAlias == "ODPB" && kappa.KappaTypeID == (int)KappaTypeEnum.K7)
                                        {
                                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRowB(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            ODPEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                            ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                            //Abstracts.Add(ODPEvaluation);
                                        }
                                        else if (iden.UserAlias == "ODPC" && kappa.KappaTypeID == (int)KappaTypeEnum.K8)
                                        {
                                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRowB(kappa, "ODP " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            ODPEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                            ParentAbstracts[i].ChildRows.Add(ODPEvaluation);
                                            //Abstracts.Add(ODPEvaluation);
                                        }
                                    }
                                }
                            }
                        }
                    }

                    // fill in k9 value
                    foreach (var kappa in KappaDataB)
                    {
                        if (kappa.KappaTypeID == (int)KappaTypeEnum.K9)
                        {
                            AbstractListRow ODPCoderComparison = ConstructNewAbstractListRowB(kappa, contractorName + " " + "vs R", ParentAbstracts[i].AbstractID);
                            ODPCoderComparison.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_COMPARISON, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
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
                                    foreach (var kappa in KappaDataB)
                                    {
                                        if (iden.UserAlias == "CdrA" && kappa.KappaTypeID == (int)KappaTypeEnum.K10)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRowB(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K11)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRowB(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                            ParentAbstracts[i].ChildRows.Add(CoderEvaluation);
                                            //Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K12)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRowB(kappa, contractorName + " " + iden.UserName + " vs R", ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetSubmissionDataIndividual(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
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
                        foreach (var kappa in KappaDataB)
                        {
                            if (kappa.KappaTypeID == (int)KappaTypeEnum.K13)
                            {
                                AbstractListRow ODPCoderComparison = ConstructNewAbstractListRowB(kappa, "ODP" + " " + "vs R", ParentAbstracts[i].AbstractID);
                                ODPCoderComparison.GetSubmissionData2(SubmissionTypeEnum.ODP_STAFF_COMPARISON, cacheSubmissions, cacheEvaluations, cacheE_StudyDesignPurposeAnswers, cacheF_PreventionCategoryAnswers, cacheE_StudyDesignPurposeAnswer_Bs, cacheF_PreventionCategoryAnswer_Bs, ParentAbstracts[i].ApplicationID);
                                ParentAbstracts[i].ChildRows.Add(ODPCoderComparison);
                                //Abstracts.Add(ODPCoderComparison);
                            }
                        }
                    }
                    /* end k13 */



                }
                else
                {
                    // ParentAbstracts[i].KappaCount = 0;
                    //  Abstracts.Add(ParentAbstracts[i]);
                }
                #endregion


            }

            return Abstracts;
        }

        public static List<AbstractListRow> SortAbstracts(List<AbstractListRow> Abstracts, string Sort, SortDirection SortDirection)
        {
            switch (Sort)
            {
                case "AbstractID":
                    return SortDirection == SortDirection.Ascending ?
                        Abstracts.OrderBy(a => a.AbstractID).ToList() :
                        Abstracts.OrderByDescending(a => a.AbstractID).ToList();
                case "ApplicationID":
                    if (SortDirection == SortDirection.Ascending)
                    {
                        return Abstracts.OrderBy(d => d.ApplicationID, new AppIdComparer<string>(true)).ToList();
                    }
                    else
                    {
                        return Abstracts.OrderBy(d => d.ApplicationID, new AppIdComparer<string>(false)).ToList();
                    }
                case "Title":
                case "ProjectTitle":
                    if (SortDirection == SortDirection.Ascending)
                    {
                        return Abstracts.OrderBy(d => d.ProjectTitle).ToList();
                    }
                    else
                    {
                        return Abstracts.OrderByDescending(d => d.ProjectTitle).ToList();
                    }
                case "Date":
                case "StatusDate":
                    if (SortDirection == SortDirection.Ascending)
                    {
                        return Abstracts.OrderBy(d => d.StatusDate).ToList();
                    }
                    else
                    {
                        return Abstracts.OrderByDescending(d => d.StatusDate).ToList();
                    }
                case "LastExportDate":
                    if (SortDirection == SortDirection.Ascending)
                    {
                        return Abstracts.OrderBy(d => d.LastExportDate).ToList();
                    }
                    else
                    {
                        return Abstracts.OrderByDescending(d => d.LastExportDate).ToList();
                    }
                case "PIProjectLeader":
                    return SortDirection == SortDirection.Ascending ?
                        Abstracts.OrderBy(a => a.PIProjectLeader).ToList() :
                        Abstracts.OrderByDescending(a => a.PIProjectLeader).ToList();

                case "Flags":
                case "A1":
                case "A2":
                case "A3":
                case "A4":
                case "B":
                case "C":
                case "D":
                case "E":
                case "F":
                    AbstractListViewData data = new AbstractListViewData();
                    var KappaData = data.getAllKappaRecordsK1K2Only(null);
                    var KappaDataB = data.getAllKappaRecordsBK1K2Only(null);
                    Abstracts = GetSpecificKappaValue(Abstracts, KappaData.ToList(), KappaDataB.ToList());

                    if (SortDirection == SortDirection.Ascending)
                    {
                        return Abstracts.OrderBy(d => d[Sort]).ToList();
                    }
                    else
                    {
                        return Abstracts.OrderByDescending(d => d[Sort]).ToList();
                    }
                default:
                    return Abstracts.OrderByDescending(d => d.StatusDate).ToList();
            }
        }

        private static List<AbstractListRow> GetSpecificKappaValue(List<AbstractListRow> Abstracts, List<KappaData> KappaData, List<KappaData_B> KappaDataB)
        {
            IEnumerable<KappaData> data = null;
            IEnumerable<KappaData_B> dataB = null;
            List<long> times = new List<long>();

            foreach (var abs in Abstracts)
            {
                data = KappaData
                    .Where(k => k.AbstractID == abs.AbstractID)
                    .OrderBy(k => k.KappaTypeID)
                    .ToList();

                if (data != null && abs.CodingType == null)
                {
                    if (data.Count() > 0)
                    {
                        FillInKappaValue(abs, data, KappaTypeEnum.K1);
                    }
                }

                dataB = KappaDataB
                    .Where(k => k.AbstractID == abs.AbstractID)
                    .OrderBy(k => k.KappaTypeID)
                    .ToList();

                if (dataB != null && abs.CodingType != null)
                {
                    if (dataB.Count() > 0)
                    {
                        FillInKappaValueB(abs, dataB, KappaTypeEnum.K1);
                    }
                }

            }

            return Abstracts;
        }

        public static AbstractListRow ConstructNewAbstractListRow(KappaData Kappa, string Title, int AbstractID = 0)
        {
            return new AbstractListRow
            {
                AbstractID = AbstractID,
                ProjectTitle = Title,
                A1 = convertKappa(Kappa.A1),
                A2 = convertKappa(Kappa.A2),
                A3 = convertKappa(Kappa.A3),
                A4 = "\u2014",
                B = convertKappa(Kappa.B),
                C = convertKappa(Kappa.C),
                D = convertKappa(Kappa.D),
                E = convertKappa(Kappa.E),
                F = convertKappa(Kappa.F)
            };
        }

        public static AbstractListRow ConstructNewAbstractListRowB(KappaData_B Kappa, string Title, int AbstractID = 0)
        {
            return new AbstractListRow
            {
                AbstractID = AbstractID,
                ProjectTitle = Title,
                A1 = "\u2014",
                A2 = "\u2014",
                A3 = "\u2014",
                A4 = convertKappa(Kappa.A4),
                B = convertKappa(Kappa.B),
                C = convertKappa(Kappa.C),
                D = convertKappa(Kappa.D),
                E = convertKappa(Kappa.E),
                F = convertKappa(Kappa.F)
            };
        }

        public static AbstractListRow ConstructNewAbstractListRow(KappaData Kappa, KappaData_B KappaB, string codingType, string Title, int AbstractID = 0)
        {
            if (codingType.ToLower() == "basic")
            {
                return new AbstractListRow
                {
                    AbstractID = AbstractID,
                    ProjectTitle = Title,
                    A1 = "\u2014",
                    A2 = "\u2014",
                    A3 = "\u2014",
                    A4 = convertKappa(KappaB.A4),
                    B = convertKappa(KappaB.B),
                    C = convertKappa(KappaB.C),
                    D = convertKappa(KappaB.D),
                    E = convertKappa(KappaB.E),
                    F = convertKappa(KappaB.F)
                };
            }
            else
            {
                return new AbstractListRow
                {
                    AbstractID = AbstractID,
                    ProjectTitle = Title,
                    A1 = convertKappa(Kappa.A1),
                    A2 = convertKappa(Kappa.A2),
                    A3 = convertKappa(Kappa.A3),
                    A4 = "\u2014",
                    B = convertKappa(Kappa.B),
                    C = convertKappa(Kappa.C),
                    D = convertKappa(Kappa.D),
                    E = convertKappa(Kappa.E),
                    F = convertKappa(Kappa.F)
                };
            }
        }

        public static AbstractListRow FillInKappaValue(AbstractListRow Abstract, IEnumerable<KappaData> KappaData, KappaTypeEnum KappaType)
        {
            foreach (var Kappa in KappaData)
            {
                if (Kappa.KappaTypeID == (int)KappaType)
                {

                    Abstract.A1 = convertKappa(Kappa.A1);
                    Abstract.A2 = convertKappa(Kappa.A2);
                    Abstract.A3 = convertKappa(Kappa.A3);
                    Abstract.A4 = "\u2014";
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

        public static AbstractListRow FillInKappaValueB(AbstractListRow Abstract, IEnumerable<KappaData_B> KappaData, KappaTypeEnum KappaType)
        {
            foreach (var Kappa in KappaData)
            {
                if (Kappa.KappaTypeID == (int)KappaType)
                {

                    Abstract.A1 = "\u2014";
                    Abstract.A2 = "\u2014";
                    Abstract.A3 = "\u2014";
                    Abstract.A4 = convertKappa(Kappa.A4);
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
                if (inVal.HasValue)
                {
                    return inVal.Value.ToString(KappaSpecifier);
                }

                return "\u2014";
            }
            catch
            {
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
