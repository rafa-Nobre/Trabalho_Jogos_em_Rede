using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ammo : MonoBehaviour
{
    // Start is called before the first frame update

    private void OnTriggerEnter(Collider other) {
        if(other.gameObject.tag == "Player"){
            this.gameObject.SetActive(false);
            Player.instance.AddMunicao(10);
            Player.instance.audiocontroller.PickUpItem();
        }
    }
}
