using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Piece : MonoBehaviour
{
	public virtual void kill()
	{
		Debug.Log("Broke");
	}
}
