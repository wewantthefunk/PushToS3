using System;
using System.Collections.Generic;
using System.Text;

namespace PushToS3
{
    public class S3FileList
    {
        public string s3url { get; set; }
        public string s3bucket { get; set; }        
        public string awsAccessKey { get; set; }
        public string awsSecretKey { get; set; }
        public S3File[] files { get; set; }
    }

    public class S3File
    {
        public string file { get; set; }
        public string subDir { get; set; }
    }
}
