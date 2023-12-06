using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine.SceneManagement;
using UnityEngine;

public class Finish : MonoBehaviour
{
    [SerializeField] GameObject[] finishEffects;

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player" && GameManager.instance.levelFinished == false)
        {
            GameManager.instance.levelFinished = true;
            SoundManager.instance.PlayAudioClip(7);
            Instantiate(finishEffects[Random.Range(0, finishEffects.Length)], transform.position, Quaternion.identity);
            ScoreBoard.instance.AddScore(GameManager.instance.levelTimer, SceneManager.GetActiveScene().buildIndex);
            ScoreBoard.instance.UpdateScoreboardText();
        }
    }

}
