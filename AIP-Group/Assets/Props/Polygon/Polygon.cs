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
            Vector2 vT = Transform(vertices[i], -transform.localRotation.eulerAngles.z);
            transformedVertices[i] = vT + (Vector2)this.transform.position;
		}

        if (polygonCollider != null)
		{
            polygonCollider.UpdateVertices(transformedVertices);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        polygonCollider = (PolygonCollider) this.GetComponent<PhysicsCollider>().collisionGeometry;
    }

    // Update is called once per frame
    void Update()
    {
        DrawLines();
    }

	private void OnValidate()
	{
        lineRenderer = this.GetComponent<LineRenderer>();
        polygonCollider = (PolygonCollider)this.GetComponent<PhysicsCollider>().collisionGeometry;
        DrawLines();
	}

    private void OnDrawGizmos()
    {
        DrawLines();
        PhysicsDebug.DrawPolygon(transformedVertices, Color.magenta);
    }
}
