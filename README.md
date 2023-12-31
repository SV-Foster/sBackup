# Simple Backup Tool

Versatile command-line backup program, configurable with XML backup profiles, automating and simplifying the backup process, that:

* Archives files with WinRAR, a popular file compression tool with recovery record support, a special type of redundant data that can be added to an archive file to protect it from any damage. It allows WinRAR to repair the archive file even if some parts of it are corrupted or lost, as long as the damage is not too severe. This ensures that the backup files are compact and secure, and can be easily opened and extracted with WinRAR or other compatible tools.

* Saves files and folders list, by the path provided, with depth options. You can specify the path of the files and folders that you want to list, and the depth of the subfolders that you want to list recursively. For example, you can list the entire C: drive, or only the C:\Program Files\ folder, not more than 2 subfolders in deep.

* Saves registry keys and values by the path provided. You can specify the path of the registry keys that you want to list, such as HKEY_LOCAL_MACHINE\Software or HKEY_CURRENT_USER\Software. The program saves the registry data in a text file.

* Uses XML files for backup profiles. You can create multiple XML files that contain the backup settings, such as the source and destination paths, the compression level and etc. and start sBackup with profile you want.

* Uses XSD schema for errors preventing. XSD is a schema language that defines the structure and rules of the XML files. The program uses the XSD file to validate the XML files and prevent any errors or inconsistencies in the backup settings.

* Uses the .NET technology, which is a software framework that supports multiple programming languages and platforms. This means that the program can work on any CPU and platform that supports the .NET Framework.


## Requirements

To use the sBackup, you need to have the following software installed:

* A Windows system with the .NET Framework
* WinRAR archiver


## Backup

To backup your files, folders and registry keys first you need to create an XML file that contains the backup settings and place it into the sBackup folder. Here is the schema, defines the structure and rules of the XML files that are used to configure the backup tasks:

![Screenshot](Documents/tasks%20schema.PNG)

The XML schema consists of the following elements:

* BackupTasks: The root element that contains a sequence of Task elements. Each Task element represents a backup task that can be executed by the backup program.

* Task: An element that contains the ID, Type, IsActive, and one of the Files, FilesList, or Registry elements. The ID element is a unique and positive integer that identifies the backup task. The Type element is an integer that indicates the type of the backup task, where 0 means files, 1 means files list, and 2 means registry. The IsActive element is a boolean that indicates whether the backup task is active or not. The Files element contains the settings for backing up files and folders. The FilesList element contains the settings for generating a list of files and folders. The Registry element contains the settings for generating a list of registry keys and values.

* Files: An element that contains the OutFileName, InputRecursive, InputPathList, ExcludeStandard, ExcludeList, and CompressionType elements. The OutFileName element is a string that specifies the name of the archive file that will contain the backup data. The InputRecursive element is a boolean that indicates whether to include the subfolders of the input paths or not. The InputPathList element contains a sequence of Item elements that specify the paths of the files and folders that will be backed up. The ExcludeStandard element is a boolean that indicates whether to exclude the standard files and folders, such as the system files and the recycle bin, or not. The ExcludeList element contains a sequence of Item elements that specify the paths of the files and folders that will be excluded from the backup. The CompressionType element is an integer that indicates the compression level of the archive file, where 0 means no compression, and 1 means maximum compression.

* FilesList: An element that contains the OutFileName, InputPathList, InputRecursiveDepthApplyed, and InputRecursiveDepth elements. The OutFileName element is a string that specifies the name of the text file that will contain the list of files and folders. The InputPathList element contains a sequence of Item elements that specify the paths of the files and folders that will be listed. The InputRecursiveDepthApplyed element is a boolean that indicates whether to apply the recursive depth to the input paths or not. The InputRecursiveDepth element is a positive integer that specifies the depth of the subfolders that will be included in the list, where 0 means no subfolders, and 1 means the first level of subfolders.

* Registry: An element that contains the OutFileName, InputPathList, InputRecursiveDepthApplyed, and InputRecursiveDepth elements. The OutFileName element is a string that specifies the name and location of the registry file that will contain the backup data. The InputPathList element contains a sequence of Item elements that specify the paths of the registry keys and values that will be backed up. The InputRecursiveDepthApplyed element is a boolean that indicates whether to apply the recursive depth to the input paths or not. The InputRecursiveDepth element is a positive integer that specifies the depth of the subkeys that will be included in the backup, where 0 means no subkeys, and 1 means the first level of subkeys.

* Item: An element that contains a string that specifies the path of a file, folder, or registry key. The path can be absolute or relative, and can use environment variables, such as %USERPROFILE% or %APPDATA%, to refer to the system paths.

<br/>

* ID: An element that contains a unique and positive integer that identifies the backup task.

* Type: An element that contains an integer that indicates the type of the backup task, where 0 means files, 1 means files list, and 2 means registry.

* IsActive: An element that indicates whether the backup task is active or not. You can turn off an element temporary by setting this element to false. The default value is true.

* OutFileName: An element that contains a string that specifies the name of the output file that will contain the backup or list data. The output file can be an archive file, a text file, or a registry file, depending on the type of the backup task.

* InputPathList: An element that contains a sequence of Item elements that specify the paths of the files, folders, or registry keys that will be backed up or listed. The path can be absolute or relative, and can use environment variables, such as %USERPROFILE% or %APPDATA%.

* InputRecursive: An element that contains a boolean that indicates whether to include the subfolders of the input paths or not. The default value is true.

* InputRecursiveDepthApplyed: An element that contains a boolean that indicates whether to apply the recursive depth to the input paths or not. The default value is false.

* InputRecursiveDepth: An element that contains a positive integer that specifies the depth of the subfolders or subkeys that will be included in the backup or list, where 0 means no subfolders or subkeys, and 1 means the first level of subfolders or subkeys. The default value is 1.

* ExcludeStandard: An element that contains a boolean that indicates whether to exclude the standard files, folders, or registry keys, such as the system files and the recycle bin, or not. The default value is false.

* ExcludeList: An element that contains a sequence of Item elements that specify the paths of the files, folders, or registry keys that will be excluded from the backup.

* CompressionType: An element that contains an integer that indicates the compression level of the archive file, where 0 means no compression, and 1 means maximum compression. The default value is 1.


### Profile Example

Here is an example of an XML file that conforms to the schema and defines two backup tasks:

```xml
<?xml version="1.0" encoding="utf-16"?>
<BackupTasks xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xsi:noNamespaceSchemaLocation="tasks.xsd">
   <Task>
      <ID>1</ID>
      <Type>0</Type>
      <IsActive>true</IsActive>
      <Files>
         <OutFileName>Files.rar</OutFileName>
         <InputRecursive>true</InputRecursive>
         <InputPathList>
            <Item>C:\Documents\</Item>
            <Item>C:\Pictures\</Item>
         </InputPathList>
         <ExcludeStandard>false</ExcludeStandard>
         <ExcludeList>
            <Item>\Temp\</Item>
            <Item>\Thumbnails\</Item>
         </ExcludeList>
         <CompressionType>1</CompressionType>
      </Files>
   </Task>   
   <Task>
      <ID>2</ID>
      <Type>2</Type>
      <IsActive>false</IsActive>
      <Registry>
         <OutFileName>Registry.txt</OutFileName>
         <InputPathList>
            <Item>HKEY_CURRENT_USER\Software</Item>
            <Item>HKEY_LOCAL_MACHINE\Software</Item>
         </InputPathList>
         <InputRecursiveDepthApplyed>true</InputRecursiveDepthApplyed>
         <InputRecursiveDepth>1</InputRecursiveDepth>
      </Registry>
   </Task>
</BackupTasks>
```

This XML file defines two backup tasks: one for backing up files and folders, and one to list registry keys and values. The first backup task has the ID 1, the type 0, and is active. It will backup the files and folders in the `C:\Documents\` and `C:\Pictures\` folders, excluding the `C:\Documents\Temp\` and `C:\Pictures\Thumbnails\` folders, to the `Files.rar` archive file, using the maximum compression level. The second backup task has the ID 2, the type 2, and is inactive. It will backup the registry keys and values in the `HKEY_CURRENT_USER\Software` and `HKEY_LOCAL_MACHINE\Software` keys, including the first level of subkeys, to the `Registry.txt` registry file.


### Profile Validation

To validate the backup XML files against the schema, you can use the sBackup program or a third-party tool. The sBackup program validates schema automatically every time it reads the profile.
The schema file is located in the same folder as the backup program executable file, and has the name `tasks.xsd`. The schema file uses the XML Schema Definition (XSD) language, which is a standard language for defining the structure and rules of XML documents.


### Starting the backup

Now, when profile is ready, open command prompt window, navigate to sBackup folder, and start sBackup:

```cmd
sBackup b /SavePath E:\
```

This reads profile named "default" from the `default.xml` file and perform the backup operation, saving output files to the root directory of the E: drive. Use `/ProfileName` option if you want to use a profile with a different name.

<br/>
Wait for the backup to finish. You will see a message that indicates whether the backup was successful or not. <!--- You can also check the log file, which is located in the same folder as the XML file, and has the same name with the .log extension. The log file contains the details of the backup, such as the start and end time, the source and destination paths, the compression and encryption settings, and any errors or warnings.-->

Open the archive file with WinRAR or another compatible tool to view or extract the backup data. You will see the files and folders in the archive file, organized by the source paths. <!--- You can also use the password that you specified to decrypt the archive file, if you used encryption.-->

You have successfully backed up your data with the sBackup program!


## Restore

To restore your files and folders you need to use WinRAR or another compatible tool to open and extract the archive file that contains the backup data. To restore your data, follow these steps:

1. Open the archive file with WinRAR or another compatible tool. You will see the files and folders in the archive file, organized by the source paths.
2. Extract the files and folders that you want to restore to the original or a different location. You can use the Extract button or the Extract to option from the right-click menu to extract the data. You can also use the drag and drop feature to extract the data to a folder or a desktop.

Lists of files and registry keys are provided for reference and can't be restored.

You have successfully restored your data, backed up with the sBackup program!


## Building

sBackup uses the Microsoft Visual Studio 2022 for its builds.

To build sBackup from source files with Microsoft Visual Studio, you can use either the graphical mode or the command-line mode. Here are the instructions for both methods:

### Graphical mode
1. Open Microsoft Visual Studio and select Open a project or solution from the start page or the File menu.
2. Browse to the folder where the .sln file is located and select it. This will load the project in Microsoft Visual Studio.
3. Select the configuration for the project by using the drop-down menus on the toolbar. For example, you can choose Debug or Release for the configuration.
4. Build the project by clicking on the Build menu and selecting Build Solution. You can also use the keyboard shortcut Ctrl+Shift+B.
5. Run the project by clicking on the Debug menu and selecting Start Debugging. You can also use the keyboard shortcut F5

### Command-line mode
1. Open a Developer Command Prompt for Microsoft Visual Studio. You can find it in the Start menu under Microsoft Visual Studio Tools.
2. Navigate to the folder where the .sln file is located by using the cd command.
3. Invoke the MSBuild tool to build the project. You can specify various options and flags for the tool. For example, the following command builds the project with the Release configuration:
```
msbuild sBackup.sln /p:Configuration=Release
```
4. Run your executable by doubleclicking on it.


## Authors

This program was written and is maintained by SV Foster.


## License

This program is available under EULA, see [EULA text file](EULA.txt) for the complete text of the license. This program is free for personal, educational and/or non-profit usage.
