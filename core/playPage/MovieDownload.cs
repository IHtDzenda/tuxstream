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

            // Create a new ProcessStartInfo
            ProcessStartInfo psi = new ProcessStartInfo()
            {
                FileName = "ffmpeg", // Assuming FFmpeg is in your system's PATH
                Arguments = ffmpegCommand,
                RedirectStandardOutput = true,
                RedirectStandardError = true,
                UseShellExecute = false,
                CreateNoWindow = true
            };

            // Create a new process
            Process ffmpegProcess = new Process() { StartInfo = psi };

            // Event handler for capturing output
            ffmpegProcess.OutputDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine(e.Data);
                }
            };

            // Event handler for capturing errors
            ffmpegProcess.ErrorDataReceived += (sender, e) =>
            {
                if (!string.IsNullOrEmpty(e.Data))
                {
                    Console.WriteLine("Error: " + e.Data);
                }
            };

            // Start FFmpeg
            ffmpegProcess.Start();

            //ffmpegProcess.BeginOutputReadLine();
            ffmpegProcess.BeginErrorReadLine();

            // Wait for FFmpeg to exit
            ffmpegProcess.WaitForExit();
        }
    }
}