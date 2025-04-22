using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
public class Gravity : MonoBehaviour
{
    public float G = 1;
    private GravityObject[] gravityObjects;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckForGravityObjects();
    }

    // Update is called once per frame
    void Update()
    {
        foreach(GravityObject ownObj in gravityObjects)
        {
            foreach (GravityObject objToAttract in gravityObjects)
            {
                if (ownObj != objToAttract)
                {
                    Vector2 direction = ownObj.gameObject.transform.position - objToAttract.gameObject.transform.position;

                    Rigidbody2D objToAttract_rb = objToAttract.GetComponent<Rigidbody2D>();
                    Rigidbody2D ownObj_rb = ownObj.GetComponent<Rigidbody2D>();



                    if (direction.magnitude > 0.01f)
                    {
                        objToAttract_rb.AddForce(direction.normalized * (G * ownObj_rb.mass * objToAttract_rb.mass * Time.deltaTime / Mathf.Pow(direction.magnitude, 2)));
                    }
                }
            }
        }
    }

    public void CheckForGravityObjects()
    {
        gravityObjects = FindObjectsByType<GravityObject>(FindObjectsSortMode.None);
    }
}
