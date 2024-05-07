using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsEngine.PhysicsColliders
{
    public class PointCollider : ColliderGeometry
    {
        public PointCollider(Vector3 position) : base(position) 
        {
            //base.Shape = MathU.Geometry.Geometry.Shapes.Point;
        }

    }
}

