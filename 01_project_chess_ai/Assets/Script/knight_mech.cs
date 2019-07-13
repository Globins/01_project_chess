using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knight_mech : Piece
{
	private SpriteRenderer thisPiece;
	private Vector2 priorPos;
	private Vector2 gridPos;
	private List<Vector2> total_moves;
	private bool selected = false;
	private bool alive = true;
	private bool is_player = false;

    // Start is called before the first frame update
    void Start()
    {
        thisPiece = GetComponent<SpriteRenderer>();
        priorPos = new Vector2(transform.position.x, transform.position.y);
        GameManager.pieceLocation.Add(priorPos,this);
        if(priorPos.y == 0 || priorPos.y == 1)
        	is_player = true;
       	if(GameManager.instance.gameStarted)
            is_player = !is_player;
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
		{
			pickUpPiece();
			total_moves = get_moves();
		}
		else if(selected)
		{
			if(total_moves.Contains(gridPos))
				transform.position = move_piece(priorPos, gridPos);
			else
				refresh_piece();
			GameManager.instance.remove_highlights();
			land_piece_set();
		}
    }

    private List<Vector2> get_moves()
    {
        List<Vector2> moves = new List<Vector2>();
        List<Vector2> spaces = new List<Vector2>();
        spaces.Add(new Vector2(priorPos.x-1, priorPos.y+2));
        spaces.Add(new Vector2(priorPos.x+1, priorPos.y+2));
        spaces.Add(new Vector2(priorPos.x-1, priorPos.y-2));
        spaces.Add(new Vector2(priorPos.x+1, priorPos.y-2));
        spaces.Add(new Vector2(priorPos.x+2, priorPos.y-1));
        spaces.Add(new Vector2(priorPos.x+2, priorPos.y+1));
        spaces.Add(new Vector2(priorPos.x-2, priorPos.y-1));
        spaces.Add(new Vector2(priorPos.x-2, priorPos.y+1));
        //Basic Moves
        for(int i = 0; i < spaces.Count; i++)
        {
        	Vector2 temp = spaces[i];
			if(GameManager.occupiedSpots.ContainsKey(temp))
			{
				if(GameManager.occupiedSpots[temp])
				{
					if(GameManager.pieceLocation[temp].player_check() != is_player)
					{
						moves.Add(temp);
						GameManager.instance.show_highlight(temp,false);
					}
				}
				else
				{
					moves.Add(temp);
					GameManager.instance.show_highlight(temp,true);
				}
			}
        }
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
