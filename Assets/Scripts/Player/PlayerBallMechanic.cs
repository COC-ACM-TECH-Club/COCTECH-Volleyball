using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Object = UnityEngine.Object;
using Random = System.Random;

public class PlayerBallMechanic : MonoBehaviour
{

    private PlayerModel _model;
    private bool _holdingBall;
    public GameObject ball;

    public float spikeAngle = 20;
    public float throwPower = 5;
    public float throwDistance = 7;
    public bool IsHoldingBall
    {
        get => _holdingBall;
        set => _holdingBall = value;
    }

    [SerializeField] public Vector3 ballOffset;
    
    public void HoldBall(GameObject ball)
    {
        this.ball = ball;
        _holdingBall = true;
        
        if (GetComponent<PlayerModel>().Type == PlayerModel.PlayerType.Player2)
        {
            ballOffset.x = -ballOffset.x;
        }

        ball.GetComponent<Rigidbody2D>().simulated = false;
        ball.transform.SetParent(transform);
        ball.transform.localPosition.Set(ballOffset.x, ballOffset.y, ballOffset.z);
    }

    public void LetBallGo(int direction)
    {
        if (ball == null)
            return;
        ball.transform.SetParent(null);
        
        SpikeBall();
        ball = null;
        _holdingBall = false;
    }

    public void SpikeBall()
    {
        if (ball == null)
            return;

        var input = -Input.GetAxis(_model.Type == PlayerModel.PlayerType.Player1 ? "Horizontal": "Horizontal2");
        
        var xPos = _model.Type == PlayerModel.PlayerType.Player1
            ? GameController.Player2StartPosition.position.x
            : GameController.Player1StartPosition.position.x;
        var moveDist = GetComponent<PlayerMovement>().MoveDistance;
        float yPos = 0;
        
        //Shoot up
        if (input > 0)
        {
            yPos += moveDist;
        } else if (input < 0) //Shoot down
        {
            yPos -= moveDist;
        }

        yPos *= _model.Type == PlayerModel.PlayerType.Player2 ? -1 : 1;

        ball.GetComponent<BallLogic>().TargetPosition = new Vector3(xPos, (GetComponent<PlayerMovement>().TargetPosition.y + yPos), 0f) * 2;
        ball.GetComponent<BallLogic>().speed = throwPower/2;
        ball.GetComponent<Rigidbody2D>().simulated = true;
    }

    public void OnCollisionEnter2D(Collision2D collision2D)
    {
        if (collision2D.gameObject.tag.Equals("Ball") && (IsHoldingBall || _model.FirePressed))
        {
            ball = collision2D.gameObject;
            SpikeBall();
        }
    }

    public void OnCollisionStay2D(Collision2D other)
    {
        OnCollisionEnter2D(other); 
    }

    public float getVerticalInput()
    {
        if (_model.Type == PlayerModel.PlayerType.Player1)
        {
            if (Input.GetAxis("Fire1") > 0 && _holdingBall)
            {
                return((int)Input.GetAxis("Vertical"));
            }
        }
        else
        {
            if (Input.GetAxis("Fire1a") > 0 && _holdingBall)
            {
                return((int)Input.GetAxis("Vertical2"));
            }
        }

        return 0;
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _model = GetComponent<PlayerModel>();
    }

    // Update is called once per frame
    void Update()
    {
        // LetBallGo((int)getVerticalInput());
        if (_model.FirePressed && IsHoldingBall)
        {
            LetBallGo(0); 
        }
        IsHoldingBall = _holdingBall;
        if (_holdingBall)
        {
            ball.transform.SetLocalPositionAndRotation(ballOffset, Quaternion.identity);
        }
    }
    
    
}
