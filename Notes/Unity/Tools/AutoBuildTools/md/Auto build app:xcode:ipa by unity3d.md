# Auto build app/xcode/ipa by unity3d

##Unity Editor Scripts

### AutoBuildProject.CS

```
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

```

### XCodeAPI

#### XCodeProjectMod.CS

```
using UnityEngine;
//#if UNITY_STANDALONE_OSX
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections.Generic;
using System.IO;
//#endif

public class XCodeProjectMod : MonoBehaviour
{
//#if UNITY_STANDALONE_OSX
    private const string SETTING_DATA_PATH = "Assets/Editor/XCodeAPI/Setting/XcodeProjectSetting.asset";
    [PostProcessBuild]
    private static void OnPostprocessBuild(BuildTarget buildTarget, string buildPath)
    {
        if (buildTarget != BuildTarget.iOS)
            return;
        PBXProject pbxProject = null;
        XcodeProjectSetting setting = null;
        string pbxProjPath = PBXProject.GetPBXProjectPath(buildPath);
        string targetGuid = null;
        Debug.Log("开始设置.XCodeProj");

        setting = AssetDatabase.LoadAssetAtPath<XcodeProjectSetting>(SETTING_DATA_PATH);
        pbxProject = new PBXProject();
        pbxProject.ReadFromString(File.ReadAllText(pbxProjPath));
        targetGuid = pbxProject.TargetGuidByName(PBXProject.GetUnityTargetName());

        pbxProject.SetBuildProperty(targetGuid, XcodeProjectSetting.ENABLE_BITCODE_KEY, setting.EnableBitCode ? "YES" : "NO");
        pbxProject.SetBuildProperty(targetGuid, XcodeProjectSetting.DEVELOPMENT_TEAM, setting.DevelopmentTeam);
        pbxProject.SetBuildProperty(targetGuid, XcodeProjectSetting.GCC_ENABLE_CPP_EXCEPTIONS, setting.EnableCppEcceptions ? "YES" : "NO");
        pbxProject.SetBuildProperty(targetGuid, XcodeProjectSetting.GCC_ENABLE_CPP_RTTI, setting.EnableCppRtti ? "YES" : "NO");
        pbxProject.SetBuildProperty(targetGuid, XcodeProjectSetting.GCC_ENABLE_OBJC_EXCEPTIONS, setting.EnableObjcExceptions ? "YES" : "NO");

        if (!string.IsNullOrEmpty(setting.CopyDirectoryPath))
            DirectoryProcessor.CopyAndAddBuildToXcode(pbxProject, targetGuid, setting.CopyDirectoryPath, buildPath, "");

        //编译器标记（Compiler flags）
        foreach (XcodeProjectSetting.CompilerFlagsSet compilerFlagsSet in setting.CompilerFlagsSetList)
        {
            foreach (string targetPath in compilerFlagsSet.TargetPathList)
            {
                if (!pbxProject.ContainsFileByProjectPath(targetPath))
                    continue;
                string fileGuid = pbxProject.FindFileGuidByProjectPath(targetPath);
                List<string> flagsList = pbxProject.GetCompileFlagsForFile(targetGuid, fileGuid);
                flagsList.Add(compilerFlagsSet.Flags);
                pbxProject.SetCompileFlagsForFile(targetGuid, fileGuid, flagsList);
            }
        }

        //引用内部框架
        foreach (string framework in setting.FrameworkList)
        {
            pbxProject.AddFrameworkToProject(targetGuid, framework, false);
        }

        //引用.tbd文件
        foreach (string tbd in setting.TbdList)
        {
            pbxProject.AddFileToBuild(targetGuid, pbxProject.AddFile("usr/lib/" + tbd, "Frameworks/" + tbd, PBXSourceTree.Sdk));
        }

        //设置OTHER_LDFLAGS
        pbxProject.UpdateBuildProperty(targetGuid, XcodeProjectSetting.LINKER_FLAG_KEY, setting.LinkerFlagArray, null);
        //设置Framework Search Paths
        pbxProject.UpdateBuildProperty(targetGuid, XcodeProjectSetting.FRAMEWORK_SEARCH_PATHS_KEY, setting.FrameworkSearchPathArray, null);
        File.WriteAllText(pbxProjPath, pbxProject.WriteToString());

        //已经存在的文件，拷贝替换
        foreach (XcodeProjectSetting.CopeFiles file in setting.CopeFilesList)
        {
            File.Copy(Application.dataPath + file.sourcePath, buildPath + file.copyPath, true);
        }

        //File.Copy(Application.dataPath + "/Editor/XCodeAPI/UnityAppController.h", buildPath + "/Classes/UnityAppController.h", true);
        //File.Copy(Application.dataPath + "/Editor/XCodeAPI/UnityAppController.mm", buildPath + "/Classes/UnityAppController.mm", true);

        //设置Plist
        InfoPlistProcessor.SetInfoPlist(buildPath, setting);
    }
//#endif
}
```

#### DirectoryProcessor.CS

```
using UnityEditor.iOS.Xcode;
using System.IO;


public static class DirectoryProcessor
{
    // 拷贝并增加到项目
    public static void CopyAndAddBuildToXcode(PBXProject pbxProject, string targetGuid, string copyDirectoryPath, string buildPath, string currentDirectoryPath, bool needToAddBuild = true)
    {
        string unityDirectoryPath = copyDirectoryPath;
        string xcodeDirectoryPath = buildPath;

        if (!string.IsNullOrEmpty(currentDirectoryPath))
        {
            unityDirectoryPath = Path.Combine(unityDirectoryPath, currentDirectoryPath);
            xcodeDirectoryPath = Path.Combine(xcodeDirectoryPath, currentDirectoryPath);
            Delete(xcodeDirectoryPath);
            Directory.CreateDirectory(xcodeDirectoryPath);
        }

        foreach (string filePath in Directory.GetFiles(unityDirectoryPath))
        {
            //过滤.meta文件
            string extension = Path.GetExtension(filePath);
            if (extension == ExtensionName.META)
                continue;
            //
            if (extension == ExtensionName.ARCHIVE)
            {
                pbxProject.AddBuildProperty(targetGuid, XcodeProjectSetting.LIBRARY_SEARCH_PATHS_KEY, XcodeProjectSetting.PROJECT_ROOT + currentDirectoryPath);
            }

            string fileName = Path.GetFileName(filePath);
            string copyPath = Path.Combine(xcodeDirectoryPath, fileName);

            //有可能是.DS_Store文件，直接过滤
            if (fileName[0] == '.')
                continue;
            File.Delete(copyPath);
            File.Copy(filePath, copyPath);

            if (needToAddBuild)
            {
                string relativePath = Path.Combine(currentDirectoryPath, fileName);
                pbxProject.AddFileToBuild(targetGuid, pbxProject.AddFile(relativePath, relativePath, PBXSourceTree.Source));
            }
        }

        foreach (string directoryPath in Directory.GetDirectories(unityDirectoryPath))
        {
            string directoryName = Path.GetFileName(directoryPath);
            bool nextNeedToAddBuild = needToAddBuild;
            if (directoryName.Contains(ExtensionName.FRAMEWORK) || directoryName.Contains(ExtensionName.BUNDLE) || directoryName == XcodeProjectSetting.IMAGE_XCASSETS_DIRECTORY_NAME)
            {
                nextNeedToAddBuild = false;
            }
            CopyAndAddBuildToXcode(pbxProject, targetGuid, copyDirectoryPath, buildPath, Path.Combine(currentDirectoryPath, directoryName), nextNeedToAddBuild);
            if (directoryName.Contains(ExtensionName.FRAMEWORK) || directoryName.Contains(ExtensionName.BUNDLE))
            {
                string relativePath = Path.Combine(currentDirectoryPath, directoryName);
                pbxProject.AddFileToBuild(targetGuid, pbxProject.AddFile(relativePath, relativePath, PBXSourceTree.Source));
                pbxProject.AddBuildProperty(targetGuid, XcodeProjectSetting.FRAMEWORK_SEARCH_PATHS_KEY, XcodeProjectSetting.PROJECT_ROOT + currentDirectoryPath);
            }
        }
    }

    //拷贝文件夹或者文件
    public static void CopyAndReplace(string sourcePath, string copyPath)
    {
        Delete(copyPath);
        Directory.CreateDirectory(copyPath);
        foreach (var file in Directory.GetFiles(sourcePath))
        {
            if (Path.GetExtension(file) == ExtensionName.META)
                continue;
            File.Copy(file, Path.Combine(copyPath, Path.GetFileName(file)));
        }
        foreach (var dir in Directory.GetDirectories(sourcePath))
        {
            CopyAndReplace(dir, Path.Combine(copyPath, Path.GetFileName(dir)));
        }
    }

    //删除目标文件夹以及文件夹内的所有文件
    public static void Delete(string targetDirectoryPath)
    {
        if (!Directory.Exists(targetDirectoryPath))
            return;
        string[] filePaths = Directory.GetFiles(targetDirectoryPath);
        foreach (string filePath in filePaths)
        {
            File.SetAttributes(filePath, FileAttributes.Normal);
            File.Delete(filePath);
        }
        string[] directoryPaths = Directory.GetDirectories(targetDirectoryPath);
        foreach (string directoryPath in directoryPaths)
        {
            Delete(directoryPath);
        }
        Directory.Delete(targetDirectoryPath, false);
    }
}
```

#### ExtensionName.CS

```
using UnityEngine;
using System.Collections;

public static class ExtensionName
{
    public const string META = ".meta";
    public const string ARCHIVE = ".a";
    public const string FRAMEWORK = ".framework";
    public const string BUNDLE = ".bundle";
}
```

#### InfoPlistProcessor.CS

```
using System.Collections.Generic;
using UnityEditor.iOS.Xcode;
using System.IO;

//Info.Plist修改设置
public static class InfoPlistProcessor
{
    private static string GetInfoPlistPath(string buildPath)
    {
        return Path.Combine(buildPath, XcodeProjectSetting.INFO_PLIST_NAME);
    }

    private static PlistDocument GetInfoPlist(string buildPath)
    {
        string plistPath = GetInfoPlistPath(buildPath);
        PlistDocument plist = new PlistDocument();
        plist.ReadFromFile(plistPath);
        return plist;
    }

    private static void SetPrivacySensiticeData(PlistDocument plist, List<string> permission, string description = "")
    {
        PlistElementDict rootDict = plist.root;
        int count = permission.Count;
        for (int i = 0; i < count; i++)
        {
            rootDict.SetString(permission[i], description);
        }
    }

    private static void SetApplicationQueriesSchemes(PlistDocument plist, List<string> _applicationQueriesSchemes)
    {
        PlistElementArray queriesSchemes;
        int count = _applicationQueriesSchemes.Count;
        string queriesScheme = null;

        if (plist.root.values.ContainsKey(XcodeProjectSetting.APPLICATION_QUERIES_SCHEMES_KEY))
            queriesSchemes = plist.root[XcodeProjectSetting.APPLICATION_QUERIES_SCHEMES_KEY].AsArray();
        else
            queriesSchemes = plist.root.CreateArray(XcodeProjectSetting.APPLICATION_QUERIES_SCHEMES_KEY);

        for (int i = 0; i < count; i++)
        {
            queriesScheme = _applicationQueriesSchemes[i];
            if (!queriesSchemes.values.Contains(new PlistElementString(queriesScheme)))
                queriesSchemes.AddString(queriesScheme);
        }
    }

    private static void SetBackgroundModes(PlistDocument plist, List<string> modes)
    {
        PlistElementDict rootDict = plist.root;
        PlistElementArray bgModes = rootDict.CreateArray("UIBackgroundModes");
        int count = modes.Count;
        for (int i = 0; i < count; i++)
        {
            bgModes.AddString(modes[i]);
        }
    }

    private static void SetURLSchemes(PlistDocument plist, List<XcodeProjectSetting.BundleUrlType> urlList)
    {
        PlistElementArray urlTypes;
        PlistElementDict itmeDict;
        if (plist.root.values.ContainsKey(XcodeProjectSetting.URL_TYPES_KEY))
            urlTypes = plist.root[XcodeProjectSetting.URL_TYPES_KEY].AsArray();
        else
            urlTypes = plist.root.CreateArray(XcodeProjectSetting.URL_TYPES_KEY);

        for (int i = 0; i < urlList.Count; i++)
        {
            itmeDict = urlTypes.AddDict();
            itmeDict.SetString(XcodeProjectSetting.URL_TYPE_ROLE_KEY, "Editor");
            itmeDict.SetString(XcodeProjectSetting.URL_IDENTIFIER_KEY, urlList[i].identifier);
            PlistElementArray schemesArray = itmeDict.CreateArray(XcodeProjectSetting.URL_SCHEMES_KEY);
            if (itmeDict.values.ContainsKey(XcodeProjectSetting.URL_SCHEMES_KEY))
                schemesArray = itmeDict[XcodeProjectSetting.URL_SCHEMES_KEY].AsArray();
            else
                schemesArray = itmeDict.CreateArray(XcodeProjectSetting.URL_SCHEMES_KEY);
            //TODO:按理说要排除已经存在的，但由于我们是新生成，所以不做排除
            for (int j = 0; j < urlList[i].bundleSchmes.Count; j++)
            {
                schemesArray.AddString(urlList[i].bundleSchmes[j]);
            }
        }
    }

    public static void SetInfoPlist(string buildPath, XcodeProjectSetting setting)
    {
        PlistDocument plist = GetInfoPlist(buildPath);
        SetPrivacySensiticeData(plist, setting.privacySensiticeData, "privacySensiticeData");
        SetApplicationQueriesSchemes(plist, setting.ApplicationQueriesSchemes);
        SetBackgroundModes(plist, setting.BackgroundModes);
        SetURLSchemes(plist, setting.BundleUrlTypeList);
        plist.WriteToFile(GetInfoPlistPath(buildPath));
    }





  

}
```

#### XcodeProjectSetting.CS

```
using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// Xcode项目的一些设定值
/// </summary>
public class XcodeProjectSetting : ScriptableObject
{
    public const string PROJECT_ROOT = "$(PROJECT_DIR)/";
    public const string IMAGE_XCASSETS_DIRECTORY_NAME = "Unity-iPhone";
    public const string LINKER_FLAG_KEY = "OTHER_LDFLAGS";
    public const string FRAMEWORK_SEARCH_PATHS_KEY = "FRAMEWORK_SEARCH_PATHS";
    public const string LIBRARY_SEARCH_PATHS_KEY = "LIBRARY_SEARCH_PATHS";
    public const string ENABLE_BITCODE_KEY = "ENABLE_BITCODE";
    public const string DEVELOPMENT_TEAM = "DEVELOPMENT_TEAM";
    public const string GCC_ENABLE_CPP_EXCEPTIONS = "GCC_ENABLE_CPP_EXCEPTIONS";
    public const string GCC_ENABLE_CPP_RTTI = "GCC_ENABLE_CPP_RTTI";
    public const string GCC_ENABLE_OBJC_EXCEPTIONS = "GCC_ENABLE_OBJC_EXCEPTIONS";
    public const string INFO_PLIST_NAME = "Info.plist";

    public const string URL_TYPES_KEY = "CFBundleURLTypes";
    public const string URL_TYPE_ROLE_KEY = "CFBundleTypeRole";
    public const string URL_IDENTIFIER_KEY = "CFBundleURLName";
    public const string URL_SCHEMES_KEY = "CFBundleURLSchemes";
    public const string APPLICATION_QUERIES_SCHEMES_KEY = "LSApplicationQueriesSchemes";

    #region XCodeproj
    public bool EnableBitCode = false;
    public bool EnableCppEcceptions = true;
    public bool EnableCppRtti = true;
    public bool EnableObjcExceptions = true;

    //要拷贝到XCode内的文件的路径
    public string CopyDirectoryPath = "Assets/Editor/XCodeAPI/Frameworks";
    //AppleDevelopment内AppID表示
    public string DevelopmentTeam = "";
    //引用的内部Framework
    public List<string> FrameworkList = new List<string>() { };
    //引用的内部.tbd
    public List<string> TbdList = new List<string>() { };
    //设置OtherLinkerFlag
    public string[] LinkerFlagArray = new string[] { };
    //设置FrameworkSearchPath
    public string[] FrameworkSearchPathArray = new string[] { "$(inherited)", "$(PROJECT_DIR)/Frameworks" };

    #region 针对单个文件进行flag标记
    [System.Serializable]
    public struct CompilerFlagsSet
    {
        public string Flags;
        public List<string> TargetPathList;

        public CompilerFlagsSet(string flags, List<string> targetPathList)
        {
            Flags = flags;
            TargetPathList = targetPathList;
        }
    }

    public List<CompilerFlagsSet> CompilerFlagsSetList = new List<CompilerFlagsSet>()
    {
        /*new CompilerFlagsSet ("-fno-objc-arc", new List<string> () {"Plugin/Plugin.mm"})*/ //实例，请勿删除
    };
    #endregion

    #endregion

    #region 拷贝文件
    [System.Serializable]
    public struct CopeFiles
    {
        public string sourcePath;
        public string copyPath;

        public CopeFiles(string sourcePath, string copyPath)
        {
            this.sourcePath = sourcePath;
            this.copyPath = copyPath;
        }
    }

    public List<CopeFiles> CopeFilesList = new List<CopeFiles>() { };
    #endregion

    #region info.Plist
    //白名单
    public List<string> ApplicationQueriesSchemes = new List<string>() { };

    //iOS10新的特性
    public List<string> privacySensiticeData = new List<string>() { };

    #region 第三方平台URL Scheme
    [System.Serializable]
    public struct BundleUrlType
    {
        public string identifier;
        public List<string> bundleSchmes;

        public BundleUrlType(string identifier, List<string> bundleSchmes)
        {
            this.identifier = identifier;
            this.bundleSchmes = bundleSchmes;
        }
    }

    public List<BundleUrlType> BundleUrlTypeList = new List<BundleUrlType>() { };
    #endregion

    //放置后台需要开启的功能
    public List<string> BackgroundModes = new List<string>() { };
    #endregion
}
```

#### XcodeProjectSettingCreator.CS

```
using UnityEngine;
using UnityEditor;


public class XcodeProjectSettingCreator : MonoBehaviour
{
    [MenuItem("Assets/Create/XcodeProjectSetting")]
    public static void CreateAsset()
    {
        string path = AssetDatabase.GenerateUniqueAssetPath("Assets/Editor/XCodeAPI/Setting/XcodeProjectSetting.asset");
        XcodeProjectSetting data = ScriptableObject.CreateInstance<XcodeProjectSetting>();
        AssetDatabase.CreateAsset(data, path);
        AssetDatabase.SaveAssets();
    }
}
```

## Shell Scripts

### autoBuildApk.sh

```
#!/bin/sh

# unity app path
UNITY_PATH=/Applications/Unity/Unity17.3.1p4.app/Contents/MacOS/Unity

# project path
PROJ_PATH=/Users/joey/UnityProjects/AutoBuildTestOne

echo "============== Unity Build APK Begin =============="

$UNITY_PATH -projectPath $PROJ_PATH -executeMethod AutoBuildProject.AutoBuildAndroidAPK -logFile $PROJ_PATH/Android/Android.log -batchMode -quit

echo "============== Unity Build APK Finish =============="
```

### autoBuildXcode.sh

```
#!/bin/sh

# unity app path
UNITY_PATH=/Applications/Unity/Unity17.3.1p4.app/Contents/MacOS/Unity

# project path
PROJ_PATH=/Users/joey/UnityProjects/AutoBuildTestOne

echo "============== Unity Build APK Begin =============="

$UNITY_PATH -projectPath $PROJ_PATH -executeMethod AutoBuildProject.AutoBuildXCode -logFile $PROJ_PATH/XCodeProjects/XCode.log -batchMode -quit

echo "============== Unity Build APK Finish =============="

#IOS打包脚本路径#
BUILD_IOS_PATH=${PROJ_PATH}/XCodeProjects/autoBuild_ipa.sh
XCODE_TRUE_PATH=${PROJ_PATH}/XCodeProjects/Test
IPA_PATH=ipa

#开始生成ipa#
echo "============== Unity Build IPA Begin =============="
echo $XCODE_TRUE_PATH
echo $IPA_PATH
echo $BUILD_IOS_PATH

$BUILD_IOS_PATH $XCODE_TRUE_PATH $IPA_PATH
echo "============== Unity Build IPA Finish =============="


```

### autoBuild_ipa.sh

```
#!/bin/sh

# 参数判断  
if [ $# != 2 ];then  
    echo "Need two params: 1.path of project 2.name of ipa file"  
    exit  
# elif [ ! -d $1 ];then  
#     echo "The first param is not a dictionary."  
#     exit      
fi  
echo "================ Start ipa ================="
# 工程路径  
xcode_project_path=$1  

# IPA名称  
ipa_name=$2  

# build文件夹路径  
build_path=${xcode_project_path}/build  

archive_path=${build_path}/Archive/AutoBuild.xcarchive

# 清理#
xcodebuild clean

# 编译工程  
cd $xcode_project_path  
xcodebuild || exit  

xcodebuild archive \
-project ${xcode_project_path}/Unity-iPhone.xcodeproj \
-scheme Unity-iPhone \
-configuration "Release" \
-archivePath ${archive_path}

xcodebuild -exportArchive \
-exportOptionsPlist ${xcode_project_path}/info.plist \
-archivePath ${archive_path} \
-exportPath ${xcode_project_path}
echo "================= End ipa ==================="
```

