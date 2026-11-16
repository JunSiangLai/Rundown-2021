using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

/// <summary>
/// Manager for audio
/// <para>Singleton</para>
/// </summary>
public class AudioManager : NetworkBehaviour
{
    #region instance
    /// <summary>
    /// Instance to class
    /// </summary>
    private static AudioManager _instance;
    /// <summary>
    /// Public handler to instance
    /// </summary>
    public static AudioManager instance { get { return _instance; } }
    #endregion

    /// <summary>
    /// enum for different types of Audio
    /// </summary>
    public enum AudioType
    {
        NULL = 0,
        PlayerFootsteps,
    }

    #region Variables
    [ReadOnlyWhenPlaying] List<AudioSourceStruct> localaudioSourceList;

    [ReadOnlyWhenPlaying] Dictionary<AudioType, AudioClip> clipTypeDictionary;
    [EditableOnlyWhenNotPlayTime] public List<AudioTypeStruct> audioList;
    #endregion

    #region Events
    public delegate void ReceiveAudio(Vector3 position, AudioType type, bool isPlaying);
    /// <summary>
    /// ReceiveAudio(Vector3 position, AudioType type)
    /// </summary>
    public static event ReceiveAudio receiveAudio;

    public delegate void EditAudio(AudioSource source, bool entityListen, params bool[] others);
    /// <summary>
    /// EditAudio(AudioSource source, bool entityListen, params bool[] others)
    /// </summary>
    public static event EditAudio editAudio;
    #endregion

    #region Functions
    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        ConvertListToDictionary();
    }

    // Start is called before the first frame update
    void Start()
    {
        if (isServer)
            SceneInit();
    }

    void ConvertListToDictionary()
    {
        clipTypeDictionary = new Dictionary<AudioType, AudioClip>();
        foreach (var audioStruct in audioList)
        {
            clipTypeDictionary.Add(audioStruct.type, audioStruct.clip);
        }
        audioList.Clear();
    }
    void SceneInit()
    {
        var audioSources = FindObjectsOfType(typeof(AudioSource), true);

        foreach (AudioSource source in audioSources)
        {
            if (!source.gameObject.TryGetComponent<AudioElement>(out AudioElement element))
            {
                throw new System.Exception("Audio source missing AudioElement component at" + source.gameObject);
            }
            //else
            //{
            //    clipTypeDictionary.TryGetValue(element.type, out AudioClip clip);
            //    source.clip = clip;
            //    if (element.EntityListen)
            //        entityaudioSourceList.Add(new AudioSourceStruct(element.type, source.gameObject.name, element.networkIdentity.netId));
            //    else
            //        audioSourceList.Add(new AudioSourceStruct(element.type, source.gameObject.name, element.networkIdentity.netId));
            //}
        }
    }

    [Command(requiresAuthority = false)]
    public void UpdateListsCommand(GameObject sourceParent)
    {
        AudioManager.instance.UpdateLists(sourceParent);
    }

    [ClientRpc]
    public void UpdateLists(GameObject sourceParent)
    {
        foreach (AudioSource source in sourceParent.GetComponentsInChildren<AudioSource>())
        {
            GameObject audioGO = source.gameObject;
            if (!audioGO.TryGetComponent<AudioElement>(out AudioElement element))
            {
                throw new System.Exception("Audio source missing AudioElement component at" + audioGO);
            }
            else
            {
                clipTypeDictionary.TryGetValue(element.type, out AudioClip clip);
                source.clip = clip;
                //if (element.EntityListen)
                //    entityaudioSourceList.Add(new AudioSourceStruct(element.type, source.gameObject.name, element.networkIdentity.netId));
                //else
                //    audioSourceList.Add(new AudioSourceStruct(element.type, source.gameObject.name, element.networkIdentity.netId));
            }
        }
    }

    /// <summary>
    /// AudioChange(AudioSource source, bool entityListen, bool Play, bool Stop, bool Pause, bool Unpause);
    /// <para>Used to Control Audio Sources</para>
    /// </summary>
    [Command(requiresAuthority = false)]
    public void AudioControlCommand(uint sourceID, string name, bool entityListen, params bool[] others)
    {
        AudioManager.instance.AudioControl(sourceID, name, entityListen, others);
    }

    /// <summary>
    /// AudioChange(AudioSource source, bool entityListen, bool Play, bool Stop, bool Pause, bool Unpause);
    /// <para>Used to Control Audio Sources</para>
    /// </summary>
    [ClientRpc]
    public void AudioControl(uint sourceID, string name, bool entityListen, params bool[] others)
    {
        GameObject idObject;
        if (isClient)
        {
            if (!NetworkClient.spawned.TryGetValue(sourceID, out NetworkIdentity identity))
            {
                throw new System.Exception("Missing GO at AudioControl() | netid = " + sourceID);
            }
            else
                idObject = identity.gameObject;
        }
        else
        {
            if (!NetworkServer.spawned.TryGetValue(sourceID, out NetworkIdentity identity))
            {
                throw new System.Exception("Missing GO at AudioControl() | netid = " + sourceID);
            }
            else
                idObject = identity.gameObject;
        }
        AudioSource audioSource = null;
        // If AudioSource object is NetworkIdentity object
        if (idObject.TryGetComponent(out AudioSource Asource) && idObject.name == name)
        {
            audioSource = Asource;
        }
        else
        {
            foreach (AudioSource source in idObject.GetComponentsInChildren<AudioSource>())
            {
                if (source.gameObject.name == name)
                {
                    audioSource = source;
                    break;
                }
            }
            if (!audioSource)
            {
                Debug.Log($"AudioSource '{name}' not found in {idObject.name}");
                return;
            }
        }

        switch (others.Length)
        {
            case 4:
                if (others[3])
                    audioSource.UnPause();
                goto case 3;
            case 3:
                if (others[2])
                    audioSource.Pause();
                goto case 2;
            case 2:
                if (others[1])
                    audioSource.Stop();
                goto default;
            default:
                if (others[0])
                    audioSource.Play();
                break;
        }
    }

    public void AudioUpdate(GameObject sourceParent, AudioType type, bool isPlaying)
    {
        receiveAudio.Invoke(sourceParent.transform.position, type, isPlaying);
    }
    #endregion
}

#region Structs
[System.Serializable]
public struct AudioTypeStruct
{
    [SerializeField] public AudioManager.AudioType type;
    [SerializeField] public AudioClip clip;
}

[System.Serializable]
struct AudioSourceStruct
{
    [SerializeReference] public AudioManager.AudioType type;
    [SerializeReference] public string sourceName;
    [SerializeReference] public uint sourceID;

    public AudioSourceStruct(AudioManager.AudioType Type, string name, uint Source)
    {
        type = Type;
        sourceName = name;
        sourceID = Source;
    }

}
#endregion