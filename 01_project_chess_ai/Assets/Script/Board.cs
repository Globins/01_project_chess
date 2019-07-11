using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
	private SpriteRenderer thisPiece;
	public Sprite test;
	public Sprite test2;
	private Sprite defaultspr;
	public bool occupied = false;
	private Vector2 currentPos;
	private Vector2 gridPos;
	private bool clicked = false;

    void Start()
    {
		currentPos = new Vector2(transform.position.x, transform.position.y);
        if(currentPos.y == 0 || currentPos.y == 1 || currentPos.y == 6 || currentPos.y == 7)
        {
        	occupied = true;
        }

        GameManager.occupiedSpots.Add(currentPos,occupied);
        thisPiece = GetComponent<SpriteRenderer>();
        defaultspr = thisPiece.sprite;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 first = new Vector3(Mathf.Round(Input.mousePosition.x), Mathf.Round(Input.mousePosition.y), 10f);
        Vector3 gridPos = Camera.main.ScreenToWorldPoint(first);
        gridPos = new Vector3(Mathf.Round(gridPos.x),Mathf.Round(gridPos.y),0);
        float deltax = gridPos.x-currentPos.x;
        float deltay = gridPos.y-currentPos.y;


        if(GameManager.instance.reset_piece && clicked)
        {
        	GameManager.occupiedSpots[new Vector2(currentPos.x, currentPos.y)] = true;
        	clicked = false;
        }
        else if(Input.GetMouseButtonDown(0))
        {
        	if(clicked) //To reset when the player presses on a different tile
        		clicked = false;
        }

		if(GameManager.occupiedSpots[new Vector2(currentPos.x, currentPos.y)])
    	{
    		thisPiece.sprite = test;
    	}
    	else if(clicked || !GameManager.occupiedSpots[new Vector2(currentPos.x, currentPos.y)])
    	{
    		thisPiece.sprite = test2;
    	}
    	else
    	{
    		thisPiece.sprite = defaultspr;
    	}
    }
}
