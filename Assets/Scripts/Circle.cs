using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OsuParsers.Beatmaps.Objects;

public class Circle : MonoBehaviour
{
    public GameObject HitCircleTexture;
    public GameObject HitCircleOverlay;
    public GameObject Number;
    public GameObject ApproachCircle;
    public HitCircle HitCircle { get; set; }
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
        HitCircle = (HitCircle)MusicPlayer.CurrentPlaying.HitObjects[PlayField.CurrentHitObject];
        ar = MusicPlayer.CurrentPlaying.DifficultySection.ApproachRate;
        cs = MusicPlayer.CurrentPlaying.DifficultySection.CircleSize;
        hp = MusicPlayer.CurrentPlaying.DifficultySection.HPDrainRate;
        od = MusicPlayer.CurrentPlaying.DifficultySection.OverallDifficulty;
        spawnDelay = (int)PlayField.GameTimer.ElapsedMilliseconds - HitCircle.StartTime;
        transform.position = Camera.main.ScreenToWorldPoint
        (
            new Vector3
            (
                x: HitCircle.Position.X * Screen.width / 512,
                y: HitCircle.Position.Y * Screen.height / 384
            )
        );
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        if (ar < 5)
        {
            fadeIn = (int)(800 + 400 * (5 - ar) / 5);
            preempt = (int)(1200 + 600 * (5 - ar) / 5);
        }
        else if (ar == 5)
        {
            fadeIn = 800;
            preempt = 1200;
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
        SetCircleSize(cs);
    }

    private void Update()
    {
        elapsedTimeSinceSpawn += (int)(Time.deltaTime * 1000);
        canvasGroup.alpha = Mathf.Clamp((float)(PlayField.GameTimer.ElapsedMilliseconds - (HitCircle.StartTime - preempt)) / fadeIn, 0, 1);
        float approachCircleScaleMultiplier = 1 - Mathf.Clamp((float)(PlayField.GameTimer.ElapsedMilliseconds - (HitCircle.StartTime - preempt)) / preempt, 0, 1);
        if (!(approachCircleScaleMultiplier == 0 && PlayField.GameTimer.ElapsedMilliseconds < HitCircle.StartTime - preempt))
        {
            ApproachCircle.transform.localScale = originalApproachCircleScale * approachCircleScaleMultiplier + HitCircleTexture.transform.localScale;
        }
        if (PlayField.GameTimer.ElapsedMilliseconds > HitCircle.StartTime)
        {
            Destroy(gameObject);
        }
    }

    private void SetCircleSize(float cs)
    {
        float targetSize = (float)((Screen.width / 16) * (1 - (0.7 * (cs - 5) / 5))); // target circle size in pixels
        float currentSize = rectTransform.rect.width * transform.localScale.x;
        float targetScale = targetSize / currentSize;
        transform.localScale *= targetScale;
    }
}