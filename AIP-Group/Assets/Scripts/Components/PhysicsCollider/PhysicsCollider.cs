using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsEngine.PhysicsBodies;
using MathU.Geometry;


/*  File: Physics Collider
 * 
 *      Individual collider component that stores collider geometry information. Does not handle collision logic.
 */

namespace PhysicsEngine.PhysicsColliders
{
    public class PhysicsCollider : MonoBehaviour
    {
		[Header("Collider Config")]
		[Tooltip("Update this from PhysicsBody component if object has a PhysicsBody component.")]
        public Geometry.Shapes colliderShape = Geometry.Shapes.Circle;
        public bool isTrigger = false;

		[Header("Debugging")]
        // Objects this collider is colliding with.
        public List<PhysicsCollider> collidingObjects = new List<PhysicsCollider>();

        // Geometry
        public ColliderGeometry collisionGeometry;
        public bool requiresUpdate = false;

        public void PhysicsTrigger(PhysicsCollider other)
		{
            
		}
        // Start is called before the first frame update
        void Start()
        {
            if (this.gameObject.GetComponent<PhysicsBody>())
            {
                SetFromPhysicsBody();
            }
        }

        // Update is called once per frame
        void Update()
        {
            
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
            if (body.geometry == Geometry.Shapes.Circle)
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
            }
        }

		private void OnValidate()
		{
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
            PhysicsDebug.DrawShape(this.colliderShape, this.transform.position, this.transform.rotation, this.transform.localScale, Color.green);
        }
    }
}