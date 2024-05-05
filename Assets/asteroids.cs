using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class asteroids : MonoBehaviour
{
    // Start is called before the first frame update
    public List<Transform> spawnPoints;
    
    public List<GameObject> DiffAsteroids;

    public List<GameObject> AllAsteroids;
    public float minSpawnTime = 0.2f;
    public float maxSpawnTime = 0.3f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(SpawnEnemies());
    }
    IEnumerator SpawnEnemies()
    {
        while (true) // Infinite loop to keep spawning enemies
        {
            // Select a random spawn point
            Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];


            GameObject randomAsteroid = DiffAsteroids[Random.Range(0, DiffAsteroids.Count)];

            // Instantiate an enemy at the spawn point
            Instantiate(randomAsteroid, spawnPoint.position, spawnPoint.rotation);



            AllAsteroids.Add(randomAsteroid);


            // Wait for a random time before spawning the next enemy
            yield return new WaitForSeconds(Random.Range(minSpawnTime, maxSpawnTime));
        }
    }
    // Update is called once per frame
    void Update()
    {

    }
}
