# Bachelor work to KPI.

ClientUI: Windows Forms app - displays graphs of sensors data and anomalyes. 
CreateNewSensor: Console app - register new sensor and send training data to service.
RandomCutForest: Class Library - main algorithm that searches for anomalies.
RandomCutForestTests: Unit test for RandomCutForest.
Sensor: Console app - Simulates the sensor and sends data to the service.
SensoreService: REST WCF Service - Registers new sensors, receives sensor data, processes them - finds anomalies and stores in the database.