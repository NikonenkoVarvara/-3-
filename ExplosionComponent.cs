using Godot;
using System.Collections.Generic;

namespace практика_333_никоненко;

// урон по площади при попадании
[GlobalClass]
public partial class ExplosionComponent : DamagingComponent
{
    [Export] public float BlastRadius { get; set; } = 3.0f; // радиус взрыва
    [Export] public float FlashTime { get; set; } = 0.2f; // сколько видно вспышку

    public override void ApplyDamage(Node3D target)
    {
        if (GetTargetHealth(target) == null) return;

        base.ApplyDamage(target);
        Vector3 center = target.GlobalPosition;
        HashSet<Node3D> hit = new HashSet<Node3D> { target }; // чтобы не задеть одну цель дважды

        foreach (string group in TargetGroups)
        {
            foreach (Node node in GetTree().GetNodesInGroup(group))
            {
                if (node is not Node3D body || hit.Contains(body)) continue;

                float dist = center.DistanceTo(body.GlobalPosition);
                if (dist <= BlastRadius)
                {
                    hit.Add(body);
                    base.ApplyDamage(body);
                }
            }
        }

        ShowExplosion(new Vector3(center.X, GlobalPosition.Y, center.Z));
    }

    private void ShowExplosion(Vector3 center) // показываем взрыв полупрозрачным шаром
    {
        MeshInstance3D flash = new MeshInstance3D();
        flash.Mesh = new SphereMesh { Radius = BlastRadius, Height = BlastRadius * 2 };
        flash.MaterialOverride = new StandardMaterial3D
        {
            AlbedoColor = new Color(0.7f, 1.0f, 0.2f, 0.35f),
            Transparency = BaseMaterial3D.TransparencyEnum.Alpha,
            ShadingMode = BaseMaterial3D.ShadingModeEnum.Unshaded
        };

        GetParent().GetParent().AddChild(flash);
        flash.GlobalPosition = center;
        GetTree().CreateTimer(FlashTime).Timeout += flash.QueueFree;
    }
}
