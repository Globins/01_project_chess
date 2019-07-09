﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checklist:

//Add function where it can turn into any lost pieces when it edges the edge

public class rook_mech : Piece
{
	private SpriteRenderer thisPiece;
	private Vector2 priorPos;
	private Vector2 gridPos;
	private bool selected = false;
	private bool refreshOnStart = true;
	private bool alive = true;
	private bool is_player = false;
	private float yneg = 0;
    private float xpos = 0;
    private float xneg = 0;
    private float ypos = 0;

    // Start is called before the first frame update
    void Start()
    {
        thisPiece = GetComponent<SpriteRenderer>();
        priorPos = new Vector2(transform.position.x, transform.position.y);
        GameManager.pieceLocation.Add(priorPos,this);
        if(priorPos.y == 0 || priorPos.y == 1)
        {
        	is_player = true;
        }
    }
    // Update is called once per frame
    void Update()
    {
        if(!alive)
        {
        	DestroyImmediate(this.gameObject);
        }
    	if(!GameManager.instance.playersTurn) return;
    	if(Input.GetMouseButtonDown(0))
        	onClick();
        if(selected)
        	moveWithMouse();
        if(refreshOnStart)
        {
        	GameManager.instance.reset_piece = true;
			transform.position = priorPos;
        	land_piece_set();
        	refreshOnStart = false;
        }
    }
    
    private void onClick()
    {
    	//on click, it positions itself in the grid and ends turn
		Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		RaycastHit2D select = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);
		gridPos = GameManager.instance.mouseToGrid(Input.mousePosition.x, Input.mousePosition.y, priorPos);
        float deltax = gridPos.x-priorPos.x;
        float deltay = gridPos.y-priorPos.y;
        //create boundary for the delta so it cant move past an object
        create_bounds();
        Debug.Log(xpos + " " + ypos + " " + xneg + " " + yneg);
		//if the object wasnt selected, it becomes selected and follows mouse
		if(select && select.transform.gameObject.tag == "Piece" &&
			Mathf.Abs(deltay) == 0 && Mathf.Abs(deltax) == 0 && 
			!GameManager.instance.hasPieceInHand &&
			GameManager.pieceLocation.ContainsKey(priorPos))
		{
			Debug.Log("P");
			pickUpPiece();
		}

		else if(selected)
		{
			//Error
			if((transform.position.y < 0 || transform.position.y > 7) && (transform.position.x < 0 || transform.position.x > 7))
			{
				Debug.Log("R1");
				GameManager.instance.reset_piece = true;
				transform.position = priorPos;
			}
			else if((Mathf.Abs(deltax) == 0 || Mathf.Abs(deltay) == 0) &&
                    ((deltax <= xpos && deltax >= xneg) && (deltay <= ypos && deltay >= yneg)))
			{
				Debug.Log("M");
				move_piece(deltax,deltay,gridPos); //places object there
			}
			//Default case
			else
			{
				Debug.Log("R2");
				GameManager.instance.reset_piece = true;
				transform.position = priorPos;
			}
            xpos = xneg = ypos = yneg = 0;
			land_piece_set();
		}
    }

    //deals with capturing
    private void capture(Vector2 remove_object_here)
    {
    	//get object thats there, delete it
    	GameManager.pieceLocation[remove_object_here].kill();
    	GameManager.pieceLocation.Remove(remove_object_here);
    	GameManager.occupiedSpots[remove_object_here] = false;
    }

    //deals with moving into an empty square, will only be called if outside parameters are correct
    private void move_piece(float x, float y, Vector2 move_here)
    {
        bool refresh = false;
        if(GameManager.occupiedSpots[move_here])
        {
            if(GameManager.pieceLocation[move_here].player_check())
                refresh = true;
            else
                capture(move_here);
        }
        if(!refresh)
        {
            transform.position = move_here;
            GameManager.pieceLocation.Remove(priorPos);
            GameManager.occupiedSpots[priorPos] = false;
            priorPos = new Vector2(transform.position.x, transform.position.y);
            GameManager.pieceLocation.Add(priorPos,this);
        }
        else
        {
            Debug.Log("R2");
            GameManager.instance.reset_piece = true;
            transform.position = priorPos;
        }
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

    private void moveWithMouse()
    {
    	Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
    	mousePos.z += 10;
    	transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    //places the piece down
    private void land_piece_set()
    {
    	GameManager.occupiedSpots[new Vector2(transform.position.x, transform.position.y)] = true;
		gameObject.layer = 9;
		thisPiece.sortingLayerName = "Piece";
		GameManager.instance.hasPieceInHand = false;
		selected = false;
    }
    private void create_bounds()
    {
/*        for(float i = priorPos.y+1; i < 8; i++)
        {
            if(!GameManager.occupiedSpots[new Vector2(priorPos.x, i)])
            {
                ypos++;
            }
            else
            {
                if(GameManager.pieceLocation[new Vector2(priorPos.x, i)].player_check() != is_player)
                {
                    ypos++;
                    break;
                }
            }
        }
        for(float i = priorPos.x+1; i < 8; i++)
        {
            if(!GameManager.occupiedSpots[new Vector2(i, priorPos.y)])
            {
                xpos++;
            }
            else
            {
                if(GameManager.pieceLocation[new Vector2(i, priorPos.y)].player_check()!= is_player)
                {
                    xpos++;
                    break;
                }
            }
        }*/
/*        for(float i = priorPos.y; i > -1; i--)
        {
            if(GameManager.occupiedSpots[new Vector2(priorPos.x, i)])
            {
                yneg--;
            }
            else
            {
                if(GameManager.pieceLocation[new Vector2(priorPos.x, i)].player_check() != is_player)
                {
                    yneg--;
                    break;
                }

            }
        }
        for(float i = priorPos.y; i > -1; i--)
        {
            if(!GameManager.occupiedSpots[new Vector2(i, priorPos.y)])
            {
                xneg--;
            }
            else
            {
                if(GameManager.pieceLocation[new Vector2(i, priorPos.y)].player_check() != is_player)
                {
                    xneg--;
                    break;
                }
            }
        }*/
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
