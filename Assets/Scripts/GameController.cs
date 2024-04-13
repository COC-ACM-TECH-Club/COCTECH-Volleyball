using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.Serialization;
using Object = UnityEngine.Object;

public class GameController : MonoBehaviour
{

    public enum GameState
    {
        Playing, 
        Paused 
    }

    
    public static Transform Player1StartPosition { get; set; }
    public static Transform Player2StartPosition { get; set ; }
    

    [SerializeField] public GameState gameState = GameState.Paused; 
    [SerializeField] public GameObject net;
    [SerializeField] public GameObject ball;
    [SerializeField] public GameObject player;
    [SerializeField] public Transform player1StartPosition;
    [SerializeField] public Transform player2StartPosition;

    private GameObject _player1;
    private GameObject _player2;
    private GameObject _ball;
    
    private Camera _camera;

    // Start is called before the first frame update
    void Start()
    {
        _camera = Camera.main;
        net = net == null ? GameObject.FindGameObjectWithTag("Net") : net;
        Player1StartPosition = player1StartPosition;
        Player2StartPosition = player2StartPosition;
        StartRound();
    }

    void StartRound()
    {
        Clear();

        _player1 = Object.Instantiate(player);
        _player2 = Object.Instantiate(player);
        _ball = Object.Instantiate(ball);
        
        _player1.transform.SetPositionAndRotation(player1StartPosition.position, Quaternion.identity);
        _player2.transform.SetPositionAndRotation(player2StartPosition.position, Quaternion.identity);
        
        _player1.GetComponent<PlayerModel>().Type = PlayerModel.PlayerType.Player1;
        _player2.GetComponent<PlayerModel>().Type = PlayerModel.PlayerType.Player2;
        
        _player1.GetComponent<PlayerMovement>().setHome(player1StartPosition);
        _player2.GetComponent<PlayerMovement>().setHome(player2StartPosition);
        
        _player1.GetComponent<PlayerBallMechanic>().HoldBall(_ball);     

        gameState = GameState.Playing;
    }

    void Clear()
    {
        if(_player1 != null)
            Destroy(_player1);
        if(_player2 != null)
            Destroy(_player2);
        if(ball != null)
            Destroy(_ball);
    }

    // Update is called once per frame
    void Update()
    {
        try
        {
            float screenMidX = (_camera.rect.xMax - _camera.rect.xMin) / 2;
            net.transform.position.Set(screenMidX, net.transform.position.y, net.transform.position.z);
            if (Input.GetKey("escape"))
            {
                Application.Quit();
            } else if ((Input.GetKey(KeyCode.LeftCommand) || Input.GetKey(KeyCode.LeftControl)) && Input.GetKey(KeyCode.R))
            {
                StartRound();
            };
        }
        catch (Exception e)
        {
            // ignored
        }
    }
}
