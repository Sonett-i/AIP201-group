using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsEngine.PhysicsBodies;
using PhysicsEngine.Engine;

public class BoidController : MonoBehaviour
{

    List<Flock> Flocks = new List<Flock>();
    List<Boid> Boids = new List<Boid>();
    PhysicsEngine.Engine.PhysicsEngine physicsEngine;
    

    // Rules

    // Rule 1: Boids Steer Toward the center of mass of nearby boids.

    // Rule 2: boids adjust direction to match nearby boids.

    // Rule 3: Boids Steer away from very close boids.

    // Optional rules

    // Rule 4: Boids speed up or slow down to match a target speed.

    // Rule 5: Boids are repelled by the edge of the box.

    // Rule 6: Boids Steer away from boids marked as predators.


    void RegisterNewFlock(Flock flock)
	{
        Debug.Log($"Registered flock {flock.name} with {flock.Boids.Count} boids.");
        if (!Flocks.Contains(flock))
            Flocks.Add(flock);

        foreach (Boid boid in flock.Boids)
		{
            physicsEngine.AddToList(boid.GetComponent<PhysicsBody>());
            Boids.Add(boid);
		}
	}

    public List<Boid> GetLocalBoids(Boid boid, float distance, bool sameGroup)
	{
        List<Boid> localBoids = new List<Boid>();

        foreach (Boid neighbor in Boids)
		{
            if (boid.getDistance(neighbor) < distance)
			{
                if (sameGroup)
				{
                    if (neighbor.parentFlock == boid.parentFlock)
					{
                        localBoids.Add(neighbor);
					}
				}
                else
				{
                    localBoids.Add(neighbor);
				}
			}
		}

        return localBoids;
	}

    // Start is called before the first frame update
    void Start()
    {
        physicsEngine = GameObject.FindFirstObjectByType<PhysicsEngine.Engine.PhysicsEngine>();

       Flock.OnFlockSpawned += RegisterNewFlock;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


}
