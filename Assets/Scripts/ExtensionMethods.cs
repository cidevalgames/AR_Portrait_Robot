using System.Collections.Generic;
using UnityEngine;
using System;

public static class ExtensionMethods
{
    public static T GetComponentInChildrenOnly<T>(this Transform t)
    {
        T[] components = t.GetComponentsInChildren<T>();

        if (components[0].Equals(t.GetComponent<T>()))
        {
            if (components.Length > 1)
            {
                return (T)(object)components[1];
            }
            else
            {
                return (T)(object)null;
            }
        }

        return (T)(object)components[0];
    }
}
