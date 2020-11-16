using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InputArrow : MonoBehaviour
{
    private bool _isPlayerLooking;

    [SerializeField] private ArrowType _arrowType;
    [SerializeField] private TextMeshProUGUI _input;

    void Start()
    {
    }

    void Update()
    {
        if (_isPlayerLooking)
        {
            if (Input.GetMouseButtonDown(0))
            {
                int inputValue = int.Parse(_input.text);

                if (_arrowType == ArrowType.UP)
                {
                    if (inputValue >= 9) return;
                    _input.text = (inputValue + 1).ToString();
                } else
                {
                    if (inputValue <= 0) return;
                    _input.text = (inputValue - 1).ToString();
                }
            }
        }
    }

    private void OnRaycastEnter(GameObject sender)
    {
        if (sender.transform.tag == "Player") _isPlayerLooking = true;
    }

    private void OnRaycastExit(GameObject sender)
    {
        if (sender.transform.tag == "Player") _isPlayerLooking = false;
    }
}

enum ArrowType
{
    UP,
    DOWN
}