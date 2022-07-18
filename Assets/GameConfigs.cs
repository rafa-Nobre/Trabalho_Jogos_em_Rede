using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
using System;
using Newtonsoft.Json;
public class GameConfigs : MonoBehaviour
{
    public Dropdown ddpResolution;
    public Dropdown ddpQuality;
    public Toggle tgWindow;

    public GameObject optionsPanel;

    public GameObject multiPanel;

    public GameObject lobbyPanel;

    public Slider sensiXslider;
    public Slider sensiYslider;

    public InputField nomeConexao;
    public InputField senhaConexao;
    public InputField emailCadastro;
    public InputField nomeCadastro;
    public InputField senhaCadastro;


    public int sensiXvalue;
    public int sensiYvalue;

    public Text sensiTextX;
    public Text sensiTextY;

    private List<string> resolutions = new List<string>();
    private List<string> quality = new List<string>();

    bool entrarLobby = false;

    public static string nomeplayer = "none"; 
    // Start is called before the first frame update
    private void Awake() {
        if(PlayerPrefs.HasKey("resW") && PlayerPrefs.HasKey("resH")){
            Screen.SetResolution(PlayerPrefs.GetInt("resW"), PlayerPrefs.GetInt("resH"), Screen.fullScreen);
        }
        else{

        }
    }
    void Start()
    {
        WS_Client.instance.ws.OnMessage += (sender, e) =>
        {
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Data);
            string option = (string)data["type"];
            switch (option)
            {
                case "enter-lobby":
                    Debug.Log("entrou no eterlobby!");
                    nomeplayer = (string)data["nome"];
                    entrarLobby = true;
                    break;
            }

        };
        //Resolution[] arrResolution = Screen.resolutions;
        //foreach (Resolution r in arrResolution){}
            resolutions.Add(string.Format("640 X 360"));
            resolutions.Add(string.Format("1280 X 720"));
            resolutions.Add(string.Format("1366 X 768"));
            resolutions.Add(string.Format("1920 X 1080"));
        
        ddpResolution.AddOptions(resolutions);
        //ddpResolution.value = (resolutions.Count - 1);
        if(PlayerPrefs.HasKey("posicaoRes")){
            ddpResolution.value = PlayerPrefs.GetInt("posicaoRes");
        }else{
            ddpResolution.value = 1;
        }

        quality = QualitySettings.names.ToList<String>();
        ddpQuality.AddOptions(quality);
        ddpQuality.value = QualitySettings.GetQualityLevel();


        if(PlayerPrefs.HasKey("sensiX")){
            sensiXvalue = PlayerPrefs.GetInt("sensiX");
        }
        else{
            PlayerPrefs.SetInt("sensiX", 4);
            sensiXvalue = 4;
        }

        if(PlayerPrefs.HasKey("sensiY")){
            sensiYvalue = PlayerPrefs.GetInt("sensiY");
        }
        else{
            PlayerPrefs.SetInt("sensiY", 4);
            sensiYvalue = 4;
        }

        sensiXslider.value = sensiXvalue;
        sensiYslider.value = sensiYvalue;

        if(PlayerPrefs.HasKey("resW") && PlayerPrefs.HasKey("resH")){
            Screen.SetResolution(PlayerPrefs.GetInt("resW"), PlayerPrefs.GetInt("resH"), Screen.fullScreen);
        }

    }

    public void SetWindowMode(){
        if(tgWindow.isOn){
            Screen.fullScreen = false;
        }
        else{
            Screen.fullScreen = true;
        }
    }

    public void SetResolution(){
        string[] res = resolutions[ddpResolution.value].Split('X');
        int w = Convert.ToInt16(res[0].Trim());
        int h = Convert.ToInt16(res[1].Trim());
        Screen.SetResolution(w, h, Screen.fullScreen);
        PlayerPrefs.SetInt("resW", w);
        PlayerPrefs.SetInt("resH", h);
        PlayerPrefs.SetInt("posicaoRes", ddpResolution.value);
    }

    public void setQuality(){
        QualitySettings.SetQualityLevel(ddpQuality.value, true);
    }

    public void Back(){
        sensiXvalue = (int)sensiXslider.value;
        sensiYvalue = (int)sensiYslider.value;
        PlayerPrefs.SetInt("sensiX", sensiXvalue);
        PlayerPrefs.SetInt("sensiY", sensiYvalue);
        optionsPanel.SetActive(false);
    }

    public void BackMultiplayer(){
        multiPanel.SetActive(false);
    }

    public void BackLobby(){
        lobbyPanel.SetActive(false);
    }

     public void OptionsMenu(){
        optionsPanel.SetActive(true);
        sensiTextX.text = sensiXvalue.ToString();
        sensiTextY.text = sensiYvalue.ToString();
    }

    public void MultiplayerMenu(){
        multiPanel.SetActive(true);
    }

    public void LobbyPanel(){
        lobbyPanel.SetActive(true);
    }

    public void EntrarButton(){
        var jsonPayload = JsonConvert.SerializeObject(new
                    {
                        type = "login",
                        nome = nomeConexao.text,
                        senha = senhaConexao.text
                    });
                WS_Client.instance.ws.Send(jsonPayload);
    }

    public void CadastrarButton(){
        var jsonPayload = JsonConvert.SerializeObject(new
                    {
                        type = "cadastro",
                        email = emailCadastro.text,
                        nome = nomeCadastro.text,
                        senha = senhaCadastro.text
                    });
                WS_Client.instance.ws.Send(jsonPayload);
    }

    public void ChangeSlideValueX(){

        sensiTextX.text = sensiXslider.value.ToString();
    }
    public void ChangeSlideValueY(){
        sensiTextY.text = sensiYslider.value.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        if(entrarLobby){
            LobbyPanel();
            entrarLobby = false;
        }
    }
}
