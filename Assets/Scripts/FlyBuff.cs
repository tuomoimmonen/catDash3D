using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyBuff : MonoBehaviour
{
    [SerializeField] GameObject flyCapeObject;
    [SerializeField] GameObject buffSmokeEffect;
    public Animator playerAnim;
    LayerMask originalLayer = 0;
    float duration = 5f;

    PlayerController player;
    MeshRenderer mesh;

    private void Start()
    {
        mesh = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if(flyCapeObject == null)
        {
            flyCapeObject = FindObjectOfType<PlayerController>().buffEffects[1];
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == "Player")
        {
            SoundManager.instance.PlayAudioClip(Random.Range(2, 3));
            mesh.enabled = false; //hide object
            player = other.GetComponent<PlayerController>();
            player.isBuffHit = true;
            player.isFlying = true;
            player.isSliding = false;
            player.canMove = true;
            playerAnim = other.gameObject.GetComponent<Animator>();
            playerAnim.SetTrigger("buff");
            Instantiate(buffSmokeEffect, transform.position, Quaternion.identity);
            other.gameObject.layer = LayerMask.NameToLayer("Fly"); //change the layer to avoid object collisions
            StartCoroutine(ActivateBuff());
        }
    }

    IEnumerator ActivateBuff()
    {
        yield return new WaitForSeconds(0.5f);
        player.isBuffHit = false;
        CanvasManager.instance.ShowBuffSlider(duration);
        flyCapeObject.SetActive(true);
        playerAnim.SetTrigger("flyStart");
        playerAnim.SetBool("flyMid", true);
        yield return new WaitForSeconds(duration);
        playerAnim.SetBool("flyMid", false);
        yield return new WaitForSeconds(1f);
        flyCapeObject.SetActive(false);
        player.gameObject.layer = originalLayer; //back to original layer
        player.isFlying = false;
        playerAnim.SetBool("flyMid", false);
        yield return new WaitForSeconds(0.3f);
        player.isBuffHit = false;

    }
}
