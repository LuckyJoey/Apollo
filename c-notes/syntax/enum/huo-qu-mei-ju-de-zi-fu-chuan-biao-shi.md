# 获取枚举的字符串表示

```text

TimeOfDay time=TimeOfDay.Afternoon;
Console.WriteLine(time.ToString());
从字符串中获取枚举值：
TimeOfDay time2=(TimeOfDay)Enum.Parse(typeof(TimeOfDay),"afternoon",true);
Console.WriteLine((int)time2);
注:Enum.Parse函数有三个参数，一是枚举类型，二是字符串，三是是否忽略大小写。typeof后面再讨论
```



