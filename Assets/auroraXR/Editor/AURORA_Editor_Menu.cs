using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class AURORA_Editor_Menu : MonoBehaviour
{
	[MenuItem("auroraXR/AURORA Game Objects/Generate New UID")]
	static void GenerateNewAuroraGameObjectUid()
	{
		AURORA_GameObject ago = Selection.activeGameObject.GetComponent<AURORA_GameObject>();

		if(ago == null)
		{
			Debug.LogWarning("Selected object is not an AURORA Game Object");
			return;
		}

		Undo.RecordObject(ago, "Generated new UID");
		ago.UUID = System.Guid.NewGuid();
		
	}

    [MenuItem("auroraXR/AURORA Context/Generate New UID")]
    static void GenerateNewAuroraContextUid()
    {
        AURORA_Context ac = Selection.activeGameObject.GetComponent<AURORA_Context>();

        if (ac == null)
        {
            Debug.LogWarning("Selected object is not an AURORA Context");
            return;
        }

        Undo.RecordObject(ac, "Generated new UID");
        ac.ContextUUID = System.Guid.NewGuid().ToString();
    }

    [MenuItem("auroraXR/AURORA Applications/Application Type Registration/Generate New UID")]
    static void GenerateNewAuroraApplicationTypeUid()
    {
        AURORA_ApplicationRegistration ac = Selection.activeGameObject.GetComponent<AURORA_ApplicationRegistration>();

        if (ac == null)
        {
            Debug.LogWarning("Selected object is not an AURORA Application Type Registration");
            return;
        }

        Undo.RecordObject(ac, "Generated new UID");
        ac.ApplicationTypeUuid = System.Guid.NewGuid();
    }

    [MenuItem("auroraXR/AURORA Applications/Application Instance/Generate New UID")]
    static void GenerateNewAuroraApplicationInstanceUid()
    {
        AURORA_ApplicationInstance ai = Selection.activeGameObject.GetComponent<AURORA_ApplicationInstance>();

        if (ai == null)
        {
            Debug.LogWarning("Selected object is not an AURORA Application Instance");
            return;
        }

        Undo.RecordObject(ai, "Generated new UID");
        ai.ApplicationInstanceUuid = System.Guid.NewGuid();
    }
}