﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using OsuParsers.Beatmaps.Objects;
using OsuHitObjects = OsuParsers.Beatmaps.Objects;

public class PlayField : MonoBehaviour
{
    public static PlayField Instance { get; set; }
    public static Stopwatch GameTimer { get; set; }
    public static int CurrentHitObject { get; private set; }
    public GameObject SongSelection;
    public GameObject Circle;
    public GameObject Slider;
    public GameObject HitObjects;
    private CanvasGroup canvasGroup;
    private Song song;

    private void Awake()
    {
        Instance = this;
        GameTimer = new Stopwatch();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        song = MusicPlayer.CurrentPlaying;
    }

    public IEnumerator SwitchToPlayField()
    {
        MusicPlayer.Stop();
        canvasGroup.alpha = 0;
        PlayerData.ActivePanel = gameObject;
        yield return StartCoroutine(SongSelection.FadeCanvasGroup(0.5f, 100, 1.0f, 0.0f));
        gameObject.SwitchPanel();
        yield return StartCoroutine(gameObject.FadeCanvasGroup(0.5f, 100, 0.0f, 1.0f));
        MusicPlayer.PlaySelected(song, 1.0f);
        MusicPlayer.Stop();
        yield return StartCoroutine(StartGame());
    }

    public IEnumerator SwitchToSongSelection()
    {
        canvasGroup.alpha = 0;
        yield return StartCoroutine(gameObject.FadeCanvasGroup(0.5f, 100, 1.0f, 0.0f));
        SongSelection.SwitchPanel();
        yield return StartCoroutine(SongSelection.FadeCanvasGroup(0.5f, 100, 0.0f, 1.0f));
    }

    private IEnumerator StartGame()
    {
        CurrentHitObject = 0;
        foreach (HitObject hitObject in song.HitObjects)
        {
            if (hitObject is HitCircle)
            {
                GameObject circle = Instantiate(Circle);
                circle.transform.SetParent(HitObjects.transform);
            }
            else if (hitObject is OsuHitObjects.Slider)
            {
                GameObject slider = Instantiate(Slider);
                slider.transform.SetParent(HitObjects.transform);
            }
            CurrentHitObject++;
        }
        MusicPlayer.Play();
        GameTimer.Start();
        while (GameTimer.ElapsedMilliseconds <= song.GeneralSection.Length)
        {
            yield return null;
        }
        StopGame();
    }
    
    public void StopGame()
    {
        StopCoroutine(nameof(StartGame));
        StopCoroutine(nameof(SwitchToPlayField));
        foreach (Transform t in HitObjects.transform)
        {
            Destroy(t.gameObject);
        }
        GameTimer.Reset();
        StartCoroutine(SwitchToSongSelection());
        UnityEngine.Debug.Log("Game stopped");
    }
}
