using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Xabe.FFmpeg;

namespace Converter
{
    class Program
    {
        public Program()
        {
            Run().GetAwaiter().GetResult();
        }
        static void Main(string[] args)
        {
            Run().GetAwaiter().GetResult();

        }
        internal static async Task Run()
        {
            string savepath = @"C:\Users\Agrre\Desktop\gege";
            Console.Out.WriteLine("[Start] Basic Conversion");
            FileInfo fileToConvert = GetFilesToConvert(savepath).First();
            //Set directory where app should look for FFmpeg executables.
            FFmpeg.SetExecutablesPath(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location));
            //Get latest version of FFmpeg. It's great idea if you don't know if you had installed FFmpeg.
          //  await FFmpegDownloader.GetLatestVersion(FFmpegVersion.Official);

            //Run conversion
            await RunConversion(fileToConvert);

            Console.Out.WriteLine("[End] Basic Conversion");
        }

        private static async Task RunConversion(FileInfo fileToConvert)
        {
            //Save file to the same location with changed extension
            string outputFileName = Path.ChangeExtension(fileToConvert.FullName, ".mp4");

            //Delete file if it already exists
            File.Delete(outputFileName);

            //var conversion = await FFmpeg.Conversions.FromSnippet.ToWebM(fileToConvert.Name, outputFileName);

            var conversion = await FFmpeg.Conversions.FromSnippet.Convert(fileToConvert.FullName, outputFileName);
            await conversion.Start();

            await Console.Out.WriteLineAsync($"Finished converion file [{fileToConvert.Name}]");
        }

        private static IEnumerable<FileInfo> GetFilesToConvert(string directoryPath)
        {
            //Return all files excluding mp4 because I want convert it to mp4
            return new DirectoryInfo(directoryPath).GetFiles().Where(x => x.Extension == ".webm");
        }










        //private static async Task<IConversion> irgendwas()
        //{
        //    string inputVideoPath = Path.Combine("C:", "Desktop", "shgsegesg", "1950Gondola.webm");
        //    string outputPathMp4 = Path.Combine("C:", "Desktop", "shgsegesg", "result.mp4");

        //    IMediaInfo info = await FFmpeg.GetMediaInfo(inputVideoPath);

        //    IStream videoStream = info.VideoStreams.FirstOrDefault()
        //        ?.SetCodec(VideoCodec.h264);
        //    IStream audioStream = info.AudioStreams.FirstOrDefault()
        //        ?.SetCodec(AudioCodec.aac);

        //    return FFmpeg.Conversions.New()
        //        .AddStream(videoStream, audioStream)
        //        .SetOutput(outputPathMp4);
        //}

    }
}
