using UnityEngine;

public class CircleDragging : MonoBehaviour
{
    Camera mainCam;
    private GameObject selectedCircle;
    [SerializeField] float strength;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 screenMousePosition = Input.mousePosition;
        screenMousePosition.z = -mainCam.gameObject.transform.position.z;
        Vector3 worldMousePosition = mainCam.ScreenToWorldPoint(screenMousePosition);

        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(worldMousePosition, Vector2.zero);
            if (hit.collider != null)
            {
                selectedCircle = hit.collider.gameObject;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            selectedCircle = null;
        }

        if (selectedCircle != null)
        {
            selectedCircle.GetComponent<Rigidbody2D>().linearVelocity = (worldMousePosition - selectedCircle.transform.position)*strength;
        }
    }   
}
