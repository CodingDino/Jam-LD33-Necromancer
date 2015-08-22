using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Contestant : MonoBehaviour {
	
	[SerializeField]
	private Color[] m_randomColors;
	
	[SerializeField]
	private ContestantRace[] m_randomRaces;
	
	[SerializeField]
	private ContestantTrait[] m_randomTraits;
	
	[SerializeField]
	private string[] m_randomNames;
	
	[SerializeField]
	private int m_numRandomTraits = 3;

	private Color m_color;
	private ContestantRace m_race;
	private List<ContestantTrait> m_traits;
	private string m_name;

	private static List<Color> m_chosenColors = new List<Color>();
	private static List<string> m_chosenNames = new List<string>();

	public void Randomise()
	{
		// Choose a name
		do {
			m_name = m_randomNames[UnityEngine.Random.Range(0,m_randomNames.Length)];
		} while (m_chosenNames.Contains(m_name));
		m_chosenNames.Add(m_name);

		// Choose a race
		m_race = m_randomRaces[UnityEngine.Random.Range(0,m_randomRaces.Length)];
		for (int i = 0; i < m_randomRaces.Length; ++i)
		{
			m_randomRaces[i].gameObject.SetActive(m_randomRaces[i] == m_race); 
		}
		
		// Choose a feature
		m_race.RandomiseFeature();

		// Choose a color
		do {
			m_color = m_randomColors[UnityEngine.Random.Range(0,m_randomColors.Length)];
		} while (m_chosenColors.Contains(m_color));
		m_chosenColors.Add(m_color);
		m_race.SetColour(m_color);

		// Get traits from race
		m_race.SupplyTraits(ref m_traits);

		// Choose random traits
		int traitsChosen = 0;
		while (traitsChosen < m_numRandomTraits)
		{
			ContestantTrait newTrait = m_randomTraits[UnityEngine.Random.Range(0,m_randomTraits.Length)];
			if (!m_traits.Contains(newTrait))
			{
				m_traits.Add(newTrait);
				++traitsChosen;
			}
		}

	}

}
