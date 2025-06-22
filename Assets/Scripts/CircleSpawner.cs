using UnityEngine;
public class CircleSpawner : MonoBehaviour
{
    Camera mainCam;
    [SerializeField] GameObject circlePrefab;
    [SerializeField] Gravity gravityScript;
    [SerializeField] Controls controlScript;
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

        if (Input.GetKeyDown(KeyCode.E))
        {
            GameObject instantiatedCircle = Instantiate(circlePrefab, worldMousePosition, Quaternion.identity);
            instantiatedCircle.GetComponent<LineRenderer>().enabled = gravityScript.showingLineRenderers;

            gravityScript.gravityObjects.Add(instantiatedCircle.GetComponent<GravityObject>());
        }

    }
}
