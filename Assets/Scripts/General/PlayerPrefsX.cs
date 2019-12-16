using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPrefsX : MonoBehaviour
{
	/// <summary>
	/// Sets the bool value for the given playerpref
	/// </summary>
	/// <param name="name">Name of the playerpref</param>
	/// <param name="booleanValue">Boolean value for the playerpref</param>
	public static void SetBool(string name, bool booleanValue)
	{
		PlayerPrefs.SetInt(name, booleanValue ? 1 : 0);
	}

	/// <summary>
	/// Get the bool value of the given playerpref
	/// </summary>
	/// <param name="name">Name of the playerpref</param>
	/// <returns></returns>
	public static bool GetBool(string name)
	{
		return PlayerPrefs.GetInt(name) == 1 ? true : false;
	}

	/// <summary>
	/// Get the bool value of the given playerpref, returns defaultValue if given playerpref doesn't exist
	/// </summary>
	/// <param name="name">Name of the playerpref</param>
	/// <param name="defaultValue">This boolean value gets returned if given name doesn't exist</param>
	/// <returns></returns>
	public static bool GetBool(string name, bool defaultValue)
	{
		if (PlayerPrefs.HasKey(name))
		{
			return GetBool(name);
		}

		return defaultValue;
	}
}
