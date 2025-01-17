﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pawn_mech : Piece
{
	private SpriteRenderer thisPiece;
	private Vector2 priorPos;
	private Vector2 gridPos;
	private List<Vector2> total_moves;
	private bool selected = false;
	private bool firstMove = true;
	private bool alive = true;
	private bool is_player = false;
	private bool en_passant = false;
	private bool kill_piece_behind = false;

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
			{
				transform.position = move_piece(priorPos, gridPos);
				if(firstMove)
				{
					firstMove = false;
				}
				if(kill_piece_behind)
				{
					base.capture(new Vector2(transform.position.x, transform.position.y-1));
					kill_piece_behind = false;
				}
			}
			else
				refresh_piece();
			GameManager.instance.remove_highlights();
			land_piece_set();
			if(priorPos.y == 7 || priorPos.y == 0)
				pawn_promotion();
		}
    }

    public override List<Vector2> get_moves()
    {
    	if(GameManager.instance.isPlayerTurn == is_player)
    		en_passant = false;
        List<Vector2> moves = new List<Vector2>();
        float ydir = 1;
        if(!is_player)
        	ydir = -1;
        //Basic Moves
		if(GameManager.occupiedSpots.ContainsKey(new Vector2(priorPos.x, priorPos.y+ydir)))
			if(!GameManager.occupiedSpots[new Vector2(priorPos.x, priorPos.y+ydir)])
			{
				moves.Add(new Vector2(priorPos.x, priorPos.y+ydir));
				GameManager.instance.show_highlight(new Vector2(priorPos.x, priorPos.y+ydir), true);
			}

		if(GameManager.occupiedSpots.ContainsKey(new Vector2(priorPos.x-1, priorPos.y+ydir)))
			if(GameManager.occupiedSpots[new Vector2(priorPos.x-1, priorPos.y+ydir)])
				if(GameManager.pieceLocation[new Vector2(priorPos.x-1, priorPos.y+ydir)].player_check() != is_player)
				{
					moves.Add(new Vector2(priorPos.x-1, priorPos.y+ydir));
					GameManager.instance.show_highlight(new Vector2(priorPos.x-1, priorPos.y+ydir), false);
				}

		if(GameManager.occupiedSpots.ContainsKey(new Vector2(priorPos.x+1, priorPos.y+ydir)))
			if(GameManager.occupiedSpots[new Vector2(priorPos.x+1, priorPos.y+ydir)])
				if(GameManager.pieceLocation[new Vector2(priorPos.x+1, priorPos.y+ydir)].player_check() != is_player)
				{
					moves.Add(new Vector2(priorPos.x+1, priorPos.y+ydir));
					GameManager.instance.show_highlight(new Vector2(priorPos.x+1, priorPos.y+ydir), false);
				}

		//En passant moves
		if(firstMove)
		{
			if(!GameManager.occupiedSpots[new Vector2(priorPos.x, priorPos.y+ydir*2)])
			{
				if(GameManager.occupiedSpots[new Vector2(priorPos.x, priorPos.y+ydir)])
				{
					if(GameManager.pieceLocation[new Vector2(priorPos.x, priorPos.y+ydir)].player_check() != is_player)
					{
						moves.Add(new Vector2(priorPos.x, priorPos.y+ydir*2));
						GameManager.instance.show_highlight(new Vector2(priorPos.x, priorPos.y+ydir*2), true);
						kill_piece_behind = true;
						en_passant = true;
					}
				}
				else
				{
					moves.Add(new Vector2(priorPos.x, priorPos.y+ydir*2));
					GameManager.instance.show_highlight(new Vector2(priorPos.x, priorPos.y+ydir*2), true);
					en_passant = true;
				}
			}
		}
		if(priorPos.y == 3 && !is_player)
		{
		if(GameManager.occupiedSpots.ContainsKey(new Vector2(priorPos.x-1, priorPos.y)))
			if(GameManager.occupiedSpots[new Vector2(priorPos.x-1, priorPos.y)])
				if(GameManager.pieceLocation[new Vector2(priorPos.x-1, priorPos.y)].player_check() != is_player && GameManager.pieceLocation[new Vector2(priorPos.x-1, priorPos.y)].pawn_enpassant())
				{
					moves.Add(new Vector2(priorPos.x-1, priorPos.y+ydir));
					GameManager.instance.show_highlight(new Vector2(priorPos.x-1, priorPos.y+ydir), true);
					kill_piece_behind = true;
				}
		if(GameManager.occupiedSpots.ContainsKey(new Vector2(priorPos.x+1, priorPos.y)))
			if(GameManager.occupiedSpots[new Vector2(priorPos.x+1, priorPos.y)])
				if(GameManager.pieceLocation[new Vector2(priorPos.x+1, priorPos.y)].player_check() != is_player && GameManager.pieceLocation[new Vector2(priorPos.x+1, priorPos.y)].pawn_enpassant())
				{
					GameManager.instance.show_highlight(new Vector2(priorPos.x+1, priorPos.y+ydir), true);
					moves.Add(new Vector2(priorPos.x+1, priorPos.y+ydir));
					kill_piece_behind = true;
				}
		}
		if(priorPos.y == 4 && is_player)
		{
			if(GameManager.occupiedSpots.ContainsKey(new Vector2(priorPos.x-1, priorPos.y)))
				if(GameManager.occupiedSpots[new Vector2(priorPos.x-1, priorPos.y)])
					if(GameManager.pieceLocation[new Vector2(priorPos.x-1, priorPos.y)].player_check() != is_player && GameManager.pieceLocation[new Vector2(priorPos.x-1, priorPos.y)].pawn_enpassant())
					{
						GameManager.instance.show_highlight(new Vector2(priorPos.x-1, priorPos.y+ydir), true);
						moves.Add(new Vector2(priorPos.x-1, priorPos.y+ydir));
						kill_piece_behind = true;
					}
			if(GameManager.occupiedSpots.ContainsKey(new Vector2(priorPos.x+1, priorPos.y)))
				if(GameManager.occupiedSpots[new Vector2(priorPos.x+1, priorPos.y)])
					if(GameManager.pieceLocation[new Vector2(priorPos.x+1, priorPos.y)].player_check() != is_player && GameManager.pieceLocation[new Vector2(priorPos.x+1, priorPos.y)].pawn_enpassant())
					{
						GameManager.instance.show_highlight(new Vector2(priorPos.x+1, priorPos.y+ydir), true);
						moves.Add(new Vector2(priorPos.x+1, priorPos.y+ydir));
						kill_piece_behind = true;
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
		base.land_piece_set();
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
    public override bool pawn_enpassant()
    {
    	return en_passant;
    }

    private void pawn_promotion()
    {
    	transform.position = new Vector2(100, 100);
    	GameManager.instance.promote(priorPos);
        DestroyImmediate(this.gameObject);
    }
}
