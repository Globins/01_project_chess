using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;
    private BoardManager boardScript;
    public static Dictionary<Vector2, bool> occupiedSpots = new Dictionary<Vector2, bool>();
    public static Dictionary<Vector2, bool> pieceLocation = new Dictionary<Vector2, bool>();
    public bool hasPieceInHand = false;
    public static int pickDelay = 0;
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
        if(pickDelay > 0)
        {
            pickDelay--;
        }
        reset_piece = false;
    }

    public Vector2 mouseToGrid(float x, float y, Vector2 current)
    {
        Vector3 first = new Vector3(Mathf.Round(x), Mathf.Round(y), 10f);
        Vector3 gridPos = Camera.main.ScreenToWorldPoint(first);
        gridPos = new Vector3(Mathf.Round(gridPos.x),Mathf.Round(gridPos.y),0);
        return gridPos;
    }
}
