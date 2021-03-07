using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class ServerSend
{
    #region Send TCP send method

    #region TCP basic method
    private static void SendTCPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].tcp.SendData(_packet);
    }

    private static void SendTCPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        foreach (Client _client in Server.clients.Values)
        {
            if (_client.tcp.isConnected())
            {
                Server.clients[_client.id].tcp.SendData(_packet);
            }
        }
    }
    
    private static void SendTCPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        foreach (Client _client in Server.clients.Values)
        {
            if (_client.tcp.isConnected())
            {
                if (_client.id != _exceptClient)
                { 
                    Server.clients[_client.id].tcp.SendData(_packet);
                }
            }
        }
    }
    #endregion
    /// <summary>Tell Client about its id and welcome him/her.</summary>
    public static void Welcome(int _toClient, string _msg)
    {
        using (Packet _packet = new Packet((int)ServerPackets.welcome))
        {
            _packet.Write(_msg);
            _packet.Write(_toClient);
            SendTCPData(_toClient, _packet);
        }
    }
    
    /// <summary>Send a package to _toClient about a player spawn. </summary>
    public static void SpawnPlayer(int _toClient, Player _player)
    {
        using (Packet _packet = new Packet((int) ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>Send packages to all about a player spawn. </summary>
    public static void SpawnPlayer(Player _player)
    {
        using (Packet _packet = new Packet((int) ServerPackets.spawnPlayer))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.username);
            _packet.Write(_player.transform.position);
            _packet.Write(_player.transform.rotation);

            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>Send Destroy player _id command to all </summary>
    public static void DestroyPlayer(int _id)
    {
        using (Packet _packet = new Packet((int)ServerPackets.destroyPlayer))
        {
            _packet.Write(_id);
            SendTCPDataToAll(_packet);
        }
    }

    public static void SpawnPickUp(int _toClient,PickUpManager _pickup)
    {
        using (Packet _packet = new Packet((int) ServerPackets.spawnPickup))
        {
            _packet.Write(_pickup.id);
            _packet.Write(_pickup.activate);
            _packet.Write(_pickup.transform.position);
            _packet.Write(_pickup.transform.rotation);

            SendTCPData(_toClient, _packet);
        }
    }

    /// <summary>Send DeactivatePickUp command to all </summary>
    public static void DeactivatePickUp(PickUpManager _pickUp)
    {
        using (Packet _packet = new Packet((int)ServerPackets.deactivatePickUp))
        {
            _packet.Write(_pickUp.id);
            SendTCPDataToAll(_packet);
        }
    }
    
    /// <summary>Send ActivatePickUp command to all </summary>
    public static void ActivatePickUp(PickUpManager _pickUp)
    {
        using (Packet _packet = new Packet((int)ServerPackets.activatePickUp))
        {
            _packet.Write(_pickUp.id);
            SendTCPDataToAll(_packet);
        }
    }

    /// <summary>Send Update Score to Client </summary>    
    public static void ScoreUpdate(int _toClient, int _score){
        using (Packet _packet = new Packet((int)ServerPackets.playerScore)){
            _packet.Write(_score);
            SendTCPData(_toClient, _packet);
        }
    }

    #endregion

    #region Server UDP send method
    private static void SendUDPData(int _toClient, Packet _packet)
    {
        _packet.WriteLength();
        Server.clients[_toClient].udp.SendData(_packet);
    }
    
    private static void SendUDPDataToAll(Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            Server.clients[i].udp.SendData(_packet);
        }
    }
    
    private static void SendUDPDataToAll(int _exceptClient, Packet _packet)
    {
        _packet.WriteLength();
        for (int i = 1; i <= Server.MaxPlayers; i++)
        {
            if (i == _exceptClient)
                continue;
            Server.clients[i].udp.SendData(_packet);
        }
    }

    
    /// <summary>
    /// Send packages to all players about one _player's position.
    /// </summary>
    /// <param name="_player"></param>
    public static void PlayerPosition(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerPosition))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.position);

            SendUDPDataToAll(_packet);
        }
    }

    /// <summary>
    /// Send packages to all players about one _player's rotation except himself.
    /// </summary>
    /// <param name="_player"></param>
    public static void PlayerRotation(Player _player)
    {
        using (Packet _packet = new Packet((int)ServerPackets.playerRotation))
        {
            _packet.Write(_player.id);
            _packet.Write(_player.transform.rotation);

            SendUDPDataToAll(_player.id, _packet);
        }
    }


    #endregion
}
