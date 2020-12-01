using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class Label : MonoBehaviour {
    private GameObject _crosshair;
    private RectTransform _rectTransform;

    void Start() {
        _crosshair = GameManager.Instance._crosshair;
        _rectTransform = _crosshair.GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update() {

    }

    private void OnRaycastEnter() {
        DOTween.To(
            () => _rectTransform.sizeDelta,
            (x) => _rectTransform.sizeDelta = x,
            new Vector2(24f, 24f),
            0.15f
        );

        _crosshair.GetComponent<Image>().DOColor(new Color(0.53f, 0.34f, 0.63f), 0.15f);
    }

    private void OnRaycastExit() {
        DOTween.To(
            () => _rectTransform.sizeDelta,
            (x) => _rectTransform.sizeDelta = x,
            new Vector2(12f, 12f),
            0.15f
        );

        _crosshair.GetComponent<Image>().DOColor(Color.white, 0.15f);
    }
}
