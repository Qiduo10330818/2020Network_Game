using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    public static Dictionary<int, PickUpManager> pickUps = new Dictionary<int, PickUpManager>();
    public static int count = 0;

    public int id;
    public bool activate;
    public DateTime respawn_time;

    void Start()
    {
        count += 1;
        id = count;
        pickUps.Add(count, this);
        activate = false;
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        respawn_time = DateTime.Now.AddSeconds(5);
    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);

        if (!activate && respawn_time < System.DateTime.Now)
        {
            Debug.Log($"{id} pick up is respawned.");
            Activate();
        }
    }

    public int Deactivate()
    {
        activate = false;
        this.gameObject.GetComponent<MeshRenderer>().enabled = false;
        respawn_time = System.DateTime.Now.AddSeconds(10); 
        ServerSend.DeactivatePickUp(this);
        return id;
    }

    public int Activate()
    {
        activate = true;
        this.gameObject.GetComponent<MeshRenderer>().enabled = true;
        ServerSend.ActivatePickUp(this);
        return id;
    }
}
