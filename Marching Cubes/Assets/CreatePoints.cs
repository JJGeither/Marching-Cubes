using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatePoints : MonoBehaviour
{


    //list of variables
    [Range(0f, 100f)] public float SurfaceRange;
    public static Vector3 center;
    public Vector3 size;
    private Vector3 corner;
    public float nodeSize;
    public GameObject nodePrefab;
    private nodeHandler nodeParentClass;

    //references to other objects

    // Start is called before the first frame update
    void Start()
    {
        nodeParentClass = new nodeHandler(calculateNodeAmount(size, nodeSize));
        calculateCorner(transform.position, size);
        createNodes(nodeSize);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //Draws a red cube representing the area in which nodes will spawn
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(transform.position, size);
    }

    //Determines how many nodes will fit within the area
    public int calculateNodeAmount(Vector3 size, float nodeSize)
    {
        return Mathf.CeilToInt((size.x * size.y * size.z) / nodeSize);
    }

    //Calculates the corner of the shape with the lowest positional value
    public void calculateCorner(Vector3 center, Vector3 size) //finds the position of the lowest cornor of the gizmo and sets corner to found position
    {
        float x = center.x - (size.x/2f);
        float y = center.y - (size.y/2f);
        float z = center.z - (size.z/2f);

        corner = new Vector3(x, y, z);
    }

    public void createNodes(float nodeSize)
    {
        int nodeArrayPositionCounter = 0;
        for (float x = corner.x; x <= size.x + corner.x; x += nodeSize)
        {
            for (float y = corner.y; y <= size.y + corner.y; y += nodeSize)
            {
                for (float z = corner.z; z <= size.z + corner.z; z += nodeSize)
                {
                    nodeParentClass.AddNode(++nodeArrayPositionCounter, Instantiate(nodePrefab, new Vector3(x, y, z), new Quaternion(0,0,0,0), this.transform), center);    //passes the center in order to make the calculation for metaballs
                }
            }
        }
    }



}



//Class that stores a list of all the node objects
public class nodeHandler
{
    node[] nodeList;
    public nodeHandler(int nodeAmount)
    {
        nodeList = new node[nodeAmount];
    }

    public void AddNode(int nodeArrayPosition, GameObject nodeObject, Vector3 center)
    {
        nodeList[nodeArrayPosition] = new node(ref nodeObject, center);
    }
}

//Class that stores the game object of a node along with all it's values
public class node   //class of nodes
{
    public GameObject nodeObject;
    public Vector3 position;
    public float surfaceLevel;

    public node(ref GameObject nodeObjectReference, Vector3 center)
    {
        NodeProperties nodeScript = nodeObjectReference.GetComponent<NodeProperties>();
        surfaceLevel = CalculateMetaballs(nodeObjectReference.transform.position, center);
        nodeScript.SetSurfaceValue(surfaceLevel);
        nodeObject = nodeObjectReference;
        position = nodeObject.transform.position;
    }

    public float CalculateMetaballs(Vector3 position, Vector3 center)   //returns the value that a node should be, creating a circular shape
    {
        //uses inverse-square law to decrease surface level the further out it goes
        float radius = 300;  //the radius of the formed sphere
        return Mathf.Min(100, radius * (1 / Mathf.Sqrt(Mathf.Pow(position.x - center.x, 2) + Mathf.Pow(position.y - center.y, 2) + Mathf.Pow(position.z - center.z, 2))));
    }

}