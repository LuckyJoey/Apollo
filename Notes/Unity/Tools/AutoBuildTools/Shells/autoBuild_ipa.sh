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

