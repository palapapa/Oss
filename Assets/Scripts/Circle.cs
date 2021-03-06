﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using OsuParsers.Beatmaps.Objects;

public class Circle : MonoBehaviour
{
    public GameObject HitCircleTexture;
    public GameObject HitCircleOverlay;
    public GameObject Number;
    public GameObject ApproachCircle;
    private HitCircle hitCircle;
    private float ar;
    private float cs;
    private float hp;
    private float od;
    /*
                                             HitCircle.StartTime = hit/collect
                    p r e e m p t            ↓
      ├───────────────────────┬──────────────┤
      0%        fadeIn          100% opacity
    */
    /// <summary>
    /// The amount of time it takes for the hit object to completely fade in in milliseconds
    /// </summary>
    private int fadeIn;
    /// <summary>
    /// The amount of time before the circle's start time that the circle starts to fade in in milliseconds
    /// </summary>
    private int preempt;
    /// <summary>
    /// The delay between the ideal spawn time of the circle and the actual spawn time
    /// </summary>
    private int spawnDelay;
    private int elapsedTimeSinceSpawn;
    private Vector3 originalApproachCircleScale;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0;
        hitCircle = (HitCircle)MusicPlayer.CurrentPlaying.HitObjects[PlayField.CurrentHitObject];
        ar = MusicPlayer.CurrentPlaying.DifficultySection.ApproachRate;
        cs = MusicPlayer.CurrentPlaying.DifficultySection.CircleSize;
        hp = MusicPlayer.CurrentPlaying.DifficultySection.HPDrainRate;
        od = MusicPlayer.CurrentPlaying.DifficultySection.OverallDifficulty;
        spawnDelay = (int)PlayField.GameTimer.ElapsedMilliseconds - hitCircle.StartTime;
        Vector2 screenPoint = Utilities.OsuPixelToScreenPointUnity(hitCircle.Position);
        transform.position = Utilities.ScreenToWorldPoint2D(screenPoint.x, screenPoint.y);
        if (ar <= 5)
        {
            fadeIn = (int)(800 + 400 * (5 - ar) / 5);
            preempt = (int)(1200 + 600 * (5 - ar) / 5);
        }
        else
        {
            fadeIn = (int)(800 - 500 * (ar - 5) / 5);
            preempt = (int)(1200 - 750 * (ar - 5) / 5);
        }
        elapsedTimeSinceSpawn = spawnDelay;
        originalApproachCircleScale = ApproachCircle.transform.localScale;
        // SetCircleSize(cs); // somehow if this line is in Awake the size of the circle will be the square of the intended size and I have no fucking idea why that happens
        // ApproachCircle.transform.localScale *= 2;
    }

    private void Start()
    {
        float targetSize = (float)(150 * (1 - 0.7 * (cs - 5) / 5)); // target circle size in pixels
        float currentSize = rectTransform.rect.width * transform.localScale.x;
        float targetScale = targetSize / currentSize;
        transform.localScale *= targetScale;
    }

    private void Update()
    {
        elapsedTimeSinceSpawn += (int)(Time.deltaTime * 1000);
        canvasGroup.alpha = Mathf.Clamp01((float)(PlayField.GameTimer.ElapsedMilliseconds - (hitCircle.StartTime - preempt)) / fadeIn);
        float approachCircleScaleMultiplier = 1 - Mathf.Clamp01((float)(PlayField.GameTimer.ElapsedMilliseconds - (hitCircle.StartTime - preempt)) / preempt);
        if (!(approachCircleScaleMultiplier == 0 && PlayField.GameTimer.ElapsedMilliseconds < hitCircle.StartTime - preempt))
        {
            ApproachCircle.transform.localScale = originalApproachCircleScale * approachCircleScaleMultiplier + HitCircleTexture.transform.localScale;
        }
        if
        (
            (
                Input.GetKeyDown(KeyCode.Mouse0) ||
                Input.GetKeyDown(KeyCode.Mouse1) ||
                Input.GetKeyDown(KeyCode.Z) ||
                Input.GetKeyDown(KeyCode.X)
            ) &&
            IsActive()
        )
        {
            Vector2 click = new Vector2(Input.mousePosition.x, Screen.height - Input.mousePosition.y);
            click = Utilities.ScreenTo2DWorldPoint(click.x, click.y);
            Vector2 circle = new Vector2(transform.position.x, transform.position.y);
            Vector3[] worldCorners = new Vector3[4];
            rectTransform.GetWorldCorners(worldCorners);
            Debug.Log($"click={click}, circle={circle}, radius={Math.Abs((worldCorners[0].x - worldCorners[3].x) / 2)}, distance={Vector2.Distance(click, circle)}");
            if (Vector2.Distance(click, circle) <= Math.Abs((worldCorners[0].x - worldCorners[3].x) / 2))
            {
                Destroy(gameObject);
            }
        }
        if (PlayField.GameTimer.ElapsedMilliseconds > hitCircle.StartTime)
        {
            Destroy(gameObject);
        }
    }

    private bool IsActive()
    {
        return canvasGroup.alpha != 0;
    }
}