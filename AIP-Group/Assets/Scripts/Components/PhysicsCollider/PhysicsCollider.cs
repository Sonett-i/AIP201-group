using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/*  File: Physics Collider
 * 
 *      Individual collider component that stores collider geometry information. Does not handle collision logic.
 */

namespace PhysicsEngine.Colliders
{
    public class PhysicsCollider : MonoBehaviour
    {
        public PhysicsCollisions.ColliderShapes colliderShape = PhysicsCollisions.ColliderShapes.Circle;

        // Objects this collider is colliding with.
        public List<PhysicsCollider> collidingObjects = new List<PhysicsCollider>();

        public bool requiresUpdate = true;

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }

		private void OnDrawGizmos()
		{
			
		}
	}
}