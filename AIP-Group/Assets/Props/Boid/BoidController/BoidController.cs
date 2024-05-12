using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsEngine.PhysicsBodies;
using PhysicsEngine.Engine;

public class BoidController : MonoBehaviour
{

	[SerializeField] int numBoids = 0;
	[SerializeField] GameObject boidPrefab;

	public List<Boid> boids = new List<Boid>();

	private void Start()
	{
		for (int i = 0; i < numBoids; i++)
		{
			boids.Add(Instantiate(boidPrefab, Vector3.zero, Quaternion.identity).GetComponent<Boid>());
		}
	}
}
