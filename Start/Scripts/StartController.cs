using Godot;

public partial class StartController : Node, IDepend
{
	public override void _Notification(int what) => this.DI(what);

	[Inject]
	private ViewManager viewManager;

	[Export]
	private Button playButton;

	[Export]
	private Button quitButton;

	public void OnResolved()
	{
		playButton.Pressed += Play;
		quitButton.Pressed += Quit;
	}

	private void Play() => viewManager.LoadLevel(Level.Scarecrow, Difficulty.Easy, LoadStrategy.Base);

	private void Quit() => GetTree().Quit();
}
