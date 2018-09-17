///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 30/04/2018 10:18
///-----------------------------------------------------------------

using UnityEngine;

namespace Fr.Sebastien.Rush.Rush.Scenes {

	public enum DifficultyLevel
	{
		EASY,
		MEDIUM,
		HARD
	}

	[CreateAssetMenu(menuName = "Levels/New Level", fileName = "New Level")]
    public class Levels : ScriptableObject {

		public string nameLevel;
		public GameObject gameObjectLevel;
		public DifficultyLevel difficultyLevel;
    }
}



