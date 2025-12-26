namespace Terminator.Web.DTOs;

public class ErrorResponse
{
    public IEnumerable<ErrorDto> Errors { get; set; } = new List<ErrorDto>();
}