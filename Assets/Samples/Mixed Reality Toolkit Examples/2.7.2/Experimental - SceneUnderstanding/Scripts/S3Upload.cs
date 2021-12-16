using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TD: AWS S3
using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using System;
using System.Threading.Tasks;
using Amazon.Runtime.CredentialManagement;
using Amazon.Runtime;

// TD: Microsoft File Directory
using System.IO;

// TD: Text UI
//using UnityEngine.UI;
using TMPro;

public class S3Upload : MonoBehaviour {

    // TD: AWS S3
    private static IAmazonS3 s3Client;
    private static readonly RegionEndpoint bucketRegion = RegionEndpoint.USWest1;

    // TD: Microsoft File Directory
    private string[] files;
    private List<string> fileList;
    private float timer;

    // TD: file text display
    //public Text text;
    //public TextMesh tm;
    public TMP_Text TMP;
    public TMP_Text bytesFiles;
    public TMP_Text timings;
    private int temp;

    // TD: telemetry/data/timings
    private System.Diagnostics.Stopwatch stopwatch;
    //private string timedEvent;

    private void Start() {
        files = null;
        fileList = new List<string>();

        FindFiles(false);
    }

    void /*async*/ FixedUpdate() {
        if(timer < 60) {
            timer += Time.fixedDeltaTime;
        }
        else {
            FindFiles(true);
            timer = 0;
        }
    }

    public void FetchFiles() {
        FindFiles(true);
        ExportFiles();
    }

    public void FindFiles(bool onlyBytesFiles) {
        if(!onlyBytesFiles) {
            files = Directory.GetFiles(Application.persistentDataPath);//, "*.bytes");
            if(files != null) {
                bytesFiles.text = TMP.text = "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "Files found:" + "\n";
            }
        }
        else {
            // TD: time file finding execution
            System.Diagnostics.Stopwatch file_stopwatch = new System.Diagnostics.Stopwatch();
            file_stopwatch.Start();

            files = Directory.GetFiles(Application.persistentDataPath, "*.bytes");
            if(files != null) {
                bytesFiles.text = TMP.text = "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "Newest .bytes file found:" + "\n";

                file_stopwatch.Stop();
                timings.text = "TD: " + "FindFiles: " + file_stopwatch.ElapsedMilliseconds + "ms elaped.\n\n";
            }
            file_stopwatch.Stop();
        }
        /*
        foreach (string s in files) {
            bytesFiles.text = TMP.text += s + "\n";
        }
        */
        bytesFiles.text = TMP.text = "Latest .bytes file: " + files[files.Length - 1];
    }

    // TD: has potential use, but currently unused
    public void ExportFiles() {
        //fileList =
        if (files != null && files.Length > 0) {
            string bytesFile = files[0];

            ///*
            string filePath = System.IO.Path.Combine(Application.persistentDataPath, bytesFile);
            using (FileStream fs = File.Open(filePath, FileMode.Open)) {

            }
            //*/
        }
    }

    // TD: using async helper function to bypass requirement to call upload code from within an ansync function, so it can be called from anywhere
    public async void UploadS3() {
        if (files != null /*&& files.Length > 0*/) {
            // TD: display the .bytes file uploading, onto the text GUI on the Observer, to the right of its buttons
            bytesFiles.text = TMP.text = "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "Uploading .bytes files:" + "\n";
            bytesFiles.text = TMP.text += files[0] + "\n";
            bytesFiles.text = TMP.text = "\n" + "\n" + "\n" + "\n" + "\n" + "\n" + "Found .bytes files:" + "\n";
            // TD: configure AWS credentials programmatically, but not securely (certificate information saved from plain text)
            // TD: TO-DO: make more secure prgrammatic credential solution 
            bytesFiles.text = TMP.text = "AWSSetup() begins!";
            AWSSteup();
            bytesFiles.text = TMP.text = "AWSSetup() complete!";

            //s3Client = new AmazonS3Client(bucketRegion);
            await WritingAnObjectAsync();
            bytesFiles.text = TMP.text = "WritingAnObjectAsync() complete!";

            Debug.LogWarning("S3 files uploaded!");

        }
        bytesFiles.text = TMP.text = "\n\n\n\n\n" + "Upload complete!\n Total .bytes files saved and uploaded: " + temp + "\n";

    }

    private async Task WritingAnObjectAsync() {
        // TD: stopwatch start
        System.Diagnostics.Stopwatch write_stopwatch = new System.Diagnostics.Stopwatch();
        write_stopwatch.Start();

        bytesFiles.text = TMP.text = "Uploading .bytes scene file...";
        try {
                var putRequest = new PutObjectRequest {
                    BucketName = "map-fragments",
                    //Key = "upload.bytes",
                    //FilePath = "D:\\Documents\\Projects\\AURORA\\map-fragments\\Assets\\Samples\\Mixed Reality Toolkit Examples\\2.7.2\\Experimental - SceneUnderstanding\\Scripts\\upload3.bytes"
                    //FilePath = "C:\\Data\\Users\\AZNno\\AppData\\Local\\Packages\\MRTKTutorials-GettingStarted_pzq3xp76mxafg\\LocalState\\playerprefs.dat.bak"
                    FilePath = files[files.Length - 1]
                };
                temp = files.Length;

                putRequest.Metadata.Add("example-metadata-text", "metadata-tlte");
                PutObjectResponse response = await s3Client.PutObjectAsync(putRequest);
                
                // TD: dummy response for timing purposes
                PutObjectResponse response2 = response;
                
                // TD: stopwatch stop
                //timings.text += "TD: PutObjectResponse response: " + response;
                write_stopwatch.Stop();
                timings.text += "TD: " + "WritingAnObjectAsync: " + write_stopwatch.ElapsedMilliseconds + "ms elaped.\n\n";
        }
        catch (AmazonS3Exception e) {
            Console.WriteLine("Error encountered ***. Message:'{0}' when writing an object", e.Message);
            bytesFiles.text = TMP.text = "\n\n\n\n\n\n" + "Error encountered ***. Message:'{0}' when writing an object" + "\n";
        }
        catch (Exception e) {
            Console.WriteLine("Unknown encountered on server. Message:'{0}' when writing an object", e.Message);
            bytesFiles.text = TMP.text = "\n\n\n\n\n\n" + "Unknown encountered on server. Message:'{0}' when writing an object" + "\n";
        }
    }
    public void AWSSteup() {
        bytesFiles.text = TMP.text = "AWSSetup() begin";
        //AWSWriteCredentials();
        AWSDirectCredentials();
    }

    private void AWSDirectCredentials() {
        var config = new AmazonS3Config {
            RegionEndpoint = RegionEndpoint.USWest1
        };

        var credentials = new BasicAWSCredentials("[REMOVED]", "[REMOVED]"); // TD: removed Dr. Jiasi Chen's AWS S3 credentials, for code commit
        s3Client = new AmazonS3Client(credentials, config);
    }
    ///*
    public void StopwatchStart() {
        stopwatch = new System.Diagnostics.Stopwatch();
        stopwatch.Start();
        timings.text = "";
    }
    public void StopwatchLap(string timedEvent) {
        string description = "TD: " + timedEvent + ": " + stopwatch.ElapsedMilliseconds + "ms elaped.\n\n";

        //Debug.Log(description);
        timings.text += description;
    }

    public void StopwatchEnd() {
        stopwatch.Stop();
    }
    //*/
    // TD: unused below this line: ====================================================================================

    // TD: Warning: Writes AWS Certificiate Credentials directly in as plain text; meant only for alpha build to work
    // TD: TODO: change to secure AWS credential method later
    private void AWSWriteCredentials() { // TD: removed Dr. Jiasi Chen's AWS S3 credentials, for code commit
        bytesFiles.text = TMP.text = "AWSWriteCredentials() begin";

        string profileName = "[default]";
        string aws_access_key_id = "[REMOVED]";
        string aws_secret_access_key = "[REMOVED]";

        bytesFiles.text = TMP.text = "AWSWriteCredentials() strings set";
        var options = new CredentialProfileOptions {
            AccessKey = aws_access_key_id,
            SecretKey = aws_secret_access_key
        };
        bytesFiles.text = TMP.text = "AWSWriteCredentials() options set";

        var profile = new CredentialProfile(profileName, options);
        bytesFiles.text = TMP.text = "AWSWriteCredentials() profile set";

        var sharedFile = new SharedCredentialsFile();
        bytesFiles.text = TMP.text = "AWSWriteCredentials() sharedFile"; // TD: Hololens executes up to here

        sharedFile.RegisterProfile(profile);
        bytesFiles.text = TMP.text = "AWSWriteCredentials() registered profile"; // TD: Hololens is unable to output this "debug" log

        //var netSdkStore = new NetSDKCredentialsFile();
        //netSdkStore.RegisterProfile(profile);
    }

    private void AWSAddRegion() {
        string profileName = "[default]";
        //RegionEndpoint region = new RegionEndpoint();

        var netSdkStore = new NetSDKCredentialsFile();
        CredentialProfile profile;
        if (netSdkStore.TryGetProfile(profileName, out profile)) {
            //profile.Region = region;
            netSdkStore.RegisterProfile(profile);
        }
    }
}
