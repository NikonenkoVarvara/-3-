namespace практика_333_никоненко;
using Godot;

// здоровье объекта
[GlobalClass]
public partial class HealthComponent : Node
{
    [Export] public int MaxHealth { get; set; } = 100; // максимальное здоровье

    public int CurrentHealth { get; private set; } // текущее здоровье

    private bool _isDead; // для предотвращения повторной смерти

    [Signal]
    public delegate void DiedEventHandler(); // сигнал, который срабатывает когда заканчивается здоровье

    public override void _Ready() // текущее здоровье в начале равно максимальному
    {
        CurrentHealth = MaxHealth;
    }

    public void TakeDamage(int damage) // метод нанесения урона
    {
        if (_isDead || damage <= 0) return;
        CurrentHealth = Mathf.Max(0, CurrentHealth - damage);
        if (CurrentHealth <= 0)
        {
            Die();
        }
    }

    public void Heal(int amount) // метод восстановления здоровья
    {
        if (_isDead || amount <= 0) return;
        CurrentHealth += Mathf.Min(amount, MaxHealth - CurrentHealth);
    }

    private void Die() // отправляем сигнал и удаляем объект
    {
        if (_isDead) return;
        _isDead = true;

        EmitSignal(SignalName.Died);
        GetParent().QueueFree();
    }
}
