using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Videoschnitt
{
    class Program
    {
        private static string output;
        private static string firstFile;
        private static string secondFile;
        string consoleOutPut = String.Empty;
        private static bool hasAudio;
        private static string episodeFolder;
        private const string episodeTextFile = "myfile.txt";


        static void Main(string[] args)
        {
            List<string> videofilesWithoutAudio = new List<string>();
            List<string> videoList = new List<string>();
            var BinFolder = Directory.GetParent(Directory.GetCurrentDirectory());
            var ParentOfBinFolder = BinFolder.Parent.FullName;
            string[] profileListsArray = Directory.GetFiles(ParentOfBinFolder + "\\Videos");
            firstFile = profileListsArray[0];
            secondFile = profileListsArray[1];
            output = ParentOfBinFolder + "\\Videos\\ficker.mp4";

            var pg = new Program();

            var ffmpegExePath = Directory.GetCurrentDirectory() + "\\ffmpeg.exe";
            var ffprobeexePath = Directory.GetCurrentDirectory() + "\\ffprobe.exe";
            int n = 0;
            string fileName = string.Empty;
            while (n != 5)
            {
                //Check if Audio Has Audio
                if (n == 0)
                {
                    foreach (var video in profileListsArray)
                    {
                        fileName = Path.GetFileName(video);

                        string parameters = $"-v error -of flat=s_ -select_streams 1 -show_entries stream=duration -of default=noprint_wrappers=1:nokey=1 {fileName}";
                        Process p = pg.CheckIfAudioExists(ffprobeexePath, parameters, fileName);
                        p.Start();
                        p.BeginOutputReadLine();
                        p.WaitForExit();
                        if (hasAudio == false)
                        {
                            videofilesWithoutAudio.Add(fileName);
                        }
                        if (hasAudio == true)
                        {
                            videoList.Add(fileName);
                        }
                    }
                }

                // Videos ohne Audio werden mit leeren Audio befüllt
                if (n == 1)
                {
                    int i = 0;
                    foreach (var video in videofilesWithoutAudio)
                    {
                        string videostring = Path.GetFileNameWithoutExtension(video);
                        string parameters = $"-y -f lavfi -i anullsrc=channel_layout=stereo:sample_rate=44100 -i {video} -c:v copy -c:a aac -shortest {videostring}abc.mp4";
                        Process process = pg.Execute(ffmpegExePath, parameters);
                        process.Start();
                        process.WaitForExit();
                        //maybe errorhandling
                        //videoList.Add(videostring);

                        videoList.Add(video);
                        i++;
                    }
                    n++;
                }
                //resize resolution to 1920x1080
                if(n==2)
                {
                    foreach(var video in videoList)
                    {
                    string parameter = $"-i {video} -s 1920x1080 -r 60 -strict experimental {video}";
                    Process process = pg.ResizeVideoResolution(ffmpegExePath, parameter);
                    process.Start();
                    process.WaitForExit();
                    }

                }
                //Concatenate Videos (Verkettung)
                if (n == 3)
                {
                    MoveVideoToEpisodeFolder(videoList);
                    GetVideoDuration(videoList);
                    string episodeListFileLocation = CreateVideoList(videoList);
                    //foreach (var video in profileListsArray)
                    //{

                    //    fileName = Path.GetFileName(video);
                    //string parameters = $@"-f concat -safe 0 -i C:\Users\Agrre\Desktop\alte\InsertTitleOnVideo\Videoschnitt\bin\Debug\myfile.txt -c copy geilesVideo.mp4";
                    string parameters = $@"-f concat -safe 0 -i {episodeListFileLocation} scale=1920x1080 -c copy geilesVideo.mp4";


                    //string parameters = "ffmpeg -hide_banner -i 1.mp4 -af volumedetect -vn -f null - 2>&1 | grep mean_volume";
                    // exe path- ffprobe.exe -hide_banner -show_format -show_streams -pretty {video_file}
                    Process process = pg.Execute(ffmpegExePath, parameters);
                        process.Start();
                        process.WaitForExit();
                    //}

                }
                n++;
            }
        }

        private static void GetVideoDuration(List<string> videoList)
        {
            foreach(var video in videoList)
            {
                File.
            }
        }

        private Process ResizeVideoResolution(string ffmpegExePath, string parameter)
        {
            if (!File.Exists(ffmpegExePath))
            {
                Console.WriteLine("ffprobe.exe not found");
                throw new FileNotFoundException();
            }
            var processStartInfo = new ProcessStartInfo(ffmpegExePath)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = parameter
            };

            var process = new Process();
            process.StartInfo = processStartInfo;
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += new DataReceivedEventHandler(MyProcOutputHandler);
            //      process.Exited += OnProcessExited;
            return process;
        }

        private static void MoveVideoToEpisodeFolder(List<string> videoList)
         {
            episodeFolder = Directory.GetCurrentDirectory() + "\\" + Guid.NewGuid().ToString();
            Directory.CreateDirectory(episodeFolder);
            foreach (var video in videoList)
            {
                File.Move(video, episodeFolder+"\\" + video);
            }
            videoList.Clear();
            foreach(var video in Directory.GetFiles(episodeFolder))
            {
                videoList.Add(video);
            }            
        }

        private static string CreateVideoList(List<string> videoList)
        {
            string episodeTextFileLocation = episodeFolder + "\\" + episodeTextFile;
            foreach (var video in videoList)
            {
            using (StreamWriter sr = new StreamWriter(episodeFolder + "\\" + episodeTextFile, true))
            {
                    sr.WriteLine($"file '{episodeFolder}\\{video}'");
            }
            }
            return episodeTextFileLocation;
        }

        private Process CheckIfAudioExists(string exePath, string parameters, string fileName)
        {

            if (!File.Exists(exePath))
            {
                Console.WriteLine("ffprobe.exe not found");
                throw new FileNotFoundException();
            }
            var processStartInfo = new ProcessStartInfo(exePath)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = parameters
            };

            var process = new Process();
            process.StartInfo = processStartInfo;
            process.EnableRaisingEvents = true;
            process.OutputDataReceived += new DataReceivedEventHandler(MyProcOutputHandler);
            Console.WriteLine(fileName);
            //      process.Exited += OnProcessExited;
            return process;
        }


        static bool passedThrough = false;
        private static void MyProcOutputHandler(object sendingProcess, DataReceivedEventArgs outLine)
        {
            // Collect the sort command output. 
            // Nicht leer
            if (!String.IsNullOrEmpty(outLine.Data))
            {

                // return true;
                hasAudio = true;
                passedThrough = true;
                Console.WriteLine("string nicht empty");
            }
            else if (String.IsNullOrEmpty(outLine.Data) && passedThrough == true)
            {
                passedThrough = false;
            }
            // leer
            else
            {
                if (passedThrough == false)
                {
                    // return false;
                    hasAudio = false;
                    Console.WriteLine("leer");
                }
            }
        }



        private Process Execute(string exePath, string parameters)
        {
            string result = String.Empty;

            if (!File.Exists(exePath))
            {
                Console.WriteLine("ffmpeg.exe not found");
                throw new FileNotFoundException();
            }
            var processStartInfo = new ProcessStartInfo(exePath)
            {
                UseShellExecute = false,
                RedirectStandardOutput = true,
                Arguments = parameters
            };

            var process = new Process();
            process.StartInfo = processStartInfo;
            process.EnableRaisingEvents = true;
            //process.OutputDataReceived += (s, e) =>
            //{
            //    consoleOutPut = e.Data;
            //};
            process.OutputDataReceived += new DataReceivedEventHandler(OnProcessExited);

            // process.Exited += OnProcessExited;
            return process;



        }

        private void OnProcessExited(object sender, EventArgs e)
        {

            var process = (Process)sender;

            string standardOutput = consoleOutPut;
            try
            {
                if (process.ExitCode != 0)
                {
                    Console.WriteLine(standardOutput);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            process.Dispose();
        }
    }
}
