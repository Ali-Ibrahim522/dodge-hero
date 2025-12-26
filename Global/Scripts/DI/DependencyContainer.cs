using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Godot;

public static class DependencyContainer
{
	private static Dictionary<Type, IInject> dependencies = new Dictionary<Type, IInject>();
	private static Dictionary<Type, IEnumerable<FieldInfo>> dependentFieldCache = new Dictionary<Type, IEnumerable<FieldInfo>>();

	public static void DI(this IDI instance, long what)
	{
		if (instance is IDepend dependent) Resolve(dependent, what);
		if (instance is IInject dependency) Inject(dependency, what);
	}

	private static void Resolve(IDepend instance, long what)
	{
		if (what == Node.NotificationReady) Resolve(instance);
	}

	private static void Inject(IInject instance, long what)
	{
		if (what == Node.NotificationEnterTree) Inject(instance);
	}
	
	private static void Inject(IInject dependency) =>
		dependencies[dependency.GetType()] = dependency;

	private static void Resolve(IDepend dependent) {
		Type dependentType = dependent.GetType();
		if (!dependentFieldCache.ContainsKey(dependentType))
		{
			dependentFieldCache[dependentType] = dependentType
				.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
				.Where(field => Attribute.IsDefined(field, typeof(InjectAttribute)));
		}

		foreach (FieldInfo field in dependentFieldCache[dependentType])
			field.SetValue(dependent, dependencies[field.FieldType]);

		Callable.From(dependent.OnResolved).CallDeferred();
	}
}
