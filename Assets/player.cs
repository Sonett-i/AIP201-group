using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // Start is called before the first frame update
    public float speed = 3;
    void Start()
    {
    
    }

    // Update is called once per frame
    void Update()
    {
        rotateTomouse();



        if (Input.GetKey(KeyCode.W))
        {

            Vector3 up = new Vector3(0, 0.007f, 0);
            Vector3 newposition = transform.position + (up);
            transform.position = newposition;
            

           
        }



        if (Input.GetKey(KeyCode.A))
        {

            Vector3 left = new Vector3(-0.007f,0 , 0);
            Vector3 newposition = transform.position + (left);
            transform.position = newposition;



        }

        if (Input.GetKey(KeyCode.D))
        {

            Vector3 right = new Vector3(0.007f,0 , 0);
            Vector3 newposition = transform.position + (right);
            transform.position = newposition;



        }

        if (Input.GetKey(KeyCode.S))
        {

            Vector3 down = new Vector3(0, -0.007f, 0);
            Vector3 newposition = transform.position + (down);
            transform.position = newposition;



        }

















    }





    public void rotateTomouse()
    {

        // Convert the mouse position to world position
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        // Ensure the z-coordinate is equal to the player's z-coordinate
        mouseWorldPosition.z = transform.position.z;

        // Calculate the direction from the player to the mouse position
        Vector3 direction = mouseWorldPosition - transform.position;

        // Calculate the angle to rotate
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Set the player's rotation to this angle
        transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle + 270)); //i subrtract -270 from the angle since the sprite is facing down
    }




}
