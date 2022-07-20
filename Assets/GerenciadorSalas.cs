using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
using UnityEngine.SceneManagement;
public class GerenciadorSalas : MonoBehaviour
{
    public GameObject salaPanel, butaoSala;
    public InputField inputSala;

    bool novasala = false;
    string nomePartida = "sala";
    string qtdPlayers = "";

    Dictionary<string, object> datasalas;

    bool chegousalas = false;

    List<GameObject> todassala;

    bool jogadorPodeEntrarSala = false;

    string idSala = "";
    // Start is called before the first frame update
    void Start()
    {
        todassala = new List<GameObject>();
        datasalas = new Dictionary<string, object>();
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
                case "todas-salas":
                    Debug.Log("salas recebidas!");
                    datasalas = data;
                    chegousalas = true;
                    break;
                case "jogador-pode-entrar":
                    idSala = (string)data["idSala"];
                    jogadorPodeEntrarSala = true;
                    break;
                case "jogador-nao-pode-entrar":
                    Debug.Log("sala cheia - servidor!");
                    break;
            }

        };
    }

    // Update is called once per frame
    void Update()
    {
        if(jogadorPodeEntrarSala){
            WS_Client.instance.idpart =  idSala;
            Debug.Log("podeentrar-idsala:"+ WS_Client.instance.idpart);
            jogadorPodeEntrarSala = false;
            SceneManager.LoadScene("Fase1");
        }
         if(novasala){
            int x =  int.Parse(qtdPlayers);
            CriarSala(nomePartida,x);
            novasala = false;
         }
        if(chegousalas){
            var sob = JsonConvert.SerializeObject(datasalas["salas"]);
            var obb = JsonConvert.DeserializeObject<Dictionary<string, object>>(sob);
            //nome: nome, players: vplayers
            foreach (var item in obb)
            {
                var sob2 = JsonConvert.SerializeObject(obb[item.Key]);
                var obb2 = JsonConvert.DeserializeObject<MeuObjeto>(sob2);
                //obb[item.Key]["nome"] = n;
                //int [] b = JsonConverter<ArrayList>();
                //int [] v = (arr)obb2["players"];
                Debug.Log("nome obb2:"+obb2.nome+"players:"+obb2.players.Length);
                CriarSala(obb2.nome,obb2.players.Length);
            }
            chegousalas = false;
        }

    }

    void CriarSala(string np, int qtdp){
        GameObject butaosala = Instantiate(butaoSala, salaPanel.transform);
        todassala.Add(butaosala);
        Text texto = butaosala.GetComponentInChildren<Text>();
        DadosButao dadosDaSala =  butaosala.GetComponentInChildren<DadosButao>();
        dadosDaSala.nomeSala = np;
        dadosDaSala.quantidadeJogadores = qtdp;
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

    public void ButtonRefresh(){
        foreach (var item in todassala)
        {
            Destroy(item);
        }
       todassala.Clear();
        var jsonPayload = JsonConvert.SerializeObject(new
                {
                    type = "status-salas",
                    id = WS_Client.instance.idp,
                });
                WS_Client.instance.ws.Send(jsonPayload);
    }



   [System.Serializable]
    public class MeuObjeto {
    public string nome;
    public string [] players;

    }
}
