using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playercollision : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "enemy")
        {
            playercontroller.instance.react();
            Debug.Log("hit");
        }
    }
}
