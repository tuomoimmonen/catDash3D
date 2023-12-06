using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    [SerializeField] List<Transform> spawnPointStart; //all of the spawnpoints
    [SerializeField] List<Transform> spawnPoints; //the points where the spawning happens
    [SerializeField] int numberOfSpawns; //how many spawns per iteration

    [SerializeField] float timeBetweenSpawnsEasy;
    [SerializeField] float timeBetweenSpawnsHard;
    float nextSpawnTime;

    [SerializeField] GameObject spikeBall;

    [SerializeField] Transform[] environmentSpawnpoints;
    [SerializeField] GameObject[] environmentPrefabs;

    [SerializeField] float timeBetweenEnvironmentSpawnsMin;
    [SerializeField] float timeBetweenEnvironmentSpawnsMax;
    float nextEnvironmentSpawnTime;

    GameManager manager;

    [SerializeField] float timeUntilMaxDifficulty;
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(manager.gameEnded == true) return;
        if(manager.tutorialComplete == false) return;
        //timer
        if(Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + Mathf.Lerp(timeBetweenSpawnsEasy, timeBetweenSpawnsHard, GetDifficultyPercent()); //reset the timer
          

            List<Vector3> spawnedPositions = new List<Vector3>();
            float minDistance = 2.0f; // Set the minimum distance between spawned objects

            for (int i = 0; i < numberOfSpawns; i++)
            {
                Transform randomSpawnPoint; //random spawn spot
                bool isValidSpawnPoint;

                do
                {
                    randomSpawnPoint = spawnPoints[Random.Range(0, spawnPoints.Count)];
                    isValidSpawnPoint = true;

                    //check the random spots distance
                    foreach (Vector3 spawnedPosition in spawnedPositions)
                    {
                        if (Vector3.Distance(randomSpawnPoint.position, spawnedPosition) < minDistance)
                        {
                            isValidSpawnPoint = false;
                            break;
                        }
                    }
                } while (!isValidSpawnPoint); //break out when valid spot is found

                Instantiate(spikeBall, randomSpawnPoint.position, randomSpawnPoint.rotation);
                spawnedPositions.Add(randomSpawnPoint.position);
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

        if (Time.time > nextEnvironmentSpawnTime)
        {
            for (int i = 0; i < environmentSpawnpoints.Length; i++)
            {
                GameObject randomPrefab = environmentPrefabs[Random.Range(0, environmentPrefabs.Length)];
                Instantiate(randomPrefab, environmentSpawnpoints[i].position, environmentSpawnpoints[i].localRotation);
            }

            //nextEnvironmentSpawnTime = Time.time + timeBetweenEnvironmentSpawns;
            nextEnvironmentSpawnTime = Time.time + Random.Range(timeBetweenEnvironmentSpawnsMin, timeBetweenEnvironmentSpawnsMax);
        }
        
    }

    public float GetDifficultyPercent()
    {
        return Mathf.Clamp01(Time.timeSinceLevelLoad / timeUntilMaxDifficulty); //return value between 0 and 1
    }
}
