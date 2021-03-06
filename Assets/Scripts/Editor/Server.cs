using System;
using System.IO;
using UnityEditor;
using UnityEngine;

public class Server
{
    private string[] scenes = { "Assets/Scenes/MainMenu.unity", "Assets/Scenes/Montañas Cochambrosas Online.unity" };

    [MenuItem("Build/Build All")]
    public static void BuildAll()
    {
        BuildWindowsServer();
        //BuildLinuxServer();
        BuildWindowsClient();
    }

    [MenuItem("Build/Build Server (Windows)")]
    public static void BuildWindowsServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/MainMenu.unity", "Assets/Scenes/Montañas Cochambrosas Online.unity" };
        buildPlayerOptions.locationPathName = "Builds/Windows/Server/Server.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode;

        Console.WriteLine("Building Server (Windows)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Built Server (Windows).");
    }

    [MenuItem("Build/Build Server (Linux)")]
    public static void BuildLinuxServer()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/LogIn.unity", "Assets/Scenes/MainMenu.unity", "Assets/Scenes/CharacterSelection.unity", "Assets/Scenes/WaitingLobby.unity", "Assets/Scenes/Montañas Cochambrosas Online.unity", "Assets/Scenes/ResultScreen.unity" };
        buildPlayerOptions.locationPathName = "Builds/Linux/Server/Server.x86_64";
        buildPlayerOptions.target = BuildTarget.StandaloneLinux64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC | BuildOptions.EnableHeadlessMode;

        Console.WriteLine("Building Server (Linux)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Built Server (Linux).");
    }


    [MenuItem("Build/Build Client (Windows)")]
    public static void BuildWindowsClient()
    {
        BuildPlayerOptions buildPlayerOptions = new BuildPlayerOptions();
        buildPlayerOptions.scenes = new[] { "Assets/Scenes/LogIn.unity", "Assets/Scenes/MainMenu.unity", "Assets/Scenes/CharacterSelection.unity", "Assets/Scenes/WaitingLobby.unity", "Assets/Scenes/Montañas Cochambrosas Online.unity", "Assets/Scenes/ResultScreen.unity" };
        buildPlayerOptions.locationPathName = "Builds/Windows/TeFeGe/TeFeGe.exe";
        buildPlayerOptions.target = BuildTarget.StandaloneWindows64;
        buildPlayerOptions.options = BuildOptions.CompressWithLz4HC;

        Console.WriteLine("Building Client (Windows)...");
        BuildPipeline.BuildPlayer(buildPlayerOptions);
        Console.WriteLine("Built Client (Windows).");
    }
}
