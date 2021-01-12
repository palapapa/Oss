using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    //Get all interfaces T of all Components attacted to GameObject objectToSearch
    public static List<T> GetInterfaces<T>(this GameObject objectToSearch) where T : class
    {
        Component[] components = objectToSearch.GetComponents<Component>();
        List<T> resultList = new List<T>();
        foreach (Component co in components)
        {
            if (co is T)
            {
                resultList.Add((T)(object)co);
            }
        }
        return resultList;
    }

    //Get the interface T of a MonoBehavior attached to GameObject objectToSearch
    public static T GetInterface<T>(this GameObject objectToSearch) where T : class
    {
        MonoBehaviour mb = objectToSearch.GetComponent<MonoBehaviour>();
        if (mb is T)
        {
            return (T)(object)mb;
        }
        else
        {
            return null;
        }
    }

    /*
    public static void DisableRaycastTarget(GameObject gameObject)
    {
        UnityEngine.EventSystems.PhysicsRaycaster raycaster = gameObject.GetComponent<UnityEngine.EventSystems.PhysicsRaycaster>();
        if (raycaster != null)
        {
            raycaster.enabled = false;
        }
    }
    public static void DisableRaycastTarget(UnityEngine.UI.Image image)
    {
        UnityEngine.EventSystems.PhysicsRaycaster raycaster = image.GetComponent<UnityEngine.EventSystems.PhysicsRaycaster>();
        if (raycaster != null)
        {
            raycaster.enabled = false;
        }
    }
    */

    //Switch to panel and make all other panels invisible
    public static void SwitchPanel(this GameObject panel)
    {
        PlayerData.ActivePanel = panel;
        panel.transform.SetAsLastSibling();//bring to front
        Debug.Log(PlayerData.ActivePanel);
    }

    //fade canvas group in speed seconds with steps steps
    public static IEnumerator FadeCanvasGroup(this GameObject gameObject, float time, uint step, float from, float to)
    {
        if (from > 1.0f || from < 0.0f || to > 1.0f || to < 0.0f)
        {
            throw new ArgumentException("Can only range from 1.0 to 0.0", nameof(from) + ", " + nameof(to));
        }
        if (time < 0.0f)
        {
            throw new ArgumentException("Cannot be less than 0.0", nameof(time));
        }
        CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();
        canvasGroup.alpha = from;
        float alphaStep = (from - to) / step, timeStep = time / step;
        float idealTime = 0, actualTime = 0;
        for (int i = 0; i < step; i++)
        {
            canvasGroup.alpha -= alphaStep;
            idealTime += timeStep;
            actualTime += Time.deltaTime;
            while (actualTime > idealTime)
            {
                canvasGroup.alpha -= alphaStep;
                idealTime += timeStep;
                i++;
            }
            yield return new WaitForSeconds(timeStep);
        }
    }

    //fade canvas group in speed seconds with steps steps
    public static IEnumerator FadeCanvasGroupTo(this GameObject gameObject, float time, uint step, float to)
    {
        if (to > 1.0f || to < 0.0f)
        {
            throw new ArgumentException("Can only range from 1.0 to 0.0", nameof(to));
        }
        if (time < 0.0f)
        {
            throw new ArgumentException("Cannot be less than 0.0", nameof(time));
        }
        CanvasGroup canvasGroup = gameObject.GetComponent<CanvasGroup>();
        float from = canvasGroup.alpha;
        float alphaStep = (from - to) / step, timeStep = time / step;
        float idealTime = 0, actualTime = 0;
        for (int i = 0; i < step; i++)
        {
            canvasGroup.alpha -= alphaStep;
            idealTime += timeStep;
            actualTime += Time.deltaTime;
            while (actualTime > idealTime)
            {
                canvasGroup.alpha -= alphaStep;
                idealTime += timeStep;
                i++;
            }
            yield return new WaitForSeconds(time / step);
        }
    }

    public static Rect RectTransformToScreenSpace(RectTransform rectTransform)
    {
        Vector2 size = Vector2.Scale(rectTransform.rect.size, rectTransform.lossyScale);
        Rect rect = new Rect(rectTransform.position.x, Screen.height - rectTransform.position.y, size.x, size.y);
        rect.x -= (rectTransform.pivot.x * size.x);
        rect.y -= ((1.0f - rectTransform.pivot.y) * size.y);
        return rect;
    }

    public static System.Numerics.Vector2 OsuPixelToScreenPoint(System.Numerics.Vector2 osuPixel)
    {
        return new System.Numerics.Vector2(OsuPixelToScreenPointX(osuPixel.X), OsuPixelToScreenPointY(osuPixel.Y));
    }

    public static System.Numerics.Vector2 OsuPixelToScreenPoint(float x, float y)
    {
        return new System.Numerics.Vector2(OsuPixelToScreenPointX(x), OsuPixelToScreenPointY(y));
    }

    public static float OsuPixelToScreenPointX(float x)
    {
        return x * Screen.width / 512;
    }

    public static float OsuPixelToScreenPointY(float y)
    {
        return y * Screen.height / 384;
    }

    public static Vector2 OsuPixelToScreenPointUnity(System.Numerics.Vector2 osuPixel)
    {
        return new Vector2(OsuPixelToScreenPointX(osuPixel.X), OsuPixelToScreenPointY(osuPixel.Y));
    }

    public static List<System.Numerics.Vector2> OsuPixelsToScreenPoints(List<System.Numerics.Vector2> osuPixels)
    {
        List<System.Numerics.Vector2> result = new List<System.Numerics.Vector2>();
        foreach (System.Numerics.Vector2 coordinate in osuPixels)
        {
            result.Add(new System.Numerics.Vector2(OsuPixelToScreenPointX(coordinate.X), OsuPixelToScreenPointY(coordinate.Y)));
        }
        return result;
    }

    public static List<float> SolveThreeVariableLinearEquation(ThreeVariableLinearEquation A, ThreeVariableLinearEquation B, ThreeVariableLinearEquation C)
    {
        float delta = new Matrix3x3
        (
            A.XCoefficient, A.YCoefficient, A.ZCoefficient,
            B.XCoefficient, B.YCoefficient, B.ZCoefficient,
            C.XCoefficient, C.YCoefficient, C.ZCoefficient
        ).CalculateMatrix();
        float deltaX = new Matrix3x3
        (
            A.Answer, A.YCoefficient, A.ZCoefficient,
            B.Answer, B.YCoefficient, B.ZCoefficient,
            C.Answer, C.YCoefficient, C.ZCoefficient
        ).CalculateMatrix();
        float deltaY = new Matrix3x3
        (
            A.XCoefficient, A.Answer, A.ZCoefficient,
            B.XCoefficient, B.Answer, B.ZCoefficient,
            C.XCoefficient, C.Answer, C.ZCoefficient
        ).CalculateMatrix();
        float deltaZ = new Matrix3x3
        (
            A.XCoefficient, A.YCoefficient, A.Answer,
            B.XCoefficient, B.YCoefficient, B.Answer,
            C.XCoefficient, C.YCoefficient, C.Answer
        ).CalculateMatrix();
        List<float> result = new List<float>
        {
            deltaX / delta,
            deltaY / delta,
            deltaZ / delta
        };
        return result;
    }

    public static System.Numerics.Vector2 CalculateCircleCenter(System.Numerics.Vector2 A, System.Numerics.Vector2 B, System.Numerics.Vector2 C)
    {
        float yDeltaA = B.Y - A.Y;
        float xDeltaA = B.X - A.X;
        float yDeltaB = C.Y - B.Y;
        float xDeltaB = C.X - B.X;
        System.Numerics.Vector2 center = new System.Numerics.Vector2(0, 0);

        float aSlope = yDeltaA / xDeltaA;
        float bSlope = yDeltaB / xDeltaB;

        System.Numerics.Vector2 ABMid = new System.Numerics.Vector2((A.X + B.X) / 2, (A.Y + B.Y) / 2);
        System.Numerics.Vector2 BCMid = new System.Numerics.Vector2((B.X + C.X) / 2, (B.Y + C.Y) / 2);

        if (yDeltaA == 0) // aSlope == 0
        {
            center.X = ABMid.X;
            if (xDeltaB == 0) // bSlope == INFINITY
            {
                center.Y = BCMid.Y;
            }
            else
            {
                center.Y = BCMid.Y + (BCMid.X - center.X) / bSlope;
            }
        }
        else if (yDeltaB == 0) // bSlope == 0
        {
            center.X = BCMid.X;
            if (xDeltaA == 0) // aSlope == INFINITY
            {
                center.Y = ABMid.Y;
            }
            else
            {
                center.Y = ABMid.Y + (ABMid.X - center.X) / aSlope;
            }
        }
        else if (xDeltaA == 0) // aSlope == INFINITY
        {
            center.Y = ABMid.Y;
            center.X = bSlope * (BCMid.Y - center.Y) + BCMid.X;
        }
        else if (xDeltaB == 0) // bSlope == INFINITY
        {
            center.Y = BCMid.Y;
            center.X = aSlope * (ABMid.Y - center.Y) + ABMid.X;
        }
        else
        {
            center.X = (aSlope * bSlope * (ABMid.Y - BCMid.Y) - aSlope * BCMid.X + bSlope * ABMid.X) / (bSlope - aSlope);
            center.Y = ABMid.Y - (center.X - ABMid.X) / aSlope;
        }
        return center;
    }

    public static float CalculateOrientedAngle(System.Numerics.Vector2 from, System.Numerics.Vector2 to)
    {
        return (float)Math.Atan2(from.X * to.Y - to.X * from.Y, from.X * to.X + from.Y * to.Y);
    }

    public static float CalculateOrientedAngle(float x1, float y1, float x2, float y2)
    {
        return (float)Math.Atan2(x1 * y2 - y1 * x2, x1 * x2 + y1 * y2);
    }

    public static bool IsCollinear(System.Numerics.Vector2 a, System.Numerics.Vector2 b, System.Numerics.Vector2 c)
    {
        return (c.Y - b.Y) * (b.X - a.X) == (b.Y - a.Y) * (c.X - b.X);
    }

    public static bool IsCollinear(System.Numerics.Vector2 a, System.Numerics.Vector2 b, System.Numerics.Vector2 c, float slopeLeniency)
    {
        return Math.Abs((a.Y - b.Y) / (a.X - b.X) - (b.Y - c.Y) / (b.X - c.X)) <= slopeLeniency;
    }
    
}