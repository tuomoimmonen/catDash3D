
using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    CatDash controls;

    private float speedDebuff = 0.85f;
    public float initialMoveSpeed = 1.05f;

    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    public float slideForce = 2f;
    private float currentSlideForce = 0f;
    public LayerMask groundLayer;

    private Rigidbody rb;
    public bool isGrounded;

    public Transform groundCheck;
    public float groundCheckDistance = 0.01f;

    public float customGravity = -15f;

    public bool isChangingLanes = false;

    public float laneSwitchSpeed = 2f;
    public float laneDistance = 2f;
    public int currentLane = 1;
    private Vector3 targetPosition;

    public Animator playerAnim;

    public float acceleration = 0.015f;
    public float maxSpeed = 11f;

    public float currentSpeed;

    public float laneSwitchCooldown = 0.5f;

    private bool isJumping = false;
    private bool canSlide = false;

    public float slideAcceleration = 2f;
    public float jumpAcceleration = 5f;

    public bool buffed = false;

    private bool hit = false;
    public bool canMove = true;
    public bool isSliding = false;
    [SerializeField] float hitForce = 10f;
    public float jumpCooldown = 0.05f;
    private bool canDoubleJump;
    public bool isDoubleJumping;
    private bool jumpForceAdded = false;
    private bool doubleJumpForceAdded = false;
    private float lastJumpTime;

    public bool isFlying = false;
    public bool isSledding = false;
    public bool isBuffHit = false;

    [SerializeField] GameObject destroyEffect;

    [SerializeField] public GameObject[] buffEffects;
    [SerializeField] public GameObject[] playerHats;

    bool levelFinished = false;

    private void Awake()
    {
        controls = new CatDash();
        controls.Enable();
    }
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        playerAnim = GetComponent<Animator>();
        currentSpeed = moveSpeed;
        targetPosition = rb.position;
        canMove = true;

        
        foreach (GameObject buff in buffEffects)
        {
            buff.SetActive(false);
        }

    }

    void FixedUpdate()
    {
        if(GameManager.instance.tutorialComplete != true) { currentSpeed = 0; }

        
        // Move player forward
        Vector3 forwardMovement = transform.forward * currentSpeed * Time.fixedDeltaTime;

        // Calculate horizontal movement
        Vector3 targetHorizontalPosition = new Vector3(targetPosition.x, rb.position.y, rb.position.z);
        Vector3 direction = (targetHorizontalPosition - rb.position);
        Vector3 horizontalMovementWithoutLerp = direction * laneSwitchSpeed * Time.fixedDeltaTime;

        // Move the player using the combined movement
        rb.MovePosition(rb.position + forwardMovement + horizontalMovementWithoutLerp);

        // Check if grounded
        //isGrounded = Physics.CheckSphere(groundCheck.position, groundCheckRadius, groundLayer);

        //jump fix not to feel floty
        if (!isGrounded)
        {
            rb.AddForce(Vector3.up * customGravity, ForceMode.Acceleration); 
        }
        
        
        if (isJumping && !jumpForceAdded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            jumpForceAdded = true;
            isJumping = false;
        }
        
        if (isDoubleJumping && !doubleJumpForceAdded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            doubleJumpForceAdded = true;
            isDoubleJumping = false;
        }
        

        if (canSlide == true)
        {
            if(GameManager.instance.tutorialComplete) { rb.AddForce(Vector3.forward * slideForce, ForceMode.Impulse); }
            currentSlideForce = slideForce;
            if (!isSledding) { playerAnim.SetTrigger("slide"); }   
            canSlide = false;
        }

        if (hit == true)
        {
            rb.AddForce(Vector3.forward * hitForce, ForceMode.Impulse);
            hit = false;
        }

        if (!GameManager.instance.tutorialComplete) { return; }

        float velocityX = Vector3.Dot(horizontalMovementWithoutLerp.normalized, transform.right);
        playerAnim.SetFloat("xVelocity", velocityX, 0.1f, Time.deltaTime);


    }

    void Update()
    {
        //gametutorial first
        if (GameManager.instance.tutorialComplete != true) { currentSpeed = 0; }

        LevelFinished();

        // Accelerate forward movement

        if (levelFinished)
        {
            currentSpeed = Mathf.Lerp(currentSpeed, 0f, Time.deltaTime);
        }
        else
        {
            currentSpeed += acceleration * Time.deltaTime;
            currentSpeed = Mathf.Clamp(currentSpeed, initialMoveSpeed, maxSpeed);
        }


        //handle animations
        SetAnimations();

        //switch lanes
        LaneSwitch();

        //jump
        Jump();

        isGrounded = CheckGround();

        bool CheckGround()
        {
            RaycastHit hit;
            Vector3 raycastOrigin = transform.position;
            bool raycastHit = Physics.Raycast(raycastOrigin, Vector3.down, out hit, groundCheckDistance, groundLayer);
            return raycastHit;
        }

    }



    private void OnCollisionEnter(Collision collision)
    {
        if(collision.collider == null) return;

        if(collision.collider.tag == "Obstacle" && !buffed) 
        {
            if(isFlying) { return; }
            SoundManager.instance.PlayAudioClip(4);
            CameraShake.instance.ShakeIt(0.4f, 0.4f);
            collision.gameObject.GetComponent<Collider>().enabled = false;
            playerAnim.SetTrigger("hit");
            currentSpeed = currentSpeed * speedDebuff;
            hit = true;
            canMove = false;
        }
        else if (collision.collider.tag == "Obstacle" && buffed)
        {
            collision.gameObject.GetComponent<Collider>().enabled = false;
            Instantiate(destroyEffect, transform.position, Quaternion.identity);
            Destroy(collision.gameObject);
            canMove = true;
        }

    }

    public void ProcessHit()
    {
        canMove = true;
    }

    public void ProcessSlide()
    {
        isSliding = false;
        gameObject.layer = 0;
    }


    private void SetAnimations()
    {
        AnimatorStateInfo stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);

        if(isGrounded)
        {
            jumpForceAdded = false;
            doubleJumpForceAdded = false;
        }

        if (!GameManager.instance.tutorialComplete) { return; }

        if (stateInfo.IsName("walk/run"))
        {
            if (currentSpeed < 1.2f)
            {
                playerAnim.speed = currentSpeed * 1f;
            }
            else if (currentSpeed > 1.7f)
            {
                playerAnim.speed = currentSpeed * 0.65f;
            }
        }
        else
        {
            playerAnim.speed = 1f;
        }

        playerAnim.SetBool("isWalking", rb.velocity.z != 0);

        float velocityY = Vector3.Dot(rb.position, transform.up);
        playerAnim.SetFloat("yVelocity", velocityY, 0.1f, Time.deltaTime);
        currentSlideForce = Mathf.Clamp(currentSlideForce, 0, slideForce);
        float normalizedValue = currentSlideForce / slideForce;
        playerAnim.SetFloat("slideVelocity", normalizedValue, 0.1f, Time.deltaTime);
        playerAnim.SetFloat("zVelocity", currentSpeed, 0.1f, Time.deltaTime);
    }

    IEnumerator TriggerDoubleJump()
    {
        yield return new WaitForSeconds(0.1f);
        playerAnim.SetBool("isDoubleJumping", true);
    }

    private void LaneSwitch()
    {
        if ((Input.GetKeyDown(KeyCode.LeftArrow) || Keyboard.current.leftArrowKey.wasPressedThisFrame) && currentLane > 0 && !isChangingLanes && isGrounded && canMove && !isSliding)
        {
            SoundManager.instance.PlayAudioClip(6);
            if (!isBuffHit)
            {
                if (!isSledding && !isFlying)
                {
                    playerAnim.SetTrigger("laneChange");
                }
            }
            isChangingLanes = true;
            currentLane--;
            StartCoroutine(SwitchingLanes());
        }
        else if ((Input.GetKeyDown(KeyCode.RightArrow) || Keyboard.current.rightArrowKey.wasPressedThisFrame) && currentLane < 2 && !isChangingLanes && isGrounded && canMove && !isSliding)
        {
            SoundManager.instance.PlayAudioClip(6);
            if (!isBuffHit)
            {
                if (!isSledding && !isFlying)
                {
                    playerAnim.SetTrigger("laneChange");
                }
            }
            isChangingLanes = true;
            currentLane++;
            StartCoroutine(SwitchingLanes());
        }
    }

    private void Jump()
    {
        if ((Input.GetButtonDown("Jump") || Input.GetKeyDown(KeyCode.UpArrow) || Keyboard.current.upArrowKey.wasPressedThisFrame) && !isChangingLanes && canMove && !isSliding && Time.time > lastJumpTime + jumpCooldown)
        {
            if(isFlying) { return; }

            if(isGrounded)
            {
                SoundManager.instance.PlayAudioClip(1);
                isJumping = true; //add physic in the fixedupdate
                if(!isSledding)
                {
                    playerAnim.SetTrigger("jump");
                }
                //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canDoubleJump = true;
                lastJumpTime = Time.time;
            }
            else if(canDoubleJump)
            {
                isDoubleJumping = true;
                GameManager.instance.doubleJumped = true;
                SoundManager.instance.PlayAudioClip(1);
                if (!isSledding)
                {
                    playerAnim.SetTrigger("doubleJump");
                }
                //rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
                canDoubleJump = false;
                lastJumpTime = Time.time;
            }
        }

        if ((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.DownArrow) || Keyboard.current.downArrowKey.wasPressedThisFrame) && isGrounded && !isChangingLanes && canMove && !isSliding && !isFlying)
        {
            if(isSledding) { return; }
            SoundManager.instance.PlayAudioClip(5);
            gameObject.layer = 9;
            isSliding = true;
            currentSlideForce += slideAcceleration * Time.deltaTime;
            canSlide = true;
        }
        else
        {
            currentSlideForce -= slideAcceleration * Time.deltaTime;

            if(currentSlideForce < 0)
            {
                currentSlideForce = 0;
                isSliding = false;
            }
        }
    }

    IEnumerator SwitchingLanes()
    {
        float targetXposition = (currentLane - 1) * laneDistance;
        targetPosition.x = targetXposition;
        yield return new WaitForSeconds(0.5f);
        isChangingLanes = false; 
    }

    private void LevelFinished()
    {
        if (GameManager.instance.levelFinished)
        {
            levelFinished = true;
            playerAnim.SetTrigger("winDance");
        }
    }
}

