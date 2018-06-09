#!/bin/sh

# unity app path
UNITY_PATH=/Applications/Unity/Unity17.3.1p4.app/Contents/MacOS/Unity

# project path
PROJ_PATH=/Users/joey/UnityProjects/AutoBuildTestOne

echo "============== Unity Build APK Begin =============="

$UNITY_PATH -projectPath $PROJ_PATH -executeMethod AutoBuildProject.AutoBuildAndroidAPK -logFile $PROJ_PATH/Android/Android.log -batchMode -quit

echo "============== Unity Build APK Finish =============="

