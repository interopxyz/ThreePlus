using Grasshopper;
using Grasshopper.Kernel;
using System;
using System.Drawing;

namespace ThreePlus
{
    public class ThreePlusInfo : GH_AssemblyInfo
    {
        public override string Name
        {
            get
            {
                return "ThreePlus";
            }
        }
        public override Bitmap Icon
        {
            get
            {
                //Return a 24x24 pixel bitmap to represent this GHA library.
                return Properties.Resources.ThreePlus_24;
            }
        }
        public override string Description
        {
            get
            {
                //Return a short string describing the purpose of this GHA library.
                return "A Three JS creation plugin for Grasshopper 3d";
            }
        }
        public override Guid Id
        {
            get
            {
                return new Guid("f5f5bd94-6265-4864-94c1-68fd1b25ce85");
            }
        }

        public override string AuthorName
        {
            get
            {
                //Return a string identifying you or your company.
                return "David Mans";
            }
        }
        public override string AuthorContact
        {
            get
            {
                //Return a string representing your preferred contact details.
                return "interopxyz@gmail.com";
            }
        }
    }

    public class BitmapPlusCategoryIcon : GH_AssemblyPriority
    {
        public object Properties { get; private set; }

        public override GH_LoadingInstruction PriorityLoad()
        {
            Instances.ComponentServer.AddCategoryIcon(Constants.ShortName, ThreePlus.Properties.Resources.ThreePlus_16);
            Instances.ComponentServer.AddCategorySymbolName(Constants.ShortName, '3');
            return GH_LoadingInstruction.Proceed;
        }
    }
}