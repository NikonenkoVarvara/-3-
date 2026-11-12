using Godot;

namespace практика_333_никоненко;

// яд наносит урон несколько раз после попадания
[GlobalClass]
public partial class PoisonComponent : DamagingComponent
{
    [Export] public int TickDamage { get; set; } = 8; // урон яда за один раз
    [Export] public int TickCount { get; set; } = 3; // сколько раз наносим урон ядом
    [Export] public float TickInterval { get; set; } = 0.5f; // пауза между срабатываниями

    public override void ApplyDamage(Node3D target)
    {
        HealthComponent health = GetTargetHealth(target);
        if (health == null) return;

        base.ApplyDamage(target);
        if (target.IsQueuedForDeletion() || TickCount <= 0 || TickInterval <= 0) return;

        int ticksLeft = TickCount;
        int tickDamage = TickDamage;
        Timer timer = new Timer();
        target.AddChild(timer);

        // таймер удаляется вместе с целью
        timer.Timeout += () =>
        {
            health.TakeDamage(tickDamage);
            ticksLeft--;
            if (ticksLeft == 0) timer.QueueFree();
        };
        timer.Start(TickInterval);
    }
}
