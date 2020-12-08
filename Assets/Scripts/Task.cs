using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Task : MonoBehaviour
{
    private TextMeshProUGUI _tmPro;
    [HideInInspector] public string _text;

    private void Awake() {
        _tmPro = GetComponent<TextMeshProUGUI>();
        _tmPro.color = new Color(255, 255, 255, 0);
    }

    public void Enter() {
        _tmPro.text = _text;
        _tmPro.DOFade(1f, 0.6f);
        _tmPro.transform.DOMoveX(_tmPro.transform.position.x - 15f, 0.6f).From();
    }
}
