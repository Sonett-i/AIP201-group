using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsEngine.PhysicsBodies;
using PhysicsEngine.PhysicsColliders;

public class PlayerControl : MonoBehaviour
{
    

    [SerializeField] float movementSpeed = 3;
    PhysicsBody pb;
    PhysicsCollider pc;

    // register events

    

    Vector3 force;
    Quaternion rotation = Quaternion.identity;

    void HandleInput()
	{
        force = new Vector3(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"), 0f);

        if (Input.GetMouseButton(0))
		{
            rotation = Quaternion.Euler(0, 0, -1);
		}
        else if (Input.GetMouseButton(1))
		{
            rotation = Quaternion.Euler(0, 0, 1);
		}
        else
		{
            rotation = Quaternion.Euler(0, 0, 0);
		}
	}

    private void isColliding(PhysicsEngine.PhysicsColliders.Collision collision)
	{
        Debug.Log("COLLIDING");
	}

    private void isTriggering(PhysicsEngine.PhysicsColliders.Collision collision)
	{
        Debug.Log("TRIGGER");
    }

    // Start is called before the first frame update
    void Start()
    {
        pb = this.GetComponent<PhysicsBody>();
        pc = this.GetComponent<PhysicsCollider>();

        pc.PhysicsCollisionEnterEvent += isColliding;
        pc.PhysicsTriggerEnterEvent += isTriggering;
    }

    // Update is called once per frame
    void Update()
    {
        HandleInput();
    }

	private void FixedUpdate()
	{
        pb.AddForce(force * movementSpeed, PhysicsBody.ForceType.Impulse);
        this.transform.rotation *= rotation;
	}
}
