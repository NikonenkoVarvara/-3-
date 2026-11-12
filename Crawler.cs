using Godot;

namespace практика_333_никоненко;

// ползущий враг, наносит урон при касании
public partial class Crawler : StaticBody3D
{
    private MoveComponent _moveComponent;
    private VisibilityComponent _visibilityComponent;

    public override void _Ready()
    {
        _moveComponent = GetNode<MoveComponent>("MoveComponent");
        _visibilityComponent = GetNode<VisibilityComponent>("VisibilityComponent");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsQueuedForDeletion()) return;

        Node3D target = _visibilityComponent.FindNearestTarget("PlayerStructures", "player"); // ищем ближайшую цель
        if (target != null)
        {
            _moveComponent.MoveTo(target.GlobalPosition, delta);
        }
        else
        {
            _moveComponent.MoveTo(Vector3.Zero, delta); // цели нет, идём к центру
        }
    }
}
