using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Manifiesto : MonoBehaviour {
    private void Update() {
        transform.position = new Vector3(Player.Instance.transform.position.x, Player.Instance.transform.position.y + 1.75f, Player.Instance.transform.position.z);
    }
}
