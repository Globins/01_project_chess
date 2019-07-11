using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private BoardManager boardScript;
    public static Dictionary<Vector2, bool> occupiedSpots = new Dictionary<Vector2, bool>();
    public static Dictionary<Vector2, Piece> pieceLocation = new Dictionary<Vector2, Piece>();
    public bool hasPieceInHand = false;
    public bool playersTurn = true;
    public bool reset_piece = false;
    
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
}
