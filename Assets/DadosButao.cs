using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class DadosButao : MonoBehaviour
{
    public int quantidadeJogadores = 0;
    public string nomeSala = "";

    Button  butao;
    // Start is called before the first frame update
    void Start()
    {
        butao.GetComponent<Button>().onClick.AddListener(delegate { btnClicked(); });
    }

    // Update is called once per frame
    void Update()
    {
        
    }
       
    public void btnClicked()
    {
        DadosButao dadosDaSala =  this.gameObject.GetComponentInChildren<DadosButao>();
         if(dadosDaSala.quantidadeJogadores < 4){
        //incrementar o tamanho de players na sala
        var jsonPayload = JsonConvert.SerializeObject(new
                {
                    type = "entrar-na-sala",
                    id = WS_Client.instance.idp,
                    sala = dadosDaSala.nomeSala,
                });
                WS_Client.instance.ws.Send(jsonPayload);
        //entrar na fase multiplayer
        //SceneManager.LoadScene(fase);
        } 
        else{
            Debug.Log("sala cheia!");
        }
    
    }
}
