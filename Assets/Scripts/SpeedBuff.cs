using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedBuff : MonoBehaviour
{
    [SerializeField] GameObject smokeEffect;
    [SerializeField] GameObject speedBuffEffect1; //this is the boots on the player
    [SerializeField] GameObject speedBuffEffect2; //second boots
    [SerializeField] float speedBoost = 2f;
    [SerializeField] float buffDuration = 5;
    PlayerController player;
    float originalSpeed; //players original speed

    Animator animator;
    MeshRenderer mesh;

    float originalMaxSpeed;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
        animator = GetComponent<Animator>();
        float randomSpeed = Random.Range(0.3f, 1.5f); //speed for animator spin
        animator.speed = randomSpeed;
    }

    private void Update()
    {
        if(speedBuffEffect1 == null || speedBuffEffect2 == null) //PLAYER PREFAB REMEMBER TO CHECK!
        {
            speedBuffEffect1 = FindObjectOfType<PlayerController>().buffEffects[2];
            speedBuffEffect2 = FindObjectOfType<PlayerController>().buffEffects[3];
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SoundManager.instance.PlayAudioClip(Random.Range(2, 3));
            player = other.gameObject.GetComponent<PlayerController>();
            player.isBuffHit = true;
            player.isSliding = false;
            player.canMove = true;
            mesh.enabled = false; //hide object
            Animator playerAnim = other.gameObject.GetComponent<Animator>();
            playerAnim.SetTrigger("buff");
            Instantiate(smokeEffect, transform.position, Quaternion.identity);
            originalSpeed = player.currentSpeed;
            originalMaxSpeed = player.maxSpeed;
            player.buffed = true;
            StartCoroutine(ActivateSpeedBuff());
        }
    }

    IEnumerator ActivateSpeedBuff()
    {
        yield return new WaitForSeconds(0.5f);
        player.isBuffHit = false;
        player.maxSpeed *= speedBoost;
        CanvasManager.instance.ShowBuffSlider(buffDuration);
        speedBuffEffect1.SetActive(true);
        speedBuffEffect2.SetActive(true);
        speedBoost = originalSpeed * speedBoost;
        player.currentSpeed = speedBoost;
        yield return new WaitForSeconds(buffDuration);
        speedBuffEffect1.SetActive(false);
        speedBuffEffect2.SetActive(false);
        player.currentSpeed = originalSpeed;
        player.maxSpeed = originalMaxSpeed;
        player.buffed = false;

    }

}
