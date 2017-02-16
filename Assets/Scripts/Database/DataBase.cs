using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class DataBase
{
	private static object ToObject(string data)
	{
		return JsonUtility.FromJson<object> (data);
	}

	private static WWW Request(string url, object table)
	{
		WWWForm form = new WWWForm ();
		System.Reflection.FieldInfo[] parameters = table.GetType ().GetFields ();

		for (int i = 0; parameters.Length > i; i++) 
		{
			form.AddField (parameters[i].Name, (string)parameters[i].GetValue(table));
		}

		return new WWW (url, form);
	}

	public static void Post(string url, object table)
	{
		WWWForm form = new WWWForm ();
		System.Reflection.FieldInfo[] parameters = table.GetType ().GetFields ();

		for (int i = 0; parameters.Length > i; i++) 
		{
			form.AddField (parameters[i].Name, parameters[i].GetValue(table).ToString());
		}

		WWW www = new WWW (url, form);


		System.Threading.ThreadPool.QueueUserWorkItem (new System.Threading.WaitCallback(Post), www); // TODO
	}

	public static void Fetch(string url, object table)
	{
		WWW www = new WWW (url);
		while (!www.isDone) { Debug.Log("test"); }

		table = ToObject(www.text);
	}

	public static void Fetch(string url, object table, char split)
	{
		WWW www = new WWW (url);
		while (!www.isDone) 
		{
			Debug.Log("test");
		}

		string rawData = www.text;
		string data = "";

		for(int i = 0; rawData.Length > i; i++)
		{
			if (rawData [i] != split) 
			{
				data += rawData [i];
			}
			else
			{
				object[] parameters = new object[1];
				parameters [0] = ToObject (data);

				table.GetType ().GetMethod ("Add").Invoke (table, parameters);
				data = "";
			}
		}
	}

	public static bool CheckQuery(string url,  object table)
	{
		WWW www = Request (url, table);

		while (!www.isDone) { Debug.Log("test"); }

		if(www.text != "") { return true; }
		else { return false; }
	}

	public static bool CheckQuery(string url,  object table, System.Action<string> comment)
	{
		WWW www = Request (url, table);

		while (!www.isDone) { Debug.Log("test"); }

		if(www.text != "")
		{
			comment(www.text);
			return true;
		}
		else { return false; }
	}
}
