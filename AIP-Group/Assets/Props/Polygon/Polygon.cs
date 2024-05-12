using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsEngine.PhysicsColliders;

public class Polygon : MonoBehaviour
{
    // Components
    LineRenderer lineRenderer;

    // Config
    [SerializeField]
    public Vector2[] vertices = new Vector2[3];
    public Vector2[] transformedVertices;

    PolygonCollider polygonCollider;

    // https://en.wikipedia.org/wiki/Shoelace_formula 
    public float Area()
	{
        float area = 0;

        for (int n = 0; n < vertices.Length; n++)
        {
            Vector2 vA = vertices[n];
            Vector2 vB = vertices[(n + 1) % vertices.Length];

            area += (vA.x * vB.y) - (vB.x * vA.y);
        }

        return Mathf.Abs(area) / 2f;
    }

    void DrawLines()
	{
        TransformVertices();
        lineRenderer.positionCount = vertices.Length + 1;
        
        for (int i = 0; i < vertices.Length; i++)
		{
            lineRenderer.SetPosition(i, transformedVertices[i]);
		}

        lineRenderer.SetPosition(vertices.Length, transformedVertices[0]);
    }

    //
    Vector2 Transform(float x, float y, float theta)
	{
        float dx = Mathf.Cos(theta) * x - Mathf.Sin(theta) * y;
        float dy = Mathf.Sin(theta) * x + Mathf.Cos(theta) * y;

        return new Vector2(dx, dy);
	}

    Vector2 Transform(Vector2 point, float theta)
	{
        return Transform(point.x, point.y, theta * Mathf.Deg2Rad);
	}

    void TransformVertices()
	{
        transformedVertices = new Vector2[vertices.Length];

        for (int i = 0; i < transformedVertices.Length; i++)
		{
            Vector2 vT = Transform(vertices[i], -transform.localRotation.eulerAngles.z) * this.transform.localScale;
            transformedVertices[i] = vT + (Vector2)this.transform.position;
		}

        if (polygonCollider != null)
		{
            polygonCollider.UpdateVertices(transformedVertices);
        }
    }

    public int step = 0;

    IEnumerator Stepper()
	{
        step = 0;
        while (step <= transformedVertices.Length)
		{
            yield return new WaitForSeconds(1f);
            step++;
        }
        StartCoroutine(Stepper());
	}

    void VisualizePolygon()
	{
        Vector2 centroid = Polygon.Centroid(transformedVertices);

        for (int j = 0; j < transformedVertices.Length; j++)
        {
            Vector2 vA = transformedVertices[j]; // vertex A
            Vector2 vB = transformedVertices[(j + 1) % transformedVertices.Length]; // vertex B

            Vector2 edge = vB - vA;
            Vector2 axis = new Vector2(-edge.y, edge.x);

            if (j == step)
			{
                Debug.DrawLine(vA, axis, Color.cyan);
                Debug.DrawLine(vB, axis, Color.cyan);

                Debug.DrawLine(axis * 5f, -axis*5f, Color.white);
                Debug.DrawLine(Vector2.Dot(vA, axis) * axis, vA, Color.green);
                Debug.DrawLine(Vector2.Dot(vB, axis) * axis, vB, Color.green);
            }

            //Debug.DrawLine(centroid, edge, Color.cyan); 

        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        polygonCollider = (PolygonCollider) this.GetComponent<PhysicsCollider>().collisionGeometry;
        //StartCoroutine(Stepper());
    }

    // Update is called once per frame
    void Update()
    {
        DrawLines();
        //VisualizePolygon();
    }

	private void OnValidate()
	{
        lineRenderer = this.GetComponent<LineRenderer>();
        polygonCollider = (PolygonCollider)this.GetComponent<PhysicsCollider>().collisionGeometry;
        DrawLines();
	}

    private void OnDrawGizmos()
    {
        //DrawLines();
        PhysicsDebug.DrawPolygon(transformedVertices, Color.magenta);
    }

    public static Vector2 Centroid(Vector2[] vertices)
    {
        float sumX = 0;
        float sumY = 0;

        for (int i = 0; i < vertices.Length; i++)
        {
            Vector2 v = vertices[i];
            sumX += v.x;
            sumY += v.y;
        }

        Vector2 centroid = new Vector2(sumX / vertices.Length, sumY / vertices.Length);

        return centroid;
    }
}
