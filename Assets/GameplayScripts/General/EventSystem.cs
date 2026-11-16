using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles events in the game
/// <para>Singleton</para>
/// </summary>
public class EventSystem : MonoBehaviour
{
    #region instance
    /// <summary>
    /// Instance to class
    /// </summary>
    private static EventSystem _instance;
    /// <summary>
    /// Public handler to instance
    /// </summary>
    public static EventSystem instance { get { return _instance; } }
    #endregion

    /// <summary>
    /// Enum for different Event Types
    /// </summary>
    public enum EventsTYPE
    {
        characterUpdate,
        soundMade,
        Total,
    }

    #region Events
    public delegate void CharacterUpdate();
    /// <summary>
    /// CharacterUpdate()
    /// </summary>
    public static event CharacterUpdate characterUpdate;

    public delegate void SoundMade(int increase);
    /// <summary>
    /// SoundMade(int)
    /// </summary>
    public static event SoundMade soundMade;

    #endregion

    #region Functions
    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public static void RaiseEvent(EventsTYPE eventRaised, params object[] paraobj)
    {
        switch (eventRaised)
        {
            case EventsTYPE.characterUpdate:
                characterUpdate.Invoke();
                break;
            case EventsTYPE.soundMade:
                soundMade.Invoke((int)paraobj[0]);
                break;
        }
    }

    #endregion
}
