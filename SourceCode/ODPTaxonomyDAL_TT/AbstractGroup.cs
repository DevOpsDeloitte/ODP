using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ODPTaxonomyDAL_TT
{
    public class AbstractGroup
    {
        private int _CategoryID;
        private int _Sorting;

        public AbstractGroup(int p_CategoryID, int p_Sorting)
        {
            this._CategoryID = p_CategoryID;
            this._Sorting = p_Sorting;
            
        }

        public int CategoryID
        {
            get { return _CategoryID; }
            set { _CategoryID = value; }
        }

        public int Sorting
        {
            get { return _Sorting; }
            set { _Sorting = value; }
        }
    }
}
