using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class TrapBehaviour : MonoBehaviour {

    public float trapReloadTime, trapActiveTime;
    public bool ready = true;
    public Object particlePrefab;
    public UnityEvent OnHit, OnReady, OnDesactivateTrap;

    private void Start()
    {
        ready = true;
        OnReady.Invoke();
        OnDesactivateTrap.Invoke();

        StartCoroutine(TrapControl_A());
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag=="Bullet" && ready)
        {
            GameObject temp = ObjectPool.instance.TakePoolObject(particlePrefab, true);
            temp.transform.position = collision.contacts[0].point;

            ready = false;
            OnHit.Invoke();
        }
    }

    IEnumerator TrapControl_A()
    {
        while (true)
        {            
            if (ready)
            {
                yield return null;
            }
            else
            {
                StartCoroutine(TrapControl_B());
                yield return new WaitForSeconds(trapReloadTime);
                ready = true;
                OnReady.Invoke();
            }
        }
    }

    IEnumerator TrapControl_B()
    {
        yield return new WaitForSeconds(trapActiveTime);
        OnDesactivateTrap.Invoke();
    }
}
