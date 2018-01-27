using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PlayerController))]
public class JointBuilder : MonoBehaviour {

    public GameObject jointPrefab;

    Rigidbody2D rigid;
    PlayerController pc;
    LineRenderer lr;
    Rigidbody2D[] lrbs;
    Rigidbody2D[] rrbs;
    int count;
    // Use this for initialization
    void Start() {
        rigid = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerController>();
        lr = GetComponent<LineRenderer>();
        count = 10;
        pc.leftWire = BuildJoints(count, out pc.leftConnect, out pc.leftWireEnd, out lrbs);
        pc.rightWire = BuildJoints(count, out pc.rightConnect, out pc.rightWireEnd, out rrbs);


    }

    Rigidbody2D BuildJoints(int count, out HingeJoint2D hingy, out CircleCollider2D colly, out Rigidbody2D[] rbs) {
        rbs = new Rigidbody2D[count];
        for (int i = 0; i < count; ++i) {
            GameObject go = Instantiate(jointPrefab, transform.position + new Vector3(i * 0.5f, 0, 0), Quaternion.identity, transform);
            go.name = "Joint" + i;
            HingeJoint2D joint = go.GetComponent<HingeJoint2D>();
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            rbs[i] = rb;
            if (i == 0) {
                joint.connectedBody = rigid;
            } else {
                joint.limits = new JointAngleLimits2D { min = -90, max = 90 };
                joint.connectedBody = rbs[i - 1];
            }

            if (i == count - 1) {
                CircleCollider2D coll;
                coll = go.AddComponent<CircleCollider2D>();
                coll.isTrigger = true;
                go.layer = 8;
                HingeJoint2D hj = go.AddComponent<HingeJoint2D>();
                hj.enabled = false;
                hingy = hj;
                colly = coll;
                return rb;
            }
        }
        hingy = null;
        colly = null;
        return null;
    }
    void RenderLine(int count) {
        lr.positionCount = count * 2;
        for(int i = lrbs.Length - 1; i >= 0; --i) {
            lr.SetPosition(lrbs.Length - i - 1, lrbs[i].transform.position);
        }
        for(int j = lrbs.Length; j < lrbs.Length + rrbs.Length; ++j) {
            lr.SetPosition(j, rrbs[j - lrbs.Length].transform.position);
        }
    }
    // Update is called once per frame
    void Update() {
        RenderLine(count);
    }
}
