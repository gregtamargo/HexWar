using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileController : MonoBehaviour {

	private int x = 0;
	private int y = 0;
	private int teamOccupying = -1;
	private List<GamePiece> occupants;

	public int NE = 1;
	public int  E = 1;
	public int SE = 1;
	public int SW = 1;
	public int  W = 1;
	public int NW = 1;

	// Use this for initialization
	void Start () {
		occupants = new List<GamePiece>();

		NE = Random.Range (1, 4);
		E  = Random.Range (1, 4);
		SE = Random.Range (1, 4);
		SW = Random.Range (1, 4);
		W  = Random.Range (1, 4);
		NW = Random.Range (1, 4);

	}

	public void SetX(int newX)
	{
		x = newX;
	}

	public void SetY(int newY)
	{
		y = newY;
	}

	public int GetX()
	{
		return x;
	}

	public int GetY()
	{
		return y;
	}

	public TileController Highlight()
	{
		//transform.rotation = Quaternion.Euler(new Vector3(0, 0, 45));
		gameObject.renderer.material.color = new Vector4 (1, 1, 1, .5f);
		return this;
	}

	public TileController Lowlight()
	{
		//transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
		gameObject.renderer.material.color = new Vector4 (1, 1, 1, 1);
		return this;
	}

	public int GetNumberOfOccupants()
	{
		return occupants.Count;
	}

	public int GetOccupantsTeam()
	{
		return teamOccupying;
	}

	public List<GamePiece> GetOccupants()
	{
		return occupants;
	}

	public void AddOccupants(GamePiece occupant)
	{
		if (occupants.Count == 3) 
		{
			return;
		}

		if (occupants.Count == 0) {
			teamOccupying = occupant.whichTeam;
		}
		occupants.Add (occupant);
	}

	public void RemoveOccupants(GamePiece occupant)
	{
		occupants.Remove (occupant);
		if (occupants.Count == 0) {
			teamOccupying = -1;
		}
	}

	// Update is called once per frame
	void Update () {
	
	}
}
