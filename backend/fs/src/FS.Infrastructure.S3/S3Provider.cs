using Amazon.S3;
using System;
using System.Collections.Generic;
using System.Text;

namespace FS.Infrastructure.S3;

public class S3Provider
{
    private readonly IAmazonS3 _amazonS3;
    public S3Provider(IAmazonS3 s3client)
    {
        _amazonS3 = s3client;
    }


}
