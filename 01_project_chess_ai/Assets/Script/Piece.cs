using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//Move pickuppiece and land_piece_set into this abstract
//pawn will override some functions
public abstract class Piece : MonoBehaviour
{

    public virtual void capture(Vector2 remove_object_here)
    {
    	//get object thats there, delete it
    	GameManager.pieceLocation[remove_object_here].kill();
    	GameManager.pieceLocation.Remove(remove_object_here);
    	GameManager.occupiedSpots[remove_object_here] = false;
    }
	public virtual void kill()
	{
		Debug.Log("Broke");
	}
	public virtual bool player_check()
	{
		return false;
	}
    public virtual Vector2 mouseToGrid(float x, float y)
    {
        Vector3 first = new Vector3(Mathf.Round(x), Mathf.Round(y), 10f);
        Vector3 gridPos = Camera.main.ScreenToWorldPoint(first);
        gridPos = new Vector3(Mathf.Round(gridPos.x),Mathf.Round(gridPos.y),0);
        return gridPos;
    }
    public virtual Vector2 move_piece(float x, float y, Vector2 move_here, Vector2 old_pos)
    {
    	bool refresh = false;
    	Vector2 new_pos = old_pos;
    	if(GameManager.occupiedSpots[move_here])
    	{
    		if(GameManager.pieceLocation[move_here].player_check())
    			refresh = true;
    		else
				capture(move_here);
    	}
    	if(!refresh)
		{
			new_pos = move_here;
	    	GameManager.pieceLocation.Remove(old_pos);
			GameManager.occupiedSpots[old_pos] = false;
			GameManager.pieceLocation.Add(new_pos,this);
		}
		else
		{
			Debug.Log("R2");
			GameManager.instance.reset_piece = true;
		}
		return new_pos;
    }
}
