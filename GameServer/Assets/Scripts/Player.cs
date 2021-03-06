using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int id;
    public string username;
    public CharacterController controller;
    public int score;
    public float gravity = -9.81f;
    public float moveSpeed = 5f;
    public float jumpSpeed = 5f;

    private bool[] inputs;
    private float yVelocity = 0;

    private void Start()
    {
        gravity *= Time.fixedDeltaTime * Time.fixedDeltaTime;
        moveSpeed *= Time.fixedDeltaTime;
        jumpSpeed *= Time.fixedDeltaTime;
    }

    public void Initialize(int _id, string _username)
    {
        id = _id;
        username = _username;
        score = 0;
        inputs = new bool[5];
        Debug.Log("My score is "+ score);
    }

    public void FixedUpdate()
    {
        Vector2 _inputDirection = Vector2.zero;

        if (inputs[0])
        {
            _inputDirection.y += 1;
        }

        if (inputs[1])
        {
            _inputDirection.y -= 1;
        }
        
        if (inputs[2])
        {
            _inputDirection.x += 1;
        }
        
        if (inputs[3])
        {
            _inputDirection.x -= 1;
        }

        Move(_inputDirection);
    }

    private void Move(Vector2 _inputDirection)
    {
        Vector3 _moveDirection = - transform.right * _inputDirection.x + transform.forward * _inputDirection.y;
        _moveDirection *= moveSpeed;

        if (controller.isGrounded)
        {
            yVelocity = 0f;

            if (inputs[4])
            {
                yVelocity = jumpSpeed;
            }
        }
        yVelocity += gravity;

        _moveDirection.y = yVelocity;
        controller.Move(_moveDirection);

        ServerSend.PlayerPosition(this);
        ServerSend.PlayerRotation(this);
    }

    public void SetInput(bool[] _inputs, Quaternion _rotation)
    {
        inputs = _inputs;
        transform.rotation = _rotation;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Pick Up"))
        {
            if(other.gameObject.GetComponent<PickUpManager>().activate == true){
                int pickupId = other.gameObject.GetComponent<PickUpManager>().Deactivate();
                this.score += 1;
                ServerSend.ScoreUpdate(this.id, this.score);
                Debug.Log("Player"+this.id+"'s score = "+this.score);
            }
            
            // TODO: Broadcast this pickup is deactivated to all player
        }
    }
}

