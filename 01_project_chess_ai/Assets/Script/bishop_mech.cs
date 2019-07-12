using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bishop_mech : Piece
{
	private SpriteRenderer thisPiece;
	private Vector2 priorPos;
	private Vector2 gridPos;
	private bool selected = false;
	private bool alive = true;
	private bool is_player = false;
	private float topleft = 0;
    private float topright = 0;
    private float botleft = 0;
    private float botright = 0;
    private bool xdir;
    private bool ydir;

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
        	DestroyImmediate(this.gameObject);
    	if(!GameManager.instance.playersTurn && is_player) return;
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
       	xdir = ydir = true;
        if(deltax < 0)
        	xdir = false;
        if(deltay < 0)
        	ydir = false;
        //create boundary for the delta so it cant move past an object
        topleft = topright = botright = botleft = 0;
        topleft = base.new_bound(1,1, false, true, priorPos);
        topright = base.new_bound(1,1, true, true, priorPos);
        botleft = base.new_bound(1,1, false, false, priorPos);
        botright = base.new_bound(1,1, true, false, priorPos);
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
			if((transform.position.y < 0 || transform.position.y > 7) && (transform.position.x < 0 || transform.position.x > 7) || gridPos == priorPos)
			{
				Debug.Log("R1");
				GameManager.instance.reset_piece = true;
				transform.position = priorPos;
			}
			else if((Mathf.Abs(deltax) == Mathf.Abs(deltay)) &&
					((xdir && ydir && Mathf.Abs(deltax) <= topright) ||
					(xdir && !ydir && Mathf.Abs(deltax) <= botright) ||
					(!xdir && ydir && Mathf.Abs(deltax) <= topleft) ||
					(!xdir && !ydir && Mathf.Abs(deltax) <= botleft)))
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
            topleft = topright = botright = botleft = 0;
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
