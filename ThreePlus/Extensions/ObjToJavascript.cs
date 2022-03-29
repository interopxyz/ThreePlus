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
        public static string ToHtml(this Scene input, bool local = false, bool embed = false)
        {
            // https://cdn.jsdelivr.net/npm/three@0.131.3/examples/js/
            StringBuilder output = new StringBuilder();

            output.AppendLine("<!DOCTYPE html>");
            output.AppendLine("<html>");
            output.AppendLine("<head>");
            output.AppendLine("<meta charset=\"utf - 8\">");
            output.AppendLine("<title>" + input.Name + "</title>");
            output.AppendLine("<style>");
            output.AppendLine("body { margin: 0; }");
            output.AppendLine("</style>");
            output.AppendLine("</head>");
            output.AppendLine("<body>");
            if (local)
            {
                output.AppendLine("<script src=\"js/three.min.js\"></script>");
                output.AppendLine("<script src=\"js/OrbitControls.js\"></script>");
                output.AppendLine("<script src=\"js/VertexNormalsHelper.js\"></script>");
                output.AppendLine("<script src=\"js/VertexTangentsHelper.js\"></script>");

                if (input.HasCurves)
                {
                    //output.AppendLine("<script src=\"js/GeometryUtils.js\"></script>");
                    output.AppendLine("<script src=\"js/LineSegmentsGeometry.js\"></script>");
                    output.AppendLine("<script src=\"js/LineSegments2.js\"></script>");
                    output.AppendLine("<script src=\"js/LineGeometry.js\"></script>");
                    output.AppendLine("<script src=\"js/LineMaterial.js\"></script>");
                    output.AppendLine("<script src=\"js/Line2.js\"></script>");
                }

                if (input.AmbientOcclusion.HasAO)
                {
                    output.AppendLine("<script src=\"js/EffectComposer.js\"></script>");
                    output.AppendLine("<script src=\"js/CopyShader.js\"></script>");
                    output.AppendLine("<script src=\"js/ShaderPass.js\"></script>");

                    output.AppendLine("<script src=\"js/SSAOPass.js\"></script>");
                    output.AppendLine("<script src=\"js/SimplexNoise.js\"></script>");
                    output.AppendLine("<script src=\"js/SSAOShader.js\"></script>");
                }

                if (input.Outline.HasOutline) output.AppendLine("<script src=\"js/OutlineEffect.js\"></script>");//
                if (input.Environment.EnvironmentMode == Environment.EnvironmentModes.CubeMap)
                {
                    output.AppendLine("<script src=\"js/LightProbeGenerator.js\"></script>");//
                    output.AppendLine("<script src=\"js/LightProbeHelper.js\"></script>");//
                }
                if(input.Sky.HasSky) output.AppendLine("<script src=\"js/Sky.js\"></script>");
            }
            else
            {
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/build/three.min.js\" ></script>");
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/controls/OrbitControls.js\" ></script>");
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/helpers/VertexNormalsHelper.js\" ></script>");
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/helpers/VertexTangentsHelper.js\" ></script>");

                if (input.AmbientOcclusion.HasAO)
                {
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/postprocessing/EffectComposer.js\" ></script>");
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/shaders/CopyShader.js\" ></script>");
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/postprocessing/ShaderPass.js\" ></script>");

                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/postprocessing/SSAOPass.js\" ></script>");
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/math/SimplexNoise.js\" ></script>");
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/shaders/SSAOShader.js\" ></script>");
                }

                if (input.Outline.HasOutline) output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/effects/OutlineEffect.js\" ></script>");
                if (input.Environment.EnvironmentMode == Environment.EnvironmentModes.CubeMap)
                {
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/lights/LightProbeGenerator.js\" ></script>");
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/helpers/LightProbeHelper.js\" ></script>");
                }

                if (input.HasCurves) 
                {
                //output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/utils/GeometryUtils.js\" ></script>");
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/lines/LineSegmentsGeometry.js\" ></script>");
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/lines/LineSegments2.js\" ></script>");
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/lines/LineGeometry.js\" ></script>");
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/lines/LineMaterial.js\" ></script>");
                output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/lines/Line2.js\" ></script>");
                }

                if(input.Sky.HasSky)
                {
                    output.AppendLine("<script src=\"https://cdn.jsdelivr.net/npm/three@0.136.0/examples/js/objects/Sky.js\" ></script>");
                }
            }

            if (embed)
            {
                output.AppendLine("<script>");
                output.AppendLine(input.ToJavascript("", false));
                output.AppendLine("</script>");
            }
            else
            {
            output.AppendLine("<script type=\"text/javascript\" src=\"app.js\"></script>");
            }
            output.AppendLine("</body>");
            output.AppendLine("</html>");

            return output.ToString();
        }

        public static string ToJavascript(this Scene input, string path, bool assets = false)
        {
            Dictionary<string, string> maps = new Dictionary<string, string>();
            StringBuilder output = new StringBuilder();
            StringBuilder sceneModifiers = new StringBuilder();
            bool hasMaterial = false;
            List<string> tweenIndices = new List<string>();

            Rg.BoundingBox bbox = input.Models.GetBoundary();

            output.AppendLine("const scene = new THREE.Scene();");

            if(input.HasClickEvent)
            {
                output.AppendLine("var raycaster = new THREE.Raycaster();");
                output.AppendLine("var mouse = new THREE.Vector2();");
            }

            output.AppendLine(input.Environment.ToJavascript(path,assets));
            if (input.Atmosphere.HasFog) output.AppendLine(input.Atmosphere.ToJavascript());

            output.AppendLine("const renderer = new THREE.WebGLRenderer({ antialias: true });");
            output.AppendLine("renderer.setSize(window.innerWidth, window.innerHeight);");
            output.AppendLine("const gradFormat = ( renderer.capabilities.isWebGL2 ) ? THREE.RedFormat : THREE.LuminanceFormat;");
            if (input.HasShadows)
            {
                output.AppendLine("renderer.shadowMap.enabled = true;");
                output.AppendLine("renderer.shadowMap.type = THREE.PCFSoftShadowMap;");
            }
            output.AppendLine(input.Sky.ToJavascript(bbox.Diagonal.Length*1000.0));

            output.AppendLine("document.body.appendChild(renderer.domElement);");

            output.AppendLine(input.Grid.ToJavascript());
            output.AppendLine(input.Axes.ToJavascript());

            System.Security.Cryptography.SHA256 crypt = System.Security.Cryptography.SHA256.Create();

            foreach (Model model in input.Models)
            {
                if (model.IsMesh | model.IsShape)
                {
                    for (int j = 0; j < 14; j++)
                    {
                        if (model.Material.Maps[j] != null)
                        {
                            string key = model.Material.Maps[j].GetHash(crypt);
                            if (!maps.ContainsKey(key))
                            {
                                string name = "map" + maps.Count;
                                maps.Add(key, name);
                                output.AppendLine(model.Material.Maps[j].ToJsObjMap(path,name, assets));
                            }
                            model.Material.MapNames[j] = maps[key];
                        }
                    }
                }
                else if (model.IsCloud)
                {
                    if(model.Cloud.HasBitmap)
                    {
                        string key = model.Cloud.Map.GetHash(crypt);
                        if (!maps.ContainsKey(key))
                        {
                            string name = "map" + maps.Count;
                            maps.Add(key, name);
                            output.AppendLine(model.Cloud.Map.ToJsObjMap(path, name, assets));
                        }
                        model.Cloud.MapName= maps[key];
                    }
                }
            }

            int i = 0;
            foreach (Model model in input.Models)
            {
                string index = i.ToString();
                if (model.IsMesh)
                {
                    if (model.Material.IsDefault)
                    {
                        if (model.Mesh.VertexColors.Count == model.Mesh.Vertices.Count)
                        {
                            output.AppendLine("const material" + index + " = new THREE.MeshBasicMaterial( {vertexColors: THREE.VertexColors, side: THREE.DoubleSide} );");
                            hasMaterial = true;
                        }
                    }
                }

                if (model.IsCurve)
                {
                    output.AppendLine(model.Curve.ToJavascript(index,model.Graphic));
                    sceneModifiers.AppendLine("lineMat" + index + ".resolution.set(window.innerWidth, window.innerHeight);");
                    output.Append(model.Tangents.ToJavascript(index));
                }
                else if (model.IsCloud)
                {
                    output.AppendLine(model.Cloud.ToJavascript(index));
                }
                else
                {

                    if (!hasMaterial) output.AppendLine(model.Material.ToJavascript(index));

                    if (model.IsPlane) output.Append(model.Plane.ToJavascript(model.Size, index));

                    if (model.IsMesh)
                    {
                        output.Append(model.Mesh.ToJavascript(index));
                    }

                    if(model.IsShape)
                    {
                        output.Append(model.Shape.ToJavascript(index));
                    }

                    if (input.HasShadows)
                    {
                        if (model.Material.DiffuseColor.A > (input.ShadowThreshold*255.0))
                        { 
                            output.AppendLine("model" + index + ".castShadow = true;");
                            if (model.Material.MaterialType != Material.Types.Toon) output.AppendLine("model" + index + ".receiveShadow = true;");
                        }
                    }

                    if (model.HasHelper)
                    {
                        output.AppendLine("const bbox" + index + " = new THREE.BoxHelper( model" + index + ", " + model.HelperColor.ToStr() + " );");
                        output.AppendLine("scene.add( bbox" + index + " );");
                    }

                    if (model.IsMesh)
                    {
                        output.Append(model.Normals.ToJavascript(index));
                        output.Append(model.ToJsEdges(index));
                    }
                }

                //if (model.HasTweens)
                //{
                //    List<string> matrix = new List<string>();
                //    foreach (Rg.Transform xform in model.Tweens)
                //    {
                //        matrix.Add("[" + xform.ToJavascript()
                //            + "]");
                //    }
                //    tweenIndices.Add(index);
                //    output.AppendLine("const mtx"+index+" = [" + String.Join(",", matrix) + "];");
                //}

                output.AppendLine("scene.add(model" + index + ");");
                output.AppendLine("model" + index + ".name = \"" + model.Name + "\";");
                output.AppendLine(model.ToUserData(index));

                i++;
            }

            if (input.Camera.IsDefault)
            {
                input.Camera.Position = bbox.Max + bbox.Diagonal;
                input.Camera.Far = bbox.Diagonal.Length * 100;
                input.Camera.Target = bbox.Center;
            }

            output.AppendLine(input.Camera.ToJavascript());

            i = 0;
            int lightCount = 16;
            if (input.Lights.Count < 16) lightCount = input.Lights.Count;
            foreach (Light light in input.Lights)
            {
                string index = i.ToString();
                output.AppendLine(light.ToJavascript(index));
                if (light.HasShadow) output.Append(light.ToJsShadow(index, bbox));
                output.AppendLine("scene.add(light" + index + ");");
                i++;
            }

            output.Append(input.AmbientOcclusion.ToJavascript());
            output.AppendLine(input.Outline.ToJavascript());

            output.AppendLine("window.addEventListener('resize', onWindowResize);");
            if(input.HasClickEvent) output.AppendLine("renderer.domElement.addEventListener('click', onClick, false);");
            output.AppendLine("function onWindowResize()");
            output.AppendLine("{");
            output.AppendLine("camera.aspect = window.innerWidth / window.innerHeight;");
            output.AppendLine("camera.updateProjectionMatrix();");
            output.AppendLine("renderer.setSize(window.innerWidth, window.innerHeight);");
            output.AppendLine("}");

            output.AppendLine("controls = new THREE.OrbitControls (camera, renderer.domElement);");
            output.AppendLine("controls.target.set(" + input.Camera.Target.ToStr() + ");");

            if (input.Camera.IsAnimated)
            {
                output.AppendLine("var camStep = 0;");
            }
            foreach (string ind in tweenIndices)
            {
                output.AppendLine("var quatStep"+ind+" = 0;");
            }

            output.AppendLine("var increment = 0;");
            output.AppendLine("const animate = function () {");

            foreach (string ind in tweenIndices) output.AppendLine("var mtrx" + ind + " = increment%" + 100 + ";");

            if (input.Camera.IsAnimated)
            {
                output.AppendLine("camStep = Math.ceil(increment)%" + input.Camera.Tweens.Count + ";");
                output.AppendLine("camera.position.set(tweenB[camStep][0],tweenB[camStep][1],tweenB[camStep][2]);");
                output.AppendLine("camera.lookAt (new THREE.Vector3(tweenA[camStep][0],tweenA[camStep][1],tweenA[camStep][2]));");
                output.AppendLine("camera.updateProjectionMatrix();");
            }

            foreach(string ind in tweenIndices)
            {
                output.AppendLine("const matrix" + ind + " = new THREE.Matrix4()");
                    output.AppendLine("matrix"+ind+".set(mtx" 
                    + ind + "[mtrx" + ind + "][0],mtx" 
                    + ind + "[mtrx" + ind + "][1],mtx"
                    + ind + "[mtrx" + ind + "][2],mtx"
                    + ind + "[mtrx" + ind + "][3],mtx"
                    + ind + "[mtrx" + ind + "][4],mtx"
                    + ind + "[mtrx" + ind + "][5],mtx"
                    + ind + "[mtrx" + ind + "][6],mtx"
                    + ind + "[mtrx" + ind + "][7],mtx"
                    + ind + "[mtrx" + ind + "][8],mtx"
                    + ind + "[mtrx" + ind + "][9],mtx"
                    + ind + "[mtrx" + ind + "][10],mtx"
                    + ind + "[mtrx" + ind + "][11],mtx"
                    + ind + "[mtrx" + ind + "][12],mtx"
                    + ind + "[mtrx" + ind + "][13],mtx"
                    + ind + "[mtrx" + ind + "][14],mtx"
                    + ind + "[mtrx" + ind + "][15]"
                    + ");");
                output.AppendLine("model"+ ind + ".applyMatrix4(matrix" + ind + ");");
            }

            output.AppendLine("controls.update();");
            output.AppendLine("requestAnimationFrame ( animate ); ");
            if (sceneModifiers.Length > 0)
            {
                output.AppendLine("renderer.clearDepth();");
                output.AppendLine(sceneModifiers.ToString());
            }

            output.AppendLine(input.Environment.ToLightProbeClose());

            output.AppendLine("renderer.render (scene, camera);");

            if (input.AmbientOcclusion.HasAO) output.AppendLine("composer.render();");
            if (input.Outline.HasOutline) output.AppendLine("outline.render(scene,camera);");

            output.AppendLine("increment=increment+"+ input.Camera.Speed + ";");
            output.AppendLine("};");

            if(input.HasClickEvent)
            {
                output.AppendLine("function onClick() {");
                output.AppendLine("event.preventDefault();");
                output.AppendLine("mouse.x = (event.clientX / window.innerWidth) * 2 - 1;");
                output.AppendLine("mouse.y = -(event.clientY / window.innerHeight) * 2 + 1;");
                output.AppendLine("raycaster.setFromCamera(mouse, camera);");
                output.AppendLine("var intersects = raycaster.intersectObject(scene, true);");

                output.AppendLine("if (intersects.length > 0) {");
                output.AppendLine("var object = intersects[0].object;");
                output.AppendLine("var objData = object.userData;");

                //If Link
                if(input.ClickEvent == Scene.ClickEvents.Link)
                {
                    output.AppendLine("if ('URL' in objData) {");
                    output.AppendLine("window.open(objData.URL);");
                    output.AppendLine("}");
                }

                //If Data
                if (input.ClickEvent == Scene.ClickEvents.Data)
                {
                    output.AppendLine("var dataSet = (`${object.name} | Data \n`);");
                    output.AppendLine("for (const key in objData) {");
                    output.AppendLine("dataSet+=(`${key}: ${objData[key]} \n`);");
                    output.AppendLine("}");
                    output.AppendLine("navigator.clipboard.writeText(dataSet);");
                    output.AppendLine("window.alert(dataSet);");
                }

                output.AppendLine("}");
                output.AppendLine("animate();");
                output.AppendLine("}");

            }


            output.AppendLine("animate();");

            return output.ToString();
        }

        public static string ToUserData(this Model input, string index)
        {

            if (input.Data.Count > 0)
            {
                List<string> data = new List<string>();
                foreach(string key in input.Data.Keys)
                {
                    data.Add(key+": \"" + input.Data[key] + "\"");
                }
                return "model" + index + ".userData = {" + string.Join(",",data) + "};";
            }
            else
            {
                return string.Empty;
            }
        }

        //public static string ToJsVertexMaterial(this Rg.Mesh mesh, string name)
        //{
        //    StringBuilder output = new StringBuilder();

        //    string starter = "const material" + name + " = new THREE.MeshBasicMaterial({ vertexColors: THREE.VertexColors });";

        //    return output.ToString();
        //}

        public static string ToJsShadow(this Light input, string index, Rg.BoundingBox bbox)
        {
            StringBuilder output = new StringBuilder();
            string name = "light" + index;
            int radius = (int)(bbox.Diagonal.Length);
            switch (input.LightType)
            {
                case Light.Types.Directional:
                case Light.Types.Spot:
                case Light.Types.Point:
                    output.AppendLine(name + ".castShadow = true;");

                    output.AppendLine(name + ".shadow.camera.left = -" + radius + ";");
                    output.AppendLine(name + ".shadow.camera.right = " + radius + ";");
                    output.AppendLine(name + ".shadow.camera.top = " + radius + ";");
                    output.AppendLine(name + ".shadow.camera.bottom = " + -radius + ";");

                    output.AppendLine(name + ".shadow.camera.near = 0.01;");
                    output.AppendLine(name + ".shadow.camera.far = " + (int)Math.Ceiling(input.Position.DistanceTo(bbox.Center) * 1.5 + radius) * 10 + ";");

                    output.AppendLine(name + ".shadow.mapSize = new THREE.Vector2(" + radius * 20 + "," + radius * 20 + ");");
                    break;

            }
            return output.ToString();
        }

        public static string ToJavascript(this Outline input)
        {
            StringBuilder output = new StringBuilder();

            if (input.HasOutline)
            {
                output.AppendLine("let outline = new THREE.OutlineEffect(renderer)");
            }

            return output.ToString();
        }

        public static string ToJavascript(this AmbientOcclusion input)
        {
            StringBuilder output = new StringBuilder();

            if (input.HasAO)
            {
                output.AppendLine("let composer = new THREE.EffectComposer( renderer );");
                output.AppendLine("const ssaoPass = new THREE.SSAOPass( scene, camera, window.innerWidth, window.innerHeight );");
                output.AppendLine("ssaoPass.kernelRadius = " + input.Radius + ";");
                output.AppendLine("ssaoPass.minDistance = " + input.MinDistance + ";");
                output.AppendLine("ssaoPass.maxDistance = " + input.MaxDistance + ";");
                output.AppendLine("composer.addPass( ssaoPass );");
            }

            return output.ToString();
        }

        //public static string ToJavascript(this Ground input)
        //{
        //    StringBuilder output = new StringBuilder();

        //    output.AppendLine("var groundPlane = new THREE.Mesh(new THREE.PlaneBufferGeometry(" + input.Size + "," + input.Size + "), materialGround);");
        //    output.AppendLine("groundPlane.position.y = " + input.Height + ";");
        //    output.AppendLine("groundPlane.rotation.x = - Math.PI / 2;");
        //    output.AppendLine("groundPlane.receiveShadow = true;");
        //    output.AppendLine("scene.add(groundPlane);");

        //    return output.ToString();
        //}

        public static string ToJavascript(this Atmosphere input)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("scene.fog = new THREE.FogExp2( " + input.Color.ToStr() + " , " + input.Density / 1000.00 + " );");

            return output.ToString();
        }

        public static string ToJsObjMap(this Sd.Bitmap input, string path, string name, bool assets = false)
        {
            StringBuilder output = new StringBuilder();
            string mapName = "";
            if (assets) mapName = "./assets/" + input.SavePng(path,name); 
            if (!assets) mapName = "data:image / png; base64," + input.ToStr();
            output.AppendLine("var " + name + " = new THREE.TextureLoader().load(\""+ mapName + "\");");
            output.AppendLine(name + ".mapping = THREE.UVMapping;");

            return output.ToString();
        }

        public static string ToJavascript(this CubeMap input)
        {
            StringBuilder output = new StringBuilder();

            StringBuilder images = new StringBuilder();
            images.Append("\"data:image/png;base64," + input.PosX.ToStr() + "\", ");
            images.Append("\"data:image/png;base64," + input.NegX.ToStr() + "\", ");
            images.Append("\"data:image/png;base64," + input.PosY.ToStr() + "\", ");
            images.Append("\"data:image/png;base64," + input.NegY.ToStr() + "\", ");
            images.Append("\"data:image/png;base64," + input.PosZ.ToStr() + "\", ");
            images.Append("\"data:image/png;base64," + input.NegZ.ToStr() + "\"");

            output.AppendLine("let lightProbe = new THREE.LightProbe();");
            output.AppendLine("scene.add( lightProbe );");

            output.AppendLine("new THREE.CubeTextureLoader().load([" + images.ToString() + "], function ( cubeTexture ) {");
            output.AppendLine("cubeTexture.encoding = THREE.sRGBEncoding;");

            output.AppendLine("lightProbe.copy( THREE.LightProbeGenerator.fromCubeTexture( cubeTexture ) );");
            output.AppendLine("});");


            return output.ToString();
        }

        public static string ToLightProbeClose(this Environment input)
        {
            StringBuilder output = new StringBuilder();
            if (input.EnvironmentMode == Environment.EnvironmentModes.CubeMap)
            {
                if(input.IsIllumination) output.AppendLine("lightProbe.intensity = "+Math.Round(input.CubeMap.Intensity,5)+";");
            }
            return output.ToString();
        }

        public static string ToJavascript(this Environment input,string path, bool assets = false)
        {
            StringBuilder output = new StringBuilder();

            output.AppendLine("scene.background = new THREE.Color(" + input.Background.ToStr() + ");");
            switch (input.EnvironmentMode)
            {
                default:
                    break;
                case Environment.EnvironmentModes.Environment:
                    string envMap = "";
                    if (assets) envMap = "./assets/"+ input.EnvMap.SavePng(path, "envMapImg");
                    if(!assets)envMap = "data:image / png; base64," + input.EnvMap.ToStr(); 
                    output.AppendLine("var envMap = new THREE.TextureLoader().load(\""+ envMap + "\");");

                    output.AppendLine("envMap.mapping = THREE.EquirectangularReflectionMapping;");

                    if (input.IsBackground) output.AppendLine("scene.background = envMap;");
                    if (input.IsEnvironment) output.AppendLine("scene.environment = envMap;");

                    break;
                case Environment.EnvironmentModes.CubeMap:
                    StringBuilder images = new StringBuilder();
                    if (assets)
                    {
                        images.Append("\"./assets/" + input.CubeMap.PosX.SavePng(path, "CubeMapPosX") + "\", ");
                        images.Append("\"./assets/" + input.CubeMap.NegX.SavePng(path, "CubeMapNegX") + "\", ");
                        images.Append("\"./assets/" + input.CubeMap.PosY.SavePng(path, "CubeMapPosY") + "\", ");
                        images.Append("\"./assets/" + input.CubeMap.NegY.SavePng(path, "CubeMapNegY") + "\", ");
                        images.Append("\"./assets/" + input.CubeMap.PosZ.SavePng(path, "CubeMapPosZ") + "\", ");
                        images.Append("\"./assets/" + input.CubeMap.NegZ.SavePng(path, "CubeMapNegZ") + "\"");
                    }
                    else
                    {
                    images.Append("\"data:image/png;base64," + input.CubeMap.PosX.ToStr() + "\", ");
                    images.Append("\"data:image/png;base64," + input.CubeMap.NegX.ToStr() + "\", ");
                    images.Append("\"data:image/png;base64," + input.CubeMap.PosY.ToStr() + "\", ");
                    images.Append("\"data:image/png;base64," + input.CubeMap.NegY.ToStr() + "\", ");
                    images.Append("\"data:image/png;base64," + input.CubeMap.PosZ.ToStr() + "\", ");
                    images.Append("\"data:image/png;base64," + input.CubeMap.NegZ.ToStr() + "\"");
                    }

                    output.AppendLine("let lightProbe = new THREE.LightProbe();");
                    output.AppendLine("scene.add( lightProbe );");

                    output.AppendLine("new THREE.CubeTextureLoader().load([" + images.ToString() + "], function ( envMap ) {");
                    output.AppendLine("envMap.encoding = THREE.sRGBEncoding;");

                    output.AppendLine("lightProbe.copy( THREE.LightProbeGenerator.fromCubeTexture( envMap ) );");

                    if (input.IsBackground) output.AppendLine("scene.background = envMap;");
                    if (input.IsEnvironment) output.AppendLine("scene.environment = envMap;");

                    output.AppendLine("});");
                    break;
            }


            return output.ToString();
        }

        public static string ToJavascript(this TangentDisplay input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.Show)
            {
                output.AppendLine("const tanHelper" + index + " = new THREE.VertexTangentsHelper(model" + index + "," + input.Width + "," + input.Color.ToStr() + "," + input.Width + ");");
                output.AppendLine("scene.add(tanHelper" + index + ");");
            }

            return output.ToString();
        }

        public static string ToJsEdges(this Model input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.HasEdges)
            {
                output.AppendLine("const edges"+index+ " = new THREE.EdgesGeometry(mesh" + index + ", "+ input.EdgeThreshold+");");
                output.AppendLine("const edge" + index + " = new THREE.LineSegments(edges" + index + ", new THREE.LineBasicMaterial({ color:" + input.Graphic.Color.ToStr() + "}));");
                output.AppendLine("scene.add(edge" + index + ");");
            }

            return output.ToString();
        }

        public static string ToJavascript(this NormalDisplay input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.Show)
            {
                output.AppendLine("const nrmHelper" + index + " = new THREE.VertexNormalsHelper(model" + index + "," + input.Width + "," + input.Color.ToStr() + "," + input.Width + ");");
                output.AppendLine("scene.add(nrmHelper" + index + ");");
            }

            return output.ToString();
        }

        public static string ToJavascript(this Sky input, double scale = 500000)
        {
            StringBuilder output = new StringBuilder();

            if (input.HasSky)
            {
            output.AppendLine("const sky = new THREE.Sky();");
            output.AppendLine("const sun = new THREE.Vector3();");
            output.AppendLine("sky.scale.setScalar("+Math.Ceiling(scale)+");");
            output.AppendLine("scene.add(sky);");
            output.AppendLine("const uniforms = sky.material.uniforms;");
            output.AppendLine("uniforms['turbidity'].value = " + input.Turbidity + ";");
            output.AppendLine("uniforms['rayleigh'].value = " + input.Rayleigh + ";");
            output.AppendLine("uniforms['mieCoefficient'].value = " + input.Coefficient + ";");
            output.AppendLine("uniforms['mieDirectionalG'].value = " + input.Directional + ";");

            output.AppendLine("const phi = THREE.MathUtils.degToRad(90 - " + input.Altitude + ");");
            output.AppendLine("const theta = THREE.MathUtils.degToRad(" + input.Azimuth + ");");
            output.AppendLine("sun.setFromSphericalCoords(1, phi, theta);");

            output.AppendLine("uniforms['sunPosition'].value.copy(sun);");

            output.AppendLine("renderer.toneMapping = THREE.ACESFilmicToneMapping;");
            output.AppendLine("renderer.toneMappingExposure = " + Math.Round(input.Exposure, 5) + ";");

            if (input.Environment)
            {
                output.AppendLine("const pmremGenerator = new THREE.PMREMGenerator(renderer);");
                output.AppendLine("scene.environment = pmremGenerator.fromScene(sky).texture;");
            }
                //output.AppendLine("const sunlight = new THREE.DirectionalLight("+input.SunColor.ToStr()+", 1.0);");
                //output.AppendLine("sunlight.position.set(sun.x, sun.y, sun.z);");
                //output.AppendLine("scene.add(sunlight);");

            }

            return output.ToString();
        }

        public static string ToJavascript(this Axes input)
        {
            StringBuilder output = new StringBuilder();

            if (input.Show)
            {
                output.AppendLine("const axes = new THREE.AxesHelper(" + input.Scale + ");");
                output.AppendLine("axes.setColors ( "+input.YAxis.ToStr() +", "+input.ZAxis.ToStr() + ", "+input.XAxis.ToStr()+" );");
                output.AppendLine("scene.add(axes);");
            }

            return output.ToString();
        }

        public static string ToJavascript(this Grid input)
        {
            StringBuilder output = new StringBuilder();

            if (input.Show)
            {
                if (input.IsPolar)
                {
                    output.AppendLine("var grid = new THREE.PolarGridHelper(" + input.Size + ", " + input.Divisions + ", " + input.Divisions + ", 64, " + input.AxisColor.ToStr() + ", " + input.GridColor.ToStr() + ");");
                }
                else
                {
                    output.AppendLine("var grid = new THREE.GridHelper(" + input.Size + "," + input.Divisions + "," + input.AxisColor.ToStr() + "," + input.GridColor.ToStr() + ");");
                }
                output.AppendLine("scene.add(grid);");
            }

            return output.ToString();
        }

        public static string ToJavascript(this Camera input)
        {
            StringBuilder output = new StringBuilder();

            if (input.IsOrthographic)
            {
                output.AppendLine("const camera = new THREE.OrthographicCamera( window.innerWidth / -2, window.innerWidth / 2, window.innerHeight / -2, window.innerHeight / 2 , " + input.Near + "," + input.Far + ");");
            }
            else
            {
                output.AppendLine("const camera = new THREE.PerspectiveCamera(" + input.FOV + ", window.innerWidth / window.innerHeight, " + input.Near + "," + input.Far + ");");
            }

            if(input.IsAnimated)
            {
                output.AppendLine("camera.position.set(" + input.Tweens[0].From.ToStr() + ");");
                output.AppendLine("camera.lookAt (new THREE.Vector3(" + input.Tweens[0].To.X + "," + input.Tweens[0].To.Z + "," + input.Tweens[0].To.Y + "));");

                List<string> ptA = new List<string>();
                List<string> ptB = new List<string>();
                foreach (Rg.Line line in input.Tweens)
                {
                    ptA.Add("[" + line.To.ToStrArray() + "]");
                    ptB.Add("[" + line.From.ToStrArray() + "]");
                }

                output.AppendLine("const tweenA = [" + String.Join(",", ptA) + "];");
                output.AppendLine("const tweenB = [" + String.Join(",", ptB) + "];");
            }
            else
            {
            output.AppendLine("camera.position.set(" + input.Position.ToStr() + ");");
            output.AppendLine("camera.lookAt (new THREE.Vector3(" + input.Target.X + "," + input.Target.Z + "," + input.Target.Y + "));");
            }

            return output.ToString();
        }

        public static string ToJavascript(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();
            string starter = "const material" + index + " = new THREE.";
            switch (input.MaterialType)
            {
                case Material.Types.Basic:
                    output.AppendLine(starter + "MeshBasicMaterial( { "
                        + input.ToJsDiffuse(index)
                        + input.ToJsOpacity(index)
                        + " } );");
                    break;
                case Material.Types.Lambert:
                    output.AppendLine(starter + "MeshLambertMaterial( { "
                        + input.ToJsDiffuse(index)
                        + input.ToJsEmissive(index)
                        + input.ToJsOpacity(index)
                        + " } );");
                    break;
                case Material.Types.Phong:
                    output.AppendLine(starter + "MeshPhongMaterial( { "
                        + input.ToJsDiffuse(index)
                        + ", shininess: " + (input.Shininess * 100.0)
                        + input.ToJsBump(index)
                        + input.ToJsDisplacement(index)
                        + input.ToJsEmissive(index)
                        + input.ToJsNormal(index)
                        + input.ToJsOpacity(index)
                        + " } );");
                    break;
                case Material.Types.Standard:
                    output.AppendLine(starter + "MeshStandardMaterial( { "
                        + input.ToJsDiffuse(index)
                        + input.ToJsBump(index)
                        + input.ToJsDisplacement(index)
                        + input.ToJsEmissive(index)
                        + input.ToJsAmbient(index)
                        + input.ToJsMetalness(index)
                        + input.ToJsNormal(index)
                        + input.ToJsRoughness(index)
                        + input.ToJsOpacity(index)
                        + " } );");
                    break;
                case Material.Types.Physical:
                    output.AppendLine(starter + "MeshPhysicalMaterial( { "
                        + input.ToJsDiffuse(index)
                        + input.ToJsBump(index)
                        + input.ToJsClearcoat(index)
                        + input.ToJsDisplacement(index)
                        + input.ToJsEmissive(index)
                        + input.ToJsAmbient(index)
                        + input.ToJsMetalness(index)
                        + input.ToJsBump(index)
                        + input.ToJsRoughness(index)
                        + input.ToJsSheen(index)
                        + input.ToJsOpacity(index)
                        + ", reflectivity: " + (input.Reflectivity)
                        + " } );");
                    break;
                case Material.Types.Toon:
                    output.AppendLine("const gradSer" + index + " = new Uint8Array( [" + String.Join(",", input.Steps) + "] );");
                    output.AppendLine("const gradMap" + index + " = new THREE.DataTexture( gradSer" + index + ", gradSer" + index + ".length, 1, gradFormat );");
                    output.AppendLine("gradMap" + index + ".needsUpdate = true;");
                    output.AppendLine(starter + "MeshToonMaterial( { "
                        + input.ToJsDiffuse(index)
                        + input.ToJsBump(index)
                        + input.ToJsDisplacement(index)
                        + input.ToJsEmissive(index)
                        + input.ToJsNormal(index)
                        + input.ToJsOpacity(index)
                        + ",  gradientMap:gradMap" + index
                        + " } );");
                    break;
                case Material.Types.Shadow:
                    output.AppendLine(starter + "ShadowMaterial( { transparent:true, color: " + input.DiffuseColor.ToStr() + " } );");
                    break;
                case Material.Types.Normal:
                    output.AppendLine(starter + "MeshNormalMaterial();");
                    break;
                case Material.Types.Depth:
                    output.AppendLine(starter + "MeshDepthMaterial();");
                    break;
            }

            if (input.IsWireframe) output.AppendLine("material" + index + ".wireframe = true;");
            if (input.IsFlatShaded) output.AppendLine("material" + index + ".flatShading = true;");

            return output.ToString();
        }

        public static string ToJsClearcoat(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.HasClearcoat) output.Append(", clearcoat : " + input.Clearcoat);
            if (input.HasClearcoatMap) output.Append(", clearcoatMap : " + input.ClearCoatMapName);
            if (input.HasClearcoatRoughness) output.Append(", clearcoatRoughness : " + input.ClearcoatRoughness);
            if (input.HasClearcoatRoughnessMap) output.Append(", clearcoatRoughnessMap : " + input.ClearCoatRoughnessMapName);
            if (input.HasClearcoatNormalMap) output.Append(", clearcoatNormalMap : " + input.ClearCoatNormalMapName);

            return output.ToString();
        }

        public static string ToJsBump(this Material input, string index)
        {
            if (input.HasBumpMap) return ", bumpMap : " + input.BumpMapName + ", bumpScale : " + input.BumpIntensity;
            return string.Empty;
        }

        public static string ToJsDiffuse(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            output.Append("color: " + input.DiffuseColor.ToStr());
            //output.Append(", side:THREE.DoubleSide");
            if (input.HasTextureMap) output.Append(", map : " + input.TextureMapName);

            return output.ToString();
        }

        public static string ToJsDisplacement(this Material input, string index)
        {
            if (input.HasDisplacement) return ", displacementMap : " + input.DisplacementMapName + ", displacementScale : " + input.DisplacementScale;
            return string.Empty;
        }

        public static string ToJsMetalness(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.IsMetalic) output.Append(", metalness : " + input.Metalness);
            if (input.HasMetalicMap) output.Append(", metalnessMap : " + input.MetalnessMapName);

            return output.ToString();
        }

        public static string ToJsNormal(this Material input, string index)
        {
            if (input.HasNormalMap) return ", normalMap : " + input.NormalMapName + ", normalScale : " + input.NormalScale + ", normalMapType : THREE.ObjectSpaceNormalMap";
            return string.Empty;
        }

        public static string ToJsRoughness(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.HasRoughness) output.Append(", roughness : " + input.Roughness);
            if (input.HasRoughnessMap) output.Append(", roughnessMap : " + input.RoughnessMapName);

            return output.ToString();
        }

        public static string ToJsSheen(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.HasSheen) output.Append(", sheen : " + input.Sheen + ", sheenColor : " + input.SheenColor.ToStr());
            if (input.HasSheenMap) output.Append(", sheenColorMap : " + input.SheenMapName);
            if (input.HasSheenRoughness) output.Append(", sheenRoughness : " + input.SheenRoughness);
            if (input.HasSheenRoughnessMap) output.Append(", sheenRoughnessMap : " + input.SheenRoughnessMapName);

            return output.ToString();
        }

        public static string ToJsEmissive(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.IsEmissive) output.Append(", emissive : " + input.EmissiveColor.ToStr() + ", emissiveIntensity : " + input.EmissiveIntensity);
            if (input.HasEmissiveMap) output.Append(", emissiveMap : " + input.EmissiveMapName);

            return output.ToString();
        }

        public static string ToJsAmbient(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            if (input.HasAmbientOcclusionMap) output.Append(", aoMapIntensity : " + input.AmbientOcclusion + ", aoMap : " + input.AmbientOcclusionMapName);

            return output.ToString();
        }

        public static string ToJsOpacity(this Material input, string index)
        {
            StringBuilder output = new StringBuilder();

            output.Append(", opacity : " + input.DiffuseColor.ToStrOpacity());
            if (input.HasOpacityMap) output.Append(", alphaMap : " + input.OpacityMapName);
            output.Append(", transparent: " + input.IsTransparent.ToString().ToLower());
            if(input.HasOpacityIor) output.Append(", ior: " + input.OpacityIor);
            if (input.HasOpacityIorMap) output.Append(", transmission : " + input.Transmission);
            if (input.HasOpacityIorMap) output.Append(", transmissionMap : " + input.TransmissionMapName);


            return output.ToString();
        }

        public static string ToJavascript(this Light input, string index)
        {
            StringBuilder output = new StringBuilder();
            string name = "light" + index;
            string starter = "const " + name + " = new THREE.";
            string hName = "h" + name;
            string helperStart = "const " + hName + " = new THREE.";
            switch (input.LightType)
            {
                default:
                    output.AppendLine(starter + "AmbientLight(" + input.Color.ToStr() + ", " + input.Intensity + " );");
                    break;
                case Light.Types.Point:
                    output.AppendLine(starter + "PointLight(" + input.Color.ToStr() + ", " + input.Intensity + ", " + input.Distance + ", " + input.Decay + " );");
                    output.AppendLine(name + ".position.set( " + input.Position.ToStr() + " );");
                    if (input.HasHelper) output.AppendLine(helperStart + "PointLightHelper(" + name + ", " + input.HelperSize + ", " + input.HelperColor.ToStr() + " );");
                    break;
                case Light.Types.Directional:
                    output.AppendLine(starter + "DirectionalLight(" + input.Color.ToStr() + ", " + input.Intensity + " );");
                    output.AppendLine(name + ".position.set( " + input.Position.ToStr() + " );");
                    output.AppendLine(input.Target.ToJsTarget(name));
                    if (input.HasHelper) output.AppendLine(helperStart + "DirectionalLightHelper(" + name + ", " + input.HelperSize + ", " + input.HelperColor.ToStr() + " );");
                    break;
                case Light.Types.Hemisphere:
                    output.AppendLine(starter + "HemisphereLight(" + input.Color.ToStr() + ", " + input.ColorB.ToStr() + ", " + input.Intensity + " );");
                    output.AppendLine(name + ".position.set(0,1,0);");
                    if (input.HasHelper) output.AppendLine(helperStart + "HemisphereLightHelper(" + name + ", " + input.HelperSize + ", " + input.HelperColor.ToStr() + " );");
                    break;
                case Light.Types.Spot:
                    output.AppendLine(starter + "SpotLight(" + input.Color.ToStr() + ", " + input.Intensity + ", " + input.Distance + ", " + input.Angle + ", " + input.Penumbra + ", " + input.Decay + " );");
                    output.AppendLine(name + ".position.set( " + input.Position.ToStr() + " );");
                    output.AppendLine(input.Target.ToJsTarget(name));
                    if (input.HasHelper) output.AppendLine(helperStart + "SpotLightHelper(" + name + ", " + input.HelperColor.ToStr() + " );");
                    break;
            }
            if ((input.HasHelper) &(input.LightType!= Light.Types.Ambient)) output.AppendLine("scene.add(" + hName + ");");
            return output.ToString();
        }

        public static string ToJavascript(this Rg.Plane input, double size, string index)
        {

            StringBuilder output = new StringBuilder();

            string name = "model" + index;
            output.AppendLine("var " + name + " = new THREE.Mesh(new THREE.PlaneBufferGeometry(" + size + "," + size + "), material" + index + ");");
            output.AppendLine(name + ".position.y = " + input.Origin.Z + ";");
            output.AppendLine(name + ".rotation.x = - Math.PI / 2;");
            output.AppendLine(name + ".receiveShadow = true;");
            output.AppendLine("scene.add(" + name + ");");

            return output.ToString();
        }

        public static string ToJavascript(this PointCloud input, string index)
        {
            StringBuilder output = new StringBuilder();
            string name = "cloud" + index;

            output.AppendLine("const " + name + " = new THREE.BufferGeometry();");

            int count = input.Points.Count;
            var points = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count);
            var colors = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count);

            Parallel.For(0, count, k =>
            {
                points[k] = input.Points[k].ToStr();
                colors[k] = input.Colors[k].ToStrArray();
            }
            );

            output.AppendLine("const positions" + index + " = new Float32Array( [" + string.Join(",", points.Values) + "] );");
            output.AppendLine("const colors" + index + " = new Float32Array( [" + string.Join(",", colors.Values) + "] );");
            //if(input.IsSprite)output.AppendLine("const scales" + index + " = new Float32Array( [" + string.Join(",", input.Scales) + "] );");
            
            output.AppendLine(name + ".setAttribute( 'position', new THREE.BufferAttribute( positions" + index + ", 3 ) );");
            output.AppendLine(name + ".setAttribute( 'color', new THREE.BufferAttribute( colors" + index + ", 3 ) );");
            //if (input.IsSprite) output.AppendLine(name + ".setAttribute( 'scale', new THREE.BufferAttribute( scales" + index + ", 1 ) );");

                output.Append("const material" + index + " = new THREE.PointsMaterial( { size: " + Math.Round(input.Scale, 5) + ", depthTest: true, transparent: true, color: 0xffffff, vertexColors: true ");
                if (input.HasBitmap) output.Append(", map: "+ input.MapName + ", alphaMap: " + input.MapName+ ", alphaTest:"+Math.Round(input.Threshold,5));
                    output.AppendLine(" } );");
                output.AppendLine("const model" + index + " = new THREE.Points( " + name + ", material" + index + " );");

            return output.ToString();
        }

        public static string ToJavascript(this Rg.NurbsCurve input, string index, Graphic graphic)
        {
            StringBuilder output = new StringBuilder();
            string name = "curve" + index;

            output.AppendLine("const " + name + " = new THREE.LineGeometry();");

            int count = input.Points.Count;
            var points = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count);
            var colors = new System.Collections.Concurrent.ConcurrentDictionary<int, string>(System.Environment.ProcessorCount, count);

            Parallel.For(0, count, k =>
            {
                points[k] = input.Points[k].Location.ToStr();
            }
            );
            output.AppendLine("const positions" + index + " = new Float32Array( [" + string.Join(",", points.Values) + "] );");
            output.AppendLine(name + ".setPositions( positions" + index + ");");

            output.Append("const lineMat" + index + " = new THREE.LineMaterial({ linewidth: " + Math.Round(graphic.Width, 5) + ", alphaToCoverage: true");


            if (graphic.HasColors)
            {
                output.Append(", color: 0xffffff , vertexColors: true ");
            }
            else
            {
                output.Append(", color: " + graphic.Color.ToStr()+ ", vertexColors: false");
            }

            if (graphic.DashLength > 0)
            {
                output.Append(", dashed: true, dashSize: "+graphic.DashLength+", gapSize: "+graphic.GapLength);
            }
            else
            {
                output.Append(", dashed: false");
            }

            output.AppendLine(" });"); ;

            if (graphic.HasColors)
            {
                Parallel.For(0, count, k =>
            {
                int m = k % graphic.Colors.Count;
                colors[k] = graphic.Colors[m].ToStrArray();
            }
            );
                output.AppendLine("const colors" + index + " = new Float32Array( [" + string.Join(",", colors.Values) + "] );");
                output.AppendLine(name + ".setColors( colors" + index + " );");
            }

            output.AppendLine("const model"+index+" = new THREE.Line2( "+name+", lineMat" + index +" );");
            output.AppendLine("model" + index + ".computeLineDistances();");

            return output.ToString();
        }

        public static string ToJavascript(this Shape input, string index)
        {

            StringBuilder output = new StringBuilder();
            string shape = "const shape" + index + " = new THREE.";
            double adjust = 0;

            switch (input.ShapeType)
            {
                case Shape.ShapeTypes.Box:
                    shape += "BoxGeometry( " + input.SizeX + ", " + input.SizeY + ", " + input.SizeZ + ", " + input.DivisionsU + ", " + input.DivisionsV + ", " + input.DivisionsW + " );";
                    break;
                case Shape.ShapeTypes.Capsule:
                    shape += "CapsuleGeometry( " + input.SizeX + ", " + input.SizeY + ", 8, " + input.DivisionsU + " );";
                    break;
                case Shape.ShapeTypes.Cone:
                    shape += "ConeGeometry( " + input.SizeX + ", " + input.SizeY + ", " + input.DivisionsU + " );";
                    break;
                case Shape.ShapeTypes.Cylinder:
                    shape += "CylinderGeometry( " + input.SizeX + ", " + input.SizeX + ", " + input.SizeY + ", " + input.DivisionsU + " );";
                    break;
                case Shape.ShapeTypes.Dodecahedron:
                    shape += "DodecahedronGeometry( " + input.SizeX + " );";
                    break;
                case Shape.ShapeTypes.Icosahedron:
                    shape += "IcosahedronGeometry( " + input.SizeX + " );";
                    break;
                case Shape.ShapeTypes.Octahedron:
                    shape += "OctahedronGeometry( " + input.SizeX + " );";
                    break;
                case Shape.ShapeTypes.Plane:
                    shape += "PlaneGeometry( " + input.SizeX + ", " + input.SizeY + " );";
                    adjust = -Math.PI / 2;
                    break;
                case Shape.ShapeTypes.Circle:
                    shape += "CircleGeometry( " + input.SizeX + ", " + input.DivisionsU + " );";
                    adjust = -Math.PI / 2;
                    break;
                case Shape.ShapeTypes.Ring:
                    shape += "RingGeometry( " + input.SizeX + ", " + input.SizeY + ", " + input.DivisionsU + " );";
                    adjust = -Math.PI / 2;
                    break;
                case Shape.ShapeTypes.Sphere:
                    shape += "SphereGeometry( " + input.SizeX + ", " + input.DivisionsU + ", " + input.DivisionsV + " );";
                    adjust = -Math.PI / 2;
                    break;
                case Shape.ShapeTypes.Tetrahedron:
                    shape += "TetrahedronGeometry( " + input.SizeX + " );";
                    break;
                case Shape.ShapeTypes.Torus:
                    shape += "TorusGeometry( " + input.SizeX + ", " + input.SizeY + ", " + input.DivisionsU + ", " + input.DivisionsV + " );";
                    adjust = -Math.PI/2;
                    break;
                case Shape.ShapeTypes.TorusKnot:
                    shape += "TorusKnotGeometry( " + input.SizeX + ", " + input.SizeY + ", " + input.DivisionsU + ", " + input.DivisionsV + ", " + input.TurnsA + ", " + input.TurnsB + " );";
                    adjust = -Math.PI / 2;
                    break;
            }

            output.AppendLine(shape);
            output.AppendLine("const model" + index + " = new THREE.Mesh( shape" + index + ", material" + index + " );");

            output.AppendLine("model" + index + ".position.set( "+input.Plane.Origin.Y+", "+ input.Plane.Origin.Z + ", "+ input.Plane.Origin.X + " );");
            output.AppendLine("model" + index + ".rotation.set( " + adjust + ", " + 0 + ", " + 0 + " );");

            return output.ToString();
        }

        public static string ToJavascript(this Rg.Mesh input, string index)
        {
            input = input.DuplicateMesh();
            input.Faces.ConvertQuadsToTriangles();
            if (input.Normals.Count < 1) input.RebuildNormals();

            bool hasUV = (input.TextureCoordinates.Count > 0);
            bool hasColor = (input.VertexColors.Count == input.Vertices.Count);

            StringBuilder output = new StringBuilder();
            string name = "mesh" + index;

            output.AppendLine("const " + name + " = new THREE.BufferGeometry();");

            int count = input.Faces.Count;
            string vertices = string.Empty;
            string normals = string.Empty;
            string uvs = string.Empty;
            string colors = string.Empty;

            vertices = input.ToVertexString();
            normals = input.ToNormalString();
            if (hasUV) uvs = input.ToUvString();
            if (hasColor) colors = input.ToColorString();

            output.AppendLine("const vertices" + index + " = new Float32Array( [" + vertices + "] );");
            output.AppendLine("const normals" + index + " = new Float32Array( [" + normals + "] );");
            if (hasUV) output.AppendLine("const uv" + index + " = new Float32Array( [" + uvs + "] );");
            if (hasColor) output.AppendLine("const clr" + index + " = new Uint8Array( [" + colors + "] );");

            output.AppendLine(name + ".setAttribute( 'position', new THREE.BufferAttribute( vertices" + index + ", 3 ) );");
            output.AppendLine(name + ".setAttribute( 'normal', new THREE.BufferAttribute( normals" + index + ", 3 ) );");
            if (hasUV) output.AppendLine(name + ".setAttribute( 'uv', new THREE.BufferAttribute( uv" + index + ", 2 ) );");
            if (hasColor) output.AppendLine(name + ".setAttribute( 'color', new THREE.BufferAttribute( clr" + index + ", 3, true ) );");
            output.AppendLine("const model" + index + " = new THREE.Mesh( mesh" + index + ", material" + index + " );");
            return output.ToString();
        }

        #region conversions

        public static string ToJsTarget(this Rg.Point3d input, string name)
        {
            StringBuilder output = new StringBuilder();

            string targetObject = name + "T";
            output.AppendLine("const " + targetObject + "= new THREE.Object3D();");
            output.AppendLine(targetObject + ".position.set( " + input.ToStr() + " );");
            output.AppendLine("scene.add(" + targetObject + ");");
            output.AppendLine(name + ".target = " + targetObject + ";");
            output.AppendLine(name + ".target.updateMatrixWorld();");
            return output.ToString();
        }

        public static string ToJavascript(this Rg.Transform input, int digits = 5)
        {
            StringBuilder output = new StringBuilder();
            output.Append(Math.Round(input.M00, digits) + ",");
            output.Append(Math.Round(input.M01, digits) + ",");
            output.Append(Math.Round(input.M02, digits) + ",");
            output.Append(Math.Round(input.M03, digits) + ",");
            output.Append(Math.Round(input.M10, digits) + ",");
            output.Append(Math.Round(input.M11, digits) + ",");
            output.Append(Math.Round(input.M12, digits) + ",");
            output.Append(Math.Round(input.M13, digits) + ",");
            output.Append(Math.Round(input.M20, digits) + ",");
            output.Append(Math.Round(input.M21, digits) + ",");
            output.Append(Math.Round(input.M22, digits) + ",");
            output.Append(Math.Round(input.M23, digits) + ",");
            output.Append(Math.Round(input.M30, digits) + ",");
            output.Append(Math.Round(input.M31, digits) + ",");
            output.Append(Math.Round(input.M32, digits) + ",");
            output.Append(Math.Round(input.M33, digits));
            return output.ToString();
        }

        #endregion

    }
}
