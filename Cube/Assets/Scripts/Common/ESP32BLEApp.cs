using System;
using System.Text;
using UnityEngine;
using TMPro;

public class ESP32BLEApp : MonoBehaviour
{
    private string DeviceName = "ESP32_mpu";
    private string ServiceUUID = "0000FFE0-0000-1000-8000-00805F9B34FB";
    private string CharacteristicUUID = "0000FFE1-0000-1000-8000-00805F9B34FB";

    private bool _workingFoundDevice = false;
    private bool _connected = false;
    private string _deviceAddress;

    [SerializeField] private TMP_Text stateText;
    [SerializeField] private TMP_Text angleText1;
    [SerializeField] private TMP_Text angleText2;

    private float angleX1;
    private float angleX2;

    void Reset()
    {
        _workingFoundDevice = false;
        _connected = false;
        _deviceAddress = null;
    }

    void SetStateText(string text)
    {
        if (stateText == null) return;
        stateText.text = text;
    }

    void SetAngleText(float angle1, float angle2)
    {
        if (angleText1 != null)
        {
            angleText1.text = $"angleX1: {angle1}";
        }

        if (angleText2 != null)
        {
            angleText2.text = $"angleX2: {angle2}";
        }
    }

    public void StartProcess()
    {
        SetStateText("Initializing...");

        Reset();
        BluetoothLEHardwareInterface.Initialize(true, false, () => {
            StartScan();
            SetStateText("Initialized");
        }, (error) => {
            BluetoothLEHardwareInterface.Log("Error: " + error);
        });
    }

    void StartScan()
    {
        SetStateText("Scanning for ESP32 devices...");

        BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) => {
            if (name.Contains(DeviceName))
            {
                _workingFoundDevice = true;
                BluetoothLEHardwareInterface.StopScan();

                _deviceAddress = address;
                ConnectToDevice();
                _workingFoundDevice = false;
            }
        }, null, false, false);
    }

    void ConnectToDevice()
    {
        SetStateText("Connecting to ESP32");

        BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) => {
            if (IsEqual(serviceUUID, ServiceUUID) && IsEqual(characteristicUUID, CharacteristicUUID))
            {
                _connected = true;
                SetStateText("Connected to ESP32");
                SubscribeToCharacteristic();
            }
        }, (disconnectedAddress) => {
            BluetoothLEHardwareInterface.Log("Device disconnected: " + disconnectedAddress);
            SetStateText("Disconnected");
            _connected = false;
        });
    }

    void SubscribeToCharacteristic()
    {
        SetStateText("Subscribing to ESP32");

        BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_deviceAddress, ServiceUUID, CharacteristicUUID, null, (address, characteristicUUID, bytes) => {
            string receivedData = Encoding.UTF8.GetString(bytes);

            string[] angleValues = receivedData.Split(',');
            if (angleValues.Length == 2)
            {
                if (float.TryParse(angleValues[0], out float parsedAngleX1) && float.TryParse(angleValues[1], out float parsedAngleX2))
                {
                    angleX1 = parsedAngleX1;
                    angleX2 = parsedAngleX2;
                    SetAngleText(angleX1, angleX2);
                }
            }
        });
    }

    public float GetAngleX1()
    {
        return angleX1;
    }

    public float GetAngleX2()
    {
        return angleX2;
    }

    public bool IsConnected()
    {
        return _connected;
    }

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        StartProcess();
    }

    string FullUUID(string uuid)
    {
        return "0000" + uuid + "-0000-1000-8000-00805F9B34FB";
    }

    bool IsEqual(string uuid1, string uuid2)
    {
        if (uuid1.Length == 4)
            uuid1 = FullUUID(uuid1);
        if (uuid2.Length == 4)
            uuid2 = FullUUID(uuid2);

        return (uuid1.ToUpper().Equals(uuid2.ToUpper()));
    }
}
