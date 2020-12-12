using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using DG.Tweening;

public class ReelScreen : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            VideoPlayer videoPlayer = GetComponent<VideoPlayer>();
            videoPlayer.enabled = true;
            videoPlayer.loopPointReached += OnReelEnded;

            foreach (var collider in GetComponents<BoxCollider>())
            {
                if (collider.isTrigger) Destroy(collider);
            }
        }
    }

    private void OnReelEnded(VideoPlayer source)
    {
        if (source.isPlaying) source.Stop();

        Sequence screenAnimations = DOTween.Sequence();
        screenAnimations.Append(transform.DOScaleX(0, 0.2f))
            .Append(transform.DOScaleY(0, 0.2f))
            .Join(transform.DOScaleZ(0, 0.2f))
            .OnComplete(() =>
            {
                Destroy(gameObject);
            });
    }
}
