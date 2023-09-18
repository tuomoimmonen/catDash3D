using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    [SerializeField] float minX, maxX; //min position on left and max on right
    void Start()
    {
        
    }

    void Update()
    {
        float inputX = Input.GetAxisRaw("Horizontal"); //left right movement, left -1, right 1, center 0

        transform.position += Vector3.right * inputX * speed * Time.deltaTime;

        
        //restrictions for movement "clamping"
        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
        
        if (transform.position.x < minX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }
        

        /*
        //screen wrap around
        if (transform.position.x > maxX)
        {
            transform.position = new Vector3(minX, transform.position.y, transform.position.z);
        }

        if (transform.position.x < minX)
        {
            transform.position = new Vector3(maxX, transform.position.y, transform.position.z);
        }
        */
    }
}
