using System;
using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Assertions;
using UnityEngine.SceneManagement;

namespace RPG.SceneManagement
{
    public class Portal : MonoBehaviour
    {
        enum DestinationIdentifier
        {
            A, B, C, D, E
        }

        [SerializeField] private int sceneToLoad = -1;
        [SerializeField] private Transform spawnPoint;
        [SerializeField] private DestinationIdentifier destination;
        [SerializeField] private float fadeOutTime = 1f;
        [SerializeField] private float fadeInTime = 2f;
        [SerializeField] private float fadeWaitTime = 0.25f;

        private Fader fader;

        private void Awake()
        {
            Assert.IsNotNull(spawnPoint, "Spawn point must be initialized");
            Assert.IsFalse(sceneToLoad == -1, "A valid Scene must be set");
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Player")
            {
                StartCoroutine(Transition());
            }
        }

        private IEnumerator Transition()
        {
            this.fader = GameObject.FindObjectOfType<Fader>();
            DontDestroyOnLoad(this.gameObject);
            yield return this.fader.FadeOut(fadeOutTime);

            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            if (savingWrapper != null)
            {
                savingWrapper.Save();
            }
                
            // Save current level

            yield return SceneManager.LoadSceneAsync(sceneToLoad);

            // Load current level

            if (savingWrapper != null)
            {
                savingWrapper.Load();
            }

            Portal otherPortal = GetOtherPortal();
            UpdatePlayer(otherPortal);

            if (savingWrapper != null)
            {
                savingWrapper.Save();
            }

            //yield return new WaitForSeconds(fadeWaitTime);
            yield return this.fader.FadeIn(fadeInTime);
            Destroy(gameObject);
        }

        private void UpdatePlayer(Portal otherPortal)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            player.GetComponent<NavMeshAgent>().enabled = false;
            player.transform.position = otherPortal.spawnPoint.position;
            player.transform.rotation = otherPortal.spawnPoint.rotation;
            player.GetComponent<NavMeshAgent>().enabled = true;
        }

        private Portal GetOtherPortal()
        {
            Portal[] portals = FindObjectsOfType<Portal>();
            foreach (Portal portal in portals)
            {
                if (portal != this && portal.destination == destination)
                {
                    return portal;
                }
            }

            return null;
        }
    }
}
