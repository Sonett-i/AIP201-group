using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsEngine.PhysicsBodies;
using PhysicsEngine.PhysicsColliders;

public class Boid : MonoBehaviour
{
    public enum BoidType
	{
        BOID_NORMAL,
        BOID_PREDATOR
	}

    public PhysicsBody physicsBody;
    PhysicsCollider physicsCollider;
    [SerializeField] BoidType boidType = BoidType.BOID_NORMAL;

    public Flock parentFlock;

    public Vector2 Velocity;

    public float maxSpeed = 5;
    public float minSpeed = 3;

    public List<Boid> Neighbors(float distance)
	{
        if (parentFlock != null)
		{
            List<Boid> neighbors = new List<Boid>();
            foreach (Boid boid in parentFlock.Boids)
			{
                if (Vector2.Distance(this.transform.position, boid.transform.position) < distance)
                    neighbors.Add(boid);
			}
            return neighbors;
		}
        return null;
	}

    public void MoveForward()
	{
        float speed = Velocity.magnitude;
        Debug.Log(Velocity);
        if (speed > maxSpeed)
        {
            Velocity.x = (Velocity.x / speed) * maxSpeed;
            Velocity.y = (Velocity.y / speed) * maxSpeed;
        }
        else if (speed < minSpeed)
        {
            Velocity.x = (Velocity.x / speed) * minSpeed;
            Velocity.y = (Velocity.y / speed) * minSpeed;
        }

        if (float.IsNaN(Velocity.x) || float.IsNaN(Velocity.y))
            Velocity = Vector2.zero;

        

        this.transform.localRotation = Quaternion.Euler(0, Velocity.y, 0);
        physicsBody.AddForce(Velocity, PhysicsBody.ForceType.Impulse);
	}

    public float getDistance(Boid neighbor)
	{
        float distance = Mathf.Abs((neighbor.transform.position - this.transform.position).magnitude);

        return distance;
	}

    // Start is called before the first frame update
    void Start()
    {
        physicsBody = GetComponent<PhysicsBody>();
        physicsCollider = GetComponent<PhysicsCollider>();
    }

	private void OnDestroy()
	{
        //GameObject.FindObjectOfType<PhysicsEngine.Engine.PhysicsEngine>().RemoveFromList(physicsBody);
        if (parentFlock != null)
		{
            parentFlock.RemoveBoid(this);
		}
	}
}
