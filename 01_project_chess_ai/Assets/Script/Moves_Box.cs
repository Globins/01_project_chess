using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Moves_Box : MonoBehaviour
{
	public Text mytest = null;
	private int cap = 14;
	public static List<string> moves_made;
    // Start is called before the first frame update
    void Start()
    {
        mytest.text = "           Prior Moves:";
        moves_made = new List<string>();
    }

    // Update is called once per frame
    void Update()
    {
    	string test = "           Prior Moves:";
    	int temp = 0;
    	//cap is 14 moves can be displayed at once      
		for(int i = moves_made.Count-1; i >= 0; i--)
        {
        	int temp_count = i + 1;
	        test = test + "\r\n" + temp_count + ": " + moves_made[i];
	        temp++;
	        if(temp == cap)
	        	break;
        }
       	mytest.text = test;
    }
    public static void display_move(Piece piece, Vector2 location)
    {
    	string piece_name = name_convert(piece.name);
    	string move = coordinate_convert(location);
    	moves_made.Add(piece_name + " to " + move);
    }
    public static void force_print(string print)
    {
        moves_made.Add(print);
    }
    private static string name_convert(string name)
    {
    	string adjusted = "";
    	if(name.IndexOf("w") == 0)
    		adjusted += "White ";
    	else
    		adjusted += "Black ";
    	if(name.IndexOf("queen") > 0)
    		adjusted += "Queen";
    	if(name.IndexOf("king") > 0)
     		adjusted += "King";
    	if(name.IndexOf("bishop") > 0)
    		adjusted += "Bishop";
    	if(name.IndexOf("knight") > 0)
    		adjusted += "Knight";
    	if(name.IndexOf("rook") > 0)
    		adjusted += "Rook";
    	if(name.IndexOf("pawn") > 0)
    		adjusted += "Pawn";
    	return adjusted;
    }
    private static string coordinate_convert(Vector2 location)
    {
    	string coordinate = "";
    	if(location.x == 0)
    	{
    		coordinate = "A";
    	}
    	if(location.x == 1)
    	{
    		coordinate = "B";
    	}
    	if(location.x == 2)
    	{
    		coordinate = "C";
    	}
    	if(location.x == 3)
    	{
    		coordinate = "D";
    	}
    	if(location.x == 4)
    	{
    		coordinate = "E";
    	}
    	if(location.x == 5)
    	{
    		coordinate = "F";
    	}
    	if(location.x == 6)
    	{
    		coordinate = "G";
    	}
    	if(location.x == 7)
    	{
    		coordinate = "H";
    	}
    	location.y += 1;
    	coordinate = coordinate + location.y.ToString();
    	return coordinate;
    }
}
