using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO.Ports;

public class SettingUI : MonoBehaviour
{
    private SerialComm serialComm;
    private StimuliStarter stimStarter;
    private Dropdown drpPortNum, drpBlink, drpBaud, drpColor, drpGraph;
    private Button btnStart;
    private Text txtSerialOn;
    private bool isSerialOn = false;
    private string[] txtBlink = { "´«À» ±ôºýÀÌÁö ¸¶¼¼¿ä", "´«À» 2¹ø ±ôºýÀÌ¼¼¿ä", "´«À» 3¹ø ±ôºýÀÌ¼¼¿ä", "" };
    private List<string> drpOption_port, drpOption_baud;
    // Start is called before the first frame update
    void Start()
    {
        serialComm = GameObject.Find("SerialComm").GetComponent<SerialComm>();
        drpPortNum = GameObject.Find("drpPortNum").GetComponent<Dropdown>();
        drpOption_port = new List<string>();
        string[] ports = SerialPort.GetPortNames();
        for(int i = 0; i < ports.Length; i++)
            drpOption_port.Add(ports[i]);
        drpPortNum.AddOptions(drpOption_port);

        drpBaud = GameObject.Find("drpBaud").GetComponent<Dropdown>();
        drpOption_baud = new List<string>() { "9600", "14400", "19200", "38400", "56000", "57600", "115200" };
        drpBaud.AddOptions(drpOption_baud);

        drpBlink = GameObject.Find("drpBlink").GetComponent<Dropdown>();
        drpGraph = GameObject.Find("drpGraph").GetComponent<Dropdown>();
        drpColor = GameObject.Find("drpColor").GetComponent<Dropdown>();
        btnStart = GameObject.Find("btnStart").GetComponent<Button>();
        txtSerialOn = GameObject.Find("txtSerialOn").GetComponent<Text>();
        stimStarter = GameObject.Find("Stimulus").GetComponent<StimuliStarter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            stimStarter.StopSession();
        }
    }

    public void BtnStartCommunication()
    {
        //.Log(drpOption_port[drpPortNum.value] + int.Parse(drpOption_baud[drpBaud.value]));
        try
        {
            if (!isSerialOn)
            {
                serialComm.onStartBtnClick(drpOption_port[drpPortNum.value], int.Parse(drpOption_baud[drpBaud.value]), true);
                btnStart.gameObject.transform.GetChild(0).transform.GetComponent<Text>().text = "Stop Communication";
                txtSerialOn.text = "Serial ON";
            }
            else
            {
                serialComm.onStartBtnClick(drpOption_port[drpPortNum.value], int.Parse(drpOption_baud[drpBaud.value]), false);
                btnStart.gameObject.transform.GetChild(0).transform.GetComponent<Text>().text = "Start Communication";
                txtSerialOn.text = "Serial OFF";
            }
            isSerialOn = !isSerialOn;
        }
        catch { Debug.LogError("Serial Communication Initializing error"); }
    }

    public void BtnExit()
    {
        Application.Quit();
    }

    public void BtnStartSession()
    {
        stimStarter.txtBlink = txtBlink[drpBlink.value];
        StimuliController.colorMode = drpColor.value;
        StimuliController.scaleMode = drpGraph.value;
        GameObject tempBG = GameObject.Find("BackGround");
        for (int i = 0; i < tempBG.transform.childCount; i++)
            tempBG.transform.GetChild(i).gameObject.SetActive(false);
        tempBG.transform.GetChild(drpColor.value).gameObject.SetActive(true);

        stimStarter.StartSession();
    }
}
