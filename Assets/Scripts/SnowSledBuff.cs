using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnowSledBuff : MonoBehaviour
{
    [SerializeField] GameObject smokeEffect;
    [SerializeField] GameObject snowSledOnPlayer;
    [SerializeField] float speedBoost = 1.4f;
    private float originalSpeed;
    [SerializeField] float buffDuration = 5f;

    PlayerController player;
    MeshRenderer mesh;
    void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    void Update()
    {
        if(player == null || snowSledOnPlayer == null)
        {
            player = FindObjectOfType<PlayerController>();
            snowSledOnPlayer = FindObjectOfType<PlayerController>().buffEffects[4];
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            SoundManager.instance.PlayAudioClip(Random.Range(2, 3));
            player.canMove = true;
            player.isBuffHit = true;
            player.isSliding = false;
            mesh.enabled = false; //hide object
            player.isSledding = true;
            player.playerAnim.SetTrigger("buff");
            Instantiate(smokeEffect, transform.position, Quaternion.identity);
            originalSpeed = player.currentSpeed;
            player.buffed = true;
            StartCoroutine(ActivateSnowSledBuff());
        }
    }

    IEnumerator ActivateSnowSledBuff()
    {
        yield return new WaitForSeconds(0.5f);
        player.isBuffHit = false;
        CanvasManager.instance.ShowBuffSlider(buffDuration);
        snowSledOnPlayer.SetActive(true);
        player.playerAnim.SetTrigger("sledStart");
        player.playerAnim.SetBool("sledMid", true);
        player.currentSpeed *= speedBoost;
        yield return new WaitForSeconds(buffDuration);
        player.playerAnim.SetBool("sledMid", false);
        Instantiate(smokeEffect,transform.position, Quaternion.identity);
        snowSledOnPlayer.SetActive(false);
        yield return new WaitForSeconds(1f);
        player.currentSpeed = originalSpeed;
        player.isSledding = false;
        player.buffed = false;
    }
}
