using Godot;


public partial class Main : Node2D
{
	[Export]
	public PackedScene ProjectileScene = GD.Load<PackedScene>("res://Scenes//Projectile.tscn");
	[Export]
	public PackedScene EnemyProjectileScene = GD.Load<PackedScene>("res://Scenes/EnemyProjectile.tscn");
	[Signal]
	public delegate void ShootEnemyProjectileEventHandler(float pos, Vector2 dir);
	private bool PlayerAlive;
	private Vector2 PlayerPosition;

	public override void _Ready()
	{
		PlayerAlive = true;
	}
	public override void _Process(double delta)
	{
		PlayerPosition = GetNode<CharacterBody2D>("Player").Position;
	}
	public void OnPlayerShootProjectile(Vector2 playerPos, Vector2 direction)
	{
		// NOTE: Important to cast the instance as Projectile. Otherwise the instance would be a 'Node' without Position attribute
		var newProjectile = ProjectileScene.Instantiate() as Projectile;
		// add the direction to the instantiated projectile
		newProjectile.ProjectileDirection = direction;
		GetNode<Node2D>("Shots").AddChild(newProjectile);
		newProjectile.Position = playerPos;
	}
	private void EnemyShootAtPlayer(Vector2 ShooterPosition, int BaseDamage)
	{
		if (PlayerAlive)
		{
			var Direction = ShooterPosition.DirectionTo(PlayerPosition);
			var newEnemyProjectile = EnemyProjectileScene.Instantiate() as Projectile;

			// Set all attributes here. Later, we want to make a single shoot function
			// that takes in the nature of the shooter, distance, damage, speed, delays
			// projectile numbers and other attributes 
			newEnemyProjectile.IsThisEnemyShot = true;
			newEnemyProjectile.ProjectileDirection = Direction;
			newEnemyProjectile.ProjectileDamage = BaseDamage;
			GetNode<Node2D>("EnemyShots").AddChild(newEnemyProjectile);
			newEnemyProjectile.Position = ShooterPosition;
		}

	}
	public void OnDeleteProjectile()
	{
		QueueFree();
		GD.Print("Projectile removed");
	}
	public void EliminatePlayer()
	{
		PlayerAlive = false;
	}
}
