using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using PhysicsEngine.PhysicsBodies;

public class Boid : MonoBehaviour
{
    public Vector3 baseRotation;

    BoidController controller;
    PhysicsBody physicsBody;

    [Range(0, 10)]
    public float maxSpeed = 1f;

    [Range(.1f, .5f)]
    public float maxForce = .03f;

    [Range(1, 10)]
    public float neighborhoodRadius = 3f;

    [Range(0, 3)]
    public float separationAmount = 1f;

    [Range(0, 3)]
    public float cohesionAmount = 1f;

    [Range(0, 3)]
    public float alignmentAmount = 1f;

    public Vector2 acceleration;
    public Vector2 velocity;

    private Vector2 Position
    {
        get
        {
            return gameObject.transform.position;
        }
        set
        {
            gameObject.transform.position = value;
        }
    }

    List<Boid> Neighbors()
	{
        List<Boid> neighbors = new List<Boid>();

        foreach (Boid boid in controller.boids)
		{
            float radii = neighborhoodRadius + boid.neighborhoodRadius;
            float distance = (boid.Position - this.Position).magnitude;
            if (distance <= radii)
			{
                neighbors.Add(boid);
			}

		}

        return neighbors;
	}

    private Vector2 Alignment(IEnumerable<Boid> boids)
	{
        Vector2 velocity = Vector2.zero;

        if (!boids.Any())
            return velocity;

        foreach (Boid boid in boids)
		{
            velocity += boid.velocity;
		}

        velocity /= boids.Count();

        Vector2 steer = Vector2.zero;

        return steer;
	}



    private Vector2 Steer(Vector2 direction)
	{
        Vector2 steer = direction - velocity;
        steer = LimitMagnitude(steer, maxForce);

        return steer;

	}
    private void Flock(IEnumerable<Boid> boids)
	{
        Vector2 alignment = Alignment(boids);
        Vector2 Separation = Vector2.one;
        Vector2 cohesion = Vector2.one;

        acceleration = (alignmentAmount * alignment) + (separationAmount * Separation) + (cohesionAmount * cohesion);
	}

    private void UpdateVelocity()
	{
        velocity += acceleration;
        velocity = LimitMagnitude(velocity, maxSpeed);

        physicsBody.AddForce(velocity, PhysicsBody.ForceType.Impulse);
	}
    private void UpdateRotation()
	{
        float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle) + baseRotation);
	}

	#region HelperFunctions

    private Vector2 LimitMagnitude(Vector2 vector, float maxMagnitude)
	{
        if (vector.sqrMagnitude > maxMagnitude * maxMagnitude)
		{
            vector = vector.normalized * maxMagnitude;
		}

        return vector;
	}
    private float DistanceTo(Boid boid)
	{
        return Vector3.Distance(boid.transform.position, Position);
	}

    private void WrapAround()
    {
        if (Position.x < -14) Position = new Vector2(14, Position.y);
        if (Position.y < -8) Position = new Vector2(Position.x, 8);
        if (Position.x > 14) Position = new Vector2(-14, Position.y);
        if (Position.y > 8) Position = new Vector2(Position.x, -8);

        this.transform.position = Position;
    }

    #endregion

    // Start is called before the first frame update
    private void Start()
    {
        controller = GameObject.FindObjectOfType<BoidController>();
        physicsBody = this.GetComponent<PhysicsBody>();

        float angle = Random.Range(0, 2 * Mathf.PI);

        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle) + baseRotation);
        velocity = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));
    }

    // Update is called once per frame
    void Update()
    {
        List<Boid> boids = Neighbors();
        boids.Remove(this);

        Flock(boids);
        UpdateVelocity();
        UpdateRotation();
        WrapAround();

    }
}
