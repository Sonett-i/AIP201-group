using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsEngine.Colliders
{
    // Collision class that stores collision information.
    public class Collision
	{
        public Vector3 normal; // the direction to push the colliding objects away.
        public float intersection; // the area of intersection.

        public Collision(Vector3 normal, float intersection)
		{
			this.normal = normal;
			this.intersection = intersection;
		}
	}

    public class PhysicsCollisions : MonoBehaviour
    {
        public enum CollisionType
        {
            POINT_POINT,
            POINT_CIRCLE,
            POINT_AABB,
            CIRCLE_CIRCLE,
            CIRCLE_AABB,
            AABB_AABB,
        }

        public delegate Collision CollisionDelegate(PhysicsCollider colliderA, PhysicsCollider colliderB);
        public static Dictionary<CollisionType, CollisionDelegate> collisionTypes = new Dictionary<CollisionType, CollisionDelegate>()
        {
			[CollisionType.POINT_POINT] = (colliderA, colliderB) => PointPointCollision(colliderA, colliderB),
            [CollisionType.POINT_CIRCLE] = (colliderA, colliderB) => Placeholder(colliderA, colliderB),
            [CollisionType.POINT_AABB] = (colliderA, colliderB) => Placeholder(colliderA, colliderB),
            [CollisionType.CIRCLE_CIRCLE] = (colliderA, colliderB) => Placeholder(colliderA, colliderB),
            [CollisionType.CIRCLE_AABB] = (colliderA, colliderB) => Placeholder(colliderA, colliderB),
            [CollisionType.AABB_AABB] = (colliderA, colliderB) => Placeholder(colliderA, colliderB),
        };


        public static Collision PointPointCollision(PhysicsCollider a, PhysicsCollider b)
		{
            return null;
		}

        public static Collision Placeholder(PhysicsCollider a, PhysicsCollider b)
        {
            return null;
        }

        public static Collision HandleCollision(PhysicsCollider colliderA, PhysicsCollider colliderB)
		{
            Collision collision = null;

            ColliderGeometry A = colliderA.collisionGeometry;
            ColliderGeometry B = colliderB.collisionGeometry;

            return collision;
		}
        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}