using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(LineRenderer))]
[RequireComponent(typeof(PlayerController))]
public class JointBuilder : MonoBehaviour {

    public GameObject jointPrefab;
    public GameObject particlesPrefab;
    public float angle = 60.0f;
    
    Rigidbody2D rigid;
    PlayerController pc;
    LineRenderer lr;
    Rigidbody2D[] lrbs;
    Rigidbody2D[] rrbs;
    ParticleSystem lps;
    ParticleSystem rps;
    int count;
    // Use this for initialization
    void Start() {
        rigid = GetComponent<Rigidbody2D>();
        pc = GetComponent<PlayerController>();
        lr = GetComponent<LineRenderer>();
        count = 10;
        pc.leftRigid = BuildJoints(count, out pc.leftHinge, out pc.leftCol, out lrbs, out lps);
        pc.rightRigid = BuildJoints(count, out pc.rightHinge, out pc.rightCol, out rrbs, out rps);


    }

    Rigidbody2D BuildJoints(int count, out HingeJoint2D hingy, out CircleCollider2D colly, out Rigidbody2D[] rbs, out ParticleSystem ps) {
        rbs = new Rigidbody2D[count];
        for (int i = 0; i < count; ++i) {
            GameObject go = Instantiate(jointPrefab, transform.position + new Vector3(i * .5f, 0, 0), Quaternion.identity, transform);
            go.name = "Joint" + i;
            HingeJoint2D joint = go.GetComponent<HingeJoint2D>();
            Rigidbody2D rb = go.GetComponent<Rigidbody2D>();
            rbs[i] = rb;
            if (i == 0) {
                joint.connectedBody = rigid;
            } else {
                joint.limits = new JointAngleLimits2D { min = -angle, max = angle };
                joint.connectedBody = rbs[i - 1];
            }

            if (i == count - 1) {
                CircleCollider2D coll;
                coll = go.AddComponent<CircleCollider2D>();
                coll.isTrigger = true;
                GameObject particles = Instantiate(particlesPrefab, go.transform.position, Quaternion.identity);
                ps = particles.GetComponent<ParticleSystem>();
                particles.transform.SetParent(go.transform);
                go.layer = 8;
                HingeJoint2D hj = go.AddComponent<HingeJoint2D>();
                go.tag = "Player";
                //go.AddComponent<Hinge>();
                hj.enabled = false;
                hingy = hj;
                colly = coll;
                return rb;
            }
        }
        hingy = null;
        colly = null;
        ps = null;
        return null;
    }
    void RenderLine(int count) {
        lr.positionCount = count * 2 - 1;
        for (int i = lrbs.Length - 1; i > 0; --i) {
            lr.SetPosition(lrbs.Length - i - 1, lrbs[i].transform.position);
        }
        for (int j = 0; j < rrbs.Length; ++j) {
            lr.SetPosition(j + lrbs.Length - 1, rrbs[j].transform.position);
        }

    }
    // Update is called once per frame
    void Update() {
        RenderLine(count);
        if (pc.wire.connectedToWall) {
            lps.Play();
            rps.Play();
        } else {
            lps.Stop();
            rps.Stop();
        }
    }
}
