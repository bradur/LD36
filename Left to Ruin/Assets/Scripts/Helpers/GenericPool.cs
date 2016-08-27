// Date   : 27.08.2016 18:21
// Project: LD36
// Author : bradur

using UnityEngine;
using System.Collections.Generic;

public class GenericPool : MonoBehaviour
{

    [SerializeField]
    private List<GameObject> poolContents;

    [SerializeField]
    private Transform parent;

    public void Init(int size, GameObject poolMemberPrefab)
    {
        Debug.Log("Initializing a size " + size + " pool.");
        poolContents = new List<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject poolObject = (GameObject)Instantiate(poolMemberPrefab);
            poolObject.transform.SetParent(parent, false);
            poolObject.SetActive(false);
            poolContents.Add(poolObject);
        }
    }

    public GameObject GetObject()
    {
        if (poolContents.Count > 0)
        {
            GameObject retrievedObject = poolContents[0];
            poolContents.RemoveAt(0);
            return retrievedObject;
        }
        return null;
    }

    public void DestroyObject(GameObject returningObject)
    {
        poolContents.Add(returningObject);
        returningObject.SetActive(false);
    }
}
