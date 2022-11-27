# Three +
Three.js scene compiler for Grasshopper 3d

---

This plugin was developed to allow for the creation of visually rich scenes for model representation in a ready-to-view format. Scene construction is based on merging Geometry, Lights, Scene Elements, Cameras, and other Helpers and Post Processing elements into the Scene component. The Scene object output from this component can then be written to a Page or JSON output. Building up elements for a Scene can be done in the following categories.

Exports an HTML index page and associated app.js scene to your desktop, which can be opened locally in a browser even with no internet connection. This static site can also be shared online via Amazon S3.

[Download the Plugin at Food4Rhino](https://www.food4rhino.com/en/app/three)


![definition](https://user-images.githubusercontent.com/25797596/159863476-469e0dad-76a9-4b71-b3b8-c39fe8cf5ac8.png)


### Object Scene JSON

Export an Object Scene as JSON (v.4) to load the scene into online viewers such as the Threejs Editor. There are currently limitations to the current exports for material maps, lights etc. as this is an alpha release of the plugin.
Local Webpage

### Materials

Geometry can either be directly added to a scene or have a material applied to control its appearance. The Materials tab contains a range of modifiers that range from a basic color to a PBR material and also include analytic materials such as normal directions. Meshes and Surfaces/Polysurfaces (which are converted to meshes) can have all standard Three.js materials applied to them and can have additional mapping layers added such as bump, opacity, clearcoat, sheen, etc. using the mapping components. Additionally, modifiers such as wireframes and outlines can be applied to alter the appearance. For Point geometry, point cloud colors and symbols can be specified. And for Curves (which will be converted into polylines) Graphics effects such as color or point colors, weight, and dashes can be applied. 

### Lights

Lights can be created using basic geometry inputs and parameters matching the particulars of the light type. Shadows can be applied to any light, but only lights that support shadows will actually apply them. At this time, the plugin is limited to 16 lights.

### Cameras

Cameras can optionally be added to a scene. If a camera isn't provided one is positioned based on the bounding box. Cameras can either be in Perspective or Orthographic projection and can also be animated by setting a series of linear positions which will be animated through. Multiple cameras can be added to the JSON file, but the page exporter is limited to just one, the last camera added. This may change in figure versions.

### Rhino Document

Three plus also supports direct referencing of geometry, lights, and cameras from the Rhino document. Geometry which is referenced will also map the materials or graphics and lights will pick up all applicable characteristics. Cameras can be specified from saved views and will provide a best-match camera. There are an additional set of utility components for deconstructing Materials as PBR Materials into their channels. 

### Helpers

A series of scene helper elements can also be applied to highlight the Bounds of geometry, Normals for meshes, Light object representations, or grids and guides for better scene orientation.

### Assets

There are a series of packaged bitmaps available for the easy creation of environment maps for scene reflections/backgrounds and Cube maps for lighting. For components that work with these assets, any System.Drawing.Bitmap can be used or Cube Maps can be constructed using a provided component. Additionally, some sample icons (or symbols) are provided for point cloud mapping. 

Public Domain Cube maps sourced from http://www.humus.name/

Pubic Domain Environment maps sourced from https://polyhaven.com/

### Scene

In addition to the Scene compilation component. This section includes several Scene Modifiers which can be merged into the Scene Component adding a Ground Plane, Environment / Background Maps, Sky System, Fog, and Post Processed effects like Ambient Occlusion and Outlining. These Scene Modifiers are not currently exported to the JSON format. 


## Learn More

[Plugin Documentation](https://interopxyz.gitbook.io/three-plus/)

### Dependencies
 - [Rhinoceros 3d](https://www.rhino3d.com/)
 - [Rhinocommon](https://www.nuget.org/packages/RhinoCommon/5.12.50810.13095)
 - [Three.Js](https://threejs.org/)
 - [jdDelivr](https://cdn.jsdelivr.net/npm/three@0.136.0/)
