#include <Wire.h>
#include <MPU6050.h>
#include "BluetoothSerial.h"

MPU6050 mpu;
BluetoothSerial SerialBT;

void setup() {
  Serial.begin(115200);
  SerialBT.begin("ESP32_MPU6050"); // Bluetooth device name
  Wire.begin();
  mpu.initialize();

  if (!mpu.testConnection()) {
    Serial.println("MPU6050 connection failed");
    while (1);
  }
  Serial.println("MPU6050 connection successful");
}

void loop() {
  int16_t ax, ay, az;
  int16_t gx, gy, gz;

  mpu.getMotion6(&ax, &ay, &az, &gx, &gy, &gz);

  float accelX = (float)ax / 16384.0;
  float accelY = (float)ay / 16384.0;
  float accelZ = (float)az / 16384.0;

  float angleX = atan2(accelY, accelZ) * 180.0 / PI;
 

  // Send data via Bluetooth
  SerialBT.print("AngleX: ");
  SerialBT.print(angleX);



  // Also print data to Serial Monitor for debugging
  Serial.print("AngleX: ");
  Serial.print(angleX);

 

  delay(100);
}
