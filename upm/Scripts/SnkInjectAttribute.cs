using System;

namespace SnkFramework.DependencyInjection
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Interface)]
    public class SnkInjectAttribute : Attribute
    {
    }
}