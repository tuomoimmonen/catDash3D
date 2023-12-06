using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    [SerializeField] float speed = 1f;

    [SerializeField] float minX, maxX; //min position on left and max on right

    GameManager gameManager;

    [SerializeField] GameObject hitEffect;
    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        if(gameManager.gameEnded) 
        { 
            Destroy(gameObject);
            return;
        }

        float inputX = Input.GetAxisRaw("Horizontal"); //left right movement, left -1, right 1, center 0

        transform.position += (Vector3.right * inputX * speed * Time.deltaTime);

        
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
      if (isRunning)
      {
          // Automatically move the player forward
          rb.velocity = new Vector3(0, 0, moveSpeed);

          // Horizontal movement (left and right)
          float horizontal = Input.GetAxisRaw("Horizontal");
          Vector3 moveDirection = new Vector3(horizontal, 0, 0).normalized;

          // Apply movement to the Rigidbody
          rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, rb.velocity.z);
      }
      else
      {
          // Stop the player if not running
          //rb.velocity = Vector3.zero;
          float horizontal = Input.GetAxisRaw("Horizontal");
          Vector3 moveDirection = new Vector3(horizontal, 0, 0).normalized;
          rb.velocity = new Vector3(moveDirection.x * moveSpeed, rb.velocity.y, 0);
      }
      */

        /*
       // Horizontal movement
       float horizontal = Input.GetAxisRaw("Horizontal");
       float vertical = Input.GetAxisRaw("Vertical");

       Vector3 moveForward = new Vector3(0, 0, initialMoveSpeed).normalized;
       rb.velocity = new Vector3(moveForward.x, rb.velocity.y, moveForward.z * moveSpeed);

       /*
       // Calculate the new move speed based on acceleration
       if (vertical != 0)
       {
           currentMoveSpeed += acceleration * Time.deltaTime;
       }
       else
       {
           currentMoveSpeed = initialMoveSpeed;
       }


       // Set the velocity with the updated move speed
       rb.velocity = new Vector3(moveDirection.x * currentMoveSpeed, rb.velocity.y, moveDirection.z * currentMoveSpeed);
       */
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Spike")
        {
            Instantiate(hitEffect, transform.position, Quaternion.identity);
        }
    }

}
