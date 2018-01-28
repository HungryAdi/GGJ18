using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightbulbTest : PowerUser {

    SpriteRenderer sr;

	// Use this for initialization
	void Start () {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public override void SetPower(bool isPowered) {
        sr.color = isPowered ? Color.yellow : Color.black;
    }
}
