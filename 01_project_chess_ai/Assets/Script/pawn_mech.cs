using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checklist:

//Add function where it can turn into any lost pieces when it edges the edge

public class pawn_mech : Piece
{
	private SpriteRenderer thisPiece;
	private Vector2 priorPos;
	private Vector2 gridPos;
	private bool selected = false;
	private bool firstMove = true;
	private bool refreshOnStart = true;
	private bool alive = true;

    // Start is called before the first frame update
    void Start()
    {
        thisPiece = GetComponent<SpriteRenderer>();
        priorPos = new Vector2(transform.position.x, transform.position.y);
        GameManager.pieceLocation.Add(priorPos,this);
    }
    // Update is called once per frame
    void Update()
    {
    	//Debug.Log(GameManager.pieceLocation.Count);
        if(!alive)
        {
        	Debug.Log("IM DED");
        	DestroyImmediate(this);
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
		//if the object wasnt selected, it becomes selected and follows mouse
		if(select && select.transform.gameObject.tag == "Piece" &&
			Mathf.Abs(deltay) == 0 && Mathf.Abs(deltax) == 0 && 
			!GameManager.instance.hasPieceInHand)
		{
			pickUpPiece();
		}

		else if(selected)
		{
			//error in correct bounds
			if(((gridPos.y < 0 && gridPos.y > 7) && (gridPos.x < 0 && gridPos.x > 7)) ||
				(GameManager.instance.playersTurn && deltay < 0) || 
				(!GameManager.instance.playersTurn && deltay > 0) ||
				(!firstMove && Mathf.Abs(deltay) == 2))
			{
				Debug.Log("reset");
				GameManager.instance.reset_piece = true;
				transform.position = priorPos;
			}
			else if(Mathf.Abs(deltax) == 1 && Mathf.Abs(deltay) == 1 &&
				GameManager.occupiedSpots[new Vector2(priorPos.x+1, priorPos.y+1)])
			{
				Debug.Log("Meep");
				capture(gridPos); //removes the object there and makes tile unoccupied
				move_piece(deltax,deltay,gridPos);
			}
			else if(new List<float>{1, 2}.Contains(Mathf.Abs(deltay)) &&
				(!GameManager.occupiedSpots[new Vector2(gridPos.x, gridPos.y)] && deltax == 0))
			{
				Debug.Log("Meep2");
				move_piece(deltax,deltay,gridPos); //places object there
			}
			else
			{
				Debug.Log("reset");
				GameManager.instance.reset_piece = true;
				transform.position = priorPos;
			}
			land_piece_set();
			//((temp.y >= 0 && temp.y <= 7) && (temp.x >= 0 && temp.x <= 7)) board bounds
		}
    }

    //deals with capturing
    private void capture(Vector2 remove_object_here)
    {
    	//get object thats there, delete it
    	Debug.Log("Attack");
    	if(firstMove)
    		firstMove = false;
    	GameManager.pieceLocation[remove_object_here].kill();
    	GameManager.pieceLocation.Remove(remove_object_here);
    	GameManager.occupiedSpots[new Vector2(remove_object_here.x, remove_object_here.y)] = false;
    }

    //deals with moving into an empty square, will only be called if outside parameters are correct
    private void move_piece(float x, float y, Vector2 move_here)
    {
    	if(x == 0 || x == 1 || x == -1)
		{
			//enPassant
	    	if(y == 2 && firstMove && GameManager.occupiedSpots[new Vector2(move_here.x, move_here.y-1)])
	    	{
    			Vector2 remove_this = new Vector2(move_here.x, move_here.y-1);
    			capture(remove_this);
	    	}
			Debug.Log("Move");
			transform.position = move_here;
	    	GameManager.pieceLocation.Remove(priorPos);
			GameManager.occupiedSpots[new Vector2(priorPos.x, priorPos.y)] = false;
			priorPos = new Vector2(transform.position.x, transform.position.y);
			GameManager.pieceLocation.Add(priorPos,this);
			land_piece_set();
	    	if(firstMove)
				firstMove = false;
	    }
    }

    //picks up the piece if it wasnt selected
    private void pickUpPiece()
    {
		Debug.Log("Picked.");
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
    
    //kills the object.
    public override void kill()
    {
        alive = false;
    }
}
