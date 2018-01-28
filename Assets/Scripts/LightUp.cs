using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightUp : MonoBehaviour {
    public bool litFam = false;
    public Sprite unlit;
    public Sprite lit;
    private SpriteRenderer citySR;
    private bool checkLight = false;
	// Use this for initialization
	void Start () {
        citySR = GetComponent<SpriteRenderer>();
        citySR.sprite = unlit;
	}
	
	// Update is called once per frame
	void Update () {
		if(litFam && !checkLight)
        {
            checkLight = true;
            citySR.sprite = lit;
        }
        if(!litFam && checkLight)
        {
            checkLight = false;
            citySR.sprite = unlit;
        }
	}
}
