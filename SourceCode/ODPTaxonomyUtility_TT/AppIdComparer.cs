using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace ODPTaxonomyUtility_TT
{
    public class AppIdComparer<T> : IComparer<string>
    {
        private bool isAscending;
        private char[] separator = new char[] { '_' };

        public AppIdComparer(bool inAscendingOrder = true)
        {
            this.isAscending = inAscendingOrder;
        }

        int IComparer<string>.Compare(string x, string y)
        {
            int returnVal = 0;

            if (x == y)
                return 0;

            // no B abstracts, so just compare them like numbers
            if (!x.Contains("_") && !y.Contains("_"))
            {
                returnVal = CompareIntId(x, y);
            }
            else
            {
                var xParts = x.Split(separator).ToList();
                var yParts = y.Split(separator).ToList();

                if (xParts.Count == 1) xParts.Add(string.Empty);
                if (yParts.Count == 1) yParts.Add(string.Empty);

                if (xParts[0] == yParts[0])
                {
                    return xParts[1].CompareTo(yParts[1]);
                }
                else
                {
                    returnVal = CompareIntId(xParts[0], yParts[0]);
                }
            }

            return isAscending ? returnVal : -returnVal;
        }

        int CompareIntId(string x, string y)
        {
            int xInt = 0, yInt = 0;

            if (Int32.TryParse(x, out xInt) && Int32.TryParse(y, out yInt))
            {
                return xInt.CompareTo(yInt);
            }
            else
            {
                return x.CompareTo(y);
            }
        }
    }
}
