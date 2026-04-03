using System;

namespace Application.Common.DTOs;

public record SenderDto(
    Guid Id,
    string BusinessName,
    string? LegalName,
    string Email,
    string? Phone,
    string City,
    string Country,
    string? LogoUrl,
    bool IsActive,
    DateTime CreatedAt
);
