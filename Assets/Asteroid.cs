using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour
{
    // Start is called before the first frame update

    public float speed = 7.0f; // Speed of the asteroid

    public float randomYvel;
    public float randomXvel;

    public Vector3 direction;



    // Start is called before the first frame update
    void Start()
    {

        randomYvel = UnityEngine.Random.Range(-speed, speed);


        randomXvel = UnityEngine.Random.Range(-speed, speed);


        direction = new Vector3(randomXvel, randomYvel, 0);



    }

    // Update is called once per frame
    void Update()
    {

        {

            transform.position += direction * speed * Time.deltaTime;
        }


    }




    void OnTriggerEnter2D(Collider2D collider)
    {

        Debug.Log("something is triggering");

       // Check if the enemy collided with the player
        if (collider.gameObject.CompareTag("Player"))
        {
            // Implement what happens when the enemy collides with the player
            // For example, reduce player's health, play a sound, etc.

            // Destroy the enemy
            ///  Destroy(gameObject);
            ///  
            Destroy(collider.gameObject);
        }
        // Check if the enemy collided with a bullet
        else if (collider.gameObject.CompareTag("Boid"))
        {
            // Implement what happens when the enemy is hit by a bullet
            // For example, play a sound, add score, etc.

            // Destroy the bullet
            collider.gameObject.SetActive(false);
            Debug.Log("collission happened");
            // Destroy the enemy
          
        }
    }

}