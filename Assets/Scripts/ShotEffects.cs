using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotEffects : MonoBehaviour {

    public Transform muzzleFlashPrefab;
    Transform shotSpawn;
    ShotEffects shootEffects;
    //public Transform sheelPrefab;

    public void Shell(Vector3 position, Quaternion rotation) {
        //Transform obj = Instantiate(sheelPrefab, position, rotation);
        //Destroy(obj.gameObject, 3f);
    }

    public void MuzzleFlash(Vector3 position, Quaternion rotation) {
        Transform obj = Instantiate(muzzleFlashPrefab, position, rotation);
        obj.localEulerAngles = new Vector3(obj.localEulerAngles.x + Random.Range(-360, 360),
                                           obj.localEulerAngles.y-110,
                                           obj.localEulerAngles.z);

        //obj.localScale = Vector3.one * 1.2f;

        Destroy(obj.gameObject, 0.1f);
    }

    private void Awake() {
        shotSpawn = transform.Find("shotSpawn");
        shootEffects = GetComponent<ShotEffects>();
    }

    void Update()
    {
        if(Input.GetKey(KeyCode.Mouse1)){
            //mira on
            if(Input.GetKeyDown(KeyCode.Mouse0)){
                //implementação do tiro
                if(Player.instance.health>0 && Player.instance.municao >0) shootEffects.MuzzleFlash(shotSpawn.position, shotSpawn.rotation);
            }
        }
    }


}
