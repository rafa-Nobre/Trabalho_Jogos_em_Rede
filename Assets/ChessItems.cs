using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChessItems : MonoBehaviour
{
    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            this.gameObject.SetActive(false);
            Player.instance.AddLife(1);
            Player.instance.AddItem();
            Player.instance.audiocontroller.PickUpItem();
        }
    }
  
}
