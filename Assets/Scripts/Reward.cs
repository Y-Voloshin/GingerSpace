using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward  {
	static Reward _empty = new Reward();
	public static Reward Empty {get { return _empty;}}
	public bool IsEmpty = true;
	public int Fuel,
			
	Expa,
	Strength,
	Exploration,
	Diplomacy,
	Management;
}
