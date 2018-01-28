using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : PowerUser {

    public SpriteRenderer sr;
    public Collider2D col;
    bool open;

    // Use this for initialization
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {
        if (open) {
            sr.enabled = false;
            col.enabled = false;
        }
    }

    public override void SetPower(bool isPowered) {
        if (isPowered) {
            open = true;
        }
    }
}
