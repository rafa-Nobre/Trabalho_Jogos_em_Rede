using System.Collections;
using System.Collections.Generic;
using WebSocketSharp;
using UnityEngine;
using UnityEngine.SceneManagement;
using Newtonsoft.Json;

public class WS_Client : MonoBehaviour
{
    //public string checkSpawn;
    public static WS_Client instance;
    public WebSocket ws;

    bool definirpartida = false;
    bool definirIDplayer = false;

    public string idp = "";

    string idpart = "";

    void Awake()
    {
        instance = this;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ws");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);

        ws = new WebSocket("ws://aeugame2022.herokuapp.com");
        //ws = new WebSocket("ws://localhost:8080"); //testar localmente
        ws.OnMessage += (sender, e) =>
        {
            //Debug.Log("Mensagem recebida de " + ((WebSocket)sender).Url + ", Dado: " + e.Data);
            //JObject stuff = JObject.Parse(e.Data);
            //string dados = (string)stuff["data"];
            //Debug.Log("dadosRecebidoCliente: " + dados);
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Data);
            string option = (string)data["type"];
            //Debug.Log("opção recebida: "+option);
            switch (option)
            {
                case "idplayer":
                    Debug.Log("============IDplayerRecebido===========");
                    idp = (string)data["id"];
                    definirIDplayer = true;
                    Debug.Log((string)data["id"]);
                break;
                case "partida":
                    Debug.Log("============IDpartidaRecebido===========");
                    idpart = (string)data["partida"];
                    definirpartida = true;
                    Debug.Log((string)data["partida"]);
                break;
                case "spawn":
                    //Debug.Log("Entrou no CheckSpawn!!!");
                    //checkSpawn = "spawn";
                break;
                case "ping":
                    //Debug.Log("Recebeu ping do servidor!");
                break;
            }

        };
        ws.Connect();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "singlePlayer"){
            Destroy(this.gameObject);
        }

        if(SceneManager.GetActiveScene().name == "Fase1"){
        if(definirpartida){
            Player.instance.IDpartida = idpart;
            definirpartida = false;
        }
        if(definirIDplayer){
            Player.instance.IDPlayer = idp;
            Player.instance.idpronto = true;
            definirIDplayer = false;
        }
        }
        

        if (ws == null)
        {
            return;
        }
        if (Input.GetKeyDown(KeyCode.Space))
        {
            //ws.Send(@"{'type':'dados', 'data': 'olá do cliente'}");
            var jsonPayload = JsonConvert.SerializeObject(new
                    {
                        type = "Ping",
                    });
                ws.Send(jsonPayload);
        }
    }

    void FecharConexao()
    {
        ws.Close();
    }

}
