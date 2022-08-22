using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using System.Text;

public class SerialComm : MonoBehaviour
{
    private static SerialComm instance = null;
    SerialPort sPort = new SerialPort();

    public static SerialComm Instance
    {
        get
        {
            if (null == instance) return null;
            return instance;
        }
    }

    private void Awake()
    {
        if (null == instance)
        {
            instance = this; //이 클래스 인스턴스가 탄생했을 때 전역변수 instance에 게임매니저 인스턴스가 담겨있지 않다면, 자신을 넣어준다.
            DontDestroyOnLoad(this.gameObject); //씬 전환이 되더라도 파괴되지 않게 한다.
        }
        else
            Destroy(this.gameObject); //이미 전역변수인 instance에 인스턴스가 존재한다면 자신(새로운 씬의 GameMgr)을 삭제해준다.
        Screen.SetResolution(1920, 1280, true);
    }
    
    void Update()
    {
        //if (sPort.IsOpen) DataReceived();
    }

    public void onStartBtnClick(string portNum, int baudrate, bool counter)
    {
        if (counter)
        {
            sPort = new SerialPort();
            sPort.PortName = portNum;
            sPort.BaudRate = baudrate;
            sPort.DataBits = (int)8;
            sPort.Parity = Parity.None;
            sPort.StopBits = StopBits.One;
            sPort.Handshake = Handshake.None;
            sPort.Encoding = Encoding.UTF8;
            sPort.Open();
            Debug.Log("포트연결");
        }
        else if (sPort != null && !counter)
        {
            sPort.Close();
            Debug.Log("포트닫음");
        }
    }

    public void DataSend(int freqIndex)
    {
        //포트 데이터 전송
        if (sPort.IsOpen)
        {
            byte[] buffer = { (byte)freqIndex, 0x00 };
            sPort.Write(buffer, 0, 1);
        }
    }

    private void OnApplicationQuit()
    {
        if (sPort.IsOpen)
        {
            sPort.Close();
        }
    }

    void DataReceived()
    {
        while (sPort.BytesToRead > 0)
        {
            //Debug.Log("Read Bytes");
            if (sPort.IsOpen)
            {
                //data 받기
                int buffer = sPort.ReadByte();
                string msg = sPort.ReadExisting();
                Debug.Log("Recieved: " + msg);
                switch (/*int.Parse(msg)*/buffer)
                {
                    case 1:
                        //CheckHzController.startEX();
                        break;
                    case 2:
                        //SceneManager.LoadScene("MainScene"); //main으로 점프
                        break;
                    case 3:
                        //CheckHzController.startHUD();
                        break;
                    default:
                        Debug.Log("Recieved trigger is not predefined");
                        break;
                }
            }
        }
    }

}
