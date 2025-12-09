namespace Terminator.Application.Common;

public static class Base64ValidationHelper
{
    private const int MaxStackAllocSizeBytes = 256; 
    
    public static bool IsValidBase64(string base64String)
    {
        Span<byte> buffer = base64String.Length <= MaxStackAllocSizeBytes 
            ? stackalloc byte[base64String.Length] 
            : new byte[base64String.Length];

        return Convert.TryFromBase64String(base64String, buffer, out _);
    }

    public static bool IsValidBase64AndLength(string base64String, int requiredLength)
    {
        Span<byte> buffer = base64String.Length <= MaxStackAllocSizeBytes 
            ? stackalloc byte[base64String.Length] 
            : new byte[base64String.Length];

        return Convert.TryFromBase64String(base64String, buffer, out int bytes)
               && bytes == requiredLength;
    }
}