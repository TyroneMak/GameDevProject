using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFunctions : MonoBehaviour
{
    public float distance;
    public Transform Player;
    public Vector3 direction;
    public float speed = 5;
    public bool isDetected = false;
    public bool isStunned = false;
    public int count = 0;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isStunned)
        {
            distance = Vector3.Distance(this.transform.position, Player.position);
            if(distance < 10)
            {
                isDetected = true;
                count=0;
            }
            

            if(isDetected)
            {
                count++;
                direction = (Player.position + this.transform.position).normalized;
                rb.AddForce ( direction * speed);
                if(count > 100)
                {
                    isDetected = false;
                }
            }
            else
            {

            }
        }
        else
        {
            count++;
            if(count > 10)
            {
                isStunned = false;
            }
        }
    }
    void StunEffect()
    {
        rb.velocity = Vector3.zero;
        count = 0;
        isDetected = false;
        isStunned= true;
    }
}
