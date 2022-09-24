using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeProperties : MonoBehaviour
{
    //variables
    private float surfaceValue;
    private Material NodeMaterial;
    private GameObject CubeArea;
    public int index;

    // Start is called before the first frame update
    void Start()
    {
        NodeMaterial = this.gameObject.GetComponent<MeshRenderer>().material; //instantiates the node material
        CubeArea = GameObject.Find("Cube Area");
    }

    // Update is called once per frame
    void Update()
    {
        UpdateColor(); //updates the color of each node
        UpdateVisibility();
    }

    public void UpdateColor()
    {
        Color lerpedColor = Color.Lerp(Color.black, Color.white, surfaceValue / 100); //changes color depending on the surface Value with black being low values and white being high values
        NodeMaterial.color = lerpedColor;
    }

    public void UpdateVisibility()
    {
        if (CubeArea.GetComponent<CreatePoints>().renderAllNodes) //renders all nodes
        {
            this.GetComponent<Renderer>().enabled = true;
        }
        else if (CubeArea.GetComponent<CreatePoints>().renderNodes) //renders node within the mesh
        {
            if (CubeArea.GetComponent<CreatePoints>().surfaceRange <= surfaceValue)
            {
                this.GetComponent<Renderer>().enabled = true;
            }
            else gameObject.GetComponent<Renderer>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<Renderer>().enabled = false;
        }
    }

    //getter & setter functions
    public float GetSurfaceValue()
    {
        return surfaceValue;
    }

    public void SetSurfaceValue(float value) //used for initial creation of nodea
    {
        surfaceValue = value; //new surface value
    }

    public void UpdateAllNodeReferences(float value) //used to update all instances of nodes 
    {
        NodeHandler nodeHandler = CubeArea.GetComponent<CreatePoints>().nodeHandler;
        CubeHandler cubeHandler = CubeArea.GetComponent<CreatePoints>().cubeHandler;

        surfaceValue = value; //new surface value

        //updates the nodehandler to reflect this change in value
        Node node = nodeHandler.GetNode(index);
        node.surfaceLevel = value; //new surfae value in nodeHandler

        //Updates all the cubes in which a node touches instead of every single cube
        foreach (var cubeRef in node.GetCubeRefList())
        {
            cubeHandler.GetCube(cubeRef.GetCubeIndex()).UpdateVerticeSurfaceValue(cubeRef.GetVerticeIndex(), value);
            cubeHandler.cubeList[cubeRef.GetCubeIndex()].UpdateVerticeSurfaceValue(cubeRef.GetVerticeIndex(), value);
        }

    }
}