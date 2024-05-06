using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour



{
    public int spawnamount;

    public GameObject boid;

    public List<GameObject> boids = new List<GameObject>();

    public int groupnumber;

    // Start is called before the first frame update
    void Start()
    {
        spawnBoids();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void spawnBoids()
    {

        for (int i = 0; i < spawnamount; i++)
        {

            GameObject newboid = Instantiate(boid);


            Vector3 spawnpos = new Vector3(transform.position.x, transform.position.y);

            newboid.transform.position = spawnpos;

            newboid.GetComponent<Boid>().groupnumber = groupnumber;
            boids.Add(newboid);


        }


    }

    public List<GameObject> getboidsList (){


        return boids;
        }
}
