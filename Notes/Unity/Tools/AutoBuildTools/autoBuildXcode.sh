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
