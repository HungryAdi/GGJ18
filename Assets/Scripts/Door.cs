using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Door : PowerUser {

    public SpriteRenderer sr;
    public Collider2D col;

    // Use this for initialization
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {

    }

    public override void SetPower(bool isPowered) {
        sr.enabled = !isPowered;
        col.enabled = !isPowered;
    }
}
