using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    public enum Geometry
	{
        Circle,
        Rectangle,
        Polygon,
        Cube,
        Sphere,
    }

    public BodyType bodyType = BodyType.Dynamic;
    public Geometry geometry = Geometry.Circle;

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
            this.transform.position += force;
            if (this.GetComponent<PhysicsCollider>() != null)
			{

			}
		}
	}

    public void MoveTo(Vector3 position)
	{
        this.transform.position = position;
	}

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void UpdatePhysicalProperties()
	{
        if (this.geometry == Geometry.Circle || this.geometry == Geometry.Sphere)
		{
            Radius = this.transform.localScale.x / 2f;
            Area = Mathf.PI * (Radius * Radius); // 2?r^2

        }
        else
		{
            Width = this.transform.localScale.x;
            Height = this.transform.localScale.y;
            Area = Width * Height;
        }

        
        InverseMass = (bodyType == BodyType.Static) ? 0 : 1f / Mass;
        Restitution = MathU.Clamp(Restitution, 0f, 1f);
        Mass = Area * Density;
    }

	private void OnValidate()
	{
        UpdatePhysicalProperties();
	}
}
