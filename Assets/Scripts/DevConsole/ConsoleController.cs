using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Reflection;

namespace DeveloperConsole
{

    public class ConsoleController : MonoBehaviour
    {
        public static ConsoleController instance;

        bool consoleEnabled;

        public GameObject console;
        public CameraController cameraControler;
        public UnityEngine.UI.Button inputButton;
        public UnityEngine.UI.Button CloseButton;
        public TMP_InputField input;

        public TMP_Text textOutput;




        private void Awake()
        {
            instance = this;

            DevConsole.OutPutConsole("Starting..");
        }

        // Start is called before the first frame update
        void Start()
        {
            DevConsole.LoadCommands(new Assembly[] { Assembly.GetAssembly(typeof(ConCommandAttribute)) });
            inputButton.onClick.AddListener(GetInput);
            CloseButton.onClick.AddListener(CloseConsole);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetButtonDown("Console"))
            {

                consoleEnabled = !consoleEnabled;
            }

            if (consoleEnabled)
            {
                console.SetActive(true);
                cameraControler.EnableCam(false);
                PlayerController.instance.onConsole = true;
            }
            else
            {
                console.SetActive(false);
                cameraControler.EnableCam(true);
                PlayerController.instance.onConsole = false;
            }

            if (Input.GetKeyDown(KeyCode.Return) && input.text != "")
            {
                GetInput();
            }

        }

        public void GetInput()
        {
            string[] command = input.text.Split(' ');
            DevConsole.RunCommand(command);
            input.text = "";
        }

        public void CloseConsole()
        {
            if (consoleEnabled)
            {
                consoleEnabled = false;
            }
        }
    }

}