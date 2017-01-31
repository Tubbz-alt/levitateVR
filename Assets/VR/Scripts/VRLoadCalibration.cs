using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

namespace Assets.VR.Scripts
{
    /// <summary>
    /// This class is responsible for loading the calibration settings from the xml
    /// </summary>
    public class VRLoadCalibration
    {
        private XmlDocument _xmlDoc;

        private string _path;
        private string _filepath;

        public struct Sensor
        {
            public Vector3 Position;
            public Vector3 Rotation;
        }
        public Sensor Sensors;

        public struct Projector
        {
            public Vector3 Position;
            public Vector3 Rotation;
            public float Fy;
            public float FOV;
            public int Display;
        }
        public Projector[] Projectors;

        public struct Surface
        {
            public Vector3 Position;
            public Vector3 Rotation;
            public Vector3 Size;
            public int Display;
        }
        public Surface[] Surfaces;

        public List<string> LoadProjectorValues(string path, int projectorIndex)
        {
            _filepath = path;
            _xmlDoc = new XmlDocument();

            var projectorValues = new List<string>();

            if (File.Exists(_filepath))
            {
                _xmlDoc.Load(_filepath);

                var configList = _xmlDoc.GetElementsByTagName("Camera" + projectorIndex);

                foreach (XmlNode configInfo in configList)
                {
                    var xmlcontent = configInfo.ChildNodes;

                    foreach (XmlNode xmlsettings in xmlcontent)
                    {
                        projectorValues.Add(xmlsettings.InnerText);
                    }
                }
            }

            return projectorValues;
        }

        public List<string> LoadKinectValues(string path)
        {
            _filepath = path;
            _xmlDoc = new XmlDocument();

            var kinectValues = new List<string>();

            if (File.Exists(_filepath))
            {
                _xmlDoc.Load(_filepath);

                var configList = _xmlDoc.GetElementsByTagName("KinectV2");

                foreach (XmlNode configInfo in configList)
                {
                    var xmlcontent = configInfo.ChildNodes;

                    foreach (XmlNode xmlsettings in xmlcontent)
                    {
                        kinectValues.Add(xmlsettings.InnerText);
                    }
                }
            }
            return kinectValues;
        }

        public void LoadCAVEConfiguration(string filepath)
        {
            _xmlDoc = new XmlDocument();

            if (File.Exists(filepath))
            {
                _xmlDoc.Load(filepath);

                LoadSensorConfiguration(out Sensors);
                LoadProjectorsConfiguration(out Projectors);
                LoadSurfacesConfiguration(out Surfaces);
            }
        }

        private void LoadSensorConfiguration(out Sensor sensor)
        {
            sensor = new Sensor();
            var sensorsConfig = _xmlDoc.GetElementsByTagName("Sensor");
            var sensorConfig = sensorsConfig[0].ChildNodes;
            foreach (XmlNode parameter in sensorConfig)
            {
                switch (parameter.Name)
                {
                    case "Pos.X":
                        sensor.Position.x = float.Parse(parameter.InnerText);
                        break;
                    case "Pos.Y":
                        sensor.Position.y = float.Parse(parameter.InnerText);
                        break;
                    case "Pos.Z":
                        sensor.Position.z = float.Parse(parameter.InnerText);
                        break;
                    case "Rot.X":
                        sensor.Rotation.x = float.Parse(parameter.InnerText);
                        break;
                    case "Rot.Y":
                        sensor.Rotation.y = float.Parse(parameter.InnerText);
                        break;
                    case "Rot.Z":
                        sensor.Rotation.z = float.Parse(parameter.InnerText);
                        break;
                    case "#comment":
                        break;
                    default:
                        Debug.Log("Error Loading the sensor calibration values: " + parameter.Name + " is an unknown parameter");
                        break;
                }
            }
        }

        private void LoadProjectorsConfiguration(out Projector[] projectors)
        {
            var projectorsConfig = _xmlDoc.GetElementsByTagName("Projector");

            projectors = new Projector[projectorsConfig.Count];

            for (int i = 0; i < projectorsConfig.Count; i++)
            {
                projectors[i] = new Projector();
                var projectorConfig = projectorsConfig[i].ChildNodes;
                foreach (XmlNode parameter in projectorConfig)
                {
                    switch (parameter.Name)
                    {
                        case "Pos.X":
                            projectors[i].Position.x = float.Parse(parameter.InnerText);
                            break;
                        case "Pos.Y":
                            projectors[i].Position.y = float.Parse(parameter.InnerText);
                            break;
                        case "Pos.Z":
                            projectors[i].Position.z = float.Parse(parameter.InnerText);
                            break;
                        case "Rot.X":
                            projectors[i].Rotation.x = float.Parse(parameter.InnerText);
                            break;
                        case "Rot.Y":
                            projectors[i].Rotation.y = float.Parse(parameter.InnerText);
                            break;
                        case "Rot.Z":
                            projectors[i].Rotation.z = float.Parse(parameter.InnerText);
                            break;
                        case "Fy":
                            projectors[i].Fy = float.Parse(parameter.InnerText);
                            break;
                        case "FOV":
                            projectors[i].FOV = float.Parse(parameter.InnerText);
                            break;
                        case "Display":
                            projectors[i].Display = int.Parse(parameter.InnerText);
                            break;
                        case "#comment":
                            break;
                        default:
                            Debug.Log("Error Loading the projector calibration values: " + parameter.Name + " is an unknown parameter");
                            break;
                    }
                }
            }
        }

        private void LoadSurfacesConfiguration(out Surface[] surfaces)
        {
            var surfacesConfig = _xmlDoc.GetElementsByTagName("Surface");

            surfaces = new Surface[surfacesConfig.Count];

            for (int i = 0; i < surfacesConfig.Count; i++)
            {
                surfaces[i] = new Surface();
                var surfaceConfig = surfacesConfig[i].ChildNodes;
                foreach (XmlNode parameter in surfaceConfig)
                {
                    switch (parameter.Name)
                    {
                        case "Pos.X":
                            surfaces[i].Position.x = float.Parse(parameter.InnerText);
                            break;
                        case "Pos.Y":
                            surfaces[i].Position.y = float.Parse(parameter.InnerText);
                            break;
                        case "Pos.Z":
                            surfaces[i].Position.z = float.Parse(parameter.InnerText);
                            break;
                        case "Rot.X":
                            surfaces[i].Rotation.x = float.Parse(parameter.InnerText);
                            break;
                        case "Rot.Y":
                            surfaces[i].Rotation.y = float.Parse(parameter.InnerText);
                            break;
                        case "Rot.Z":
                            surfaces[i].Rotation.z = float.Parse(parameter.InnerText);
                            break;
                        case "Size.X":
                            surfaces[i].Size.x = float.Parse(parameter.InnerText);
                            break;
                        case "Size.Y":
                            surfaces[i].Size.y = float.Parse(parameter.InnerText);
                            break;
                        case "Size.Z":
                            surfaces[i].Size.z = float.Parse(parameter.InnerText);
                            break;
                        case "Display":
                            surfaces[i].Display = int.Parse(parameter.InnerText);
                            break;
                        case "#comment":
                            break;
                        default:
                            Debug.Log("Error Loading the surface calibration values: " + parameter.Name + " is an unknown parameter");
                            break;
                    }
                }
            }
        }
    }
}

