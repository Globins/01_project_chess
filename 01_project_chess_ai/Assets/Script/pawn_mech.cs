using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Checklist:
//Implement Attacked: if an object is there and its a valid attack, then it replaces that object's position.
//Implement blocked: if an object is there in valid movement, then it returns piece to original spot
//GET COLLISION TO WORK




//Restrict to edge of board
//Add function where it can turn into any lost pieces when it edges the edge

public class pawn_mech : MonoBehaviour
{
	private bool selected = false;
	private SpriteRenderer thisPiece;
	private Vector2 currentPos;
	private bool firstMove = true;
	private bool playersTurn = true; //Temporary, will be put in game instance

    // Start is called before the first frame update
    void Start()
    {
        thisPiece = GetComponent<SpriteRenderer>();
        currentPos = new Vector2(transform.position.x, transform.position.y);
    }

    // Update is called once per frame
    void Update()
    {
    	if(Input.GetMouseButtonDown(0))
    	{
        	onClick();
    	}
        if(selected)
        	move();
    }
    
    private void onClick()
    {
		Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		RaycastHit2D select = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);

		//finds current grid position and sets temp at that location, sets up calculations for if valid move calculations.
		Vector3 gridPos = new Vector3(Mathf.Round(Input.mousePosition.x), Mathf.Round(Input.mousePosition.y), 10f);
		Vector3 temp = Camera.main.ScreenToWorldPoint(gridPos);
		temp = new Vector3(Mathf.Round(temp.x),Mathf.Round(temp.y),0);
		float deltax = temp.x-currentPos.x;
		float deltay = temp.y-currentPos.y;

		//on click, it positions itself in the grid and ends turn
		if(select && select.transform.gameObject.tag == "Piece")
		{
			if(!selected && Mathf.Abs(deltay) == 0 && Mathf.Abs(deltax) == 0) //if the object wasnt selected, it becomes selected and follows mouse
			{
				selected = true;
				gameObject.layer = 8;
				thisPiece.sortingLayerName = "Highlight";
			}
			else if(selected && deltax == 0 && deltay != 0 && Mathf.Abs(deltay) <= 2) 
			{
				gameObject.layer = 9;
				thisPiece.sortingLayerName = "Piece";
				selected = false;
				if((playersTurn && deltay < 0) || (!playersTurn && deltay > 0) || (Mathf.Abs(deltay) == 2 && !firstMove))
					{
						transform.position = currentPos;
					}
				else
				{
					transform.position = temp;
					currentPos = new Vector2(transform.position.x, transform.position.y);
					firstMove = false;
				}
			}
			else //if click on any invalid spot it returns the pieces to its original place
			{
				transform.position = currentPos;
				selected = false;
				gameObject.layer = 9;
				thisPiece.sortingLayerName = "Piece";
			}
		}

    }
    private void move()
    {
    	Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
    	mousePos.z += 10;
    	transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

	void OnCollisionEnter2D(Collision2D other)
	{
		if(other.gameObject.tag == "Piece" && gameObject.layer == other.gameObject.layer)
		{
			Debug.Log("Annoy");
			Destroy(other.gameObject);
		}
	}
}
