using System.Data;
using Godot;

public partial class TargetSystem : Node
{
	private enum TargetState
	{
		Waiting,
		Proposed,
		Done
	}

	private const double _doneWindowScale = .5;
	public string input = ScarecrowInputAction.scarecrow_hit_target.ToString();
	private RandomNumberGenerator _rng = new RandomNumberGenerator();

	[Export]
	private TargetChallenge[] targets;
	[Export]
	private RayCast2D mouse;

	private int _challenge;
	private double _elapsed;
	private double _doneWindow;
	private double _proposedWindow = 2;
	private double _waitingWindow = .5;
	private bool pressed = false;
	private TargetState _state;

	public override void _Ready() => StartChallenge();

	public override void _Process(double delta) => pressed |= Input.IsActionJustPressed(input);

	public override void _PhysicsProcess(double delta)
	{
		_elapsed += delta;
		if (pressed)
		{
			mouse.Position = GetViewport().GetMousePosition();
			mouse.ForceRaycastUpdate();
			TargetChallenge target = (TargetChallenge) mouse.GetCollider();
			
			if (target != null) SetHit();
			else SetMissed();
			
			pressed = false;
		}
		
		switch (_state)
		{
			case TargetState.Waiting:
				if (_elapsed >= _waitingWindow) ProposeChallenge();
				break;
			case TargetState.Proposed:
				if (_elapsed >= _proposedWindow) SetMissed();	
				break;
			case TargetState.Done:
				if (_elapsed >= _doneWindow) {
					ResetTarget();
					StartChallenge();
				}
				break;
		}
	}
	
	private void ResetTarget()
	{
		targets[_challenge].indicator.Modulate = Colors.White;
		targets[_challenge].target.Visible = false;
		targets[_challenge].stump.Visible = true;
	}

	private void StartChallenge()
	{
		_challenge = _rng.RandiRange(0, targets.Length - 1);
		_state = TargetState.Waiting;
		_elapsed = 0;
	}

	private void ProposeChallenge()
	{
		targets[_challenge].ProcessMode = ProcessModeEnum.Inherit;
		targets[_challenge].indicator.Modulate = Colors.Cyan;
		targets[_challenge].target.Visible = true;
		targets[_challenge].stump.Visible = false;
		_state = TargetState.Proposed;
		_elapsed = 0;
	}
	
	private void SetMissed()
	{
		if (_state != TargetState.Proposed) return;
		targets[_challenge].ProcessMode = ProcessModeEnum.Disabled;
		targets[_challenge].indicator.Modulate = Colors.Red;
		
		_state = TargetState.Done;
		_doneWindow = _proposedWindow * _doneWindowScale;
		_elapsed = 0;
	}

	private void SetHit()
	{
		targets[_challenge].ProcessMode = ProcessModeEnum.Disabled;
		targets[_challenge].indicator.Modulate = Colors.Green;
		
		_state = TargetState.Done;
		_doneWindow = _proposedWindow * _doneWindowScale;
		_elapsed = 0;
	}
}
