using System;
using System.Diagnostics;

namespace TuxStream.Core
{
    public class Download
    {
        public Download()
        {

        }
        public void DownloadMovie(string link)
        {
            string ffmpegCommand = $" -i {link} -bsf:a aac_adtstoasc -loglevel quiet -stats -vcodec copy -c copy -crf 50 file.mp4";

            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "ffmpeg", 
                Arguments = ffmpegCommand,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            Process ffmpegProcess = new Process() { StartInfo = psi };

            ffmpegProcess.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine(e.Data);
                }
            };

            ffmpegProcess.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine("Error: " + e.Data);
                }
            };

            ffmpegProcess.Start();

            ffmpegProcess.BeginErrorReadLine();

            ffmpegProcess.WaitForExit();
        }
    }
}