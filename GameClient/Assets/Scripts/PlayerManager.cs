using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public int id;
    public string username;
    public float health;
    public float maxHealth = 100f;
    public MeshRenderer model;

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        health = maxHealth;
    }

    public void ReduceHealth(float _reduceHealth)
    {
        health -= _reduceHealth;
        Debug.Log($"Health remain {health}");
        if (health <= 0f)
        {
            Die();
        }
    }

    public void SetHealth(float _health)
    {
        health = _health;

        if (health <= 0f)
        {
            Die();
        }
    }
    public void SetPosition(Vector3 _position)
    {
        this.transform.position = _position;
    }

    public void Die()
    {
        model.enabled = false;
    }

    public void Respawn()
    {
        model.enabled = true;
        SetHealth(maxHealth);
    }
}
