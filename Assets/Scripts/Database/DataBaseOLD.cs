using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class DataBaseOLD<TABLE> : MonoBehaviour
{
	private WWW www;

	public TABLE ToObject(string data)
	{
		return JsonUtility.FromJson<TABLE> (data);
	}

	/// <summary>
	/// Post the specified table.
	/// </summary>
	/// <param name="url">URL.</param>
	/// <param name="table">Table.</param>
	public IEnumerator Post(string url, TABLE table)
	{
		WWWForm form = new WWWForm ();
		System.Reflection.FieldInfo[] parameters = table.GetType ().GetFields ();

		for (int i = 0; parameters.Length > i; i++) 
		{
			form.AddField (parameters[i].Name, (string)parameters[i].GetValue(table));
		}

		www = new WWW (url, form);
		yield return www;
	}

	/// <summary>
	/// Query the specified url and return a single value.
	/// </summary>
	/// <param name="url">URL.</param>
	/// <param name="data">Data.</param>
	public IEnumerator Query(string url, string data)
	{
		www = new WWW (url);
		yield return www;

		data = www.text;
	}

	/// <summary>
	/// Query the specified url and return a single table.
	/// </summary>
	/// <param name="url">URL.</param>
	/// <param name="data">Data.</param>
	public IEnumerator Query(string url, TABLE table)
	{
		www = new WWW (url);
		yield return www;

		table = ToObject(www.text);
	}

	/// <summary>
	/// Query the specified url and return an array of table value.
	/// </summary>
	/// <param name="url">URL.</param>
	/// <param name="data">Data.</param>
	public IEnumerator Query(string url, List<TABLE> table)
	{
		www = new WWW (url);
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

	private IEnumerator CheckQuery(string url,  TABLE table)
	{
		WWWForm form = new WWWForm ();
		System.Reflection.FieldInfo[] parameters = table.GetType ().GetFields ();

		for (int i = 0; parameters.Length > i; i++) 
		{
			form.AddField (parameters[i].Name, (string)parameters[i].GetValue(table));
		}

		www = new WWW (url, form);
		yield return www;
	}

	/// <summary>
	/// Checks if the table exist in the database.
	/// </summary>
	/// <returns>The query.</returns>
	/// <param name="url">URL.</param>
	/// <param name="table">Table.</param>
	/// <param name="answer">Answer.</param>
	public IEnumerator CheckQuery(string url,  TABLE table, System.Action<bool> answer)
	{
		yield return StartCoroutine (CheckQuery (url, table));

		if(www.text != null) { answer(true); }
		else { answer(false); }
	}

	/// <summary>
	/// Checks if the table exist and return the id.
	/// </summary>
	/// <returns>The query.</returns>
	/// <param name="url">URL.</param>
	/// <param name="table">Table.</param>
	/// <param name="answer">Answer.</param>
	/// <param name="id">Identifier.</param>
	public IEnumerator CheckQuery(string url,  TABLE table, System.Action<bool> answer, System.Action<int> id)
	{
		yield return StartCoroutine (CheckQuery (url, table));

		if(www.text != "")
		{
			answer(true);
			id(int.Parse (www.text));
		}
		else
		{
			answer(false);
		}
	}
	/// <summary>
	/// Checks if the table exist, if not return the error speficified in php.
	/// </summary>
	/// <returns>The query.</returns>
	/// <param name="url">URL.</param>
	/// <param name="table">Table.</param>
	/// <param name="answer">Answer.</param>
	/// <param name="error">Error.</param>
	public IEnumerator CheckQuery(string url,  TABLE table, System.Action<bool> answer, System.Action<string> error)
	{
		yield return StartCoroutine (CheckQuery (url, table));

		if(www.text == "1")
		{
			answer(true);
		}
		else
		{
			error(www.text);
			answer(false);
		}
	}
}
