using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CharacterChanger : MonoBehaviour
{
    [SerializeField] GameObject[] skins;
    [SerializeField] GameObject[] femaleHats;
    [SerializeField] GameObject[] maleHats;
    public Character[] characters;
    public int selectedCharacter;
    public int selectedFemaleHat;
    public int selectedMaleHat;
    public int selectedIndex;

    [SerializeField] TMP_Text[] costTexts;
    [SerializeField] TMP_Text coinText;

    [SerializeField] Button buyButton;

    private void Awake()
    {
        UpdateUI();
    }

    public void UpdateUI()
    {
        selectedCharacter = PlayerPrefs.GetInt("SelectedCharacter", 0);
        selectedFemaleHat = PlayerPrefs.GetInt("SelectedFemaleHat", 0);
        selectedMaleHat = PlayerPrefs.GetInt("SelectedMaleHat", 0);
 
        foreach (GameObject skins in skins)
        {
            skins.SetActive(false);
        }
        skins[selectedCharacter].SetActive(true);

        foreach (GameObject hats in femaleHats)
        {
            hats.SetActive(false);
        }
        femaleHats[selectedFemaleHat].SetActive(true);

        foreach (GameObject hats in maleHats) { hats.SetActive(false); }
        maleHats[selectedMaleHat].SetActive(true);
    }

    private void Start()
    {
        UpdateHats();
    }

    private void Update()
    {
        coinText.text = CoinManager.instance.coins.ToString();
    }

    private void UpdateHats()
    {
        for (int i = 0; i < characters.Length; i++)
        {
            if (PlayerPrefs.GetInt("unlockedHat" + i) == 1)
            {
                characters[i].isUnlocked = true;
            }
            else
            {
                PlayerPrefs.SetInt("unlockedHat" + i, 0);
            }
        }

        for (int i = 0; i < costTexts.Length; i++)
        {
            
            costTexts[i].text = characters[i].cost.ToString();
            if (PlayerPrefs.GetInt("unlockedHat" + i) == 1)
            {
                costTexts[i].text = "PURCHASED";
            }
        }
    }

    public void NextCharacter()
    {
        SoundManager.instance.PlayAudioClip(8);
        skins[selectedCharacter].SetActive(false);
        selectedCharacter++;
        if (selectedCharacter == skins.Length)
        {
            selectedCharacter = 0;
        }
        skins[selectedCharacter].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
    }

    public void PreviousCharacter()
    {
        SoundManager.instance.PlayAudioClip(8);
        skins[selectedCharacter].SetActive(false);
        selectedCharacter--;
        if (selectedCharacter == -1)
        {
            selectedCharacter = skins.Length - 1;
        }
        skins[selectedCharacter].SetActive(true);
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
    }

    public void NextHat()
    {
        if(selectedCharacter == 1)
        {
            femaleHats[selectedFemaleHat].SetActive(false);
            selectedFemaleHat++;
            if (selectedFemaleHat == femaleHats.Length) { selectedFemaleHat = 0; }
            femaleHats[selectedFemaleHat].SetActive(true);
            PlayerPrefs.SetInt("SelectedFemaleHat", selectedFemaleHat);
        }
        else if(selectedCharacter == 0)
        {
            maleHats[selectedMaleHat].SetActive(false);
            selectedMaleHat++;
            if (selectedMaleHat == maleHats.Length) {selectedMaleHat = 0; }
            maleHats[selectedMaleHat].SetActive(true);
            PlayerPrefs.SetInt("SelectedMaleHat", selectedMaleHat);
        }

    }

    public void PreviousHat()
    {
        if(selectedCharacter == 1)
        {
            femaleHats[selectedFemaleHat].SetActive(false);
            selectedFemaleHat--;
            if (selectedFemaleHat == -1) { selectedFemaleHat = femaleHats.Length - 1; }
            femaleHats[selectedFemaleHat].SetActive(true);
            PlayerPrefs.SetInt("SelectedFemaleHat", selectedFemaleHat);
        }
        else if (selectedCharacter == 0)
        {
            maleHats[selectedMaleHat].SetActive(false);
            selectedMaleHat--;
            if (selectedMaleHat == -1) { selectedMaleHat = maleHats.Length - 1; }
            maleHats[selectedMaleHat].SetActive(true);
            PlayerPrefs.SetInt("SelectedMaleHat", selectedMaleHat);
        }
    }

    public void SelectHat(int index)
    {
        if (CoinManager.instance.coins >= characters[index].cost || characters[index].isUnlocked)
        {
            buyButton.interactable = true;
        }
        else
        {
            buyButton.interactable = false;
        }

        if (selectedCharacter == 1)
        {
            femaleHats[PlayerPrefs.GetInt("SelectedFemaleHat",0)].SetActive(false);
            foreach (GameObject hats in femaleHats) { hats.SetActive(false); }
            femaleHats[index].SetActive(true);
            selectedIndex = index;
            SoundManager.instance.PlayAudioClip(8);
        }
        else if (selectedCharacter == 0)
        {
            maleHats[PlayerPrefs.GetInt("SelectedMaleHat", 0)].SetActive(false);
            foreach (GameObject hats in maleHats) { hats.SetActive(false); }
            maleHats[index].SetActive(true);
            selectedIndex = index;
            SoundManager.instance.PlayAudioClip(8);

        }
    }

    public void SetHat()
    {
        if (selectedCharacter == 1)
        {
            SoundManager.instance.PlayAudioClip(8);
            PlayerPrefs.SetInt("SelectedFemaleHat", selectedIndex);
            CoinManager.instance.coins -= characters[selectedIndex].cost;
            PlayerPrefs.SetInt("coins", CoinManager.instance.coins);
            characters[selectedIndex].cost = 0;
            PlayerPrefs.SetInt("unlockedHat" + selectedIndex, 1);
            coinText.text = CoinManager.instance.coins.ToString();

            UpdateHats();
        }
        else if(selectedCharacter == 0)
        {
            SoundManager.instance.PlayAudioClip(8);
            PlayerPrefs.SetInt("SelectedMaleHat", selectedIndex);
            CoinManager.instance.coins -= characters[selectedIndex].cost;
            PlayerPrefs.SetInt("coins", CoinManager.instance.coins);
            characters[selectedIndex].cost = 0;
            PlayerPrefs.SetInt("unlockedHat" + selectedIndex, 1);
            coinText.text = CoinManager.instance.coins.ToString();


            UpdateHats();
        }
    }
}
