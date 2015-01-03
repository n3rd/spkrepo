# Synology Package Server

A .spk package server for synology NAS systems in ASP.NET / C#.

## How to run

### IIS

1. Publish the SpkRepo web project form Visual Studio to you webhost.
2. Put the .spk packages you want to serve in the App_Data folder.

### Synology NAS

1. Read the section on **how to build** and create the spk package.
2. Make sure you installed the mono (beta) package from [SynoCommunity](https://synocommunity.com/)
3. Install the spk on your synology *Package Center -> Manual Installation*
4. Upload your spk packages to /volume1/public/packages *or the directory you configured during installation*

## How to build

### The spk package for the server

1. Make sure you have Visual Studio 2013 installed.
2. Open a console window and run **build.cmd**
3. The spk package can now be found in the artifacts directory.
