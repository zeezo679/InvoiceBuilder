namespace Api.Requests.Senders;

public record class UpdateContactInfoRequest
{
    public string Email { get; init; } = null!;
    public string? Phone { get; init; }
    public string? Website { get; init; }
}
