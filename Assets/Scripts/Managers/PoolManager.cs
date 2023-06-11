using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoSingleton<PoolManager>
{
    private Dictionary<string, Queue<object>> poolingObjects = new();

    public void SetObject(string _objectKey, object _object)
    {
        if (poolingObjects.ContainsKey(_objectKey))
        {
            poolingObjects[_objectKey].Enqueue(_object);
        }
        else
        {
            Queue<object> objectQueue = new();
            objectQueue.Enqueue(_object);

            poolingObjects.Add(_objectKey, objectQueue);
        }
    }
    public void SetObject(string _objectKey, GameObject _object)
    {
        if (_object.activeSelf)
        {
            _object.SetActive(false);
        }

        SetObject(_objectKey, (object)_object);
    }

    public T GetObject<T>(string _objectKey)
    {
        T result = default(T);

        if (poolingObjects.ContainsKey(_objectKey))
        {
            if (poolingObjects[_objectKey].Count > 0)
            {
                result = (T)poolingObjects[_objectKey].Dequeue();
            }
        }

        return result;
    }
}
