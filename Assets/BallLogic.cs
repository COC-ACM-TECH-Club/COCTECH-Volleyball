using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallLogic : MonoBehaviour
{

    private Vector3 _target;
    private Vector3 _originalPosition;
    private float yMod = 3;
    private float _initialDistance;
    private Animator _animator;
    
    public float speed;
    public bool isThrown = false;
    public float throwHeight = 2;
    public float rotationSpeed = 1;
    private static readonly int IsThrown = Animator.StringToHash("IsThrown");
    private static readonly int Direction = Animator.StringToHash("Direction");

    public Vector3 OriginalLocation
    {
        get => _originalPosition;
    }
    public Vector3 TargetPosition
    {
        get => _target;
        set => setTarget(value);
    }

    private void setTarget(Vector3 vect)
    {
        isThrown = true;
        _target = vect;
        _originalPosition = transform.position;
        _initialDistance = Vector3.Distance(_target, _originalPosition);
    }
    
    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isThrown)
        {
            transform.Translate((_target-transform.position) * (Time.deltaTime * speed), Space.World);
            float distance = Vector3.Distance(transform.position, _target);
            if (distance < 0.2)
            {
                _animator.SetBool(IsThrown, false);
                isThrown = false;
            }
            else
            {
                _animator.SetInteger(Direction, (int)(_target.x - transform.position.x));
            }
        } 
        _animator.SetBool(IsThrown, isThrown);
    }
}
