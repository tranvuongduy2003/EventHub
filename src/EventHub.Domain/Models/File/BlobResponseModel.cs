namespace EventHub.Domain.Models.File;

public class BlobResponseModel
{
    public BlobResponseModel()
    {
        Blob = new BlobModel();
    }

    public string? Status { get; set; }
    public bool Error { get; set; }
    public BlobModel Blob { get; set; }
}