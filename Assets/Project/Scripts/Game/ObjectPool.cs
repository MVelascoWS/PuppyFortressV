using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour {

    public static ObjectPool instance;

    [System.Serializable]
    public struct ObjectConfig
    {
        public Object prefab;
        public int amount;
    }

    public ObjectConfig[] objectPrefabs;

    public Dictionary<string, List<GameObject>> objectPool;

    private void Awake()
    {
        instance = this;
    }

    void Start () {
        objectPool = new Dictionary<string, List<GameObject>>(objectPrefabs.Length);

        for (int i = 0; i < objectPrefabs.Length; i++)
        {
            GameObject temp;
            string objName = objectPrefabs[i].prefab.name;
            objectPool.Add(objName, new List<GameObject>());

            for (int j = 0; j < objectPrefabs[i].amount; j++)
            {
                temp = Instantiate(objectPrefabs[i].prefab) as GameObject;
                temp.name = objName;

                ReturnPoolObject(temp, false);                
            }
        }

	}

    public GameObject TakePoolObject(Object obj, bool allowNewInstance)
    {
        return TakePoolObject(obj.name, allowNewInstance);
    }

    public GameObject TakePoolObject(string typeName, bool allowNewInstance)
    {
        if (objectPool.ContainsKey(typeName))
        {
            if (objectPool[typeName].Count > 0)
            {
                GameObject temp = objectPool[typeName][0];
                objectPool[typeName].RemoveAt(0);
                temp.transform.parent = null;
                temp.SetActive(true);
                return temp;
            }
            else if (allowNewInstance)
            {
                for (int i = 0; i < objectPrefabs.Length; i++)
                {
                    if (objectPrefabs[i].prefab.name == typeName)
                    {
                        GameObject temp = Instantiate(objectPrefabs[i].prefab) as GameObject;
                        temp.name = typeName;
                        temp.SetActive(true);
                        return temp;
                    }
                }
            }                
        }
        return null;
    }

    public void ReturnPoolObject(GameObject go, bool setActive)
    {
        go.SetActive(setActive);
        if (!setActive)
        {            
            go.transform.SetPositionAndRotation(Vector3.zero, Quaternion.identity);
        }
        go.transform.parent = this.gameObject.transform;
        objectPool[go.name].Add(go);
    }
}
