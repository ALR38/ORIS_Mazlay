using System;

namespace Application.Common.Interfaces;

/// <summary>Кто залогинен сейчас.</summary>
public interface ICurrentUserService
{
    Guid  Id              { get; }
    bool  IsAuthenticated { get; }
    bool  IsInRole(string role);
}