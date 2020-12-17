using UnityEngine;
using DG.Tweening;
using UnityEngine.Video;
using Cinemachine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ManifiestoTrigger : MonoBehaviour
{
    private GameObject _manifiestoSphere;

    private void Start() {
        _manifiestoSphere = GameManager._i._manifiestoSphere;
    }

    private void OnTriggerEnter(Collider other) {
        if (other.CompareTag("Player")) {
            GameObject.Find("Final Path/Walking Path").transform.DOScaleX(0f, 1f)
                    .OnComplete(() => {
                        GameManager._i._manifiestoSphere.SetActive(true);
                        GameManager._i._isManifiestoPlaying = true;

                        VideoPlayer manifiestoPlayer = _manifiestoSphere.GetComponent<VideoPlayer>();
                        manifiestoPlayer.loopPointReached += OnManifiestoEnded;
                        GameManager._i.ClearOutRenderTexture(manifiestoPlayer.targetTexture);

                        manifiestoPlayer.enabled = true;
                        GameObject.Find("vcam Player").GetComponent<CinemachineVirtualCamera>().m_Lens.FarClipPlane = 10f;
                        //Destroy(GameObject.Find("Visibility Sphere"));
                        Invoke("PauseBackgroundMusic", 0.5f);
                    });
        }
    }

    private void OnManifiestoEnded(VideoPlayer source) {
        GameManager._i._isManifiestoPlaying = true;

        GameManager._i._crosshair.GetComponent<Image>().DOFade(0f, 0.3f)
            .OnComplete(() => {
                SceneManager.LoadScene("MainMenu");
            }
        );
    }

    private void PauseBackgroundMusic() {
        GameManager._i.PauseBackgroundMusic();
    }
}
