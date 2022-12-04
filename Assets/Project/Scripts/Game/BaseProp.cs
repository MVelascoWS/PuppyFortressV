using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public abstract class BaseProp : MonoBehaviour
{
    protected Rigidbody rb;
    
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    /*
    protected virtual void FixedUpdate()
    {
        //rb.velocity = SpawnController.instance.velocity * SpawnController.instance.speedFactor;
        if (GameManager.instance.currentState != GameState.PLAY)
        {
            rb.isKinematic = true;
        }
    }
    */
    private void OnEnable()
    {
        if(rb==null)
            rb = GetComponent<Rigidbody>();
        OnSpawn();
    }

    public virtual void Hide(bool returnActivated = false)
    {
        OnHide();
        ObjectPool.instance.ReturnPoolObject(gameObject, returnActivated);
    }

    public abstract void OnSpawn();
    public abstract void OnHide();
    
    protected abstract void OnCollisionEnter(Collision collision);

}