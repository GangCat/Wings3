using UnityEngine;

public delegate void VoidVoidDelegate();
public delegate void VoidFloatDelegate(float _value);
public delegate void VoidIntDelegate(int _value);
public delegate void VoidGameObjectDelegate(GameObject _go);
public delegate void VoidBoolDelegate(bool _value);
public delegate void PlayEffectAudioDelegate(EEffectAudio _effectAudio);

public enum EPublisherType 
{ 
    NONE = -1, 
    GAME_MANAGER,
    BOSS_CONTROLLER,
    LENGTH 
}

public enum EMessageType
{
    NONE = -1,
    PAUSE,
    PHASE_CHANGE,
    SHIELD_BROKEN,
    GAME_START_ALERT,
    GIANT_MISSILE_ALERT,
    FIRST_PATTERN_1_ALERT,
    FIRST_PATTERN_2_ALERT,
    LAST_PATTERN_ALERT,
    GAME_CLEAR_ALERT,
    LENGTH
}
