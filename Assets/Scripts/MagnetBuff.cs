using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetBuff : MonoBehaviour
{
    private float duration = 5f;
    [SerializeField] GameObject magnetEffect; //the effect on the player
    [SerializeField] GameObject buffPickUpEffect;
    [SerializeField] GameObject buffSmokeEffect;
    MeshRenderer mesh;
    Animator animator;
    Animator playerAnim;

    PlayerController player;


    void Start()
    {
        mesh = GetComponentInChildren<MeshRenderer>();

        animator = GetComponent<Animator>();
        float randomSpeed = Random.Range(0.3f, 1.5f);
        animator.speed = randomSpeed;
    }

    void Update()
    {
        
        if(magnetEffect == null || player == null)
        {
            player = FindObjectOfType<PlayerController>();
            magnetEffect = GameObject.FindObjectOfType<PlayerController>().buffEffects[0];
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            player.isBuffHit = true;
            player.isSliding = false;
            player.canMove = true;
            playerAnim = other.GetComponent<Animator>();
            SoundManager.instance.PlayAudioClip(Random.Range(2,3));
            playerAnim.SetTrigger("buff");
            Instantiate(buffSmokeEffect, transform.position, Quaternion.identity);
            mesh.enabled = false; //hide object
            StartCoroutine(ActivateMagnet());
        }
    }

    private IEnumerator ActivateMagnet()
    {
        yield return new WaitForSeconds(0.3f);
        magnetEffect.SetActive(true);
        player.isBuffHit = false;
        yield return new WaitForSeconds(0.3f);
        player.isBuffHit = false;
        CanvasManager.instance.ShowBuffSlider(duration);
        yield return new WaitForSeconds(duration);
        magnetEffect.SetActive(false);
        Destroy(gameObject);
    }
}
