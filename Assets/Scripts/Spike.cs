using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spike : MonoBehaviour
{
    [SerializeField] float speed = 1f;
    void Start()
    {
        
    }

    void Update()
    {
        transform.position += Vector3.back * speed * Time.deltaTime; //vector3.back towards the player
    }
}
