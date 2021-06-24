using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AT { GameOver, Jump, Floor, Coin, Click, Purchase }

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance = null;
    private void Awake()
    {
        if (instance == null) instance = this;
    }
    #endregion

    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip coin;
    [SerializeField] private AudioClip gameOver;
    [SerializeField] private AudioClip jump;
    [SerializeField] private AudioClip isFloor;
    [SerializeField] private AudioClip click;
    [SerializeField] private AudioClip purchase;


    public void PlaySound(AT audioType)
    {
        if (PlayerPrefs.GetInt("sound") != 0) return;

        AudioClip clip = null;
        switch (audioType)
        {
            case AT.GameOver: clip = gameOver; break;
            case AT.Jump: clip = jump; break;
            case AT.Floor: clip = isFloor; break;
            case AT.Coin: clip = coin; break;
            case AT.Click: clip = click; break;
            case AT.Purchase: clip = purchase; break;
        }

        if (clip != null)
            source.PlayOneShot(clip);
    }

    public void Vibrate()
    {
        if (PlayerPrefs.GetInt("vibrate") == 0)
            Handheld.Vibrate();
    }
}
