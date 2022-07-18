using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;

public class GameManager : MonoBehaviour
{
    public int maxMessages = 25;

    public string userField;
    public GameObject chatPanel, textObject;
    public InputField chatBox;

    public Color playerMessage, info;
    
    [SerializeField]
    List<Message> messageList = new List<Message>();

    string chatdata = "vazio";
    bool msg = false;

    void Start()
    {
        WS_Client.instance.ws.OnMessage += (sender, e) =>
        {
            var data = JsonConvert.DeserializeObject<Dictionary<string, object>>(e.Data);
            string option = (string)data["type"];
            switch (option)
            {
                case "chat-g":
                    Debug.Log("chat-global-msg");
                    chatdata = (string)data["data"];
                    msg = true;
                    break;
            }

        };
    }
    
    void Update() {
        if(msg){
            SendMessageToChat(GameConfigs.nomeplayer + ": " + chatdata, Message.MessageType.playerMessage);
            msg = false;
        }
        if(chatBox.text != "") {
            if(Input.GetKeyDown(KeyCode.Return)) {
                var jsonPayload = JsonConvert.SerializeObject(new
                {
                    type = "chat-global",
                    data = chatBox.text,
                    nome = GameConfigs.nomeplayer,
                });
                WS_Client.instance.ws.Send(jsonPayload);
                //SendMessageToChat(userField + ": " + chatBox.text, Message.MessageType.playerMessage);
                chatBox.text = "";
            }
        }else {
            if(!chatBox.isFocused && Input.GetKeyDown(KeyCode.Return)) {
                chatBox.ActivateInputField();
            }
        }
        if(!chatBox.isFocused) {
            if(Input.GetKeyDown(KeyCode.Space)) {
                SendMessageToChat("Space Pressed!", Message.MessageType.info);
                Debug.Log("Space");
            }
        }
    }

    public void SendMessageToChat(string text, Message.MessageType messageType) {
        if(messageList.Count >= maxMessages) {
            Destroy(messageList[0].textObject.gameObject);
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();
        newMessage.text = text;

        GameObject newText = Instantiate(textObject, chatPanel.transform);

        newMessage.textObject = newText.GetComponent<Text>();
        newMessage.textObject.text = newMessage.text;

        newMessage.textObject.color = MessageTypeColor(messageType);

        messageList.Add(newMessage);
    }

    Color MessageTypeColor(Message.MessageType messageType) {
        Color currentColor = info;

        switch(messageType) {
            case Message.MessageType.playerMessage:
                currentColor = playerMessage;
                break;
        }

        return currentColor;
    }
}

[System.Serializable]
public class Message {
    public string text;
    public Text textObject;
    public MessageType messageType;
    public enum MessageType {
        playerMessage,
        info
    }
}
