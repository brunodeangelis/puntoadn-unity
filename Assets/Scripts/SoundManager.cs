using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SoundManager
{
    public enum Sound {
        Success,
        Error
    }

    private static AudioSource _oneShotAudioSource;
    private static GameObject _oneShotGameObject;

    public static void PlaySound(Sound sound) {
        if (_oneShotGameObject == null) {
            _oneShotGameObject = new GameObject("One Shot Sound");
            _oneShotAudioSource = _oneShotGameObject.AddComponent<AudioSource>();
        }

        _oneShotAudioSource.PlayOneShot(GetAudioClip(sound));
        //Object.Destroy(soundGameObject, clip.length);
    }
    
    public static void PlaySound(AudioClip audioClip) {
        if (_oneShotGameObject == null) {
            _oneShotGameObject = new GameObject("One Shot Sound");
            _oneShotAudioSource = _oneShotGameObject.AddComponent<AudioSource>();
        }

        _oneShotAudioSource.PlayOneShot(audioClip);
        //Object.Destroy(soundGameObject, clip.length);
    }

    public static AudioClip GetAudioClip(Sound sound) {
        foreach(GameManager.SoundAudioClip soundAudioClip in GameManager._i._soundAudioClipArray) {  
            if (soundAudioClip._sound == sound) {
                return soundAudioClip._audioClip;
            }
        }

        Debug.LogError($"Sound: {sound} not found!");
        return null;
    }
}
