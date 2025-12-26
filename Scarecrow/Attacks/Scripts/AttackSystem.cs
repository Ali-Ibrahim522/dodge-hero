using Godot;

public partial class AttackSystem : Node
{

    private const double _doneWindowScale = .5;

    [Export]
    private AttackChallenge[] challenges;
    [Export]
    private Sprite2D attack;

    private RandomNumberGenerator _rng = new RandomNumberGenerator();
    private bool _proposing;
    private int _challenge;
    private double _time;
    private double _elapsed;
    private ScaledAttribute _window = new ScaledAttribute(
        x => 1 / Mathf.Sqrt(.2 * x + 1) + .45
    );

    public override void _Ready()
    {
        _time = 0;
        _elapsed = 0;
        _challenge = 0;
        _window.Update(_time);
        attack.Texture = null;
        StartChallenge();
    }

    public override void _Process(double delta)
    {
        _time += delta;
        _elapsed += delta;
        if (_proposing) {
            CheckInput();
            CheckWindow();
        } else {
            CheckDoneWindow();
        }
    }

    private void StartChallenge()
    {
        _challenge = _rng.RandiRange(0, challenges.Length - 1);

        attack.Texture = challenges[_challenge].pre;
        challenges[_challenge].arrow.Modulate = Colors.Cyan;

        _window.Update(_time);
        _elapsed = 0;
        _proposing = true;
    }

    private void CheckInput()
    {
        for (int i = 0; i < challenges.Length; i++)
        {
            if (Input.IsActionPressed(challenges[i].input.ToString()))
            {
                if (i == _challenge) SetHit();
                else SetMissed();
            }
        }
    }

    private void CheckWindow()
    {
        if (_elapsed >= _window.Value())
            SetMissed();
    }
    
    private void CheckDoneWindow()
    {
        if (_elapsed >= _window.Value() * _doneWindowScale)
            EndChallenge();
    }

    private void SetHit()
    {
        challenges[_challenge].arrow.Modulate = Colors.Green;
        attack.Texture = challenges[_challenge].post;

        _elapsed = 0;
        _proposing = false;
    }

    private void SetMissed()
    {
        challenges[_challenge].arrow.Modulate = Colors.Red;
        attack.Texture = challenges[_challenge].post;

        _elapsed = 0;
        _proposing = false;
    }
    
    private void EndChallenge()
    {
        challenges[_challenge].arrow.Modulate = Colors.White;
        StartChallenge();
    }
}