using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class MusicManager : MonoBehaviour
{
    private AudioSource musicSource;
    [SerializeField] private AudioClip menuMusic;
    [SerializeField] private AudioClip gameMusic;

    private void OnEnable()
    {
        GameManager.OnStateChanged += OnGameStateChanged;
        musicSource = GetComponent<AudioSource>();
    }

    private void OnDestroy()
    {
        GameManager.OnStateChanged -= OnGameStateChanged;
    }


    public void OnGameStateChanged(GameState state)
    {
        if (state == GameState.StartGame)
        {
            musicSource.clip = gameMusic;
            musicSource.Play();
        }

        if (state == GameState.Menu) 
        {
            musicSource.clip = menuMusic;
            musicSource.Play();
        }
    }
}
