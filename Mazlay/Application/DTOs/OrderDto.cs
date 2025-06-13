using System;

namespace Application.DTOs;

/// <summary>Передаём в SignalR только то, что нужно клиенту.</summary>
public record OrderDto(Guid OrderId, Guid UserId, decimal Total);