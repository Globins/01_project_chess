using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	public bool occupied = false;
	private Vector2 currentPos;
	private Vector2 gridPos;
	private bool clicked = false;

    void Start()
    {
		currentPos = new Vector2(transform.position.x, transform.position.y);
        if(currentPos.y == 0 || currentPos.y == 1 || currentPos.y == 8 || currentPos.y == 7)
        {
        	occupied = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
    	gridPos = GameManager.instance.mouseToGrid(Input.mousePosition.x, Input.mousePosition.y, currentPos);
        float deltax = gridPos.x-currentPos.x;
        float deltay = gridPos.y-currentPos.y;
        if(GameManager.instance.reset_piece && clicked)
        {
        	occupied = true;
        	clicked = false;
        }
        else if(Input.GetMouseButtonDown(0))
        {
        	if(clicked) //To reset when the player presses on a different tile
        		clicked = false;
        	if(deltax == 0 && deltay == 0 && occupied)
        	{
	        	occupied = false;
        		clicked = true;
        	}
        }
    }
}
