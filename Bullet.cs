using Godot;
using практика_333_никоненко;

/// <summary>Пуля летит по прямой, за попадание отвечает компонент урона.</summary>
[GlobalClass]
public partial class Bullet : Node3D
{
    [Export] public float Speed { get; set; } = 20.0f;   // скорость пули
    [Export] public float Lifetime { get; set; } = 5.0f; // время жизни пули, сек

    private Vector3 _direction = Vector3.Forward; // вектор полёта пули
    private double _age;

    public void Launch(Vector3 direction, string[] targetGroups)
    {
        _direction = direction;
        GetNode<DamagingComponent>("DamagingComponent").TargetGroups = targetGroups; // кого пуля может ранить
    }

    public override void _PhysicsProcess(double delta)
    {
        GlobalPosition += _direction * Speed * (float)delta;

        _age += delta;
        if (_age >= Lifetime)
        {
            QueueFree(); // удаляем пулю, если ни во что не попала
        }
    }
}
