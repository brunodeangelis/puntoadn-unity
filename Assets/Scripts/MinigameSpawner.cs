using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameSpawner : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SpawnRandomMinigame();
        }
    }
    private void SpawnRandomMinigame()
    {
        List<GameObject> minigames = GameManager.Instance._minigames;

        int r = UnityEngine.Random.Range(0, minigames.Count);

        GameObject instancedMinigame = Instantiate(minigames[r]);
        instancedMinigame.transform.position = transform.GetChild(0).transform.position;
        instancedMinigame.transform.LookAt(Player.Instance.transform);

        if (instancedMinigame.GetComponent<DisableAtStart>() != null)
            instancedMinigame.SetActive(true);

        minigames.RemoveAt(r);

        GameManager.Instance._minigames = minigames;
        Destroy(gameObject);
    }
}
