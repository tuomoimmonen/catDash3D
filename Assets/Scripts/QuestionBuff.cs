using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class QuestionBuff : MonoBehaviour
{
    [SerializeField] GameObject smokeEffect;
    PlayerController player;
    GameObject speedBuffEffect1;
    GameObject speedBuffEffect2;
    GameObject flyCape;
    GameObject snowSled;
    GameObject vacuumCleaner;

    [SerializeField] float duration;
    [SerializeField] float speedBoost;

    float originalSpeed;
    Animator playerAnim;
    LayerMask originalLayer = 0;

    int randomIndex;
    MeshRenderer mesh;



    void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();
        randomIndex = Random.Range(0, 4);

    }

    void Update()
    {
        if(player == null)
        {
            player = GameObject.FindObjectOfType<PlayerController>();
        }
        if (speedBuffEffect1 == null || speedBuffEffect2 == null) //PLAYER PREFAB REMEMBER TO CHECK!
        {
            speedBuffEffect1 = FindObjectOfType<PlayerController>().buffEffects[2];
            speedBuffEffect2 = FindObjectOfType<PlayerController>().buffEffects[3];
        }
        if (flyCape == null)
        {
            flyCape = FindObjectOfType<PlayerController>().buffEffects[1];
        }
        if (snowSled == null)
        {
            snowSled = FindObjectOfType<PlayerController>().buffEffects[4];
        }

        if (vacuumCleaner == null)
        {
            vacuumCleaner = GameObject.FindObjectOfType<PlayerController>().buffEffects[0];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            if(randomIndex == 0)
            {
                SoundManager.instance.PlayAudioClip(Random.Range(2, 3));
                Animator playerAnim = other.GetComponent<Animator>();
                playerAnim.SetTrigger("buff");
                player.isSliding = false;
                player.canMove = true;
                player.isBuffHit = true;
                Instantiate(smokeEffect, transform.position, Quaternion.identity);
                mesh.enabled = false; //hide object
                StartCoroutine(ActivateMagnet());
            }
            else if(randomIndex == 1)
            {
                SoundManager.instance.PlayAudioClip(Random.Range(2, 3));
                mesh.enabled = false; //hide object
                player = other.GetComponent<PlayerController>();
                player.isFlying = true;
                player.isSliding = false;
                player.canMove = true;
                player.isBuffHit = true;
                playerAnim = other.gameObject.GetComponent<Animator>();
                playerAnim.SetTrigger("buff");
                Instantiate(smokeEffect, transform.position, Quaternion.identity);
                other.gameObject.layer = LayerMask.NameToLayer("Fly"); //change the layer to avoid object collisions
                StartCoroutine(ActivateFlyBuff());
            }
            else if (randomIndex == 2)
            {
                SoundManager.instance.PlayAudioClip(Random.Range(2, 3));
                mesh.enabled = false; //hide object
                player.isSliding = false;
                player.canMove = true;
                player.isBuffHit = true;
                player = other.gameObject.GetComponent<PlayerController>();
                Animator playerAnim = other.gameObject.GetComponent<Animator>();
                playerAnim.SetTrigger("buff");
                Instantiate(smokeEffect, transform.position, Quaternion.identity);
                originalSpeed = player.currentSpeed;
                player.buffed = true;
                StartCoroutine(ActivateSpeedBuff());
            }
            else if(randomIndex == 3)
            {
                SoundManager.instance.PlayAudioClip(Random.Range(2, 3));
                mesh.enabled = false; //hide object
                player.isSledding = true;
                player.isSliding = false;
                player.canMove = true;
                player.playerAnim.SetTrigger("buff");
                Instantiate(smokeEffect, transform.position, Quaternion.identity);
                originalSpeed = player.currentSpeed;
                player.buffed = true;
                StartCoroutine(ActivateSnowSledBuff());
            }
        }
    }

    IEnumerator ActivateSpeedBuff()
    {
        yield return new WaitForSeconds(0.5f);
        player.isBuffHit = false;
        speedBuffEffect1.SetActive(true);
        speedBuffEffect2.SetActive(true);
        speedBoost = originalSpeed * speedBoost;
        player.currentSpeed = speedBoost;
        CanvasManager.instance.ShowBuffSlider(duration);
        yield return new WaitForSeconds(duration);
        speedBuffEffect1.SetActive(false);
        speedBuffEffect2.SetActive(false);
        player.currentSpeed = originalSpeed;
        player.buffed = false;

    }

    IEnumerator ActivateFlyBuff()
    {
        yield return new WaitForSeconds(0.5f);
        player.isBuffHit = false;
        flyCape.SetActive(true);
        playerAnim.SetTrigger("flyStart");
        playerAnim.SetBool("flyMid", true);
        CanvasManager.instance.ShowBuffSlider(duration);
        yield return new WaitForSeconds(duration);
        playerAnim.SetBool("flyMid", false);
        yield return new WaitForSeconds(1f);
        flyCape.SetActive(false);
        playerAnim.gameObject.layer = originalLayer; //back to original layer
        player.isFlying = false;
        yield return new WaitForSeconds(0.3f);
        player.isBuffHit = false;

    }

    IEnumerator ActivateSnowSledBuff()
    {
        yield return new WaitForSeconds(0.5f);
        player.isBuffHit = false;
        snowSled.SetActive(true);
        player.playerAnim.SetTrigger("sledStart");
        player.playerAnim.SetBool("sledMid", true);
        player.currentSpeed *= speedBoost;
        CanvasManager.instance.ShowBuffSlider(duration);
        yield return new WaitForSeconds(duration);
        player.playerAnim.SetBool("sledMid", false);
        Instantiate(smokeEffect, transform.position, Quaternion.identity);
        snowSled.SetActive(false);
        yield return new WaitForSeconds(1f);
        player.currentSpeed = originalSpeed;
        player.isSledding = false;
        player.buffed = false;
    }

    private IEnumerator ActivateMagnet()
    {
        yield return new WaitForSeconds(0.3f);
        player.isBuffHit = false;
        vacuumCleaner.SetActive(true);
        CanvasManager.instance.ShowBuffSlider(duration);
        yield return new WaitForSeconds(duration);
        vacuumCleaner.SetActive(false);
        Destroy(gameObject);
    }
}
