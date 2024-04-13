using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class PlayerMovement : MonoBehaviour
{

    private PlayerModel _model;
    private Animator _animator;
    private Vector3 _positionVector;
    private Vector3 _homeVector;
    private float _yVal;
    private bool _inputCleared = true;
    public GameObject line;

    public Vector3 TargetPosition;
    
    [SerializeField] public float verticalMoveSpeed = 1;

    public float MoveDistance = 2;
    
    
    public bool isWalking;
    public bool isJumping;
    private static readonly int Jump = Animator.StringToHash("Jump");
    private static readonly int Walk = Animator.StringToHash("IsWalking"); 

    void Awake()
    {
        _positionVector = transform.localPosition;
        _model = gameObject.GetComponent<PlayerModel>();
        _animator = gameObject.GetComponent<Animator>();
        if (!_model)
        {
            throw new NullReferenceException("Model is not in player prefab!");
        }

    }

    // Update is called once per frame
    void Update()
    {
        float hInput = 0, yInput = 0, jumpInput = 0 ;
        
        if (!_model) return;
        switch (_model.Type)
        {
            case PlayerModel.PlayerType.Player1:
                yInput = Input.GetAxis("Vertical");
                hInput = Input.GetAxis("Horizontal");
                jumpInput = Input.GetAxis("Jump");
                break;
            
            case PlayerModel.PlayerType.Player2:
                yInput = Input.GetAxis("Vertical2");
                hInput = Input.GetAxis("Horizontal2");
                jumpInput = Input.GetAxis("Jump2");
                break;
        }
        
        if (jumpInput > 0 && !isJumping)
        {
        }

        if (yInput != 0)
        {
            moveVert(yInput);
            _inputCleared = false;
        }
        else
        {
            _inputCleared = true;
        }

        float distance = Vector3.Distance(transform.position-_homeVector, _positionVector);
        if (distance > 0.1)
        {
            transform.Translate((_positionVector-_homeVector-gameObject.transform.position) * (verticalMoveSpeed * Time.deltaTime));
            if (distance <= 0.7)
            {
                isJumping = false;
                _animator.ResetTrigger(Jump);
            }
        } 
        else
        {
            isJumping = false;
            _animator.ResetTrigger(Jump);
        }

        TargetPosition = _positionVector;
        
        var linescale = line.transform.localScale;

        if (hInput == 0)
        {
            linescale.x = 0;
        }
        else
        {
            float dy = 0;
            float dx = 0;
            
            if(_model.Type == PlayerModel.PlayerType.Player1)
            {
                linescale.x = 1;
                dy = hInput > 0 ? -MoveDistance : MoveDistance;
                dx = GameController.Player2StartPosition.position.x - transform.position.x;
            }
            else
            {
                linescale.x = -1;
                dy = hInput > 0 ? MoveDistance : -MoveDistance;
                dx = GameController.Player1StartPosition.position.x - transform.position.x;
            }
            
            line.transform.rotation = new Quaternion(0, 0, (float)Math.Atan((dy/2)/dx), 1);
        }
        
        line.transform.localScale = linescale;
    }

    public void setHome(Transform startTransform)
    {
        _positionVector = startTransform.position;
    }


    void moveVert(float inp)
    {
        if (!_inputCleared)
            return;
        
        if (inp > 0)
        {
            if (_yVal < MoveDistance)
            {
                _yVal += MoveDistance;
                _positionVector.y = _yVal;
                isJumping = true;
                _animator.SetTrigger(Jump);
            }
        }
        else
        {
            if (_yVal > -MoveDistance)
            {
                _yVal -= this.MoveDistance;
                _positionVector.y = _yVal;
                isJumping = true;
                _animator.SetTrigger(Jump);
            }
        }
    }

}
