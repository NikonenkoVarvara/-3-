using Godot;

namespace практика_333_никоненко;

// управление игроком
public partial class Player : CharacterBody3D
{
    [Export] public float FieldHalfSize { get; set; } = 9.4f; // граница движения по полю

    private MoveComponent _moveComponent;
    private ShootComponent _shootComponent;

    public override void _Ready()
    {
        _moveComponent = GetNode<MoveComponent>("MoveComponent");
        _shootComponent = GetNode<ShootComponent>("ShootComponent");
    }

    public override void _PhysicsProcess(double delta)
    {
        if (IsQueuedForDeletion()) return;

        Vector2 input = Input.GetVector("move_left", "move_right", "move_up", "move_down");
        _moveComponent.MoveInDirection(new Vector3(input.X, 0, input.Y));

        // не даём игроку выйти за край поля
        Vector3 position = GlobalPosition;
        position.X = Mathf.Clamp(position.X, -FieldHalfSize, FieldHalfSize);
        position.Z = Mathf.Clamp(position.Z, -FieldHalfSize, FieldHalfSize);
        GlobalPosition = position;

        AimAtMouse();
    }

    private void AimAtMouse() // поворот в сторону курсора
    {
        Camera3D camera = GetViewport().GetCamera3D();
        if (camera == null) return;

        Vector2 mouse = GetViewport().GetMousePosition();
        Plane floor = new Plane(Vector3.Up, GlobalPosition.Y); // ищем точку на плоскости поля
        Vector3? aim = floor.IntersectsRay(camera.ProjectRayOrigin(mouse), camera.ProjectRayNormal(mouse));

        if (!aim.HasValue || aim.Value.IsEqualApprox(GlobalPosition)) return;

        LookAt(aim.Value, Vector3.Up);
        if (Input.IsActionPressed("shoot"))
        {
            _shootComponent.Shoot(aim.Value, "enemies");
        }
    }
}
