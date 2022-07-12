using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBOT : MonoBehaviour
{
    public static PlayerBOT instance;
    public bool destruir = false;
    public string playerAtual = "";

    public string IDpartida = "";

    public bool vivo = true;
    void Awake()
    {
        instance = this;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(destruir){
            Destroy(this.gameObject);
        }

       /*  if(!vivo){
            RestartPlayerBot();
        } */

    }

    /* void RestartPlayerBot(){
       var rposition = new Vector3(UnityEngine.Random.Range(-3.0f, 3.0f), 0, UnityEngine.Random.Range(-3.0f, 3.0f));
       transform.position = new Vector3(456.38f, 2.17f, 518.71f) + rposition;
       vivo = true;
    } */
}
