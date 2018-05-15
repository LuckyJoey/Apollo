# 名称空间的别名

```text
using System;
using Introduction=Wrox.ProCSharp.Basics;
class Text{
    public static int Main(){
        Introduction::NamespaceExample NSEx=new Introduction::NamespaceExample();
        Console.WriteLine(NSEx.GetNamespace());
        return 0;
    }
}
namespace Wrox.ProCSharp.Basics{
    class NamespaceExample{
        public string GetNamespace(){
            return this.GetType().Namespace;
        }
    }
}
注：::是名称空间的修饰符
GetType是每个类都有的方法。
```

