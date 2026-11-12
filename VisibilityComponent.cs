using Godot;

namespace практика_333_никоненко;

// зона видимости
[GlobalClass]
public partial class VisibilityComponent : Node3D
{
	[Export]
	public float ViewRange { get; set; } = 15.0f; // дальность зоны видимости

	public bool CanSeeTarget(Node3D target) // проверяем, попадает ли цель в зону видимости
	{
		if (target == null || target.IsQueuedForDeletion()) return false;
		return GlobalPosition.DistanceTo(target.GlobalPosition) <= ViewRange;
	}

	public Node3D FindNearestTarget(params string[] targetGroups) // ищем ближайшую цель в нужных группах
	{
		Node3D nearest = null;
		float minDist = float.MaxValue;

		foreach (string group in targetGroups)
		{
			foreach (Node node in GetTree().GetNodesInGroup(group))
			{
				if (node is Node3D target && CanSeeTarget(target))
				{
					float dist = GlobalPosition.DistanceTo(target.GlobalPosition);
					if (dist < minDist)
					{
						minDist = dist;
						nearest = target;
					}
				}
			}
		}

		return nearest;
	}
}
