using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;
using Rg = Rhino.Geometry;

namespace ThreePlus
{
    public static class ObjToJson
    {

        #region conversions

        private static string StartProject(string type, double version, string generator)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("\"metadata\": {");
            output.AppendLine(SetItem("version",version));
            output.AppendLine(SetItem("type", type) );
            output.AppendLine(SetItem("generator", generator,false));
            output.AppendLine("},");

            return output.ToString();
        }

        private static string OpenObject(string title)
        {
            return "\"" + title + "\": {";
        }

        private static string CloseObject(bool comma = true)
        {
            string txt = "}";
            if (comma) txt += ",";
            return txt;
        }

        private static string OpenArray(string title)
        {
            return "\"" + title + "\": [";
        }

        private static string SetItem(string title, double[] value, bool comma = true)
        {
            string txt = "\"" + title + "\": [" + String.Join(",",value) + "] ";
            if (comma) txt += ", ";
            return txt;
        }

        private static string SetItem(string title, float[] value, bool comma = true)
        {
            string txt = "\"" + title + "\": [" + String.Join(",", value) + "] ";
            if (comma) txt += ", ";
            return txt;
        }

        private static string SetItem(string title, string value, bool comma = true)
        {
            string txt = "\"" + title + "\": \"" + value + "\" ";
            if (comma) txt += ", ";
            return txt;
        }

        private static string SetItem(string title, bool value, bool comma = true)
        {
            string txt = "\"" + title + "\": " + value.ToString().ToLower() + " ";
            if (comma) txt += ", ";
            return txt;
        }

        private static string SetItem(string title, int value, bool comma = true)
        {
            string txt = "\"" + title + "\": " + value.ToString() + " ";
            if (comma) txt += ", ";
            return txt;
        }

        private static string SetItem(string title, Guid value, bool comma = true)
        {
            string txt = "\"" + title + "\": \"" + value.ToString().ToUpper() + "\" ";
            if (comma) txt += ", ";
            return txt;
        }

        private static string SetItem(string title, double value, bool comma = true)
        {
            string txt = "\"" + title + "\": " + value.ToString() + " ";
            if (comma) txt += ", ";
            return txt;
        }

        private static string SetItem(string title, Sd.Color value, bool comma = true, bool isHex = true)
        {
            string txt = "\"" + title + "\": " + Sd.ColorTranslator.ToOle(value).ToString() + " ";
            if (isHex) txt = "\"" + title + "\": \"" + value.ToHex() + "\" ";
            if (comma) txt += ", ";
            return txt;
        }

        private static string SetMatrixItem(string title, Light input, bool absolute = true, int digits = 5, bool comma = true)
        {
            string matrix = string.Empty;
            if (absolute)
            {
                matrix = "[1,0,0,0,0,1,0,0,0,0,1,0," + Math.Round(-input.Position.X, digits) + ","
                    + Math.Round(-input.Position.Z, digits) + ","
                    + Math.Round(input.Position.Y, digits) + ",1]";
            }
            else
            {
                matrix = "[1,0,0,0,0,1,0,0,0,0,1,0," + Math.Round(-(input.Position.X - input.Target.X), digits) + ","
                    + Math.Round(input.Position.Z - input.Target.Z, digits) + ","
                    + Math.Round(input.Position.Y - input.Target.Y, digits) + ",1]";
            }

            string txt = "\"" + title + "\": " + matrix + " ";
            if (comma) txt += ", ";
            return txt;
        }

        private static string SetItem(string title, Rg.Transform value, bool comma = true)
        {
            int digits = 5;
            float[] t = value.ToFloatArray(false);

            string txt = "\"" + title + "\": ["
                + Math.Round(t[1], digits) + ", "
                + Math.Round(t[2], digits) + ", "
                + Math.Round(t[0], digits) + ", 0, "

                + Math.Round(t[9], digits) + ", "
                + Math.Round(t[10], digits) + ", "
                + Math.Round(t[8], digits) + ", 0, "

                + Math.Round(t[5], digits) + ", "
                + Math.Round(t[6], digits) + ", "
                + Math.Round(t[4], digits) + ", 0, "

                + Math.Round(t[13], digits) + ", "
                + Math.Round(t[14], digits) + ", "
                + Math.Round(t[12], digits) + ", 1"
                + "] ";
            if (comma) txt += ", ";
            return txt;
        }

        private static string SetItem2(string title, Rg.Transform value, bool comma = true)
        {
            int digits = 5;
            float[] t = value.ToFloatArray(false);

            string txt = "\"" + title + "\": ["
                + Math.Round(t[1], digits) + ", "
                + Math.Round(t[2], digits) + ", "
                + Math.Round(t[0], digits) + ", 0, "

                + Math.Round(t[5], digits) + ", "
                + Math.Round(t[6], digits) + ", "
                + Math.Round(t[4], digits) + ", 0, "

                + Math.Round(t[9], digits) + ", "
                + Math.Round(t[10], digits) + ", "
                + Math.Round(t[8], digits) + ", 0, "

                + Math.Round(t[13], digits) + ", "
                + Math.Round(t[14], digits) + ", "
                + Math.Round(t[12], digits) + ", 1"
                + "] ";
            if (comma) txt += ", ";
            return txt;
        }

        #endregion

        #region scene

        public static string ToJson(this Scene input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("{");

            output.Append(StartProject("Object",4.3,"ThreePlus"));
            output.Append(input.Settings.ToJson());

            output.AppendLine(input.Models.ToJsonGeometries());
            output.Append(input.Models.ToJsonMaterials());

            output.AppendLine(OpenObject("object"));
            output.AppendLine(input.ToJsonObjects());

            output.AppendLine(input.Scripts.ToJson());

            output.AppendLine("}");

            return output.ToString();
        }

        private static string ToJsonObjects(this Scene input)
        {
            StringBuilder output = new StringBuilder();

            Rg.BoundingBox bbox = input.Models.GetBoundary();
            for (int i =0;i<input.Cameras.Count;i++)
            {
                if (input.Cameras[i].IsDefault)
                {
                    input.Cameras[i].Position = bbox.Max + bbox.Diagonal;
                    input.Cameras[i].Target = bbox.Center;
                }
            }

            output.Append("\t"+((MetaData)input).ToJsonObjectMeta());

            output.AppendLine(SetItem("background", input.Environment.Background));

            output.AppendLine(OpenArray("children"));
            output.Append(input.Cameras.ToJsonObjects());
            output.AppendLine(input.Lights.ToJsonObjects(input.Camera));
            output.Append(input.Models.ToJsonObjects());

            output.Append("]},");

            return output.ToString();
        }

        #endregion

        #region models

        private static string ToJson(this Model input, bool comma = true)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("{");

            output.AppendLine(SetItem("uuid", input.GeoId));
            output.AppendLine(SetItem("type", input.Type));

            if (input.IsMesh)
            {
                output.Append(input.Mesh.ToJson());
            }
            else if(input.IsCloud)
            {
                output.Append(input.Cloud.ToJson());
            }
            else if(input.IsCurve)
            {
                output.Append(input.Curve.ToJson(input.Graphic));
            }
            output.AppendLine(CloseObject(comma));

            return output.ToString();
        }

        #endregion

        #region materials

        private static string ToJsonMaterial(this Model input, bool comma = true)
        {
            StringBuilder output = new StringBuilder();
            Material material = new Material(input.Material);
            Graphic graphic = new Graphic(input.Graphic);

            output.AppendLine("{");
                output.AppendLine(SetItem("uuid", material.Uuid));

            if (input.IsMesh) 
            {

            if (material.IsDefault)
            {
                output.AppendLine(SetItem("type", "MeshBasicMaterial"));
                output.AppendLine(SetItem("vertexColors", true));
                output.AppendLine(SetItem("side", 2));
                output.AppendLine(SetItem("color", Sd.Color.White, false, false));
            }
            else
            {
                output.AppendLine(SetItem("type", material.Type));
                if (material.IsWireframe) output.AppendLine(SetItem("wireframe",true));
                switch (material.MaterialType)
                {
                    case Material.Types.Basic:
                    case Material.Types.Lambert:
                    case Material.Types.Phong:
                    case Material.Types.Standard:
                    case Material.Types.Physical:
                        output.AppendLine(SetItem("transparent", material.Transparent));
                        output.AppendLine(SetItem("opacity", material.Opacity));
                        break;
                }

                switch (material.MaterialType)
                {
                    case Material.Types.Lambert:
                    case Material.Types.Phong:
                    case Material.Types.Standard:
                    case Material.Types.Physical:
                        output.AppendLine(SetItem("emissive", material.EmissiveColor));
                        output.AppendLine(SetItem("emissiveIntensity", material.EmissiveIntensity));
                        break;
                }

                switch (material.MaterialType)
                {
                    case Material.Types.Basic:
                        break;
                    case Material.Types.Lambert:
                        break;
                    case Material.Types.Phong:
                        output.AppendLine(SetItem("shininess", material.Shininess * 100.0));
                        break;
                    case Material.Types.Standard:
                        output.AppendLine(SetItem("roughness", material.Roughness));
                        output.AppendLine(SetItem("metalness", material.Metalness));
                        break;
                    case Material.Types.Physical:
                        output.AppendLine(SetItem("clearcoat", material.Clearcoat));
                        output.AppendLine(SetItem("clearcoatRoughness", material.ClearcoatRoughness));
                        output.AppendLine(SetItem("metalness", material.Metalness));
                        output.AppendLine(SetItem("roughness", material.Roughness));
                        output.AppendLine(SetItem("sheen", material.Sheen));
                        output.AppendLine(SetItem("sheenRoughness", material.SheenRoughness));
                        output.AppendLine(SetItem("reflectivity", material.Reflectivity));
                        break;
                }

                output.AppendLine(SetItem("color", material.DiffuseColor, false, false));
            }
            }
            else if (input.IsCurve)
            {
                bool hasVertexColors = graphic.Colors.Count > 1;
                string materialType = "LineBasicMaterial";
                if (graphic.HasDash) materialType = "LineDashedMaterial";
                output.AppendLine(SetItem("type", materialType));
                if (hasVertexColors)
                {
                    output.AppendLine("\"color\" : " + Sd.ColorTranslator.ToOle(Sd.Color.White).ToString() + ",");
                }
                else
                {
                    output.AppendLine("\"color\" : " + Sd.ColorTranslator.ToOle(graphic.Color).ToString() + ",");
                }
                output.AppendLine(SetItem("linewidth", graphic.Width));
                if (graphic.HasDash) output.AppendLine(SetItem("dashSize", graphic.DashLength));
                if (graphic.HasDash) output.AppendLine(SetItem("gapSize", graphic.GapLength));
                output.AppendLine(SetItem("vertexColors", hasVertexColors, false));

            }
            else if (input.IsCloud)
            {
                output.AppendLine(SetItem("type", "PointsMaterial"));
                output.AppendLine("\"color\" : " + Sd.ColorTranslator.ToOle(Sd.Color.White).ToString() + ",");
                output.AppendLine(SetItem("vertexColors", true, false));
            }
            output.AppendLine(CloseObject(comma));
            return output.ToString();
        }

        #endregion

        #region lights

        private static string ToJson(this Light input)
        {
            StringBuilder output = new StringBuilder();

            Rg.Plane frame = Rg.Plane.WorldZX;
            frame.Rotate(Math.PI, frame.ZAxis);
            frame.Rotate(Math.PI, frame.YAxis);

            Rg.Plane plane = new Rg.Plane(input.Frame);
            plane.Origin = new Rg.Point3d(plane.Origin-new Rg.Point3d(input.Target.X, input.Target.Y, input.Target.Z));
            //frame.Origin = new Rg.Point3d(input.Target);

            output.AppendLine("{");
            output.Append(((MetaData)input).ToJsonObjectMeta(false));
            Rg.Transform xform = Rg.Transform.PlaneToPlane(frame, plane);
            //Rg.Transform xform = Rg.Transform.Multiply(Rg.Transform.PlaneToPlane(frame, input.Frame),Rg.Transform.Translation(new Rg.Vector3d(input.Target)));

            switch (input.LightType)
            {
                case Light.Types.Point:
                    output.AppendLine(SetMatrixItem("matrix", input));
                    output.AppendLine(SetItem("distance", input.Distance));
                    output.AppendLine(SetItem("decay", input.Decay));
                    break;
                case Light.Types.Ambient:
                    output.AppendLine(SetItem("matrix", input.Matrix));
                    break;
                case Light.Types.Directional:
                    output.AppendLine(SetItem2("matrix", xform));
                    break;
                case Light.Types.Hemisphere:
                    output.AppendLine(SetMatrixItem("matrix", input, false));
                    output.AppendLine(SetItem("groundColor", input.ColorB));
                    break;
                case Light.Types.Spot:
                    output.AppendLine(SetItem2("matrix", xform));
                    output.AppendLine(SetItem("angle", input.Angle));
                    output.AppendLine(SetItem("distance", input.Distance));
                    output.AppendLine(SetItem("decay", input.Decay));
                    break;
            }
            output.AppendLine(SetItem("intensity", input.Intensity));
            output.AppendLine(SetItem("color", input.Color,false));
                //output.AppendLine(camera.ToShadowJson());

                output.AppendLine("},");

            return output.ToString();
        }

        private static string ToJsonMatrix(this Light input, bool absolute = true, int digits = 5)
        {
            if(absolute)
            {
                return "[1,0,0,0,0,1,0,0,0,0,1,0," + Math.Round(-input.Position.X,digits) + "," 
                    + Math.Round(input.Position.Z, digits) + "," 
                    + Math.Round(input.Position.Y, digits) + ",1]";
            }
            else
            {
                return "[1,0,0,0,0,1,0,0,0,0,1,0," + Math.Round(-(input.Position.X-input.Target.X), digits) + ","
                    + Math.Round(input.Position.Z-input.Target.Z, digits) + ","
                    + Math.Round(input.Position.Y - input.Target.Y, digits) + ",1]";
            }
        }

        #endregion

        #region cameras

        private static string ToJsonCamera(this Camera input)
        {
            StringBuilder output = new StringBuilder();

            output.Append(input.ToJson());

            return output.ToString();
        }

        private static string ToJson(this Camera input)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("{");
            output.Append(((MetaData)input).ToJsonObjectMeta(false));

            Rg.Plane frame = Rg.Plane.WorldZX;
            frame.Rotate(Math.PI, frame.ZAxis);
            frame.Rotate(Math.PI, frame.YAxis);

            output.AppendLine(SetItem("matrix", Rg.Transform.PlaneToPlane(frame, input.Frame)));

            output.AppendLine(SetItem("fov", input.FOV));
            output.AppendLine(SetItem("zoom", input.Zoom));
            output.AppendLine(SetItem("near", input.Near));
            output.AppendLine(SetItem("far", input.Far));
            output.AppendLine(SetItem("focus", input.Focus));
            output.AppendLine(SetItem("aspect", input.Aspect));
            output.AppendLine(SetItem("filmGauge", input.FilmGauge));
            output.AppendLine(SetItem("filmOffset", input.FilmOffset, false));

            output.AppendLine("},");

            return output.ToString();
        }

        private static string ToShadowJson(this Camera input)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine(OpenObject("shadow"));
            output.AppendLine(OpenObject("camera"));

            output.AppendLine(((MetaData)input).ToJson());

            output.AppendLine(SetItem("layers", input.Layers));
            output.AppendLine(SetItem("fov", input.FOV));
            output.AppendLine(SetItem("zoom", input.Zoom));
            output.AppendLine(SetItem("near", input.Near));
            output.AppendLine(SetItem("far", input.Far));
            output.AppendLine(SetItem("focus", input.Focus));
            output.AppendLine(SetItem("aspect", input.Aspect));
            output.AppendLine(SetItem("filmGauge", input.FilmGauge));
            output.AppendLine(SetItem("filmOffset", input.FilmOffset, false));

            output.AppendLine("}");
            output.AppendLine("},");

            return output.ToString();
        }

        #endregion

        #region metadata

        private static string ToJsonType(this MetaData input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine(OpenObject(input.Title));

            output.AppendLine(SetItem("type", input.Type));

            output.AppendLine("},");

            return output.ToString();
        }

        private static string ToJson(this MetaData input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine(OpenObject(input.Title));

            output.AppendLine(SetItem("uuid", input.Uuid));
            output.AppendLine(SetItem("type",input.Type));

            output.AppendLine("},");

            return output.ToString();
        }

        private static string ToJsonElementMeta(this MetaData input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine(OpenObject(input.Title));

            output.AppendLine(SetItem("version", input.Version));
            output.AppendLine(SetItem("type", "Object"));
            output.AppendLine(SetItem("generator", input.Generator,false));

            output.AppendLine("},");

            return output.ToString();
        }

        private static string ToJsonObjectMeta(this MetaData input, bool hasMatrix = true)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine(SetItem("uuid", input.Uuid));
            output.AppendLine(SetItem("type", input.ObjectType));
            output.AppendLine(SetItem("name", input.ObjectType.Substring(0,3)+"_"+input.Name.Replace("-",string.Empty)));
            output.AppendLine(SetItem("layers", input.Layers));
            if(hasMatrix)output.AppendLine(SetItem("matrix", input.Matrix));


            return output.ToString();
        }

        #endregion

        #region scripts

        private static string ToJson(this Script input, bool comma=true)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine(OpenArray(input.Uuid.ToString()));
            output.AppendLine(SetItem("name", input.Title));
            output.AppendLine(SetItem("source", input.Contents));

            string txt = "}]";
            if (comma) txt += ",";
            output.AppendLine(txt);

            return output.ToString();
        }

        private static string ToJson(this Settings input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine(OpenObject(input.Title));

            output.AppendLine(SetItem("shadows", input.Shadows));
            output.AppendLine(SetItem("shadowType", input.ShadowType));
            output.AppendLine(SetItem("vr", input.VR));
            output.AppendLine(SetItem("physicallyCorrectLights", input.PhysicalLights));
            output.AppendLine(SetItem("toneMapping", input.ToneMapping));
            output.AppendLine(SetItem("toneMappingExposure", input.ToneMappingExposure,false));

            output.AppendLine("},");

            return output.ToString();
        }

        #endregion


        #region collections

        private static string ToJsonGeometries(this List<Model> inputs)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("\"geometries\": [");

            int i = 1;
            foreach (Model input in inputs)
            {
                output.Append(input.ToJson(i<inputs.Count));
                i++;
            }

            output.Append("],");

            return output.ToString();
        }

        private static string ToJsonObjects(this List<Model> inputs)
        {
            StringBuilder output = new StringBuilder();

            int i = 1;
            foreach (Model input in inputs)
            {
                output.AppendLine("{");
                output.Append(((MetaData)input).ToJsonObjectMeta());
                output.AppendLine(SetItem("geometry", input.GeoId));
                output.AppendLine(SetItem("material", input.Material.Uuid,false));
                output.AppendLine(CloseObject(i<inputs.Count));
                i++;
            }

            return output.ToString();
        }

        private static string ToJsonObjects(this List<Camera> inputs)
        {
            StringBuilder output = new StringBuilder();

            foreach (Camera input in inputs)
            {
                output.Append(input.ToJson());
            }

            return output.ToString();
        }

        private static string ToJsonObjects(this List<Light> inputs, Camera camera)
        {
            StringBuilder output = new StringBuilder();

            foreach (Light input in inputs)
            {
                output.Append(input.ToJson());
            }

            return output.ToString();
        }

        private static string ToJsonMaterials(this List<Model> inputs)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("\"materials\": [");

            int i = 1;
            foreach (Model input in inputs)
            {
                    output.Append(input.ToJsonMaterial(i < inputs.Count));
                i++;
            }

            output.AppendLine("],");

            return output.ToString();
        }

        private static string ToJson(this List<Light> inputs)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("\"lights\": [");

            foreach (Light input in inputs)
            {
                output.AppendLine(input.ToJson());
            }

            output.AppendLine("],");

            return output.ToString();
        }

        private static string ToJson(this List<Camera> inputs)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("\"cameras\": {");

            foreach (Camera input in inputs)
            {
                output.AppendLine(input.ToJson());
            }

            output.AppendLine("],");

            return output.ToString();
        }

        private static string ToJson(this List<Script> inputs)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("\"scripts\": {");

            int i = 1;
            foreach (Script input in inputs)
            {
                output.AppendLine(input.ToJson(i<inputs.Count));
            }

            output.AppendLine("}");

            return output.ToString();
        }

        #endregion

        #region geometry

        private static string ToJson(this Rg.NurbsCurve input, Graphic graphic)
        {
            StringBuilder output = new StringBuilder();

            //input.Flip(true, true, true);
            output.AppendLine(OpenObject("data"));
            output.AppendLine(OpenObject("attributes"));

            //Color
            if (graphic.HasColors) 
            { 
            output.AppendLine(OpenObject("color"));
            output.AppendLine(SetItem("itemSize", 3));
            output.AppendLine(SetItem("type", "Float32Array"));
            output.AppendLine(SetItem("normalized", false));
            output.Append(OpenArray("array"));
            output.Append(graphic.ToColorsString());
            output.AppendLine("]");
            output.AppendLine("},");
            }

            //Vertices
            output.AppendLine(OpenObject("position"));
            output.AppendLine(SetItem("itemSize", 3));
            output.AppendLine(SetItem("type", "Float32Array"));
            output.AppendLine(SetItem("normalized", false));
            output.Append(OpenArray("array"));
            output.Append(input.Points.ControlPolygon().ToList().ToVertexString());
            output.AppendLine("]");
            output.AppendLine("}");

            //Closes Attributes
            output.AppendLine("}");

            //Closes Data
            output.AppendLine("}");

            return output.ToString();
        }

        private static string ToJson(this PointCloud input)
        {
            StringBuilder output = new StringBuilder();

            //input.Flip(true, true, true);
            output.AppendLine(OpenObject("data"));
            output.AppendLine(OpenObject("attributes"));

            //Vertices
            output.AppendLine(OpenObject("position"));
            output.AppendLine(SetItem("itemSize", 3));
            output.AppendLine(SetItem("type", "Float32Array"));
            output.AppendLine(SetItem("normalized", false));
            output.Append(OpenArray("array"));
            output.Append(input.Points.ToVertexString());
            output.AppendLine("]");
            output.AppendLine("},");

            //Color
            output.AppendLine(OpenObject("color"));
            output.AppendLine(SetItem("itemSize", 3));
            output.AppendLine(SetItem("type", "Float32Array"));
            output.AppendLine(SetItem("normalized", false));
            output.Append(OpenArray("array"));
            output.Append(input.Colors.ToColorFloatString());
            output.AppendLine("]");
            output.AppendLine("}");

            //Closes Attributes
            output.AppendLine("}");

            //Closes Data
            output.AppendLine("}");

            return output.ToString();
        }

        private static string ToJson(this Rg.Mesh input)
        {
            StringBuilder output = new StringBuilder();

            input = input.DuplicateMesh();
            input.Faces.ConvertQuadsToTriangles();
            //input.Flip(true, true, true);
            output.AppendLine(OpenObject("data"));
            output.AppendLine(OpenObject("attributes"));

            //Vertices
            output.AppendLine(OpenObject("position"));
            output.AppendLine(SetItem("itemSize", 3));
            output.AppendLine(SetItem("type", "Float32Array"));
            output.AppendLine(SetItem("normalized",false));
            output.Append(OpenArray("array"));
            output.Append(input.ToVertexString());
            output.AppendLine("]");
            output.AppendLine("},");

            //Normals
            output.AppendLine(OpenObject("normal"));
            output.AppendLine(SetItem("itemSize", 3));
            output.AppendLine(SetItem("type", "Float32Array"));
            output.AppendLine(SetItem("normalized", false));
            output.Append(OpenArray("array"));
            output.Append(input.ToNormalString());
            output.AppendLine("]");
            output.AppendLine("},");

            //UV
            output.AppendLine(OpenObject("uv"));
            output.AppendLine(SetItem("itemSize", 2));
            output.AppendLine(SetItem("type", "Float32Array"));
            output.AppendLine(SetItem("normalized", false));
            output.Append(OpenArray("array"));
            output.Append(input.ToUvString());
            output.AppendLine("]");
            output.AppendLine("},");

            //Color
            output.AppendLine(OpenObject("color"));
            output.AppendLine(SetItem("itemSize", 3));
            output.AppendLine(SetItem("type", "Float32Array"));
            output.AppendLine(SetItem("normalized", false));
            output.Append(OpenArray("array"));
            output.Append(input.ToColorFloatString());
            output.AppendLine("]");
            output.AppendLine("},");

            //Index
            output.AppendLine(OpenObject("index"));
            output.AppendLine(SetItem("type", "Uint16Array"));
            output.Append(OpenArray("array"));
            output.Append(input.ToFaceString());
            output.AppendLine("]");
            output.AppendLine("}");//Close Index

            //Closes Attributes
            output.AppendLine("}");

            //Closes Data
            output.AppendLine("}");

            return output.ToString();
        }

        #endregion

    }


}
