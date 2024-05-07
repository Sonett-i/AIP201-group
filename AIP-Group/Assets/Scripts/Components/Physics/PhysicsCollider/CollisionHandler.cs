using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MathU.Geometry;
using PhysicsEngine.PhysicsBodies;


namespace PhysicsEngine.PhysicsColliders
{
    public class CollisionHandler : MonoBehaviour
    {
        public enum CollisionType
        {
            CIRCLE_CIRCLE,
            CIRCLE_POLYGON,
            OBB_OBB,
            OBB_POLYGON,
            POLYGON_POLYGON,
            POLYGON_CIRCLE,
            INVALID
        }

        // Collision Disambiguation
        public delegate Collision CollisionDelegate(PhysicsCollider colliderA, PhysicsCollider colliderB);
        public static Dictionary<CollisionType, CollisionDelegate> collisionTypes = new Dictionary<CollisionType, CollisionDelegate>()
        {
            [CollisionType.CIRCLE_CIRCLE] = (colliderA, colliderB) => CircleCircleCollision(colliderA, colliderB),
			[CollisionType.CIRCLE_POLYGON] = (colliderA, colliderB) => CirclePolygonCollision(colliderA, colliderB),
			[CollisionType.POLYGON_CIRCLE] = (colliderA, colliderB) => PolygonCircleCollision(colliderA, colliderB),
            //[CollisionType.OBB_OBB] = (colliderA, colliderB) => OBBCollision(colliderA, colliderB),
            //[CollisionType.OBB_POLYGON] = (colliderA, colliderB) => PolygonOBBCollision(colliderA, colliderB),
			[CollisionType.POLYGON_POLYGON] = (colliderA, colliderB) => PolygonCollision(colliderA, colliderB),
            [CollisionType.INVALID] = (colliderA, colliderB) => Placeholder(colliderA, colliderB),
        };

        static Dictionary<(Geometry.Shapes, Geometry.Shapes), CollisionType> collisionMapping = new Dictionary<(Geometry.Shapes, Geometry.Shapes), CollisionType>
            {
                // Define mappings for each combination of shapes
                {(Geometry.Shapes.Circle, Geometry.Shapes.Circle), CollisionType.CIRCLE_CIRCLE},
                {(Geometry.Shapes.Circle, Geometry.Shapes.Polygon), CollisionType.CIRCLE_POLYGON},
                //{(Geometry.Shapes.OBB, Geometry.Shapes.OBB), CollisionType.OBB_OBB},
                //{(Geometry.Shapes.OBB, Geometry.Shapes.Polygon), CollisionType.OBB_POLYGON},
                {(Geometry.Shapes.Polygon, Geometry.Shapes.Polygon), CollisionType.POLYGON_POLYGON},
                {(Geometry.Shapes.Polygon, Geometry.Shapes.Circle), CollisionType.POLYGON_CIRCLE},

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

        // OBB
        public static Collision CirclePolygonCollision(PhysicsCollider a, PhysicsCollider b)
		{
            Collision collision = new Collision() { Colliding = false };

            CircleCollider colliderA = null;
            PolygonCollider colliderB = null;

            if (a.collisionGeometry.Shape is Geometry.Shapes.Circle)
			{
                colliderA = a.collisionGeometry as CircleCollider;
                colliderB = b.collisionGeometry as PolygonCollider;
			}

            if (colliderA != null && colliderB != null)
                collision = SAT.IntersectCirclePolygon(a.transform.position, colliderA.Radius, colliderB.TransformedVertices);

            return collision;
		}

        public static Collision PolygonCircleCollision(PhysicsCollider a, PhysicsCollider b)
        {
            Collision collision = new Collision() { Colliding = false };

            PolygonCollider colliderA = null;
            CircleCollider colliderB = null;

            if (a.collisionGeometry.Shape is Geometry.Shapes.Polygon)
            {
                colliderA = a.collisionGeometry as PolygonCollider;
                colliderB = b.collisionGeometry as CircleCollider;
            }

            if (colliderA != null && colliderB != null)
                collision = SAT.IntersectCirclePolygon(b.transform.position, colliderB.Radius, colliderA.TransformedVertices, true);

            return collision;
        }

        // https://research.ncl.ac.uk/game/mastersdegree/gametechnologies/previousinformation/physics4collisiondetection/2017%20Tutorial%204%20-%20Collision%20Detection.pdf
        public static Collision OBBCollision(PhysicsCollider a, PhysicsCollider b)
		{
            //Debug.Log("OBBOBB");
            Collision collision = new Collision() { Colliding = false };

            OBBCollider aCollider = a.collisionGeometry as OBBCollider;
            OBBCollider bCollider = b.collisionGeometry as OBBCollider;

            collision = SAT.IntersectPolygons(aCollider.TransformedVertices, bCollider.TransformedVertices);

            return collision;
		}

        public static Collision PolygonCollision(PhysicsCollider a, PhysicsCollider b)
		{
            Collision collision = new Collision() { Colliding = false };

            PolygonCollider aCollider = a.collisionGeometry as PolygonCollider;
            PolygonCollider bCollider = b.collisionGeometry as PolygonCollider;
            
            collision = SAT.IntersectPolygons(aCollider.TransformedVertices, bCollider.TransformedVertices);

            return collision;
        }

        public static Collision PolygonOBBCollision(PhysicsCollider a, PhysicsCollider b)
        {
            Collision collision = new Collision() { Colliding = false };

            OBBCollider aCollider = a.collisionGeometry as OBBCollider;
            PolygonCollider bCollider = b.collisionGeometry as PolygonCollider;

            collision = SAT.IntersectPolygons(aCollider.TransformedVertices, bCollider.Vertices);

            return collision;
        }

        // placeholder delegate, remove prior to shipping lol
        public static Collision Placeholder(PhysicsCollider a, PhysicsCollider b)
        {
            //Debug.Log("PLACEHOLDER");
            Collision collision = new Collision() { Colliding = false };
            return collision;
        }

		#endregion

		#region Abstraction

        public static void UpdateShapes(PhysicsCollider colliderA, PhysicsCollider colliderB)
		{
            if (colliderA.collisionGeometry.requiresUpdate)
            {
                colliderA.collisionGeometry.Update(colliderA.transform.position, colliderA.transform.localScale, colliderA.transform.rotation);

                if (colliderA.colliderShape == Geometry.Shapes.Polygon)
                {
                    colliderA.collisionGeometry.UpdateGeometry(colliderA.GetComponent<Polygon>().transformedVertices);
                }
            }

            if (colliderB.collisionGeometry.requiresUpdate)
            {
                colliderB.collisionGeometry.Update(colliderB.transform.position, colliderB.transform.localScale, colliderB.transform.rotation);
                if (colliderB.colliderShape == Geometry.Shapes.Polygon)
				{
                    colliderB.collisionGeometry.UpdateGeometry(colliderB.GetComponent<Polygon>().transformedVertices);
                }
            }
        }

        public static Collision HandleCollision(PhysicsCollider colliderA, PhysicsCollider colliderB)
		{
            Collision collision = new Collision() { Colliding = false };

            Color lineColour = PhysicsConfig.DefaultDebugColour;

            if (colliderA.collisionGeometry != null && colliderB.collisionGeometry != null)
			{
                collision = collisionTypes[GetCollisionType(colliderA.collisionGeometry.Shape, colliderB.collisionGeometry.Shape)].Invoke(colliderA, colliderB);

                

                if (collision.Colliding)
                {
                    collision.colliderA = colliderA;
                    collision.colliderB = colliderB;

                    collision.Resolve();
                    //Debug.Log("COLLIDING " + GetCollisionType(colliderA.collisionGeometry.Shape, colliderB.collisionGeometry.Shape));
                    lineColour = PhysicsConfig.CollidingColour;
                }

                Debug.DrawLine(colliderB.transform.position, colliderA.transform.position, lineColour);
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
            bool isTrigger = false;

            if (colliderA.isTrigger || colliderB.isTrigger)
			{
                isTrigger = true;
			}
            else
			{
                colliderA.GetComponent<PhysicsBody>().Move(-normal * (intersection / 2f));
                colliderB.GetComponent<PhysicsBody>().Move(normal * (intersection / 2f));
            }
               
            Broadcast(isTrigger);
        }

        public void Broadcast(bool isTrigger)
		{
            if (Colliding)
			{
                if (colliderA != null)
                {
                    if (isTrigger)
					{
                        colliderA.PhysicsTriggerEnter(this);

                    }
                    else
					{
                        colliderA.PhysicsCollisionEnter(this);

                    }
                }

                if (colliderB != null)
                {
                    if (isTrigger)
                    {
                        colliderB.PhysicsTriggerEnter(this);

                    }
                    else
                    {
                        colliderB.PhysicsCollisionEnter(this);

                    }
                }
            }
            //send events to each colliding object
		}
    }
}