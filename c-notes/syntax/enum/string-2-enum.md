# string 2 enum



{% code-tabs %}
{% code-tabs-item title="string 2 enum" %}
```csharp
 var test = Enum.Parse(typeof(enUIEventID), tempStrs[1]);
CUIEventManager.GetInstance().DispatchUIEvent((Framework.enUIEventID)test);
```
{% endcode-tabs-item %}
{% endcode-tabs %}



