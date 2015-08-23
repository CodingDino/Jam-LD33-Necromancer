using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Text;

public enum Questions
{
	INVALID = -1,
	
	// == WHAT
	
	WHAT_IS_YOUR_FAVORITE_COLOR,
	WHAT_IS_YOUR_FAVORITE_SEASON,
	WHAT_IS_YOUR_FAVORITE_BOOK,
	WHAT_IS_YOUR_FAVORITE_MOVIE,

	WHAT_DO_YOU_DO_FOR_A_LIVING,
	WHAT_DO_YOU_DO_IN_YOUR_FREE_TIME,
	WHAT_DO_YOU_WISH_YOU_COULD_CHANGE,
	WHAT_DO_YOU_PLAN_TO_DO_WHEN_YOU_RETIRE,

	WHAT_ARE_YOU_LOOKING_FOR_A,
	WHAT_ARE_YOU_LOOKING_FOR_B,
	WHAT_ARE_YOU_LOOKING_FOR_C,
	WHAT_ARE_YOU_LOOKING_FOR_D,

	WHAT_WORD_DESCRIBES_A,
	WHAT_WORD_DESCRIBES_B,
	WHAT_WORD_DESCRIBES_C,
	WHAT_WORD_DESCRIBES_D,
	
	// == WHERE
	
	WHERE_A_A,
	WHERE_A_B,
	WHERE_A_C,
	WHERE_A_D,

	WHERE_B_A,
	WHERE_B_B,
	WHERE_B_C,
	WHERE_B_D,

	WHERE_C_A,
	WHERE_C_B,
	WHERE_C_C,
	WHERE_C_D,

	WHERE_D_A,
	WHERE_D_B,
	WHERE_D_C,
	WHERE_D_D,

	// == HOW

	HOW_A_A,
	HOW_A_B,
	HOW_A_C,
	HOW_A_D,

	HOW_B_A,
	HOW_B_B,
	HOW_B_C,
	HOW_B_D,

	HOW_C_A,
	HOW_C_B,
	HOW_C_C,
	HOW_C_D,

	HOW_D_A,
	HOW_D_B,
	HOW_D_C,
	HOW_D_D,

	// == IF...

	IF_YOU_COULD_A,
	IF_YOU_COULD_B,
	IF_YOU_COULD_C,
	IF_YOU_COULD_D,

	IF_SOMEONE_A,
	IF_SOMEONE_B,
	IF_SOMEONE_C,
	IF_SOMEONE_D,

	IF_YOU_WERE_A,
	IF_YOU_WERE_B,
	IF_YOU_WERE_C,
	IF_YOU_WERE_D,

	IF_WE_WERE_A,
	IF_WE_WERE_B,
	IF_WE_WERE_C,
	IF_WE_WERE_D,

	// ==
	NUM,
}

public class QuestionSceneGameFlow : MonoBehaviour {

	public Contestant m_player;
	public GameObject m_playerSpotlight;
	
	public GameObject[] m_contestantSpotlights;
	public Contestant[] m_contestants;
	public Contestant m_chosenContestant = null;
	private int m_contestantIndex = -1;

	public Text m_mainText;
	public AudioSource m_mainTextAudio;

	public AudioSource m_audio;
	public AudioClip m_spotlightSFX;
	public AudioClip m_spotlightSmallSFX;
	
	public Text m_chooseContestantText;
	public Text m_chooseQuestionText;
	public Text[] m_questionButtonText;
	public Animator[] m_questionButtonAnimators;
	
	public Text m_playerSpeechBubbleText;
	public Animator m_playerSpeechBubbleAnimator;
	public Text[] m_contestorSpeechBubbleText;
	public Animator[] m_contestorSpeechBubbleAnimator;
	
	public Text m_chooseLocationText;
	public Animator[] m_locationButtonAnimators;
	public GameObject m_locationsRoot;
	
	private int m_chosenLocation = -1;

	private int m_chosenQuestion = -1;
	private int m_questionChoice1 = -1;
	private int m_questionChoice2 = -1;
	private int m_questionChoice3 = -1;

	private List<string> m_questionText1;
	private List<List<string> > m_questionText2;
	private List<List<List<string> >> m_questionText3;

	IEnumerator Start () 
	{
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

		yield return StartCoroutine(DisplayText("Then you'll choose one lucky contestant for a date!"));
		
		yield return StartCoroutine(DisplayText("You'll also get to choose a romantic destination for your date."));
		
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

		SetupQuestionText();
	}

	public void SetupQuestionText()
	{
		m_questionText1 = new List<string>();
		m_questionText2 = new List<List<string>>();
		m_questionText3 = new List<List<List<string>>>();
		int i1, i2;

		// == WHAT
		i1 = 0;
		m_questionText1.Add("What");
		m_questionText2.Add(new List<string>());
		m_questionText3.Add(new List<List<string>>());

		i2 = 0;
		m_questionText2[i1].Add("is your favorite");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("color");
		m_questionText3[i1][i2].Add("season");
		m_questionText3[i1][i2].Add("book");
		m_questionText3[i1][i2].Add("movie");
		
		i2 = 1;
		m_questionText2[i1].Add("do you");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("do for a living");
		m_questionText3[i1][i2].Add("do in your free time");
		m_questionText3[i1][i2].Add("wish you could change");
		m_questionText3[i1][i2].Add("plan to do when you retire");
		
		i2 = 2;
		m_questionText2[i1].Add("are you looking for");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("a");
		m_questionText3[i1][i2].Add("b");
		m_questionText3[i1][i2].Add("c");
		m_questionText3[i1][i2].Add("d");
		
		i2 = 3;
		m_questionText2[i1].Add("word describes");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("a");
		m_questionText3[i1][i2].Add("b");
		m_questionText3[i1][i2].Add("c");
		m_questionText3[i1][i2].Add("d");
		
		// == WHERE
		i1 = 1;
		m_questionText1.Add("Where");
		m_questionText2.Add(new List<string>());
		m_questionText3.Add(new List<List<string>>());
		
		i2 = 0;
		m_questionText2[i1].Add("a");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("a");
		m_questionText3[i1][i2].Add("b");
		m_questionText3[i1][i2].Add("c");
		m_questionText3[i1][i2].Add("d");
		
		i2 = 1;
		m_questionText2[i1].Add("b");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("a");
		m_questionText3[i1][i2].Add("b");
		m_questionText3[i1][i2].Add("c");
		m_questionText3[i1][i2].Add("d");

		i2 = 2;
		m_questionText2[i1].Add("c");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("a");
		m_questionText3[i1][i2].Add("b");
		m_questionText3[i1][i2].Add("c");
		m_questionText3[i1][i2].Add("d");

		i2 = 3;
		m_questionText2[i1].Add("d");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("a");
		m_questionText3[i1][i2].Add("b");
		m_questionText3[i1][i2].Add("c");
		m_questionText3[i1][i2].Add("d");
		
		// == HOW
		i1 = 2;
		m_questionText1.Add("How");
		m_questionText2.Add(new List<string>());
		m_questionText3.Add(new List<List<string>>());
		
		i2 = 0;
		m_questionText2[i1].Add("a");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("a");
		m_questionText3[i1][i2].Add("b");
		m_questionText3[i1][i2].Add("c");
		m_questionText3[i1][i2].Add("d");
		
		i2 = 1;
		m_questionText2[i1].Add("b");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("a");
		m_questionText3[i1][i2].Add("b");
		m_questionText3[i1][i2].Add("c");
		m_questionText3[i1][i2].Add("d");
		
		i2 = 2;
		m_questionText2[i1].Add("c");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("a");
		m_questionText3[i1][i2].Add("b");
		m_questionText3[i1][i2].Add("c");
		m_questionText3[i1][i2].Add("d");
		
		i2 = 3;
		m_questionText2[i1].Add("d");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("a");
		m_questionText3[i1][i2].Add("b");
		m_questionText3[i1][i2].Add("c");
		m_questionText3[i1][i2].Add("d");
		
		// == IF...
		i1 = 3;
		m_questionText1.Add("If");
		m_questionText2.Add(new List<string>());
		m_questionText3.Add(new List<List<string>>());
		
		i2 = 0;
		m_questionText2[i1].Add("you could");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("a");
		m_questionText3[i1][i2].Add("b");
		m_questionText3[i1][i2].Add("c");
		m_questionText3[i1][i2].Add("d");
		
		i2 = 1;
		m_questionText2[i1].Add("someone");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("a");
		m_questionText3[i1][i2].Add("b");
		m_questionText3[i1][i2].Add("c");
		m_questionText3[i1][i2].Add("d");
		
		i2 = 2;
		m_questionText2[i1].Add("you were");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("a");
		m_questionText3[i1][i2].Add("b");
		m_questionText3[i1][i2].Add("c");
		m_questionText3[i1][i2].Add("d");
		
		i2 = 3;
		m_questionText2[i1].Add("we were");
		m_questionText3[i1].Add(new List<string>());
		m_questionText3[i1][i2].Add("a");
		m_questionText3[i1][i2].Add("b");
		m_questionText3[i1][i2].Add("c");
		m_questionText3[i1][i2].Add("d");
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
		yield return null;
		
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
		for (int i = 0; i < m_contestants.Length; ++i)
		{
			if (m_contestants[i] == _contestant)
				m_contestantIndex = i;
		}
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
		yield return StartCoroutine(FadeText(m_chooseContestantText,0.5f,true));
		
		m_chosenContestant = null;
		while (m_chosenContestant == null) // Updated by button press
			yield return null;

		// Text fades out
		yield return StartCoroutine(FadeText(m_chooseContestantText,0.5f,false));
	}
	
	public void QuestionButtonPressed(int _chosenQuestion)
	{
		m_chosenQuestion = _chosenQuestion;
	}

	private IEnumerator ChooseQuestion()
	{
		// Setup first question selection
		m_chooseQuestionText.text = "[Choose a question to ask]";
		for (int i = 0; i < m_questionButtonText.Length; ++i)
		{
			m_questionButtonText[i].transform.parent.gameObject.SetActive(false);
		}
		for (int i = 0; i < m_questionText1.Count; ++i)
		{
			m_questionButtonText[i].text = m_questionText1[i];
			m_questionButtonText[i].transform.parent.gameObject.SetActive(true);
		}

		//  Show first question selection
		StartCoroutine(FadeText(m_chooseQuestionText,0.5f,true));
		for (int i = 0; i < m_questionButtonAnimators.Length; ++i)
		{
			m_questionButtonAnimators[i].SetBool("Hidden",false);
			yield return new WaitForSeconds(0.04f);
		}

		// Wait for choice
		while (m_chosenQuestion == -1) // Updated by button press
			yield return null;

		// Hide first question selection
		StartCoroutine(FadeText(m_chooseQuestionText,0.5f,false));
		for (int i = 0; i < m_questionButtonAnimators.Length; ++i)
		{
			m_questionButtonAnimators[i].SetBool("Hidden",true);
			yield return new WaitForSeconds(0.04f);
		}

		// Store first question selection
		m_questionChoice1 = m_chosenQuestion;
		m_chosenQuestion = -1;

		// Setup second question selection
		m_chooseQuestionText.text = "["+m_questionText1[m_questionChoice1]+"...]";
		for (int i = 0; i < m_questionButtonText.Length; ++i)
		{
			m_questionButtonText[i].transform.parent.gameObject.SetActive(false);
		}
		for (int i = 0; i < m_questionText2[m_questionChoice1].Count; ++i)
		{
			m_questionButtonText[i].text = m_questionText2[m_questionChoice1][i];
			m_questionButtonText[i].transform.parent.gameObject.SetActive(true);
		}
		
		// Show second question selection
		StartCoroutine(FadeText(m_chooseQuestionText,0.5f,true));
		for (int i = 0; i < m_questionButtonAnimators.Length; ++i)
		{
			m_questionButtonAnimators[i].SetBool("Hidden",false);
			yield return new WaitForSeconds(0.04f);
		}
		
		// Wait for choice
		while (m_chosenQuestion == -1) // Updated by button press
			yield return null;
		
		// Hide second question selection
		StartCoroutine(FadeText(m_chooseQuestionText,0.5f,false));
		for (int i = 0; i < m_questionButtonAnimators.Length; ++i)
		{
			m_questionButtonAnimators[i].SetBool("Hidden",true);
			yield return new WaitForSeconds(0.04f);
		}
		
		// Store second question selection
		m_questionChoice2 = m_chosenQuestion;
		m_chosenQuestion = -1;
		
		// Setup third question selection
		m_chooseQuestionText.text = "["+m_questionText1[m_questionChoice1]+" "+m_questionText2[m_questionChoice1][m_questionChoice2]+"...]";
		for (int i = 0; i < m_questionButtonText.Length; ++i)
		{
			m_questionButtonText[i].transform.parent.gameObject.SetActive(false);
		}
		for (int i = 0; i < m_questionText3[m_questionChoice1][m_questionChoice2].Count; ++i)
		{
			m_questionButtonText[i].text = m_questionText3[m_questionChoice1][m_questionChoice2][i];
			m_questionButtonText[i].transform.parent.gameObject.SetActive(true);
		}
		
		// Show third question selection
		StartCoroutine(FadeText(m_chooseQuestionText,0.5f,true));
		for (int i = 0; i < m_questionButtonAnimators.Length; ++i)
		{
			m_questionButtonAnimators[i].SetBool("Hidden",false);
			yield return new WaitForSeconds(0.04f);
		}
		
		// Wait for choice
		while (m_chosenQuestion == -1) // Updated by button press
			yield return null;
		
		// Hide third question selection
		StartCoroutine(FadeText(m_chooseQuestionText,0.5f,false));
		for (int i = 0; i < m_questionButtonAnimators.Length; ++i)
		{
			m_questionButtonAnimators[i].SetBool("Hidden",true);
			yield return new WaitForSeconds(0.04f);
		}
		
		// Store third question selection
		m_questionChoice3 = m_chosenQuestion;
		m_chosenQuestion = -1;
		
		yield return null;

	}
	
	private IEnumerator AskQuestion()
	{
		m_playerSpeechBubbleText.text = m_questionText1[m_questionChoice1]+" "
				+m_questionText2[m_questionChoice1][m_questionChoice2]+" "
				+m_questionText3[m_questionChoice1][m_questionChoice2][m_questionChoice3]+"?";

		m_playerSpeechBubbleAnimator.SetBool("Shown",true);
		
		yield return new WaitForSeconds(3.0f);

		m_playerSpeechBubbleAnimator.SetBool("Shown",false);

		yield return new WaitForSeconds(0.2f);
	}
	
	private IEnumerator RespondToQuestion()
	{
		Questions question = (Questions)(m_questionChoice1 * 4 * 4 + m_questionChoice2 * 4 + m_questionChoice3);
		m_contestorSpeechBubbleText[m_contestantIndex].text = m_chosenContestant.GetQuestionResponse(question);
		
		m_contestorSpeechBubbleAnimator[m_contestantIndex].SetBool("Shown",true);
		
		yield return new WaitForSeconds(3.0f);
		
		m_contestorSpeechBubbleAnimator[m_contestantIndex].SetBool("Shown",false);
		
		yield return new WaitForSeconds(0.2f);

		// Clear choices for next round
		m_chosenContestant = null;
		m_questionChoice1 = -1;
		m_questionChoice2 = -1;
		m_questionChoice3 = -1;
		yield return null;
	}
	
	public void LocationButtonPressed(int _choice)
	{
		m_chosenLocation = _choice;
	}
	
	private IEnumerator ChooseDestination()
	{		
		//  Show first question selection
		m_locationsRoot.SetActive(true);
		StartCoroutine(FadeText(m_chooseLocationText,0.5f,true));
		for (int i = 0; i < m_locationButtonAnimators.Length; ++i)
		{
			m_locationButtonAnimators[i].SetBool("Hidden",false);
			yield return new WaitForSeconds(0.04f);
		}
		
		// Wait for choice
		while (m_chosenLocation == -1) // Updated by button press
			yield return null;
		
		// Hide first question selection
		StartCoroutine(FadeText(m_chooseLocationText,0.5f,false));
		for (int i = 0; i < m_locationButtonAnimators.Length; ++i)
		{
			m_locationButtonAnimators[i].SetBool("Hidden",true);
			yield return new WaitForSeconds(0.04f);
		}

		yield return null;
	}
	
	private IEnumerator ChooseContestant()
	{
		m_chooseContestantText.text = "[Choose your date]";

		// Text fades in
		yield return StartCoroutine(FadeText(m_chooseContestantText,0.5f,true));
		
		while (m_chosenContestant == null) // Updated by button press
			yield return null;
		
		// Text fades out
		yield return StartCoroutine(FadeText(m_chooseContestantText,0.5f,false));
	}
	
	private IEnumerator RevealContestant()
	{
		m_chosenContestant.SetGravestone(false);
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
