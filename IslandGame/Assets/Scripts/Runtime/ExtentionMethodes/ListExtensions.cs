using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class ListExtensions
{
    public static T Random<T>(this List<T> list)
    {
        int randomIndex = UnityEngine.Random.Range(0, list.Count);
        return list[randomIndex];
    }

    public static List<T> Shuffle<T>(this List<T> list)
    {
        Random rng = new Random();
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }

        return list;
    }

    public static List<T> AddIfNotExist<T>(this List<T> list, T item)
    {
        if (!list.Contains(item)) list.Add(item);
        return list;
    }

    public static bool AddIfNotExistBool<T>(this List<T> list, T item)
    {
        if (!list.Contains(item))
        {
            list.Add(item);
            return true;
        }
        return false;
    }
}
