﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pawn_mech : MonoBehaviour
{
	private bool selected = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
    	if(Input.GetMouseButtonDown(0))
        	onClick();
        if(selected)
        	move();
    }
    private void onClick()
    {
		Vector2 mousePos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
		RaycastHit2D select = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(mousePos), Vector2.zero);
		if(select && select.transform.gameObject.tag == "Piece")
		{
			if(selected)
			{
				selected = false;	
				Vector3 gridPos = new Vector3(Mathf.Round(Input.mousePosition.x), Mathf.Round(Input.mousePosition.y), 10f);
				transform.position = Camera.main.ScreenToWorldPoint(gridPos);
				transform.position = new Vector3(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y),10f);
			}
			else
				selected = true;
		}

    }
    private void move()
    {
    	Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
    	mousePos.z += 10;
    	transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }
}
