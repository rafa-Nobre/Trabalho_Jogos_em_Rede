using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Newtonsoft.Json;
public class Player : MonoBehaviour
{
    public int playerNumber = 0;

    public bool liberarposicao = false;
    
    CharacterController controller; //implementação da colisão atravez do CharacterController;
    Vector3 forward; //movimento em z, frente e trás;
    Vector3 strafe; //movimento em x, esquerda e direita;
    Vector3 vertical; //movimento em y, pulo;

    float forwardSpeed = 5f;
    float strafeSpeed = 5f;

    //------variaveis do pulo-----------
    float gravity;
    float jumpSpeed;
    float maxJumpHeigth = 2f;
    float timeToMaxHeight = 0.5f;
    //----------------------------------
    private Animator anim;
    //public float speed;
    //public float jump;
    //public float gravity;
    //private float rot;
    //public float rotSpeed;
    //private Vector3 moveDirection;

    public static Player instance;

    public audio audiocontroller;

    //float rotationX = 0;
    //float rotationY = 0;

    public Transform characterBody;

    public int health;

    public int municao;

    bool auxItems = true;

    public int itemscollected = 0;

    public int enemys = 0;
    void Awake()
    {
        instance = this;
    }
    void Start()
    {
        controller = GetComponent<CharacterController>();
        gravity = (-2 * maxJumpHeigth) / (timeToMaxHeight * timeToMaxHeight); //gravity = (-2*Hmax)/t^2
        jumpSpeed = (2 * maxJumpHeigth) / timeToMaxHeight; //jumpSpeed = (2*Hmax)/t

        anim = GetComponent<Animator>();

    }

    // Update is called once per frame
    void Update()
    {   
        if(playerNumber == 0){
            if(health > 0){
                Move();
            }else{
                anim.SetInteger("transition", 6);
            }

            if(itemscollected == 4 && auxItems==true){
                health = 99;
                auxItems = false;
            }

            RunFaster();
        }
    }

    private void LateUpdate()
    {
        if(playerNumber == 0 && liberarposicao){
               var jsonPayload = JsonConvert.SerializeObject(new
                    {
                        type = "posicao",
                        x = transform.position.x,
                        y = transform.position.y,
                        z = transform.position.z
                    });
                WS_Client.instance.ws.Send(jsonPayload);
                //Debug.Log("posicao player0: x"+transform.position.x+" y"+transform.position.y+" z"+transform.position.z);
        }
    }

    void RunFaster(){
        if(Input.GetKey(KeyCode.LeftShift)){
            forwardSpeed = 10f;
        }
        else{
            forwardSpeed = 5f;
        }
    }
    
    void Move(){
        if(controller.isGrounded){
            if(anim.GetInteger("transition") == 2){
                anim.SetInteger("transition", 0);
            }
        }

        //Move();
        float forwardInput = Input.GetAxisRaw("Vertical"); //retorna 1 para W, e -1 para S
        float strafeInput = Input.GetAxisRaw("Horizontal"); //retorna 1 para D, e -1 para A

        if(strafeInput != 0 || forwardInput !=0){
            //anim.SetInteger("transition", 1);
            if(Input.GetKey(KeyCode.Mouse1)){
                    anim.SetInteger("transition", 4);
                }
            else{
                    anim.SetInteger("transition", 1); 
            }
        }
        else{
            
            if(Input.GetKey(KeyCode.Mouse1)){
                    //anim.SetInteger("transition", 3);
                    if(Input.GetKeyDown(KeyCode.Mouse0)){
                        anim.SetInteger("transition", 5);
                    }
                    else{
                        anim.SetInteger("transition", 3);
                    }
                }
            else{
               if(anim.GetInteger("transition") == 1 || anim.GetInteger("transition") == 3){
                    anim.SetInteger("transition", 0); 
                }
            }

        }

        // force = input * speed * direction
        forward = forwardInput * forwardSpeed * transform.forward;
        strafe = strafeInput * strafeSpeed * transform.right;

        vertical += gravity * Time.deltaTime * Vector3.up; //soma a gravidade a força vertical a cada frame
        if(controller.isGrounded){
            vertical = Vector3.down; //reseta a força vertical sempre que o player estiver no chão
        }
        if(Input.GetKeyDown(KeyCode.Space) && controller.isGrounded){
            vertical = jumpSpeed * Vector3.up;
            anim.SetInteger("transition", 2);
        }

        if(vertical.y > 0 && (controller.collisionFlags & CollisionFlags.Above) != 0){
            vertical = Vector3.zero;
        }

        Vector3 finalVelocity = forward + strafe + vertical; //soma dos movimentos nos 3 eixos;

        controller.Move(finalVelocity * Time.deltaTime); //recebe como parametro os deslocamentos e move o personagem calculando a colisão;

    }

    public void Playsteps(){
        audiocontroller.FX();
    }

    public void SubLife(int dano){
        if(health > 0) {
            audiocontroller.DamagePlayer();
            health -= dano;
        }
        if(health <0){
            health = 0;
        }
    }

    public void AddMunicao(int x){
        municao += x;
    }

    public void SubMunicao(int y){
        municao -= y;
    }

    public void AddLife(int l){
        health += l;
    }

    public void AddItem(){
        itemscollected ++;
    }
}
