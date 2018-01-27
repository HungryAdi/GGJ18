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
    public List<GameObject> connectedObjects;
    [HideInInspector]
    public Collider2D[] overlapArr = new Collider2D[10];

    private Rigidbody2D rb2d;
    


    // Use this for initialization
    void Start() {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        if (connectedObjects.Count > 0 && CheckConnectedToNonPlayer())
            rb2d.mass = .5f;
        else
            rb2d.mass = 200f;
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
                //connector.GetComponent<Hinge>().connected = true;
                connector.enabled = true;
                PlayerController pc = null;
                Rigidbody2D closestRb = null;
                connectedObjects.Add(closestOverlap.gameObject);
                if (closestOverlap.CompareTag("Player"))
                {
                    pc = closestOverlap.transform.parent.GetComponent<PlayerController>();
                    pc.connectedObjects.Add(connector.gameObject);
                    closestRb = closestOverlap.GetComponent<Rigidbody2D>();
                }
                if (closestRb)
                    connector.connectedBody = closestRb;
                else
                    connector.connectedAnchor = closestOverlap.transform.position;
            }

        } else if (triggerValue <= .5f && connector.enabled) {   // else disconnect if trigger isnt down
            if (connector.connectedBody)
                connectedObjects.Remove(connector.connectedBody.gameObject);
            connector.connectedBody = null;
            connector.enabled = false;

        }
    }

    bool CheckConnectedToNonPlayer()
    {
        foreach(GameObject g in connectedObjects)
        {
            if (!g.CompareTag("Player"))
                return true;
        }
        return false;
    }

}
