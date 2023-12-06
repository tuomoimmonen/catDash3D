using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControllerLanes : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float laneSwitchSpeed = 2f;
    public float laneDistance = 2f;

    private Rigidbody rb;
    private int currentLane = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        // Move player forward
        Vector3 forwardMovement = transform.forward * moveSpeed * Time.deltaTime;
        rb.MovePosition(rb.position + forwardMovement);

        // Switch lanes
        if (Input.GetKeyDown(KeyCode.LeftArrow) && currentLane > 0)
        {
            currentLane--;
            SwitchLane();
        }
        else if (Input.GetKeyDown(KeyCode.RightArrow) && currentLane < 2)
        {
            currentLane++;
            SwitchLane();
        }
    }

    void SwitchLane()
    {
        float targetXPosition = (currentLane - 1) * laneDistance;
        Vector3 targetPosition = new Vector3(targetXPosition, rb.position.y, rb.position.z);
        StartCoroutine(SmoothLaneSwitch(targetPosition));
    }

    IEnumerator SmoothLaneSwitch(Vector3 targetPosition)
    {
        while (Mathf.Abs(rb.position.x - targetPosition.x) > 0.1f)
        {
            Vector3 newPosition = Vector3.Lerp(rb.position, targetPosition, laneSwitchSpeed * Time.deltaTime);
            rb.MovePosition(newPosition);
            yield return null;
        }
    }
}