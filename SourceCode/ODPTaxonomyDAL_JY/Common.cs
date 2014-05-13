using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODPTaxonomyDAL_JY
{
    public enum AbstractStatusEnum
    {
        OPEN_0 = 1,
        RETRIEVED_FOR_CODING_1 = 2,
        CODED_BY_CODER_1A = 3,
        CONSENSUS_COMPLETE_1B = 4,
        CONSENSUS_COMPLETE_WITH_NOTES_1N = 6,
        RETRIEVED_FOR_ODP_CODING_2 = 7,
        CODED_BY_ODP_STAFF_2A = 8,
        ODP_STAFF_CONSENSUS_2B = 9,
        ODP_STAFF_AND_CODER_CONSENSUS_2C = 10,
        ODP_CONSENSUS_WITH_NOTES_2N = 12
    }

    public enum SubmissionTypeEnum
    {
        CODER_EVALUATION = 1,
        CODER_CONSENSUS = 2,
        ODP_STAFF_EVALUATION = 3,
        ODP_STAFF_CONSENSUS = 4,
        ODP_STAFF_COMPARISON = 5
    }

    public enum EvaluationTypeEnum
    {
        CODER_EVALUATION = 1,
        ODP_EVALUATION = 2
    }

    public enum KappaTypeEnum
    {
        CODER_COMPARISON_K1 = 1,
        CODER_A_VS_CONSENSUS_K2 = 2,
        CODER_B_VS_CONSENSUS_K3 = 3,
        CODER_C_VS_CONSENSUS_K4 = 4,
        ODP_STAFF_COMPARISON_K5 = 5,
        ODP_STAFF_A_VS_CONSENSUS_K6 = 6,
        ODP_STAFF_B_VS_CONSENSUS_K7 = 7,
        ODP_STAFF_C_VS_CONSENSUS_K8 = 8,
        CODER_CONSENSUS_VS_ODP_CONSENSUS_K9 = 9
    }

    public enum TeamTypeEnum
    {
        Coder = 1,
        ODP_STAFF = 2
    }
}
