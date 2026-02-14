
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameTag : MonoBehaviour
{
    private static Dictionary<string, HashSet<GameTag>> aliveTags = new();

    [SerializeField] private string gameTag;

    private void OnEnable()
    {
        if (!aliveTags.ContainsKey(gameTag))
        {
            aliveTags[gameTag] = new HashSet<GameTag>();
        }
        var list = aliveTags[gameTag];
        list.Add(this);
    }

    private void OnDisable()
    {
        aliveTags[gameTag].Remove(this);
        if (!aliveTags[gameTag].Any())
        {
            aliveTags.Remove(gameTag);
        }
    }

    public static GameObject[] GetObjectsWithTag(string tag)
    {
        if (!aliveTags.ContainsKey(tag))
            return Array.Empty<GameObject>();

        return Array.ConvertAll(
            aliveTags[tag].ToArray(),
            gt => gt.gameObject
            );
    }

    public static GameObject GetFirstObjectWith(string tag)
    {
        if (!aliveTags.ContainsKey(tag))
            return null;

        return aliveTags[tag].First().gameObject;
    }
}
