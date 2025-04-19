using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    [SerializeField] float speed;
    private Vector3 newPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        newPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            newPos.y += speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            newPos.y -= speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            newPos.x += speed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            newPos.x -= speed * Time.deltaTime;
        }
        transform.position = newPos;
    }
}
