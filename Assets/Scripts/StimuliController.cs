using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StimuliController : MonoBehaviour
{
    public static bool isStart = false; //시작트리거
    public static int scaleMode = 0; // MODE - 0: binary, 1: linear, 2: sine
    public static int colorMode = 0;

    public int setFrame = 10; //10 frame 마다
    public float scale = 0.0135f;
    public float scaleChangeRatio = 0.2f; //크기 변경 비율
    public Color[] color1 = new Color[3];
    public Color[] color2 = new Color[3];

    private float defaultScale; //기본 scale
    private int frameCount = 0; //현재 frame
    private int BWcount = 6;
    private bool upward = true;
    private float inverse = 0;

    private SpriteRenderer sprite;
    private SpriteRenderer spriteIcon;

    int k = 0;
        //W 0 0.7
        //R 0     
        //G 0
        //B 0

    private void Awake()
    {
        sprite = GetComponent<SpriteRenderer>();
        spriteIcon = transform.GetChild(0).GetComponent<SpriteRenderer>();
    }
    // Start is called before the first frame update
    void Start()
    {
        defaultScale = scale;
        transform.localScale = new Vector3(defaultScale, defaultScale, 1);
        transform.GetChild(0).gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        FrameUpdate();
    }

    private void FrameUpdate()
    {
        frameCount++;
        if (!isStart)
        {
            transform.localScale = new Vector3(defaultScale, defaultScale, 1);
            sprite.color = new Color(1f, 1f, 1f);
            spriteIcon.color = new Color(0, 0, 0);
            frameCount = 0;
            upward = true;
            //k = 0;
            return;
        }

        if (frameCount <= setFrame)
        {
            /*
            switch (scaleMode) //scale 변경
            {
                case 0:
                    if (!upward) scale = defaultScale * (1 - scaleChangeRatio);
                    else scale = defaultScale * (1 + scaleChangeRatio);
                    break;
                case 1:
                    if (upward) scale = defaultScale * (1 - scaleChangeRatio) - frameCount * defaultScale * scaleChangeRatio * 2 / setFrame;
                    else scale = defaultScale * (1 + scaleChangeRatio) + frameCount * defaultScale * scaleChangeRatio * 2 / setFrame;
                    break;
                case 2:
                    if (upward) scale = defaultScale + Mathf.Cos((float)frameCount / setFrame * Mathf.PI) * defaultScale * scaleChangeRatio;
                    else scale = defaultScale - Mathf.Cos((float)frameCount / setFrame * Mathf.PI) * defaultScale * scaleChangeRatio;
                    break;
                default:
                    break;
            }
            transform.localScale = new Vector3(scale, scale, 1);*/

            switch (scaleMode) 
            {
                case 0:
                    if (!upward) inverse = 1f;
                    else inverse = 0f;
                    break;
                case 1:
                    if (!upward) inverse = 1f - (float)frameCount / setFrame;
                    else inverse = (float)frameCount / setFrame;
                    break;
                case 2:
                    if (!upward) inverse = 0.5f + Mathf.Cos((float)frameCount / setFrame * Mathf.PI) / 2;
                    else inverse = 0.5f - Mathf.Cos((float)frameCount / setFrame * Mathf.PI) / 2;
                    break;
                default:
                    break;
            }
            scale = Mathf.Lerp(defaultScale * (1 - scaleChangeRatio), defaultScale * (1 + scaleChangeRatio), inverse);
            transform.localScale = new Vector3(scale, scale, 1);

            if (colorMode < BWcount)
            {
                //sprite.color = new Color(1f - 0.7f * inverse, 1f - 0.7f * inverse, 1f - 0.7f * inverse);
                //spriteIcon.color = new Color(inverse, inverse, inverse);
                sprite.color = Color.Lerp(new Color(0.3f, 0.3f, 0.3f), Color.white, inverse);
                spriteIcon.color = Color.Lerp(Color.white, new Color(0.3f, 0.3f, 0.3f), inverse);
            }
            else
            {
                sprite.color = Color.Lerp(color1[colorMode - BWcount], color2[colorMode - BWcount], inverse);
                spriteIcon.color = Color.Lerp(color2[colorMode - BWcount], color1[colorMode - BWcount], inverse);
            }

            if (frameCount == setFrame)
            {
                k++;
                frameCount = 0;
                upward = !upward;
            }
        }
    }
}
