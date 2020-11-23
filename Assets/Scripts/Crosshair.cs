using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Crosshair : MonoBehaviour
{
    private void Start()
    {
        InputArrow.OnPointToClickableObject += Grow;
        InputArrow.OnExitClickableObject += Shrink;
    }

    private void OnDestroy()
    {
        InputArrow.OnPointToClickableObject -= Grow;
        InputArrow.OnExitClickableObject -= Shrink;
    }

    public void Grow()
    {
    }

    public void Shrink()
    {

    }
}
