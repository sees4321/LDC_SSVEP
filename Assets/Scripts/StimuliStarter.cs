using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StimuliStarter : MonoBehaviour
{
    public int classes = 0;
    public string txtBlink = "";

    public int[] lookat;

    private SerialComm serialComm;
    private TextMesh textLook;
    private GameObject Canvas;
    private GameObject[] Icons;
    private GameObject[] Stims;
    private AudioSource audioSc;

    void Awake()
    {
        Application.targetFrameRate = 60;
        Canvas = GameObject.Find("Canvas");
        serialComm = GameObject.Find("SerialComm").GetComponent<SerialComm>();
        textLook = transform.GetChild(0).GetComponent<TextMesh>();
        Icons = GameObject.FindGameObjectsWithTag("Icon");
        Stims = GameObject.FindGameObjectsWithTag("Stimulus");
        audioSc = GameObject.Find("AudioSource").GetComponent<AudioSource>();
        audioSc.loop = false;
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
        StartCoroutine(StartTrial());
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
        Shuffler();
        for (int i = 0; i < lookat.Length; i++)
        {
            if (classes != 0)
            {
                StimMover(false);//자극 이동
                int a = lookat[i] < 4 ? 1 : 2;
                textLook.gameObject.SetActive(true);
                textLook.text = a + "번을 보세요\n" + txtBlink;
                yield return new WaitForSeconds(3f);
                textLook.gameObject.SetActive(false);
                audioSc.Play();
                StimuliController.isStart = true;
                serialComm.DataSend(a + 4); //시리얼통신으로 인덱스 전송
                yield return new WaitForSeconds(4f);
                StimuliController.isStart = false;
                serialComm.DataSend(a + 14); //시리얼통신으로 인덱스 전송
                StimMover(true);//자극 이동
            }

            textLook.gameObject.SetActive(true);
            textLook.text = ((lookat[i] % 4) + 1).ToString() + "번을 보세요\n" + txtBlink;
            yield return new WaitForSeconds(3f);
            textLook.gameObject.SetActive(false);
            audioSc.Play();
            StimuliController.isStart = true;
            serialComm.DataSend(lookat[i]); //시리얼통신으로 인덱스 전송
            yield return new WaitForSeconds(4f);
            StimuliController.isStart = false;
            serialComm.DataSend(lookat[i] + 10); //시리얼통신으로 인덱스 전송
        }
        textLook.gameObject.SetActive(true);
        textLook.text = "대기하세요";
        serialComm.DataSend(5); //시리얼통신으로 인덱스+시간 전송
        yield return new WaitForSeconds(5.0f);
        StopSession();
    }

    private void Shuffler()
    {
        lookat = new int[20 + (20 * classes)];
        for (int i  = 0; i < lookat.Length; i++)
        {
            lookat[i] = i % (4 + 4 * classes);
        }
        int temp1, temp2;
        for (int m = 0; m < lookat.Length; m++)
        {
            temp1 = lookat[m];
            temp2 = Random.Range(0, lookat.Length);
            lookat[m] = lookat[temp2];
            lookat[temp2] = temp1;
        }
    }

    private void StimMover(bool flag)
    {
        Stims[0].SetActive(flag);
        Stims[1].SetActive(flag);
        float tmp = flag ? -0.11f : 0;
        Stims[2].transform.localPosition = new Vector3(-0.2f, tmp, -1);
        Stims[3].transform.localPosition = new Vector3(0.2f, tmp, -1);

    }
}
