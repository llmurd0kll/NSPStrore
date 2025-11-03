using Microsoft.AspNetCore.Http;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;
using Microsoft.AspNetCore.Hosting;

namespace NspStore.Web.Services
    {
    public class ImageService
        {
        private readonly IWebHostEnvironment _env;

        public ImageService(IWebHostEnvironment env)
            {
            _env = env;
            }

        public async Task<(string OriginalUrl, string MediumUrl, string ThumbUrl)> SaveAsync(IFormFile file)
            {
            if (file == null || file.Length == 0)
                throw new ArgumentException("Файл пустой");

            var fileName = Guid.NewGuid() + Path.GetExtension(file.FileName);

            // Папки
            var originalFolder = Path.Combine(_env.WebRootPath, "images", "original");
            var mediumFolder = Path.Combine(_env.WebRootPath, "images", "medium");
            var thumbFolder = Path.Combine(_env.WebRootPath, "images", "thumbs");

            Directory.CreateDirectory(originalFolder);
            Directory.CreateDirectory(mediumFolder);
            Directory.CreateDirectory(thumbFolder);

            // Пути
            var originalPath = Path.Combine(originalFolder, fileName);
            var mediumPath = Path.Combine(mediumFolder, fileName);
            var thumbPath = Path.Combine(thumbFolder, fileName);

            // Сохраняем оригинал
            using (var stream = new FileStream(originalPath, FileMode.Create))
                {
                await file.CopyToAsync(stream);
                }

            // Создаём уменьшенные копии через ImageSharp
            using (var image = await Image.LoadAsync(file.OpenReadStream()))
                {
                // Medium (например, 800px по ширине)
                image.Mutate(x => x.Resize(new ResizeOptions
                    {
                    Mode = ResizeMode.Max,
                    Size = new Size(800, 0)
                    }));
                await image.SaveAsync(mediumPath);

                // Thumb (например, 200px по ширине)
                image.Mutate(x => x.Resize(new ResizeOptions
                    {
                    Mode = ResizeMode.Max,
                    Size = new Size(200, 0)
                    }));
                await image.SaveAsync(thumbPath);
                }

            // Возвращаем относительные URL для сохранения в БД
            return (
                OriginalUrl: $"/images/original/{fileName}",
                MediumUrl: $"/images/medium/{fileName}",
                ThumbUrl: $"/images/thumbs/{fileName}"
            );
            }
        public void Delete(string url)
            {
            if (string.IsNullOrEmpty(url))
                return;

            var path = Path.Combine(_env.WebRootPath, url.TrimStart('/'));
            if (File.Exists(path))
                {
                File.Delete(path);
                }
            }
        }
    }
