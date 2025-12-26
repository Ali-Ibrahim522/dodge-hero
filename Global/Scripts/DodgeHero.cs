using Godot;

public partial class DodgeHero : Node, IDepend
{
	public override void _Notification(int what) => this.DI(what);

	[Inject]
	private ViewManager viewManager;

	public void OnResolved() =>
		viewManager.LoadView(View.Start, LoadStrategy.Base);
}
