using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StimuliStarter : MonoBehaviour
{
    private int[] lookat = { 1, 1, 1, 1, 1, 2, 2, 2, 2, 2, 3, 3, 3, 3, 3, 4, 4, 4, 4, 4 };
    private SerialComm serialComm;
    private TextMesh textLook;
    public string txtBlink = "";
    private GameObject Canvas;
    private GameObject[] Icons;

    void Awake()
    {
        Application.targetFrameRate = 60;
        Canvas = GameObject.Find("Canvas");
        serialComm = GameObject.Find("SerialComm").GetComponent<SerialComm>();
        textLook = transform.GetChild(0).GetComponent<TextMesh>();
        Icons = GameObject.FindGameObjectsWithTag("Icon");
    }
    
    public void StartSession()
    {
        Canvas.SetActive(false);
        if (StimuliController.colorMode != 0)
        {
            foreach (GameObject g in Icons)
            {
                g.SetActive(true);
            }
        }
        StartCoroutine("StartTrial");
    }
    public void StopSession()
    {
        textLook.text = "";
        StimuliController.isStart = false;
        textLook.gameObject.SetActive(false);
        StopAllCoroutines();
        foreach (GameObject g in Icons)
        {
            g.SetActive(false);
        }
        Canvas.SetActive(true);

    }

    IEnumerator StartTrial()
    {
        int temp1, temp2;
        for (int m = 0; m < 20; m++)
        {
            temp1 = lookat[m];
            temp2 = Random.Range(0, 19);
            lookat[m] = lookat[temp2];
            lookat[temp2] = temp1;
        }
        for (int i = 0; i < 20; i++)
        {
            textLook.gameObject.SetActive(true);
            textLook.text = lookat[i].ToString() + "번을 보세요\n" + txtBlink;
            yield return new WaitForSeconds(3f);
            textLook.gameObject.SetActive(false);
            StimuliController.isStart = true;
            serialComm.DataSend(lookat[i]); //시리얼통신으로 인덱스 전송
            yield return new WaitForSeconds(4f);
            StimuliController.isStart = false;
            serialComm.DataSend(lookat[i]+10); //시리얼통신으로 인덱스 전송
        }
        textLook.gameObject.SetActive(true);
        textLook.text = "대기하세요";
        serialComm.DataSend(5); //시리얼통신으로 인덱스+시간 전송
        yield return new WaitForSeconds(5.0f);
        StopSession();
    }
}
