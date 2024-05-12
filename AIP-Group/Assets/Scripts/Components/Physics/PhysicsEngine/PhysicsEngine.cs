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

        public void AddToList(PhysicsBody physicsBody)
		{
            if (!physicsBodies.Contains(physicsBody))
                physicsBodies.Add(physicsBody);
		}

        public void AddToList(PhysicsCollider physicsCollider)
        {
            if (!physicsColliders.Contains(physicsCollider))
                physicsColliders.Add(physicsCollider);
        }

        public void RemoveFromList(PhysicsBody physicsBody)
		{
            if (physicsBodies.Contains(physicsBody))
                physicsBodies.Remove(physicsBody);
		}

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

        [SerializeField] float stepDuration = 1.5f;
        IEnumerator Stepper()
        {
            step = 0;
            while (step < 4)
            {
                yield return new WaitForSeconds(stepDuration);
                step++;
            }
            StartCoroutine(Stepper());
        }
        public static int step = 0;

        [SerializeField] GameObject[] objects;
        Vector3 mousePos;
        Vector3 mouseWPos;
        void HandleInput()
		{
            mousePos = Input.mousePosition;
            mouseWPos = Camera.main.ScreenToWorldPoint(mousePos);

            //Debug.Log($"{mousePos} {mouseWPos}");

            if (Input.GetMouseButtonDown(0))
			{
                GameObject _obj = Instantiate(objects[Random.Range(0, objects.Length)]);

                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                _obj.transform.position = pos;
			}
		}

        // Start is called before the first frame update
        void Start()
        {
            Initialize();

            if (simulationType == SimulationType.BROAD_PHASE)
            {
                BroadPhase();
            }
            StartCoroutine(Stepper());
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
                //HandleInput();
            }
        }

        public void Step(float time)
		{

		}

        public static IEnumerator Visualize(Vector2[] vertices)
        {
            for (int i = 0; i < vertices.Length; i++)
            {
                Vector2 vA = vertices[i]; // vertex A
                Vector2 vB = vertices[(i + 1) % vertices.Length]; // vertex B

                Vector2 edge = vB - vA;
                Vector2 axis = new Vector2(-edge.y, edge.x);

                Debug.DrawLine(axis, vA, Color.cyan);
                Debug.DrawLine(axis, vB, Color.cyan);

                Debug.DrawLine(edge, axis, Color.cyan);

                yield return new WaitForSeconds(0.25f);
            }
        }
    }
}

