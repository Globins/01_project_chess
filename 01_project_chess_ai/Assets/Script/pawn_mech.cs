using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pawn_mech : MonoBehaviour
{
	private bool selected = false;
	private SpriteRenderer thisPiece;
	private Vector2 currentPos;
	private bool firstMove = true;
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
		//Fix, teleports to any object labeled piece
		if(select && select.transform.gameObject.tag == "Piece")
		{
			Vector3 gridPos = new Vector3(Mathf.Round(Input.mousePosition.x), Mathf.Round(Input.mousePosition.y), 10f);
			Vector3 temp = Camera.main.ScreenToWorldPoint(gridPos);
			temp = new Vector3(Mathf.Round(transform.position.x),Mathf.Round(transform.position.y),0);
			float deltax = temp.x-currentPos.x;
			float deltay = temp.y-currentPos.y;

			if(selected && deltax == 0 && deltay > 0 && deltay <= 2)
			{
				if(deltay == 2 && !firstMove)
				{

				}
				else
				{				
					transform.position = temp;
					currentPos = new Vector2(transform.position.x, transform.position.y);
					firstMove = false;
					selected = false;
					gameObject.layer = 9;
					thisPiece.sortingLayerName = "Piece";
				}
			}
			else
			{
				selected = true;
				gameObject.layer = 8;
				thisPiece.sortingLayerName = "Highlight";
			}
		}

    }
    private void move()
    {
    	Vector3 mousePos = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f);
    	mousePos.z += 10;
    	transform.position = Camera.main.ScreenToWorldPoint(mousePos);
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
    	Debug.Log("Ya hit me boi");
        Destroy(collision.collider.gameObject);
    }
}
