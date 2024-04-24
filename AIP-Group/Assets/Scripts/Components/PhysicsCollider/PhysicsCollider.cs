using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsEngine.PhysicsBodies;


/*  File: Physics Collider
 * 
 *      Individual collider component that stores collider geometry information. Does not handle collision logic.
 */

namespace PhysicsEngine.Colliders
{
    public class PhysicsCollider : MonoBehaviour
    {
        public PhysicsCollisions.ColliderShape colliderShape = PhysicsCollisions.ColliderShape.Circle;

        // Objects this collider is colliding with.
        public List<PhysicsCollider> collidingObjects = new List<PhysicsCollider>();

        ColliderGeometry collisionGeometry;
        public bool requiresUpdate = true;
        public PhysicsCollider physicsCollider;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

        void SetFromPhysicsBody()
		{
            PhysicsBody body = (PhysicsBody)this.gameObject.GetComponent<PhysicsBody>();

            if (body.geometry == PhysicsBody.Geometry.Circle)
            {
                this.colliderShape = PhysicsCollisions.ColliderShape.Circle;
                this.collisionGeometry = new CircleCollider(this.transform.position, this.transform.localScale.x / 2f);
            }
            else if (body.geometry == PhysicsBody.Geometry.Rectangle)
            {
                this.colliderShape = PhysicsCollisions.ColliderShape.OBB;
                this.collisionGeometry = new OBBCollider(this.transform.position, this.transform.localScale);
            }
            else if (body.geometry == PhysicsBody.Geometry.Polygon)
            {
                this.colliderShape = PhysicsCollisions.ColliderShape.Polygon;
            }
        }

        void GetColliderShape()
		{

        }

		private void OnValidate()
		{
            if (this.gameObject.GetComponent<PhysicsBody>())
			{
                SetFromPhysicsBody();
			}

            //GetColliderShape();
		}

        private void OnDrawGizmos()
        {

        }
    }
}