using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class IntroPlayer : MonoBehaviour
{
    public VideoPlayer VideoPlayer;
    public Image Fade;
    public Image BeforeImageReadyBlack;
    public Button MouseBlocker;

    // Start is called before the first frame update
    private void Start()
    {
        VideoPlayer.Prepare();
        VideoPlayer.prepareCompleted += (VideoPlayer source) =>
        {
            MusicPlayer.PlaySelected(Audio.Instance.Triangles, PlayerData.Instance.MusicVolume);
            VideoPlayer.Play();
        };
        VideoPlayer.loopPointReached += (VideoPlayer source) =>
        {
            VideoPlayer.targetCameraAlpha = 0;
            Fade.GetComponent<Animator>().SetTrigger("Fading");
            BeforeImageReadyBlack.color = new Color(0, 0, 0, 0);
            Destroy(MouseBlocker.gameObject);
            PlayerData.Instance.HasIntroFinished = true;
        };
    }

    // Update is called once per frame
    private void Update()
    {
    }
}