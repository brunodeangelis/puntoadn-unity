using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class MainMenu : MonoBehaviour {
    // Start is called before the first frame update
    void Start() {

    }

    // Update is called once per frame
    void Update() {

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
