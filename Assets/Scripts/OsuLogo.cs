using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OsuLogo : MonoBehaviour
{
    public GameObject PopUpButtonsLogo;
    public Button Play;
    public Button Options;
    public Button Exit;
    private Animator animator;
    private Animator popUpButtonsAnimator;
    private Animator[] popUpButtonsAnimators;

    private void Start()
    {
        animator = GetComponent<Animator>();
        popUpButtonsAnimator = PopUpButtonsLogo.GetComponent<Animator>();
        popUpButtonsAnimators = PopUpButtonsLogo.GetComponentsInChildren<Animator>();
    }
    public void ClickOsuLogo()
    {
        PopUpButtonsLogo.SetActive(true);
        popUpButtonsAnimator.SetTrigger("LogoPressed");
    }
}