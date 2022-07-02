using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
using WebSocketSharp;

public class spawnerServidor : MonoBehaviour
{
    public GameObject s; 
    bool estadoSpawner = false;
    bool spawnPlayer = false;

    bool liberarpos = false;
    public GameObject splayer;
    public GameObject playerprefeb;

    Vector3 posicaojogadores;

    int numPlayers = 0;

    float x = 0f;
    float y = 0f;
    float z = 0f;

    string dadosconfig = "";

    Player[] obj;
    bool posicaoNovojogador = false;
    // Start is called before the first frame update
    void Start()
    {
        obj = new Player[5];
        posicaojogadores = splayer.transform.position;

        var jsonPayload = JsonConvert.SerializeObject(new
                    {
                        type = "Fase",
                        data = "1"
                    });
        WS_Client.instance.ws.Send(jsonPayload);

        WS_Client.instance.ws.OnMessage += (sender, e) =>
        {
            //Debug.Log("Mensagem recebida de " + ((WebSocket)sender).Url + ", Dado: " + e.Data);
            //JObject stuff = JObject.Parse(e.Data);
            //string dados = (string)stuff["data"];
            //Debug.Log("dadosRecebidoCliente: " + dados);
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Data);
            string option = (string)data["type"];
            //dadosconfig = e.Data;
            //Debug.Log("opção recebida: "+option);
            switch (option)
            {
                case "spawn":
                    Debug.Log("Entrou no CheckSpawn Mobs!!!");
                    estadoSpawner = true;
                break;
                case "spawn-player":
                    Debug.Log("Entrou no SpawnPlayer!!!");
                    spawnPlayer = true;
                    Player.instance.liberarposicao = true;
                break;
                case "posicao-jogadores":
                    var data2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Data);
                    try{
                    string ox = (string)data2["x"];
                    string oy = (string)data2["y"];
                    string oz = (string)data2["z"];
                    x = float.Parse(ox, System.Globalization.CultureInfo.InvariantCulture);
                    y = float.Parse(oy, System.Globalization.CultureInfo.InvariantCulture);
                    z = float.Parse(oz, System.Globalization.CultureInfo.InvariantCulture);
                    }
                    catch(Exception ex){
                        Debug.Log("erro:"+ex);
                    }
                    liberarpos = true;
                    //Debug.Log("x"+x+"y"+y+"z"+z);
                break;
            }

        };
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log("check:"+WS_Client.instance.checkSpawn+", S:"+s.activeSelf);
         if(estadoSpawner && s.activeSelf==false){
            s.SetActive(true);
        }
        if(spawnPlayer){
            ++numPlayers;
            obj[numPlayers] = Instantiate(playerprefeb, splayer.transform.position, playerprefeb.transform.rotation).GetComponent<Player>();
            obj[numPlayers].playerNumber = numPlayers;
            spawnPlayer = false;
            posicaoNovojogador = true;
        }

        if(posicaoNovojogador && liberarpos){
           /*  var datas = JsonConvert.DeserializeObject<Dictionary<string, object>>(dadosconfig);
            x = (float)datas["x"];
            y = (float)datas["y"];
            z = (float)datas["z"]; */
            posicaojogadores = new Vector3(x,y,z); 
            //soluçao temporário -- somente para teste ==================================
            obj[numPlayers].transform.position = posicaojogadores;
        }
         
    }

}
