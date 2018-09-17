///-----------------------------------------------------------------
///   Author : Sebastien Raymondaud                    
///   Date   : 30/04/2018 15:07
///-----------------------------------------------------------------

using UnityEngine;
using Fr.Sebastien.Rush.Rush.Scripts.Manager;
using UnityEngine.Events;

namespace Fr.Sebastien.Rush.Rush.Scripts.Utils {
	[CreateAssetMenu(menuName = "ActionBlock/New ActionBlock", fileName ="New ActionBlock")]
    public class ActionBlock : ScriptableObject {

		public Sprite icon;
		public GameObject prefabSelector;
		public GameObject prefabGame;
		public int nbMax;
		public int nbLeft;
		public BlockList type;
	}
}



