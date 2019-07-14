using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private BoardManager boardScript;
    public static Dictionary<Vector2, bool> occupiedSpots = new Dictionary<Vector2, bool>();
    public static Dictionary<Vector2, Piece> pieceLocation = new Dictionary<Vector2, Piece>();
    public static List<Vector2> white_moves = new List<Vector2>();
    public static List<Vector2> black_moves = new List<Vector2>();
    public bool hasPieceInHand = false;
    public bool isPlayerTurn = true;
    public bool reset_piece = false;
    public bool gameStarted = false;
    public static Piece bking_location;
    public static Piece wking_location;
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
    public void check_game_end()
    {
        if((white_moves.Count == 0 && !white_is_in_check()) || (black_moves.Count == 0 && !black_is_in_check()))
        {
            Moves_Box.force_print("Stalemate!");
            UnityEditor.EditorApplication.isPlaying = false;
        }
        if((white_moves.Count == 0 && white_is_in_check()) || !pieceLocation.ContainsValue(wking_location))
        {
            Moves_Box.force_print("Black Victory!");
            UnityEditor.EditorApplication.isPlaying = false;
        }
        if((black_moves.Count == 0 && black_is_in_check()) || !pieceLocation.ContainsValue(bking_location))
        {
            Moves_Box.force_print("White Victory!");
            UnityEditor.EditorApplication.isPlaying = false;
        }
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
    public bool black_is_in_check()
    {
        Vector2 location = new Vector2(0,0);
        foreach(var item in pieceLocation)
        {
            if(item.Value.name.IndexOf("bking")  > -1)
                location = item.Key;
        }
        if(white_moves.Contains(location))
            return true;
        return false;
    }
    public bool white_is_in_check()
    {
        Vector2 location = new Vector2(0,0);
        foreach(var item in pieceLocation)
        {
            if(item.Value.name.IndexOf("wking")  > -1)
                location = item.Key;
        }
        if(black_moves.Contains(location))
            return true;
        return false;
    }
    public void get_black_moves()
    {
        black_moves = new List<Vector2>();
        foreach(var item in pieceLocation)
        {
            if(item.Value.name[0] == 'b' && item.Value.name.IndexOf("bking")  == -1)
            {
                black_moves.AddRange(item.Value.get_moves());
            }
        }
        remove_highlights();
    }
    public void get_white_moves()
    {
        white_moves = new List<Vector2>();
        foreach(var item in pieceLocation)
        {
            if(item.Value.name[0] == 'w' && item.Value.name.IndexOf("wking")  == -1)
            {
                white_moves.AddRange(item.Value.get_moves());
            }
        }
        remove_highlights();
    }
}
