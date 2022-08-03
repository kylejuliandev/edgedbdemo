using NetEscapades.EnumGenerators;

namespace EdgeDbDemo.Model;

[EnumExtensions]
public enum TODOState
{
    NotStarted,
    InProgress,
    Complete
}