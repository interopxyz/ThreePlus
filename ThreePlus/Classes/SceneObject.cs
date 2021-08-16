using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThreePlus
{
    public abstract class SceneObject
    {

        #region members

        protected Guid uuid = new Guid();

        protected string title = string.Empty;
        protected string type = string.Empty;

        #endregion

        #region constructors

        public SceneObject()
        {
            this.uuid = Guid.NewGuid();
        }

        public SceneObject(string type, string title)
        {
            this.uuid = Guid.NewGuid();
        }

        public SceneObject(SceneObject sceneObject)
        {
            this.uuid = new Guid(sceneObject.uuid.ToString());
            this.title = sceneObject.title;
            this.type = sceneObject.type;

        }

        #endregion

        #region properties

        public virtual Guid Uuid
        {
            get { return this.uuid; }
        }

        public virtual string Type
        {
            get { return this.type; }
        }

        public virtual string Title
        {
            get { return this.title; }
        }

        #endregion

        #region methods



        #endregion

        #region overrides

        public override string ToString()
        {
            return "Scene Object | "+this.type;
        }

        #endregion

    }
}
