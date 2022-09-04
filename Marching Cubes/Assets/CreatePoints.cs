using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePoints : MonoBehaviour
{

    //list of variables
    [Range(0f, 100)] public float surfaceRange;
    [Range(0f, 10000)] public float cubeRange;
    [Range(0, 10000)] public int renderRange;
    public bool renderCubes;
    public bool renderNodes = true;
    public static Vector3 center;
    public Vector3 size;
    private Vector3 corner;
    public float nodeSize;
    public GameObject nodePrefab;
    private NodeHandler nodeHandler;
    public CubeHandler cubeHandler;

    //references to other objects

    // Start is called before the first frame update
    void Start()
    {
        surfaceRange = 73;
        nodeHandler = new NodeHandler(CalculateNodeAmount(size, nodeSize)); //Allocates space to hold the nodes
        cubeHandler = new CubeHandler(CalculateCubesAmount());  //Allocates space to hold the cubes
        CalculateCorner(transform.position, size);  //Builds cubes from the negative-most corner of the building area
        CreateNodes(nodeSize);  //Process to create nodes
        CreateCubes(ref nodeHandler); //creates cubes from a nodehandler class, requires that nodes previously be made
    }

    //Draws a red cube representing the area in which nodes will spawn
    private void OnDrawGizmosSelected()
    {
        if (!renderCubes)
        {
            Gizmos.color = new Color(1, 0, 0, 0.5f);
            Gizmos.DrawCube(transform.position, size);
        }
    }

    private void OnDrawGizmos()
    {
        if (renderCubes && cubeHandler != null)
        {
            int i = 0;
            foreach (Cube cube in cubeHandler.GetList())
            {
                if (i < cubeRange)
                {
                    Gizmos.color = Color.black;
                    Gizmos.DrawWireCube(cube.getCenter(), new Vector3(nodeSize, nodeSize, nodeSize));
                    i++;
                }
  
            }
        }

    }

    //Calculates the corner of the shape with the lowest positional value
    public void CalculateCorner(Vector3 center, Vector3 size) //finds the position of the lowest cornor of the gizmo and sets corner to found position
    {
        float x = center.x - (size.x / 2f);
        float y = center.y - (size.y / 2f);
        float z = center.z - (size.z / 2f);

        corner = new Vector3(x, y, z);
    }

    public int CalculateNodeAmount(Vector3 size, float nodeSize) //Determines how many nodes will fit within the area
    {
        return (int)((size.x / nodeSize) + 1) * (int)((size.y / nodeSize) + 1) * (int)((size.z / nodeSize) + 1);
    }

    public int CalculateCubesAmount() //calculates the number of cubes that can fit into the shape
    {
        return (int)(size.x / nodeSize) * (int)(size.y / nodeSize) * (int)(size.z / nodeSize);
    }

    public void CreateNodes(float nodeSize) //builds along the z-axis first, then the y-axis, the x-axis
    {
        for (float x = corner.x; x <= size.x + corner.x; x += nodeSize)
        {
            for (float y = corner.y; y <= size.y + corner.y; y += nodeSize)
            {
                for (float z = corner.z; z <= size.z + corner.z; z += nodeSize)
                {
                    nodeHandler.AddNode(Instantiate(nodePrefab, new Vector3(x, y, z), new Quaternion(0, 0, 0, 0), this.transform), center); //passes the center in order to make the calculation for metaballs
                }
            }
        }
    }

    public void CreateCubes(ref NodeHandler nodeHandler)
    {

        for (int i = 0; i < nodeHandler.GetAmount() - (SizeToNumberOfNodes('y') * SizeToNumberOfNodes('z')); i++) //subtracts from size.z and size.y so that it doesn't go out of bounds
        {
            if (To3D(i, 'z') < SizeToNumberOfNodes('z') - 1 && To3D(i, 'y') < SizeToNumberOfNodes('y') - 1) //if the cube has not reached the end of the z and y axis
                cubeHandler.AddCube(GetEdges(ref nodeHandler, i));
        }
    }

    public int To3D(int position, char direction) //These convert coodinates on the 1D lists into 3D coordinates
    {
        switch (direction)
        {
            case 'x':
                return (int)(position / (SizeToNumberOfNodes('z') * SizeToNumberOfNodes('y')));
            case 'y':
                return (int)((position / SizeToNumberOfNodes('z')) % SizeToNumberOfNodes('y'));
            case 'z':
                return (int)(position % SizeToNumberOfNodes('z'));
            default:
                Debug.Log("ERROR: Invalid Entry");
                return -1;
        }
    }

    public int SizeToNumberOfNodes(char direction) //converts the size of a direction into the number of nodes in that direction
    {
        switch (direction)
        {
            case 'x':
                return (int)(size.x / nodeSize + 1);
            case 'y':
                return (int)(size.y / nodeSize + 1);
            case 'z':
                return (int)(size.z / nodeSize + 1);
            default:
                Debug.Log("ERROR: Invalid Entry");
                return -1;
        }

    }

    public Node[] GetEdges(ref NodeHandler nodeHandler, int node) //returns the array index of a node's neighbors
    {
        Node[] neighbors = new Node[8];
        neighbors[0] = nodeHandler.GetNode(node); //the node
        neighbors[1] = nodeHandler.GetNode(node + 1); //the neightbor directly next to the node along the Z-axis
        neighbors[4] = nodeHandler.GetNode(node + (int)(size.z / nodeSize) + 1); //the neighbor directly above the node
        neighbors[5] = nodeHandler.GetNode(node + (int)(size.z / nodeSize) + 2); //the neighbor diagonal to the node on the Z-axis
        neighbors[3] = nodeHandler.GetNode(node + ((int)(size.z / nodeSize) + 1) * ((int)(size.y / nodeSize) + 1)); //the neighbor directly to the next of the node on the X-axis
        neighbors[2] = nodeHandler.GetNode(node + ((int)(size.z / nodeSize) + 1) * ((int)(size.y / nodeSize) + 1) + 1); //the neighbor diagonally to the node on the x and z axis
        neighbors[7] = nodeHandler.GetNode(node + ((int)(size.z / nodeSize) + 1) * ((int)(size.y / nodeSize) + 1) + (int)(size.z / nodeSize) + 1); //the neighbor diagonally across from the node on the y and x axis
        neighbors[6] = nodeHandler.GetNode(node + ((int)(size.z / nodeSize) + 1) * ((int)(size.y / nodeSize) + 1) + (int)(size.z / nodeSize) + 2); //neighbor directly diagonally from the node

        return neighbors;



        //     5___________6
        //     |`          :\
        //     | `         : \
        //     |  `        :  \
        //     |   4-----------7
        //     |   :       :   :
        //     1__ : ______2   :
        //     `   :        \  :
        //      `  :         \ :
        //       ` :          \:
        //        `0___________3

        //     ______5______
        //     |`          :\
        //     | 4         : 6
        //     |  `        10 \
        //     9   -----7-------
        //     |   :       :   :
        //     |__ : _1____:   11
        //     `   8        \  :
        //      0  :         2 :
        //       ` :          \:
        //        `_____3______:

        //  <   ^
        //   \  |
        //    Z Y
        //     \|
        //      N - X - >
    }



}



//Class that stores a list of all the node objects
public class NodeHandler
{
    Node[] nodeList;
    int nodeListSize;
    public NodeHandler(int numberOfNodes)
    {
        nodeList = new Node[numberOfNodes];
        nodeListSize = 0;
    }

    public void AddNode(GameObject nodeObject, Vector3 center)
    {
        nodeList[nodeListSize++] = new Node(ref nodeObject, center);
    }

    public int GetAmount() //returns the number of nodes within array
    {
        return nodeListSize;
    }

    public Node GetNode(int position)
    {
        return nodeList[position];
    }
}

//Class that stores the game object of a node along with all it's values
public class Node //class of nodes
{
    public GameObject nodeObject;
    Vector3 position;
    public float surfaceLevel;

    public Node(ref GameObject nodeObjectReference, Vector3 center)
    {
        NodeProperties nodeScript = nodeObjectReference.GetComponent<NodeProperties>();
        surfaceLevel = CalculateMetaballs(nodeObjectReference.transform.position, center);
        nodeScript.SetSurfaceValue(surfaceLevel);
        nodeObject = nodeObjectReference;
        position = nodeObject.transform.position;
    }

    public float CalculateMetaballs(Vector3 position, Vector3 center) //returns the value that a node should be, creating a circular shape
    {
        //uses inverse-square law to decrease surface level the further out it goes
        float radius = 300; //the radius of the formed sphere
        return Mathf.Min(100, radius * (1 / Mathf.Sqrt(Mathf.Pow(position.x - center.x, 2) + Mathf.Pow(position.y - center.y, 2) + Mathf.Pow(position.z - center.z, 2))));
    }

    public Vector3 GetPosition()
    {
        return position;
    }
    public float GetX()
    {
        return position.x;
    }
    public float GetY()
    {
        return position.y;
    }
    public float GetZ()
    {
        return position.z;
    }

}

public class CubeHandler
{
    public Cube[] cubeList;
    public int cubeListAmount;

    public CubeHandler(int numberOfCubes)
    {
        cubeList = new Cube[numberOfCubes];
        cubeListAmount = 0;
    }

    public void AddCube(Node[] nodeList)
    {
        cubeList[cubeListAmount++] = new Cube(nodeList, cubeListAmount);
    }

    public Cube[] GetList()
    {
        return cubeList;
    }
}

public class Cube
{
    public Node[] vertices; //all the 8 vertices that a cube has
    public int vertLength;
    public Vector3 center; //the center of the cube
    public int index;

    Cube()
    {
        vertLength = 8;
        vertices = new Node[vertLength];
    }

    public Cube(Node[] nodeList, int indexArg)
    {
        index = indexArg;
        vertLength = 8;
        vertices = new Node[vertLength];
        if (nodeList.Length != vertLength) Debug.Log("ERROR: array incorrect size for cube class");

        nodeList.CopyTo(vertices, 0);
        CalculateCenter();
    }

    public Vector3 getCenter()
    {
        return center;
    }

    public void CalculateCenter() //calculates the center point by finding the average between two diagonal points across the rectangle
    {
        float x1 = vertices[0].GetX(),
        y1 = vertices[0].GetY(),
        z1 = vertices[0].GetZ();
        float x2 = vertices[6].GetX(),
        y2 = vertices[6].GetY(),
        z2 = vertices[6].GetZ();
        center = new Vector3((x1 + x2) / 2, (y1 + y2) / 2, (z1 + z2) / 2);
    }

}