using UnityEngine;
using System.Collections;

public class SceneManager : Singleton<SceneManager> {

	[SerializeField]
	private Animator m_SceneChangeAnimator;
	[SerializeField]
	private float m_minLoadTime = 3.0f;

	private string m_sceneToLoad;
	private float m_loadStartTime;

	public static void ChangeScene(string _newScene, bool _useTransition = true)
	{
		instance.m_sceneToLoad = _newScene;
		if (_useTransition)
			instance.m_SceneChangeAnimator.SetBool("Shown", true);
		else
			instance.LoadScene();
	}

	public void OnTransitionInFinished()
	{
		StartCoroutine(LoadScene());
		m_SceneChangeAnimator.SetBool("Shown", false);
	}

	private IEnumerator LoadScene()
	{
		Debug.Log("Loading scene: "+m_sceneToLoad);
		m_loadStartTime = Time.time;
		AsyncOperation async = Application.LoadLevelAsync(m_sceneToLoad);
		yield return async;

		while (m_loadStartTime + m_minLoadTime < Time.time)
			yield return null;

		instance.m_SceneChangeAnimator.SetBool("Shown", false);
	}

}
