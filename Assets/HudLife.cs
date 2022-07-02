using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HudLife : MonoBehaviour
{
    private Text life;
    // Start is called before the first frame update
    void Start()
    {
        life = gameObject.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        int lifehud = Player.instance.health;
        life.text = lifehud.ToString();
    }
}
