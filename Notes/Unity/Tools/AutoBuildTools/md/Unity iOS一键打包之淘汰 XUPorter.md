这篇文章是针对Unity5以上的玩家，4.x的还是老老实实用XUPorter吧。很感谢喵神提供的XUPorter,让我们一键打包更方便快捷。也许官方注意到了我们有这种需求，所以在Unity5以后的版本都内置了导出Xcode工程以后对Xcode工程各种操作修改。
XUPorter以前是提供一份配置文件，类似Json格式，我们可以自己填一些东西。新版本提供了一些API，没有提供界面，我就从GitHub上找到了一个功能还算完全的拿过来改了一下。界面如下：
[![20170114105020](http://www.newhappy.com.cn/wp-content/uploads/2017/01/20170114105020.png)](http://www.newhappy.com.cn/wp-content/uploads/2017/01/20170114105020.png)熟悉XUPorter的人应该清楚这里面的一些内容。具体表示些啥我就不深入说了，反正你在XUPorter里面要实现的功能，这里面都有，而且增加了一些新的属性。比较遗憾的是我的Unity5版本比较旧，没有XCode8.2新增加的远程推送开关设置，所以每次上架还是要手动用Xocde点一下。这个在Unity5.4以后的版本就加了，你也可以自己加到代码内。

有个小技巧，比如你不知道BackGround Modes里面，远程推送为啥是remote-motification
你可以打开xcode,选中info.PList 然后右键Open As–>Source Code就能找到字段了。
Privacy SenditiceData设置也是如此。
地址：[XCodeAPI](http://www.newhappy.com.cn/wp-content/uploads/2017/01/XCodeAPI.zip)放在你项目内的Editor目录下就好,此时你就可以与时俱进了