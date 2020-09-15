using UnityEngine;

public class ClickingDetector : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hitInfo = Physics2D.GetRayIntersection(ray);
            if (hitInfo.collider != null)
            {
                ILeftClickable clicked = InterfaceUtilities.GetInterface<ILeftClickable>(hitInfo.collider.gameObject);
                if (clicked != null)
                {
                    clicked.OnLeftClick();
                }
            }
        }
    }
}