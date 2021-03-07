using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpManager : MonoBehaviour
{
    public int id;
    public float point;
    public MeshRenderer model;

    // Update is called once per frame
    void Update()
    {
        this.transform.Rotate(new Vector3(15, 30, 45) * Time.deltaTime);
    }

    public void Initialize(int _id, bool _activate)
    {
        id = _id;
        if (!_activate)
        { 
            Deactivate();
        }
    }

    public void SetPosition(Vector3 _position)
    {
        this.transform.position = _position;
    }

    public void Deactivate()
    {
        model.enabled = false;
    }

    public void Activate()
    {
        model.enabled = true;
    }
}
