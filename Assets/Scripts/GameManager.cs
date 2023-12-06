using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System;
using Cinemachine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] GameObject rightTutorial;
    [SerializeField] GameObject leftTutorial;
    [SerializeField] GameObject spaceKeyTutorial;
    [SerializeField] GameObject doubleJumpTutorial;
    [SerializeField] GameObject slideTutorial;
    public bool gameEnded = false;
    public bool tutorialComplete = false;
    bool rightArrowPressed = false;
    bool leftArrowPressed = false;
    bool spaceKeyPressed = false;
    bool doubleJumpPressed = false;
    public bool doubleJumped = false;
    bool slideKeyPressed = false;

    [SerializeField] GameObject[] playerCharacters;
    [SerializeField] GameObject[] femaleHats;
    [SerializeField] GameObject[] maleHats;

    CinemachineVirtualCamera playerCamera;

    public bool levelFinished = false;

    [SerializeField] GameObject finishLevelScreen;
    public float levelTimer;
    [SerializeField] TMP_Text levelTimerText;

    public int spaceKeyPresses = 0;

    PlayerController player;

    [SerializeField] GameObject audioPanel;
    [SerializeField] GameObject startGameCanvas;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        startGameCanvas.SetActive(true);
        Instantiate(playerCharacters[PlayerPrefs.GetInt("SelectedCharacter", 0)], transform.position, Quaternion.identity);
    }
    void Start()
    {
        levelFinished = false;
        finishLevelScreen.SetActive(false);
        levelTimer = 0;

        //femaleHats = GameObject.FindGameObjectsWithTag("femaleHats");
        //maleHats = GameObject.FindGameObjectsWithTag("maleHats");

        //get hats from spawned player
        femaleHats = FindObjectOfType<PlayerController>().playerHats;
        maleHats = FindObjectOfType <PlayerController>().playerHats;

        //make sure hats are off when game starts
        foreach(var hats in femaleHats){ hats.SetActive(false); }
        foreach(var hats in maleHats) { hats.SetActive(false); }

        //set the selected hat on
        //male
        if(PlayerPrefs.GetInt("SelectedCharacter") == 0)
        {
            if (PlayerPrefs.HasKey("SelectedMaleHat")) { maleHats[PlayerPrefs.GetInt("SelectedMaleHat", 0)].SetActive(true); }
            else
            {
                maleHats[0].SetActive(true);
            }
        }
        //female
        else if(PlayerPrefs.GetInt("SelectedCharacter") == 1)
        {
            if (PlayerPrefs.HasKey("SelectedFemaleHat"))
            {
                femaleHats[PlayerPrefs.GetInt("SelectedFemaleHat", 0)].SetActive(true);
            }
            else
            {
                femaleHats[0].SetActive(true);
            }
        }
        playerCamera = FindObjectOfType<CinemachineVirtualCamera>();
        playerCamera.m_Follow = GameObject.FindGameObjectWithTag("Player").transform;
        playerCamera.m_LookAt = GameObject.FindGameObjectWithTag("Player").transform;
        player = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        /*
        if(Input.GetKeyDown(KeyCode.Return) )
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
        */

        if(Input.GetKeyDown(KeyCode.Escape) )
        {
            Time.timeScale = 0f;
            audioPanel.SetActive(true);
        }


        GameTutorial();
        ChangeLevel();

        if (tutorialComplete && !levelFinished)
        {
            levelTimer += Time.deltaTime;
        }
    }

    private void GameTutorial()
    {
        
        if (SceneManager.GetActiveScene().buildIndex <= 5)
        {
            if(!rightArrowPressed) 
            {
                rightTutorial.SetActive(true); 
            }
            
            if ((Input.GetKeyDown(KeyCode.RightArrow) || Keyboard.current.rightArrowKey.wasPressedThisFrame) && !rightArrowPressed)
            {
                rightArrowPressed = true;
                rightTutorial.SetActive(false);
                leftTutorial.SetActive(true);
                CanvasManager.instance.SetAnimationTrigger("left");
            }
            if ((Input.GetKeyDown(KeyCode.LeftArrow) || Keyboard.current.leftArrowKey.wasPressedThisFrame) && !leftArrowPressed && rightArrowPressed)
            {
                leftTutorial.SetActive(false);
                spaceKeyTutorial.SetActive(true);
                leftArrowPressed = true;
                CanvasManager.instance.SetAnimationTrigger("space");
            }
            if((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow) || Keyboard.current.upArrowKey.wasPressedThisFrame) && !spaceKeyPressed && rightArrowPressed && leftArrowPressed)
            {
                spaceKeyPressed = true;
                spaceKeyTutorial.SetActive(false);
                doubleJumpTutorial.SetActive(true);
                CanvasManager.instance.SetAnimationTrigger("doubleSpace");
            }
            
            if(!doubleJumpPressed && rightArrowPressed && leftArrowPressed && spaceKeyPressed)
            {
                if(doubleJumped)
                {
                    doubleJumpPressed = true;
                    doubleJumpTutorial.SetActive(false);
                    slideTutorial.SetActive(true);
                    CanvasManager.instance.SetAnimationTrigger("shift");
                }
            }
            if((Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.DownArrow) || Keyboard.current.downArrowKey.wasPressedThisFrame) && !slideKeyPressed && doubleJumpPressed)
            {
                slideKeyPressed = true;
                slideTutorial.SetActive(false);
                tutorialComplete = true;
            }
        }
        else
        {
            tutorialComplete = true;
        }
    }

    private void ChangeLevel()
    {
        if (levelFinished)
        {
            StartCoroutine(ChangeScene());
        }
    }
    private IEnumerator ChangeScene()
    {
        yield return new WaitForSeconds(5f);
        finishLevelScreen.SetActive(true);
        levelTimerText.text = levelTimer.ToString("F2") + "s";
    }

    public void BackToMainMenu()
    {
        StartCoroutine(ToMainMenu());
    }

    private IEnumerator ToMainMenu()
    {

        yield return new WaitForSeconds(1f);
    }
}
