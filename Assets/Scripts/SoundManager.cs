using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public static void PlayAudioClip(AudioClip clip) {
        GameObject soundGameObject = new GameObject("One Shot Sound");
        AudioSource audioSource = soundGameObject.AddComponent<AudioSource>();
        audioSource.PlayOneShot(clip);

        Object.Destroy(soundGameObject, clip.length);
    }
}
