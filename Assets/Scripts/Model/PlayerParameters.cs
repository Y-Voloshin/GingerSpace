using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerParameters  {

	public int FuelMax = 10,
				FuelCurrent = 10,
				
				//Хоть что-то игрок всегда может сделать, так что сила у него минимум 1
				StrengthMin = 1,
				StrengthCurrent,
					
				DiplomacyMax,
				DiplomacyCurrent,//Нет максимума и минимума

				//Ну и поиск как и сила не может быть нулевой
				ExplorationMin = 1,
				ExplorationCurrent = 10,

				ManagementMax,
				ManagementCurrent,

				ExpreiencePoints;
	public int EmptyFuelPoints { get { return FuelMax - FuelCurrent;} }

	public void CopyValuesFrom (PlayerParameters p)
	{
		DiplomacyCurrent = p.DiplomacyCurrent;
		DiplomacyMax = p.DiplomacyMax;
		//p.EmptyFuelPoints = p.EmptyFuelPoints;
		ExplorationCurrent = p.ExplorationCurrent;
		ExplorationMin = p.ExplorationMin;
		ExpreiencePoints = p.ExpreiencePoints;
		FuelCurrent = p.FuelCurrent;
		FuelMax = p.FuelMax;
		ManagementCurrent = p.ManagementCurrent;
		ManagementMax = p.ManagementMax;
		StrengthCurrent = p.StrengthCurrent;
		StrengthMin = p.StrengthMin;
	}
	//А еще может быть экипаж, его численность, слаженность, члены могут сбегать
}
