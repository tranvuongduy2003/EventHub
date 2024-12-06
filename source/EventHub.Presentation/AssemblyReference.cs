using System.Reflection;

namespace EventHub.Presentation;

internal static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}
