using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Text;

public class QuestionSceneGameFlow : MonoBehaviour {

	public Contestant m_player;
	public GameObject m_playerSpotlight;
	
	public GameObject[] m_contestantSpotlights;
	public Contestant[] m_contestants;
	public Contestant m_chosenContestant = null;

	public Text m_mainText;
	public AudioSource m_mainTextAudio;

	public AudioSource m_audio;
	public AudioClip m_spotlightSFX;
	public AudioClip m_spotlightSmallSFX;
	
	public Text m_chooseContestantQuestionText;

	IEnumerator Start () {

		InitialSetup();

		yield return new WaitForSeconds(2.0f);

		// Flicker spotlight
		m_playerSpotlight.SetActive(true);
		m_audio.PlayOneShot(m_spotlightSmallSFX);
		yield return new WaitForSeconds(0.3f);
		m_playerSpotlight.SetActive(false);
		yield return new WaitForSeconds(0.1f);
		m_playerSpotlight.SetActive(true);
		m_audio.PlayOneShot(m_spotlightSmallSFX);
		yield return new WaitForSeconds(0.1f);
		m_playerSpotlight.SetActive(false);
		yield return new WaitForSeconds(0.1f);
		m_playerSpotlight.SetActive(true);
		m_audio.PlayOneShot(m_spotlightSFX);
		
		yield return new WaitForSeconds(1.0f);

		yield return StartCoroutine(DisplayText("Welcome to Nec-Romancer, the undead dating service!"));

		// yield return StartCoroutine(DisplayText("What's your name?"));

		// TODO: Text input for name

		// Move player left
		yield return StartCoroutine(MoveToPoint(m_playerSpotlight.transform, 
		                                        new Vector2(-4.91f, m_playerSpotlight.transform.position.y), 
		                                        1.0f, 
		                                        EasingFunction.QuadEaseInOut));

		// Reveal contestants
		for (int i = 0; i < m_contestantSpotlights.Length; ++i)
		{
			yield return new WaitForSeconds(0.5f);
			m_contestantSpotlights[i].SetActive(true);
			m_audio.PlayOneShot(m_spotlightSFX);
		}
		
		yield return StartCoroutine(DisplayText("Let's welcome today's contestants: "+m_contestants[0].m_name+", "+m_contestants[1].m_name+", and "+m_contestants[2].m_name+"!"));
		
		yield return StartCoroutine(DisplayText("First you'll ask them questions to get to know them."));

		yield return StartCoroutine(DisplayText("Then you'll choose one lucky contestant and a romantic destination for a date!"));
		
		yield return StartCoroutine(DisplayText("Let's get started."));
		
		yield return StartCoroutine(DisplayText("First choose a contestant to ask a question."));

		int numQuestions = 6;

		while (numQuestions > 0)
		{			
			yield return StartCoroutine(ChooseContestantForQuestion());

			if (numQuestions == 6) // FIRST TIME ONLY
				yield return StartCoroutine(DisplayText("Now choose a type of question to ask."));

			yield return StartCoroutine(ChooseQuestion());
			
			// Player's character asks question in speech bubble
			yield return StartCoroutine(AskQuestion());
			
			// Contestant answers question in speech bubble
			yield return StartCoroutine(RespondToQuestion());
			
			if (numQuestions == 6) // FIRST TIME ONLY
				yield return StartCoroutine(DisplayText("Now you know a little more about them!"));

			--numQuestions;
			if (numQuestions > 0)
			{
				yield return StartCoroutine(DisplayText("You get "+numQuestions+" more question"+(numQuestions > 1 ? "s" : "")+" to ask. Choose carefully!"));
			}
		}

		yield return StartCoroutine(DisplayText("That was it! Now, choose a romantic destination for your date."));

		// Destination choice
		yield return StartCoroutine(ChooseDestination());
		
		yield return StartCoroutine(DisplayText("And finally, choose the contestant you want by your side."));

		// Contestant choice
		yield return StartCoroutine(ChooseContestant());
		
		yield return StartCoroutine(DisplayText("Alright, time to reveal your chosen contestant!"));

		// Reveal contestant
		yield return StartCoroutine(RevealContestant());
		
		yield return StartCoroutine(DisplayText("Have a good time you two!"));

		// Scene change
		SceneManager.ChangeScene("DateScreen");
	}

	public void InitialSetup()
	{
		m_playerSpotlight.transform.position = new Vector2(0.0f, m_playerSpotlight.transform.position.y);
		m_mainText.text = "";
		m_playerSpotlight.gameObject.SetActive(false);
		m_player.Randomise();

		for (int i = 0; i < m_contestantSpotlights.Length; ++i)
			m_contestantSpotlights[i].SetActive(false);

		for (int i = 0; i < m_contestants.Length; ++i)
		{
			m_contestants[i].Randomise();
			m_contestants[i].SetGravestone(true);
		}
	}

	private IEnumerator DisplayText(string _toDisplay, float _duration = 3.0f, bool _waitForInput = false)
	{
		Color myColor = m_mainText.color;
		myColor.a = 1.0f;
		m_mainText.color = myColor;
		
		// Display text one character at a time
		StringBuilder fullString = new StringBuilder(_toDisplay);
		StringBuilder currentDisplay = new StringBuilder("");
		int index = 0;
		float perlinY = Time.time;
		while (!Input.anyKey && currentDisplay.ToString() != _toDisplay)
		{
			currentDisplay.Append(fullString[index]);
			m_mainText.text = currentDisplay.ToString();

			// Play audio with pitch variation
			if (fullString[index] != '\n' && fullString[index] != ' ')
			{
				float randomPitchVariation = Mathf.PerlinNoise(((float)index) * 0.1f, (float)perlinY);
				m_mainTextAudio.pitch = 1.75f + randomPitchVariation * 0.05f;
				m_mainTextAudio.Play();
			}

			++index;

			yield return new WaitForSeconds(0.04f);
		}

		// Display text for duration
		m_mainText.text = _toDisplay;
		float endDisplayTime = Time.time + _duration;
		while (!Input.anyKeyDown && Time.time < endDisplayTime)
		{
			yield return null;
		}
		
		// Text fades out
		yield return StartCoroutine(FadeText(m_mainText,0.5f,false));

		m_mainText.text = "";
	}

	public void ChooseContestant(Contestant _contestant)
	{
		m_chosenContestant = _contestant;
	}
	
	private IEnumerator FadeText(Text _text, float _duration, bool fadeIn)
	{
		Color myColor = _text.color;

		float startFadeTime = Time.time;
		while (Time.time < startFadeTime + _duration)
		{
			myColor.a = Easing.Ease (EasingFunction.QuadEaseInOut, Time.time - startFadeTime, 0.0f, 1.0f, _duration);
			if (!fadeIn) myColor.a = 1.0f - myColor.a;
			_text.color = myColor;
			yield return null;
		}
		myColor.a = fadeIn ? 1.0f : 0.0f;
		_text.color = myColor;

	}

	
	private IEnumerator ChooseContestantForQuestion()
	{
		// Text fades in
		yield return StartCoroutine(FadeText(m_chooseContestantQuestionText,0.5f,true));

		while (m_chosenContestant == null) // Updated by button press
			yield return null;

		// Text fades out
		yield return StartCoroutine(FadeText(m_chooseContestantQuestionText,0.5f,false));
	}

	private IEnumerator ChooseQuestion()
	{
		// TODO
		yield return null;
	}
	
	private IEnumerator AskQuestion()
	{
		// TODO
		yield return null;
	}
	
	private IEnumerator RespondToQuestion()
	{
		// TODO

		m_chosenContestant = null;
		yield return null;
	}
	
	private IEnumerator ChooseDestination()
	{
		// TODO
		yield return null;
	}
	
	private IEnumerator ChooseContestant()
	{
		m_chooseContestantQuestionText.text = "[Choose a date]";

		// Text fades in
		yield return StartCoroutine(FadeText(m_chooseContestantQuestionText,0.5f,true));
		
		while (m_chosenContestant == null) // Updated by button press
			yield return null;
		
		// Text fades out
		yield return StartCoroutine(FadeText(m_chooseContestantQuestionText,0.5f,false));
	}
	
	private IEnumerator RevealContestant()
	{
		// TODO
		yield return null;
	}

	private IEnumerator MoveToPoint(Transform _transform, Vector2 _newPosition, float _duration, EasingFunction _easingFunc )
	{
		Vector3 target = _newPosition;
		target.z = _transform.position.z;
		Vector3 starting = _transform.position;
		float startingTime = Time.time;
		Vector3 direction = (target - _transform.position).normalized;
		float totalDistance = (target - _transform.position).magnitude;
		
		while (Time.time - startingTime < _duration)
		{
			float movement = Easing.Ease (_easingFunc, Time.time - startingTime, 0, totalDistance, _duration);
			_transform.position = starting + direction * movement;
			yield return null;
		}

		_transform.position = target;
		
	}
}
