using RPG.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG.SceneManagement
{


    public class SavingWrapper : MonoBehaviour
    {
        const string defaultSaveFile = "save";
        private SavingSystem savingSystem;
        [SerializeField] private float fadeInTime = 0.25f;

        private IEnumerator Start()
        {
            this.savingSystem = this.GetComponent<SavingSystem>();
            Fader fader = FindObjectOfType<Fader>();
            //fader.FadeOutInmediate();
            yield return this.savingSystem.LoadLastScene(defaultSaveFile);
            //yield return fader.FadeIn(fadeInTime);
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.L))
            {
                Load();
            }

            if (Input.GetKeyDown(KeyCode.S))
            {
                Save();
            }
        }

        public void Load()
        {
            this.savingSystem.Load(defaultSaveFile);
        }

        public void Save()
        {
            this.savingSystem.Save(defaultSaveFile);
        }
    }
}