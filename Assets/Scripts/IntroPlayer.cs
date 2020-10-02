using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroPlayer : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public Image Fade;
    public Image BeforeImageReadyBlack;
    public Button OsuLogo;
    // Start is called before the first frame update
    void Start()
    {
        VideoPlayer.Prepare();
        VideoPlayer.prepareCompleted += (VideoPlayer source) =>
        {
            AudioSource.PlayClipAtPoint(Audios.Instance.Triangles, Camera.main.transform.position, 0.2f);
            VideoPlayer.Play();
        };
        VideoPlayer.loopPointReached += (VideoPlayer source) =>
        {
            VideoPlayer.targetCameraAlpha = 0;
            Fade.GetComponent<Animator>().SetTrigger("Fading");
            BeforeImageReadyBlack.color = new Color(0, 0, 0, 0);
            OsuLogo.interactable = true;
        };
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}