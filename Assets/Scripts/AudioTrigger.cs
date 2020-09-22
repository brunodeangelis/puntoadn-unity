using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioTrigger : MonoBehaviour
{
    //private AudioSource _audioSource;
    [SerializeField] private AudioClip _audioClip;

    private void Awake() {
        //_audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter(Collider other) {
        if (other.transform.CompareTag("Player")) {
            //if (_audioSource.isPlaying) return;
            //_audioSource.PlayOneShot(_audioClip);
            //Destroy(gameObject, _audioClip.length);
            SoundManager.PlayAudioClip(_audioClip);
            Destroy(gameObject);
        }
    }
}
