namespace практика_333_никоненко;
using Godot;

// движение объекта по полю
[GlobalClass]
public partial class MoveComponent : Node
{
    [Export] public float Speed { get; set; } = 3.0f; // скорость объекта
    [Export] public float StopDistance { get; set; } = 0.1f; // на каком расстоянии цель достигнута

    public void MoveTo(Vector3 targetPosition, double delta) // перемещение в сторону цели
    {
        Node3D parent = GetParent() as Node3D;
        if (parent == null || Speed <= 0) return;

        targetPosition.Y = parent.GlobalPosition.Y; // двигаемся только по плоскости
        if (parent.GlobalPosition.DistanceTo(targetPosition) <= StopDistance) return; // цель достигнута

        parent.GlobalPosition = parent.GlobalPosition.MoveToward(targetPosition, Speed * (float)delta);
    }

    public void MoveInDirection(Vector3 direction) // движение игрока с учётом столкновений
    {
        CharacterBody3D parent = GetParent<CharacterBody3D>();
        parent.Velocity = direction * Speed;
        parent.MoveAndSlide();
    }
}
