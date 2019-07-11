using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checklist:

//Add function where it can turn into any lost pieces when it edges the edge

public class queen_mech : Piece
{
	private SpriteRenderer thisPiece;
	private Vector2 priorPos;
	private Vector2 gridPos;
	private bool selected = false;
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
        	is_player = true;
    }
    // Update is called once per frame
    void Update()
    {
        if(!alive)
        	DestroyImmediate(this.gameObject);
    	if(!GameManager.instance.playersTurn) return;
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
        //create boundary for the delta so it cant move past an object
        xneg = xpos = yneg = ypos = 0;
        create_bounds();
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
				transform.position = base.move_piece(deltax, deltay, gridPos, priorPos); //places object there
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
        priorPos = new Vector2(transform.position.x, transform.position.y);
		gameObject.layer = 9;
		thisPiece.sortingLayerName = "Piece";
		GameManager.instance.hasPieceInHand = false;
		selected = false;
    }
    private void create_bounds()
    {
    	while(GameManager.occupiedSpots.ContainsKey(new Vector2(priorPos.x, priorPos.y+ypos+1)))
    	{
    		if(!GameManager.occupiedSpots[new Vector2(priorPos.x, priorPos.y+ypos+1)])
    		{
    			ypos++;
    		}
    		else
    		{
    			if(GameManager.pieceLocation[new Vector2(priorPos.x, priorPos.y+ypos+1)].player_check() != is_player)
    				ypos++;
    			break;
    		}
    	}
    	while(GameManager.occupiedSpots.ContainsKey(new Vector2(priorPos.x+xpos+1, priorPos.y)))
    	{
    		if(!GameManager.occupiedSpots[new Vector2(priorPos.x+xpos+1, priorPos.y)])
    		{
    			xpos++;
    		}
    		else
    		{
    			if(GameManager.pieceLocation[new Vector2(priorPos.x+xpos+1, priorPos.y)].player_check() != is_player)
    				xpos++;
    			break;
    		}
    	}

    	while(GameManager.occupiedSpots.ContainsKey(new Vector2(priorPos.x, priorPos.y+yneg-1)))
    	{
    		if(!GameManager.occupiedSpots[new Vector2(priorPos.x, priorPos.y+yneg-1)])
    		{
    			yneg--;
    		}
    		else
    		{
    			if(GameManager.pieceLocation[new Vector2(priorPos.x, priorPos.y+yneg-1)].player_check() != is_player)
    				yneg--;
    			break;
    		}
    	}
    	while(GameManager.occupiedSpots.ContainsKey(new Vector2(priorPos.x+xneg-1, priorPos.y)))
    	{
    		if(!GameManager.occupiedSpots[new Vector2(priorPos.x+xneg-1, priorPos.y)])
    		{
    			xneg--;
    		}
    		else
    		{
    			if(GameManager.pieceLocation[new Vector2(priorPos.x+xneg-1, priorPos.y)].player_check() != is_player)
    				xneg--;
    			break;
    		}
    	}
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
