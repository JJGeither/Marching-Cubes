# Marching Cubes Algorithm in Unity

## Overview
This project implements the Marching Cubes algorithm in C# using the Unity game engine. The algorithm is used to generate isosurfaces within a 3D scalar field by processing individual cubes, determining their surface level values, and constructing a mesh based on these values. The implementation allows for dynamic terrain generation and smooth 3D surface rendering.

## How It Works
The Marching Cubes algorithm operates on a 3D grid where each cube contains vertices assigned scalar values. These values determine whether a vertex is inside or outside the isosurface. The algorithm follows these steps:

1. **Grid Initialization**  
   - A 3D array stores scalar values at each grid point.
   - Each cube consists of eight vertices, each with a scalar value representing its density.

2. **Thresholding and Binary Indexing**  
   - A user-defined threshold determines which vertices are inside or outside the isosurface.
   - A unique 8-bit index is generated for each cube based on which of its vertices exceed the threshold.

3. **Lookup Table for Triangle Configurations**  
   - The algorithm references a **precomputed edge table** and **triangulation table** to determine how to connect vertices.
   - These tables define the arrangement of triangles for each possible cube configuration.

4. **Vertex Interpolation**  
   - To improve accuracy, the exact intersection points along cube edges are interpolated between vertices.
   - Linear interpolation is used to approximate the position where the surface should pass through an edge.

5. **Mesh Generation**  
   - Interpolated vertices are stored in a `List<Vector3>`.
   - Triangles are defined in a `List<int>` to create the final mesh structure.
   - The `Mesh` class in Unity is used to dynamically generate and update the geometry.

6. **Rendering in Unity**  
   - The `MeshFilter` component updates the generated mesh in real-time.
   - The `MeshCollider` component is optionally added for physics interactions.
   - GPU-based optimizations can be applied for better performance.

## Cube Configurations

The Marching Cubes algorithm functions by employing 15 distinct configurations, each with its rotations, resulting in a total of 256 possible polygons available for visualizing any unique shape.

![Marching Cubes Configurations](https://graphics.stanford.edu/~mdfisher/Images/MarchingCubesCases.png)

## Step-by-Step Visualization

1. **Grid Representation**  
   This image represents a matrix of cubes where each dot represents a vertex of a cube. The color value of the dot represents where to draw the object.  
   ![Grid Representation](https://github.com/JJGeither/Marching-Cubes/blob/main/MC_1.jpg)

2. **Threshold Application**  
   A threshold is applied to isolate the desired vertices.  
   ![Threshold Application](https://github.com/JJGeither/Marching-Cubes/blob/main/MC_2.jpg)

3. **Mesh Creation**  
   The algorithm then takes these vertices and creates a mesh from these points.  
   ![Mesh Creation](https://github.com/JJGeither/Marching-Cubes/blob/main/MC_3.jpg)

4. **Final Mesh Output**  
   The completed mesh can represent complex 3D surfaces.  
   ![Final Mesh Output](https://github.com/JJGeither/Marching-Cubes/blob/main/MC_4.jpg)

## Applications
- **Medical Imaging**: Used in MRI scan visualizations to create 3D representations of internal structures.
- **Terrain Generation**: Commonly used in game development to generate smooth 3D landscapes.
- **Scientific Simulations**: Used in physics and fluid simulations to visualize complex surfaces.

## References
- [Paul Bourke - Polygonising a Scalar Field](http://paulbourke.net/geometry/polygonise/)
- [Lorensen and Cline - Original Marching Cubes Paper](https://people.eecs.berkeley.edu/~jrs/meshpapers/LorensenCline.pdf)
- [Sebastian Lague - Marching Cubes Explained](https://www.youtube.com/watch?v=M3iI2l0ltbE&ab_channel=SebastianLague)

