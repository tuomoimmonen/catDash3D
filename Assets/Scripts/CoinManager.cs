using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CoinManager : MonoBehaviour
{
    public static CoinManager instance;
    public int coins;

    [SerializeField] TMP_Text coinText;

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        coins = PlayerPrefs.GetInt("coins", 0);
    }
    void Start()
    {
        if(SceneManager.GetActiveScene().buildIndex == 0) { return; }

        coinText.text = coins.ToString();
        if(coinText == null)
        {
            Canvas canvas = GameObject.FindAnyObjectByType<Canvas>();
            coinText = canvas.GetComponentInChildren<TMP_Text>();
        }
    }

    void Update()
    {
        
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        PlayerPrefs.SetInt("coins", coins);
        coinText.text = PlayerPrefs.GetInt("coins", 0).ToString();
    }

    //carefull with onenable and disable, playerprefs not resetting
    /*
    private void OnEnable()
    {
        PlayerPrefs.SetInt("coins", coins);
    }

    private void OnDisable()
    {
        PlayerPrefs.SetInt("coins", coins);
    }
    */
}
