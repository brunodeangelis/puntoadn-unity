using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using TMPro;

public class Label : MonoBehaviour {
    
    private TextMeshProUGUI _actionLabel;
    private GameObject _crosshair;
    private RectTransform _rectTransform;

    public string _text;
    
    void Start() {
        _actionLabel = GameManager._i._actionLabel;
        _crosshair = GameManager._i._crosshair;
        _rectTransform = _crosshair.GetComponent<RectTransform>();
    }

    private void OnRaycastEnter() {
        DOTween.To(
            () => _rectTransform.sizeDelta,
            (x) => _rectTransform.sizeDelta = x,
            new Vector2(24f, 24f),
            0.15f
        );

        _crosshair.GetComponent<Image>().DOColor(new Color(0.53f, 0.34f, 0.63f), 0.15f);

        _actionLabel.text = _text;
        _actionLabel.DOFade(1f, 0.2f);
    }

    private void OnRaycastExit() {
        DOTween.To(
            () => _rectTransform.sizeDelta,
            (x) => _rectTransform.sizeDelta = x,
            new Vector2(12f, 12f),
            0.15f
        );

        _crosshair.GetComponent<Image>().DOColor(Color.white, 0.15f);

        _actionLabel.DOFade(0f, 0.2f);
    }
}
