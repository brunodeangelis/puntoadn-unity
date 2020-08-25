using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public static void PlayAudioClip(AudioClip clip) {
        GameObject _soundGameObject = new GameObject("One Shot Sound");
        AudioSource _audioSource = _soundGameObject.AddComponent<AudioSource>();
        _audioSource.PlayOneShot(clip);

        Object.Destroy(_soundGameObject, clip.length);
    }
}
