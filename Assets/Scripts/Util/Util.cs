using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public static partial class ScriptHelper
{
    public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
    {
        foreach (var item in list)
        {
            action(item);
        }
    }
    public static List<T> ToList<T>(this IEnumerable<T> a)
    {
        List<T> results = new List<T>();

        foreach (T item in a)
        {
            results.Add(item);
        }

        return results;
    }
    public static Sprite GetTileSprite(this Tilemap tilemap, Vector2 position, float tilemapY)
    {
        Vector3Int localPlace = (new Vector3Int((int)position.x, (int)position.y, (int)tilemapY));

        if (tilemap.HasTile(localPlace))
        {
            return tilemap.GetSprite(localPlace);
        }
        else
        {
            return null;
        }
    }
    public static int Abs(this int a)
    {
        if(a < 0)
        {
            return -a;
        }
        else
        {
            return a;
        }
    }
    public static float Abs(this float a)
    {
        if (a < 0)
        {
            return -a;
        }
        else
        {
            return a;
        }
    }
    public static int Round(this float a)
    {
        float result = a;

        while(true)
        {
            if(--result < 1)
            {
                break;
            }
        }

        if(result < 0.5f)
        {
            return (int)(a - result);
        }
        else
        {
            return (int)++a;
        }
    }
    public static Vector2 RandomVector(Vector2 min, Vector2 max)
    {
        float x = UnityEngine.Random.Range(min.x, max.x);
        float y = UnityEngine.Random.Range(min.y, max.y);

        return new Vector2(x, y);
    }
    public static Vector2 Sum(this Vector2 vec1, Vector2 vec2)
    {
        return new Vector2(vec1.x + vec2.x, vec1.y + vec2.y);
    }
    public static Vector3 Sum(this Vector3 vec1, Vector3 vec2)
    {
        return new Vector3(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z);
    }
    public static Vector4 Sum(this Vector4 vec1, Vector4 vec2)
    {
        return new Vector4(vec1.x + vec2.x, vec1.y + vec2.y, vec1.z + vec2.z, vec1.w + vec2.w);
    }
    public static Color SetColorAlpha(this Color color, float a)
    {
        Color newColor = color;
        newColor.a = a;

        return newColor;
    }
    public static bool CompareGameObjectLayer(this LayerMask layerMask, GameObject targetObj) // targetObj�� layer�� layerMask�ȿ� �ִ��� üũ
    {
        int layer = 1 << targetObj.layer;

        return layerMask == (layerMask | layer);
    }
    // ---Limit�żҵ忡 ���� ����---
    // value = 0, min = 1, max = 3�� �� 3�� �����Ѵ�.
    // value = -2, min = 1, max = 3�� �� 1�� �����Ѵ�.
    // value = -5, min = 1, max = 3�� �� 1�� �����Ѵ�.
    // value = 4, min = 1, max = 3�� �� 1�� �����Ѵ�.
    // value = 5, min = 1, max = 3�� �� 2�� �����Ѵ�.
    // value = 8, min = 1, max = 3�� �� 2�� �����Ѵ�.
    // value���� min���� max�� ���̸� �պ��Ѵٰ� �����ϸ� �ȴ�.
    // value���� min������ �������� (min - value) ��ŭ�� max������ ���� ���� ���� value���� �����Ѵ�. �� ������ value���� min�� �̻��� �� �� ���� �ݺ��Ѵ�.
    // value���� max������ �������� (value - max) ��ŭ�� min������ ���ؼ� ���� ���� value���� �����Ѵ�. �� ������ value���� max�� ���ϰ� �� �� ���� �ݺ��Ѵ�.
    public static int Limit(this int value, int min, int max)
    {
        if (value < min)
        {
            return Limit(max - (min - value - 1), min, max);
        }
        else if (value > max)
        {
            return Limit(min + (value - max - 1), min, max);
        }
        else
        {
            return value;
        }
    }
}