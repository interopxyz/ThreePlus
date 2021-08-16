using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreePlus
{
    public class Constants
    {

        #region naming

        public static string LongName
        {
            get { return ShortName + " v" + Major + "." + Minor; }
        }

        public static string ShortName
        {
            get { return "ThreeJs"; }
        }

        private static string Minor
        {
            get { return typeof(Constants).Assembly.GetName().Version.Minor.ToString(); }
        }
        private static string Major
        {
            get { return typeof(Constants).Assembly.GetName().Version.Major.ToString(); }
        }

        #endregion

    }
}
