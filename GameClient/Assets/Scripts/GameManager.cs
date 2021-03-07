using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            Debug.Log("Instance already exists, destroying object!");
            Destroy(this);
        }
    }

    #region PlayerManagement Management
    
    public static Dictionary<int, PlayerManager> players = new Dictionary<int, PlayerManager>();
    public GameObject localPlayerPrefab;
    public GameObject playerPrefab;

    public void SpawnPlayer(int _id, string _username, Vector3 _position, Quaternion _rotation)
    {
        GameObject _player;
        if (_id == Client.instance.myId)
        {
            _player = Instantiate(localPlayerPrefab, _position, _rotation);
        }
        else
        {
            _player = Instantiate(playerPrefab, _position, _rotation);
        }

        Debug.Log($"Spawn Player {_id}");

        _player.GetComponent<PlayerManager>().Initialize(_id, _username);
        players.Add(_id, _player.GetComponent<PlayerManager>());
    }

    public void DestroyPlayer(int id)
    {
        Destroy(players[id].gameObject);
        players.Remove(id);
    }

    public void UpdatePosition(int _id, Vector3 _position)
    {
        players[_id].SetPosition(_position);
    }
    #endregion

    #region PickUp Management
    public static Dictionary<int, PickUpManager> pickUps = new Dictionary<int, PickUpManager>();
    public GameObject pickUpPrefab;

    public void SpawnPickUp(int _id, bool _activate, Vector3 _position, Quaternion _rotation)
    {
        GameObject _pickUp;
        _pickUp = Instantiate(pickUpPrefab, _position, _rotation);

        _pickUp.GetComponent<PickUpManager>().Initialize(_id, _activate);
        pickUps.Add(_id, _pickUp.GetComponent<PickUpManager>());
    }

    public void DeactivatePickUp(int _id)
    {
        pickUps[_id].gameObject.SetActive(false);
    }

    public void ActivatePickUp(int _id)
    {
        pickUps[_id].gameObject.SetActive(true);
    }
    #endregion

}
