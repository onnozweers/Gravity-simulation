using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;
public class Gravity : MonoBehaviour
{
    public float G = 1;
    public int predictionIterations = 100;
    private GravityObject[] gravityObjects;
    private List<Vector2>[] predictedPositions;
    private List<Vector2>[] predictedVelocities;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        CheckForGravityObjects();
        int n = gravityObjects.Length;
        predictedPositions = new List<Vector2>[n];
        predictedVelocities = new List<Vector2>[n];
        for (int i = 0; i < n; i++)
        {
            predictedPositions[i] = new List<Vector2>(predictionIterations);
            predictedVelocities[i] = new List<Vector2>(predictionIterations);
        }
        InitializePredictions();
        UpdateAllLineRenderers(); // Draw initial predictions
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("Trying to update predictions");
            InitializePredictions();
            UpdateAllLineRenderers();
        }
        foreach (GravityObject ownObj in gravityObjects)
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
                        objToAttract_rb.AddForce(direction.normalized * (G * ownObj_rb.mass * objToAttract_rb.mass / Mathf.Pow(direction.magnitude, 2)));
                    }
                }
            }
        }
    }

    public void CheckForGravityObjects()
    {
        gravityObjects = FindObjectsByType<GravityObject>(FindObjectsSortMode.None);
    }

    public void InitializePredictions()
    {
        int n = gravityObjects.Length;

        predictedPositions = new List<Vector2>[n];
        predictedVelocities = new List<Vector2>[n];
        for (int i = 0; i < n; i++)
        {
            predictedPositions[i] = new List<Vector2>(predictionIterations);
            predictedVelocities[i] = new List<Vector2>(predictionIterations);
        }

        Vector2[] positions = new Vector2[n];
        Vector2[] velocities = new Vector2[n];
        float[] masses = new float[n];

        for (int i = 0; i < n; i++)
        {
            positions[i] = gravityObjects[i].transform.position;
            velocities[i] = gravityObjects[i].GetComponent<Rigidbody2D>().linearVelocity;
            masses[i] = gravityObjects[i].GetComponent<Rigidbody2D>().mass;
        }

        for (int step = 0; step < predictionIterations; step++)
        {
            // Save predictions
            for (int i = 0; i < n; i++)
            {
                predictedPositions[i].Add(positions[i]);
                predictedVelocities[i].Add(velocities[i]);
            }
            // Gravity step (simple Euler)
            Vector2[] forces = new Vector2[n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i == j) continue;
                    Vector2 dir = positions[j] - positions[i];
                    float distSqr = dir.sqrMagnitude + 0.01f;
                    forces[i] += G * masses[i] * masses[j] * dir.normalized / distSqr;
                }
            }
            for (int i = 0; i < n; i++)
            {
                velocities[i] += forces[i] / masses[i] * Time.fixedDeltaTime;
                positions[i] += velocities[i] * Time.fixedDeltaTime;
            }
        }
    }

    public void UpdateAllLineRenderers()
    {
        Debug.Log("Updating line renderers");
        int n = gravityObjects.Length;
        for (int i = 0; i < n; i++)
        {
            LineRenderer lr = gravityObjects[i].GetComponent<LineRenderer>();
            if (lr == null)
            {
                lr = gravityObjects[i].gameObject.AddComponent<LineRenderer>();
                lr.widthMultiplier = 0.05f;
                lr.material = new Material(Shader.Find("Sprites/Default")); // Simple visible material
                lr.startColor = lr.endColor = Color.yellow;
            }

            lr.positionCount = predictedPositions[i].Count;
            for (int j = 0; j < predictedPositions[i].Count; j++)
            {
                // Convert 2D position to 3D for LineRenderer
                Vector2 pos2D = predictedPositions[i][j];
                lr.SetPosition(j, new Vector3(pos2D.x, pos2D.y, 0));
            }
        }
    }
}