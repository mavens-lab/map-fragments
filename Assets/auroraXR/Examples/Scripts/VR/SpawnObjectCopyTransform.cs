using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnObjectCopyTransform : MonoBehaviour
{
    [SerializeField]
    AURORA_Proxy m_auroraProxy;

    [SerializeField]
    AURORA_Spawnable m_objectToSpawn;

    System.Guid m_objectToSpawnGuid;

    [SerializeField]
    Transform m_transformToCopy;

    [SerializeField]
    bool m_copyLocalPosition = true;

    [SerializeField]
    bool m_copyLocalRotation = true;

    [SerializeField]
    bool m_copyLocalScale = false;

    [SerializeField]
    bool m_deleteSpawnedObjectOnServerWhenDestroyed = true;

	[SerializeField]
	int m_spawnToLayer;

    public AURORA_Spawnable ObjectToSpawn
    {
        get
        {
            return m_objectToSpawn;
        }

        set
        {
            m_objectToSpawn = value;
        }
    }

    private void Reset()
    {
        m_transformToCopy = transform;
    }

    // Start is called before the first frame update
    void Start()
    {
		if (m_objectToSpawn != null)
		{
			if (m_objectToSpawn.gameObject.scene.name == null)
			{
				//Is a prefab... instantiate to get data.
				AURORA_Spawnable temp = Instantiate(m_objectToSpawn);
				temp.gameObject.SetActive(false);
				m_objectToSpawnGuid = temp.SpawnableTypeUuid;
				DestroyImmediate(temp);
			}
			else
			{
				m_objectToSpawnGuid = m_objectToSpawn.SpawnableTypeUuid;
			}
		}
    }

    public void SpawnInstance()
    {        
        AURORA_GameObject spawnedObject = m_auroraProxy.PrimaryAuroraContext.Spawn(
            //m_objectToSpawnGuid,
            m_objectToSpawn.SpawnableTypeUuid,
            m_transformToCopy.position,
            m_transformToCopy.rotation,
            m_transformToCopy.localScale,
            m_copyLocalPosition,
            m_copyLocalRotation,
            m_copyLocalScale);

        // Will be null if in offline mode.
        if (spawnedObject != null)
        {
            spawnedObject.gameObject.tag = "Spawned";

			spawnedObject.gameObject.layer = m_spawnToLayer;

            spawnedObject.DeleteOnServerWhenDestroyed = m_deleteSpawnedObjectOnServerWhenDestroyed;

            spawnedObject.RequestObjectOwnership();
        }
    }
}
