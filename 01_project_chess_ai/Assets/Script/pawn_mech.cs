using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checklist:
//Figure out collision glitch where objects are not registering the collision until a reset

//Add function where it can turn into any lost pieces when it edges the edge

public class pawn_mech : MonoBehaviour
{
	private bool selected = false;
	private SpriteRenderer thisPiece;
	private Vector2 currentPos;
	private Vector2 priorPos;
	private bool firstMove = true;
	private bool validAttack = false;
	private bool isTouching = false;

    // Start is called before the first frame update
    void Start()
    {
        thisPiece = GetComponent<SpriteRenderer>();
        currentPos = new Vector2(transform.position.x, transform.position.y);
        priorPos = currentPos;
    }

    // Update is called once per frame
    void Update()
    {
    	if(Input.GetMouseButtonDown(0))
        	onClick();
        if(selected)
        	move();
        if(!selected)
        	refresh();
    }
    
    private void onClick()
    {
    	//on click, it positions itself in the grid and ends turn
		Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		RaycastHit2D select = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);

		//finds current grid position and sets temp at that location, sets up calculations for if valid move calculations.
		Vector3 gridPos = new Vector3(Mathf.Round(Input.mousePosition.x), Mathf.Round(Input.mousePosition.y), 10f);
		Vector3 temp = Camera.main.ScreenToWorldPoint(gridPos);
		temp = new Vector3(Mathf.Round(temp.x),Mathf.Round(temp.y),0);
		float deltax = temp.x-currentPos.x;
		float deltay = temp.y-currentPos.y;
		//if the object wasnt selected, it becomes selected and follows mouse
		if(select && select.transform.gameObject.tag == "Piece" &&
			Mathf.Abs(deltay) == 0 && Mathf.Abs(deltax) == 0 && !GameManager.instance.hasPieceInHand)
		{
			priorPos = currentPos;
			selected = true;
			gameObject.layer = 8;
			thisPiece.sortingLayerName = "Highlight";
			GameManager.instance.hasPieceInHand = true;
		}

		else if(selected)
		{
			if(deltay != 0 && Mathf.Abs(deltay) <= 2 && (Mathf.Abs(deltax) == 1 || Mathf.Abs(deltax) == 0) && 
				((temp.y >= 0 && temp.y <= 7) && (temp.x >= 0 && temp.x <= 7)))
			{
								//Invalid move within correct bounds
				if((GameManager.instance.playersTurn && deltay < 0) ||
					(!GameManager.instance.playersTurn && deltay > 0) ||
					(Mathf.Abs(deltay) == 2 && !firstMove) || (isTouching && Mathf.Abs(deltax) != 1))
				{
					transform.position = currentPos;
				}

				//Attack Move fix so it cant move unless a piece is there
				else if(Mathf.Abs(deltax) == 1 && Mathf.Abs(deltay) == 1 && isTouching)
				{
					transform.position = temp;
					currentPos = new Vector2(transform.position.x, transform.position.y);
					firstMove = false;
					validAttack = true;
					isTouching = false;
				}
				//valid move
				else
				{
					transform.position = temp;
					currentPos = new Vector2(transform.position.x, transform.position.y);
					firstMove = false;
				}	
			}
			 //if click on any invalid spot it returns the pieces to its original place
			else
			{
				transform.position = currentPos;
			}
			//resets the piece to being still after click
			gameObject.layer = 9;
			thisPiece.sortingLayerName = "Piece";
			GameManager.instance.hasPieceInHand = false;
			selected = false;
		}
    }

    private void move()
    {
    	Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
    	mousePos.z += 10;
    	transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    private void refresh()
    {
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
		Debug.Log("hehe");
		if(other.gameObject.tag == "Piece" && gameObject.layer == other.gameObject.layer && validAttack)
		{
			Debug.Log("OI");
			validAttack = false;
			Destroy(other.	gameObject);
		}
		else
		{
			currentPos = priorPos;
		}
	}
	void OnCollisionExit2D(Collision2D other)
	{		
		isTouching = false;
	}
}
