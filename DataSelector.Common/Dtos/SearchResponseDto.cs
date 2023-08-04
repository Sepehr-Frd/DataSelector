namespace DataSelector.Common.Dtos;

public class SearchResponseDto<T>
{
    public string? Term { get; set; }

    public List<T>? Result { get; set; }
}