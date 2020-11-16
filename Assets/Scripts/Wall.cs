using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wall : MonoBehaviour
{
    public void Up()
    {
        transform.DOScale(new Vector3(0.15f, 8f, transform.localScale.z), 0.2f);
    }

    public void Down()
    {
        transform.DOScale(new Vector3(0f, 0f, transform.localScale.z), 0.2f);
    }
}
