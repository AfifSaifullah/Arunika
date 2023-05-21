using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : Musuh
{
    [SerializeField] private Rigidbody2D myRig;
    [SerializeField] private Collider2D myCold;
    [SerializeField] private Collider2D myTrig;
    [SerializeField] private Transform PlayerP;
    private bool facingRight;
    private float health;
    private float attackDamage;
    public float speed;
    
    


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Attacked(float damage)
    {
        
    }

    
}
