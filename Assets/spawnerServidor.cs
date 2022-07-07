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

    bool aux = true;
    public GameObject splayer;
    public GameObject playerprefeb;

    Vector3 posicaojogadores;

    float x = 0f;
    float y = 0f;
    float z = 0f;
    float ry = 0f;
    string idpartida = "";
    int anim = 0;

    string IDjogadores = "";

    string idprincipal = "";

    bool posicaoNovojogador = false;

    Dictionary<string, Player> jogadores;
    Dictionary<string, object> clientes;
    // Start is called before the first frame update
    void Start()
    {
        jogadores = new Dictionary<string, Player>();
        clientes = new Dictionary<string, object>();
        posicaojogadores = splayer.transform.position;

        var jsonPayload = JsonConvert.SerializeObject(new
        {
            type = "Fase",
            data = "1"
        });
        WS_Client.instance.ws.Send(jsonPayload);

        WS_Client.instance.ws.OnMessage += (sender, e) =>
        {
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Data);
            string option = (string)data["type"];
            switch (option)
            {
                case "spawn":
                    Debug.Log("Entrou no CheckSpawn Mobs!!!");
                    estadoSpawner = true;
                    //idprincipal = Player.instance.IDPlayer;
                    //jogadores[Player.instance.IDPlayer] = Player.instance; //===============================
                    break;
                case "spawn-player":
                    Debug.Log("Entrou no SpawnPlayer!!!");
                    spawnPlayer = true; //============================
                    IDjogadores = (string)data["id"];
                    idpartida = (string)data["idpart"];
                    clientes = data;
                    Debug.Log(data["objeto"]);
                    Player.instance.liberarposicao = true;
                    break;
                case "posicao-jogadores":
                    var data2 = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Data);
                    try
                    {
                        string ox = (string)data2["x"];
                        string oy = (string)data2["y"];
                        string oz = (string)data2["z"];
                        string idp = (string)data2["idPlayer"];
                        string idpart = (string)data2["idPartida"];
                        string roty = (string)data2["ry"];
                        string ani = (string)data2["anim"];
                        x = float.Parse(ox, System.Globalization.CultureInfo.InvariantCulture);
                        y = float.Parse(oy, System.Globalization.CultureInfo.InvariantCulture);
                        z = float.Parse(oz, System.Globalization.CultureInfo.InvariantCulture);
                        IDjogadores = idp;
                        idpartida = idpart;
                        ry = float.Parse(roty, System.Globalization.CultureInfo.InvariantCulture);
                        anim = int.Parse(ani);
                        //IDjogadores = (string)data["id"];
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("erro:" + ex);
                    }
                    liberarpos = true;
                    //Debug.Log("x"+x+"y"+y+"z"+z);
                    break;
                case "desconectou":
                    string desc = (string)data["id"];
                    jogadores[desc].destruir = true;
                    jogadores.Remove(desc);
                    Debug.Log("player:"+desc+" desconectou!");
                    break;
            }

        };
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.instance.idpronto && aux){
            idprincipal = Player.instance.IDPlayer;
            aux = false;
        }
        //Debug.Log("check:"+WS_Client.instance.checkSpawn+", S:"+s.activeSelf);
        if (estadoSpawner && s.activeSelf == false)
        {
            s.SetActive(true);
        }
        if (spawnPlayer)
        {
            //var objeto = clientes["objeto"];
            //var objeto = JsonConvert.DeserializeObject<Dictionary<string, object>>((string)clientes["objeto"]);
            var ob = JsonConvert.SerializeObject(clientes["objeto"]);
            var obb = JsonConvert.DeserializeObject<Dictionary<string, object>>(ob);
            Debug.Log(obb);
            foreach (var item in obb)
            {
                Debug.Log("item:"+item.Key+" id:"+idprincipal);
                if(!((item.Key).Equals(idprincipal)) && !jogadores.ContainsKey(item.Key)){
                try
                {
                    var rposition = new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), 0, UnityEngine.Random.Range(-10.0f, 10.0f));
                    jogadores.Add(item.Key, Instantiate(playerprefeb, splayer.transform.position + rposition, playerprefeb.transform.rotation).GetComponent<Player>());
                    jogadores[item.Key].playerAtual = item.Key;
                    jogadores[item.Key].IDpartida = idpartida;
                    posicaoNovojogador = true; 
                }
                catch
                {
                    Debug.Log("Erro em spwanPlayer: jogadores.Add()!");
                }
                }else{
                    Debug.Log("item contem Chave idprincipal ou chaves repetidas!");
                }
                //Debug.Log(item.Key);
            }
           
            spawnPlayer = false;
        }

        if (posicaoNovojogador && liberarpos)
        {
            /*  var datas = JsonConvert.DeserializeObject<Dictionary<string, object>>(dadosconfig);
             x = (float)datas["x"];
             y = (float)datas["y"];
             z = (float)datas["z"]; */
            posicaojogadores = new Vector3(x, y, z);
            jogadores[IDjogadores].transform.position = posicaojogadores;
            //Debug.Log(jogadores[IDjogadores].transform.position);
            //=======anim==========
            jogadores[IDjogadores].transform.eulerAngles = new Vector3(jogadores[IDjogadores].transform.localRotation.eulerAngles.x, ry, jogadores[IDjogadores].transform.localRotation.eulerAngles.z);

            //obj[numPlayers].transform.position = posicaojogadores;
        }

    }

}
