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
    private List<Bezier> curves;
    private LineRenderer lineRenderer;
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
        curves = new List<Bezier>();
        List<Vector3> points = new List<Vector3>();
        lineRenderer.widthCurve = AnimationCurve.Linear(0, 0.1f, 1, 0.1f);
        lineRenderer.numCornerVertices = 32;
        lineRenderer.numCapVertices = 32;
        switch (slider.CurveType)
        {
            case CurveType.Bezier:
            {
                name = "Bezier Slider(Clone)";
                int start = 0;
                bool isMultipleCurves = false;
                for (int i = 0; i < slider.SliderPoints.Count; i++)
                {
                    if (i != 0)
                    {
                        if (slider.SliderPoints[i] == slider.SliderPoints[i - 1])
                        {
                            isMultipleCurves = true;
                            List<System.Numerics.Vector2> controlPoints = new List<System.Numerics.Vector2>();
                            for (int j = start; j < i; j++) // split the SliderPoint list into separate curves(a bezier can be composed of multiple curves)
                            {
                                controlPoints.Add(Utilities.OsuPixelToScreenPoint(slider.SliderPoints[j]));
                            }
                            curves.Add(new Bezier(controlPoints, ACCURACY));
                            start = i;
                        }
                    }
                }
                if (!isMultipleCurves)
                {
                    curves.Add(new Bezier(Utilities.OsuPixelsToScreenPoints(slider.SliderPoints), ACCURACY));
                }
                lineRenderer.positionCount = ACCURACY * curves.Count;
                for (int i = 0; i < curves.Count; i++) // unpack all calculated points into the line renderer
                {
                    for (int j = 0; j < curves[i].Points.Count; j++)
                    {
                        Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(curves[i].Points[j].X, Screen.height - curves[i].Points[j].Y, 0));
                        point = new Vector3(point.x, point.y, 0);
                        points.Add(point);
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
                lineRenderer.positionCount = 2;
                System.Numerics.Vector2 pointA2D = Utilities.OsuPixelToScreenPoint(new System.Numerics.Vector2(slider.SliderPoints[0].X, slider.SliderPoints[0].Y));
                Vector3 pointA = Camera.main.ScreenToWorldPoint(new Vector3(pointA2D.X, Screen.height - pointA2D.Y, 0));
                pointA = new Vector3(pointA.x, pointA.y, 0);
                points.Add(pointA);
                System.Numerics.Vector2 pointB2D = Utilities.OsuPixelToScreenPoint(new System.Numerics.Vector2(slider.SliderPoints[slider.SliderPoints.Count - 1].X, slider.SliderPoints[slider.SliderPoints.Count - 1].Y));
                Vector3 pointB = Camera.main.ScreenToWorldPoint(new Vector3(pointB2D.X, Screen.height - pointB2D.Y, 0));
                pointB = new Vector3(pointB.x, pointB.y, 0);
                points.Add(pointB);
                lineRenderer.SetPositions(points.ToArray());
                break;
            }
            case CurveType.PerfectCurve:
            {
                
                name = "Circle Slider(Clone)";
                if (Utilities.IsCollinear(Utilities.OsuPixelToScreenPoint(slider.SliderPoints[0]), Utilities.OsuPixelToScreenPoint(slider.SliderPoints[1]), Utilities.OsuPixelToScreenPoint(slider.SliderPoints[2])))
                {
                    lineRenderer.positionCount = 2;
                    System.Numerics.Vector2 pointA2D = Utilities.OsuPixelToScreenPoint(new System.Numerics.Vector2(slider.SliderPoints[0].X, slider.SliderPoints[0].Y));
                    Vector3 pointA = Camera.main.ScreenToWorldPoint(new Vector3(pointA2D.X, Screen.height - pointA2D.Y, 0));
                    pointA = new Vector3(pointA.x, pointA.y, 0);
                    points.Add(pointA);
                    System.Numerics.Vector2 pointB2D = Utilities.OsuPixelToScreenPoint(new System.Numerics.Vector2(slider.SliderPoints[slider.SliderPoints.Count - 1].X, slider.SliderPoints[slider.SliderPoints.Count - 1].Y));
                    Vector3 pointB = Camera.main.ScreenToWorldPoint(new Vector3(pointB2D.X, Screen.height - pointB2D.Y, 0));
                    pointB = new Vector3(pointB.x, pointB.y, 0);
                    points.Add(pointB);
                    lineRenderer.SetPositions(points.ToArray());
                    break;
                }
                System.Numerics.Vector2 center = Utilities.CalculateCircleCenter(slider.SliderPoints[0], slider.SliderPoints[1], slider.SliderPoints[2]);
                // Debug.Log($"{center:F6}, r={radius:F6}, A={Utilities.OsuPixelToScreenPoint(slider.SliderPoints[0]):F6}, B={Utilities.OsuPixelToScreenPoint(slider.SliderPoints[1]):F6}, C={Utilities.OsuPixelToScreenPoint(slider.SliderPoints[2]):F6}");
                System.Numerics.Vector2 OA = Utilities.OsuPixelToScreenPoint(new System.Numerics.Vector2(slider.SliderPoints[0].X - center.X, slider.SliderPoints[0].Y - center.Y));
                System.Numerics.Vector2 OB = Utilities.OsuPixelToScreenPoint(new System.Numerics.Vector2(slider.SliderPoints[1].X - center.X, slider.SliderPoints[1].Y - center.Y));
                System.Numerics.Vector2 OC = Utilities.OsuPixelToScreenPoint(new System.Numerics.Vector2(slider.SliderPoints[2].X - center.X, slider.SliderPoints[2].Y - center.Y));
                float radius = OA.Length();
                float angleCcwOaOb = Utilities.CalculateOrientedAngle(OA, OB);
                if (angleCcwOaOb < 0)
                {
                    angleCcwOaOb += 2 * (float)Math.PI;
                }
                float angleCcwOaOc = Utilities.CalculateOrientedAngle(OA, OC);
                if (angleCcwOaOc < 0)
                {
                    angleCcwOaOc += 2 * (float)Math.PI;
                }
                if (angleCcwOaOb > angleCcwOaOc)
                {
                    angleCcwOaOc -= 2 * (float)Math.PI;
                }
                float segmentAngle = angleCcwOaOc / ACCURACY;
                float currentAngle = Mathf.Atan2(OA.Y, OA.X);
                lineRenderer.positionCount = ACCURACY;
                for (int i = 0; i < ACCURACY; i++)
                {
                    System.Numerics.Vector2 point2D = new System.Numerics.Vector2(Utilities.OsuPixelToScreenPointX(center.X) + radius * Mathf.Cos(currentAngle), Utilities.OsuPixelToScreenPointY(center.Y) + radius * Mathf.Sin(currentAngle));
                    Vector3 point = Camera.main.ScreenToWorldPoint(new Vector3(point2D.X, Screen.height - point2D.Y, 0));
                    point = new Vector3(point.x, point.y, 0);
                    points.Add(point);
                    currentAngle += segmentAngle;
                }
                lineRenderer.SetPositions(points.ToArray());
                break;
            }
        }
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
        if (PlayField.GameTimer.ElapsedMilliseconds > slider.EndTime)
        {
            Destroy(gameObject);
        }
    }
}
