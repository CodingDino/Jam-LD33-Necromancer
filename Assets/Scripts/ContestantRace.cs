using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum QuirkType
{
	INVALID = -1,
	// ==
	LONG_OOO = 0,
	W_FOR_V,
	// ==
	NUM
}

public class ContestantRace : MonoBehaviour {

	[SerializeField]
	private SpriteRenderer[] m_randomFeatures;

	[SerializeField]
	private TraitType[] m_racialTraits;

	[SerializeField]
	private QuirkType m_speechQuirk = QuirkType.INVALID;

	private SpriteRenderer m_feature;

	private static List<Sprite> m_chosenFeatures = new List<Sprite>();

	public void SetColour(Color _newColor)
	{
		m_feature.color = _newColor;
	}

	public void RandomiseFeature()
	{
//		do {
			m_feature = m_randomFeatures[UnityEngine.Random.Range(0,m_randomFeatures.Length)];
//		} while (m_chosenFeatures.Contains(m_feature.sprite));
		m_chosenFeatures.Add(m_feature.sprite);
		for (int i = 0; i < m_randomFeatures.Length; ++i)
		{
			m_randomFeatures[i].gameObject.SetActive(m_randomFeatures[i] == m_feature); 
		}
	}

	public void SupplyTraits(ref List<TraitType> _traits)
	{
		for (int i = 0; i < m_racialTraits.Length; ++i)
		{
			_traits.Add(m_racialTraits[i]);
		}
	}

	public string ProcessSpeechQuirk (string _text)
	{
		switch (m_speechQuirk)
		{
		case QuirkType.LONG_OOO :
		{
			_text =  _text.Replace("oo","ooOOoo");
		}
			break;
		case QuirkType.W_FOR_V :
		{
			_text =  _text.Replace("w","v");
			_text =  _text.Replace("W","V");
		}
			break;
		}
		return _text;
	}

}
