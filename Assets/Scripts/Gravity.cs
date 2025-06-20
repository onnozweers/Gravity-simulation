using UnityEngine;
using System.Collections.Generic;
using Unity.Mathematics;

public class Gravity : MonoBehaviour
{
    bool showingLineRenderers = true;
    public float G = 0;
    public int predictionIterations = 100;
    private float lastG;
    private int lastObjectCount;

    private GravityObject[] gravityObjects;
    private List<Vector2>[] predictedPositions;
    private List<Vector2>[] predictedVelocities;

    bool hasReleasedMouse;

    void Start()
    {
        CheckForGravityObjects();
        lastG = G;
        lastObjectCount = gravityObjects.Length;
        InitializePredictionArrays();
        InitializePredictions();
        // UpdateAllLineRenderers();
    }

    void Update()
    {
        ToggleLineRenderers();
        if (Input.GetMouseButtonUp(0))
        {
            hasReleasedMouse = true;
        }


    }
    void FixedUpdate()
    {
        CheckForGravityObjects();

        if (G != lastG || gravityObjects.Length != lastObjectCount || hasReleasedMouse == true)
        {
            hasReleasedMouse = false;
            lastG = G;
            lastObjectCount = gravityObjects.Length;

            if (showingLineRenderers)
            {
                InitializePredictionArrays();
                InitializePredictions();
                UpdateAllLineRenderers();
            }
        }

        foreach (GravityObject ownObj in gravityObjects)
        {
            foreach (GravityObject objToAttract in gravityObjects)
            {
                if (ownObj != objToAttract)
                {
                    Vector2 direction = ownObj.transform.position - objToAttract.transform.position;
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

    private void InitializePredictionArrays()
    {
        int n = gravityObjects.Length;
        predictedPositions = new List<Vector2>[n];
        predictedVelocities = new List<Vector2>[n];
        for (int i = 0; i < n; i++)
        {
            predictedPositions[i] = new List<Vector2>(predictionIterations);
            predictedVelocities[i] = new List<Vector2>(predictionIterations);
        }
    }

    public void InitializePredictions()
    {
        int n = gravityObjects.Length;
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
            for (int i = 0; i < n; i++)
            {
                predictedPositions[i].Add(positions[i]);
                predictedVelocities[i].Add(velocities[i]);
            }

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
        int n = gravityObjects.Length;
        for (int i = 0; i < n; i++)
        {
            LineRenderer lr = gravityObjects[i].GetComponent<LineRenderer>();
            if (lr == null)
            {
                lr = gravityObjects[i].gameObject.AddComponent<LineRenderer>();
                lr.widthMultiplier = 0.05f;
                lr.material = new Material(Shader.Find("Sprites/Default"));
                lr.startColor = lr.endColor = Color.yellow;
            }

            lr.positionCount = predictedPositions[i].Count;
            for (int j = 0; j < predictedPositions[i].Count; j++)
            {
                Vector2 pos2D = predictedPositions[i][j];
                lr.SetPosition(j, new Vector3(pos2D.x, pos2D.y, 0));
            }
        }
    }

    public void ToggleLineRenderers()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            showingLineRenderers = !showingLineRenderers; // Toggle the value
            if (showingLineRenderers)
            {
                UpdateAllLineRenderers();
            }
            // Loop through each GravityObject in the list
            foreach (GravityObject gravityObject in gravityObjects)
            {
                LineRenderer lr = gravityObject.gameObject.GetComponent<LineRenderer>();
                if (lr != null)
                {
                    lr.enabled = showingLineRenderers;
                }
            }
        }
    }
}
