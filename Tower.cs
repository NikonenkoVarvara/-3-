using Godot;

namespace практика_333_никоненко;

// башня игрока
public partial class Tower : StaticBody3D
{
    private VisibilityComponent _visibilityComponent;
    private ShootComponent _shootComponent;

    public override void _Ready()
    {
        _visibilityComponent = GetNodeOrNull<VisibilityComponent>("VisibilityComponent");
        _shootComponent = GetNodeOrNull<ShootComponent>("ShootComponent");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsQueuedForDeletion()) return;

        Node3D enemy = _visibilityComponent?.FindNearestTarget("enemies"); // поиск ближайшего врага
        if (enemy != null)
        {
            _shootComponent?.Shoot(enemy.GlobalPosition, "enemies");
        }
    }
}
