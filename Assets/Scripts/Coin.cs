using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] int coinAmount = 1;
    [SerializeField] GameObject pickUpEffect;

    Animator animator;

    float randomSpeed;

    private void Start()
    {
        animator = GetComponent<Animator>();
        randomSpeed = Random.Range(0.5f, 1.2f);
        animator.speed = randomSpeed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player") || other.gameObject.CompareTag("Vacuum") || other.gameObject.CompareTag("Cape"))
        {
            CoinManager.instance.AddCoins(coinAmount);
            SoundManager.instance.PlayAudioClip(0);
            Instantiate(pickUpEffect, transform.position, Quaternion.identity);
            Destroy(this.gameObject);
        }
    }
}
