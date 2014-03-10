using UnityEngine;
using System.Collections;

public class MapController : MonoBehaviour {
	public Transform spawnThis;   
 
	public int x = 5;
	public int y = 5;

	public static int numberOfTeams = 2;
	public static int whoseTurn = 0;
 
	public float radius = 0.5f;
	public bool useAsInnerCircleRadius = true;
 
	private float offsetX, offsetY;

	public static TileController[][] tiles;

	public struct Tile{
		public int x;
		public int y;


	}
	
	// Use this for initialization
	void Start () {
		float unitLength = ( useAsInnerCircleRadius )? (radius / (Mathf.Sqrt(3)/2)) : radius;
 
		offsetX = unitLength * Mathf.Sqrt(3);
		offsetY = unitLength * 1.5f;

		tiles = new TileController[x][];
 
		for( int i = 0; i < x; i++ ) {
			tiles[i] = new TileController[y];
			for( int j = 0; j < y; j++ ) {
				Vector2 hexpos = HexOffset( i, j );
				Vector3 pos = new Vector3( hexpos.x, hexpos.y, 0 );
				Transform newTile = Instantiate(spawnThis, pos, Quaternion.identity ) as Transform;
				newTile.parent = transform;
				TileController thisTile = newTile.gameObject.GetComponent<TileController>();
				thisTile.SetX (i);
				thisTile.SetY (j);
				tiles[i][j] = newTile.gameObject.GetComponent<TileController>();
			}
		}
	}

	public static TileController GetTile(int x, int y)
	{
		return tiles [x] [y].GetComponent<TileController> ();
	}

	public static TileController Highlight(int x, int y)
	{
		return tiles [x] [y].GetComponent<TileController> ().Highlight ();
	}

	public static TileController Lowlight(int x, int y)
	{
		return tiles [x] [y].GetComponent<TileController> ().Lowlight ();
	}

	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown ("p"))
		{
			whoseTurn++;

			if(whoseTurn>=numberOfTeams)
			{
				whoseTurn=0;
			}
		}


		GamePiece.UpdateMousePosition();
	}
	
	Vector2 HexOffset( int x, int y ) {
		Vector2 position = Vector2.zero;
 
		if( y % 2 == 0 ) {
			position.x = x * offsetX;
			position.y = y * offsetY;
		}
		else {
			position.x = ( x + 0.5f ) * offsetX;
			position.y = y * offsetY;
		}
		return position;
	}
}


/*
using UnityEngine;
using System.Collections;
 
public class HexGrid : MonoBehaviour {
   
 
 
   
}

*/