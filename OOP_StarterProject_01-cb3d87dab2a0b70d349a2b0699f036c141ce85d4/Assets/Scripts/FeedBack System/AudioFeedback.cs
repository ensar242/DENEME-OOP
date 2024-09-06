using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace PA.FeedbackSystem
{
    public class AudioFeedback : MonoBehaviour
    {
        [SerializeField] private AudioSource source;
        [SerializeField] private AudioClip audioclip;

        private void Awake()
        {
            source = GetComponent<AudioSource>();   
        }

        private void Start()
        {
            source.PlayOneShot(audioclip);
            StartCoroutine(DestroyAfterFinishedPlaying());
        }

        private IEnumerator DestroyAfterFinishedPlaying()
        {
            yield return new WaitForSeconds(audioclip.length);
            Destroy(gameObject); 

        }
    }
}