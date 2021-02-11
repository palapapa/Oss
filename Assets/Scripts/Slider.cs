using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OsuHitObjects = OsuParsers.Beatmaps.Objects;
using OsuParsers.Enums.Beatmaps;
using UnityEngine.UI;

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
    [SerializeField]
    private GameObject sliderHead;
    [SerializeField]
    private GameObject hitCircleTextureHead;
    [SerializeField]
    private GameObject hitCircleOverlayHead;
    [SerializeField]
    private GameObject sliderEnd;
    [SerializeField]
    private GameObject hitCircleTextureEnd;
    [SerializeField]
    private GameObject hitCircleOverlayEnd;
    [SerializeField]
    private GameObject followCircle;
    [SerializeField]
    private GameObject sliderBall;
    private OsuHitObjects.Slider slider;
    private List<BezierPath> curves;
    private CirclePath circlePath;
    private LinearPath linearPath;
    private LineRenderer lineRenderer;
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    /// <summary>
    /// World points on the path of the slider.
    /// </summary>
    private List<Vector3> points = new List<Vector3>();
    /// <summary>
    /// World evenly spaced points on the path of the slider.
    /// </summary>
    private List<Vector3> evenlySpacedPoints = new List<Vector3>();
    private float sliderSnakingInterval;
    private int originalPositionCount;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    private const int ACCURACY = 50;
    private float pathWidth;

    private void Awake()
    {
        ar = MusicPlayer.CurrentPlaying.DifficultySection.ApproachRate;
        cs = MusicPlayer.CurrentPlaying.DifficultySection.CircleSize;
        hp = MusicPlayer.CurrentPlaying.DifficultySection.HPDrainRate;
        od = MusicPlayer.CurrentPlaying.DifficultySection.OverallDifficulty;
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
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
        meshFilter = GetComponent<MeshFilter>();
        meshRenderer = GetComponent<MeshRenderer>();
        curves = new List<BezierPath>();
        lineRenderer.numCornerVertices = 16;
        lineRenderer.numCapVertices = 16;
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
                        evenlySpacedPoints.Add(Utilities.ScreenToWorldPoint2D(curves[i].EvenlySpacedPoints[j].X, curves[i].EvenlySpacedPoints[j].Y));
                    }
                }
                lineRenderer.SetPositions(points.ToArray());
                break;
            }
            case CurveType.Catmull:
            {
                name = "Catmull Slider(Clone)";
                Debug.LogWarning("Catmull sliders are currently not supproted.");
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
                foreach (System.Numerics.Vector2 ev in linearPath.EvenlySpacedPoints)
                {
                    evenlySpacedPoints.Add(Utilities.ScreenToWorldPoint2D(ev.X, ev.Y));
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
                foreach (System.Numerics.Vector2 ev in circlePath.EvenlySpacedPoints)
                {
                    evenlySpacedPoints.Add(Utilities.ScreenToWorldPoint2D(ev.X, ev.Y));
                }
                lineRenderer.positionCount = ACCURACY;
                lineRenderer.SetPositions(points.ToArray());
                break;
            }
        }
        sliderSnakingInterval = (float)fadeIn / lineRenderer.positionCount;
        originalPositionCount = lineRenderer.positionCount;
    }

    private void Start()
    {
        float targetSize = (float)(150 * (1 - 0.7 * (cs - 5) / 5)); // target circle size in pixels
        float currentSize = rectTransform.rect.width * transform.localScale.x;
        float targetScale = targetSize / currentSize;
        transform.localScale *= targetScale;
        Vector3[] worldCorners = new Vector3[4];
        rectTransform.GetWorldCorners(worldCorners);
        lineRenderer.widthCurve = AnimationCurve.Linear(0, worldCorners[0].x - worldCorners[3].x, 1, worldCorners[0].x - worldCorners[3].x);
        pathWidth = worldCorners[0].x - worldCorners[3].x;
        gameObject.transform.position = points[0];
        sliderHead.transform.position = points[0];
        sliderEnd.transform.position = points.Last(); // this must be placed after SetCircleSize because scale affects the position of the circles
        // meshFilter.mesh = CalculatePathMesh(evenlySpacedPoints, pathWidth);
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
                Mathf.Clamp01((float)(PlayField.GameTimer.ElapsedMilliseconds - (slider.StartTime - preempt)) / fadeIn)
            )
        );
        meshRenderer.material.SetColor
        (
            "_Color",
            new Color
            (
                meshRenderer.material.color.r,
                meshRenderer.material.color.g,
                meshRenderer.material.color.b,
                Mathf.Clamp01((float)(PlayField.GameTimer.ElapsedMilliseconds - (slider.StartTime - preempt)) / fadeIn)
            )
        );
        int positionCount = (int)Math.Max(0, (PlayField.GameTimer.ElapsedMilliseconds - (slider.StartTime - preempt)) / sliderSnakingInterval);
        positionCount = Mathf.Clamp(positionCount, 0, originalPositionCount);
        lineRenderer.positionCount = positionCount;
        lineRenderer.SetPositions(points.ToArray());
        positionCount = Mathf.Clamp(positionCount, 0, originalPositionCount - 1);
        sliderEnd.transform.position = points[positionCount];
        canvasGroup.alpha = Mathf.Clamp01((float)(PlayField.GameTimer.ElapsedMilliseconds - (slider.StartTime - preempt)) / fadeIn);
        if (PlayField.GameTimer.ElapsedMilliseconds > slider.EndTime)
        {
            Destroy(gameObject);
        }
    }
    /*
    /// <summary>
    /// 
    /// </summary>
    /// <param name="points">Evenly spaced points on the curve</param>
    /// <returns></returns>
    private Mesh CalculatePathMesh(List<Vector3> points, float width)
    {
        Vector3[] vertices = new Vector3[points.Count * 2];
        int vertexIndex = 0;
        int[] triangles = new int[2 * (points.Count - 1) * 3];
        int triangleIndex = 0;
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 forward = Vector3.zero;
            if (i != points.Count - 1) // if not last
            {
                forward += points[i + 1] - points[i]; // from current to next
            }
            if (i != 0) // if not first
            {
                forward += points[i] - points[i - 1]; // from last to current
            }
            forward.Normalize(); // get average of last to current & current to next
            Vector3 left = new Vector3(-forward.y, forward.x, 0); // perpendicular to forward
            vertices[vertexIndex] = points[i] + left * width * 0.5f;
            vertices[vertexIndex + 1] = points[i] - left * width * 0.5f;
            if (i != points.Count - 1)
            {
                triangles[triangleIndex] = vertexIndex;
                triangles[triangleIndex + 1] = vertexIndex + 2;
                triangles[triangleIndex + 2] = vertexIndex + 1;
                triangles[triangleIndex + 3] = vertexIndex + 1;
                triangles[triangleIndex + 4] = vertexIndex + 2;
                triangles[triangleIndex + 5] = vertexIndex + 3;
            }
            vertexIndex += 2;
            triangleIndex += 6;
        }
        Mesh mesh = new Mesh
        {
            vertices = vertices,
            triangles = triangles
        };
        return mesh;
    }
    */
}
