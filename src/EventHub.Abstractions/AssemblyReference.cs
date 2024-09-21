using System.Reflection;

namespace EventHub.Abstractions;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}