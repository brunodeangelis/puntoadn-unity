using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class StationNameSetter : MonoBehaviour
{
    private TextMeshProUGUI _textElement;
    private CanvasGroup _group;
    private bool _isUIShowing;

    [SerializeField] private float _checkDistance = 11f;

    [HideInInspector] public string _nameForUI;

    private void Start() {
        _textElement = GameObject.Find("Current Station UI/Current Station Text").GetComponent<TextMeshProUGUI>();
        _group = _textElement.transform.parent.GetComponent<CanvasGroup>();
    }

    private void Update() {
        if (Vector3.Distance(transform.position, Player.Instance.transform.position) < _checkDistance) {
            if (!_isUIShowing) {
                _isUIShowing = true;

                _textElement.text = _nameForUI;
                _group.DOFade(1f, 0.2f);
            }
        } else {
            _group.DOFade(0f, 0.2f)
                .OnComplete(() => {
                    _isUIShowing = false;
                });
        }
    }
}
