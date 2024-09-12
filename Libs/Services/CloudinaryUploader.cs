using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace CondigiBack.Libs.Services
{
    public class CloudinaryUploader
    {
        private readonly Cloudinary cloudinary;

        public CloudinaryUploader(Cloudinary cloudinary)
        {
            this.cloudinary = cloudinary;
        }

        public async Task<RawUploadResult> UploadPdfToCloudinary(byte[] pdf)
        {
            using (var stream = new MemoryStream(pdf))
            {
                var uploadParams = new RawUploadParams
                {
                    File = new FileDescription("contract.pdf", stream),
                    Folder = "contracts",
                    AccessMode = "public",
                };

                return await cloudinary.UploadAsync(uploadParams);
            }
        }
    }
}
