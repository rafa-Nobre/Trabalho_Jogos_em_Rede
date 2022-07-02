using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audio : MonoBehaviour
{
    public AudioClip somsteps1;
    public AudioClip somsteps2;
    public AudioClip damage1;
    public AudioClip damage2;

    public AudioClip outofammo;

    public AudioClip pickupitem;

    public AudioClip alienNextDeath;

    public AudioClip alienMobDeath;
    // Start is called before the first frame update
    public AudioSource audioSource;
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
        //if(Player.instance.steps == true){
        //    Invoke("FX", 1);
        //}
    }
    public void FX(){
        int aleatorio = Random.Range(0,2); 
        switch (aleatorio)
        {
            case 0:
                audioSource.PlayOneShot(somsteps1, 0.3f);
                break;
            case 1:
                audioSource.PlayOneShot(somsteps2, 0.3f);
                break;
            default:
                break;
        }
        
    }
    public void DamagePlayer(){
        int aleatorio = Random.Range(0,2); 
        switch (aleatorio)
        {
            case 0:
                audioSource.PlayOneShot(damage1, 0.7f);
                break;
            case 1:
                audioSource.PlayOneShot(damage2, 0.7f);
                break;
            default:
                audioSource.PlayOneShot(damage2, 0.7f);
                break;
        }
    }

    public void OutofAmmo(){
        audioSource.PlayOneShot(outofammo, 0.7f);
    }

    public void PickUpItem(){
        audioSource.PlayOneShot(pickupitem, 0.7f);
    }

    public void NextDeath(){
        audioSource.PlayOneShot(alienNextDeath, 0.7f);
    }

    public void AlienDeathMob(){
        audioSource.PlayOneShot(alienMobDeath, 1f);
    }
}
