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
            instance = this; //�� Ŭ���� �ν��Ͻ��� ź������ �� �������� instance�� ���ӸŴ��� �ν��Ͻ��� ������� �ʴٸ�, �ڽ��� �־��ش�.
            DontDestroyOnLoad(this.gameObject); //�� ��ȯ�� �Ǵ��� �ı����� �ʰ� �Ѵ�.
        }
        else
            Destroy(this.gameObject); //�̹� ���������� instance�� �ν��Ͻ��� �����Ѵٸ� �ڽ�(���ο� ���� GameMgr)�� �������ش�.
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
            Debug.Log("��Ʈ����");
        }
        else if (sPort != null && !counter)
        {
            sPort.Close();
            Debug.Log("��Ʈ����");
        }
    }

    public void DataSend(int freqIndex)
    {
        //��Ʈ ������ ����
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
                //data �ޱ�
                int buffer = sPort.ReadByte();
                string msg = sPort.ReadExisting();
                Debug.Log("Recieved: " + msg);
                switch (/*int.Parse(msg)*/buffer)
                {
                    case 1:
                        //CheckHzController.startEX();
                        break;
                    case 2:
                        //SceneManager.LoadScene("MainScene"); //main���� ����
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
