using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    #region Singleton
    public static UI_Manager instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion

    [Header("-CAMERAS-")]
    [SerializeField] private Camera mainCamera;
    [SerializeField] private Camera shopCamera;
    private bool rotateCamera;

    [Header("-PANELS-")]
    [SerializeField] private GameObject gamePanel;
    [SerializeField] private GameObject menuPanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject shopPanel;
    [SerializeField] private GameObject[] shopTabs;

    [Header("-TEXTS-")]
    [SerializeField] private Text scoreText;
    [SerializeField] private Text gameOverScoreText;
    [SerializeField] private Text gameOverBestScoreText;
    [SerializeField] private Text coinText;

    private void Update()
    {
        if (rotateCamera) shopCamera.transform.parent.Rotate(0,Time.deltaTime * 45,0);
    }

    public void Menu(bool activate)
    {
        menuPanel.SetActive(activate);
        GameOver(false);       
    }
    public void GamePanel(bool activate)
    {
        gamePanel.SetActive(activate);      
    }
    public void GameOver(bool activate)
    {
        gameOverPanel.SetActive(activate);
        if (activate)
        {
            gameOverBestScoreText.text = "Best: " + ((int)PlayerPrefs.GetFloat("best")).ToString();
            gameOverScoreText.text = "Score: " + scoreText.text;
        }
            GamePanel(!activate);        
    }
    public void SettingPanel(bool activate)
    {
        settingPanel.SetActive(activate);
    }
    public void ShopPanel(bool activate)
    {
        rotateCamera = activate;
        shopPanel.SetActive(activate);
        Menu(false);
        GamePanel(false);
        mainCamera.enabled = !activate;
        shopCamera.enabled = activate;
        ChangeShopTab(0);
    }
    public void ChangeShopTab(int id)
    {
        for (int i = 0; i < shopTabs.Length; i++) shopTabs[i].SetActive(false);
        shopTabs[id].SetActive(true);
    }

    public void UpdateScore(float score)
    {
        scoreText.text = ((int)score).ToString() + " m";
    }
    public void UpdateCoin(int coin)
    {
        coinText.text = coin.ToString();
    }
}
