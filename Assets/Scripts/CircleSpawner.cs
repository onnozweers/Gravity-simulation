using UnityEngine;
public class CircleSpawner : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] GameObject circlePrefab;
    [SerializeField] Gravity gravityScript;
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

        Debug.Log(worldMousePosition);

        if (Input.GetKeyDown(KeyCode.E))
        {
            Instantiate(circlePrefab, worldMousePosition, Quaternion.identity);
            gravityScript.CheckForGravityObjects();
        }
        
    }
}
