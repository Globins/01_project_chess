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
    		if(GameManager.pieceLocation[move_here].player_check() == GameManager.pieceLocation[old_pos].player_check())
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
        GameManager.instance.playersTurn = !GameManager.instance.playersTurn;
		return new_pos;
    }
    //Creates bounds for rook, queen and bishop
    public virtual float new_bound(float xdir, float ydir, bool xchange, bool ychange, Vector2 old_pos)
    {
        float newb = 0;
        float ysign = 1;
        float xsign = 1;
        if(ychange == false)
            ysign = -1;
        if(xchange == false)
            xsign = -1;
        while(GameManager.occupiedSpots.ContainsKey(new Vector2(old_pos.x+(xdir*xsign), old_pos.y+(ydir*ysign))))
        {
            if(!GameManager.occupiedSpots[new Vector2(old_pos.x+(xdir*xsign), old_pos.y+(ydir*ysign))])
            {
                if(xdir == ydir)
                {
                    xdir++;
                    ydir++;
                }
                else if(xdir == 0)
                    ydir++;
                else
                    xdir++;
            }
            else
            {
                if(GameManager.pieceLocation[new Vector2(old_pos.x+(xdir*xsign), old_pos.y+(ydir*ysign))].player_check() != 
                	GameManager.pieceLocation[old_pos].player_check())
                {
                    if(xdir == ydir)
                    {
                        xdir++;
                        ydir++;
                    }
                    else if(xdir == 0)
                        ydir++;
                    else
                        xdir++;
                }
                break;
            }
        }
        if(xdir == 0)
            newb = ydir;
        else
            newb = xdir;
        if((old_pos.x == 7 || old_pos.x == 0) && newb == 1)
            newb = 0;
        else if((old_pos.y == 7 || old_pos.y == 0) && newb == 1)
            newb = 0;
        else
        {
            if(ydir != 0 && ydir != xdir)
            {
                if(ychange)
                    newb += ysign*-1;
                else
                    newb -= ysign*-1;
            }
            if(xdir != 0)
            {
                if(xchange)
                    newb += xsign*-1;
                else
                    newb -= xsign*-1;
            }
        }
        return newb;
    }
}
