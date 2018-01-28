using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {
    public float scale;
    public float smoothing;
    float cameraLastY;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        Vector3 target = transform.position + Vector3.up * (Camera.main.transform.position.y - cameraLastY) * scale;
        transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * smoothing);
        cameraLastY = Camera.main.transform.position.y;
	}
}
