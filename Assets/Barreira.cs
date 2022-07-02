using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Barreira : MonoBehaviour
{
     private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            Player.instance.health = 0;
        }
    }
}
