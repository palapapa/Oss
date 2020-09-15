using System.Collections.Generic;
using UnityEngine;

public class InterfaceUtilities
{
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
}