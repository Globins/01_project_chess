using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checklist:

//Add function where it can turn into any lost pieces when it edges the edge

public class pawn_mech : MonoBehaviour
{
	private SpriteRenderer thisPiece;
	private Vector2 priorPos;
	private Vector2 gridPos;
	private bool selected = false;
	private bool firstMove = true;

    // Start is called before the first frame update
    void Start()
    {
        thisPiece = GetComponent<SpriteRenderer>();
        priorPos = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
    	if(!GameManager.instance.playersTurn) return;

    	if(Input.GetMouseButtonDown(0))
        	onClick();
        if(selected)
        	moveWithMouse();
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
			!GameManager.instance.hasPieceInHand && GameManager.instance.pickDelay == 0)
		{
			pickUpPiece();
		}

		else if(selected)
		{

/*			if(((gridPos.y < 0 && gridPos.y > 7) && (gridPos.x < 0 && gridPos.x > 7)) ||
				(GameManager.instance.playersTurn && deltay < 0) || 
				(!GameManager.instance.playersTurn && deltay > 0))
			{
				Debug.Log("reset");
				transform.position = currentPos;
			}
			else if(Mathf.Abs(deltax) == 1 && Mathf.Abs(deltay) == 1 && that_tile_is_occupied)
			{
				capture(tile_x,tile_y,tile object); //removes the object there and makes tile unoccupied
				move_piece(x,y,gridPos)
			}
			else if(new []{1, 2}.Contains(Mathf.Abs(deltay)) && that_tile_is_not_occupied)
			{
				move_piece(x,y,gridPos); //places object there
			}
			else
			{
				Debug.Log("reset");
				transform.position = currentPos;
			}
			land_piece_set();
			//((temp.y >= 0 && temp.y <= 7) && (temp.x >= 0 && temp.x <= 7)) board bounds
			//(GameManager.instance.playersTurn && deltaPos.y < 0) || (!GameManager.instance.playersTurn && deltaPos.y > 0) 
			*/
		}
    }

    //deals with capturing
    private void capture(float x, float y)
    {
    	//get object thats there, delete it
    	Debug.Log("Attack");
    	if(firstMove)
    		firstMove = false;
    }

    //deals with moving into an empty square, will only be called if outside parameters are correct
    private void move_piece(float x, float y, Vector2 gridPos)
    {
    	if(x == 0 || x == 1)
		{
			//enPassant
	    	if(y == 2 && firstMove)
	    	{
	    		//if that square is occupied add tile object
	    			capture(x, y-1);
	    	}
			Debug.Log("Move");
			transform.position = gridPos;
			priorPos = new Vector2(transform.position.x, transform.position.y);
	    	if(firstMove)
				firstMove = false;
			land_piece_set();
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
		gameObject.layer = 9;
		thisPiece.sortingLayerName = "Piece";
		GameManager.instance.hasPieceInHand = false;
		selected = false;
    }

/*    //Try to remove the functions below.
    private void refresh()
    {
    	if(validAttack)
    	{
    		Destroy(cancer.gameObject);
    		validAttack = false;
    	}
    	isTouching = false;
		transform.position = currentPos;
		priorPos = currentPos;
		selected = false;
		gameObject.layer = 9;
		thisPiece.sortingLayerName = "Piece";
    }

	void OnCollisionEnter2D(Collision2D other)
	{
		isTouching = true;
		cancer = other;
		if(other.gameObject.tag == "Piece" && gameObject.layer == other.gameObject.layer && validAttack)
		{
			Debug.Log("OI");
			validAttack = false;
			Destroy(other.gameObject);
		}
		else
		{
			currentPos = priorPos;
		}
	}
	void OnCollisionExit2D(Collision2D other)
	{		
		isTouching = false;
	}*/
}
