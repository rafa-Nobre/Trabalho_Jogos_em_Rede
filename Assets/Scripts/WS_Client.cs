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

    void Awake()
    {
        instance = this;
        GameObject[] objs = GameObject.FindGameObjectsWithTag("ws");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }
        DontDestroyOnLoad(this.gameObject);
    }
    void Start()
    {
        ws = new WebSocket("ws://aeugame2022.herokuapp.com");
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
                case "spawn":
                    //Debug.Log("Entrou no CheckSpawn!!!");
                    //checkSpawn = "spawn";
                break;
                case "ping":
                    Debug.Log("Recebeu ping do servidor!");
                break;
            }

        };
        ws.Connect();
    }

    // Update is called once per frame
    void Update()
    {
        if(SceneManager.GetActiveScene().name == "singlePlayer"){
            Destroy(this.gameObject);
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

    private void LateUpdate()
    {
       /*  if (SceneManager.GetActiveScene().name == "Fase1")
        {
            //ws.Send("posição: " + Player.instance.transform.position + ",rotação: " + Player.instance.transform.rotation);
               // ws.Send($@"{{'type':'posicao', 'data':'{Player.instance.transform.position}'}}");
               var jsonPayload = JsonConvert.SerializeObject(new
                    {
                        type = "Heartbeat",
                        data = "heartbeat"
                    });
                ws.Send(jsonPayload);
        } */
    }

    void FecharConexao()
    {
        ws.Close();
    }

}
