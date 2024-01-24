# Marching-Cubes

Marching cubes is an algorithm used to create isosurfaces within a 3D field. It operates by creating a field of cubes where each individual vertice of the cube has a surface level number; determined by the desired mesh. Depending on the different value combinations within each vertice of a cube, a different triangular pattern can be used to represent that cube. Each cube is cycled until the entire field has been processed; thus creating a mesh. Two of the most common uses of the marching cubes algorithm is in MRI scan results and creating 3D terrain commonly used in video games.

This project was written in C# through Unity game engine.

![alt text](https://graphics.stanford.edu/~mdfisher/Images/MarchingCubesCases.png)

The marching cubes algorithm functions by employing 15 distinct configurations, each with its rotations, resulting in a total of 256 possible polygons available for visualizing any unique shape.

![alt text](https://github.com/JJGeither/Marching-Cubes/blob/main/MC_1.jpg)

This above image represents a matrix of cubes where each dot represents a vertice of a cube. The color value of the dot represents where to draw the object.

![alt text](https://github.com/JJGeither/Marching-Cubes/blob/main/MC_2.jpg)

A threshold is applied to isolate the desired vertices.

![alt text](https://github.com/JJGeither/Marching-Cubes/blob/main/MC_3.jpg)

The algorithm then takes these vertices and creates a mesh from these points.

![alt text](https://github.com/JJGeither/Marching-Cubes/blob/main/MC_4.jpg)

This can be used to create any desired shape by simply increasing the value of the nodes.

![alt text](https://www.researchgate.net/publication/283259761/figure/fig4/AS:614127911313411@1523430912166/3D-segment-visualization-and-isosurface-rendering-using-marching-cube-in-YaDiV.png)

Here is an example of the algorithm being used for MRI scans.

Resources used:
http://paulbourke.net/geometry/polygonise/
https://people.eecs.berkeley.edu/~jrs/meshpapers/LorensenCline.pdf
https://www.youtube.com/watch?v=M3iI2l0ltbE&ab_channel=SebastianLague
