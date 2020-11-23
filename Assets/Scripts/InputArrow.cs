using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputArrow : MonoBehaviour
{
    private bool _isPlayerLooking;

    [SerializeField] private InputArrowType _arrowType;
    [SerializeField] private TextMeshProUGUI _input;

    public static event Action OnPointToClickableObject;
    public static event Action OnExitClickableObject;

    void Update()
    {
        if (_isPlayerLooking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                int inputValue;

                if (_input.text == "X")
                {
                    inputValue = 1;
                } else
                {
                    inputValue = int.Parse(_input.text);
                }

                if (_arrowType == InputArrowType.UP)
                {
                    inputValue++;
                    if (inputValue > 9) inputValue = 0;
                    _input.text = inputValue.ToString();
                } else
                {
                    inputValue--;
                    if (inputValue < 0) inputValue = 9;
                    _input.text = inputValue.ToString();
                }
            }
        }
    }

    private void OnRaycastEnter(GameObject sender)
    {
        if (sender.transform.tag == "Player") _isPlayerLooking = true;
        OnPointToClickableObject?.Invoke();
    }

    private void OnRaycastExit(GameObject sender)
    {
        if (sender.transform.tag == "Player") _isPlayerLooking = false;
        OnExitClickableObject?.Invoke();
    }
}

enum InputArrowType
{
    UP = 0,
    DOWN = 1
}