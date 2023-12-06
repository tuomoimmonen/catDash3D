using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScoreBoard : MonoBehaviour
{
    public static ScoreBoard instance;
    public float[] topScores = new float[3]; //top 3 array

    [SerializeField] TMP_Text scoreTexts;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    private void Start()
    {
        topScores = new float[3];
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            for (int i = 0; i < 3; i++)
            {
                topScores[i] = PlayerPrefs.GetFloat("topTimesLevel1" + i, 100);
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 2)
        {
            for (int i = 0; i < 3; i++)
            {
                topScores[i] = PlayerPrefs.GetFloat("topTimesLevel2" + i, 100);
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 3)
        {
            for (int i = 0; i < 3; i++)
            {
                topScores[i] = PlayerPrefs.GetFloat("topTimesLevel3" + i, 100);
            }
        }
        else if (SceneManager.GetActiveScene().buildIndex == 4)
        {
            for (int i = 0; i < 3; i++)
            {
                topScores[i] = PlayerPrefs.GetFloat("topTimesLevel4" + i, 100);
            }
        }
        else if(SceneManager.GetActiveScene().buildIndex == 5)
        {
            for (int i = 0; i < 3; i++)
            {
                topScores[i] = PlayerPrefs.GetFloat("topTimesLevel5" + i, 100);
            }
        }
        /*
        for(int i = 0; i < 3; i++)
        {
            topScores[i] = PlayerPrefs.GetFloat("topTimes" + i, 0);
        }
        */
    }

    private void Update()
    {

    }

    public void AddScore(float newScore, int sceneNumber)
    {
        for (int i = 0; i < topScores.Length; i++)
        {
            if(sceneNumber == 1)
            {
                if (newScore < topScores[i])
                {
                    for (int j = topScores.Length - 1; j > i; j--) //nested, start from the index 2 of array
                    {
                        topScores[j] = topScores[j - 1];
                    }

                    topScores[i] = newScore; //set the new score
                    PlayerPrefs.SetFloat("topTimesLevel1" + i, newScore); //save to playerprefs
                    PlayerPrefs.Save();
                    break; //leave the loop, no need to go through the rest
                }

                for (int x = 0; x < 3; x++)
                {
                    topScores[x] = PlayerPrefs.GetFloat("topTimesLevel1" + x, 100);
                }
            }
            else if (sceneNumber == 2)
            {
                if (newScore < topScores[i])
                {
                    for (int j = topScores.Length - 1; j > i; j--) //nested, start from the index 2 of array
                    {
                        topScores[j] = topScores[j - 1];
                    }

                    topScores[i] = newScore; //set the new score
                    PlayerPrefs.SetFloat("topTimesLevel2" + i, newScore); //save to playerprefs
                    PlayerPrefs.Save();
                    break; //leave the loop, no need to go through the rest
                }

                for (int x = 0; x < 3; x++)
                {
                    topScores[x] = PlayerPrefs.GetFloat("topTimesLevel2" + x, 100);
                }
            }
            else if(sceneNumber == 3)
            {
                if (newScore < topScores[i])
                {
                    for (int j = topScores.Length - 1; j > i; j--) //nested, start from the index 2 of array
                    {
                        topScores[j] = topScores[j - 1];
                    }

                    topScores[i] = newScore; //set the new score
                    PlayerPrefs.SetFloat("topTimesLevel3" + i, newScore); //save to playerprefs
                    PlayerPrefs.Save();
                    break; //leave the loop, no need to go through the rest
                }

                for (int x = 0; x < 3; x++)
                {
                    topScores[x] = PlayerPrefs.GetFloat("topTimesLevel3" + x, 100);
                }
            }
            else if (sceneNumber == 4)
            {
                if (newScore < topScores[i])
                {
                    for (int j = topScores.Length - 1; j > i; j--) //nested, start from the index 2 of array
                    {
                        topScores[j] = topScores[j - 1];
                    }

                    topScores[i] = newScore; //set the new score
                    PlayerPrefs.SetFloat("topTimesLevel4" + i, newScore); //save to playerprefs
                    PlayerPrefs.Save();
                    break; //leave the loop, no need to go through the rest
                }

                for (int x = 0; x < 3; x++)
                {
                    topScores[x] = PlayerPrefs.GetFloat("topTimesLevel4" + x, 100);
                }
            }
            else if (sceneNumber == 5)
            {
                if (newScore < topScores[i])
                {
                    for (int j = topScores.Length - 1; j > i; j--) //nested, start from the index 2 of array
                    {
                        topScores[j] = topScores[j - 1];
                    }

                    topScores[i] = newScore; //set the new score
                    PlayerPrefs.SetFloat("topTimesLevel5" + i, newScore); //save to playerprefs
                    PlayerPrefs.Save();
                    break; //leave the loop, no need to go through the rest
                }

                for (int x = 0; x < 3; x++)
                {
                    topScores[x] = PlayerPrefs.GetFloat("topTimesLevel5" + x, 100);
                }
            }
            /*
            if (newScore > topScores[i])
            {
                for (int j = topScores.Length - 1; j > i; j--) //nested, start from the index 2 of array
                {
                    topScores[j] = topScores[j - 1];
                }

                topScores[i] = newScore; //set the new score
                PlayerPrefs.SetFloat("topTimes" + i, newScore); //save to playerprefs
                PlayerPrefs.Save();
                break; //leave the loop, no need to go through the rest
            }
            */
        }
    }

    public void UpdateScoreboardText()
    {
        scoreTexts.text = "";

        for (int i = 0; i < 3; i++)
        {

            if (topScores[i] == 100)
            {
                scoreTexts.text += (i + 1) + ". " + "0,00" + "s" + "\n";
            }
            else
            {
                scoreTexts.text += (i + 1) + ". " + topScores[i].ToString("F2") + "s" + "\n";
            }
            
        }
    }

}
