using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePiece : MonoBehaviour {
	public static bool touching  = false;
	public static Vector2 touchPosition;

	bool beingHeld = false;
	bool beingInFocus = false;
	bool ableToAct = true;
	Vector2 initialPosition;
	Vector2 grabOffset;

	public int whichTeam= 0;

	public int initialX = -1;
	public int initialY = -1;
	public int currentX = -1;
	public int currentY = -1;
	public int movementFactor;
	public int health;	
	public Vector2 position;

	public bool moving;
	public bool isOurTurn;

	//private static const Vector2[] movesFromEven = {};

	// Use this for initialization

	private static Transform theCamera;
	private static bool holding = false;
	void Start () {
		if(theCamera == null)
		{
			theCamera = Camera.main.transform;
		}

		possibleMoves = new List<TileController> ();
	}
	
	// Update is called once per frame
	void Update () {
		if(whichTeam == MapController.whoseTurn)
		{
			if(!isOurTurn)
			{
				//gameObject.collider2D.enabled=true;
				gameObject.renderer.material.color = Color.blue;
				initialX = currentX;
				initialY = currentY;
				ableToAct = true;
				isOurTurn = true;
			}
			//Debug.Log (holding+" holding");
			if(holding && beingHeld)
			{

				//if mouse is still down 
				if(touching)
				{
					Debug.Log ("MOVE WITH THE MOUSEEEEEEEE!!!!!!!");
					float r = Random.value;
					float g = Random.value;
					float b = Random.value;
					gameObject.renderer.material.color = new Vector4(r,g,b,1);
					transform.position = touchPosition-grabOffset;
				}
				else
				{//Release this tile!
					beingHeld = false;
					holding = false;

					//PICK THE CLOSEST TILE! THIS DOESNT REALLY DO IT YET!
					Collider2D[] theseTiles = Physics2D.OverlapPointAll(transform.position);
					Transform closestTile = transform;
					foreach( Collider2D tile in theseTiles)
					{
						if(tile != this.collider2D)
						{
							closestTile = tile.transform;
						}
					}


					if(closestTile !=transform)
					{
						TileController thatTile = closestTile.gameObject.GetComponent<TileController>();
						if(possibleMoves.Contains(thatTile) || initialX<0)
						{
							Debug.Log ("PLACE IT "+Time.time);
							transform.position = closestTile.position;


							 
							currentX = thatTile.GetX();
							currentY = thatTile.GetY();

							if(initialX<0)
							{
								Debug.Log ("JUST GOT PLACED FOR THE FIRST TIME!"+Time.time);

								initialX = currentX;
								initialY = currentY;
								ableToAct = false;
							}
							else{
								MapController.GetTile(initialX,initialY).RemoveOccupants(this);
							}

							thatTile.AddOccupants (this);
						}
						else{
							Debug.Log ("1 SNAP IT BACK"+Time.time);
							transform.position = initialPosition;
						}
					}
					else{
						Debug.Log ("2 SNAP IT BACK"+Time.time);
						transform.position = initialPosition;
					}

				}
			}
			else
			{
				//if mouse is being held down
				if(touching)
				{
					Debug.Log ("being held? "+beingHeld);
					//if within me
					//UpdateMousePosition ();
					if (ableToAct && this.collider2D == Physics2D.OverlapPoint(touchPosition))	
					{
						Debug.Log ("GRABHOLD "+Time.time);
						initialPosition = transform.position;
						grabOffset = touchPosition - initialPosition;
						beingHeld=true;
						beingInFocus = true;
						holding = true;
						if(currentX>0)
						{
							Debug.Log ("needed to build movement"+Time.time);
							transform.localScale = Vector3.one *1.2f;
							gameObject.renderer.material.color = Color.white;
							possibleMoves.Clear();
							BuildMovementRange (initialX, initialY, movementFactor);
							//initialX=-1;
							//initialY=-1;
						}
					}
					else
					{
						//Debug.Log ("herp "+Time.time);
						DiscardMovementRange();
						beingInFocus = false;
						transform.localScale = Vector3.one;

					}
				}
			}
		}
		else//it's not my turn.
		{
			if(isOurTurn)
			{
				//collider2D.enabled=false;
				DiscardMovementRange();
				transform.localScale = Vector3.one;
				gameObject.renderer.material.color = Color.red;
				isOurTurn = false;
			}
		}
	}

	private void DiscardMovementRange()
	{
		foreach(TileController oldSpace in possibleMoves)
		{
			TileController oldTile = oldSpace.GetComponent<TileController>();
			MapController.Lowlight (oldTile.GetX(),oldTile.GetY());
		}
	}

	private List<TileController> possibleMoves;
	private void BuildMovementRange(int x, int y, int movesRemaining)
	{
//		Debug.Log ("starting buildmovement ("+x+"-"+y+") __"+movesRemaining);
		List<TileController> surroundingSpaces = new List<TileController>();

		//NE
		Debug.Log ("buiding movement from ("+x+","+y+")");
		if (y + 1 < MapController.tiles [x].Length) {
			if (y % 2 == 0) 
			{//even row
				surroundingSpaces.Add (MapController.tiles [x] [y + 1]);
			}
			else
			{
				if(x+1< MapController.tiles.Length)
				{
					surroundingSpaces.Add(MapController.tiles[x+1][y+1]);
				}
			}
		}

		//E
		if(x+1< MapController.tiles.Length)
		{
			surroundingSpaces.Add(MapController.tiles[x+1][y]);
		}

		//SE
		if (y> 0) {
			if (y % 2 == 0) 
			{//even row
				surroundingSpaces.Add (MapController.tiles [x] [y - 1]);
			}
			else
			{
				if(x+1< MapController.tiles.Length)
				{
					surroundingSpaces.Add(MapController.tiles[x+1][y-1]);
				}
			}
		}

		//SW
		if (y> 0) {
			if (y % 2 == 0) 
			{//even row
				if(x>0)
				{
					surroundingSpaces.Add (MapController.tiles [x-1] [y - 1]);
				}
			}
			else
			{
				surroundingSpaces.Add(MapController.tiles[x][y-1]);
			}
		}

		//W
		if(x> 0)
		{
			surroundingSpaces.Add(MapController.tiles[x-1][y]);
		}

		//NW
		if (y + 1 < MapController.tiles [x].Length) {
			if (y % 2 == 0) 
			{//even row
				if(x>0)
				{
					surroundingSpaces.Add (MapController.tiles [x-1] [y + 1]);
				}
			}
			else
			{
				surroundingSpaces.Add(MapController.tiles[x][y+1]);
			}
		}

		//MapController.Highlight (x,y);
		foreach(TileController tile in surroundingSpaces)
		{
			{
				if(/*space.terrainCost*/1<= movesRemaining)
				{
					//add this space to the list of possible spaces
					int tileAlignment = tile.GetOccupantsTeam();
					if(!possibleMoves.Contains (tile))
					{
						if(tileAlignment == -1 || (tileAlignment == whichTeam && tile.GetNumberOfOccupants()<3))
						{
							possibleMoves.Add(MapController.Highlight (tile.GetX(),tile.GetY()));
						}
					}
				 	if(/*space.terrainCost*/1<movesRemaining)
					{
						BuildMovementRange (tile.GetX(), tile.GetY(),movesRemaining-1/*Space.terrainCost*/);
					}
				}
			}
		}
	}

	public void StartMoving()
	{
		moving = true;
	}

	public void FinishMoving()
	{
		moving = false;
	}

	public void SetPosition(int x, int y)
	{
		position = new Vector2 (x, y);
	}

	private void Attack(GamePiece opponent)
	{

	}

	private static Vector3 initialTouchPosition;
	private static Vector3 wp;
	public static void UpdateMousePosition() {
		//if (Input.touchCount == 1)
		wp = Camera.main.ScreenToWorldPoint(Input.mousePosition);
		if(Input.GetMouseButton(0))
		{
		//	Debug.Log ("MOUSE BUTTONS IS DOWN "+Time.time);
			touching = true;
			//Vector3 wp = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);

			//Debug.Log ("WORLD POINT "+wp);
			touchPosition = new Vector2(wp.x, wp.y);
			if(Input.GetMouseButtonDown (0))
			{
				initialTouchPosition = (Vector3)touchPosition;
			}
			else
			{
				if(!holding)
				{
					Vector3 touchTranslation = initialTouchPosition - (Vector3)touchPosition;
					theCamera.Translate(touchTranslation);
				}
			}

		}
		else
		{
			touching = false;	
		}
	}
}
