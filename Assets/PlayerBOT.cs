using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBOT : MonoBehaviour
{
    public static PlayerBOT instance;
    public bool destruir = false;
    public string playerAtual = "";

    public string IDpartida = "";
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
    }
}
