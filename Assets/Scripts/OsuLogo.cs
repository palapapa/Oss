using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OsuLogo : MonoBehaviour, ILeftClickable
{
    public GameObject PopUpButtonsLogo;
    public Button Play;
    public Button Options;
    public Button Exit;
    public Animator Animator;
    [HideInInspector]
    public OsuLogo Instance;
    private Animator popUpButtonsAnimator;

    private OsuLogo()
    {
        Instance = this;
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
        SetLogoBpm(this, 160);//placeholder
    }
    public static void SetLogoBpm(OsuLogo ol, int bpm)
    {
        ol.Instance.Animator.SetFloat("PulsatingSpeed", bpm / 180.0f);//dependent on animation clip OsuLogoPulsating
    }
}