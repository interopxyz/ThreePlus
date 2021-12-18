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

        private static string StartProject(string title)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("\"metadata\": {");
            output.AppendLine("\"type\": \""+title+"\"");
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

        private static string SetItem(string title, double[] value, bool comma = true)
        {
            string txt = "\"" + title + "\": [" + string.Join(",", value) + "] ";
            if (comma) txt += ", ";
            return txt;
        }

        private static string SetItem(string title, Sd.Color value, bool comma = true)
        {
            string txt = "\"" + title + "\": " + Sd.ColorTranslator.ToOle(value) + " ";
            if (comma) txt += ", ";
            return txt;
        }

        #endregion

        #region objects

        public static string ToJson(this Scene input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("{");

            output.Append(StartProject("App"));
            output.Append(input.Settings.ToJson());
            output.Append(input.Camera.ToJsonCamera());

            output.AppendLine(OpenObject("scene"));
            output.Append(((MetaData)input).ToJsonElementMeta());
            output.Append(input.Models.ToJsonGeometries());
            output.Append(input.Models.ToJsonMaterials());

            output.AppendLine(OpenObject("object"));
            output.AppendLine(input.ToJsonObjects());

            output.Append("},");

            output.AppendLine(input.Scripts.ToJson());

            output.AppendLine("}");

            return output.ToString();
        }

        private static string ToJsonObjects(this Scene input)
        {
            StringBuilder output = new StringBuilder();


            output.Append(((MetaData)input).ToJsonObjectMeta());

            output.AppendLine(SetItem("background", input.Environment.Background));

            output.AppendLine(OpenArray("children"));
            output.AppendLine(input.Models.ToJsonObjects());
            output.AppendLine(input.Lights.ToJsonObjects(input.Camera));

            output.AppendLine("]");
            output.AppendLine("}");

            return output.ToString();
        }

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
            output.AppendLine(CloseObject(comma));

            return output.ToString();
        }

        private static string ToJson(this Material input, bool comma=true)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine("{");

            output.AppendLine(SetItem("uuid", input.Uuid));
            output.AppendLine(SetItem("type", input.Type));
            output.AppendLine(SetItem("color", input.Color));
            output.AppendLine(SetItem("transparent", input.Transparent));
            output.AppendLine(SetItem("opacity", input.Opacity));
            output.AppendLine(SetItem("roughness", input.Roughness));
            output.AppendLine(SetItem("metalness", input.Metalness));
            output.AppendLine(SetItem("emissive", input.Emissive));
            output.AppendLine(SetItem("emissiveIntensity", input.EmissiveIntensity,false));

            output.AppendLine(CloseObject(comma));
            return output.ToString();
        }

        private static string ToJson(this Light input)
        {
            StringBuilder output = new StringBuilder();


            return output.ToString();
        }

        private static string ToJson(this Camera input)
        {
            StringBuilder output = new StringBuilder();

            output.Append(((MetaData)input).ToJsonObjectMeta());

            output.AppendLine(SetItem("fov", input.FOV));
            output.AppendLine(SetItem("zoom", input.Zoom));
            output.AppendLine(SetItem("near", input.Near));
            output.AppendLine(SetItem("far", input.Far));
            output.AppendLine(SetItem("focus", input.Focus));
            output.AppendLine(SetItem("aspect", input.Aspect));
            output.AppendLine(SetItem("filmGauge", input.FilmGauge));
            output.AppendLine(SetItem("filmOffset", input.FilmOffset,false));

            output.AppendLine("}");

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
            output.AppendLine(SetItem("filmOffset", input.FilmOffset,false));

            output.AppendLine("}");
            output.AppendLine("},");

            return output.ToString();
        }

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

        private static string ToJsonObjectMeta(this MetaData input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine(SetItem("uuid", input.Uuid));
            output.AppendLine(SetItem("type", input.Type));
            output.AppendLine(SetItem("name", input.Name));
            output.AppendLine(SetItem("layers", input.Layers));
            output.AppendLine(SetItem("matrix", input.Matrix));


            return output.ToString();
        }

        #endregion

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

        private static string ToJsonCamera(this Camera input)
        {
            StringBuilder output = new StringBuilder();
            output.AppendLine(OpenObject("camera"));

            output.Append(((MetaData)input).ToJsonElementMeta());

            output.AppendLine(OpenObject("object"));
            output.Append(input.ToJson());

            output.AppendLine("},");

            return output.ToString();
        }

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

            output.AppendLine("],");

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

        private static string ToJsonObjects(this List<Light> inputs, Camera camera)
        {
            StringBuilder output = new StringBuilder();

            foreach (Light input in inputs)
            {
                output.AppendLine("{");
                output.AppendLine(((MetaData)input).ToJsonObjectMeta());
                output.AppendLine(SetItem("color", input.Color));
                output.AppendLine(SetItem("intensity", input.Color));
                output.AppendLine(SetItem("distance", input.Color));
                output.AppendLine(SetItem("decay", input.Color));
                output.AppendLine(SetItem("color", input.Color));
                output.AppendLine(camera.ToShadowJson());

                output.AppendLine("},");
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
                Material material = input.Material;
                output.Append(material.ToJson(i<inputs.Count));
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

        private static string ToJson(this Rg.Mesh input)
        {
            StringBuilder output = new StringBuilder();

            input = input.DuplicateMesh();
            input.Faces.ConvertQuadsToTriangles();

            output.AppendLine(OpenObject("data"));
            output.AppendLine(OpenObject("attributes"));

            //Vertices
            output.AppendLine(OpenObject("position"));
            output.AppendLine(SetItem("itemSize", 3));
            output.AppendLine(SetItem("type", "Float32Array"));
            output.AppendLine(OpenArray("array"));
            output.AppendLine(string.Join(",", input.Vertices.ToFloatArray()));
            output.AppendLine("]");
            output.AppendLine("},");

            //Normals
            output.AppendLine(OpenObject("normal"));
            output.AppendLine(SetItem("itemSize", 3));
            output.AppendLine(SetItem("type", "Float32Array"));
            output.AppendLine(OpenArray("array"));
            output.AppendLine(string.Join(",", input.Normals.ToFloatArray()));
            output.AppendLine("]");
            output.AppendLine("},");

            //UV
            output.AppendLine(OpenObject("uv"));
            output.AppendLine(SetItem("itemSize", 2));
            output.AppendLine(SetItem("type", "Float32Array"));
            output.AppendLine(OpenArray("array"));
            output.AppendLine(string.Join(",", input.TextureCoordinates.ToFloatArray()));
            output.AppendLine("]");
            output.AppendLine("},");

            //Index
            output.AppendLine(OpenObject("index"));//Open Index
            output.AppendLine(SetItem("type", "Uint8Array"));
            output.AppendLine(OpenArray("array"));
            output.AppendLine(string.Join(",", input.Faces.ToIntArray(false)));
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
