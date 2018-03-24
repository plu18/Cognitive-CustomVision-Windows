// 
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license.
// 
// Microsoft Cognitive Services: https://azure.microsoft.com/en-us/services/cognitive-services
// 
// Microsoft Cognitive Services GitHub:
// https://github.com/Microsoft/Cognitive-CustomVision-Windows
// 
// Copyright (c) Microsoft Corporation
// All rights reserved.
// 
// MIT License:
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED ""AS IS"", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
// 


using Microsoft.Cognitive.CustomVision.Prediction;
using Microsoft.Cognitive.CustomVision.Training;
using Microsoft.Cognitive.CustomVision.Training.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;

namespace CustomVision.Sample
{
    class Program
    {
        private static List<string> imageLists;

        private static List<List<string>> allImages;
        

        static void Main(string[] args)
        {
            // Add your training key from the settings page of the portal
            //76af0d521ed848ce8d4c8c70b7fb1f2b
            //5d0b18588d934e1293008dcf75dd0606
            string trainingKey = "76af0d521ed848ce8d4c8c70b7fb1f2b";


            // Create the Api, passing in the training key
            TrainingApi trainingApi = new TrainingApi() { ApiKey = trainingKey };

            // Create a new project
            Console.WriteLine("Creating new project:");
            var project = trainingApi.CreateProject("SilverFernAI");

            // Load all image folders
            imageLists = Directory.GetDirectories(@"..\..\..\Images").ToList();


            List<Tag> imageTags = new List<Tag>();
            foreach (string imageList in imageLists)
            {
                string imageClass = imageList.Split('\\').LastOrDefault();

                Tag imageTag = trainingApi.CreateTag(project.Id, imageClass);
                imageTags.Add(imageTag);

                DirectoryInfo d = new DirectoryInfo(@imageList);
                FileInfo[] infos = d.GetFiles();

                List<string> images = Directory.GetFiles(@imageList).ToList();

                foreach (var image in images)
                {
                    using (var stream = new MemoryStream(File.ReadAllBytes(image)))
                    {
                        trainingApi.CreateImagesFromData(project.Id, stream, new List<string>() { imageTag.Id.ToString() });
                    }
                }
            }



            //    // Now there are images with tags start training the project
            //    Console.WriteLine("\tTraining");
            //    var iteration = trainingApi.TrainProject(project.Id);

            //    // The returned iteration will be in progress, and can be queried periodically to see when it has completed
            //    while (iteration.Status == "Training")
            //    {
            //        Thread.Sleep(1000);

            //        // Re-query the iteration to get it's updated status
            //        iteration = trainingApi.GetIteration(project.Id, iteration.Id);
            //    }

            //    // The iteration is now trained. Make it the default project endpoint
            //    iteration.IsDefault = true;
            //    trainingApi.UpdateIteration(project.Id, iteration.Id, iteration);
            //    Console.WriteLine("Done!\n");

            //    // Now there is a trained endpoint, it can be used to make a prediction

            //    // Add your prediction key from the settings page of the portal
            //    // The prediction key is used in place of the training key when making predictions
            //    string predictionKey = "<your key here>";

            //    // Create a prediction endpoint, passing in obtained prediction key
            //    PredictionEndpoint endpoint = new PredictionEndpoint() { ApiKey = predictionKey };

            //    // Make a prediction against the new project
            //    Console.WriteLine("Making a prediction:");
            //    var result = endpoint.PredictImage(project.Id, testImage);

            //    // Loop over each prediction and write out the results
            //    foreach (var c in result.Predictions)
            //    {
            //        Console.WriteLine($"\t{c.Tag}: {c.Probability:P1}");
            //    }
            //    Console.ReadKey();
            //}

            //private static void LoadImagesFromDisk()
            //{
            //    // this loads the images to be uploaded from disk into memory
            //    hemlockImages = Directory.GetFiles(@"..\..\..\Images\Hemlock").ToList();
            //    japaneseCherryImages = Directory.GetFiles(@"..\..\..\Images\Japanese Cherry").ToList();
            //    testImage = new MemoryStream(File.ReadAllBytes(@"..\..\..\Images\Test\test_image.jpg"));
            //}
        }
    }
}
