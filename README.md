# sBackup

Versatile command-line backup program, configurable with .xml backup profiles, automatingand simplifying the backup process:

* It uses the .NET technology, which is a software framework that supports multiple programming languages and platforms. This means that the program can work on any CPU and platform that supports the .NET framework.
* It archives files with WinRAR, which is a popular file compression tool. This ensures that the backup files are compact and secure, and can be easily opened and extracted with WinRAR or other compatible tools.
* It saves files and folders list with depth options by the path provided. You can specify the path of the files and folders that you want to backup, and the depth of the subfolders that you want to include. For example, you can backup the entire C:\ drive, or only the C:\Users folder, or only the C:\Users\user\Documents folder.
* It saves registry keys and values by the path provided. You can specify the path of the registry keys and values that you want to backup, such as HKEY_LOCAL_MACHINE\SOFTWARE or HKEY_CURRENT_USER\Software. The program will save the registry data in a text file.
* It uses XML files as a profile task for files, folders, registry backup. You can create and edit XML files that contain the backup settings, such as the source and destination paths, the compression level and etc.
* It uses XSD for errors preventing. XSD is a schema language that defines the structure and rules of the XML files. The program uses the XSD file to validate the XML files and prevent any errors or inconsistencies in the backup settings.


## Requirements

To install the sBackup program, you need to have the following prerequisites:

* A Windows system with the .NET framework installed
* WinRAR installed on your system


## Backup

To backup your files, folders, and registry keys and values, you need to create or load an XML file that contains the backup settings. You can also edit the XML file manually or with other programs.

![XML SCHEMA HERE]

* Source: The path of the file, folder, or registry key that you want to backup. You can use the Browse button to select the path from a dialog box, or type it manually. You can also use environment variables, such as %USERPROFILE% or %APPDATA%, to refer to the system paths.

* Destination: The path of the archive file that will contain the backup data. You can use the Browse button to select the path from a dialog box, or type it manually. You can also use environment variables, such as %USERPROFILE% or %APPDATA%, to refer to the system paths. You need to use the .rar extension for the archive file name.

* Depth: The depth of the subfolders that you want to include in the backup, if the source is a folder. You can use the slider or the input box to adjust the depth. The depth can be from 0 to 9, where 0 means no subfolders, and 9 means all subfolders.

* Compression: The compression level that you want to use for the archive file. You can use the slider or the input box to adjust the compression level. The compression level can be from 0 to 5, where 0 means no compression, and 5 means maximum compression.

* Password: The password that you want to use to encrypt the archive file. You can type the password in the text box, or leave it blank if you don't want to use encryption. The password can be up to 127 characters long, and can contain any characters.


To backup your data, open command prompt window and start sBackup:
```sBackup b /SavePath E:\```
Read profile named "default" from the default.xml file and perform the backup operation, saving output files to the root directory of the E: drive.


Wait for the backup to finish. You will see a message that indicates whether the backup was successful or not. You can also check the log file, which is located in the same folder as the XML file, and has the same name with the .log extension. The log file contains the details of the backup, such as the start and end time, the source and destination paths, the compression and encryption settings, and any errors or warnings.

Open the archive file with WinRAR or another compatible tool to view or extract the backup data. You will see the files, folders, and registry data in the archive file, organized by the source paths. You can also use the password that you specified to decrypt the archive file, if you used encryption.

You have successfully backed up your data with the sBackup program!


## Restore

To restore your files, folders, and registry keys and values, you need to use WinRAR or another compatible tool to open and extract the archive file that contains the backup data. To restore your data, follow these steps:

1. Open the archive file with WinRAR or another compatible tool. You will see the files and folders in the archive file, organized by the source paths.
2. Extract the files and folders that you want to restore to the original or a different location. You can use the Extract button or the Extract to option from the right-click menu to extract the data. You can also use the drag and drop feature to extract the data to a folder or a desktop.
3. Restart your system if necessary to apply the changes.

You have successfully restored your data with the sBackup program.


## Building

sBackup uses the Microsoft Visual Studio 2022 for its builds.

To build sBackup from source files with Microsoft Visual Studio, you can use either the graphical mode or the command-line mode. Here are the instructions for both methods:

Graphical mode:
1. Open Microsoft Visual Studio and select Open a project or solution from the start page or the File menu.
2. Browse to the folder where the .sln file is located and select it. This will load the project in Microsoft Visual Studio.
3. Select the configuration for the project by using the drop-down menus on the toolbar. For example, you can choose Debug or Release for the configuration.
4. Build the project by clicking on the Build menu and selecting Build Solution. You can also use the keyboard shortcut Ctrl+Shift+B.
5. Run the project by clicking on the Debug menu and selecting Start Debugging. You can also use the keyboard shortcut F5

Command-line mode:
1. Open a Developer Command Prompt for Microsoft Visual Studio. You can find it in the Start menu under Microsoft Visual Studio Tools.
2. Navigate to the folder where the .sln file is located by using the cd command.
3. Invoke the MSBuild tool to build the project. You can specify various options and flags for the tool. For example, the following command builds the project with the Release configuration:

```msbuild sBackup.sln /p:Configuration=Release```


Run your executable by clicking on it.

## Authors

This program was written and is maintained by SV Foster.


## License

This program is available under EULA, see [EULA file](EULA.txt) for the complete text of the license. This program is free for personal, educational and/or non-profit usage.
