using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class PlayerController : MonoBehaviour {
    public PlayerIndex index;
    public float armSpeed = 5.0f;


    [HideInInspector]
    public Rigidbody2D leftRigid;
    [HideInInspector]
    public HingeJoint2D leftHinge;
    [HideInInspector]
    public CircleCollider2D leftCol;

    [HideInInspector]
    public Rigidbody2D rightRigid;
    [HideInInspector]
    public HingeJoint2D rightHinge;
    [HideInInspector]
    public CircleCollider2D rightCol;

    [HideInInspector]
    public Collider2D[] overlapArr = new Collider2D[10];

    // Use this for initialization
    void Start() {
    }

    // Update is called once per frame
    void Update() {
        float lx = GamePad.GetState(index).ThumbSticks.Left.X;
        float ly = GamePad.GetState(index).ThumbSticks.Left.Y;

        float rx = GamePad.GetState(index).ThumbSticks.Right.X;
        float ry = GamePad.GetState(index).ThumbSticks.Right.Y;

        leftRigid.velocity = new Vector3(lx, ly) * armSpeed;
        rightRigid.velocity = new Vector3(rx, ry) * armSpeed;

        CheckWire(GamePad.GetState(index).Triggers.Left, leftHinge, leftCol);
        CheckWire(GamePad.GetState(index).Triggers.Right, rightHinge, rightCol);

    }

    void CheckWire(float triggerValue, HingeJoint2D connector, CircleCollider2D col) {
        if (triggerValue > .5f && !connector.enabled) { // if trigger down
            // find all nearby things on connect layer
            int count = Physics2D.OverlapCircleNonAlloc(col.transform.position, col.radius * col.transform.localScale.x, overlapArr, 1 << 8);

            // find closest object
            Collider2D closestOverlap = null;
            float distance = float.PositiveInfinity;
            for (int i = 0; i < count; i++) {
                if (overlapArr[i] == col)
                    continue;
                float sqr = Vector2.SqrMagnitude(overlapArr[i].transform.position - col.transform.position);
                if (sqr < distance) {
                    distance = sqr;
                    closestOverlap = overlapArr[i];
                }
            }

            // if found any object then connect to it
            if (closestOverlap) {
                connector.enabled = true;
                Rigidbody2D closestRb = closestOverlap.GetComponent<Rigidbody2D>();
                if (closestRb) {
                    connector.connectedBody = closestRb;
                } else {
                    connector.connectedAnchor = closestOverlap.transform.position;
                }
            }

        } else if (triggerValue <= .5f && connector.enabled) {   // else disconnect if trigger isnt down
            connector.connectedBody = null;
            connector.enabled = false;
        }
    }



}
