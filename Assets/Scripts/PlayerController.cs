using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XInputDotNetPure; // Required in C#

public class PlayerController : MonoBehaviour {
    public PlayerIndex index;
    public float armSpeed;


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

    private Rigidbody2D rb2d;

    public Wire wire;


    // Use this for initialization
    void Start() {
        wire = GetComponent<Wire>();
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (wire.connectedToWall)
            rb2d.mass = .5f;
        else
            rb2d.mass = 100f;
        float lx = GamePad.GetState(index).ThumbSticks.Left.X;
        float ly = GamePad.GetState(index).ThumbSticks.Left.Y;

        float rx = GamePad.GetState(index).ThumbSticks.Right.X;
        float ry = GamePad.GetState(index).ThumbSticks.Right.Y;

        leftRigid.velocity = new Vector3(lx, ly) * armSpeed;
        rightRigid.velocity = new Vector3(rx, ry) * armSpeed;

        if (wire.connectedToWall)
        {
            leftRigid.velocity *= 2;
            rightRigid.velocity *= 2;
        }


        CheckWire(false, GamePad.GetState(index).Triggers.Left, leftHinge, leftCol);
        CheckWire(true, GamePad.GetState(index).Triggers.Right, rightHinge, rightCol);
    }

    void CheckWire(bool rightArm, float triggerValue, HingeJoint2D connector, CircleCollider2D col) {
        if (triggerValue > .5f && !connector.enabled) { // if trigger down
            // find all nearby things on connect layer
            int count = Physics2D.OverlapCircleNonAlloc(col.transform.position, col.radius * col.transform.localScale.x, overlapArr, 1 << 8);

            // find closest object
            Collider2D closestOverlap = null;
            float distance = float.PositiveInfinity;
            for (int i = 0; i < count; i++) {
                if (overlapArr[i] == rightCol || overlapArr[i] == leftCol)
                    continue;
                float sqr = Vector2.SqrMagnitude(overlapArr[i].transform.position - col.transform.position);
                if (sqr < distance) {
                    distance = sqr;
                    closestOverlap = overlapArr[i];
                }
            }

            // if found any object
            if (closestOverlap) {
                connector.enabled = true;
                Rigidbody2D closestRb = closestOverlap.GetComponent<Rigidbody2D>();

                wire.Connect(rightArm, closestOverlap);

                if (closestRb)
                    connector.connectedBody = closestRb;
                else
                    connector.connectedAnchor = closestOverlap.transform.position;
            }

        } else if (triggerValue <= .5f && connector.enabled) {   // else disconnect if trigger isnt down
            connector.useMotor = false;
            connector.connectedBody = null;
            connector.enabled = false;

            wire.Disconnect(rightArm);
        }
    }

}
