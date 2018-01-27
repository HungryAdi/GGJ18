using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class PlayerController : MonoBehaviour {
    public PlayerIndex index;
    [HideInInspector]
    public Rigidbody2D leftWire;
    [HideInInspector]
    public HingeJoint2D leftConnect;
    [HideInInspector]
    public Rigidbody2D rightWire;
    [HideInInspector]
    public HingeJoint2D rightConnect;
    [HideInInspector]
    public CircleCollider2D leftWireEnd;
    [HideInInspector]
    public CircleCollider2D rightWireEnd;

    public Collider2D[] overlapArr = new Collider2D[10];


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

        if (GamePad.GetState(index).Triggers.Left > .5f && !leftConnect.enabled)
        {

            int count = Physics2D.OverlapCircleNonAlloc(leftWire.transform.position, leftWireEnd.radius * leftWireEnd.transform.localScale.x, overlapArr,1<<8);
            Collider2D closestOverlap = null;
            float distance = float.PositiveInfinity;
            for (int i = 0; i < count; i++)
            {
                if(overlapArr[i] == leftWireEnd)
                    continue;
                float sqr = Vector2.SqrMagnitude(overlapArr[i].transform.position - leftWire.transform.position);
                if (sqr < distance)
                {
                    distance = sqr;
                    closestOverlap = overlapArr[i];
                }
            }
            if (closestOverlap)
            {
                leftConnect.enabled = true;
                Rigidbody2D rb2d = closestOverlap.GetComponent<Rigidbody2D>();
                if (rb2d)
                    leftConnect.connectedBody = rb2d;
                else
                    leftConnect.connectedAnchor = closestOverlap.transform.position;
            }
                
        }
        else if(GamePad.GetState(index).Triggers.Left <= .5f)
        {
            if (leftConnect.enabled)
            {
                leftConnect.connectedBody = null;
                leftConnect.enabled = false;
            }
        }

        if (GamePad.GetState(index).Triggers.Right > .5f && !rightConnect.enabled)
        {
        }

        //leftWire.AddForce(new Vector3(lx, ly)*armSpeed);
        //rightWire.AddForce(new Vector3(rx, ry)*armSpeed);

        //leftWire.transform.Translate(new Vector3(lx, ly) * Time.deltaTime);
        //rightWire.transform.Translate(new Vector3(rx, ry) * Time.deltaTime);

    }



}
