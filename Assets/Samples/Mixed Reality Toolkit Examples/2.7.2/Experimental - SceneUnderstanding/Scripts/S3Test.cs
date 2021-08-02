using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

//Unity
using UnityEngine;
using UnityEngine.Events;

// To interact with Amazon S3.
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;

public class S3Test : MonoBehaviour
{
    private static IAmazonS3 s3Client;
    private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest1;
    // Start is called before the first frame update
    void Start()
    {
        LoadFileS3().Wait();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private static async Task LoadFileS3()
    {
        // Before running this app:
        // - Credentials must be specified in an AWS profile. If you use a profile other than
        //   the [default] profile, also set the AWS_PROFILE environment variable.
        // - An AWS Region must be specified either in the [default] profile
        //   or by setting the AWS_REGION environment variable.

        Debug.Log("JC: entered LoadFileS3");

        
        // Create an S3 client object.
        s3Client = new AmazonS3Client(bucketRegion);

        
        // List the buckets owned by the user.
        // Call a class method that calls the API method.
        Debug.Log("JC: Getting a list of your buckets...");

        ReadObjectDataAsync();

        
        //s3Client.ListBucketsAsync().Wait();
        //var listResponse = await MyListBucketsAsync(s3Client);
        /*
        Debug.Log("JC: Number of buckets: " + listResponse.Buckets.Count);
        foreach (S3Bucket b in listResponse.Buckets)
        {
            Debug.Log(b.BucketName);
        }
            
        ReadObjectDataAsync().Wait();
        */
        
    }

    static async Task ReadObjectDataAsync()
    {
        string responseBody = "";
        try
        {
            GetObjectRequest request = new GetObjectRequest
            {
                BucketName = "map-fragments",
                Key = "upstairs.bytes"
            };
            using (GetObjectResponse response = await s3Client.GetObjectAsync(request))
            using (Stream responseStream = response.ResponseStream)
            using (StreamReader reader = new StreamReader(responseStream))
            {
                string title = response.Metadata["x-amz-meta-title"]; // Assume you have "title" as medata added to the object.
                string contentType = response.Headers["Content-Type"];
                Debug.Log("JC: Object metadata, Title: " + title);
                Debug.Log("JC: Content type: " + contentType);

                responseBody = reader.ReadToEnd(); // Now you process the response body.

                //Debug.Log(responseBody);
            }
        }
        catch (AmazonS3Exception e)
        {
            // If bucket or object does not exist
            Debug.Log("JC: Error encountered ***. Message: " + e.Message + " when reading object");
        }
        catch (Exception e)
        {
            Debug.Log("JC: Unknown encountered on server. Message: " + e.Message + " when reading object");
        }
    }


    // 
    // Method to parse the command line.
    private static Boolean GetBucketName(string[] args, out String bucketName)
    {
        Boolean retval = false;
        bucketName = String.Empty;
        if (args.Length == 0)
        {
            Console.WriteLine("\nNo arguments specified. Will simply list your Amazon S3 buckets." +
              "\nIf you wish to create a bucket, supply a valid, globally unique bucket name.");
            bucketName = String.Empty;
            retval = false;
        }
        else if (args.Length == 1)
        {
            bucketName = args[0];
            retval = true;
        }
        else
        {
            Console.WriteLine("\nToo many arguments specified." +
              "\n\ndotnet_tutorials - A utility to list your Amazon S3 buckets and optionally create a new one." +
              "\n\nUsage: S3CreateAndList [bucket_name]" +
              "\n - bucket_name: A valid, globally unique bucket name." +
              "\n - If bucket_name isn't supplied, this utility simply lists your buckets.");
            Environment.Exit(1);
        }
        return retval;
    }


    //
    // Async method to get a list of Amazon S3 buckets.
    private static async Task<ListBucketsResponse> MyListBucketsAsync(IAmazonS3 s3Client)
    {
        Debug.Log("JC: MyListBucketsAsync");
        return await s3Client.ListBucketsAsync();
    }
}
