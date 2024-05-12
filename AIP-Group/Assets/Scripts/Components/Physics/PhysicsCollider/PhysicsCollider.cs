using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsEngine.PhysicsBodies;
using MathU.Geometry;
using UnityEngine.Events;

/*  File: Physics Collider
 * 
 *      Individual collider component that stores collider geometry information. Does not handle collision logic.
 */

namespace PhysicsEngine.PhysicsColliders
{
    public class PhysicsCollider : MonoBehaviour
    {
        // Events
        public delegate void CollisionEvent(PhysicsColliders.Collision collision);
        public event CollisionEvent PhysicsCollisionEnterEvent;

        public delegate void TriggerEvent(PhysicsColliders.Collision collision);
        public event TriggerEvent PhysicsTriggerEnterEvent;

        [Header("Collider Config")]
		[Tooltip("Update this from PhysicsBody component if object has a PhysicsBody component.")]
        public Geometry.Shapes colliderShape = Geometry.Shapes.Circle;
        public bool isTrigger = false;
        [SerializeField] Vector3 scale = Vector3.zero;
        bool initialized = false;

		[Header("Debugging")]
        // Objects this collider is colliding with.
        public List<PhysicsCollider> collidingObjects = new List<PhysicsCollider>();

        // Geometry
        public ColliderGeometry collisionGeometry;
        public bool requiresUpdate = false;

        public UnityEvent colliding;

        public void PhysicsTriggerEnter(PhysicsColliders.Collision collision)
		{
            if (PhysicsTriggerEnterEvent != null)
            {
                PhysicsTriggerEnterEvent.Invoke(collision);
            }
        }

        public void PhysicsCollisionEnter(PhysicsColliders.Collision collision)
		{
            if (PhysicsCollisionEnterEvent != null)
			{
                PhysicsCollisionEnterEvent.Invoke(collision);
			}
		}
        // Set Collider Info From Physics
        public void SetFromPhysicsBody()
		{
            PhysicsBody body = (PhysicsBody)this.gameObject.GetComponent<PhysicsBody>();

            if (body.geometry == Geometry.Shapes.Point)
			{
                this.colliderShape = Geometry.Shapes.Point;
                this.collisionGeometry = new PointCollider(this.transform.position);
			}
            else if (body.geometry == Geometry.Shapes.Circle)
            {
                this.colliderShape = Geometry.Shapes.Circle;
                this.collisionGeometry = new CircleCollider(this.transform.position, this.transform.localScale.x / 2f);
            }
            else if (body.geometry == Geometry.Shapes.AABB)
			{
                this.colliderShape = Geometry.Shapes.AABB;
                this.collisionGeometry = new AABBCollider(this.transform.position, this.transform.localScale);
			}
            else if (body.geometry == Geometry.Shapes.OBB)
            {
                this.colliderShape = Geometry.Shapes.OBB;
                this.collisionGeometry = new OBBCollider(this.transform.position, this.transform.localScale, this.transform.rotation.eulerAngles);
            }
            else if (body.geometry == Geometry.Shapes.Polygon)
            {
                this.colliderShape = Geometry.Shapes.Polygon;
                Vector2[] vertices = this.GetComponent<Polygon>().vertices;
                this.collisionGeometry = new PolygonCollider(this.transform.position, vertices);
            }
        }

        void Initialize()
		{
            requiresUpdate = true;

            if (this.gameObject.GetComponent<PhysicsBody>())
            {
                SetFromPhysicsBody();
            }

            initialized = true;
            StartCoroutine(Init());
        }

        IEnumerator Init()
		{
            yield return new WaitForSeconds(Time.deltaTime);
            GameObject.FindAnyObjectByType<PhysicsEngine.Engine.PhysicsEngine>().AddToList(this.GetComponent<PhysicsBody>());
            GameObject.FindAnyObjectByType<PhysicsEngine.Engine.PhysicsEngine>().AddToList(this);

        }

        // Start is called before the first frame update
        void Start()
        {
            Initialize();
        }

        // Update is called once per frame
        void Update()
        {
            if (initialized)
			{
                this.collisionGeometry.Update(this.transform.position, scale, this.transform.rotation);
                if (this.colliderShape == Geometry.Shapes.Polygon)
				{
                    this.collisionGeometry.UpdateGeometry(this.GetComponent<Polygon>().transformedVertices);
				}
            }
            
        }

        private void OnValidate()
		{
            if (scale == Vector3.zero)
            {
                scale = this.transform.localScale;
            }

            if (this.gameObject.GetComponent<PhysicsBody>())
			{
                SetFromPhysicsBody();
			}

            if (this.gameObject.GetComponent<PhysicsBody>().geometry != this.colliderShape)
			{
                this.gameObject.GetComponent<PhysicsBody>().UpdateGeometry(this.colliderShape);
            }
            //GetColliderShape();
		}

        private void OnDrawGizmos()
        {
            PhysicsDebug.DrawShape(this.colliderShape, this.transform.position, this.transform.rotation, scale, Color.green);
        }
    }
}