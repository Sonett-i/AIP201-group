using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsEngine.Colliders
{
    public class PhysicsCollisions : MonoBehaviour
    {
        public enum ColliderShapes
        {
            Circle,
            AABB,
            OBB,
            Polygon
        }
        public enum CollisionTypes
        {
            POINT_POINT,
            POINT_CIRCLE,
            POINT_AABB,
            CIRCLE_CIRCLE,
            CIRCLE_AABB,
            AABB_AABB,
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