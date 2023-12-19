using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuController : MonoBehaviour
{
    public string gameScene;
    public GameObject startPanel;
    public GameObject instructionsPanel;
    public GameObject creditsPanel;
    public TextMeshProUGUI numberOfPlayersText;
    public Image checkMarkTwinMode; 
        
    [Header("Configuration, UI starts with that")]
    public int maxNumberOfPlayers = 6;
    public int minNumberOfPlayers = 2;
    [Header("User Settable")]
    public int numberOfPlayers = 3;
    public bool isTwinMode = false;
    // Start is called before the first frame update
    void Start()
    {
        startPanel.SetActive(false);
        instructionsPanel.SetActive(false);
        creditsPanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        checkMarkTwinMode.gameObject.SetActive(isTwinMode);
    }
    
    public void ShowStartPanel()
    {
        startPanel.SetActive(true);
    }
    
    public void HideStartPanel()
    {
        startPanel.SetActive(false);
    }
    
    public void ShowInstructionsPanle()
    {
        instructionsPanel.SetActive(true);
    }
    
    public void HideInstructionsPanle()
    {
        instructionsPanel.SetActive(false);
    }
    
    public void ShowCreditsPanel()
    {
        creditsPanel.SetActive(true);
    }
    
    public void HideICreditPanel()
    {
        creditsPanel.SetActive(false);
    }

    public void IncreaseNumberOfPlayers()
    {
        numberOfPlayers = Math.Min(maxNumberOfPlayers, numberOfPlayers + 1);
        numberOfPlayersText.text = "" + numberOfPlayers;
    }
    
    public void DecreaseNumberOfPlayers()
    {
        numberOfPlayers = Math.Max(minNumberOfPlayers, numberOfPlayers - 1);
        numberOfPlayersText.text = "" + numberOfPlayers;
    }

    public void ToggleTwinMode()
    {
        isTwinMode = !isTwinMode;
    }

    public void StartGame()
    {
        PlayerPrefs.SetInt("numberOfPlayers",numberOfPlayers);
        SceneManager.LoadScene(gameScene);
    }
}
