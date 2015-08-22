//// ************************************************************************ 
//// File Name:   PlayerProfile.cs 
//// Purpose:    	
//// Project:		Framework
//// Author:      Sarah Herzog  
//// Copyright: 	2014 Bounder Games
//// ************************************************************************ 
//
//
//// ************************************************************************ 
//// Imports 
//// ************************************************************************ 
//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using MiniJSON;
//
//// ************************************************************************ 
//// Class: ProfileManager
//// ************************************************************************ 
//public class ProfileManager : Singleton<ProfileManager> {
//	
//	// ********************************************************************
//	// Private Data Members 
//	// ********************************************************************
//	[SerializeField]
//	private bool m_autoLoad = false;
//	[SerializeField]
//	private PlayerProfile m_profile = new PlayerProfile();
//	[SerializeField]
//	private string m_version = "0.1";
//
//	private bool m_loadedData = false;
//
//	// ********************************************************************
//	// Properties 
//	// ********************************************************************
//	public static PlayerProfile profile { 
//		get {
//			if (instance == null) return null;
//			if (instance.m_profile == null)
//				instance.m_profile = new PlayerProfile();
//			return instance.m_profile;
//		} 
//		set {
//			instance.m_profile = value;
//		}
//	}
//	public static bool loadedData { 
//		get {
//			if (instance == null)
//				return false;
//			else
//				return instance.m_loadedData;
//		}
//	}
//	
//	
//	// ********************************************************************
//	// Events 
//	// ********************************************************************
//	public delegate void MoneyChanged(int numMoneyAwarded, int newTotalMoney);
//	public static event MoneyChanged OnMoneyChanged;
//	public delegate void ItemChanged(string itemID, int numAdded, int newTotalItems);
//	public static event ItemChanged OnItemChanged;
//	public delegate void BlueprintUnlocked(string blueprintID, int newBlueprintLevel);
//	public static event BlueprintUnlocked OnBlueprintUnlocked;
//
//	
//	// ********************************************************************
//	void Start()
//	{
//		if (m_autoLoad)
//			Load (profile.saveID);
//	}
//	// ********************************************************************
//	
//	// ********************************************************************
//	public static void Load(int slot)
//	{
//		string saveID = "SaveSlot" + slot.ToString();
//		Load(saveID);
//	}
//	public static void Load(string saveID = "")
//	{
//		if (saveID == "")
//			saveID = profile.saveID;
//		
//		profile = new PlayerProfile();
//
//		string jsonString = PlayerPrefs.GetString (saveID);
//
//		if (jsonString != "")
//		{
//			Debug.Log ("ProfileManager --- LOAD "+saveID+" --- JSON String loaded: "+jsonString);
//
//			Dictionary<string,object> N = Json.Deserialize(jsonString) as Dictionary<string,object>;
//			//string version = N["version"] as string;
//			Dictionary<string,object> data = N["data"] as Dictionary<string,object>;
//
//			// LOCATION
//			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//			if (data.ContainsKey("currentTown")) 
//				profile.currentStation = data["currentTown"] as string;
//			if (data.ContainsKey("destinationTown")) 
//				profile.destinationTown = data["destinationTown"] as string;
//			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//
//			// MONEY
//			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//			if (data.ContainsKey("money")) 
//				profile.money = int.Parse(data["money"] as string);
//			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//
//			// INVENTORY
//			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//			if (data.ContainsKey("items"))
//			{
//				foreach(KeyValuePair<string, object> entry in (data["items"] as Dictionary<string, object>))
//				{
//					profile.items[entry.Key] = int.Parse(entry.Value as string);
//				}
//			}
//			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//
//			// KNOWN BLUEPRINGS
//			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//			if (data.ContainsKey("blueprints"))
//			{
//				foreach(object item in (data["blueprints"] as List<object>))
//				{
//					profile.blueprints.Add (item as string);
//				}
//			}
//			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//			
//			// EQUIPPED ITEMS
//			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//			if (data.ContainsKey("train")) 
//				profile.train = TrainData.Load(data["train"] as Dictionary<string, object>);
//			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//
//			// STATION INVENTORY
//			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//			if (data.ContainsKey("stations"))
//			{
//				foreach(KeyValuePair<string, object> entry in (data["stations"] as Dictionary<string, object>))
//				{
//					Dictionary<string, object> stationJSON = entry.Value as Dictionary<string, object>;
//					profile.stations[entry.Key] = StationData.Load(stationJSON);
//				}
//			}
//			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//		}
//		else
//		{
//			Debug.Log ("ProfileManager --- LOAD "+saveID+" --- no data found, fresh profile created");
//
//			profile.FirstTimeSetup();
//			profile.saveID = saveID;
//		}
//
//		instance.m_loadedData = true;
//
//		Save ();
//	}
//	// ********************************************************************
//
//
//	// ********************************************************************
//	public static void Save(string saveID = "")
//	{
//		if (saveID == "")
//			saveID = profile.saveID;
//
//		Dictionary<string,object> N = new Dictionary<string,object>();
//		N["version"] = instance.m_version;
//		Dictionary<string,object> data = new Dictionary<string,object>();
//
//		// LOCATION
//		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//		data["currentTown"] = profile.currentStation;
//		data["destinationTown"] = profile.destinationTown;
//		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//
//		// MONEY
//		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//		data["money"] = profile.money.ToString();
//		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//		
//		// INVENTORY
//		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//		data["items"] = new Dictionary<string,object>();
//		foreach(KeyValuePair<string, int> entry in profile.items)
//		{
//			(data["items"] as Dictionary<string,object>)[entry.Key] = entry.Value.ToString();
//		}
//		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//		
//		// KNOWN BLUEPRINTS
//		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//		data["blueprints"] = new List<string>();
//		foreach(string blueprint in profile.blueprints)
//		{
//			(data["blueprints"] as List<string>).Add(blueprint);
//		}
//		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//		
//		// EQUIPPED ITEMS
//		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//		data["train"] = profile.train.Save();
//		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//
//		// STATION INVENTORY
//		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//		data["stations"] = new Dictionary<string,object>();
//		foreach(KeyValuePair<string, StationData > entry in profile.stations)
//		{
//			(data["stations"] as Dictionary<string,object>)[entry.Key] = entry.Value.Save();
//		}
//		//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//
//		N["data"] = data;
//
//		PlayerPrefs.SetString(saveID, Json.Serialize(N));
//		PlayerPrefs.Save();
//		Debug.Log ("ProfileManager --- SAVE "+saveID+" --- JSON String saved: "+Json.Serialize(N));
//	}
//	// ********************************************************************
//
//	
//	// ********************************************************************
//	public static void Clear()
//	{
//		Clear (profile.saveID);
//	}
//	public static void Clear(int slot)
//	{
//		string saveID = "SaveSlot" + slot.ToString();
//		Clear (saveID);
//	}
//	public static void Clear(string saveID)
//	{
//		Debug.Log("ProfileManager --- CLEAR "+saveID);
//		PlayerPrefs.DeleteKey(saveID);
//	}
//	// ********************************************************************
//
//	
//	// ********************************************************************
//	public static void ClearAll()
//	{
//		PlayerPrefs.DeleteAll();
//	}
//	// ********************************************************************
//
//
//	// ********************************************************************
//	public static bool TryAddMoney(int moneyAddValue)
//	{
//		if (profile.money + moneyAddValue < 0)
//			return false;
//		else
//		{
//			profile.money += moneyAddValue;
//			if (OnMoneyChanged != null)
//				OnMoneyChanged(moneyAddValue, profile.money);
//			return true;
//		}
//	}
//	// ********************************************************************
//	
//	
//	// ********************************************************************
//	public static bool TryAddItem(string itemID, int numItemsToAdd)
//	{
//		if (!profile.items.ContainsKey(itemID))
//			profile.items[itemID] = 0;
//
//		if (profile.items[itemID] + numItemsToAdd < 0)
//		{
//			return false;
//		}
//		else
//		{
//			profile.items[itemID] += numItemsToAdd;
//			
//			if (OnItemChanged != null)
//				OnItemChanged(itemID, numItemsToAdd, profile.items[itemID]);
//
//			return true;
//		}
//	}
//	// ********************************************************************
//	
//	
//	// ********************************************************************
//	public static bool TryAddBlueprint(string blueprintID, int level = 1)
//	{
//		// TODO: Blueprint levels (for leveling up weapons / carts / engine parts)
//
//		if (profile.blueprints.Contains(blueprintID))
//		{
//		    return false;
//		}
//		else
//		{
//			profile.blueprints.Add(blueprintID);
//
//			if (OnBlueprintUnlocked != null)
//				OnBlueprintUnlocked(blueprintID, level);
//
//			return true;
//		}
//	}
//	// ********************************************************************
//	
//	
//	// ********************************************************************
//	public static int GetNumEquipped(string _itemID)
//	{
//		ItemDefinition itemData = ItemDatabase.GetItemDefinition(_itemID);
//
//		int numEquipped = 0;
//		List<CartData> carts = profile.train.carts;
//
//		switch (itemData.type)
//		{
//		case ItemType.CART :
//			for (int i = 0; i < carts.Count; ++i)
//			{
//				if (carts[i].id == _itemID)
//					++numEquipped;
//			}
//			break;
//		case ItemType.WEAPON :
//			for (int i = 0; i < carts.Count; ++i)
//			{
//				if (carts[i].weapon != null && carts[i].weapon.id == _itemID)
//					++numEquipped;
//			}
//			break;
//		case ItemType.COMPONENT :
//			for (int i = 0; i < (int)ComponentType.NUM; ++i)
//			{
//				ComponentData component = profile.train.components[i];
//				if (component != null && component.id == _itemID)
//					++numEquipped;
//			}
//			break;
//		case ItemType.CONSUMABLE :
//			for (int i = 0; i < 3; ++i)
//			{
//				ConsumableData consumable = profile.train.consumables[i];
//				if (consumable != null && consumable.id == _itemID)
//					++numEquipped;
//			}
//			break;
//		}
//
//		return numEquipped;
//
//	}
//	// ********************************************************************
//
//	
//	// ********************************************************************
//	public static SaveSlotData GetSaveSlotInfo(int slot)
//	{
//		SaveSlotData slotInfo = null;
//
//		string saveID = "SaveSlot" + slot.ToString();
//					
//		string jsonString = PlayerPrefs.GetString (saveID);
//		
//		if (jsonString != "")
//		{
//			slotInfo = new SaveSlotData();
//
//			Dictionary<string,object> N = Json.Deserialize(jsonString) as Dictionary<string,object>;
//			//string version = N["version"] as string;
//			Dictionary<string,object> data = N["data"] as Dictionary<string,object>;
//
//			// TODO: Determine zone
//			slotInfo.zone = "ZN-YellowSands";
//
//			// TODO: Determine completion
//			slotInfo.completion = 0;
//			
//			// MONEY
//			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//			if (data.ContainsKey("money")) 
//				slotInfo.money = int.Parse(data["money"] as string);
//			//~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~
//		}
//
//		return slotInfo;
//
//	}
//	// ********************************************************************
//}
//
//
//// ************************************************************************ 
//// Class: PlayerProfile
//// ************************************************************************ 
//[System.Serializable]
//// ************************************************************************ 
//public class SaveSlotData {
//
//	public string zone;
//	public int completion;
//	public int money;
//
//}
//
//
//// ************************************************************************ 
//// Class: PlayerProfile
//// ************************************************************************ 
//[System.Serializable]
//// ************************************************************************ 
//public class PlayerProfile {
//	
//	public delegate void LoadoutUpdateEvent();
//	public static event LoadoutUpdateEvent OnLoadoutUpdated;
//	public static void LoadoutUpdated() 
//	{ 
//		if (OnLoadoutUpdated != null) OnLoadoutUpdated(); 
//	}
//
//	public string saveID = "SaveSlot01";
//	public string version = "0.1";
//
//	public string currentStation = "ST-TumbledownTown";
//	public string destinationTown = "ST-TumbledownTown";
//	public string currentLevel = "";
//
//	public int money = 0;
//	public Dictionary<string,int> items = new Dictionary<string, int>();
//	public List<string> blueprints = new List<string>();
//
//	public TrainData train = new TrainData();
//
//	public Dictionary<string, StationData > stations = new Dictionary<string, StationData >();
//
//    public List<string> completedQuests = new List<string>();
//    public List<string> conversationsSeen = new List<string>();
//    public List<string> choicesMade = new List<string>();
//	public HashSet<string> unlockedTowns;
//
//	public List<string> tutorialsSeen = new List<string>();
//	
//	public void FirstTimeSetup()
//	{
//		SetupStartingTrain();
//		SetupStartingStations();
//	}
//
//	public void SetupStartingTrain()
//	{
//		// Starting cart
//		items["CR-Wood"] = 2;
//		TrainPartData.CreatePart(ItemDatabase.GetItemDefinition("CR-Wood")).Equip(0,true);
//		TrainPartData.CreatePart(ItemDatabase.GetItemDefinition("CR-Wood")).Equip(1,true);
//		items["WP-TeslaCoil"] = 2;
//		TrainPartData.CreatePart(ItemDatabase.GetItemDefinition("WP-TeslaCoil")).Equip(1,true);
//
//		// Starting coal
//		CargoData initialCoal = TrainPartData.CreatePart(ItemDatabase.GetItemDefinition("CG-Coal")) as CargoData;
//		initialCoal.originStation = "ST-TumbledownTown";
//		initialCoal.destination = "ST-DesertCrossing";
//		initialCoal.rewardItem = "";
//		initialCoal.rewardItemNum = 0;
//		initialCoal.rewardMoney = 10;
//		initialCoal.daysRemaining = 7;
//		initialCoal.rare = false;
//		initialCoal.quest = false;
//		
//		train.carts[0].cargo = initialCoal; // MANUAL equip to avoid attempt to remove from station inventory
//
//		// Starting equipment in inventory
//		items["CR-Reinforced"] = 1;
//	}
//
//	public void SetupStartingStations()
//	{
//		// Create blank station entries for all stations
//		foreach (KeyValuePair<string, StationDefinition> entry in StationDatabase.GetAllStationDefs())
//		{
//			stations[entry.Key] = new StationData();
//		}
//
//		// TODO: Set up very specific initial jobs
//		stations["ST-TumbledownTown"].inventory.Add(StationDatabase.GetStationDef("ST-TumbledownTown").GenerateJob());
//		stations["ST-TumbledownTown"].inventory.Add(StationDatabase.GetStationDef("ST-TumbledownTown").GenerateJob());
//		stations["ST-TumbledownTown"].inventory.Add(StationDatabase.GetStationDef("ST-TumbledownTown").GenerateJob());
//		stations["ST-TumbledownTown"].inventory.Add(StationDatabase.GetStationDef("ST-TumbledownTown").GenerateJob());
//		stations["ST-TumbledownTown"].inventory.Add(StationDatabase.GetStationDef("ST-TumbledownTown").GenerateJob());
//
//		// TODO: Unlock initial stations
//		
//	}
//
//	public int CalculateTrainWeight()
//	{
//		int weight = 0;
//
//		for (int i = 0; i < train.carts.Count; ++i)
//		{
//			weight += ItemDatabase.GetItemDefinition(train.carts[i].id).weight;
//			if (train.carts[i].cargo != null)
//				weight += ItemDatabase.GetItemDefinition(train.carts[i].cargo.id).weight;
//			if (train.carts[i].weapon != null)
//				weight += ItemDatabase.GetItemDefinition(train.carts[i].weapon.id).weight;
//		}
//		for (int i = 0; i < train.components.Length; ++i)
//		{
//			if (train.components[i] != null)
//				weight += ItemDatabase.GetItemDefinition(train.components[i].id).weight;
//		}
//		for (int i = 0; i < train.consumables.Length; ++i)
//		{
//			if (train.consumables[i] != null)
//				weight += ItemDatabase.GetItemDefinition(train.consumables[i].id).weight;
//		}
//
//		return weight;
//	}
//}