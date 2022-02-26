using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AURORA.Utils
{
    public class EnableOneGameObjectInSet : MonoBehaviour
    {
        [SerializeField] List<GameObject> m_gameObjectList;
        [SerializeField] int m_activeIndex;

        public int ActiveIndex
        {
            get => m_activeIndex;
            set => ActivateIndex(value);
        }

        public GameObject ActiveGameObject
        {
            get
            {
                if (m_activeIndex < 0 || m_activeIndex >= m_gameObjectList.Count)
                {
                    return null;
                }
                return m_gameObjectList[m_activeIndex];
            }
        }

        private void Start()
        {
            if (m_activeIndex <0 || m_activeIndex > m_gameObjectList.Count)
            {
                Debug.LogError("Active Index not set to a valid terrain number!");
            }
            else
            {
                ActiveIndex = m_activeIndex;
            }  
        }

        public void ActivateIndex(int index)
        {
            // Sets the gameobject in the list equal to index as active, and all others to inactive

            m_activeIndex = index;

            int i = 0;
            foreach (GameObject go in m_gameObjectList)
            {
                go.SetActive(i == m_activeIndex);

                GameObjectActiveStateHandler goash = go.GetComponent<GameObjectActiveStateHandler>();

                if (goash != null)
                {
                    goash.SetGameObjectActiveState(go.activeSelf);
                }

                i++;
            }
        }

        private void Reset()
        {
            m_gameObjectList.Clear();

            foreach (Transform child in transform)
            {
                m_gameObjectList.Add(child.gameObject);
            }
        }
    }
}