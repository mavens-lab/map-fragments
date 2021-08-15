using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TD: AWS S3
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;

public class S3Upload : MonoBehaviour {

    // TD: AWS S3
    private static IAmazonS3 s3Client;
    private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest1;

    private  void Start() {
        UploadS3();
    }

    void /*async*/ FixedUpdate() {
        
    }

    // TD: using async helper function to bypass requirement to call upload code from within an ansync function, so it can be called from anywhere
    private async void UploadS3() {
        s3Client = new AmazonS3Client(bucketRegion);
        await WritingAnObjectAsync();

        Debug.LogWarning("S3 files uploaded!");
    }

    private async Task WritingAnObjectAsync() {
        try {
            var putRequest = new PutObjectRequest
            {
                BucketName = "map-fragments",
                Key = "upload.bytes",
                FilePath = "D:\\Documents\\Projects\\AURORA\\map-fragments\\Assets\\Samples\\Mixed Reality Toolkit Examples\\2.7.2\\Experimental - SceneUnderstanding\\Scripts\\upload.bytes"
            };

            putRequest.Metadata.Add("example-metadata-text", "metadata-tlte");
            PutObjectResponse response = await s3Client.PutObjectAsync(putRequest);
        }
        catch (AmazonS3Exception e)
        {
            Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
        }
        catch (Exception e)
        {
            Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
        }
    }
}
