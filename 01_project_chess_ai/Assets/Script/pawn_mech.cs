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
    }
    // Update is called once per frame
    void Update()
    {
        if(!alive)
        	DestroyImmediate(this.gameObject);
    	if(!GameManager.instance.playersTurn == is_player) return;
    	if(Input.GetMouseButtonDown(0))
        	onClick();
        if(selected)
        	transform.position = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10f));
    }
    
    private void onClick()
    {
    	//on click, it positions itself in the grid and ends turn
		Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		RaycastHit2D select = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);
		gridPos = base.mouseToGrid(Input.mousePosition.x, Input.mousePosition.y);
        float deltax = gridPos.x-priorPos.x;
        float deltay = gridPos.y-priorPos.y;
        bool refresh = false;
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
			if(((transform.position.y < 0 || transform.position.y > 7) && (transform.position.x < 0 || transform.position.x > 7)) ||
				(GameManager.instance.playersTurn && deltay < 0) || (!GameManager.instance.playersTurn && deltay > 0) ||
				(!firstMove && Mathf.Abs(deltay) == 2))
			{
				Debug.Log("R1");
				GameManager.instance.reset_piece = true;
				transform.position = priorPos;
			}
			else if(Mathf.Abs(deltax) == 1 && Mathf.Abs(deltay) == 1 &&
				GameManager.occupiedSpots[new Vector2(priorPos.x+deltax, priorPos.y+deltay)])
			{
				Debug.Log("A");
	    		if(GameManager.pieceLocation[new Vector2(priorPos.x+deltax, priorPos.y+deltay)].player_check() == is_player)
	    			refresh = true;
	    		else
	    		{
					base.capture(gridPos);
			    	if(firstMove)
    					firstMove = false;
					move_piece(deltax,deltay,gridPos);
	    		}
			}
			else if(new List<float>{1, 2}.Contains(Mathf.Abs(deltay)) &&
				(!GameManager.occupiedSpots[gridPos] && deltax == 0))
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
			if(refresh)
			{
				Debug.Log("R2");
				GameManager.instance.reset_piece = true;
				transform.position = priorPos;
			}
			land_piece_set();
		}
    }

    //deals with moving into an empty square, will only be called if outside parameters are correct
    private void move_piece(float x, float y, Vector2 move_here)
    {
    	if(x == 0 || x == 1 || x == -1)
		{
			//enPassant
	    	if(y == 2 && firstMove && GameManager.occupiedSpots[new Vector2(move_here.x, move_here.y-1)])
	    	{
	    		if(!GameManager.pieceLocation[new Vector2(move_here.x, move_here.y-1)].player_check())
 				{
 					Vector2 remove_this = new Vector2(move_here.x, move_here.y-1);
     				base.capture(remove_this);
     			}
	    	}
			transform.position = move_here;
	    	GameManager.pieceLocation.Remove(priorPos);
			GameManager.occupiedSpots[priorPos] = false;
			priorPos = new Vector2(transform.position.x, transform.position.y);
			GameManager.pieceLocation.Add(priorPos,this);
	    	if(firstMove)
				firstMove = false;
			GameManager.instance.playersTurn = !GameManager.instance.playersTurn;
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
    public override bool player_check()
    {
        return is_player;
    }
}
