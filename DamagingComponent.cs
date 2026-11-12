using Godot;

namespace практика_333_никоненко;

// урон при попадании или касании
[GlobalClass]
public partial class DamagingComponent : Area3D
{
    [Export] public int Damage { get; set; } = 25; // урон от одного попадания
    [Export] public string[] TargetGroups { get; set; } = new[] { "enemies" }; // группы, которым можно нанести урон
    [Export] public bool OneShot { get; set; } = true; // удаляем объект после первого попадания
    [Export] public float ContactInterval { get; set; } = 1.0f; // время между атаками при контакте

    private bool _used;
    private double _timeToHit;

    public override void _Ready()
    {
        if (OneShot) BodyEntered += OnBodyEntered; // подключаем сигнал столкновения
    }

    public override void _PhysicsProcess(double delta)
    {
        if (OneShot) return; // для пули хватает сигнала столкновения

        _timeToHit -= delta;
        if (_timeToHit > 0) return;

        foreach (Node3D body in GetOverlappingBodies())
        {
            if (GetTargetHealth(body) == null) continue;

            ApplyDamage(body);
            _timeToHit = ContactInterval;
        }
    }

    public virtual void ApplyDamage(Node3D target) // наносим урон цели
    {
        GetTargetHealth(target)?.TakeDamage(Damage);
    }

    protected HealthComponent GetTargetHealth(Node3D target)
    {
        if (target.IsQueuedForDeletion()) return null;

        foreach (string group in TargetGroups)
        {
            if (target.IsInGroup(group))
            {
                return target.GetNodeOrNull<HealthComponent>("HealthComponent");
            }
        }

        return null;
    }

    private void OnBodyEntered(Node3D body)
    {
        if (_used || GetParent().IsQueuedForDeletion() || GetTargetHealth(body) == null) return;

        _used = true;
        ApplyDamage(body);
        Consume();
    }

    private void Consume() // удаляем пулю после попадания
    {
        GetParent().QueueFree();
    }
}
