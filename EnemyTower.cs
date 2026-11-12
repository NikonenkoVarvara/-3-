namespace практика_333_никоненко;
using Godot;

// вражеская турель
public partial class EnemyTower : StaticBody3D
{
    private MoveComponent _moveComponent;
    private VisibilityComponent _visibilityComponent;
    private ShootComponent _shootComponent;

    public override void _Ready()
    {
        _moveComponent = GetNodeOrNull<MoveComponent>("MoveComponent");
        _visibilityComponent = GetNodeOrNull<VisibilityComponent>("VisibilityComponent");
        _shootComponent = GetNodeOrNull<ShootComponent>("ShootComponent");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsQueuedForDeletion()) return;

        Node3D target = _visibilityComponent?.FindNearestTarget("PlayerStructures", "player"); // ищем ближайшую цель
        if (target != null)
        {
            // стоим на месте и стреляем
            _shootComponent?.Shoot(target.GlobalPosition, "PlayerStructures", "player");
        }
        else
        {
            _moveComponent?.MoveTo(Vector3.Zero, delta); // цели нет, идём к центру
        }
    }
}
