using System;
using System.Collections;
using System.Collections.Generic;
using AURORA.ClientAPI;
using AURORA.Protobuf;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class StringMetadataValueChanged : UnityEngine.Events.UnityEvent<string>
{ }

public class MetadataFieldStringInterface : MetadataInterface
{
    [SerializeField]
    private StringMetadataValueChanged m_textChangedHandlers;

    [Tooltip("Default data value to use if metadata field is not set. UTF-8")]
    [SerializeField]
    [TextArea(3, 30)]
    string m_defaultText;

    public UnityEvent<string> TextChangedHandlers { get => m_textChangedHandlers; }

    public string Text
    {
        get
        {
            return Data.ToStringUtf8();
        }
        set
        {
            Data = Google.Protobuf.ByteString.CopyFromUtf8(value);
        }
    }

    public string DefaultText { get => m_defaultText; set => m_defaultText = value; }

    protected override void Start()
    {
        base.Start();

        if (!m_metadataCollection.HasKey(MetadataKey) && AuroraGameObject.IsLocallyOwned)
        {
            SetText(m_defaultText, ContentType);
        }
    }

    public void SetText(string text, string contentType = "text/plain")
    {
        SetData(Google.Protobuf.ByteString.CopyFromUtf8(text), contentType);
    }

    protected override void SetupEventHandlers()
    {
        // Call the base method to setup core handlers
        base.SetupEventHandlers();

        // We need to avoid creating multiple entries for the handler, so if it is not null (empty)
        // remove it from the list of handlers (-=)
        if (m_metadataCollection.MetadataKeyModifiedEventHandler != null)
        {
            MetadataChanged.RemoveListener(OnValueChanged);
        }
        MetadataChanged.AddListener(OnValueChanged);
        
        // We need to avoid creating multiple entries for the handler, so if it is not null (empty)
        // remove it from the list of handlers (-=)
        if (m_metadataCollection.MetadataKeyDeletedEventHandler != null)
        {
            MetadataDeleted.RemoveListener(OnValueDeleted);
        }
        MetadataDeleted.AddListener(OnValueDeleted);
    }

    private void OnValueChanged(string key, Metadata metadata)
    {
        // If the value is null or empty, update with an empty string
        // One might consider sending the default value in this case, but that may not be
        // appropriate if the value were intentionally set to empty string or null.
        if (metadata == null || metadata.Data == null || metadata.Data.Length == 0)
        {
            m_textChangedHandlers.Invoke("");
        }
        else
        {
            // Convert the data to a uft-8 string and invoke handlers.
            m_textChangedHandlers.Invoke(metadata.Data.ToStringUtf8());
        }
    }

    private void OnValueDeleted(string key)
    {
        m_textChangedHandlers.Invoke("");
    }

    protected override void Reset()
    {
        base.Reset();

        DefaultContentType = "text/plain";
    }
}
