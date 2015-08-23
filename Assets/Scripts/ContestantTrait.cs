using UnityEngine;
using System.Collections;

public enum TraitType
{
	INVALID = -1,
	// ==

	SHY, // Doesn't like to be the center of attention
	BOLD, // Likes to be the center of attention

	QUIET, // Doesn't like to talk
	TALKATIVE, // Likes to talk

	RELAXED, // Likes inactive things
	RESTLESS, // Doesn't like inactive things
	ACTIVE, // Likes active things
	LAZY, // Doesn't like active things
	
	GROUCHY, // Negative points are increased
	FORGIVING, // Negative points are reduced

	CYNICAL, // Positive points are decreased
	OPTIMISTIC, // Positive points are increased

	// ==
	NUM_RANDOM,
	// ==

	NO_PHYSICAL_FORM,
	HATES_SUNLIGHT,
	CANT_EAT,
	ALLERGIC_TO_GARLIC,
	LIKES_BRAINS,
	LIKES_BLOOD
}

[System.Serializable]
public class ContestantTrait  {


}
