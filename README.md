# About SortR

Console utility that helps you automatically sort files from specified folder by their extensions or names. Supports most popular OS' such as all Windows versions, MacOs and linux distributions.

## Documentation

**Configuration file** is located in \User\Roaming\AppData for Windows, \home\User for Linux and \Users\User for MacOs, which is automatically created with the first launch and called "config.json". 

### Settings

"settings": {

    "bufferName": "D:\\Buffer",

    "logsDirectory": "C:\\Users\\User\\AppData\\Roaming\\SortR\\Logs",

    "existingFilesAction": "replace",

    "writeLogs": false,

    "resetConfig": false

  }

__"bufferName"__ - is a path to the folder where all to be sorted files located, SortR will detect all files placed here, even if they are hidden or in the subfolder.

__"logsDirectory"__ - is a path where SortR will save its logs when "writeLogs" is true. 

__"existingFilesAction"__ - decides what to do if the file to be moved already exists, it can be "replace" or "ignore", upper or lower case doesn't matter.

__"writeLogs"__ - decides if SorR' output should be logged to the file. New file crated in "logsDirectory" each time you launch the program.

__"resetConfig"__ - Next launch SortR will use it's default settings and write them to the config file. PREVIOUS CONFIG FILE WILL BE ERASED!


### jobs
{

      "category": "Images",

      "path": "C:\\Users\\User\\Pictures",

      "names": [],

      "extensions": [

        ".png",

        ".jpg",

        ".webp",

        ".gif",

        ".bmp",

        ".heic",

        ".psd",

        ".ai",

        ".svg",

        ".vsdx"

      ]

   }

__Jobs__ is a list of objects named "job", each job is just a number of characteristics and a path to move files that matches them.
Priority of each job is decreases from top, which means that if two jobs will have same characteristics but different paths, job that is placed higher in the config file will be handled first.

__"category"__ - Name of this job, affects basically nothing but it's name in logs.

__"path"__ - Where to move files that matches characteristics.

__"names" and "extensions"__ - Are characteristics, "names" looks for certain words in a files name while "extensions" is obviously for files extensions.
They both are sensitive to the letter case and unnecessary white spaces. Extensions should have dot as a first character.

You may create as much jobs as you want by just writing them to config.json or change those created by default. 

# Thank you

Thank you for checking up this little project, feel free to report any issues and gramma mistakes in the code, there will probably be a lot of them since English isn't my first language.  
