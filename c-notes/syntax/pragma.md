# \#pragma

\#pragma  
可抑制或还原指定的编译警告

```text

Based on: .NET (2018)C# program that uses pragma directiveusing System;class Program{    static void Main()    {        // This example has unreachable code!        // ... The pragma directives hide the warning.#pragma warning disable        if (false)        {            Console.WriteLine("Perls");        }#pragma warning restore    }}Result    When compiled, no warnings are issued.
```

