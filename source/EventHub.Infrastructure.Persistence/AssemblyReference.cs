using System.Reflection;

namespace EventHub.Infrastructure.Persistence;

public static class AssemblyReference
{
    public static readonly Assembly Assembly = typeof(AssemblyReference).Assembly;
}