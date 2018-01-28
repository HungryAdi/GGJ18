using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : PowerUser {
    public GameObject platform;
    public GameObject referencePoint;
    public Rigidbody2D rb;
    bool switchPressed;
    Vector3 origin;
    float timer;
    SpriteRenderer sr;
    Collider2D col;
    bool wasPressed;

    // Use this for initialization
    protected override void Start() {
        base.Start();
        origin = platform.transform.position;
        sr = platform.GetComponent<SpriteRenderer>();
        col = platform.GetComponent<Collider2D>();
        sr.enabled = false;
        col.enabled = false;
        timer = 0;
    }

    // Update is called once per frame
    void Update() {
        if (switchPressed && !wasPressed) {
            sr.enabled = true;
            col.enabled = true;
            //if (referencePoint.transform.position.y == platform.transform.position.y) {
            //    platform.transform.position += Vector3.right * Mathf.Sin(timer) * (referencePoint.transform.position.x - origin.x) * Time.deltaTime;
            //    rb.MovePosition(platform.transform.position + Vector3.right * Mathf.Sin(timer) * (referencePoint.transform.position.x - origin.x) * Time.deltaTime);
            //    timer += Time.deltaTime;
            //} else if (referencePoint.transform.position.x == platform.transform.position.x) {
            //    platform.transform.position += Vector3.up * Mathf.Sin(timer) * (referencePoint.transform.position.y - origin.y) * Time.deltaTime;
            //    rb.MovePosition(platform.transform.position += Vector3.up * Mathf.Sin(timer) * (referencePoint.transform.position.y - origin.y) * Time.deltaTime);
            //    timer += Time.deltaTime;
            //}
        }
        wasPressed = switchPressed;
    }

    public override void SetPower(bool isPowered) {
        if (isPowered) {
            switchPressed = true;
        }
    }
}
