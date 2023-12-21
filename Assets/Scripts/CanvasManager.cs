using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CanvasManager : MonoBehaviour
{
    public static CanvasManager instance;

    [SerializeField] Slider buffSlider;
    [SerializeField] TMP_Text timerText;
    [SerializeField] TMP_Text currentLevelText;

    private float buffDuration;
    bool buffActive = false;

    int selectedLevel;

    Animator animator;

    [SerializeField] GameObject audioPanel;

    private void Awake()
    {
        if(instance != this && instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;

    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        
    }
    private void Update()
    {
        currentLevelText.text = SceneManager.GetActiveScene().buildIndex.ToString();
        if (buffActive)
        {
            StartCoroutine(StartSlider());
        }
    }
    public void BackToMainMenu()
    {
        Time.timeScale = 1f;
        StartCoroutine(StartTransition());
    }

    IEnumerator StartTransition()
    {
        SoundManager.instance.PlayAudioClip(8);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene("Mainmenu");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        StartCoroutine(LevelTransition());
    }

    IEnumerator LevelTransition()
    {
        SoundManager.instance.PlayAudioClip(8);
        int currentSceneIndex = SceneManager.GetActiveScene().buildIndex;
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene((currentSceneIndex + 1) % 6);
    }

    public void ShowBuffSlider(float duration)
    {
        buffSlider.gameObject.SetActive(true);
        buffDuration = duration;
        buffActive = true;
    }

    IEnumerator StartSlider()
    {
        buffDuration -= Time.deltaTime;
        buffSlider.value = buffDuration;
        timerText.text = buffDuration.ToString("F0");
        yield return new WaitForSeconds(buffDuration);
        buffActive = false;
        buffSlider.gameObject.SetActive(false);
    }

    public void SetAnimationTrigger(string triggerName)
    {
        animator.SetTrigger(triggerName.ToString());
    }

    public void SelectLevelToLoad(int level)
    {
        Time.timeScale = 1f;
        selectedLevel = level;
        StartCoroutine(StartSelectedLevel());
    }

    private IEnumerator StartSelectedLevel()
    {
        SoundManager.instance.PlayAudioClip(8);
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(selectedLevel);
    }

    public void MovementButton(KeyCode keyCode)
    {
        Input.GetKeyDown(keyCode);
    }

    public void LeftMovement()
    {
        Input.GetKeyDown(KeyCode.LeftArrow);
    }

    public void RightMovement()
    {
        Input.GetKeyDown(KeyCode.RightArrow);
    }

    public void JumpMovement()
    {
        Input.GetButtonDown("Jump");
    }

    public void SlideMovement()
    {
        Input.GetKeyDown(KeyCode.LeftShift);
    }

    public void OpenAudioPanel()
    {
        Time.timeScale = 0f;
        audioPanel.SetActive(true);
    }
}
