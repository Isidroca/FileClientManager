# FileClientManager

Use
```c#
startup.cs

services.AddScoped<IFileClient, AzureFileShareClient>(client => {
    var azureConnectionString = Configuration["AzureStorageConnectionString"];
    return new AzureFileShareClient(azureConnectionString);
});
```

# you api/mvc controller
```c#
    private readonly IFileClient _fileClient;
    
    public FilesController(IFileClient fileClient)
    {
        _fileClient = fileClient;
    }
  ```  
# by local file you don't not need azure connection string

```c#
public async Task<IActionResult> UploadToAzureStore(IFormFile file) {

 
        using (var fileStream = file.OpenReadStream())
        {
            await _fileClient.SaveFile("you_store_name", you_file_name, fileStream);
        }
 
        return RedirectToAction("Index");
    }
 }
 ```
