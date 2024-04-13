using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    public enum PlayerType
    {
        Player1,
        Player2
    }

    private PlayerType _type;
    private bool _firePressed = false;
    public bool FirePressed { 
        get => _firePressed;
        set => _firePressed = value; 
    }
    
    public PlayerType Type 
    { 
        get => _type;
        set => setType(value);
    }

    private void setType(PlayerType type)
    {
        this._type = type;
        if (type == PlayerType.Player2)
        {
            GetComponent<SpriteRenderer>().flipX = true;
        }
        else
        {
            GetComponent<SpriteRenderer>().flipX = false;
        }
    }

    private void Update()
    {
        if (_type == PlayerType.Player1)
        {
            FirePressed = Input.GetAxis("Fire1") > 0;
        }
        else
        {
            FirePressed = Input.GetAxis("Fire1a") > 0;
        } 
    }

}
