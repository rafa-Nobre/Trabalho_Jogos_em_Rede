using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mira : MonoBehaviour
{
    public Image image;
    public static mira instance;
    // Start is called before the first frame update
     void Awake()
    {
        instance = this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
       
    }

    public void miraON(){
        image.gameObject.SetActive(true);
    }
    public void miraOFF(){
        image.gameObject.SetActive(false);
    }
}
