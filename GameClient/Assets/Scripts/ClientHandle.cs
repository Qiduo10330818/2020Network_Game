using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    public static void Welcome(Packet _packet)
    {
        string _msg = _packet.ReadString();
        int _myid = _packet.ReadInt();

        Debug.Log($"Message from server: {_msg} my id {_myid}");
        Client.instance.myId = _myid;
        ClientSend.WelcomeReceived();
        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        Debug.Log($"Received spawnPlayer packet {_id} {_username}");

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerPosition(Packet _packet)
    {
        int _id = _packet.ReadInt();
        Vector3 _position = _packet.ReadVector3();

        try {
            GameManager.instance.UpdatePosition(_id, _position);
        }
        catch(Exception _ex)
        { 
            Debug.Log($"Update Player {_id} position failed: {_ex}");
        }
    }

    public static void PlayerRotation(Packet _packet)
    {
        /*int _id = _packet.ReadInt();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.players[_id].transform.rotation = _rotation;*/
    }

    public static void DestroyPlayer(Packet _packet)
    {
        int id = _packet.ReadInt();
        GameManager.instance.DestroyPlayer(id);
    }

    public static void SpawnPickUp(Packet _packet)
    { 
        int _id = _packet.ReadInt();
        bool _activate = _packet.ReadBool();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();
        
        GameManager.instance.SpawnPickUp(_id, _activate, _position, _rotation);
    }

    public static void DeactivatePickUp(Packet _packet)
    {
        int _id = _packet.ReadInt();
        GameManager.instance.DeactivatePickUp(_id);
    }

    public static void ActivatePickUp(Packet _packet)
    {
        int _id = _packet.ReadInt();
        GameManager.instance.ActivatePickUp(_id);
    }
    public static void ScoreUpdate(Packet _packet)
    {
        int _score = _packet.ReadInt();
        Client.instance.myScore = _score;
        Debug.Log("My score is update to "+ Client.instance.myScore);
        UIManager.instance.ScoreText.text = "Score: "+ _score.ToString();
    }
}
