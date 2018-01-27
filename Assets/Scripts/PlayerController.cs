using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class PlayerController : MonoBehaviour {
    public PlayerIndex index;
    [HideInInspector]
    public Rigidbody2D leftWire;
    [HideInInspector]
    public Rigidbody2D rightWire;

    public float armSpeed = 5.0f;

    // Use this for initialization
    void Start() {

    }

    // Update is called once per frame
    void Update() {
        float lx = GamePad.GetState(index).ThumbSticks.Left.X;
        float ly = GamePad.GetState(index).ThumbSticks.Left.Y;

        float rx = GamePad.GetState(index).ThumbSticks.Right.X;
        float ry = GamePad.GetState(index).ThumbSticks.Right.Y;

        leftWire.velocity = new Vector3(lx, ly) * armSpeed;
        rightWire.velocity = new Vector3(rx, ry) * armSpeed;

        //leftWire.AddForce(new Vector3(lx, ly)*armSpeed);
        //rightWire.AddForce(new Vector3(rx, ry)*armSpeed);

        //leftWire.transform.Translate(new Vector3(lx, ly) * Time.deltaTime);
        //rightWire.transform.Translate(new Vector3(rx, ry) * Time.deltaTime);

    }



}
