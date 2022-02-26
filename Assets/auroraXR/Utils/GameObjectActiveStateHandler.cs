using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AURORA.Utils
{
	[System.Serializable]
	public class GameObjectActivationEvent : UnityEngine.Events.UnityEvent<bool>
	{ }

	public class GameObjectActiveStateHandler : MonoBehaviour
	{
		[SerializeField] GameObjectActivationEvent m_gameObjectActivatedEvent;
		[SerializeField] GameObjectActivationEvent m_gameObjectDectivationEvent;

		public void SetGameObjectActiveState(bool isActive)
		{
			//if(isActive == gameObject.activeSelf)
			//{
			//    return;
			//}

			if(isActive)
			{
				m_gameObjectActivatedEvent.Invoke(isActive);
			}
			else
			{
				m_gameObjectDectivationEvent.Invoke(isActive);
			}
		}
	}
}