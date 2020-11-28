using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;

public class PlayReelTrigger : MonoBehaviour
{
    [SerializeField] private GameObject _reelScreen;
    //public static event Action OnPlayReel;

    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.CompareTag("Player"))
    //    {
    //        OnPlayReel?.Invoke();
    //        Destroy(gameObject);
    //    }
    //}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VideoPlayer videoPlayer = _reelScreen.GetComponent<VideoPlayer>();
            videoPlayer.enabled = true;
            videoPlayer.loopPointReached += OnReelEnded;

            Destroy(gameObject);
        }
    }

    private void OnReelEnded(VideoPlayer source)
    {
        if (source.isPlaying) source.Stop();
    }
}
