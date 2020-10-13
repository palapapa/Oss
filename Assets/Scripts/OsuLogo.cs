using UnityEngine;
using UnityEngine.UI;

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

    public void OnLeftClick()
    {
        PopUpButtonsLogo.SetActive(true);
        popUpButtonsAnimator.SetTrigger("LogoPressed");
    }

    private void Awake()
    {
        SetLogoBpm(160);//placeholder
    }

    public static void SetLogoBpm(int bpm)
    {
        Instance.Animator.SetFloat("PulsatingSpeed", bpm / 180.0f);//dependent on animation clip OsuLogoPulsating
    }
}