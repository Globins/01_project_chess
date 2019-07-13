using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//To Do:
//Make checkmate, check, castle, and promotion
//make squares you can go on highlighted
//make prior moves seeable by text
//make teams changeable

//Refactor code, all of it
public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private BoardManager boardScript;
    public static Dictionary<Vector2, bool> occupiedSpots = new Dictionary<Vector2, bool>();
    public static Dictionary<Vector2, Piece> pieceLocation = new Dictionary<Vector2, Piece>();
    public bool hasPieceInHand = false;
    public bool isPlayerTurn = true;
    public bool reset_piece = false;
    public float white_check = 0;
    public float black_check = 0;
    
    void Awake()
    {
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);    
        DontDestroyOnLoad(gameObject);
        boardScript = GetComponent<BoardManager>();
        InitGame();
    }
    void InitGame()
    {
        boardScript.SetupScene();
    }
    void update()
    {
        reset_piece = false;
    }
    public void promote_pawn(Vector2 pos)
    {

    }
}
