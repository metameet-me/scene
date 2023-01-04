using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;

public class LoginDialog : EditorWindow
{ 


   

    [MenuItem("Examples/Editor Password field usage")]
    public static void Init()
    {
        if (MetameetConfig.window == null)
        {
            MetameetConfig.window = new LoginDialog();
            MetameetConfig.window.titleContent = new GUIContent("Metameet");
            MetameetConfig.window.Show();
        }
        else
        {
            MetameetConfig.window.Focus();
        }
    }

 

    void OnGUI()
    {
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUI.BeginChangeCheck();
        EditorGUILayout.LabelField("Select the environmnet you wish to upload to");
        MetameetConfig._selected = EditorGUILayout.Popup("Environment", MetameetConfig._selected, MetameetConfig._options);

        if (EditorGUI.EndChangeCheck())
        {
            switch (MetameetConfig._selected)
            {
                case 1:
                    MetameetConfig.BaseURLWebService = "https://game-data-dev.fruss.net/";
                    break;
                case 2:
                    MetameetConfig.BaseURLWebService = "https://game-data-staging.fruss.net/";
                    break;
                case 3:
                    MetameetConfig.BaseURLWebService = "https://game-data-prod.fruss.net/";
                    break;
                case 4:
                    MetameetConfig.BaseURLWebService = "https://localhost:44336/";
                    break;

                default:
                    MetameetConfig.BaseURLWebService = null;
                    break;
            }
        }


        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();

        if (MetameetConfig.token == null)
        {
            EditorGUILayout.LabelField("Please enter your username and password to login");
            MetameetConfig.username = EditorGUILayout.TextField("Username:", MetameetConfig.username);
            MetameetConfig.password = EditorGUILayout.PasswordField("Password:", MetameetConfig.password);
            var IDPServer = "https://account.metameet.me/frussusers.onmicrosoft.com/B2C_1A_RBAC_ROPC_AUTH/oauth2/v2.0/token/";
            if (GUILayout.Button("Login"))
            {
                MAASTokenRequest mmasTokenRequest = new MAASTokenRequest() { username = MetameetConfig.username, password = MetameetConfig.password };
                MetameetConfig.token = MAASLogin.PostLogin(IDPServer, mmasTokenRequest);
                if (MetameetConfig.token != null)
                {
                    EditorUtility.DisplayDialog("Login Successful",
      "Enter the world id and environment to continue", "Ok");
                }
                else
                {
                    EditorUtility.DisplayDialog("Login Failed",
    "please try again", "Ok");
                }
            }
        }

  

        if(MetameetConfig.token != null)
        {
            EditorGUILayout.TextArea("You are Logged in");
            if (GUILayout.Button("Reenter Credentials"))
            {
                MetameetConfig.token = null;
            }
        }
   
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Enter the id of the world you want to modify");
        MetameetConfig.EventID = EditorGUILayout.TextField("World id:", MetameetConfig.EventID);
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        EditorGUILayout.Space();
        if (GUILayout.Button("Validate Scene"))
        {
            if (!SceneValidation.ValidateScene())
            {
                EditorUtility.DisplayDialog("Invalid Scene", "Check the console log for details", "Ok");
            }
        }


    }

    public static class MetameetConfig
    {
        public static string password = "";
        public static string username = "";
        public static  int _selected = 0;
        public static string[] _options = new string[5] { "Select An Environment", "Dev", "Staging", "Prod", "Local" };
        public static string EventID = "";
        public static LoginDialog window;
        public static string BaseURLWebService = "https://localhost:44336/";
        public static MAASToken token;
    }

}

