# deploymentHelper <!-- omit in toc -->

![logo](./logo.png)

This helper program is written to automate complex deployment toolchains with one easy script file.

Written in C# using the .NET Core 3.1.

## Table of contents <!-- omit in toc -->
- [Features](#features)
- [Overview](#overview)
- [Deployment-File strucure](#deployment-file-strucure)
- [Implemented Steps](#implemented-steps)
  - [QT-Deploy Step](#qt-deploy-step)
  - [Upload using FTP](#upload-using-ftp)
  - [Build latex](#build-latex)
  - [Build documentation](#build-documentation)
  - [Clean Step](#clean-step)

## Features
* Automated deployment of QT-C++ Apps on Windows using 'windeployqt'
* Automated documentation build using Doxygen
* Automated FTP Uploads

## Overview
This program uses an deployment file written in XML (for more information about this file: [Deployment-File strucure](#deployment-file-strucure)).

First it analyzes the file while looking for errors. If no errors are found within the file, the application displays all steps and asks if you want to execute them.

## Deployment-File strucure
The deployment file always need to contain
```XML
<?xml version='1.0' ?>
<deployment>
    ...
</deployment>
```
as an overall wrapper. In those `deployment`-tags you can define many different deploymentstep-lists, e.g. to seperate deployments of different applications in one script. One deploymentstep-list is added by using 
```XML
<deploymentsteps type='qt-cpp'>
    ...
</deploymentsteps>
```
where the `type`-attribute needs to be chosen according to the following table:
|type|usecase|
|---|---|
|qt-cpp|Application written in QT & C++|
|cs|Application written in C#|

Within those deploymentstep-lists you can add many steps, which are outlined in [Implemented Steps](#implemented-steps).

## Implemented Steps
### QT-Deploy Step
Uses the 'windeployqt'-tool to deploy an QT-application. 'windeployqt' needs to be in the PATH of your OS.

This kind of step is added by using
```xml
<deploy-qt>
    ...
</deploy-qt>
```
and needs to be filled with arguments (optional) and the path to the executable to be deployed. 

Add an argument by using the `argument`-tag. Some arguments may need an given directory in order to work. To add an directory to an argument, you can use the `dir`-tag, which uses absolute paths by default, but can be changed to relative paths (to the location of the deployment file) by adding the `path-type='rel'` attribute to the tag.

Define the executable by using the `file`-tag. The `file`-tag uses absolute paths by default, but can be changed to relative paths (to the location of the deployment file) by adding the `path-type='rel'` attribute to the tag.

So one QT-Deploy Step could look as follows:
```xml
<deploy-qt>
    <argument>release</argument>
    <argument>no-translations</argument>
    <argument>no-svg</argument>            
    <argument>
        dir
        <dir path-type='rel'>../target/bin/x86_64</dir>
    </argument>
    <file path-type='rel'>../target/bin/x86_64/binary.exe</file> 
</deploy-qt>
```
and would result in
```
windeployqt --release --no-translations --no-svg --dir [...]\target\bin\x86_64 [...]\target\bin\x86_64\binary.exe
```
which will be executed.

### Upload using FTP
to be done

### Build latex
This step comiles a latex file to pdf. The whole latex-toolchain need to be installed and the command 'pdflatex' needs to be in the PATH of the used OS.

This kind of step is added by using 
```xml
<latexcompile>
    ...
</latexcompile>
```
and needs at least a source file given. Define this file by using the `file`-tag embeeded into a `source`-tag. The `file`-tag uses absolute paths by default, but can be changed to relative paths (to the location of the deployment file) by adding the `path-type='rel'` attribute to the tag. There is the possibility to specify a destination filename. If it is done so (like the source filename, but by using the `destination`-tag instead of the `source`-tag), the compiled .pdf file will be copied and renamend to the given directory/ filename.

One latex compile step could look as follows:
```xml
<latexcompile>
    <source>
        <file path-type='rel'>..\..\test.tex</file>
    </source>
    <destination>
        <file path-type='rel'>target\release\hello_world.pdf</file>
    </destination>
</latexcompile>
```

### Build documentation
This step automatically builds doxygen docs. For the program to generate the documentation, 'doxygen' must be available in the PATH of your OS and the doxygen config-file needs to be already generated.

This kind of step is added by using
```xml
<build-doc>
    ...
</build-doc>
```
and needs to be filled with the path to the doxygen configuration file. Define this file by using the `file`-tag. The `file`-tag uses absolute paths by default, but can be changed to relative paths (to the location of the deployment file) by adding the `path-type='rel'` attribute to the tag.

So one build documentation step could look as follows:
```xml
<build-doc>
    <file path-type='rel'>../Doxyfile</file>
</build-doc>
```
and would result in
```
doxygen [...]/Doxyfile
```
which will be executed.

### Clean Step
This step is added by the tag

```xml
<clean>
    <dir path-type='rel'>../target/bin/x86_64</dir>
</clean>
```

which removes all files and directories inside the given directory.