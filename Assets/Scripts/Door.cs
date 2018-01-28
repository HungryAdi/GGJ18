using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : PowerUser {

    public SpriteRenderer sr;
    public Collider2D col;
    public bool zoom;
    bool open;
    bool wasOpen;

    // Use this for initialization
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {
        if (open && !wasOpen) {
            sr.enabled = false;
            col.enabled = false;
            if (zoom) {
                StartCoroutine(Camera.main.GetComponent<camerastuff>().ZoomOutTime(5));
            }
        }
        wasOpen = open;
    }

    public override void SetPower(bool isPowered) {
        if (isPowered) {
            open = true;
        }
    }
}
