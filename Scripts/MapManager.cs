using Godot;
using System;

public partial class MapManager : Node2D
{
	// This manager handles room generation and and transition between rooms.
	// For starters we want a default testroom
	private PackedScene TestRoomScene = GD.Load<PackedScene>("res://Scenes/Rooms/TestRoom.tscn");
	private int RoomWidth = 20;
	private int RoomHeight = 14;
	public override void _Ready()
	{
		GD.Print("MAPMANAGER: Initiating Map Manager");
		RoomManager CurrentRoomNode = TestRoomScene.Instantiate() as RoomManager;
		
		//CurrentRoomNode.width = RoomWidth;
		//CurrentRoomNode.height = RoomHeight;
		AddChild(CurrentRoomNode);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
