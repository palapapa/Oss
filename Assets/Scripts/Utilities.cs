using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utilities
{
    public static float PlayAudio(AudioClip ac)
    {
        GameObject gameObject = new GameObject(ac.name);
        AudioSource audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(ac);
        return Time.time;
    }
    //Get all interfaces T of all Components attacted to GameObject objectToSearch
    public static List<T> GetInterfaces<T>(GameObject objectToSearch) where T : class
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
    public static T GetInterface<T>(GameObject objectToSearch) where T : class
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
    public static void SwitchPanelSingle(this GameObject panel)
    {
        GameObject[] panels = GameObject.FindGameObjectsWithTag("Panel");
        foreach (GameObject p in panels)
        {
            p.GetComponent<CanvasGroup>().alpha = 0;
        }
        panel.GetComponent<CanvasGroup>().alpha = 1;
        Data.ActivePanel = panel;
        panel.transform.SetAsLastSibling();//bring to front
    }
}
