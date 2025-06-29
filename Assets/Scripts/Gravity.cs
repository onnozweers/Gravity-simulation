using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using Unity.Mathematics;

public class Gravity : MonoBehaviour
{
    [Header("Repulsion settings")]
    public float repulsionConstant;
    public float logarithmBase;
    public float maximumRepulsion;

    [Header("Rest")]
    public bool showingLineRenderers = true;
    public float G = 100;
    public int predictionIterations = 100;
    private float lastG;
    private int lastObjectCount;

    public List<GravityObject> gravityObjects = new List<GravityObject>();
    private List<Vector2>[] predictedPositions;
    private List<Vector2>[] predictedVelocities;

    bool hasReleasedMouse;

    void Start()
    {
        lastG = G;
        lastObjectCount = gravityObjects.Count;
        InitializePredictionArrays();
        InitializePredictions();
        UpdateAllLineRenderers();
        StartCoroutine(PeriodicallyRepredictTrajectories());
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
        if (G != lastG || gravityObjects.Count != lastObjectCount || hasReleasedMouse == true)
        {
            hasReleasedMouse = false;
            lastG = G;
            lastObjectCount = gravityObjects.Count;

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

                    objToAttract_rb.AddForce(direction.normalized * Mathf.Min(
                        AttractionForce(ownObj_rb, objToAttract_rb, direction),
                        RepulsionForce(direction, repulsionConstant, logarithmBase, maximumRepulsion)
                        ));
                }
            }
        }
    }

    float AttractionForce(Rigidbody2D ownObj_rb, Rigidbody2D objToAttract_rb, Vector2 direction)
    {
        float x = direction.magnitude;
        float y = G * ownObj_rb.mass * objToAttract_rb.mass / Mathf.Pow(x, 2);

        return y;
    }

    float RepulsionForce(Vector2 direction, float repulsionConstant, float logBase, float maxRepulsion)
    {
        float x = direction.magnitude;
        float y = Mathf.Max(Mathf.Log(x + repulsionConstant, logBase), 0.2f * x - maxRepulsion);

        return y;
    }


    private void InitializePredictionArrays()
    {
        int n = gravityObjects.Count;
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
        int n = gravityObjects.Count;
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
                    float distSqr = dir.sqrMagnitude;
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
        int n = gravityObjects.Count;
        for (int i = 0; i < n; i++)
        {
            LineRenderer lr = gravityObjects[i].GetComponent<LineRenderer>();

            lr.widthMultiplier = 0.05f;
            lr.material = new Material(Shader.Find("Sprites/Default"));
            lr.startColor = Color.yellow;
            lr.endColor = Color.black;
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
                InitializePredictionArrays();
                InitializePredictions();
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

    public IEnumerator PeriodicallyRepredictTrajectories()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.1f);
            Debug.Log(showingLineRenderers);
            if (showingLineRenderers)
            {
                InitializePredictionArrays();
                InitializePredictions();
                UpdateAllLineRenderers();
            }
        }
    }
}
