using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [SerializeField] float speedEasy = 1f;
    [SerializeField] float speedHard = 3f;

    float speed;

    Spawner spawner;
    void Start()
    {
        spawner = FindObjectOfType<Spawner>();
        speed = Mathf.Lerp(speedEasy, speedHard, spawner.GetDifficultyPercent());
    }

    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime;
    }
}
