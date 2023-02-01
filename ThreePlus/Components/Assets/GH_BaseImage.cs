using Grasshopper.GUI.Canvas;
using Grasshopper.Kernel;
using Grasshopper.Kernel.Attributes;
using Rhino.Geometry;
using System;
using System.Collections.Generic;

using Sd = System.Drawing;

namespace ThreePlus.Components.Assets
{
    public abstract class GH_BaseImage : GH_Component
    {
        public Sd.Image img = null;
        int width = 100;
        int height = 100;

        /// <summary>
        /// Initializes a new instance of the GH_BaseImage class.
        /// </summary>
        public GH_BaseImage()
          : base("GH_BaseImage", "Nickname",
              "Description",
              "Category", "Subcategory")
        {
        }

        public GH_BaseImage(string Name, string NickName, string Description, string Category, string Subcategory) : base(Name, NickName, Description, Category, Subcategory)
        {
        }

        /// <summary>
        /// Registers all the input parameters for this component.
        /// </summary>
        protected override void RegisterInputParams(GH_Component.GH_InputParamManager pManager)
        {
        }

        /// <summary>
        /// Registers all the output parameters for this component.
        /// </summary>
        protected override void RegisterOutputParams(GH_Component.GH_OutputParamManager pManager)
        {
        }

        /// <summary>
        /// This is the method that actually does the work.
        /// </summary>
        /// <param name="DA">The DA object is used to retrieve from inputs and store in outputs.</param>
        protected override void SolveInstance(IGH_DataAccess DA)
        {
        }

        /// <summary>
        /// Provides an Icon for the component.
        /// </summary>
        protected override System.Drawing.Bitmap Icon
        {
            get
            {
                //You can add image files to your project resources and access them like this:
                // return Resources.IconForThisComponent;
                return null;
            }
        }

        /// <summary>
        /// Gets the unique ID for this component. Do not change this ID after release.
        /// </summary>
        public override Guid ComponentGuid
        {
            get { return new Guid("a9fda59c-c675-4d4b-aa69-28e1f13a36ca"); }
        }

        public class Attributes_Custom : GH_ComponentAttributes
        {
            public Attributes_Custom(GH_Component owner) : base(owner) { }

            private Sd.Rectangle ButtonBounds { get; set; }
            protected override void Layout()
            {
                base.Layout();
                GH_BaseImage comp = Owner as GH_BaseImage;

                Sd.Rectangle rec0 = GH_Convert.ToRectangle(Bounds);

                comp.width = rec0.Width; ;
                comp.height = (int)Math.Floor(comp.width * ((double)comp.img.Height) / ((double)comp.img.Width));

                rec0.Width = comp.width;
                rec0.Height += comp.height;

                Sd.Rectangle rec1 = rec0;
                rec1.Y = rec1.Bottom - comp.height;
                rec1.Height = comp.height;
                rec1.Width = comp.width;

                Bounds = rec0;
                ButtonBounds = rec1;

            }

            protected override void Render(GH_Canvas canvas, Sd.Graphics graphics, GH_CanvasChannel channel)
            {
                base.Render(canvas, graphics, channel);
                GH_BaseImage comp = Owner as GH_BaseImage;

                if (channel == GH_CanvasChannel.Objects)
                {
                    GH_Capsule capsule = GH_Capsule.CreateCapsule(ButtonBounds, GH_Palette.Normal, 0, 0);
                    capsule.Render(graphics, Selected, Owner.Locked, true);
                    capsule.AddOutputGrip(this.OutputGrip.Y);
                    capsule.Dispose();
                    capsule = null;

                    Sd.StringFormat format = new Sd.StringFormat();
                    format.Alignment = Sd.StringAlignment.Center;
                    format.LineAlignment = Sd.StringAlignment.Center;

                    Sd.RectangleF textRectangle = ButtonBounds;

                    graphics.DrawImage(comp.img, Bounds.X + 2, m_innerBounds.Y - (ButtonBounds.Height - Bounds.Height), comp.width - 4, comp.height - 4);

                    format.Dispose();
                }
            }
        }

    }
}