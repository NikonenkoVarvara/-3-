namespace практика_333_никоненко;
using Godot;

// выстрел в сторону цели
[GlobalClass]
public partial class ShootComponent : Node
{
	[Export]
	public float FireRate { get; set; } = 1.0f; // скорость выстрелов
	[Export]
	public PackedScene BulletScene { get; set; } // сцена пули
	[Export] public float SpawnOffset { get; set; } = 1.5f; // отступ вылета пули от центра башни
	[Export] public float ShotHeight { get; set; } = 1.0f; // высота выстрела над основанием башни

	private double _timeSinceLastShot; // время с последнего выстрела

	public override void _PhysicsProcess(double delta) // накопление времени перезарядки
	{
		_timeSinceLastShot += delta;
	}

	public void Shoot(Vector3 targetPosition, params string[] targetGroups)
	{
		if (BulletScene == null || FireRate <= 0 || _timeSinceLastShot < 1.0 / FireRate) return;

		Node3D parent = GetParent<Node3D>();
		Vector3 start = parent.GlobalPosition + Vector3.Up * ShotHeight;
		Vector3 direction = targetPosition - parent.GlobalPosition;
		direction.Y = 0;
		if (direction.IsZeroApprox()) return;

		Bullet bullet = BulletScene.Instantiate<Bullet>();
		Vector3 forward = direction.Normalized();
		bullet.Launch(forward, targetGroups);

		parent.GetParent().AddChild(bullet);
		// если цель близко, уменьшаем отступ вылета пули
		float offset = Mathf.Clamp(SpawnOffset, 0, direction.Length() * 0.5f);
		bullet.GlobalPosition = start + forward * offset;
		bullet.LookAt(bullet.GlobalPosition + forward, Vector3.Up);
		_timeSinceLastShot = 0;
	}
}
