using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public int maxMessages = 25;
    public GameObject chatPanel, textObject;
    
    [SerializeField]
    List<Message> messageList = new List<Message>();

    void Start()
    {
        
    }

    
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)) {
            SendMessageToChat("Space Pressed!");
            Debug.Log("Space");
        }
    }

    public void SendMessageToChat(string text) {
        if(messageList.Count >= maxMessages) {
            messageList.Remove(messageList[0]);
        }

        Message newMessage = new Message();
        newMessage.text = text;

        messageList.Add(newMessage);
    }
}

[System.Serializable]
public class Message {
    public string text;
}
