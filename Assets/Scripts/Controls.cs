using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    Camera mainCam;
    public float zoomSpeed;
    public TextMeshProUGUI tutorialText;
    public TextMeshProUGUI colliderActivityText;
    public Gravity gravityScript;

    public bool collidersActive = true;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Physics2D.IgnoreLayerCollision(0, 0, false);
        mainCam = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
        }

        if (Input.GetKey(KeyCode.Equals))
        {
            mainCam.fieldOfView -= zoomSpeed * Time.unscaledDeltaTime;
        }
        if (Input.GetKey(KeyCode.Minus))
        {
            mainCam.fieldOfView += zoomSpeed * Time.unscaledDeltaTime;
        }
        mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView, 17.4f, 100f);

        if (Input.GetKeyDown(KeyCode.H))
        {
            tutorialText.gameObject.SetActive(!tutorialText.gameObject.activeSelf);
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            collidersActive = !collidersActive;

            if (collidersActive)
            {
                Physics2D.IgnoreLayerCollision(0, 0, false);
                colliderActivityText.text = "Colliders active";
            }
            else
            {
                Physics2D.IgnoreLayerCollision(0, 0, true);
                colliderActivityText.text = "Colliders inactive";
            }
        }
    }
}
