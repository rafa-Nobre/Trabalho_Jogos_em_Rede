using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gun : MonoBehaviour
{
    FisrtPersonCamera fpsCam;
    AudioSource audioSource;

    ShotEffects shootEffects;
    Transform shotSpawn;

    //public audio audiocontroller;
    // Start is called before the first frame update
    
    private void Awake() {
        fpsCam = transform.parent.GetComponent<FisrtPersonCamera>();
        audioSource = transform.GetComponent<AudioSource>();
        //shotSpawn = transform.Find("shotSpawn");
        //shootEffects = GetComponent<ShotEffects>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.instance.health >0){
            if(Input.GetKey(KeyCode.Mouse1)){
                //mira on
                mira.instance.miraON();
                if(Input.GetKeyDown(KeyCode.Mouse0) && Player.instance.municao >0){
                    //implementação do tiro
                    ShootRaycast();
                    audioSource.Play();
                    Player.instance.SubMunicao(1);
                    //shootEffects.MuzzleFlash(shotSpawn.position, shotSpawn.rotation);
                }
                if(Input.GetKeyDown(KeyCode.Mouse0) && Player.instance.municao <=0){
                    Player.instance.audiocontroller.OutofAmmo();
                }
            }
            else{
                //mira off
                mira.instance.miraOFF();
            }
        }
    }

    void ShootRaycast(){
        RaycastHit hitInfo;
        //Physics.Raycast(position, direction, hitInfo, length, layer)
        //position - posição de onde sai o raio
        //direction - direção para qual o raio vai
        //hitInfo - struct que armazena as informaçoes do objeto atingido
        //length - comprimento do raio
        //layer - layer para qual o raio vai ser atingido, ex:Parede, inimigos, npc's ...
        if(Physics.Raycast(fpsCam.transform.position, fpsCam.GetForwardDirection(), out hitInfo, Mathf.Infinity, LayerMask.GetMask("hittable"))){
            IShotHit hitted = hitInfo.transform.GetComponent<IShotHit>();
            if(hitted != null){
                hitted.Hit(fpsCam.GetForwardDirection());
            }
        }
    }
}
