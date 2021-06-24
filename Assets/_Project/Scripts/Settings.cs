using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [Header("- SETTINGS -")]
    [SerializeField] private Image soundImage;
    [SerializeField] private Image vibrateImage;
    [SerializeField] private Sprite[] soundSprites;
    [SerializeField] private Sprite[] vibrateSprites;
    [SerializeField] private Slider sensitivitySlider;
    [SerializeField] private Text sensitivityText;

    private void Start()
    {
        GetData();
    }
    private void GetData()
    {
        int i = PlayerPrefs.GetInt("sound");
        soundImage.sprite = soundSprites[i];
        int a = PlayerPrefs.GetInt("vibrate");
        vibrateImage.sprite = vibrateSprites[a];
        if (PlayerPrefs.HasKey("sensitivity"))
        {
            float f = PlayerPrefs.GetFloat("sensitivity");
            sensitivitySlider.value = f;
            sensitivityText.text = f.ToString();
        }
        else { 
            PlayerPrefs.SetFloat("sensitivity", 3);
            sensitivityText.text = PlayerPrefs.GetFloat("sensitivity").ToString(); 
        }
       
    }

    public void SoundOnOff()
    {
        int i = PlayerPrefs.GetInt("sound");
        i = i == 0 ? 1 : 0;
        PlayerPrefs.SetInt("sound", i);
        AudioManager.instance.PlaySound(AT.Click);
        GetData();
    }
    public void VibrateOnOff()
    {
        int i = PlayerPrefs.GetInt("vibrate");
        i = i == 0 ? 1 : 0;
        PlayerPrefs.SetInt("vibrate", i);
        AudioManager.instance.PlaySound(AT.Click);
        GetData();
    }

    public void SetSensitivity()
    {
        float f = sensitivitySlider.value;
        PlayerPrefs.SetFloat("sensitivity", f);
        sensitivityText.text = f.ToString();
        AudioManager.instance.PlaySound(AT.Click);
        GameManager.instance.SetSensitivity();
    }

}
