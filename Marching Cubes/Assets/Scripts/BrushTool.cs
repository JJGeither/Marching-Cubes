using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrushTool : MonoBehaviour
{
    Vector3 mousePosition;
    public float size;
    public float incrementStrength;
    public float growthSpeed;
    public int zDistance;
    public bool isDrawing;

    public KeyCode increaseKey;
    public KeyCode decreaseKey;
    public KeyCode increaseRadius;
    public KeyCode decreaseRadius;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    // Start is called before the first frame update


    // Update is called once per frame
    void Update()
    {

        mousePosition = Input.mousePosition;

        //input to adjust size of sphere
        if (Input.GetKey(increaseRadius))
        {
            size += growthSpeed;
        }
        if (Input.GetKey(decreaseRadius))
        {
            size -= growthSpeed;
        }

        isDrawing = (Input.GetKey(increaseKey) || Input.GetKey(decreaseKey));    //determines if actively drawing

        //the local size of the sphere
        transform.localScale = new Vector3(size,size,size);

        Ray ray = mainCamera.ScreenPointToRay(mousePosition);

        if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
        {
            this.GetComponent<Renderer>().enabled = true;
            this.transform.position = raycastHit.point;
        }
        else
        {
            this.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, zDistance));
        }



        if (Input.GetKey(increaseKey))    //increases material
        {
            BrushDraw(1);
        }

        if (Input.GetKey(decreaseKey))    //decreases material
        {
            BrushDraw(-1);
        }



    }

    public void BrushDraw(int sign) //the sign determines if material is added or subtracted
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, size / 2f);
        foreach (var hitCollider in hitColliders)
        {
            if (hitCollider.gameObject.name.Equals("Node(Clone)"))
            {
                var node = hitCollider.gameObject.GetComponent<NodeProperties>();
                var value = node.GetSurfaceValue();
                node.UpdateAllNodeReferences(value + (incrementStrength * sign));
            }
        }
    }

    public void OnDrawGizmos()
    {
        
    }

}
