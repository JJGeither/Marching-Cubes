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
    public bool isDrawing = false;

    public KeyCode increaseKey;
    public KeyCode decreaseKey;
    public KeyCode increaseRadius;
    public KeyCode decreaseRadius;

    public Sprite ringSprite;

    [SerializeField] private Camera mainCamera;
    [SerializeField] private LayerMask layerMask;
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    void FixedUpdate()
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

        //if(!isDrawing)  //prevents cursor from moving to prevent 'janky' drawing
        {
            if (Physics.Raycast(ray, out RaycastHit raycastHit, float.MaxValue, layerMask))
            {
                this.GetComponent<Renderer>().enabled = true;
                transform.position = raycastHit.point;
            }
            else
            {
                this.transform.position = mainCamera.ScreenToWorldPoint(new Vector3(mousePosition.x, mousePosition.y, zDistance));
            }
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
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, size / 2);
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

}
