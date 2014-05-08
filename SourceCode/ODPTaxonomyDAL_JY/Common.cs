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
        CODER_CONSENSUS = 2
    }

    public enum EvaluationTypeEnum
    {
        CODER_EVALUATION = 1,
        ODP_EVALUATION = 2
    }
}
