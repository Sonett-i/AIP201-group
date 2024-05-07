using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsEngine.PhysicsColliders;
using PhysicsEngine.PhysicsBodies;

namespace PhysicsEngine.Engine
{
    public class PhysicsEngine : MonoBehaviour
    {
        public enum SimulationType
        {
            BROAD_PHASE,
            NARROW_PHASE
        }


        List<PhysicsBody> physicsBodies = new List<PhysicsBody>();
        List<PhysicsCollider> physicsColliders = new List<PhysicsCollider>();

        [SerializeField] Vector3 Gravity = Vector3.zero;

        bool initialized = false;

        public SimulationType simulationType;

        void UpdateVelocity()
        {
            foreach (PhysicsBody body in physicsBodies)
            {
                if (body.bodyType != PhysicsBody.BodyType.Static)
				{
                    body.AddForce(body.Acceleration, PhysicsBody.ForceType.Impulse);
                    body.AddForce(Gravity, PhysicsBody.ForceType.Impulse);
                }
            }
        }

        void UpdateCollisions()
        {
            for (int i = 0; i < physicsColliders.Count - 1; i++)
            {
                PhysicsCollider colliderA = physicsColliders[i];

                for (int j = i + 1; j < physicsColliders.Count; j++)
                {
                    PhysicsCollider colliderB = physicsColliders[j];

                    // skip if both are static...
                    if (colliderA.GetComponent<PhysicsBody>().bodyType == PhysicsBody.BodyType.Static && colliderB.GetComponent<PhysicsBody>().bodyType == PhysicsBody.BodyType.Static)
                        continue;

                    if (colliderA && colliderB)
					{
                        CollisionHandler.HandleCollision(colliderA, colliderB);
                    }
                }
            }
        }

        void HandleGravity()
        {
            for (int i = 0; i < physicsBodies.Count - 1; i++)
            {
                PhysicsBody bodyA = physicsBodies[i];

                for (int j = i + 1; j < physicsBodies.Count; j++)
                {
                    PhysicsBody bodyB = physicsBodies[j];

                    Vector3 direction = (bodyB.transform.position - bodyA.transform.position);

                    // Physics Calcs
                    float d = direction.magnitude;
                    float F = (PhysicsConfig.G * bodyA.Mass * bodyB.Mass) / (d * d);
                    Vector3 gravityForce = (direction.normalized * F);

                    bodyA.AddForce(gravityForce * Time.deltaTime, PhysicsBody.ForceType.Impulse);
                    bodyB.AddForce(-gravityForce * F * Time.deltaTime, PhysicsBody.ForceType.Impulse);
                }
            }
        }

        void UpdatePositions()
        {
            foreach (PhysicsBody body in physicsBodies)
            {
                if (body && body.bodyType != PhysicsBody.BodyType.Static)
                {
                    body.Move(body.LinearVelocity * Time.deltaTime);
                }
            }
        }

        void BroadPhase()
        {

        }

        void Initialize()
        {
            physicsBodies.AddRange(GameObject.FindObjectsOfType<PhysicsBody>());
            physicsColliders.AddRange(GameObject.FindObjectsOfType<PhysicsCollider>());
            initialized = true;
        }

        // Start is called before the first frame update
        void Start()
        {
            Initialize();

            if (simulationType == SimulationType.BROAD_PHASE)
            {
                BroadPhase();
            }
        }

        // Update is called once per frame
        void Update()
        {
            if (initialized)
            {
                UpdateVelocity();
                //HandleGravity();
                UpdatePositions();
                UpdateCollisions();
            }
        }

        public void Step(float time)
		{

		}
    }
}

