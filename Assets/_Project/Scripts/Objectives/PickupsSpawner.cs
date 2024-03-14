using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupsSpawner : MonoBehaviour
{
    [SerializeField] private Transform WrenchParent;
    [SerializeField] private int WrenchesToSpawn = 2;
    [SerializeField] private Transform KeysParent;
    [SerializeField] private int KeysToSpawn = 2;

    private void Start()
    {
        EnableRandomAmount(WrenchParent, WrenchesToSpawn);
        EnableRandomAmount(KeysParent, KeysToSpawn);
    }

    private static void EnableRandomAmount(Transform parent, int amount)
    {
        foreach (Transform child in parent)
        {
            child.gameObject.SetActive(false);
        }

        for (int i = 0; i < amount; i++)
        {
            int randomIndex = Random.Range(0, parent.childCount);
            if (parent.GetChild(randomIndex).gameObject.activeSelf)
            {
                i--;
                continue;
            }
            parent.GetChild(randomIndex).gameObject.SetActive(true);
        }
    }
}
