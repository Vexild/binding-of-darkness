using Godot;
using System;

public partial class RoomManager : Node2D
{
	// Initiate with certain size
	// layout textures to wall and floor
	// Initiate spawn points for enemies and player
	// Spawn door points
	// Spawn player, items, obstacles and enemies
	private CompressedTexture2D FloorSprite = GD.Load<CompressedTexture2D>("res://Assets/Room/Floors/floor1.png");
	private CompressedTexture2D WallSprite = GD.Load<CompressedTexture2D>("res://Assets/Room/Walls/wall1.png");
	private CompressedTexture2D CornerWallSprite = GD.Load<CompressedTexture2D>("res://Assets/Room/Walls/wall1-corner.png");
	private Node2D EnemySpawnPoint;
	private ShooterEnemy ShooterEnemy = GD.Load<ShooterEnemy>("res://Scenes/Enemy/ShooterEnemy.tscn");
	public int width = 40;
	public int height = 23;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("ROOMMANAGER: Initiating Room Manager");
		SetupEnemySpawnPoints();
		//SetupFloorTexture(width, height);
		//SetupWallTextures(width, height);
		// These are not mandatory for now. First we need a simple enemy spanw point. We can fill the textures later.
	}


	public override void _Process(double delta)
	{
	}
	private void SetupEnemySpawnPoints()
	{
		// we still need to set the rooms to x size and center it. Resolution should not affect the room size
		
		
	}

	private void SetupFloorTexture(int width, int height)
	{
		Node2D floorNode = GetNode<Node2D>("FloorNode");
		for (int i = 0; i < height; i++)
		{
			for (int j = 0; j < width; j++)
			{
				Sprite2D floorTile = new Sprite2D();
				floorTile.Texture = FloorSprite;
				floorTile.Position = new Vector2(i * FloorSprite.GetWidth(), j * FloorSprite.GetHeight());
				floorNode.AddChild(floorTile);
			}
		}
		AddChild(floorNode);
		GD.Print("ROOMMANAGER: Floor texture setup complete");
	}

	private void SetupWallTextures(int width, int height)
	{
		Node2D wallNode = GetNode<Node2D>("WallNode");
		for (int i = 0; i < height; i++)
		{
			if( i== 0 || i == height - 1 ){
				for (int j = 0; j < width; j++)
				{
					Sprite2D wallTile = new Sprite2D();
					wallTile.Texture = WallSprite;
					wallTile.Position = new Vector2(i * WallSprite.GetWidth(), j * WallSprite.GetHeight());
					wallNode.AddChild(wallTile);
				}
			} else
			{
				Sprite2D wallTileStart = new Sprite2D();
				wallTileStart.Texture = WallSprite;
				wallTileStart.Position = new Vector2(WallSprite.GetWidth(), WallSprite.GetHeight());
				wallNode.AddChild(wallTileStart);

				Sprite2D wallTileEnd = new Sprite2D();
				wallTileEnd.Texture = WallSprite;
				wallTileEnd.Position = new Vector2(width * WallSprite.GetWidth(), i * WallSprite.GetHeight());
				wallNode.AddChild(wallTileEnd);
			}
		}
		AddChild(wallNode);
		GD.Print("ROOMMANAGER: Wall texture setup complete");
	}
}
