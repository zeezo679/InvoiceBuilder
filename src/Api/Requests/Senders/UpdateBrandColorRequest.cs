namespace Api.Requests.Senders;

public record class UpdateBrandColorRequest
{
    public string PrimaryColor { get; init; } = null!;
}
