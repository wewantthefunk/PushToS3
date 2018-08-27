using Amazon.S3;
using Amazon;
using Amazon.S3.Transfer;
using System;
using System.Collections;

namespace PushToS3
{
    public class AWS
    {
        public bool sendMyFileToS3(string localFilePath, string bucketName, string subDirectoryInBucket, string fileNameInS3, string s3ServiceUrl, string awsaccess, string awssecret, out string info, out string fullBucket)
        {
            info = string.Empty;
            fullBucket = string.Empty;

            AmazonS3Config config = new AmazonS3Config();
            config.ServiceURL = s3ServiceUrl;

            RegionEndpoint regionEndpoint = RegionEndpoint.USEast1;

            IAmazonS3 client = new AmazonS3Client(awsaccess, awssecret, regionEndpoint);

            TransferUtility utility = new TransferUtility(client);

            TransferUtilityUploadRequest request = new TransferUtilityUploadRequest();

            try
            {
                if (string.IsNullOrWhiteSpace(subDirectoryInBucket))
                {
                    request.BucketName = bucketName;
                }
                else
                {
                    if (!bucketName.EndsWith("/"))
                    {
                        bucketName += "/";
                    }

                    request.BucketName = bucketName + subDirectoryInBucket;
                }
                if (request.BucketName.EndsWith("/"))
                {
                    request.BucketName = request.BucketName.Substring(0, request.BucketName.Length - 1);
                }
                request.Key = fileNameInS3;
                request.FilePath = localFilePath;
                request.StorageClass = S3StorageClass.Standard;
                request.CannedACL = S3CannedACL.PublicRead;
                request.AutoCloseStream = true;

                try
                {
                    utility.Upload(request);
                }
                catch(Exception e)
                {
                    throw new Exception(fileNameInS3 + "\n" + e.ToString());
                }

                info = request.Key + Environment.NewLine + request.FilePath + Environment.NewLine + request.BucketName;
                IEnumerator keys = request.Metadata.Keys.GetEnumerator();
                for (int x=0; x< request.Metadata.Keys.Count; x++)
                {
                    info += request.Metadata[keys.Current.ToString()];
                    keys.MoveNext();
                }
                fullBucket = request.BucketName + "/" + fileNameInS3;
            }
            catch (Exception e)
            {
                return false;
            }
            finally
            {
                utility.Dispose();
                client.Dispose();
                config = null;
                request = null;

            }

            return true;
        }
    }
}
