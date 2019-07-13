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

    public virtual Vector2 move_piece(Vector2 old_pos, Vector2 move_here)
    {
        if(GameManager.occupiedSpots[move_here])
        {
            capture(move_here);
        }
        GameManager.pieceLocation.Remove(old_pos);
        GameManager.occupiedSpots[old_pos] = false;
        GameManager.pieceLocation.Add(move_here,this);
        GameManager.instance.isPlayerTurn = !GameManager.instance.isPlayerTurn;
        return move_here;
    }
    public virtual bool firstMoveCheck()
    {
        return false;
    }
	public virtual void kill()
	{
		Debug.Log("Broke");
	}
	public virtual bool player_check()
	{
		return false;
	}
    public virtual void land_piece_set()
    {
        Debug.Log("Broke");
    }
    public virtual bool pawn_enpassant()
    {
        return false;
    }
    public virtual Vector2 mouseToGrid(Vector2 location)
    {
        Vector3 first = new Vector3(Mathf.Round(location.x), Mathf.Round(location.y), 10f);
        Vector3 gridPos = Camera.main.ScreenToWorldPoint(first);
        gridPos = new Vector3(Mathf.Round(gridPos.x),Mathf.Round(gridPos.y),0);
        return gridPos;
    }

    //Creates bounds for rook, queen and bishop
    public virtual List<Vector2> horizontal_bounds(Vector2 old_pos)
    {
        List<Vector2> moves = new List<Vector2>();
        Vector2 temp = old_pos;
        temp.x += 1;
        while(GameManager.occupiedSpots.ContainsKey(temp))
        {
            if(!GameManager.occupiedSpots[temp])
                moves.Add(temp);
            else
            {
                if(GameManager.pieceLocation[temp].player_check() != GameManager.pieceLocation[old_pos].player_check())
                    moves.Add(temp);
                break;
            }
            temp.x += 1;
        }

        temp = old_pos;
        temp.x -= 1;
        while(GameManager.occupiedSpots.ContainsKey(temp))
        {
            if(!GameManager.occupiedSpots[temp])
                moves.Add(temp);
            else
            {
                if(GameManager.pieceLocation[temp].player_check() != GameManager.pieceLocation[old_pos].player_check())
                    moves.Add(temp);
                break;
            }
            temp.x -= 1;
        }
        return moves;
    }

    public virtual List<Vector2> vertical_bounds(Vector2 old_pos)
    {
        List<Vector2> moves = new List<Vector2>();
        Vector2 temp = old_pos;
        temp.y += 1;
        while(GameManager.occupiedSpots.ContainsKey(temp))
        {
            if(!GameManager.occupiedSpots[temp])
                moves.Add(temp);
            else
            {
                if(GameManager.pieceLocation[temp].player_check() != GameManager.pieceLocation[old_pos].player_check())
                    moves.Add(temp);
                break;
            }
            temp.y += 1;
        }
        temp = old_pos;
        temp.y -= 1;
        while(GameManager.occupiedSpots.ContainsKey(temp))
        {
            if(!GameManager.occupiedSpots[temp])
                moves.Add(temp);
            else
            {
                if(GameManager.pieceLocation[temp].player_check() != GameManager.pieceLocation[old_pos].player_check())
                    moves.Add(temp);
                temp = old_pos;
                break;
            }
            temp.y -= 1;
        }
        return moves;
    }
    //Creates bounds for rook, queen and bishop
    public virtual List<Vector2> diagonal_bounds(Vector2 old_pos)
    {
        List<Vector2> moves = new List<Vector2>();
        Vector2 temp = old_pos;
        temp.y += 1;
        temp.x += 1;
        while(GameManager.occupiedSpots.ContainsKey(temp))
        {
            if(!GameManager.occupiedSpots[temp])
                moves.Add(temp);
            else
            {
                if(GameManager.pieceLocation[temp].player_check() != GameManager.pieceLocation[old_pos].player_check())
                    moves.Add(temp);
                break;
            }
            temp.y += 1;
            temp.x += 1;
        }

        temp = old_pos;
        temp.x -= 1;
        temp.y += 1;
        while(GameManager.occupiedSpots.ContainsKey(temp))
        {
            if(!GameManager.occupiedSpots[temp])
                moves.Add(temp);
            else
            {
                if(GameManager.pieceLocation[temp].player_check() != GameManager.pieceLocation[old_pos].player_check())
                    moves.Add(temp);
                break;
            }
            temp.x -= 1;
            temp.y += 1;
        }

        temp = old_pos;
        temp.y -= 1;
        temp.x += 1;
        while(GameManager.occupiedSpots.ContainsKey(temp))
        {
            if(!GameManager.occupiedSpots[temp])
                moves.Add(temp);
            else
            {
                if(GameManager.pieceLocation[temp].player_check() != GameManager.pieceLocation[old_pos].player_check())
                    moves.Add(temp);
                break;
            }
            temp.y -= 1;
            temp.x += 1;
        }

        temp = old_pos;
        temp.x -= 1;
        temp.y -= 1;
        while(GameManager.occupiedSpots.ContainsKey(temp))
        {
            if(!GameManager.occupiedSpots[temp])
                moves.Add(temp);
            else
            {
                if(GameManager.pieceLocation[temp].player_check() != GameManager.pieceLocation[old_pos].player_check())
                    moves.Add(temp);
                break;
            }
            temp.x -= 1;
            temp.y -= 1;
        }
        return moves;
    }
    public virtual bool is_in_check(Vector2 old_pos)
    {
        bool in_check = false;
        return in_check;
    }
}
