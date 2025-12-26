using System.Collections.Generic;
using Godot;

public enum View
{
	Start
}

public enum Level
{
	Scarecrow
}

public enum Difficulty
{
	Easy,
	Medium,
	Hard
}


public enum LoadStrategy
{
	Base,
	Additive
}

public partial class ViewManager : Node, IInject
{
	public override void _Notification(int what) => this.DI(what);

	[Export]
	private AnimationPlayer transitionPlayer;

	[Export]
	private Node dodgeHero;

	private const string FADE_OUT = "fade_out";
	private const string FADE_IN = "fade_out";

	private List<Node> sceneRoots;

	public override void _Ready()
	{
		sceneRoots = new List<Node>();
	}

	public void LoadView(View view, LoadStrategy loadStrategy) =>
		LoadScene($"{view}/{view}.tscn", loadStrategy);

	public void LoadLevel(Level level, Difficulty difficulty, LoadStrategy loadStrategy) =>
		LoadScene($"{level}/Scenes/{difficulty}.tscn", loadStrategy);
	
	private void LoadScene(string scene, LoadStrategy loadStrategy)
	{
		transitionPlayer.Seek(0);
		transitionPlayer.Play(FADE_IN, fromEnd: true);
		ResourceLoader.LoadThreadedRequest(scene);
		if (loadStrategy == LoadStrategy.Base)
		{
			sceneRoots.ForEach(sceneRoot => sceneRoot.QueueFree());
			sceneRoots.Clear();
		}
		while (ResourceLoader.LoadThreadedGetStatus(scene) == ResourceLoader.ThreadLoadStatus.InProgress) ;
		Node newView = ((PackedScene)ResourceLoader.LoadThreadedGet(scene)).Instantiate();
		sceneRoots.Add(newView);
		dodgeHero.AddChild(newView);
		transitionPlayer.Seek(0);
		transitionPlayer.Play(FADE_OUT);
	}
}
