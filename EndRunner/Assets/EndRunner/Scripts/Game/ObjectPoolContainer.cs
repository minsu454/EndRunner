using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPoolContainer : MonoBehaviour {
    private static ObjectPoolContainer instance;
    public static ObjectPoolContainer Instance {
        get {
            if (instance == null) {
                GameObject obj = new GameObject("ObjectPoolContainer");
                instance = obj.AddComponent<ObjectPoolContainer>();
            }
            return instance;
        }
    }

    private Dictionary<string, List<GameObject>> objectPoolDic = new Dictionary<string, List<GameObject>>();

    public void CreateObjectPool(string poolingName, GameObject obj, int createCount, Transform parentTr = null) {
        GameObject cloneobj;
        List<GameObject> poolList = new List<GameObject>();
        for (int i = 0; i < createCount; i++) {
            if (parentTr != null)
            {
                cloneobj = Instantiate(obj, parentTr);
            }
            else {
                cloneobj = Instantiate(obj, this.transform);
            }
            cloneobj.name = poolingName;
            poolList.Add(cloneobj);
        }
        objectPoolDic.Add(poolingName, poolList);
    }

    public GameObject Pop(string poolingName) {
        if (objectPoolDic[poolingName].Count == 1) {
            GameObject cloneObj = Instantiate(objectPoolDic[poolingName][0], objectPoolDic[poolingName][0].transform.parent);
            cloneObj.name = poolingName;
            objectPoolDic[poolingName].Add(cloneObj);

            Debug.LogError("Create More Object Pool : " + poolingName);
        }

        GameObject returnObj = objectPoolDic[poolingName][0];
        objectPoolDic[poolingName].RemoveAt(0);
        return returnObj;
    }

    public void Return(GameObject obj) {
        objectPoolDic[obj.name].Add(obj);
    }

    private void OnDestroy()
    {
        instance = null;
    }

}
