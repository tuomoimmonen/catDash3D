using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] float speedEasy = 1f; 
    [SerializeField] float speedHard = 3f;

    float speed;

    Spawner spawner;

    GameManager manager;

    public bool canHit = true;
    void Start()
    {
        manager = FindObjectOfType<GameManager>();
        spawner = FindObjectOfType<Spawner>();
        speed = Mathf.Lerp(speedEasy, speedHard, spawner.GetDifficultyPercent());
    }

    void Update()
    {
        //math.lerp to increase difficulty over time (min, max, multiplier)
        transform.position += Vector3.back * speed * Time.deltaTime; //vector3.back towards the player
    }

    private void OnTriggerEnter(Collider other)
    {
        if (canHit && other.tag == "Player")
        {
            canHit = false;
            Destroy(gameObject);
        }
    }
}
