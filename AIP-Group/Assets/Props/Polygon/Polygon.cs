using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Polygon : MonoBehaviour
{
    // Components
    LineRenderer lineRenderer;

    // Config
    [SerializeField]
    List<Vector2> vertices = new List<Vector2>();


    void DrawLines()
	{
        for (int i = 0; i < vertices.Count; i++)
		{

		}
	}
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	private void OnValidate()
	{
		
	}
}
