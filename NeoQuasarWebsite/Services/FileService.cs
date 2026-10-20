using NeoQuasarWebsite.Data;
using NeoQuasarWebsite.DTO;

namespace NeoQuasarWebsite.Services;

public interface IFileService
{
    byte[]? GetFileContent(int fileID);
    void UpdateFileContent(int fileID, byte[] content);
    void AddFileContent(StockItemDTO item);
    public byte[]? GetFileContentByName(string fileName);
}

public class FileService : IFileService
{

    private readonly NeoQuasarDBContext database;
    public FileService (NeoQuasarDBContext dbContext)
    {
        database = dbContext;
    }

    public void AddFileContent(StockItemDTO item)
    {
        FileData? file = database.Files.FirstOrDefault(f => f.Name == item.FileName);
        if (file != null)
        {
            return;
        }
        FileData newFile = new FileData
        {
            Name = item.FileName,
            Size = item.FileSize,
            MIMEType = item.FileType
        };
        database.Add(newFile);
        database.SaveChanges();

        FileContentData fileContent = new FileContentData
        {
            FileID = database.Files.FirstOrDefault(f => f.Name == item.FileName).FileID,
            Content = item.FileContent
        };
        database.Add(fileContent);
        database.SaveChanges();
    }

    public byte[]? GetFileContent(int fileID)
    {
        FileContentData file = database.FileContent.FirstOrDefault(f => f.FileID == fileID);
        if (file != null)
        {
            return file.Content;
        }
        else return null;
    }
    public byte[]? GetFileContentByName(string fileName)
    {
        FileData fileData = database.Files.FirstOrDefault(f => f.Name == fileName);
        if (fileData != null)
        {
            FileContentData file = database.FileContent.FirstOrDefault(f => f.FileID == fileData.FileID);
            if (file != null)
            {
                return file.Content;
            }
        }
        return null;
    }

    public void UpdateFileContent(int fileID, byte[] content)
    {
        FileContentData file = database.FileContent.FirstOrDefault(f => f.FileID == fileID);
        if (file != null){
            file.Content = content;
            database.SaveChanges();
        }
    }
}