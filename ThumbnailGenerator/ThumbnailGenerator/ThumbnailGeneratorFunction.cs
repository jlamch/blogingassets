//using ImageResizer;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using SixLabors.Primitives;
using System.IO;

namespace ThumbnailGenerator
{
  public static class ThumbnailGeneratorFunction
  {
    [FunctionName("ThumbnailGenerator")]
    public static void Run(
      [BlobTrigger("blogging-assets/{name}", Connection = "jlbloggingconnection")]Stream inputblob,
      [Blob("blogging-assets-thumbs/th-{name}", FileAccess.Write, Connection = "jlbloggingconnection")]Stream outputBlob,
      string name,
      ILogger log)
    {
      using (Image<Rgba32> image = Image.Load(inputblob))
      {
        image.Mutate(x => x
                .Resize(new ResizeOptions
                {
                  Mode = ResizeMode.Max,
                  Size = new Size(750, 600)
                }).BackgroundColor(new Rgba32(0, 0, 0)));

        image.SaveAsPng(outputBlob);
      }

      log.LogInformation($"C# Blob trigger function Processed blob\n Name:{name} \n Size: {inputblob.Length} Bytes");
    }
  }
}
