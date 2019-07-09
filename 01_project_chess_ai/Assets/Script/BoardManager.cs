using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoardManager : MonoBehaviour
{
	public GameObject[] outerBoardTiles;
	public GameObject[] gameTiles;
	public GameObject[] boardPieces;
	public int columns = 8;
	public int rows = 8;
	private bool is_black = true;
	private Transform boardHolder;
	private Transform pieceHolder;
	private List <Vector2> gridPositions = new List <Vector2> ();

	void InitialiseList()
	{
		gridPositions.Clear();
		for(int x = 1; x < columns-1; x++)
		{
			for(int y = 1; y < rows-1; y++)
			{
				gridPositions.Add(new Vector2(x,y));
			}
		}
	}
    //Sets up the outer walls and floor (background) of the game board.
    void BoardSetup ()
    {
        //Instantiate Board and set boardHolder to its transform.
        boardHolder = new GameObject ("Board").transform;
        for(int x = -1; x < columns + 1; x++)
        {
            for(int y = -1; y < rows + 1; y++)
            {
            	GameObject toInstantiate = gameTiles[0];
                if(x > -1 && x < columns && y > -1 && y < rows)
                {
                	if(is_black)
                		toInstantiate = gameTiles[1];
                	else
                		toInstantiate = gameTiles[0];
                	is_black = !is_black;
                }
                //Edge of the board
                if(x == -1)
                {
                	toInstantiate = outerBoardTiles[3];
                	if(y == -1)
                		toInstantiate = outerBoardTiles[0];
                	else if(y == rows)
                		toInstantiate = outerBoardTiles[7];
                }
                else if(x == columns)
                {
                	toInstantiate = outerBoardTiles[4];
                	if(y == -1)
                		toInstantiate = outerBoardTiles[1];
                	else if(y == rows)
                		toInstantiate = outerBoardTiles[5];
                }
                else if(y == -1)
                	toInstantiate = outerBoardTiles[2];
                else if(y == rows)
                {
                	is_black = !is_black;
                	toInstantiate = outerBoardTiles[6];
                }

                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
                GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

                //Set the parent of our newly instantiated object instance to boardHolder, this is just organizational to avoid cluttering hierarchy.
                instance.transform.SetParent (boardHolder);
            }
        }
    }
    
    //places the pieces on the board
    void PieceSetup()
    {
		pieceHolder = new GameObject ("Piece").transform;
        for(int x = 0; x < columns; x++)
        {
            for(int y = 0; y < rows; y++)
            {
            	GameObject toInstantiate = boardPieces[0];
            	if(y <= 1 || y >= rows-2)
            	{
	            	if(y == 1)
	            		toInstantiate = boardPieces[3];
	            	else if(y==rows-2)
	            		toInstantiate = boardPieces[9];
	            	else if(y == 0)
	            	{
	            		if(x == 0 || x == columns-1)
	            			toInstantiate = boardPieces[5];
	            		else if(x == 1 || x == columns-2)
	            			toInstantiate = boardPieces[2];
	            		else if(x == 2 || x == columns-3)
	            			toInstantiate = boardPieces[0];
	            		else if(x == 3)
	            			toInstantiate =boardPieces[4];
	            		else
	            			toInstantiate= boardPieces[1];
	            	}
	            	else if(y == rows-1)
	            	{
	            		if(x == 0 || x == columns-1)
	            			toInstantiate = boardPieces[11];
	            		else if(x == 1 || x == columns-2)
	            			toInstantiate = boardPieces[8];
	            		else if(x == 2 || x == columns-3)
	            			toInstantiate = boardPieces[6];
	            		else if(x == 3)
	            			toInstantiate =boardPieces[10];
	            		else
	            			toInstantiate= boardPieces[7];
	            	}
	                //Instantiate the GameObject instance using the prefab chosen for toInstantiate at the Vector3 corresponding to current grid position in loop, cast it to GameObject.
	                GameObject instance = Instantiate (toInstantiate, new Vector3 (x, y, 0f), Quaternion.identity) as GameObject;

	                instance.transform.SetParent (pieceHolder);
            	}
            }
        }
    }
    //SetupScene initializes our level and calls the previous functions to lay out the game board
    public void SetupScene ()
    {
        //Creates the outer walls and floor.
        BoardSetup ();
        PieceSetup ();
        InitialiseList ();
    }
}
