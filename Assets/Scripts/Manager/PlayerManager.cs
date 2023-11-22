using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Windows;

public class PlayerManager : MonoBehaviour
{
    public void Init(VoidIntDelegate _spUpdateCallback, VoidFloatDelegate _hpUpdateCallback, PlayerController.PlayAudioDelegate _playAudioCallback,VoidVoidDelegate _gameoverCallback)
    {
        input = GetComponent<PlayerInputHandler>();
        playerCtrl = GetComponentInChildren<PlayerController>();
        playerData.input = input;

        playerCtrl.Init(playerData, _spUpdateCallback, _hpUpdateCallback, _gameoverCallback, _playAudioCallback, volumeProfile, EnableDummy);
        pmc.Init(playerData);
        dummyGo.SetActive(false);
    }

    private void FixedUpdate()
    {
        pmc.PlayerModelRotate();
    }

    private void EnableDummy()
    {
        dummyGo.SetActive(true);
    }

    public PlayerController PC => playerCtrl;

    [SerializeField]
    private PlayerData playerData = null;
    [SerializeField]
    private PlayerModelRotateController pmc = null;
    [SerializeField]
    private VolumeProfile volumeProfile = null;
    [SerializeField]
    private GameObject dummyGo = null;

    private PlayerController playerCtrl = null;
    private PlayerInputHandler input = null;


    public PlayerData PData => playerData;

}
