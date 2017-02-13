using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class DataBase<TABLE> : MonoBehaviour
{
	public TABLE ToObject(string data)
	{
		return JsonUtility.FromJson<TABLE> (data);
	}

	public IEnumerator Post(string url, TABLE table)
	{
		WWWForm form = new WWWForm ();
		System.Reflection.FieldInfo[] parameters = table.GetType ().GetFields ();

		for (int i = 0; parameters.Length > i; i++) 
		{
			form.AddField (parameters[i].Name, (string)parameters[i].GetValue(table));
		}

		WWW www = new WWW (url, form);
		yield return www;
	}

	public void Query(string url, ref List<TABLE> table)
	{
		StartCoroutine (QueryCoroutine (url, table));
	}

	private IEnumerator QueryCoroutine(string url, List<TABLE> table)
	{
		WWW www = new WWW (url);
		yield return www;

		string rawData = www.text;
		string data = "";

		for(int i = 0; rawData.Length > i; i++)
		{
			if (rawData [i] != ';') 
			{
				data += rawData [i];
			}
			else
			{
				table.Add(ToObject(data));
				data = "";
			}
		}
	}

	public IEnumerator CheckQuery(string url,  TABLE table, System.Action<bool> answer)
	{
		WWWForm form = new WWWForm ();
		System.Reflection.FieldInfo[] parameters = table.GetType ().GetFields ();

		for (int i = 0; parameters.Length > i; i++) 
		{
			form.AddField (parameters[i].Name, (string)parameters[i].GetValue(table));
		}

		WWW www = new WWW (url, form);
		yield return www;

		string rawData = www.text;

		if(rawData == "1")
		{
			answer(true);
		}
		else
		{
			answer(false);
		}
	}

	public IEnumerator CheckQuery(string url,  TABLE table, System.Action<bool> answer, System.Action<string> error)
	{
		WWWForm form = new WWWForm ();
		System.Reflection.FieldInfo[] parameters = table.GetType ().GetFields ();

		for (int i = 0; parameters.Length > i; i++) 
		{
			form.AddField (parameters[i].Name, (string)parameters[i].GetValue(table));
		}

		WWW www = new WWW (url, form);
		yield return www;

		string rawData = www.text;

		if(rawData == "1")
		{
			answer(true);
		}
		else
		{
			error(rawData);
			answer(false);
		}
	}
}
