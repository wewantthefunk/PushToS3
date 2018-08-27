using Newtonsoft.Json;
using System;
using System.IO;

namespace PushToS3
{
    class Program
    {
        static void Main(string[] args)
        {
            S3FileList s3FileList = JsonConvert.DeserializeObject<S3FileList>(File.ReadAllText(args[0]));

            AWS aws = new AWS();

            foreach (S3File file in s3FileList.files)
            {
                string info = string.Empty;
                string fullBucket = string.Empty;
                string fileName = file.file.Replace("\\", "/");
                int start = file.file.LastIndexOf("/");
                if(start < 0)
                {
                    start = 0;
                }
                string s3FileName = file.file.Substring(start).Replace("/", string.Empty);
                Console.Write("Moving: " + fileName + ": ");
                bool b = aws.sendMyFileToS3(fileName, s3FileList.s3bucket, file.subDir, s3FileName, s3FileList.s3url, s3FileList.awsAccessKey, s3FileList.awsSecretKey, out info, out fullBucket);
                Console.WriteLine(b);
            }
        }
    }
}
