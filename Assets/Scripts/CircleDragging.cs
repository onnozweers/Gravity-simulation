using System.Collections.Generic;
using UnityEngine;

public class CircleDragging : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] Gravity gravityScript;
    private List<GameObject> selectedObjects = new List<GameObject>();
    [SerializeField] Transform cursorTransform;
    public float strength;
    public float damping;
    public float cursorRadius;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenMousePosition = new(Input.mousePosition.x, Input.mousePosition.y, -mainCam.transform.position.z);
        Vector3 worldMousePosition = mainCam.ScreenToWorldPoint(screenMousePosition);

        Debug.Log(worldMousePosition);

        if (Input.GetMouseButtonDown(0))
        {

            foreach (GravityObject obj in gravityScript.gravityObjects)
            {
                if (Vector2.Distance(worldMousePosition, obj.transform.position) < cursorRadius)
                {
                    obj.gameObject.GetComponent<Rigidbody2D>().linearDamping = damping;
                    selectedObjects.Add(obj.gameObject);
                }
            }
        }
        else if (Input.GetMouseButtonUp(0) && selectedObjects != null)
        {
            foreach (GameObject obj in selectedObjects)
            {
                obj.GetComponent<Rigidbody2D>().linearDamping = 0;
            }
            selectedObjects.Clear();
        }

        if (selectedObjects.Count != 0)
        {
            foreach (GameObject obj in selectedObjects)
            {
                // Destroy the object when right clicked
                if (Input.GetKeyDown(KeyCode.D))
                {
                    selectedObjects.Remove(obj);
                    gravityScript.gravityObjects.Remove(obj.GetComponent<GravityObject>());
                    Destroy(obj);
                }
                Rigidbody2D rb = obj.GetComponent<Rigidbody2D>();
                rb.AddForce(rb.mass * strength * (worldMousePosition - obj.transform.position));
            }
        }

        cursorTransform.position = new(worldMousePosition.x, worldMousePosition.y, -0.01f);
        cursorTransform.localScale = 2 * cursorRadius * Vector3.one;
    }
}
