using UnityEngine;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEditor.iOS.Xcode;
using System.Collections;
using System.IO;

public class XCodeProjectModPro : MonoBehaviour
{
    //[PostProcessBuild]
    //public static void OnPostprocessBuild(BuildTarget buildTarget, string path)
    //{

    //    if (buildTarget != BuildTarget.iOS) return;

    //        // 添加额外的 AssetsLibrary.framework
    //        // 用于检测相册权限等功能

    //        string projPath = PBXProject.GetPBXProjectPath(path);
    //        PBXProject proj = new PBXProject();

    //        proj.ReadFromString(File.ReadAllText(projPath));
    //        string target = proj.TargetGuidByName("Unity-iPhone");

    //        // add extra framework(s)
    //        proj.AddFrameworkToProject(target, "AssetsLibrary.framework", false);

    //        // set code sign identity & provisioning profile
    //        proj.SetBuildProperty(target, "CODE_SIGN_IDENTITY", "iPhone Distribution: _______________");
    //        proj.SetBuildProperty(target, "PROVISIONING_PROFILE", "********-****-****-****-************");

    //        // rewrite to file
    //        File.WriteAllText(projPath, proj.WriteToString());
            
    //        // 由于我的开发机是英文系统，但游戏需要设置为中文；
    //        // 需要在修改 Info.plist 中的 CFBundleDevelopmentRegion 字段为 zh_CN

    //        // Get plist
    //        string plistPath = path + "/Info.plist";
    //        PlistDocument plist = new PlistDocument();
    //        plist.ReadFromString(File.ReadAllText(plistPath));

    //        // Get root
    //        PlistElementDict rootDict = plist.root;

    //        // Change value of CFBundleDevelopmentRegion in Xcode plist
    //        rootDict.SetString("CFBundleDevelopmentRegion", "zh_CN");

    //        PlistElementArray urlTypes = rootDict.CreateArray("CFBundleURLTypes");

    //        // add weixin url scheme
    //        PlistElementDict wxUrl = urlTypes.AddDict();
    //        wxUrl.SetString("CFBundleTypeRole", "Editor");
    //        wxUrl.SetString("CFBundleURLName", "weixin");
    //        PlistElementArray wxUrlScheme = wxUrl.CreateArray("CFBundleURLSchemes");
    //        wxUrlScheme.AddString("____________");

    //        // Write to file
    //        File.WriteAllText(plistPath, plist.WriteToString());

    //}
}