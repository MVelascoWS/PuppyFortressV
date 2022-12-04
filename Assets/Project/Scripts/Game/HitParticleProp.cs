using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitParticleProp : BaseProp
{
    public float hideTime;
    protected Transform player;
    
    protected override void Awake()
    {
        base.Awake();
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    //protected override void FixedUpdate()
    //{
    //    transform.LookAt(player);
    //}

    private void Update()
    {
        transform.LookAt(player);
    }

    public override void OnSpawn()
    {
        Invoke("HideByInvoke", hideTime);
    }
    public override void OnHide()
    {

    }
    
    protected override void OnCollisionEnter(Collision collision)
    {

    }

    void HideByInvoke()
    {
        Hide();
    }
}