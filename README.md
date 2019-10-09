# FileClientManager

## Install
nuget package 
```nuget
Install-Package FileClientManager -Version 1.0.0
```


Use
```c#
startup.cs

services.AddScoped<IFileClient, AzureFileShareClient>(client => {
    var azureConnectionString = Configuration["AzureStorageConnectionString"];
    return new AzureFileShareClient(azureConnectionString);
});
```

## Api/mvc controller
```c#
    private readonly IFileClient _fileClient;
    
    public FilesController(IFileClient fileClient)
    {
        _fileClient = fileClient;
    }
  ```  
## Local file you don't not need azure connection string

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
 
## Contributing
Pull requests are welcome. For major changes, please open an issue first to discuss what you would like to change.

## License
[MIT](https://choosealicense.com/licenses/mit/)
