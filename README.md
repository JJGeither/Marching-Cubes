# Marching-Cubes

Marching cubes is an algorithm used to create isosurfaces within a 3D field. It operates by creating a field of cubes where each individual vertice of the cube has a surface level number; determined by the type of desired mesh. Depending on the different value combinations within each vertice of a cube, a different triangular pattern can be used to represent that cube. Each cube is cycled until the entire field has been processed; thus creating a mesh. Two of the most common uses of the marching cubes algorithm is in MRI scan results and creating 3D terrain commonly used in video games.

This project was written in C# through Unity game engine.

Resources used:
http://paulbourke.net/geometry/polygonise/
https://people.eecs.berkeley.edu/~jrs/meshpapers/LorensenCline.pdf
https://www.youtube.com/watch?v=M3iI2l0ltbE&ab_channel=SebastianLague
