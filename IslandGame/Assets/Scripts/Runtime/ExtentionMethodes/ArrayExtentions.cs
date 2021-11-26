using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = System.Random;

public static class ArrayExtentions
{
    public static T Random<T>(this T[] array)
    {
        int randomIndex = UnityEngine.Random.Range(0, array.Length);
        return array[randomIndex];
    }

    public static T[] Shuffle<T>(this T[] array)
    {
        List<T> list = new List<T>(array);
        Random rng = new Random();
        int n = list.Count;  
        while (n > 1) {  
            n--;  
            int k = rng.Next(n + 1);  
            T value = list[k];  
            list[k] = list[n];  
            list[n] = value;  
        }

        return list.ToArray();
    }
}
