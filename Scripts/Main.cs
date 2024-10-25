using Godot;


public partial class Main : Node2D
{
	[Export]
	public PackedScene ProjectileScene = GD.Load<PackedScene>("res://Scenes//Projectile.tscn");
	[Export]
	public PackedScene EnemyProjectileScene = GD.Load<PackedScene>("res://Scenes/EnemyProjectile.tscn");

	public void OnPlayerShootProjectile(Vector2 playerPos, Vector2 direction) {
		// NOTE: Important to cast the instance as Projectile. Otherwise the instance would be a 'Node' without Position attribute
		var newProjectile = ProjectileScene.Instantiate() as Projectile;
		// add the direction to the instantiated projectile
		newProjectile.ProjectileDirection = direction;
		GetNode<Node2D>("Shots").AddChild(newProjectile);
		newProjectile.Position = playerPos;
		
	}
	public void OnEnemyShootProjectile(Vector2 enemyPos, Vector2 direction) {
		var newEnemyProjectile = EnemyProjectileScene.Instantiate() as Projectile;

		// Set all attributes here. Later, we want to make a single shoot function
		// that takes in the nature of the shooter, distance, damage, speed, delays
		// projectile numbers and other attributes 
		newEnemyProjectile.IsThisEnemyShot = true;
		newEnemyProjectile.ProjectileDirection = direction;
		GetNode<Node2D>("EnemyShots").AddChild(newEnemyProjectile);
		newEnemyProjectile.Position = enemyPos;
		
	}
	public void OnDeleteProjectile() {
		QueueFree();
		GD.Print("Projectile removed");
	}
}
