namespace WebProgramlamaOdev.Services
{
    public interface IAIPhotoService
    {
        // Geriye üretilen resmin URL'sini döner
        Task<string> TransformBodyAsync(string localImagePath, string intensity);
    }
}
