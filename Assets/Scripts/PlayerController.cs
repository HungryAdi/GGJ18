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

    Wire wire;


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
            rb2d.mass = 150f;
        float lx = GamePad.GetState(index).ThumbSticks.Left.X;
        float ly = GamePad.GetState(index).ThumbSticks.Left.Y;

        float rx = GamePad.GetState(index).ThumbSticks.Right.X;
        float ry = GamePad.GetState(index).ThumbSticks.Right.Y;

        leftRigid.velocity = new Vector3(lx, ly) * armSpeed;
        rightRigid.velocity = new Vector3(rx, ry) * armSpeed;


        CheckWire(false, GamePad.GetState(index).Triggers.Left, leftHinge, leftCol);
        CheckWire(true, GamePad.GetState(index).Triggers.Right, rightHinge, rightCol);

    }
    void FixedUpdate() {
        if (wire.powered) {
            //rb2d.AddForce(Random.insideUnitCircle * Random.value * 50.0f);
            float POWER = 100.0f;
            if(wire.leftType == CType.SOURCE && wire.rightType == CType.USER || wire.rightType == CType.SOURCE && wire.leftType == CType.USER) {
                POWER *= 5.0f;
            }
            rb2d.velocity = Random.insideUnitCircle * Random.value * POWER;
        }

        SetRumbleTown();
    }

    float leftMotorTime = 0.0f;
    float rightMotorTime = 0.0f;

    void SetRumbleTown() {
        bool leftSource = wire.leftType == CType.SOURCE;
        bool leftPower = wire.leftType == CType.USER && wire.powered;
        bool rightSource = wire.rightType == CType.SOURCE;
        bool rightPower = wire.rightType == CType.USER && wire.powered;

        float leftMotor = 0.0f;
        float rightMotor = 0.0f;
        // take me to rumble town
        if (leftSource && leftPower) {
            leftMotor = 1.0f;
        } else if (leftSource || leftPower) {
            leftMotor = Mathf.Max(leftMotor, 0.2f);
        }

        if (leftSource && rightPower || rightSource && leftPower) {
            leftMotor = 1.0f;
            rightMotor = 1.0f;
        } else {
            if (leftSource || leftPower) {
                leftMotor = Mathf.Max(leftMotor, 0.2f);
            }
            if (rightSource || rightPower) {
                rightMotor = Mathf.Max(leftMotor, 0.2f);
            }
        }

        leftMotorTime -= Time.deltaTime;
        rightMotorTime -= Time.deltaTime;
        if (leftMotorTime > 0.0f) {
            leftMotor = Mathf.Max(leftMotor, 0.5f);
        }
        if (rightMotorTime > 0.0f) {
            rightMotor = Mathf.Max(rightMotor, 0.5f);
        }

        // yes daddy
        GamePad.SetVibration(index, leftMotor, rightMotor);
        //Debug.Log(leftMotor + " " + rightMotor);
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
                if (rightArm) {
                    rightMotorTime = 0.3f;
                } else {
                    leftMotorTime = 0.3f;
                }

                if (closestRb)
                    connector.connectedBody = closestRb;
                else
                    connector.connectedAnchor = closestOverlap.transform.position;
            }

        } else if (triggerValue <= .5f && connector.enabled) {   // else disconnect if trigger isnt down
            connector.connectedBody = null;
            connector.enabled = false;

            // lock it but dont pop it (to combat electricity)
            // this doesnt do shit because arms are all rigidbodies too so f this (saving for later) love john, godbless
            //if (wire.powered) {
            //    Debug.Log("depower");
            //    float mag = rb2d.velocity.magnitude;
            //    mag = Mathf.Min(mag, 1.0f);
            //    rb2d.velocity = rb2d.velocity.normalized * mag;
            //}

            wire.Disconnect(rightArm);

        }
    }

    IEnumerator Vibration(bool rightArm, float forTime) {
        float t = 0.0f;
        float lft = !rightArm ? 1.0f : 0.0f;
        float rght = rightArm ? 1.0f : 0.0f;
        while (t < forTime) {
            GamePad.SetVibration(index, lft, rght);
            t += Time.deltaTime;
            yield return null;
        }
    }

}
