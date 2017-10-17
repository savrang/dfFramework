using System;

public abstract class msSingleton<T> where T : class, new()
{
	private static volatile T _instance;
	private static object syncRoot = new Object();


	public static T Instance
	{
		get 
		{
			lock (syncRoot) 
			{
				if (_instance == null) 
					_instance = new T();
			}

			return _instance;
		}
	}
}

//public class SingletonA : Singleton<SingletonA>