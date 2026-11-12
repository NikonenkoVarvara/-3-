using Godot;

namespace практика_333_никоненко;

// постановка башни перед игроком
[GlobalClass]
public partial class BuildComponent : Node
{
	[Export] public PackedScene[] TowerScenes { get; set; } // сцены башен для клавиш 1, 2 и 3
	[Export] public float BuildDistance { get; set; } = 3.0f; // расстояние от игрока до башни
	[Export] public float BuildLimit { get; set; } = 9.0f;
	[Export] public CylinderShape3D TowerShape { get; set; }

	private Node3D _player;
	private MeshInstance3D _preview;
	private StandardMaterial3D _previewMaterial;
	private Color _previewColor;
	private int _towerIndex;

	public override void _Ready()
	{
		_player = GetParent<Node3D>();
		_previewMaterial = new StandardMaterial3D();
		_previewMaterial.Transparency = BaseMaterial3D.TransparencyEnum.Alpha;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (_player.IsQueuedForDeletion()) return;

		if (Input.IsActionJustPressed("tower_1")) SelectTower(0);
		if (Input.IsActionJustPressed("tower_2")) SelectTower(1);
		if (Input.IsActionJustPressed("tower_3")) SelectTower(2);

		if (_preview != null && Input.IsActionJustPressed("cancel_build"))
		{
			RemovePreview();
			return;
		}

		if (_preview == null)
		{
			if (Input.IsActionJustPressed("build")) ShowPreview();
			return;
		}

		Vector3 position = _player.ToGlobal(Vector3.Forward * BuildDistance);
		bool canBuild = CanBuild(position);
		_previewMaterial.AlbedoColor = canBuild ? _previewColor : new Color(1, 0.2f, 0.2f, 0.45f);

		if (canBuild && Input.IsActionJustPressed("build"))
		{
			Node3D tower = TowerScenes[_towerIndex].Instantiate<Node3D>();
			_player.GetParent().AddChild(tower);
			tower.GlobalPosition = position;
			RemovePreview();
		}
	}

	private bool CanBuild(Vector3 position) // проверка места для башни
	{
		if (Mathf.Abs(position.X) > BuildLimit || Mathf.Abs(position.Z) > BuildLimit) return false;

		PhysicsShapeQueryParameters3D query = new PhysicsShapeQueryParameters3D();
		query.Shape = TowerShape;
		query.Transform = new Transform3D(Basis.Identity, position + Vector3.Up);
		query.CollisionMask = 1; // пол на другом слое
		return _player.GetWorld3D().DirectSpaceState.IntersectShape(query, 1).Count == 0;
	}

	private void SelectTower(int index)
	{
		if (_towerIndex == index) return;

		_towerIndex = index;
		if (_preview != null)
		{
			RemovePreview();
			ShowPreview();
		}
	}

	private void ShowPreview()
	{
		// у заготовки оставляем только меш
		Node3D tower = TowerScenes[_towerIndex].Instantiate<Node3D>();
		MeshInstance3D mesh = tower.GetNode<MeshInstance3D>("MeshInstance3D");
		_preview = new MeshInstance3D();
		_preview.Mesh = mesh.Mesh;
		_preview.Position = Vector3.Forward * BuildDistance + mesh.Position;
		_preview.MaterialOverride = _previewMaterial;

		StandardMaterial3D material = (StandardMaterial3D)mesh.GetActiveMaterial(0);
		_previewColor = material.AlbedoColor;
		_previewColor.A = 0.45f;
		_previewMaterial.AlbedoColor = _previewColor;

		_player.AddChild(_preview);
		tower.Free();
	}

	private void RemovePreview()
	{
		_preview.QueueFree();
		_preview = null;
	}
}
