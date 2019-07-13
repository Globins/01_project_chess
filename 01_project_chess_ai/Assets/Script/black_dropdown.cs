using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class black_dropdown : MonoBehaviour
{
	Dropdown drop;
	public static GameObject promoted;
	public GameObject black_knight;
	public GameObject black_bishop;
	public GameObject black_rook;
	public GameObject black_queen;
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
    		promoted = black_knight;
    	}
    	else if(drop.value == 2)
    	{
    		promoted = black_bishop;
    	}
    	else if(drop.value == 3)
    	{
    		promoted = black_rook;
    	}
    	else if (drop.value == 4)
    	{
    		promoted = black_queen;
    	}
    	else
    	{
    		promoted = black_queen;
    	}
    }
}
