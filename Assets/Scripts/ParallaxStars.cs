using UnityEngine;
using System.Collections.Generic;
public class ParallaxStars : MonoBehaviour
{
    [SerializeField] GameObject starPrefab;
    [SerializeField] float amountOfStars;

    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;

    Camera mainCam;
    public static float areaSize = 0.1279f;
    private float xBounds = 16 * areaSize;
    private float yBounds = 9 * areaSize;

    private List<GameObject> stars = new List<GameObject>();

    void Start()
    {
        mainCam = Camera.main;
        // Instantiate stars for in the background
        for (int i = 0; i < amountOfStars; i++)
        {
            GameObject spawnedStar = Instantiate(
            starPrefab,
            randomPosition(minDistance, maxDistance),
            Quaternion.identity,
            transform
            );

            stars.Add(spawnedStar); // Add the newly spawned star to the list.
        }
    }

    void Update()
    {
        foreach (GameObject star in stars)
        {
            Vector3 relativePos = star.transform.position - mainCam.transform.position;
            float z = relativePos.z;

            bool wrapped = false;
            Vector3 newPosition = star.transform.position;

            if (relativePos.x < -xBounds * z) { newPosition.x += 2 * xBounds * z; wrapped = true; }
            else if (relativePos.x > xBounds * z) { newPosition.x -= 2 * xBounds * z; wrapped = true; }

            if (relativePos.y < -yBounds * z) { newPosition.y += 2 * yBounds * z; wrapped = true; }
            else if (relativePos.y > yBounds * z) { newPosition.y -= 2 * yBounds * z; wrapped = true; }

            if (wrapped)
            {
                star.transform.position = newPosition;
            }
        }
    }

    Vector3 randomPosition(float minZDistance, float maxZDistance)
    {
        Vector3 worldPosition;

        worldPosition.z = Random.Range(minZDistance, maxZDistance);

        worldPosition.x = Random.Range(-xBounds, xBounds) * Mathf.Abs(worldPosition.z);
        worldPosition.y = Random.Range(-yBounds, yBounds) * Mathf.Abs(worldPosition.z);

        return worldPosition;
    }
}
