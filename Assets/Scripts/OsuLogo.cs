using System;
using UnityEngine;

public class OsuLogo : MonoBehaviour, ILeftClickable
{
    public static OsuLogo Instance;
    public GameObject PopUpButtonsLogo;
    public Animator Animator;
    private Animator popUpButtonsAnimator;

    private OsuLogo()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    private void Start()
    {
        popUpButtonsAnimator = PopUpButtonsLogo.GetComponent<Animator>();
    }

    private void Update()
    {
        try
        {
            SetLogoBpm((float)(1 / MusicPlayer.CurrentPlaying.TimingPoints[0].BeatLength * 60000));
        }
        catch (ArgumentOutOfRangeException)
        {
            SetLogoBpm(160);
        }
    }

    public void OnLeftClick()
    {
        PopUpButtonsLogo.SetActive(true);
        popUpButtonsAnimator.SetTrigger("LogoPressed");
    }

    private void Awake()
    {
        SetLogoBpm(160);//placeholder
    }

    public static void SetLogoBpm(float bpm)
    {
        Instance.Animator.SetFloat("PulsatingSpeed", bpm / 180.0f);//dependent on animation clip OsuLogoPulsating
    }
}