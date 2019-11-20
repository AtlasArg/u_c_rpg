using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.Core
{
    public class PersistentObjectsSpawner : MonoBehaviour
    {
        [SerializeField] GameObject prefab;

        static bool hasSpawn = false;

        private void Awake()
        {
            if (!hasSpawn)
            {
                SpawnPersistentObjects();
                hasSpawn = true;
            }
        }

        private void SpawnPersistentObjects()
        {
            GameObject persistentObject = Instantiate(prefab);
            DontDestroyOnLoad(persistentObject);
        }

    }
}

