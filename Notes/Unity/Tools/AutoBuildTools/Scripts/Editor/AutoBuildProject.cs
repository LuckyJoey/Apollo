using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System;
using UnityEngine.iOS;
public class AutoBuildProject : Editor {
    [MenuItem("Tool/AutoBuild_AndroidAPK")]
    public static void AutoBuildAndroidAPK()
    {
        BuildTargetGroup buildTargetGroup = BuildTargetGroup.Android;
        BuildTarget buildTarget = BuildTarget.Android;
        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup,buildTarget);
        //// keystore 路径, G:\keystore\one.keystore
        //PlayerSettings.Android.keystoreName = "G:\\keystore\\one.keystore";
        //// one.keystore 密码
        //PlayerSettings.Android.keystorePass = "123456";
        // one.keystore 别名
        //PlayerSettings.Android.keyaliasName = "bieming1";
        //// 别名密码
        //PlayerSettings.Android.keyaliasPass = "123456";

        string apkName = string.Format("./Android/Apks/{0}.apk", "Test");
        string res = BuildPipeline.BuildPlayer(GetBuildScenes(), apkName, buildTarget, BuildOptions.None);
        AssetDatabase.Refresh();
    }
    [MenuItem("Tool/AutoBuild_XCode")]
    public static void AutoBuildXCode()
    {
        //打包之前先设置一下 预定义标签， 我建议大家最好 做一些  91 同步推 快用 PP助手一类的标签。 这样在代码中可以灵活的开启 或者关闭 一些代码。
        //因为 这里我是承接 上一篇文章， 我就以sharesdk做例子 ，这样方便大家学习 ，
        //PlayerSettings.SetScriptingDefineSymbolsForGroup(BuildTargetGroup.iOS, "USE_SHARE");
        BuildTargetGroup buildTargetGroup = BuildTargetGroup.iOS;
        BuildTarget buildTarget = BuildTarget.iOS;
        EditorUserBuildSettings.SwitchActiveBuildTarget(buildTargetGroup, buildTarget);


        //PlayerSettings.bundleVersion = "";
        string pathName = string.Format("./XCodeProjects/{0}","Test"); 
        string res = BuildPipeline.BuildPlayer(GetBuildScenes(), pathName, buildTarget, BuildOptions.None);
        AssetDatabase.Refresh();
    }

    //在这里找出你当前工程所有的场景文件，假设你只想把部分的scene文件打包 那么这里可以写你的条件判断 总之返回一个字符串数组。
    static string[] GetBuildScenes()
    {
        List<string> names = new List<string>();

        foreach (EditorBuildSettingsScene e in EditorBuildSettings.scenes)
        {
            if (e == null)
                continue;
            if (e.enabled)
                names.Add(e.path);
        }
        return names.ToArray();
    }
    private static string[] s;
    static void BuildPackage()
    {
        s = System.Environment.GetCommandLineArgs();
        var target = s[s.Length - 3];
        if (target.Equals("0")) //windows
        {
            BuildForStandaloneWindows();
        }
        else if (target.Equals("1")) //android
        {
            //BuildForAndroid();
        }
        else if (target.Equals("2"))  //ios
        {
            //BuildForIOS();
        }
        //其他平台自己看着写就行 
        s = null;
    }

    static void BuildForStandaloneWindows()
    {
        string[] levels = { "Assets/Scenes/1.unity" };
        string path = GetExportPath(BuildTarget.StandaloneWindows);
        PlayerSettings.defaultIsFullScreen = false;
        PlayerSettings.companyName = companyName;
        PlayerSettings.productName = productName;
        PlayerSettings.resizableWindow = false;
        PlayerSettings.displayResolutionDialog = ResolutionDialogSetting.Disabled;

        var tx = Resources.Load<Texture2D>("icon");
        Texture2D[] txs = { tx, tx, tx, tx, tx, tx, tx, };
        var lengt = PlayerSettings.GetIconsForTargetGroup(BuildTargetGroup.Standalone);
        if (txs != null)
            PlayerSettings.SetIconsForTargetGroup(BuildTargetGroup.Standalone, txs);
        var st = BuildPipeline.BuildPlayer(levels, path, BuildTarget.StandaloneWindows, BuildOptions.None);
    }
    private static string GetExportPath(BuildTarget target)
    {
        //var s = System.Environment.GetCommandLineArgs();
        string path = s[s.Length - 2];
        string name = string.Empty;
        if (target == BuildTarget.Android)
        {
            name = "/" + s[s.Length - 1] + ".apk";
        }
        else if (target == BuildTarget.iOS)
        {
            name = "/" + s[s.Length - 1];
        }
        else if (target == BuildTarget.StandaloneWindows)
        {
            name = "/" + s[s.Length - 1] + ".exe";
        }
        UnityEngine.Debug.Log(path);
        UnityEngine.Debug.Log(name);
        string exepath = @path + name;
        UnityEngine.Debug.Log(exepath);
        return exepath;
    }
    private static string companyName
    {
        get
        {
            return s[s.Length - 5];
        }
    }
    private static string productName
    {
        get
        {
            return s[s.Length - 4];
        }
    }
}
