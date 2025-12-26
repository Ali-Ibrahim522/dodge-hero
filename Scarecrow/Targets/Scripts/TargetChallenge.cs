using Godot;

public partial class TargetChallenge : Area2D
{
	[Export]
	public Node2D target;
	[Export]
	public Node2D stump;
	[Export]
	public Sprite2D indicator;
}
