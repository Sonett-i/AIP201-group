using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Flock : MonoBehaviour
{
    public enum BoidProperty
	{
        PROPERTY_VELOCITY,
        PROPERTY_POSITION,
        PROPERTY_CLOSENESS
	}

    BoidController controller;

    [SerializeField] GameObject boid;

    public float Width = 5;
    public float Height = 5;

    public List<Boid> Boids = new List<Boid>();
    [SerializeField] int numBoids = 10;

    Vector2 flockCenter;

    public delegate void FlockSpawnEvent(Flock flock);
    public static event FlockSpawnEvent OnFlockSpawned;

    bool boidsSpawned = false;

    void SpawnBoids()
	{
        if (controller == null)
		{
            Debug.LogError("Controller not found");
            return;
		}

        for (int i = 0; i < numBoids; i++)
		{
            Vector2 randomPosition = Random.insideUnitCircle;

            GameObject _boid = Instantiate(boid);
            _boid.transform.SetParent(this.transform);
            _boid.transform.position = (Vector3) randomPosition - this.transform.position;
            _boid.GetComponent<Boid>().parentFlock = this;

            AddBoid(_boid.GetComponent<Boid>(), i);
		}

        OnFlockSpawned.Invoke((Flock)this);
        boidsSpawned = true;
    }

    public void AddBoid(Boid boid, int i)
	{
        if (!Boids.Contains(boid))
		{
            Boids.Add(boid);
		}
	}
    public void RemoveBoid(Boid boid)
	{
        if (Boids.Contains(boid))
		{
            Boids.Remove(boid);
		}
	}

    Vector2 Centroid(List<Boid> boids)
	{
        if (boids.Count == 0)
		{
            return this.transform.position;
		}

        float x = 0;
        float y = 0;

        for (int i = 0; i < boids.Count; i++)
		{
            x += Boids[i].transform.position.x;
            y += Boids[i].transform.position.y;
		}

        return new Vector2(x/Boids.Count, y/Boids.Count);
	}

    private Vector2 Cohesion(Boid boid, float distance, float power)
	{
        Vector2 cohesion = Vector2.zero;

        List<Boid> neighbors = controller.GetLocalBoids(boid, distance, true);

        foreach (Boid neighbor in neighbors)
		{
            cohesion += (Vector2) neighbor.transform.position;
		}

        cohesion /= neighbors.Count;

        return cohesion * power;
	}

    private Vector2 Alignment(Boid boid, float distance, float power)
	{
        Vector2 meanVelocity = Vector2.zero;
        List<Boid> neighbors = controller.GetLocalBoids(boid, distance, true);
        
        foreach (Boid neighbor in neighbors)
		{
            meanVelocity += neighbor.Velocity;
		}

        meanVelocity /= neighbors.Count;
        meanVelocity -= boid.Velocity;


        return meanVelocity * power;
	}

    private Vector2 Separation(Boid boid, float distance, float power)
	{
        Vector2 separation = Vector2.zero;

        List<Boid> neighbors = controller.GetLocalBoids(boid, distance, true);

        foreach (Boid neighbor in neighbors)
		{
            float closeness = distance - boid.getDistance(neighbor);
            separation += (Vector2) (neighbor.transform.position - boid.transform.position) * closeness;
		}

        return separation * power;
	}

    private Vector2 Avoid(Boid boid, float distance, float power)
	{
        Vector2 avoidMouse = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        Vector2 separation = Vector2.zero;

        float closeness = distance - ((Vector2)boid.transform.position - avoidMouse).magnitude;

        separation = ((Vector2)boid.transform.position - avoidMouse) * closeness;


        return separation * power;
	}

    void ApplyFlockBehavior()
    {
        foreach (Boid boid in Boids)
        {
            Vector2 flockVelocity = Cohesion(boid, 50, .03f);
            Vector2 flockAlignment = Alignment(boid, 10, 0.01f);
            Vector2 flockSeparation = Separation(boid, 50, 0.1f);
            Vector2 flockAvoidance = Avoid(boid, 2, .100f);

            boid.Velocity += flockVelocity + flockAlignment + flockSeparation + flockAvoidance;

            boid.rotate(boid.Velocity);
            boid.MoveForward();
        }
    }

    IEnumerator Init()
	{
        yield return new WaitForSeconds(0.25f);
        SpawnBoids();
	}
    // Start is called before the first frame update
    void Start()
    {
        controller = GameObject.FindObjectOfType<BoidController>();
        StartCoroutine(Init());
    }

    // Update is called once per frame
    void Update()
    {
        if (boidsSpawned)
		{
            flockCenter = Centroid(Boids);
            ApplyFlockBehavior();
        }
    }
}
