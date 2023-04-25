using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTrigger : MonoBehaviour
{

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("colisionEnter");
        if (collision.gameObject.tag == "Hand")
        {
            Destroy(this.gameObject);
        }

    }
}
