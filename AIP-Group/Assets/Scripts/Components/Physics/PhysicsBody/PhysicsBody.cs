using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsEngine.Engine;
using PhysicsEngine.PhysicsColliders;
using MathU.Geometry;
using MathU;
using UnityEditor;


namespace PhysicsEngine.PhysicsBodies
{
    public class PhysicsBody : MonoBehaviour
    {
        public enum ForceType
        {
            Impulse,
            Passive
        }

        public enum BodyType
        {
            Dynamic,
            Kinetic,
            Static,
        }

        PhysicsCollider colliderComponent;

        public BodyType bodyType = BodyType.Dynamic;
        public Geometry.Shapes geometry = Geometry.Shapes.Circle;


        // Physics Config
        public Vector3 LinearVelocity = Vector3.zero; // v
        public Vector3 Acceleration = Vector3.zero; // delta v

        public float Rotation;
        public float RotationalVelocity;

        public float Density; // g/cm^3
        public float Mass = 1f; // area * density 
        public float InverseMass = 0.1f;
        public float Restitution; // e

        public float Area;
        public float Radius;
        public float Width;
        public float Height;


        // Cache Positions
        Vector3 lastPosition;
        Vector3 lastScale;
        Quaternion lastRotation;
        public void AddForce(Vector3 force, ForceType forceType)
        {
            if (this.bodyType != BodyType.Static)
            {
                Vector3 acceleration = force / this.Mass;
                this.LinearVelocity += acceleration * Time.deltaTime;
            }
        }

        public void Move(Vector3 force)
        {
            if (this.bodyType != BodyType.Static)
            {
                Vector3 newPosition = this.transform.position + force;

                MoveTo(newPosition);
            }
        }

        public void MoveTo(Vector3 position)
        {
            this.transform.position = position;

            if (this.GetComponent<PhysicsCollider>() != null)
            {
                this.GetComponent<PhysicsCollider>().collisionGeometry.requiresUpdate = HasMoved();
            }

            lastPosition = this.transform.position;
            lastRotation = this.transform.localRotation;
            lastScale = this.transform.localScale;
        }

        bool HasMoved()
		{
            return (this.transform.position != lastPosition || this.transform.rotation != lastRotation || this.transform.localScale != lastScale);
		}
        // Start is called before the first frame update
        void Start()
        {
            GetComponents();
        }



        void GetComponents()
		{
            colliderComponent = this.GetComponent<PhysicsCollider>();


            if (colliderComponent == null)
			{
                Debug.LogWarning(this.gameObject.name + " is missing a collider component");
			}
		}

        public void UpdateGeometry(Geometry.Shapes shape)
		{
            this.geometry = shape;
		}

        void UpdatePhysicalProperties()
        {
            if (this.geometry == Geometry.Shapes.Circle)
            {
                Radius = this.transform.localScale.x / 2f;
                Area = Mathf.PI * (Radius * Radius); // 2?r^2

                Width = 0;
                Height = 0;
            }
            else if (this.geometry == Geometry.Shapes.Polygon)
			{
                if (this.GetComponent<Polygon>())
				{
                    this.Area = this.GetComponent<Polygon>().Area();
				}
                Width = 1;
                Height = 1;
                Radius = 0;
			}
            else
            {
                Width = this.transform.localScale.x;
                Height = this.transform.localScale.y;
                Area = Width * Height;

                Radius = 0;
            }

            InverseMass = (bodyType == BodyType.Static) ? 0 : 1f / Mass;
            Restitution = Piecewise.Clamp(Restitution, 0f, 1f);
            Mass = Area * Density;

            lastPosition = this.transform.position;
            lastRotation = this.transform.rotation;
            lastScale = this.transform.localScale;
        }

        void UpdateCollider()
		{
            if (this.GetComponent<PhysicsCollider>())
			{
                this.GetComponent<PhysicsCollider>().SetFromPhysicsBody();
			}
		}

        private void OnValidate()
        {
            GetComponents();
            UpdatePhysicalProperties();
            UpdateCollider();
        }

		private void OnDrawGizmos()
		{
            if (HasMoved())
			{
                UpdatePhysicalProperties();
            }
        }
	}
}
