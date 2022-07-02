using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextFase : MonoBehaviour
{

    public string lvlName;
    // Update is called once per frame
    void Update()
    {
        if(Player.instance.itemscollected == 4 && Player.instance.enemys == 0){
            SceneManager.LoadScene(lvlName);
        }
    }
}
