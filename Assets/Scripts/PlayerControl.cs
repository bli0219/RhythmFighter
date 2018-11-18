using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerControl : MonoBehaviour {

    public static PlayerControl Instance;
    public Sprite sprite;
    public GameObject imagePrefab;
    List<GameObject> imagePool;
    float timer = 0f;
    Rigidbody2D rb;
    Animator anim;
    bool dodging = true;
    bool moving = false;

    public delegate void Attack();
    public static event Attack DashAttack;

    void Awake() {
        Instance = this;
        imagePool = new List<GameObject>();
        sprite = GetComponent<SpriteRenderer>().sprite;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetKeyDown(KeyCode.Space)) {
            if (dodging) {
                Dash();
            } else {
                Dodge();
            }
        }
        if (Time.time > timer + 0.05f && rb.velocity != Vector2.zero) {
            timer = Time.time;
            CreateAfterImage();
        }

    }

    void Dodge() {
        anim.SetTrigger("Dodge");
    }

    void Dash() {
        anim.SetTrigger("Dash");
        DashAttack();
    }

    public void EnemyAttack() {
        if (!dodging) {
            anim.SetTrigger("Attacked");
        }
    }

    public void BeCountered() {

    }

    public void BeAttacked() {
        if (!dodging) {
            //do something

        }
    }

    public void CreateAfterImage() {
        Debug.Log("called");
        bool found = false;
        foreach (GameObject i in imagePool) {
            if (!i.activeSelf) {
                i.SetActive(true);
                found = true;
                break;
            }
        }
        if (!found) {
            GameObject image = Instantiate(imagePrefab, transform.position, transform.rotation);
            imagePool.Add(image);
        }

    }
}