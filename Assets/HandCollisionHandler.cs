using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCollisionHandler : MonoBehaviour
{
    [SerializeField]
    private GameObject hand;

    private BlinkOnCollision handBlink;

    private void Awake(){
        handBlink = hand.GetComponent<BlinkOnCollision>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        string colTag = collision.gameObject.tag;
        Debug.Log("[_DEBUG_] Collision with " + colTag);
        if (colTag == "Ball")
        {
            handBlink.CollisionBlink();
        }
    }
}
