using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class white_dropdown : MonoBehaviour
{
	Dropdown drop;
	public static GameObject promoted;
	public GameObject white_knight;
	public GameObject white_bishop;
	public GameObject white_rook;
	public GameObject white_queen;
    // Start is called before the first frame update
    void Start()
    {
    	drop = GetComponent<Dropdown>();
    }

    // Update is called once per frame
    void Update()
    {
    	if(drop.value == 1)
    	{
    		promoted = white_knight;
    	}
    	else if(drop.value == 2)
    	{
    		promoted = white_bishop;
    	}
    	else if(drop.value == 3)
    	{
    		promoted = white_rook;
    	}
    	else if (drop.value == 4)
    	{
    		promoted = white_queen;
    	}
    	else
    	{
    		promoted = white_queen;
    	}
    }
}
