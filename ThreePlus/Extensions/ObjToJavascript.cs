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

            output.AppendLine("const renderer = new THREE.WebGLRenderer();");
            output.AppendLine("renderer.setSize(window.innerWidth, window.innerHeight);");
            output.AppendLine("document.body.appendChild(renderer.domElement);");

            output.Append(input.Camera.ToJavascript());
            int i = 0;
            foreach(Model model in input.Models)
            {
                string index = i.ToString();
                if (model.IsMesh) output.Append(model.Mesh.ToJavascript(index));
                output.AppendLine(model.Material.ToJavascript(index));
                output.AppendLine("const model"+ index + " = new THREE.Mesh( mesh"+ index + ", material" + index + " );");
                output.AppendLine("scene.add(model"+ index + ");");

                i++;
            }

            output.AppendLine("renderer.render(scene, camera); ");

            return output.ToString();
        }

        public static string ToJavascript(this Camera input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("const camera = new THREE.PerspectiveCamera(" +input.FOV+", window.innerWidth / window.innerHeight, "+input.Near+"," + input.Far + ");");
            output.AppendLine("camera.position.x = "+input.Position.X+";");
            output.AppendLine("camera.position.y = "+ input.Position.Y+ ";");
            output.AppendLine("camera.position.z = " + input.Position.Z + ";");

            return output.ToString();
        }

        public static string ToJavascript(this Material input, string name)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("const material" + name + " = new THREE.MeshBasicMaterial( { color: " + SetColor(input.Color) + " } );");

            return output.ToString();
        }

        public static string ToJavascript(this Rg.Mesh input,string name)
        {
            input = input.DuplicateMesh();
            input.Faces.ConvertQuadsToTriangles();

            StringBuilder output = new StringBuilder();

            output.AppendLine("const mesh" + name + " = new THREE.BufferGeometry();");

            output.AppendLine("const vertices" + name + " = new Float32Array( [" + string.Join(",", input.Vertices.ToFloatArray()) + "] );");
            output.AppendLine("const normals" + name + " = new Float32Array( [" + string.Join(",", input.Normals.ToFloatArray()) + "] );");
            output.AppendLine("const uv" + name + " = new Float32Array( [" + string.Join(",", input.TextureCoordinates.ToFloatArray()) + "] );");
            output.AppendLine("const faces" + name + " = new Uint8Array( [" + string.Join(",", input.Faces.ToIntArray(false)) + "] );");

            output.AppendLine("mesh"+name + ".setAttribute( 'position', new THREE.BufferAttribute( vertices" + name + ", 3 ) );");
            output.AppendLine("mesh"+name + ".setAttribute( 'normal', new THREE.BufferAttribute( normals" + name + ", 3 ) );");
            output.AppendLine("mesh"+name + ".setAttribute( 'uv', new THREE.BufferAttribute( uv" + name + ", 2 ) );");
            output.AppendLine("mesh"+name + ".setAttribute( 'index', new THREE.BufferAttribute( faces" + name + ", 3 ) );");

            return output.ToString();
        }

        #region conversions

        public static string SetColor(Sd.Color value)
        {
            return Sd.ColorTranslator.ToOle(value).ToString();
        }

            #endregion
        }
}
