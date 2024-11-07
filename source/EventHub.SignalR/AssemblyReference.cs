using System.Reflection;

namespace EventHub.SignalR;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}