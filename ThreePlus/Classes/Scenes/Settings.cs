using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreePlus
{
    public class Settings : SceneObject
    {

        #region members

        bool shadows = true;
        int shadowType = 1;
        bool vr = false;
        bool physicalLights = false;
        int toneMapping = 0;
        int toneMappingExposure = 1;

        #endregion

        #region constructors

        public Settings()
        {
            this.title = "project";
        }

        #endregion

        #region properties

        public virtual bool Shadows
        {
            get { return shadows; }
        }

        public virtual int ShadowType
        {
            get { return shadowType; }
        }

        public virtual bool VR
        {
            get { return vr; }
        }

        public virtual bool PhysicalLights
        {
            get { return physicalLights; }
        }

        public virtual int ToneMapping
        {
            get { return toneMapping; }
        }

        public virtual int ToneMappingExposure
        {
            get { return toneMappingExposure; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Camera | ";
        }

        #endregion

    }
}
