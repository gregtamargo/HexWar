using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TileController : MonoBehaviour {

	private int x = 0;
	private int y = 0;
	private int teamOccupying = -1;
	private List<TileController> occupation;

	public int NE = 1;
	public int  E = 1;
	public int SE = 1;
	public int SW = 1;
	public int  W = 1;
	public int NW = 1;

	// Use this for initialization
	void Start () {
		occupation = new List<TileController>();

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

	//public int GetNumberOf

	// Update is called once per frame
	void Update () {
	
	}
}
