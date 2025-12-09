namespace Terminator.Core.Result;

// TODO: Add metadata (perhaps Dict<string, object> ?)
public record class Error(
    string Code, 
    string Message);