using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PhysicsEngine.Colliders
{
    // oriented bounding box
    public class OBBCollider : ColliderGeometry
    {
        public Vector2 position;
        public Vector2 min;
        public Vector2 max;

        public OBBCollider(Vector2 position, Vector2 dimensions)
        {
            this.position = position;

            this.min = new Vector2(this.position.x - dimensions.x / 2f, this.position.y - dimensions.y / 2f);
            this.max = new Vector2(this.position.x + dimensions.x / 2f, this.position.y + dimensions.y / 2f);
        }
    }
}
