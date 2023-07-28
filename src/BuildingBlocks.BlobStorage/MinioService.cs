using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Minio;
using Minio.Exceptions;
using Newtonsoft.Json;

namespace BlobStorage;

public class MinioService : IMinioService
{
    private readonly ILogger<MinioService> _logger;
    private readonly MinioClient _minioClient;

    public MinioService(MinioClient minioClient, ILogger<MinioService> logger)
    {
        _minioClient = minioClient;
        _logger = logger;
    }

    public async Task<string?> CreateReadOnlyBucketAsync(string bucket, string id, IFormFile file, CancellationToken cancellationToken)
    {
        if (!await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket), cancellationToken))
        {
            await _minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucket),
                cancellationToken);
            string policy = JsonConvert.SerializeObject(new
            {
                Version="2012-10-17",
                Statement = new[] {
                  new {
                      Effect = "Allow",
                      Principal = "*",
                      Action = new[] {"s3:GetObject"},
                      Resource = new[] {$"arn:aws:s3:::{bucket}/*"},
                      Sid = ""
                  }
              }
            });
            await _minioClient.SetPolicyAsync(new SetPolicyArgs().WithBucket(bucket).WithPolicy(policy), cancellationToken);
        }

        PutObjectResponse result = await _minioClient.PutObjectAsync(
                                       new PutObjectArgs().WithBucket(bucket).WithObject(id).WithObjectSize(file.Length)
                                           .WithStreamData(file.OpenReadStream()), cancellationToken);
        return result.ObjectName;
    }

    public async Task<string?> CreateBucketAsync(string bucket, string id, IFormFile file, CancellationToken cancellationToken)
    {
        if (!await _minioClient.BucketExistsAsync(new BucketExistsArgs().WithBucket(bucket), cancellationToken))
        {
            await _minioClient.MakeBucketAsync(
                new MakeBucketArgs().WithBucket(bucket),
                cancellationToken);
        }

        PutObjectResponse result = await _minioClient.PutObjectAsync(
                                       new PutObjectArgs().WithBucket(bucket).WithObject(id).WithObjectSize(file.Length)
                                           .WithStreamData(file.OpenReadStream()), cancellationToken);
        return result.ObjectName;
    }

    public async Task<bool> DeleteAsync(string bucket, string id, CancellationToken cancellationToken = default)
    {
        await _minioClient.RemoveObjectAsync(
            new RemoveObjectArgs().WithBucket(bucket).WithObject(id),
            cancellationToken);
        return true;
    }
}
