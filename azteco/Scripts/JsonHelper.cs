using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class JsonHelper
{
    public static T[] FromJson<T>(string json)
    {
        Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);
        return wrapper.Items;
    }

    public static string fixJson(string value)
    {
        value = "{\"Items\":" + value + "}";
        return value;
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public T[] Items;
    }
}