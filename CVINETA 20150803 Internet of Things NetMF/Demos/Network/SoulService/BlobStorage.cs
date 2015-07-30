using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure;
using Microsoft.WindowsAzure.StorageClient;

//NOTE: This class uses a now deprecated storage client api. Look for the latest API for new apps.

namespace SoulService
{
    public class BlobStorage
    {
        public void PutBlob(byte[] bytes, string blobName, string contentType)
        {
            var blob = mContainer.GetBlobReference(blobName);
            blob.UploadByteArray(bytes);
            blob.Properties.ContentType = contentType;
            blob.SetProperties();
            
        }

        public IEnumerable<string> ListBlobs()
        {
            var myReturn = new List<string>();
            foreach (var item in mContainer.ListBlobs())
            {
                myReturn.Add(item.Uri.AbsoluteUri);
            }
            return myReturn;
        }

        public void Clear()
        {
            foreach (var item in mContainer.ListBlobs())
            {
                var cloudBlob = mContainer.GetBlobReference(item.Uri.ToString());
                cloudBlob.DeleteIfExists();
            }
        }
        private CloudBlobContainer mContainer;
        public BlobStorage(string connectionStringName, string containerName)
        {
            // Retrieve storage account from connection-string
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(
                CloudConfigurationManager.GetSetting(connectionStringName));

            // Create the blob client 
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            // Retrieve a reference to a container 
            // Container name must use lower case
            mContainer = blobClient.GetContainerReference(containerName);

            //// Create the container if it doesn't already exist
            mContainer.CreateIfNotExist();

            ////// Enable public access to blob
            var permissions = mContainer.GetPermissions();
            if (permissions.PublicAccess == BlobContainerPublicAccessType.Off)
            {
                permissions.PublicAccess = BlobContainerPublicAccessType.Blob;
                mContainer.SetPermissions(permissions);
            }

        }
    }
}