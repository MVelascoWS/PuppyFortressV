using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletProp : BaseProp {

    public float impulse;
    public bool firstHit;

    protected override void OnCollisionEnter(Collision collision)
    {
        if (firstHit)
        {
            if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "TrapTrigger")
            {
                Hide();
                // spawn particle?
            }
            else
            {
                StartCoroutine(WaitToHide(3));
                gameObject.tag = "Untagged";
            }            
            firstHit = false;
        }
    }

    public override void OnSpawn()
    {
        gameObject.tag = "Bullet";
        StartCoroutine(Shot());
    }

    public override void OnHide()
    {

    }

    IEnumerator Shot()
    {

        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        yield return new WaitForFixedUpdate();
        rb.AddForce(transform.forward * impulse, ForceMode.Impulse);
        firstHit = true;
    }

    IEnumerator WaitToHide(float t)
    {
        yield return new WaitForSeconds(t);
        Hide();
    }
}
