using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OsuHitObjects = OsuParsers.Beatmaps.Objects;
using OsuParsers.Enums.Beatmaps;

public class Slider : MonoBehaviour
{
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
    private OsuHitObjects.Slider slider;
    private List<BezierPath> curves;
    private CirclePath circlePath;
    private LinearPath linearPath;
    private LineRenderer lineRenderer;
    private List<Vector3> points = new List<Vector3>();
    private float sliderSnakingInterval;
    private int originalPositionCount;
    private RectTransform rectTransform;
    private const int ACCURACY = 50;

    private void Awake()
    {
        ar = MusicPlayer.CurrentPlaying.DifficultySection.ApproachRate;
        cs = MusicPlayer.CurrentPlaying.DifficultySection.CircleSize;
        hp = MusicPlayer.CurrentPlaying.DifficultySection.HPDrainRate;
        od = MusicPlayer.CurrentPlaying.DifficultySection.OverallDifficulty;
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
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.material.SetColor
        (
            "_Color",
            new Color
            (
                lineRenderer.material.color.r,
                lineRenderer.material.color.g,
                lineRenderer.material.color.b,
                0
            )
        );
        slider = (OsuHitObjects.Slider)MusicPlayer.CurrentPlaying.HitObjects[PlayField.CurrentHitObject];
        slider.SliderPoints.Insert(0, slider.Position);
        rectTransform = GetComponent<RectTransform>();
        curves = new List<BezierPath>();
        lineRenderer.widthCurve = AnimationCurve.Linear(0, 0.2f, 1, 0.2f);
        lineRenderer.numCornerVertices = 32;
        lineRenderer.numCapVertices = 32;
        switch (slider.CurveType)
        {
            case CurveType.Bezier:
            {
                name = "Bezier Slider(Clone)";
                int start = 0;
                bool isMultipleCurves = false;
                for (int i = 1; i < slider.SliderPoints.Count; i++)
                {
                    if (slider.SliderPoints[i] == slider.SliderPoints[i - 1])
                    {
                        isMultipleCurves = true;
                        List<System.Numerics.Vector2> controlPoints = new List<System.Numerics.Vector2>();
                        for (int j = start; j < i; j++) // split the SliderPoint list into separate curves(a bezier can be composed of multiple curves)
                        {
                            controlPoints.Add(Utilities.OsuPixelToScreenPoint(slider.SliderPoints[j]));
                        }
                        curves.Add(new BezierPath(controlPoints, ACCURACY, ACCURACY));
                        start = i;
                    }
                }
                if (!isMultipleCurves)
                {
                    curves.Add(new BezierPath(Utilities.OsuPixelsToScreenPoints(slider.SliderPoints), ACCURACY, ACCURACY));
                }
                lineRenderer.positionCount = ACCURACY * curves.Count;
                for (int i = 0; i < curves.Count; i++) // unpack all calculated points into the line renderer
                {
                    for (int j = 0; j < curves[i].Points.Count; j++)
                    {
                        points.Add(Utilities.ScreenToWorldPoint2D(curves[i].Points[j].X, curves[i].Points[j].Y));
                    }
                }
                lineRenderer.SetPositions(points.ToArray());
                break;
            }
            case CurveType.Catmull:
            {
                name = "Catmull Slider(Clone)";
                Debug.LogWarning("Catmull sliders are not supproted.");
                Destroy(gameObject);
                break;
            }
            case CurveType.Linear:
            {
                name = "Linear Slider(Clone)";
                linearPath = new LinearPath(Utilities.OsuPixelToScreenPoint(slider.SliderPoints[0]), Utilities.OsuPixelToScreenPoint(slider.SliderPoints[slider.SliderPoints.Count - 1]), ACCURACY, ACCURACY);
                lineRenderer.positionCount = ACCURACY;
                foreach (System.Numerics.Vector2 v in linearPath.Points)
                {
                    points.Add(Utilities.ScreenToWorldPoint2D(v.X, v.Y));
                }
                lineRenderer.SetPositions(points.ToArray());
                break;
            }
            case CurveType.PerfectCurve:
            {
                name = "Circle Slider(Clone)";
                circlePath = new CirclePath(Utilities.OsuPixelToScreenPoint(slider.SliderPoints[0]), Utilities.OsuPixelToScreenPoint(slider.SliderPoints[1]), Utilities.OsuPixelToScreenPoint(slider.SliderPoints[2]), ACCURACY, ACCURACY);
                foreach (System.Numerics.Vector2 v in circlePath.Points)
                {
                    points.Add(Utilities.ScreenToWorldPoint2D(v.X, v.Y));
                }
                lineRenderer.positionCount = ACCURACY;
                lineRenderer.SetPositions(points.ToArray());
                break;
            }
        }
        sliderSnakingInterval = (float)fadeIn / lineRenderer.positionCount;
        originalPositionCount = lineRenderer.positionCount;
    }

    private void Update()
    {
        lineRenderer.material.SetColor
        (
            "_Color",
            new Color
            (
                lineRenderer.material.color.r,
                lineRenderer.material.color.g,
                lineRenderer.material.color.b,
                Mathf.Clamp((float)(PlayField.GameTimer.ElapsedMilliseconds - (slider.StartTime - preempt)) / fadeIn, 0, 1)
            )
        );
        int positionCount = (int)Math.Max(0, (PlayField.GameTimer.ElapsedMilliseconds - (slider.StartTime - preempt)) / sliderSnakingInterval);
        positionCount = Mathf.Clamp(positionCount, 0, originalPositionCount);
        lineRenderer.positionCount = positionCount;
        lineRenderer.SetPositions(points.ToArray());
        if (PlayField.GameTimer.ElapsedMilliseconds > slider.EndTime)
        {
            Destroy(gameObject);
        }
    }
}
