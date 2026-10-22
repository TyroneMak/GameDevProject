using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterFunctions : MonoBehaviour
{
    public float distance;
    public Transform Player;
    Vector3 direction;
    float speed = 5;
    bool isDetected = false;
    bool isStunned = false;
    int count = 0;
    Vector3 startPosition;
    public int leashDistance = 20;
    public int chaseThreshold = 25;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        // in Start
        startPosition = this.transform.position;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isStunned)
        {
            distance = Vector3.Distance(this.transform.position, Player.position);
            if(distance <= 10)
            {
                isDetected = true;
                count=0;
            }
            
            //This is the check for if the object is still within the test room
            if(isDetected && (Vector3.Distance(this.transform.position, startPosition)<=leashDistance))
            {
                count++;
                direction = (Player.position + this.transform.position).normalized;
                rb.AddForce ( direction * speed);
                if(count > chaseThreshold)
                {
                    isDetected = false;
                }
            }
            else
            {
                this.transform.position = startPosition;
                this.transform.rotation = Quaternion.identity;
                rb.velocity = Vector3.zero;
                isDetected = false;
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
    public void StunEffect()
    {
        rb.velocity = Vector3.zero;
        count = 0;
        isDetected = false;
        isStunned= true;
    }
}
