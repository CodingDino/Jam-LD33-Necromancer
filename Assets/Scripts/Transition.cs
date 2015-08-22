using UnityEngine;
using System.Collections;

public class Transition : MonoBehaviour {

	[SerializeField]
	private SceneManager manager;

	public void OnTransitionInFinished()
	{
		manager.OnTransitionInFinished();
	}
}
