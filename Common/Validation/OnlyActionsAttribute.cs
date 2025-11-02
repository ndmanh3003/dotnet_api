namespace dotnet.Common.Validation;

[AttributeUsage(AttributeTargets.Class, Inherited = true)]
public class OnlyActionsAttribute(params string[] actions) : Attribute
{
    public string[] Actions { get; } = actions ?? [];

    public bool Allows(string actionName)
    {
        if (Actions.Length == 0)
            return true;
        return Actions.Contains(actionName, StringComparer.OrdinalIgnoreCase);
    }
}