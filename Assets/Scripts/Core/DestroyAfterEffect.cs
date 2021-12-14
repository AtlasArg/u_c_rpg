using UnityEngine;

namespace RPG.Core
{
    public class DestroyAfterEffect : MonoBehaviour
    {
        private ParticleSystem myParticleSystem;

        // Start is called before the first frame update
        void Start()
        {
            myParticleSystem = GetComponent<ParticleSystem>();
        }

        // Update is called once per frame
        void Update()
        {
            if (myParticleSystem != null && myParticleSystem.IsAlive())
            {
                Destroy(gameObject);
            }
        }
    }
}

