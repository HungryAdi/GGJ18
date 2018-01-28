using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightningFlash : MonoBehaviour {

    SpriteRenderer sr;
    float offset = 0.0f;

    // Use this for initialization
    void Start() {
        sr = GetComponent<SpriteRenderer>();
        offset = Random.value * 2.0f;
    }

    // Update is called once per frame
    void Update() {
        Color c = sr.color;
        c.a = Mathf.Abs(Mathf.Sin(Time.time * (3.0f + offset)));
        sr.color = c;
    }
}
