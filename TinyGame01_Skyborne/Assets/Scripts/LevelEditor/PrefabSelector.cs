using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrefabSelector : MonoBehaviour
{
    public GameObject[] Prefabs;

    public GameObject ChooseRandom()
    {
        int index = Random.Range(0, Prefabs.Length);
        GameObject go = Prefabs[index];

        return go;
    }
}
