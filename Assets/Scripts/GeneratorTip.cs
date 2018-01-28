using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratorTip : PowerUser {

    public SpriteRenderer sr;
    public Sprite OnSprite;
    public Sprite OffSprite;
    public bool powered = false;
    [HideInInspector]
    public bool freeze = false;

    // Use this for initialization
    protected override void Start() {
        base.Start();
    }

    // Update is called once per frame
    void Update() {

    }

    public override void SetPower(bool isPowered) {
        if (freeze) {
            return;
        }
        powered = isPowered;
        sr.sprite = isPowered ? OnSprite : OffSprite;
    }
}
