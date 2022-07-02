using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AlienNest : MonoBehaviour
{
    bool aux = true;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.instance.itemscollected == 4 && aux == true){
            Player.instance.audiocontroller.NextDeath();
            aux = false;
            Destroy(gameObject,1f);
        }
    }
}
