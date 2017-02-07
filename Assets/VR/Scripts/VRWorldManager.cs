using System;
using UnityEngine;
using Vector4 = UnityEngine.Vector4;

namespace Assets.VR.Scripts
{
    public class VRWorldManager : MonoBehaviour
    {
        public Camera SensorPrefab;
        public Camera ProjectorPrefab;
        public GameObject SurfacePrefab;
        public Camera UserViewCameraPrefab;
        public GameObject UserViewProjectorPrefab;
        public Shader ProjectorShader;

        private VRLoadCalibration _caveConfiguration = new VRLoadCalibration();

        private Camera _sensor;
        private Camera[] _projectors;
        private GameObject[] _surfaces;
        private GameObject[,] _surfaceCorners;
        private Camera[] _userViewCameras;
        private RenderTexture[] _userViewTextures;
        private GameObject[] _userViewProjectors;
        private Material[] _userViewMaterials;

        private int _caveLayer = 31;

        private GameObject _head;
        private int _numberProjectors;
        private int _textureWidth = 1024;
        private int _textureHeight = 1024;

        void Start()
        {
            var caveConfiguration = Load();
            InstantiateCAVE(caveConfiguration);
            _head = _sensor.transform.FindChild("Body").transform.FindChild("Head").gameObject;

            _surfaceCorners = new GameObject[_numberProjectors, 4];
            for (int surface = 0; surface < _numberProjectors; surface++)
                for (int corner = 0; corner < 4; corner++)
                    _surfaceCorners[surface, corner] = _surfaces[surface].transform.GetChild(corner).gameObject;

            int numberOfDisplays = Display.displays.Length < _projectors.Length ? Display.displays.Length : _projectors.Length;
            for (int i = 1; i < numberOfDisplays; i++)
                Display.displays[i].Activate();
        }

        private void InstantiateCAVE(VRLoadCalibration caveConfiguration)
        {
            _numberProjectors = caveConfiguration.Projectors.Length;

            //Instatiate the Sensor:
            _sensor = Instantiate(SensorPrefab, new Vector3(caveConfiguration.Sensors.Position.x, caveConfiguration.Sensors.Position.y, caveConfiguration.Sensors.Position.z) + transform.position, Quaternion.Euler(caveConfiguration.Sensors.Rotation.x, caveConfiguration.Sensors.Rotation.y, caveConfiguration.Sensors.Rotation.z), transform);
            _sensor.gameObject.layer = _caveLayer;

            //Instantiate the Projectors:
            int index = 0;
            _projectors = new Camera[caveConfiguration.Projectors.Length];
            foreach (var projector in caveConfiguration.Projectors)
            {
                _projectors[index] = Instantiate(ProjectorPrefab, new Vector3(projector.Position.x, projector.Position.y, projector.Position.z) + transform.position, Quaternion.Euler(projector.Rotation.x, projector.Rotation.y, projector.Rotation.z), transform);
                _projectors[index].fieldOfView = projector.FOV;
                SetObliqueness(0, projector.Fy, _projectors[index]);
                _projectors[index].gameObject.layer = _caveLayer;
                _projectors[index].cullingMask = 1 << (_caveLayer - projector.Display);
                _projectors[index].cullingMask |= 1 << _caveLayer;
                _projectors[index].targetDisplay = projector.Display - 1;
                index++;
            }

            //Instantiate the Surfaces:
            index = 0;
            _surfaces = new GameObject[caveConfiguration.Surfaces.Length];
            foreach (var surface in caveConfiguration.Surfaces)
            {
                _surfaces[index] = Instantiate(SurfacePrefab, new Vector3(surface.Position.x, surface.Position.y, surface.Position.z) + transform.position, Quaternion.Euler(surface.Rotation.x, surface.Rotation.y, surface.Rotation.z), transform);
                _surfaces[index].transform.localScale = new Vector3(surface.Size.x / 10, surface.Size.y / 10, surface.Size.z / 10);
                _surfaces[index].layer = _caveLayer - surface.Display;
                index++;
            }

            //Instantiate the user view cameras (cameras attached to the user head in the virtual world):
            _userViewCameras = new Camera[caveConfiguration.Projectors.Length];
            _userViewTextures = new RenderTexture[caveConfiguration.Projectors.Length];
            for (int i = 0; i < caveConfiguration.Projectors.Length; i++)
            {
                index = i;
                _userViewCameras[index] = Instantiate(UserViewCameraPrefab, new Vector3(0, 0, 0) + transform.position, Quaternion.Euler(0, 0, 0), transform);
                _userViewCameras[index].gameObject.layer = _caveLayer;
                _userViewTextures[index] = new RenderTexture(_textureWidth, _textureHeight, 24, RenderTextureFormat.ARGB32);
                _userViewTextures[index].antiAliasing = 2;
                _userViewTextures[index].Create();
                _userViewCameras[index].targetTexture = _userViewTextures[index];
                //_userViewCameras[index].cullingMask = 1 << ();      //The user is set to only see the default layer. Change this culling mask if you want the camera to see different layers. 
            }

            //Instantiate the user view projectors (projectors attached to the user head in the CAVE world):
            _userViewProjectors = new GameObject[caveConfiguration.Projectors.Length];
            _userViewMaterials = new Material[caveConfiguration.Projectors.Length];
            for (int i = 0; i < caveConfiguration.Projectors.Length; i++)
            {
                index = i;
                _userViewProjectors[index] = Instantiate(UserViewProjectorPrefab, new Vector3(0, 0, 0) + transform.position, Quaternion.Euler(0, 0, 0), transform);
                _userViewProjectors[index].gameObject.layer = _caveLayer;
                _userViewProjectors[index].GetComponent<Projector>().ignoreLayers &= ~(1 << _surfaces[index].layer);
                _userViewMaterials[index] = new Material(ProjectorShader);
                _userViewMaterials[index].SetTexture("_ShadowTex", _userViewTextures[index]);    //Sets the shader _ShadowTex property (shadow texture) as my render texture
                _userViewProjectors[index].GetComponent<Projector>().material = _userViewMaterials[index];
            }
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                Quit();
            }

            SetHead();
        }

        private void SetHead()
        {
            //Set the position
            SetHeadPosition();

            //Set the FOV & Orientation
            for (int cameraIndex = 0; cameraIndex < _numberProjectors; cameraIndex++)
            {
                SetHeadFovAndOrientation(cameraIndex);
            }

        }

        private void SetHeadFovAndOrientation(int index)
        {
            //Set FOV

            var cameraToTopLeft = _surfaceCorners[index, 0].transform.position - _head.transform.position;
            //var cameraToTopRight = SurfaceCorners[1].transform.position - Head.transform.position;
            var cameraToBottomRight = _surfaceCorners[index, 2].transform.position - _head.transform.position;
            //var cameraToBottomLeft = SurfaceCorners[3].transform.position - Head.transform.position;

            //float angleLeftVertical = Vector3.Angle(cameraToTopLeft, cameraToBottomLeft);
            //float angleRightVertical = Vector3.Angle(cameraToTopRight, cameraToBottomRight);
            //float angleTopHorizontal = Vector3.Angle(cameraToTopLeft, cameraToTopRight);
            //float angleBottomHorizontal = Vector3.Angle(cameraToBottomLeft, cameraToBottomRight);

            //var angleVertical = (angleLeftVertical > angleRightVertical) ? angleLeftVertical : angleRightVertical;
            //var angleHorizontal = (angleTopHorizontal > angleBottomHorizontal) ? angleTopHorizontal : angleBottomHorizontal;

            //HeadProjector.GetComponent<Projector>().fieldOfView = angleVertical;
            //HeadCamera.fieldOfView = angleVertical;

            float angle = Vector3.Angle(cameraToTopLeft, cameraToBottomRight);
            _userViewProjectors[index].GetComponent<Projector>().fieldOfView = angle;
            _userViewCameras[index].fieldOfView = angle;

            //Set the orientation

            //HeadProjector.transform.LookAt(SurfaceCorners[0].transform,Vector3.forward);
            //HeadProjector.transform.Rotate(angleVertical/2, 0, 0);
            //HeadProjector.transform.Rotate(0, angleHorizontal / 2, 0);

            //HeadCamera.transform.rotation = HeadProjector.transform.rotation;
            _userViewProjectors[index].transform.LookAt(_surfaces[index].transform, Vector3.up);
            _userViewCameras[index].transform.rotation = _userViewProjectors[index].transform.rotation;
        }

        private void SetHeadPosition()
        {
            foreach (var userCamera in _userViewCameras)
                userCamera.transform.position = _head.transform.position;

            foreach (var userProjector in _userViewProjectors)
                userProjector.transform.position = _head.transform.position;
        }

        private VRLoadCalibration Load()
        {
            var path = Application.dataPath + "/StreamingAssets/" + "caveCalibration.xml";
            _caveConfiguration.LoadCAVEConfiguration(path);

            String[] arguments = Environment.GetCommandLineArgs();
            for (int n = 1; n < arguments.Length; n++)
            {
                switch (arguments[n])
                {
                    case ("-mode"):
                        //Todo: Some behaviour
                        break;
                    default:
                        break;
                }
            }

            return _caveConfiguration;
        }

        private void Quit()
        {
            Application.Quit();
        }

        void SetObliqueness(float horizObl, float vertObl, Camera cam)
        {
            Matrix4x4 mat = cam.projectionMatrix;
            mat[0, 2] = horizObl;
            mat[1, 2] = vertObl;
            cam.projectionMatrix = mat;
        }
    }
}
