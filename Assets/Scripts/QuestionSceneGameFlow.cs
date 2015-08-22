using UnityEngine;
using System.Collections;

public class QuestionSceneGameFlow : MonoBehaviour {

	public Contestant m_player;
	public GameObject m_playerSpotlight;

	IEnumerator Start () {

		InitialSetup();

		yield return new WaitForSeconds(2.0f);

		// Flicker spotlight
		m_playerSpotlight.SetActive(true);
		// TODO: Play spotlight sound
		yield return new WaitForSeconds(0.3f);
		m_playerSpotlight.SetActive(false);
		yield return new WaitForSeconds(0.1f);
		m_playerSpotlight.SetActive(true);
		// TODO: Play spotlight sound


	}

	public void InitialSetup()
	{
		m_playerSpotlight.gameObject.SetActive(false);
		m_player.Randomise();
	}
}
