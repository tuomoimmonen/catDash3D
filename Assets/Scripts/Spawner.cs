using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPointStart; //all of the spawnpoints
    [SerializeField] List<Transform> spawnPoints; //the points where the spawning happens
    [SerializeField] int numberOfSpawns; //how many spawns per iteration

    [SerializeField] float timeBetweenSpawns;
    float nextSpawnTime;

    [SerializeField] GameObject spikeBall; 

    void Start()
    {
        
    }

    void Update()
    {
        //timer
        if(Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + timeBetweenSpawns; //reset the timer

            //first loop to spawn the objects
            for (int i = 0; i < numberOfSpawns; i++) 
            {
                Transform randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                Instantiate(spikeBall, randomSpawnPoint.position, randomSpawnPoint.rotation);

                spawnPoints.Remove(randomSpawnPoint);
            }
            //clear the list
            spawnPoints.Clear();
            //fill the list with ORIGINAL spawnpoints
            for (int i = 0; i < spawnPointStart.Count; i++)
            {
                spawnPoints.Add(spawnPointStart[i]);
            }
        }
        
    }
}
