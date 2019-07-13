using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//To Do:
//Make checkmate, check
//make teams changeable

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private BoardManager boardScript;
    public static Dictionary<Vector2, bool> occupiedSpots = new Dictionary<Vector2, bool>();
    public static Dictionary<Vector2, Piece> pieceLocation = new Dictionary<Vector2, Piece>();
    public static List<Vector2> white_moves;
    public static List<Vector2> black_moves;
    public bool hasPieceInHand = false;
    public bool isPlayerTurn = true;
    public bool reset_piece = false;
    public float white_check = 0;
    public float black_check = 0;
    public bool gameStarted = false;

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
    public void promote(Vector2 pos)
    {
    	pieceLocation.Remove(pos);
    	boardScript.promote_pawn(pos);
    }
    public void show_highlight(Vector2 location, bool move)
    {
        boardScript.show_moves(location, move);
    }
    public void remove_highlights()
    {
        boardScript.remove_all_highlights();
    }
    public void next_moves()
    {

    }
    public void checkmate()
    {

    }
}
