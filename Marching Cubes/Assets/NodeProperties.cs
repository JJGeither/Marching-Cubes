using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeProperties : MonoBehaviour
{
    //variables
    private float SurfaceValue;
    private Material NodeMaterial;
    private GameObject cube;

    // Start is called before the first frame update
    void Start()
    {
        NodeMaterial = this.gameObject.GetComponent<MeshRenderer>().material;   //instantiates the node material
        cube = GameObject.Find("Cube Area");
    }

    // Update is called once per frame
    void Update()
    {
        updateColor();  //updates the color of each node
        updateVisibility();
    }

    public void updateColor()
    {
        Color lerpedColor = Color.Lerp(Color.black, Color.white, SurfaceValue / 100); //changes color depending on the surface Value with black being low values and white being high values
        NodeMaterial.color = lerpedColor;
    }

    public void updateVisibility()
    {
        if (cube.GetComponent<CreatePoints>().SurfaceRange <= SurfaceValue)
        {
            this.GetComponent<Renderer>().enabled = true;
        } else
            gameObject.GetComponent<Renderer>().enabled = false;

    }

    //getter & setter functions
    public float GetSurfaceValue()
    {
        return SurfaceValue;
    }

    public void SetSurfaceValue(float value)
    {
        SurfaceValue = value;
    }
}
