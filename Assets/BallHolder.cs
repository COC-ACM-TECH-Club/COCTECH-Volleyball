using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BallHolder : MonoBehaviour
{
    private GameObject _ball;
    private bool _hasBall = false;
    
    public GameObject Ball
    {
        get => _ball;
        set => setBall(value); 
    }

    public bool IsHoldingBall;

    private void setBall(GameObject ball)
    {
        _ball = ball;
        _hasBall = ball != null;
        IsHoldingBall = _hasBall;
    }

    // Update is called once per frame
    void Update()
    {
        if (_hasBall)
        {
            _ball.transform.localPosition.Set(transform.localPosition.x, transform.localPosition.y, transform.localPosition.z);
        } 
    }
}
