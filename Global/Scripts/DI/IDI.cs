public interface IDI { }

public interface IInject : IDI {}

public interface IDepend : IDI
{
    public void OnResolved() { }
}
