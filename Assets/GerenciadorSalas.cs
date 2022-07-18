using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
public class GerenciadorSalas : MonoBehaviour
{
    public GameObject salaPanel, butaoSala;
    public InputField inputSala;
    bool novasala = false;
    string nomePartida = "sala";
    string qtdPlayers = "";
    // Start is called before the first frame update
    void Start()
    {
        WS_Client.instance.ws.OnMessage += (sender, e) =>
        {
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Data);
            string option = (string)data["type"];
            switch (option)
            {
                case "sala-criada":
                    Debug.Log("entrou em sala-criada!");
                    nomePartida = (string)data["nome"];
                    qtdPlayers = (string)data["qtdPlayers"];
                    novasala = true;
                    Debug.Log("saiu de sala-criada!");
                    break;
            }

        };
    }

    // Update is called once per frame
    void Update()
    {
         if(novasala){
            int x =  int.Parse(qtdPlayers);
            CriarSala(nomePartida,x);
            novasala = false;
         }
    }

    void CriarSala(string np, int qtdp){
        GameObject sala = Instantiate(butaoSala, salaPanel.transform);
        Text texto = sala.GetComponentInChildren<Text>();
        texto.text = np + " - " + qtdp.ToString() + "/4";
    }

    public void ButtonEnviarSala(){
        if(inputSala.text != "") {
                var jsonPayload = JsonConvert.SerializeObject(new
                {
                    type = "criar-sala",
                    nome = inputSala.text,
                    id = WS_Client.instance.idp,
                });
                WS_Client.instance.ws.Send(jsonPayload);
            
        }
    }
}
