using Godot;

namespace практика_333_никоненко;

// состояние игры и конец боя
public partial class GameManager : Node3D
{
    private bool _ended;

    public override void _Ready()
    {
        HealthComponent coreHealth = GetNode<HealthComponent>("Core/HealthComponent");
        coreHealth.Died += OnCoreDied; // если ядро уничтожено, игра заканчивается
    }

    public override void _Process(double delta)
    {
        if (GetTree().GetNodeCountInGroup("enemies") == 0)
        {
            EndGame("Победа! Все враги уничтожены");
        }
    }

    private void OnCoreDied()
    {
        EndGame("Поражение. Ядро уничтожено");
    }

    private void EndGame(string text)
    {
        if (_ended) return;

        _ended = true;
        GD.Print(text); // пишем результат в Вывод Godot
        GetTree().CallGroup("bullets", Node.MethodName.QueueFree);
        GetTree().Paused = true; // останавливаем бой
    }
}
