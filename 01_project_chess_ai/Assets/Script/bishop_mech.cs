using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checklist:

//Add function where it can turn into any lost pieces when it edges the edge

public class bishop_mech : Piece
{
    private SpriteRenderer thisPiece;
    private Vector2 priorPos;
    private Vector2 gridPos;
    private bool selected = false;
    private bool alive = true;
    private bool is_player = false;
    private List<Vector2> total_moves;

    // Start is called before the first frame update
    void Start()
    {
        thisPiece = GetComponent<SpriteRenderer>();
        priorPos = new Vector2(transform.position.x, transform.position.y);
        GameManager.pieceLocation.Add(priorPos,this);
        if(priorPos.y == 0 || priorPos.y == 1)
            is_player = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(!alive)
            DestroyImmediate(this.gameObject);
        if(!GameManager.instance.isPlayerTurn == is_player) return;
        if(Input.GetMouseButtonDown(0) && alive)
            onClick();
        if(selected)
            transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
    }
    
    private void onClick()
    {
        //on click, it positions itself in the grid and ends turn
        Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        RaycastHit2D select = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);
        gridPos = base.mouseToGrid(mousePos);
        if(select && select.transform.gameObject.tag == "Piece" && !GameManager.instance.hasPieceInHand && priorPos == gridPos)
            pickUpPiece();
        else if(selected)
        {
            total_moves = get_moves();
            if(total_moves.Contains(gridPos))
                transform.position = move_piece(priorPos, gridPos);
            else
                refresh_piece();
            land_piece_set();
        }
    }

    private List<Vector2> get_moves()
    {
        List<Vector2> moves = base.diagonal_bounds(priorPos);
        return moves;
    }

    //picks up the piece if it wasnt selected
    private void pickUpPiece()
    {
        priorPos = transform.position;
        selected = true;
        gameObject.layer = 8;
        thisPiece.sortingLayerName = "Highlight";
        GameManager.instance.hasPieceInHand = true;
    }

    //places the piece down
    public override void land_piece_set()
    {
        GameManager.occupiedSpots[transform.position] = true;
        priorPos = transform.position;
        gameObject.layer = 9;
        thisPiece.sortingLayerName = "Piece";
        GameManager.instance.hasPieceInHand = false;
        selected = false;
    }

    private void refresh_piece()
    {
        GameManager.instance.reset_piece = true;
        transform.position = priorPos;
    }

    //kills the object.
    public override void kill()
    {
        alive = false;
    }
    public override bool player_check()
    {
        return is_player;
    }
}
