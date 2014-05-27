using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODPTaxonomyDAL_JY
{
    public static class AbstractListViewHelper
    {
        private static string KappaSpecifier = "#.##";

        public static List<AbstractListRow> ProcessAbstracts(List<AbstractListRow> ParentAbstracts, AbstractViewRole AbstractView)
        {
            List<AbstractListRow> Abstracts = new List<AbstractListRow>();
            AbstractListViewData data = new AbstractListViewData();

            for (int i = 0; i < ParentAbstracts.Count; i++)
            {
                ParentAbstracts[i].GetComment();
                ParentAbstracts[i].GetUnableToCode(SubmissionTypeEnum.CODER_CONSENSUS);
                ParentAbstracts[i].GetAbstractScan();

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
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, "Coder: " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetUnableToCode(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
                                            Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrB" && kappa.KappaTypeID == (int)KappaTypeEnum.K3)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, "Coder: " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetUnableToCode(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
                                            Abstracts.Add(CoderEvaluation);
                                        }
                                        else if (iden.UserAlias == "CdrC" && kappa.KappaTypeID == (int)KappaTypeEnum.K4)
                                        {
                                            AbstractListRow CoderEvaluation = ConstructNewAbstractListRow(kappa, "Coder: " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            CoderEvaluation.GetUnableToCode(SubmissionTypeEnum.CODER_EVALUATION, iden.UserId);
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
                            AbstractListRow ODPConsensus = ConstructNewAbstractListRow(kappa, "ODP Staff", ParentAbstracts[i].AbstractID);
                            ODPConsensus.GetUnableToCode(SubmissionTypeEnum.ODP_STAFF_CONSENSUS);
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
                                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP Coder: " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            ODPEvaluation.GetUnableToCode(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId);
                                            Abstracts.Add(ODPEvaluation);
                                        }
                                        else if (iden.UserAlias == "ODPB" && kappa.KappaTypeID == (int)KappaTypeEnum.K7)
                                        {
                                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP Coder: " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            ODPEvaluation.GetUnableToCode(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId);
                                            Abstracts.Add(ODPEvaluation);
                                        }
                                        else if (iden.UserAlias == "ODPC" && kappa.KappaTypeID == (int)KappaTypeEnum.K8)
                                        {
                                            AbstractListRow ODPEvaluation = ConstructNewAbstractListRow(kappa, "ODP Coder: " + iden.UserName, ParentAbstracts[i].AbstractID);
                                            ODPEvaluation.GetUnableToCode(SubmissionTypeEnum.ODP_STAFF_EVALUATION, iden.UserId);
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
                            AbstractListRow ODPCoderComparison = ConstructNewAbstractListRow(kappa, "ODP vs. Coder", ParentAbstracts[i].AbstractID);
                            ODPCoderComparison.GetUnableToCode(SubmissionTypeEnum.ODP_STAFF_COMPARISON);
                            Abstracts.Add(ODPCoderComparison);
                        }
                    }
                }
                else
                {
                    Abstracts.Add(ParentAbstracts[i]);
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
                A1 = ((decimal)(Kappa.A1)).ToString(KappaSpecifier),
                A2 = ((decimal)(Kappa.A2)).ToString(KappaSpecifier),
                A3 = ((decimal)(Kappa.A3)).ToString(KappaSpecifier),
                B = ((decimal)(Kappa.B)).ToString(KappaSpecifier),
                C = ((decimal)(Kappa.C)).ToString(KappaSpecifier),
                D = ((decimal)(Kappa.D)).ToString(KappaSpecifier),
                E = ((decimal)(Kappa.E)).ToString(KappaSpecifier),
                F = ((decimal)(Kappa.F)).ToString(KappaSpecifier)
            };
        }

        public static AbstractListRow FillInKappaValue(AbstractListRow Abstract, IEnumerable<KappaData> KappaData, KappaTypeEnum KappaType)
        {
            foreach (var Kappa in KappaData)
            {
                if (Kappa.KappaTypeID == (int)KappaType)
                {
                    Abstract.A1 = ((decimal)(Kappa.A1)).ToString(KappaSpecifier);
                    Abstract.A2 = ((decimal)(Kappa.A2)).ToString(KappaSpecifier);
                    Abstract.A3 = ((decimal)(Kappa.A3)).ToString(KappaSpecifier);
                    Abstract.B = ((decimal)(Kappa.B)).ToString(KappaSpecifier);
                    Abstract.C = ((decimal)(Kappa.C)).ToString(KappaSpecifier);
                    Abstract.D = ((decimal)(Kappa.D)).ToString(KappaSpecifier);
                    Abstract.E = ((decimal)(Kappa.E)).ToString(KappaSpecifier);
                    Abstract.F = ((decimal)(Kappa.F)).ToString(KappaSpecifier);

                    break;
                }
            }

            return Abstract;
        }
    }
}
