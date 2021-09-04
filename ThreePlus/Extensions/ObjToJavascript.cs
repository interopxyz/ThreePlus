using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Sd = System.Drawing;
using Rg = Rhino.Geometry;

namespace ThreePlus
{
    public static class ObjToJavascript
    {
        public static string ToHtml(this Scene input, bool local=false)
        {
            // https://cdn.jsdelivr.net/npm/three@0.131.3/examples/js/
            StringBuilder output = new StringBuilder();

            output.AppendLine("<!DOCTYPE html>");
            output.AppendLine("<html>");
            output.AppendLine("<head>");
            output.AppendLine("<meta charset=\"utf - 8\">");
            output.AppendLine("<title>"+input.Name+"</title>");
            output.AppendLine("<style>");
            output.AppendLine("body { margin: 0; }");
            output.AppendLine("</style>");
            output.AppendLine("</head>");
            output.AppendLine("<body>");
            if (local)
            {
                output.AppendLine("<script src=\"js/three.js\"></script>");
                output.AppendLine("<script src=\"js/OrbitControls.js\"></script>");
                output.AppendLine("<script src=\"js/VertexNormalsHelper.js\"></script>");
                output.AppendLine("<script src=\"js/VertexTangentsHelper.js\"></script>");
            }
            else
            {
                output.AppendLine("<script src=\"http://threejs.org/build/three.min.js\"></script>");
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.101.1/examples/js/controls/OrbitControls.js\" ></script>");
            }
            output.AppendLine("<script type=\"text/javascript\" src=\"app.js\"></script>");
            output.AppendLine("</body>");
            output.AppendLine("</html>");

            return output.ToString();
        }

        public static string ToJavascript(this Scene input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("const scene = new THREE.Scene();");

            output.AppendLine("const renderer = new THREE.WebGLRenderer({ antialias: true });");
            output.AppendLine("renderer.setSize(window.innerWidth, window.innerHeight);");
            output.AppendLine("document.body.appendChild(renderer.domElement);");

            output.Append(input.Camera.ToJavascript());
            output.Append(input.Grid.ToJavascript());
            output.Append(input.Axes.ToJavascript());

            int i = 0;
            foreach(Model model in input.Models)
            {
                string index = i.ToString();
                if (model.IsMesh) output.Append(model.Mesh.ToJavascript(index));
                output.AppendLine(model.Material.ToJavascript(index));
                output.AppendLine("const model"+ index + " = new THREE.Mesh( mesh"+ index + ", material" + index + " );");
                output.AppendLine("scene.add(model"+ index + ");");
                if (model.IsCurve) output.Append(model.Tangents.ToJavascript(index));
                if (model.IsMesh) output.Append(model.Normals.ToJavascript(index));
                i++;
            }

            i = 0;
            foreach (Light light in input.Lights)
            {
                string index = i.ToString();
                output.AppendLine(light.ToJavascript(index));
                output.AppendLine("scene.add(light" + index + ");");
                i++;
            }

            output.AppendLine("controls = new THREE.OrbitControls (camera, renderer.domElement);");
            output.AppendLine("const animate = function () {");
            output.AppendLine("controls.update();");
            output.AppendLine("requestAnimationFrame ( animate ); ");
            output.AppendLine("renderer.render (scene, camera);");
            output.AppendLine("};");
            output.AppendLine("animate();");

            return output.ToString();
        }

        public static string ToJavascript(this TangentDisplay input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.Show)
            {
                output.AppendLine("const tanHelper" + index + " = new THREE.VertexTangentsHelper(model" + index + "," + input.Width + "," + input.Color.ToJs() + "," + input.Width + ");");
                output.AppendLine("scene.add(tanHelper" + index + ");");
            }

            return output.ToString();
        }

        public static string ToJavascript(this NormalDisplay input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.Show)
            {
                output.AppendLine("const nrmHelper" + index + " = new THREE.VertexNormalsHelper(model" + index + "," + input.Width + "," + input.Color.ToJs() + "," + input.Width + ");");
                output.AppendLine("scene.add(nrmHelper" + index + ");");
            }

            return output.ToString();
        }

        public static string ToJavascript(this Axes input)
        {
            StringBuilder output = new StringBuilder();

            if (input.Show)
            {
                output.AppendLine("const axes = new THREE.AxesHelper(" + input.Scale + ");");
                output.AppendLine("axes.setColors (" + input.XAxis.ToJs() + ");");
                output.AppendLine("scene.add(axes);");
            }

            return output.ToString();
        }

        public static string ToJavascript(this Grid input)
        {
            StringBuilder output = new StringBuilder();

            if (input.Show) {
            output.AppendLine("var grid = new THREE.GridHelper(" + input.Size + "," + input.Divisions + ","+ input.AxisColor.ToJs() + ","+ input.GridColor.ToJs() + ");");
            output.AppendLine("scene.add(grid);");
            }

            return output.ToString();
        }

        public static string ToJavascript(this Camera input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("const camera = new THREE.PerspectiveCamera(" +input.FOV+", window.innerWidth / window.innerHeight, "+input.Near+"," + input.Far + ");");
            output.AppendLine("camera.position.set("+input.Position.ToJs()+");");
            output.AppendLine("camera.lookAt (new THREE.Vector3("+ input.Target.X + ","+ input.Target.Z + ","+ input.Target.Y + "));");

            return output.ToString();
        }

        public static string ToJavascript(this Material input, string name)
        {
            StringBuilder output = new StringBuilder();

            string starter = "const material" + name + " = new THREE.";
            switch (input.MaterialType)
            {
                default:
                    output.AppendLine(starter+"MeshBasicMaterial( { color: " + input.Color.ToJs() + " } );");
                    break;
                case Material.Types.Lambert:
                    output.AppendLine(starter + "MeshLambertMaterial( { color: " + input.Color.ToJs() + " } );");
                    break;
                case Material.Types.Normal:
                    output.AppendLine(starter + "MeshNormalMaterial();");
                    break;
                case Material.Types.Depth:
                    output.AppendLine(starter + "MeshDepthMaterial();");
                    break;
            }

            return output.ToString();
        }
        public static string ToJavascript(this Light input, string index)
        {
            StringBuilder output = new StringBuilder();
            string name = "light" + index;
            string starter = "const " + name + " = new THREE.";
            switch (input.LightType)
            {
                default:
                    output.AppendLine(starter + "AmbientLight( { color: " + input.Color.ToJs() + ", intensity: " + input.Intensity + " } );");
                    break;
                case Light.Types.Point:
                    output.AppendLine(starter + "PointLight( { color: " + input.Color.ToJs() + " , intensity : " + input.Intensity + " , distance : " + input.Distance + " , decay  : " + input.Decay + " } );");
                    output.AppendLine(name + ".position.set( " + input.Position.ToJs() + " );");
                    break;
                case Light.Types.Directional:
                    output.AppendLine(starter + "DirectionalLight( { color: " + input.Color.ToJs() + ", intensity: " + input.Intensity + " } );");
                    output.AppendLine(name + ".position.set( " + input.Position.ToJs() + " );");
                    output.AppendLine(input.Target.ToJsTarget(name));
                    break;
                case Light.Types.Hemisphere:
                    output.AppendLine(starter + "HemisphereLight( { skyColor : " + input.Color.ToJs() + ", groundColor : " + input.ColorB.ToJs() + ", intensity: " + input.Intensity + " } );");
                    break;
            }

            return output.ToString();
        }



        public static string ToJavascript(this Rg.Mesh input,string name)
        {
            input = input.DuplicateMesh();
            input.Faces.ConvertQuadsToTriangles();
            //input.RebuildNormals();

            StringBuilder output = new StringBuilder();

            output.AppendLine("const mesh" + name + " = new THREE.BufferGeometry();");

            List<string> vertices = new List<string>();
            List<string> normals = new List<string>();
            List<string> uvs = new List<string>();
            
            foreach (Rg.MeshFace face in input.Faces)
            {
                vertices.Add(input.Vertices[face.A].ToJs());
                normals.Add(input.Normals[face.A].ToJs());
                uvs.Add(input.TextureCoordinates[face.A].ToJs());

                vertices.Add(input.Vertices[face.C].ToJs());
                normals.Add(input.Normals[face.C].ToJs());
                uvs.Add(input.TextureCoordinates[face.C].ToJs());

                vertices.Add(input.Vertices[face.B].ToJs());
                normals.Add(input.Normals[face.B].ToJs());
                uvs.Add(input.TextureCoordinates[face.B].ToJs());
            }

            output.AppendLine("const vertices" + name + " = new Float32Array( [" + string.Join(",", vertices) + "] );");
            output.AppendLine("const normals" + name + " = new Float32Array( [" + string.Join(",", normals) + "] );");
            output.AppendLine("const uv" + name + " = new Float32Array( [" + string.Join(",", uvs) + "] );");
            //output.AppendLine("const uv" + name + " = new Float32Array( [" + string.Join(",", input.TextureCoordinates.ToFloatArray()) + "] );");
            //output.AppendLine("const faces" + name + " = new Uint8Array( [" + string.Join(",", input.Faces.ToIntArray(false)) + "] );");

            output.AppendLine("mesh"+name + ".setAttribute( 'position', new THREE.BufferAttribute( vertices" + name + ", 3 ) );");
            output.AppendLine("mesh"+name + ".setAttribute( 'normal', new THREE.BufferAttribute( normals" + name + ", 3 ) );");
            output.AppendLine("mesh"+name + ".setAttribute( 'uv', new THREE.BufferAttribute( uv" + name + ", 2 ) );");
            //output.AppendLine("mesh"+name + ".setAttribute( 'index', new THREE.BufferAttribute( faces" + name + ", 3 ) );");

            return output.ToString();
        }

        #region conversions

        public static string ToJsTarget(this Rg.Point3d input, string name)
        {
            StringBuilder output = new StringBuilder();

            string targetObject = name + "Target";
            output.AppendLine("const "+ targetObject + "= new THREE.Object3D();");
            output.AppendLine(targetObject + ".position.set( "+input.ToJs()+" );");
            output.AppendLine("scene.add("+targetObject+");");
            output.AppendLine(name + ".target = " + targetObject + ";");

            return output.ToString();
        }

        public static string ToJs(this Sd.Color value)
        {
            return Sd.ColorTranslator.ToOle(value).ToString();
        }

        public static string ToJs(this Rg.Point3d value)
        {
            return value.X+","+value.Z+","+value.Y;
        }

        public static string ToJs(this Rg.Point3f value)
        {
            return value.X + "," + value.Z + "," + value.Y;
        }

        public static string ToJs(this Rg.Vector3d value)
        {
            return value.X + "," + value.Z + "," + value.Y;
        }

        public static string ToJs(this Rg.Vector3f value)
        {
            return value.X + "," + value.Z + "," + value.Y;
        }

        public static string ToJs(this Rg.Point2f value)
        {
            return value.X + "," + value.Y;
        }

        #endregion

    }
}
