using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    public int vidaDoPlayer = 3;
    public GameObject gameOverPanel;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Player.instance.health <= 0){
            ShowGameOver();
        }else{
            if(Input.GetKeyDown(KeyCode.Escape)){
                if(Time.timeScale == 1){
                    gameOverPanel.SetActive(true);
                    //Time.timeScale = 0;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                }
                else{
                    gameOverPanel.SetActive(false);
                    //Time.timeScale = 1;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                }
            }
        }
    }

    public void ShowGameOver(){
        gameOverPanel.SetActive(true);
        //ta faltando alguma coisa aqui
        //Cursor.visible = true;
        //Cursor.lockState = CursorLockMode.None;
    }

    public void RestartGame(string lvlName){
        gameOverPanel.SetActive(false);
        //Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        SceneManager.LoadScene(lvlName);
    }

    public void RetryButton(){
        gameOverPanel.SetActive(false);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        Player.instance.ResetarPlayer();
    }

    public void fecharWS(){
        WS_Client.instance.ws.Close();
    }

    public void Exit(){
        Application.Quit();
    }
}
