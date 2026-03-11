using PartsManager.Shared.DTOs;

namespace PartsManager.Api.Services;

public interface IStockService
{
    Task InboundAsync(InboundDto dto);
    Task<OutboundResultDto> OutboundAsync(OutboundDto dto);
    Task AuditAsync(AuditDto dto);
}

