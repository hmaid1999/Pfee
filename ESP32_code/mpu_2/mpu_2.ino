#include <Wire.h>
#include <MPU6050.h>
#include <BLEDevice.h>
#include <BLEServer.h>
#include <BLEUtils.h>
#include <BLE2902.h>

MPU6050 mpu1(0x68); // Address of the first MPU6050
MPU6050 mpu2(0x69); // Address of the second MPU6050

BLECharacteristic *pCharacteristic;
BLEServer *pServer;
bool deviceConnected = false;

#define SERVICE_UUID "0000FFE0-0000-1000-8000-00805F9B34FB"
#define CHARACTERISTIC_UUID "0000FFE1-0000-1000-8000-00805F9B34FB"

class MyServerCallbacks: public BLEServerCallbacks {
  void onConnect(BLEServer* pServer) {
    deviceConnected = true;
  };

  void onDisconnect(BLEServer* pServer) {
    deviceConnected = false;
  }
};

void setup() {
  Serial.begin(115200);
  Wire.begin();

  mpu1.initialize();
  mpu2.initialize();

  if (!mpu1.testConnection()) {
    Serial.println("MPU6050 (0x68) connection failed");
    while (1);
  }
  Serial.println("MPU6050 (0x68) connection successful");

  if (!mpu2.testConnection()) {
    Serial.println("MPU6050 (0x69) connection failed");
    while (1);
  }
  Serial.println("MPU6050 (0x69) connection successful");

  BLEDevice::init("ESP32_mpu");
  pServer = BLEDevice::createServer();
  pServer->setCallbacks(new MyServerCallbacks());

  BLEService *pService = pServer->createService(SERVICE_UUID);

  pCharacteristic = pService->createCharacteristic(
                      CHARACTERISTIC_UUID,
                      BLECharacteristic::PROPERTY_READ |
                      BLECharacteristic::PROPERTY_WRITE |
                      BLECharacteristic::PROPERTY_NOTIFY
                    );

  pCharacteristic->addDescriptor(new BLE2902());

  pService->start();

  pServer->getAdvertising()->start();
  Serial.println("Waiting for a client connection to ESP32_mpu...");
}

void loop() {
  int16_t ax1, ay1, az1;
  int16_t gx1, gy1, gz1;
  int16_t ax2, ay2, az2;
  int16_t gx2, gy2, gz2;

  // Read from first MPU6050
  mpu1.getMotion6(&ax1, &ay1, &az1, &gx1, &gy1, &gz1);
  float accelX1 = (float)ax1 / 16384.0;
  float accelY1 = (float)ay1 / 16384.0;
  float accelZ1 = (float)az1 / 16384.0;
  int angleX1 = (int)(atan2(accelY1, accelZ1) * 180.0 / PI);

  // Read from second MPU6050
  mpu2.getMotion6(&ax2, &ay2, &az2, &gx2, &gy2, &gz2);
  float accelX2 = (float)ax2 / 16384.0;
  float accelY2 = (float)ay2 / 16384.0;
  float accelZ2 = (float)az2 / 16384.0;
  int angleX2 = (int)(atan2(accelY2, accelZ2) * 180.0 / PI);

  Serial.print("AngleX1: ");
  Serial.print(angleX1);
  Serial.print(" AngleX2: ");
  Serial.println(angleX2);

  if (deviceConnected) {
    char buffer[32];
    snprintf(buffer, sizeof(buffer), "%d,%d", angleX1, angleX2);
    pCharacteristic->setValue(buffer);
    pCharacteristic->notify();
  }

  delay(100);
}
