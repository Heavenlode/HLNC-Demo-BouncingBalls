using System.Diagnostics;
using Godot;
using HLNC;

namespace BouncingBalls {

	[NetworkScenes("res://ball/ball.tscn")]
	public partial class Ball : NetworkNode3D
	{

		[Export]
		public MeshInstance3D mesh;

		[NetworkProperty]
		public Vector3 Color { get; set; }

		public void OnNetworkChangeColor(int tick, Vector3 oldColor, Vector3 newColor)
		{
			mesh.MaterialOverride.Set("albedo_color", new Color(newColor.X, newColor.Y, newColor.Z));
		}

		public override void _Ready()
		{
			base._Ready();
			GD.Seed((ulong)InputAuthority);

			if (NetworkRunner.Instance.IsServer)
			{
				Color = new Vector3((float)GD.Randf(), (float)GD.Randf(), (float)GD.Randf());
			}
		}

        public override void _NetworkProcess(int _tick)
        {
            base._NetworkProcess(_tick);
			
			if (NetworkRunner.Instance.IsServer)
			{
				// Every 60th tick, change the color of the character
				if (_tick % 60 == 0)
				{
					Position = new Vector3((float)GD.RandRange(-4, 4), 0, (float)GD.RandRange(-4, 4));
					Color = new Vector3((float)GD.Randf(), (float)GD.Randf(), (float)GD.Randf());
				}
			}
		}
    }
}
