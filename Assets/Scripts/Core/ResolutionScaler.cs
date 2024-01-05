using UnityEngine;
using UnityEngine.EventSystems;

public class ResolutionScaler : UIBehaviour
{
    [SerializeField] private Vector2 _preserveAspect = new Vector2(16, 9);

    private Vector2 _prevScreenSize = Vector2.zero;
    private Camera _camera;

    protected override void Awake()
    {
        base.Awake();
        _camera = GetComponent<Camera>();
        _prevScreenSize = new Vector2(Screen.width, Screen.height);
        RecalculateResolution();
    }

    private void Update()
    {
        if (_prevScreenSize != new Vector2(Screen.width, Screen.height))
        {
            _prevScreenSize = new Vector2(Screen.width, Screen.height);
            RecalculateResolution();
        }
    }

    private void RecalculateResolution()
    {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = _preserveAspect.x / _preserveAspect.y;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = _camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            _camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = _camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            _camera.rect = rect;
        }
    }
}
