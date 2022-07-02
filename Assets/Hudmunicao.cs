using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Hudmunicao : MonoBehaviour
{
    private Text ammo;
    // Start is called before the first frame update
    void Start()
    {
        ammo = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int municao = Player.instance.municao;
        ammo.text = municao.ToString();
    }
}
