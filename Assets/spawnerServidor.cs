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

    bool vivo = true;
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

    bool desconectou = false;

    string desconectouID = "";

    Dictionary<string, PlayerBOT> jogadores;
    Dictionary<string, object> clientes;
    // Start is called before the first frame update
    void Start()
    {
        jogadores = new Dictionary<string, PlayerBOT>();
        clientes = new Dictionary<string, object>();
        posicaojogadores = splayer.transform.position;

        StartCoroutine(EntrouFase1());

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
                    try{
                    Debug.Log("Entrou no SpawnPlayer!!!");
                    spawnPlayer = true; //============================
                    IDjogadores = (string)data["pid"];
                    clientes = data;
                    Debug.Log(data["objeto"]);
                    Player.instance.liberarposicao = true;
                    }catch{Debug.Log("Sem acesso a chaves [objeto]! em case:spawn-player");}
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
                        string v = (string)data2["vivo"];
                        x = float.Parse(ox, System.Globalization.CultureInfo.InvariantCulture);
                        y = float.Parse(oy, System.Globalization.CultureInfo.InvariantCulture);
                        z = float.Parse(oz, System.Globalization.CultureInfo.InvariantCulture);
                        IDjogadores = idp;
                        idpartida = idpart;
                        ry = float.Parse(roty, System.Globalization.CultureInfo.InvariantCulture);
                        anim = int.Parse(ani);
                        vivo = bool.Parse(v);
                        //IDjogadores = (string)data["id"];
                    }
                    catch (Exception ex)
                    {
                        Debug.Log("erro em posicao-jogadores em spawnerServidor:" + ex);
                    }
                    liberarpos = true;
                    //Debug.Log("x"+x+"y"+y+"z"+z);
                    break;
                case "desconectou":
                    desconectouID = (string)data["id"];
                    desconectou = true;
                    Debug.Log("player:"+desconectouID+" desconectou!");
                    break;
            }

        };
    }

    // Update is called once per frame
    void Update()
    {
        if(desconectou){
            try{
            jogadores[desconectouID].destruir = true;
            jogadores.Remove(desconectouID);
            }catch{
                Debug.Log("erro na remoção do player!!");
            }
            desconectou = false;
        }
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
            try{
            //var objeto = clientes["objeto"];
            //var objeto = JsonConvert.DeserializeObject<Dictionary<string, object>>((string)clientes["objeto"]);
            var ob = JsonConvert.SerializeObject(clientes["objeto"]);
            Debug.Log("antesdoReplace:"+ob);
            var replacement = ob.Replace("null", "\"-\"");
            Debug.Log("DepoisdoReplace:"+replacement);
            //var obb = JsonConvert.DeserializeObject<Dictionary<string, object>>(ob);
            var obb = JsonConvert.DeserializeObject<MeuObjeto>(replacement);
            Debug.Log("obb:"+obb);
            foreach (var item in obb.players)
            {   
                if(item.Equals("") || item.Equals("null") || item == null || item.Equals("-")){Debug.Log("item vazio! continuando.."); continue;}
                //Debug.Log("item:"+item.Key+" id:"+idprincipal);
                Debug.Log("item:"+item);
                //if(!((item.Key).Equals(idprincipal)) && !jogadores.ContainsKey(item.Key)){
                if(!((item).Equals(idprincipal)) && !jogadores.ContainsKey(item)){
                try
                {
                    var rposition = new Vector3(UnityEngine.Random.Range(-10.0f, 10.0f), 0, UnityEngine.Random.Range(-10.0f, 10.0f));
                    //jogadores.Add(item.Key, Instantiate(playerprefeb, splayer.transform.position + rposition, playerprefeb.transform.rotation).GetComponent<PlayerBOT>());
                    //jogadores[item.Key].playerAtual = item.Key;
                    jogadores.Add(item, Instantiate(playerprefeb, splayer.transform.position + rposition, playerprefeb.transform.rotation).GetComponent<PlayerBOT>());
                    jogadores[item].playerAtual = item;
                    //jogadores[item.Key].IDpartida = idpartida;
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
            }
            catch{
                Debug.Log("=Erro em spawnplayer!=");
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
            try
            {
                jogadores[IDjogadores].transform.position = posicaojogadores;
                jogadores[IDjogadores].vivo = vivo;

                //Debug.Log(jogadores[IDjogadores].transform.position);
                //=======anim==========
                jogadores[IDjogadores].transform.eulerAngles = new Vector3(jogadores[IDjogadores].transform.localRotation.eulerAngles.x, ry, jogadores[IDjogadores].transform.localRotation.eulerAngles.z);
            }
            catch
            {
                Debug.Log("acesso a chave inexistente!");
            }
            

            //obj[numPlayers].transform.position = posicaojogadores;
            liberarpos = false;
        }

    }

    IEnumerator EntrouFase1(){
        yield return new WaitForSeconds(1.5f);
        var jsonPayload = JsonConvert.SerializeObject(new
        {
            type = "Fase",
            data = "1",
            partidaid = Player.instance.IDpartida,
        });
        WS_Client.instance.ws.Send(jsonPayload);

        Debug.Log("EntrouFase1Coroutine!");
    }

    [System.Serializable]
    public class MeuObjeto {
    public string nome;
    public string [] players;

    }

}
