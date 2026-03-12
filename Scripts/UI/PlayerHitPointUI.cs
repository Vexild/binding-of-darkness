using Godot;
using System;
using System.ComponentModel.DataAnnotations;

public partial class PlayerHitPointUI : Control
{
	[Export]
	private HBoxContainer HeartRow;
	private CoreManager CoreManagerNode;
	private enum HeartState { FULL, HALF, EMPTY };
	private Texture2D heartFull = GD.Load<Texture2D>("res://Assets/hearts/heartfull.png");
	private Texture2D heartHalf = GD.Load<Texture2D>("res://Assets/hearts/hearthalf.png");
	private Texture2D heartEmpty = GD.Load<Texture2D>("res://Assets/hearts/heartempty.png");
	public override void _Ready()
	{
		CoreManagerNode = GetParent() as CoreManager;
		InitiateCoreManager();
		InitiatePlayerStartingHealth(CoreManagerNode);
	}

	public override void _Process(double delta)
	{
	}

	private void InitiatePlayerStartingHealth(CoreManager CoreManager)
	{
		HeartRow = GetNode<HBoxContainer>("Hearts");
		for (int i = 0; i < Math.Floor(CoreManager.GetPlayerMaxHitPoints); i++)
		{
			var newHeart = new TextureRect
			{
				Texture = heartFull
			};
			HeartRow.AddChild(newHeart);
		}
		UpdatePlayersHealth(CoreManagerNode.GetPlayerHitPoints);

	}

	public void UpdatePlayersHealth(float newHealthValue)
	{
		float maxHP = CoreManagerNode.GetPlayerMaxHitPoints;
		float difference = (float)Math.Round(maxHP - newHealthValue, 2); // allow only 1 decimal
		bool IsNewHealthValueDecimal = newHealthValue % 1 != 0;
		for (int i = 0; i < newHealthValue; i++)
		{
			HeartRow.GetChild<TextureRect>(i).Texture = heartFull;
		}
		for (int i = 0; i < difference; i++)
		{
			if (IsNewHealthValueDecimal && i == 0)
			{
				HeartRow.GetChild<TextureRect>((int)(newHealthValue + i)).Texture = heartHalf;
			}
			else
			{
				HeartRow.GetChild<TextureRect>((int)(newHealthValue + i)).Texture = heartEmpty;
			}
		}
	}
	private void InitiateCoreManager()
	{
		CoreManagerNode = GetParent() as CoreManager;
		if (CoreManagerNode == null)
		{
			GD.PushError("UI: CoreManager parent is missing or wrong type.");
		}
	}
}
