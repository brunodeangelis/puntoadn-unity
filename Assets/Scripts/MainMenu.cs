using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour {

    private void Start() {
        GameObject.Find("UI Group").GetComponent<CanvasGroup>().DOFade(1f, 1.25f)
            .OnComplete(() => {
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            });
    }

    public void StartGame() {
        GameObject.Find("UI Group").GetComponent<CanvasGroup>()
            .DOFade(0, 0.3f)
            .OnComplete(() => {
                SceneManager.LoadScene("Game");
            });
    }

    public void ExitGame() {
        Application.Quit();
    }
}
