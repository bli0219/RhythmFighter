using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImage : MonoBehaviour {

    PlayerControl player;
    SpriteRenderer sr;
    float timer = 0.2f;

    void Awake() {
        player = PlayerControl.Instance;
        sr = GetComponent<SpriteRenderer>();
        sr.color = new Vector4(50, 50, 50, 0.2f);
    }
    void Start () {

	}

    void OnEnable() {
        if (timer == 0.2f) {
            return;
        }
        timer = 0.2f;
        player = PlayerControl.Instance;

        sr.sprite = player.sprite;
        transform.position = player.transform.position;
        transform.localScale = player.transform.localScale;
    }
    // Update is called once per frame
    void Update () {
        timer -= Time.deltaTime;
        if (timer <= 0) {
            gameObject.SetActive(false);
        }
	}
}
