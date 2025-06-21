using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Controls : MonoBehaviour
{
    Camera mainCam;
    public float zoomSpeed;
    public TextMeshProUGUI tutorialText;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
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
            mainCam.fieldOfView -= zoomSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Minus))
        {
            mainCam.fieldOfView += zoomSpeed * Time.deltaTime;
        }
        mainCam.fieldOfView = Mathf.Clamp(mainCam.fieldOfView, 17.4f, 61.7f);

        if (Input.GetKeyDown(KeyCode.H))
        {
            tutorialText.gameObject.SetActive(!tutorialText.gameObject.activeSelf);
        }
    }
}
