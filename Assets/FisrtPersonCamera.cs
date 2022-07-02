using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FisrtPersonCamera : MonoBehaviour
{
    public Transform characterBody;
    public Transform characterHead;
    float rotationX = 0;
    float rotationY = 0;
    float angleYmin = -90;
    float angleYMax = 90;
    //----sensibilidade do mouse------
    float sensitivityX = 4f;
    float sensitivityY = 4f;
    //--------------------------------
    public Vector3 GetForwardDirection() => transform.forward;
    // Start is called before the first frame update
    void Start()
    {
        sensitivityX = PlayerPrefs.GetInt("sensiX");
        sensitivityY = PlayerPrefs.GetInt("sensiY");
        //remover cursor da tela
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void LateUpdate() {
        transform.position = characterHead.position; //faz a camera ficar fixa na cabeça do personagem
    }

    // Update is called once per frame
    void Update()
    {
        float verticalDelta = Input.GetAxisRaw("Mouse Y") * sensitivityY;
        float horizontalDelta = Input.GetAxisRaw("Mouse X") * sensitivityX;

        rotationX += horizontalDelta;
        rotationY += verticalDelta;

        rotationY = Mathf.Clamp(rotationY, angleYmin, angleYMax); //limita a rotação para um angulo max e min

        if(Player.instance.health >0) {
            characterBody.localEulerAngles = new Vector3(0, rotationX, 0); //passa a rotação para o corpo do personagem
        }else{
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        transform.localEulerAngles = new Vector3(-rotationY, rotationX, 0); //passa a rotação do mouse para a rotação da camera
    }
}
