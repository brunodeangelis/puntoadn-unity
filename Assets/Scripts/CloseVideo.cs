using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CloseVideo : MonoBehaviour {
    public static event Action OnCloseVideo;

    public void EndVideo() {
        if (GameManager.Instance._isVideoPlaying) {
            OnCloseVideo?.Invoke();
            GetComponent<Text>().DOFade(0f, 0.3f);
        }
    }
}
