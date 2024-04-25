using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathU.Geometry;
using PhysicsEngine.PhysicsBodies;


namespace PhysicsEngine.PhysicsColliders
{


    public class PhysicsCollisions : MonoBehaviour
    {
        public enum CollisionType
        {
            POINT_POINT,
            POINT_CIRCLE,
            POINT_AABB,
            POINT_OBB,
            CIRCLE_POINT,
            CIRCLE_CIRCLE,
            CIRCLE_AABB,
            CIRCLE_OBB,
            AABB_POINT,
            AABB_CIRCLE,
            AABB_AABB,
            AABB_OBB,
            OBB_POINT,
            OBB_CIRCLE,
            OBB_AABB,
            OBB_OBB,
            INVALID
        }

        // Collision Disambiguation
        public delegate Collision CollisionDelegate(PhysicsCollider colliderA, PhysicsCollider colliderB);
        public static Dictionary<CollisionType, CollisionDelegate> collisionTypes = new Dictionary<CollisionType, CollisionDelegate>()
        {
			[CollisionType.POINT_POINT] = (colliderA, colliderB) => PointPointCollision(colliderA, colliderB),
            [CollisionType.POINT_CIRCLE] = (colliderA, colliderB) => Placeholder(colliderA, colliderB),
            [CollisionType.POINT_AABB] = (colliderA, colliderB) => Placeholder(colliderA, colliderB),
            [CollisionType.CIRCLE_CIRCLE] = (colliderA, colliderB) => CircleCircleCollision(colliderA, colliderB),
            [CollisionType.CIRCLE_AABB] = (colliderA, colliderB) => Placeholder(colliderA, colliderB),
            [CollisionType.AABB_AABB] = (colliderA, colliderB) => Placeholder(colliderA, colliderB),
            [CollisionType.INVALID] = (colliderA, colliderB) => Placeholder(colliderA, colliderB),
        };

        static Dictionary<(Geometry.Shapes, Geometry.Shapes), CollisionType> collisionMapping = new Dictionary<(Geometry.Shapes, Geometry.Shapes), CollisionType>
            {
                // Define mappings for each combination of shapes
                {(Geometry.Shapes.Point, Geometry.Shapes.Point), CollisionType.POINT_POINT},
                {(Geometry.Shapes.Point, Geometry.Shapes.Circle), CollisionType.POINT_CIRCLE},
                {(Geometry.Shapes.Point, Geometry.Shapes.AABB), CollisionType.POINT_AABB},
                {(Geometry.Shapes.Point, Geometry.Shapes.OBB), CollisionType.POINT_OBB},
                {(Geometry.Shapes.Circle, Geometry.Shapes.Point), CollisionType.CIRCLE_POINT},
                {(Geometry.Shapes.Circle, Geometry.Shapes.Circle), CollisionType.CIRCLE_CIRCLE},
                {(Geometry.Shapes.Circle, Geometry.Shapes.AABB), CollisionType.CIRCLE_AABB},
                {(Geometry.Shapes.Circle, Geometry.Shapes.OBB), CollisionType.CIRCLE_OBB},
                {(Geometry.Shapes.AABB, Geometry.Shapes.Point), CollisionType.AABB_POINT},
                {(Geometry.Shapes.AABB, Geometry.Shapes.Circle), CollisionType.AABB_CIRCLE},
                {(Geometry.Shapes.AABB, Geometry.Shapes.AABB), CollisionType.AABB_AABB},
                {(Geometry.Shapes.AABB, Geometry.Shapes.OBB), CollisionType.AABB_OBB},
                {(Geometry.Shapes.OBB, Geometry.Shapes.Point), CollisionType.OBB_POINT},
                {(Geometry.Shapes.OBB, Geometry.Shapes.Circle), CollisionType.OBB_CIRCLE},
                {(Geometry.Shapes.OBB, Geometry.Shapes.AABB), CollisionType.OBB_AABB},
                {(Geometry.Shapes.OBB, Geometry.Shapes.OBB), CollisionType.OBB_OBB}
                // Add more mappings as needed
            };

        public static CollisionType GetCollisionType(Geometry.Shapes shapeA, Geometry.Shapes shapeB)
        {
            if (collisionMapping.TryGetValue((shapeA, shapeB), out CollisionType collisionType))
            {
                return collisionType;
            }
            else
            {
                return CollisionType.INVALID;
            }
        }


        #region Collisions

        // Point Collisions
        public static Collision PointPointCollision(PhysicsCollider a, PhysicsCollider b)
		{
            Debug.Log("POINTPOINT");
            Collision collision = new Collision() { Colliding = false };
            return collision;
		}

        public static Collision PointCircleCollision(PhysicsCollider a, PhysicsCollider b)
		{
            Debug.Log("POINTCIRCLE");
            Collision collision = new Collision() { Colliding = false };

            PointCollider pointA = a.collisionGeometry as PointCollider;
            CircleCollider circleB = b.collisionGeometry as CircleCollider;

            return collision;
		}

        public static Collision PointAABBCollision(PhysicsCollider a, PhysicsCollider b)
		{
            Debug.Log("POINTAABB");
            Collision collision = new Collision() { Colliding = false };
            return collision;
		}

        // Circle Collisions
        public static Collision CircleCircleCollision(PhysicsCollider a, PhysicsCollider b)
		{
            //Debug.Log("CIRCLE CIRCLE");
            Collision collision = new Collision() { Colliding = false };

            CircleCollider CircleA = a.collisionGeometry as CircleCollider;
            CircleCollider CircleB = b.collisionGeometry as CircleCollider;

            float radii = CircleA.Radius + CircleB.Radius;

            
            Vector2 difference = (b.transform.position - a.transform.position);
            float magnitude = difference.magnitude;

            if (magnitude <= radii)
			{
                collision.Colliding = true;
                collision.normal = difference.normalized;
                collision.intersection = radii - magnitude;
			}

            return collision;
		}

        public static Collision CircleAABBCollision(PhysicsCollider a, PhysicsCollider b)
		{
            Debug.Log("CIRCLEAABB");
            Collision collision = new Collision() { Colliding = false };
            return collision;
		}

        // AABB Collisions

        public static Collision AABBAABBCollision(PhysicsCollider a, PhysicsCollider b)
		{
            Debug.Log("AABBAABB");
            Collision collision = new Collision() { Colliding = false };
            return collision;
		}

        // OBB

        // https://research.ncl.ac.uk/game/mastersdegree/gametechnologies/previousinformation/physics4collisiondetection/2017%20Tutorial%204%20-%20Collision%20Detection.pdf
        public static Collision OBBCollision(PhysicsCollider a, PhysicsCollider b)
		{
            Debug.Log("OBBOBB");
            Collision collision = new Collision() { Colliding = false };
            return collision;
		}
        // placeholder delegate, remove prior to shipping lol
        public static Collision Placeholder(PhysicsCollider a, PhysicsCollider b)
        {
            Debug.Log("PLACEHOLDER");
            Collision collision = new Collision() { Colliding = false };
            return collision;
        }

		#endregion

		#region Abstraction
		public static Collision HandleCollision(PhysicsCollider colliderA, PhysicsCollider colliderB)
		{
            Collision collision = new Collision() { Colliding = false }; 

            if (colliderA.collisionGeometry != null && colliderB.collisionGeometry != null)
			{
                if (colliderA.collisionGeometry.requiresUpdate)
                    colliderA.collisionGeometry.Update(colliderA.transform.position, colliderA.transform.localScale, colliderA.transform.rotation);

                if (colliderB.collisionGeometry.requiresUpdate)
                    colliderB.collisionGeometry.Update(colliderB.transform.position, colliderB.transform.localScale, colliderB.transform.rotation);

                collision = collisionTypes[GetCollisionType(colliderA.collisionGeometry.Shape, colliderB.collisionGeometry.Shape)].Invoke(colliderA, colliderB);

                Debug.DrawLine(colliderB.transform.position, colliderA.transform.position, Color.cyan);

                if (collision.Colliding)
                {
                    collision.colliderA = colliderA;
                    collision.colliderB = colliderB;

                    collision.Resolve();
                }
            }
           
            return collision;
		}
		#endregion
	}

    // Collision class that stores collision information.
    public class Collision
    {
        public PhysicsCollider colliderA;
        public PhysicsCollider colliderB;

        public Vector3 normal = Vector3.zero; // the direction to push the colliding objects away.
        public float intersection = 0; // the area of intersection.
        public bool Colliding = true;

        public void Resolve()
        {
            if (Colliding == false)
                return;

            colliderA.GetComponent<PhysicsBody>().Move(-normal * (intersection / 2f));
            colliderB.GetComponent<PhysicsBody>().Move(normal * (intersection / 2f));
        }
    }
}