using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODPTaxonomyDAL_JY
{
    public enum AbstractViewRole
    {
        Admin,
        ODPSupervisor,
        ODPStaff,
        CoderSupervisor
    }

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
        ODP_CONSENSUS_WITH_NOTES_2N = 12,
        CLOSED_3 = 13,
        DATA_EXPORTED_4 = 14,
        REOPEN_FOR_REVIEW_BY_ODP = 15
    }

    public enum SubmissionTypeEnum
    {
        NA = 0,
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
        K1 = 1,
        K2 = 2,
        K3 = 3,
        K4 = 4,
        K5 = 5,
        K6 = 6,
        K7 = 7,
        K8 = 8,
        K9 = 9
    }

    public enum TeamTypeEnum
    {
        Coder = 1,
        ODP_STAFF = 2
    }
}
