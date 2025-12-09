namespace Terminator.Web.DTOs;

// separate error class so that we don't accidentally leak anything
// (e.g. if Error definition changes to include more metadata) 
public record class ErrorDto(
    string Code, 
    string Message);