using UnityEngine;
using System.Collections;

public class SceneChangeButton : MonoBehaviour {

	public void ChangeScene(string _newScene)
	{
		SceneManager.ChangeScene(_newScene);
	}

}
