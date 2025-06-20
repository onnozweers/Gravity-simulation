using UnityEngine;
using System.Collections.Generic;
public class ParallaxStars : MonoBehaviour
{
    [SerializeField] GameObject starPrefab;
    [SerializeField] float amountOfStars;

    [SerializeField] float minDistance;
    [SerializeField] float maxDistance;
    private Camera mainCam;

    private float width = Screen.width;
    private float height = Screen.height;

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
            bool wrapped = false;
            Vector3 screenPosition = mainCam.WorldToScreenPoint(star.transform.position);
            if (screenPosition.x < 0) { screenPosition.x = width; wrapped = true; }
            else if (screenPosition.x > width) { screenPosition.x = 0; wrapped = true; }
            if (screenPosition.y < 0) { screenPosition.y = height; wrapped = true; }
            else if (screenPosition.y > height) { screenPosition.y = 0; wrapped = true; }

            if (wrapped)
            {
                star.transform.position = mainCam.ScreenToWorldPoint(new(screenPosition.x, screenPosition.y, screenPosition.z));
            }
        }
    }

    Vector3 randomPosition(float minDistance, float maxDistance)
    {
        // Generate a random coordinate on the screen.
        Vector2 screenCoordinate = new Vector2(
            Random.Range(0, width),
            Random.Range(0, height)
        );

        // Convert the screen coordinate to a world position, and return said world position.
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(
            new Vector3(screenCoordinate.x, screenCoordinate.y, Random.Range(minDistance, maxDistance))
            );
        return worldPosition;
    }
}
